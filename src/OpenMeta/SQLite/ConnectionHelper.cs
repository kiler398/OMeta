using System;
using System.Data.SQLite;

namespace MyMeta.SQLite
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
		static public SQLiteConnection CreateConnection(dbRoot dbRoot)
		{
			SQLiteConnection cn = new SQLiteConnection(dbRoot.ConnectionString);
			cn.Open();
			//cn.ChangeDatabase(database);
			return cn;
		}
	}
}
