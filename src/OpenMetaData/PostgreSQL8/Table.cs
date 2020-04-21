using System;
using System.Data;
using Npgsql;

namespace OMeta.PostgreSQL8
{
 
	public class PostgreSQL8Table : Table
	{
		public PostgreSQL8Table()
		{

		}


		public override IColumns PrimaryKeys
		{
			get
			{
				if(null == _primaryKeys)
				{
                    // New PostgreSQL primary key query from Michael McKean
                    string query = "SELECT  c.column_name " +
                                   "FROM   information_schema.key_column_usage c " +
                                   "INNER JOIN  information_schema.table_constraints t " +
                                   "ON  c.constraint_name = t.constraint_name " +
                                   "WHERE  c.table_name = '" + this.Name + "' " +
                                   " AND c.table_schema = '" + this.Schema + "' " +
                                   " AND  t.constraint_type = 'PRIMARY KEY' ";
					/*string query = "select c.column_name from information_schema.table_constraints t, information_schema.constraint_column_usage c " +
						"where t.table_name = '" + this.Name + "' and t.table_schema = '" + this.Schema +  
						"' and c.constraint_name = t.constraint_name and t.constraint_type = 'PRIMARY KEY'";*/

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
