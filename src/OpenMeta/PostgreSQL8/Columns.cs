using System;
using System.Data;
using Npgsql;

namespace MyMeta.PostgreSQL8
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumns))]
#endif 
	public class PostgreSQL8Columns : Columns
	{
		public PostgreSQL8Columns()
		{

		}

		internal DataColumn f_TypeName = null;
		internal DataColumn f_TypeNameComplete	= null;

		override internal void LoadForTable()
		{
			NpgsqlConnection cn = null;

			try
			{
				string query = 	"select * from information_schema.columns where table_catalog = '" + 
					this.Table.Database.Name + "' and table_schema = '" + this.Table.Schema + 
					"' and table_name = '" + this.Table.Name + "' order by ordinal_position";

				cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Table.Database.Name);

				DataTable metaData = new DataTable();
				NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);

				adapter.Fill(metaData);

				metaData.Columns["udt_name"].ColumnName  = "TYPE_NAME";
				metaData.Columns["data_type"].ColumnName = "TYPE_NAMECOMPLETE";

				if(metaData.Columns.Contains("TYPE_NAME"))
					f_TypeName = metaData.Columns["TYPE_NAME"];

				if(metaData.Columns.Contains("TYPE_NAMECOMPLETE"))
					f_TypeNameComplete = metaData.Columns["TYPE_NAMECOMPLETE"];
		
				PopulateArray(metaData);

				// IsAutoKey logic
//				query = "SELECT a.attname AS column_name, c2.relname AS seq_name " +
//				"FROM pg_class c1, pg_class c2, pg_namespace n, pg_depend d, pg_attribute a " +
//				"WHERE n.nspname = '" + this.Table.Schema + "' AND c1.relname = '" + this.Table.Name + "' " +
//				"AND c2.relkind = 'S' AND c1.relnamespace = n.oid " +
//				"AND d.refobjid = c1.oid AND c2.oid = d.objid " +
//				"AND a.attrelid = c1.oid AND d.refobjsubid = a.attnum";

				query = @"SELECT a.attname AS column_name, substring(pg_get_expr(ad.adbin, c.oid) " +
					@"FROM '[\'""]+(.+?)[\'""]+') AS seq_name " +
					"FROM pg_class c, pg_namespace n, pg_attribute a, pg_attrdef ad " +
					"WHERE n.nspname = '" + this.Table.Schema + "' AND c.relname = '" + this.Table.Name + "' " +
					"AND c.relnamespace = n.oid " +
					"AND a.attrelid = c.oid  AND a.atthasdef = true " +
					"AND ad.adrelid = c.oid AND ad.adnum = a.attnum " +
					@"AND pg_get_expr(ad.adbin, c.oid) LIKE 'nextval(%'";

				DataTable seqData = new DataTable();
				adapter = new NpgsqlDataAdapter(query, cn);
				adapter.Fill(seqData);

				DataRowCollection rows = seqData.Rows;

				if(rows.Count > 0)
				{
					string colName;

					for(int i = 0; i < rows.Count; i++)
					{
						colName = rows[i]["column_name"] as string;

						PostgreSQL8Column col = this[colName] as PostgreSQL8Column;
						col._isAutoKey = true;

						query = "SELECT min_value, increment_by FROM \"" + rows[i]["seq_name"] + "\""; 
						adapter = new NpgsqlDataAdapter(query, cn);
						DataTable autokeyData = new DataTable();
						adapter.Fill(autokeyData);

						Int64 a;
						
						a = (Int64)autokeyData.Rows[0]["min_value"];
						col._autoInc  = Convert.ToInt32(a);

						a = (Int64)autokeyData.Rows[0]["increment_by"];
						col._autoSeed  = Convert.ToInt32(a);
					}
				}

				cn.Close();
			}
			catch
			{
				if(cn != null)
				{
					if(cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}
			}
		}

		override internal void LoadForView()
		{
			try
			{
				string query = 	"select * from information_schema.columns where table_catalog = '" + 
					this.View.Database.Name + "' and table_schema = '" + this.View.Schema + 
					"' and table_name = '" + this.View.Name + "' order by ordinal_position";

				NpgsqlConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.View.Database.Name);

				DataTable metaData = new DataTable();
				NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);

				adapter.Fill(metaData);
				cn.Close();

				metaData.Columns["udt_name"].ColumnName  = "TYPE_NAME";
				metaData.Columns["data_type"].ColumnName = "TYPE_NAMECOMPLETE";

				if(metaData.Columns.Contains("TYPE_NAME"))
					f_TypeName = metaData.Columns["TYPE_NAME"];

				if(metaData.Columns.Contains("TYPE_NAMECOMPLETE"))
					f_TypeNameComplete = metaData.Columns["TYPE_NAMECOMPLETE"];
		
				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
