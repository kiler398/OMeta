using System;
using System.Data;
using Npgsql;

namespace OMeta.PostgreSQL
{
 
	public class PostgreSQLTable : Table
	{
		public PostgreSQLTable()
		{

		}


		public override IColumns PrimaryKeys
		{
			get
			{
				if(null == _primaryKeys)
				{
					string query =
						"select a.attname as COLUMN_NAME, c.conname as constraint, d.relname as table " +
						"from pg_attribute a, pg_index b, pg_constraint c, pg_class d, pg_namespace n " +
						"WHERE n.nspname = '" + this.Schema + "' AND d.relname = '" + this.Name + "' AND a.attrelid = b.indexrelid " +
						"and b.indrelid = c.conrelid and b.indrelid = d.relfilenode and c.contype = 'p' AND b.indisprimary";

					NpgsqlConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Database.Name);

					DataTable metaData = new DataTable();
					NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);

					adapter.Fill(metaData);
					cn.Close();

					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryKeys.Table = this;
					_primaryKeys.dbRoot = this.dbRoot;

					string colName = "";

					int count = metaData.Rows.Count;
					for(int i = 0; i < count; i++)
					{
						colName = metaData.Rows[i]["COLUMN_NAME"] as string;
						_primaryKeys.AddColumn((Column)this.Columns[colName]);
					}
				}

				return _primaryKeys;
			}
		}
	}
}
