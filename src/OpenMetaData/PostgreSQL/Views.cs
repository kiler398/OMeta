using System;
using System.Data;
using Npgsql;

namespace OMeta.PostgreSQL
{
 
	public class PostgreSQLViews : Views
	{
		public PostgreSQLViews()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string query =
					"SELECT CAST(current_database() AS character varying) AS table_catalog, " +
						"CAST(nc.nspname AS character varying) AS table_schema, " +
						"CAST(c.relname AS character varying) AS table_name, " +
						"CAST('VIEW' AS character varying) AS table_type, d.description " +
					"FROM pg_namespace nc, pg_user u, pg_class c LEFT OUTER JOIN pg_description d ON d.objoid = c.oid AND d.objsubid = 0  " +
					"WHERE c.relnamespace = nc.oid AND u.usesysid = c.relowner AND c.relkind= 'v'";

				if(!this.dbRoot.ShowSystemData)
				{
					query += " AND nc.nspname <> 'pg_catalog'";
				}

				query += " ORDER BY c.relname";

				NpgsqlConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Database.Name);

				DataTable metaData = new DataTable();
				NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);

				adapter.Fill(metaData);
				cn.Close();
		
				PopulateArray(metaData);
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
		}
	}
}
