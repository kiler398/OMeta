using System;
using System.Data;
using Npgsql;

namespace OMeta.PostgreSQL8
{
 
	public class PostgreSQL8Views : Views
	{
		public PostgreSQL8Views()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string query = "select * from information_schema.views where table_schema = '" +
					 this.Database.SchemaName + "'";

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
