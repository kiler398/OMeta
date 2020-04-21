using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace OMeta.MySql5
{
 
	public class MySql5Databases : Databases
	{
		internal string Version = "";

		public MySql5Databases()
		{
        }

		internal override void LoadAll()
		{
			try
			{
				string name = "";

				// We add our one and only Database
                using (MySqlConnection conn = new MySqlConnection(this.dbRoot.ConnectionString))
                {
                    conn.Open();
                    name = conn.Database;

                    MySql5Database database = (MySql5Database)this.dbRoot.ClassFactory.CreateDatabase();
                    database._name = name;
                    database.dbRoot = this.dbRoot;
                    database.Databases = this;
                    this._array.Add(database);

                    try
                    {
                        DataTable metaData = new DataTable();
                        DbDataAdapter adapter = new MySqlDataAdapter("SELECT VERSION()", conn);// MySql5Databases.CreateAdapter("SELECT VERSION()", this.dbRoot.ConnectionString);

                        adapter.Fill(metaData);

                        this.Version = metaData.Rows[0][0] as string;
                    }
                    catch { }
                }
			}
			catch {}
		}

        static internal IDbConnection CreateConnection()
        {
            return new MySqlConnection();
        }

		static internal IDbConnection CreateConnection(string connStr)
		{
            IDbConnection con = CreateConnection();
            con.ConnectionString = connStr;
            con.Open();
            return con;
		}

		static internal DbDataAdapter CreateAdapter(string query, string connStr)
		{
            return new MySqlDataAdapter(query, connStr);
		}
	}
}
