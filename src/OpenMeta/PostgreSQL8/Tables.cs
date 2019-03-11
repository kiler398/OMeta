using System;
using System.Data;
using Npgsql;

namespace MyMeta.PostgreSQL8
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(ITables))]
#endif 
	public class PostgreSQL8Tables : Tables
	{
		public PostgreSQL8Tables()
		{

		}

		override internal void LoadAll()
		{
			try
            {
                // New PostgreSQL sorted query from Michael McKean
                string query = "select * from information_schema.tables where table_type = 'BASE TABLE' " +
                                      " and table_schema = '" + this.Database.SchemaName + "' ORDER BY table_name;";
				/*string query = "select * from information_schema.tables where table_type = 'BASE TABLE' " +
					" and table_schema = '" + this.Database.SchemaName + "'";*/

				NpgsqlConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Database.Name);

				DataTable metaData = new DataTable();
				NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);

				adapter.Fill(metaData);
				cn.Close();
		
				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
