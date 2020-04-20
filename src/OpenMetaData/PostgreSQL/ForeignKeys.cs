using System;
using System.Data;
using Npgsql;

namespace OMeta.PostgreSQL
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IForeignKeys))]
#endif 
	public class PostgreSQLForeignKeys : ForeignKeys
	{
		static string _query = 
			"SELECT pfk.relname || '.' || ct.conname as FK_NAME, pn.nspname as PK_TABLE_SCHEMA, pfk.relname as PK_TABLE_NAME, " +
			"ct.confkey as PK_COLS, fn.nspname as FK_TABLE_SCHEMA, ffk.relname as FK_TABLE_NAME, ct.conkey as FK_COLS, d.DESCRIPTION, " +
			"CAST(CASE ct.confupdtype WHEN 'c' THEN 'CASCADE' " +
				"WHEN 'n' THEN 'SET NULL' " +
				"WHEN 'd' THEN 'SET DEFAULT' " +
				"WHEN 'r' THEN 'RESTRICT' " +
				"WHEN 'a' THEN 'NO ACTION' END " +
			"AS character varying) AS update_rule,  " +
			"CAST(CASE ct.confdeltype WHEN 'c' THEN 'CASCADE' " +
				"WHEN 'n' THEN 'SET NULL' " +
				"WHEN 'd' THEN 'SET DEFAULT' " +
				"WHEN 'r' THEN 'RESTRICT' " +
				"WHEN 'a' THEN 'NO ACTION' END " +
			"AS character varying) AS delete_rule " +
			"FROM pg_constraint ct " +
			"JOIN pg_class pfk on pfk.oid = confrelid " +
			"JOIN pg_class ffk on ffk.oid = ct.conrelid " +
			"JOIN pg_namespace pn ON pn.oid = pfk.relnamespace  " +
			"JOIN pg_namespace fn ON fn.oid = ffk.relnamespace  " +
			"LEFT OUTER JOIN pg_description d ON d.objoid = ct.oid " +
			"WHERE contype='f'";


		public PostgreSQLForeignKeys()
		{

		}

		override internal void LoadAll()
		{
			string query1 = _query +
				"AND pn.nspname = '" + this.Table.Schema + "' AND pfk.relname = '" + this.Table.Name + "' ORDER BY ct.conname";

			string query2 = _query +
				"AND fn.nspname = '" + this.Table.Schema + "' AND ffk.relname = '" + this.Table.Name + "' ORDER BY ct.conname";

			this._LoadAll(query1, query2);
		}

		private void _LoadAll(string query1, string query2)
		{
			NpgsqlConnection cn = null;

			try
			{
				cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Table.Database.Name);

				DataTable metaData1 = new DataTable();
				DataTable metaData2 = new DataTable();

				NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query1, cn);
				adapter.Fill(metaData1);

				adapter = new NpgsqlDataAdapter(query2, cn);
				adapter.Fill(metaData2);

				DataRowCollection rows = metaData2.Rows;
				int count = rows.Count;
				for(int i = 0; i < count; i++)
				{
					metaData1.ImportRow(rows[i]);
				}

				PopulateArrayNoHookup(metaData1);

				if(metaData1.Rows.Count > 0)
				{
					string catalog = this.Table.Database.Name;
					string schema;
					string table;
					string[] cols = null;
					string q;

					string query =
						"SELECT a.attname as COLUMN from pg_attribute a, pg_class c, pg_namespace n " +
						"WHERE a.attrelid = c.oid AND c.relnamespace = n.oid " +
						"AND a.attnum > 0 AND NOT a.attisdropped AND c.relkind = 'r' ";

					foreach(ForeignKey key in this)
					{
						//------------------------------------------------
						// Primary
						//------------------------------------------------
						cols = ParseColumns(key._row["PK_COLS"] as string);

						schema = key._row["PK_TABLE_SCHEMA"] as string;
						table  = key._row["PK_TABLE_NAME"] as string;

						q = query;
						q += "AND n.nspname = '" + schema + "' AND c.relname = '" + table + "' AND attnum IN(";

						for(int i = 0; i < cols.GetLength(0); i++)
						{
							if(i > 0) q += ',';
							q += cols[i].ToString();
						}

						q += ") ORDER BY attnum;";

						DataTable metaData = new DataTable();
						adapter = new NpgsqlDataAdapter(q, cn);

						adapter.Fill(metaData);

						for(int i = 0; i < cols.GetLength(0); i++)
						{
							key.AddForeignColumn(catalog, "", table, metaData.Rows[i]["COLUMN"] as string, true); 
						}

						//------------------------------------------------
						// Foreign
						//------------------------------------------------
						cols = ParseColumns(key._row["FK_COLS"] as string);

						schema = key._row["FK_TABLE_SCHEMA"] as string;
						table  = key._row["FK_TABLE_NAME"] as string;

						q = query;
						q += "AND n.nspname = '" + schema + "' AND c.relname = '" + table + "' AND attnum IN(";

						for(int i = 0; i < cols.GetLength(0); i++)
						{
							if(i > 0) q += ',';
							q += cols[i].ToString();
						}

						q += ") ORDER BY attnum;";

						metaData = new DataTable();
						adapter = new NpgsqlDataAdapter(q, cn);

						adapter.Fill(metaData);

						for(int i = 0; i < cols.GetLength(0); i++)
						{
							key.AddForeignColumn(catalog, "", table, metaData.Rows[i]["COLUMN"] as string, false); 
						}
					}
				}
			}
			catch {}

			cn.Close();
		}

		private string[] ParseColumns(string cols)
		{
			cols = cols.Replace("{", "");
			cols = cols.Replace("}", "");
			return cols.Split(',');
		}
	}
}
