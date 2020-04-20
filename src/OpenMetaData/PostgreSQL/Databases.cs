using System;
using System.Data;
using Npgsql;

namespace OMeta.PostgreSQL
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabases))]
#endif 
	public class PostgreSQLDatabases : Databases
	{
		public PostgreSQLDatabases()
		{

		}

		override internal void LoadAll()
		{
			string query =
				"select datname as CATALOG_NAME, s.usename as SCHEMA_OWNER from pg_database d " +
				"INNER JOIN pg_user s on d.datdba = s.usesysid where datistemplate = 'f' ORDER BY datname";

			NpgsqlConnection cn = new Npgsql.NpgsqlConnection(this.dbRoot.ConnectionString);

			NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);
			DataTable metaData = new DataTable();

			adapter.Fill(metaData);
		
			PopulateArray(metaData);
		}
	}
}
