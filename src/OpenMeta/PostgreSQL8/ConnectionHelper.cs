using System;
using Npgsql;

namespace MyMeta.PostgreSQL8
{
	/// <summary>
	/// Summary description for ConnectionHelper.
	/// </summary>
	public class ConnectionHelper
	{
		public ConnectionHelper()
		{

		}
//
//		static public NpgsqlConnection CreateConnection(MyMeta.dbRoot dbRoot, string database)
//		{
//			string cnstr = dbRoot.ConnectionString + "Database=" + database + ";";
//			NpgsqlConnection cn = new Npgsql.NpgsqlConnection(cnstr);
//			return cn;
//		}

		static public NpgsqlConnection CreateConnection(dbRoot dbRoot, string database)
		{
			NpgsqlConnection cn = new Npgsql.NpgsqlConnection(dbRoot.ConnectionString);
			cn.Open();
			cn.ChangeDatabase(database);
			return cn;
		}
	}
}
