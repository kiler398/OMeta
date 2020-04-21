using System;
using System.Data;
using Npgsql;

namespace OMeta.PostgreSQL8
{
 
	public class PostgreSQL8Databases : Databases
	{
		public PostgreSQL8Databases()
		{

		}

		override internal void LoadAll()
		{
			string query =
				"select datname as CATALOG_NAME, s.usename as SCHEMA_OWNER, current_schema() as SCHEMA_NAME from pg_database d " +
				"INNER JOIN pg_user s on d.datdba = s.usesysid where datistemplate = 'f' ORDER BY datname";

			NpgsqlConnection cn = new Npgsql.NpgsqlConnection(this.dbRoot.ConnectionString);

			NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);
			DataTable metaData = new DataTable();

			adapter.Fill(metaData);
		
			PopulateArray(metaData);
		}
	}
}
