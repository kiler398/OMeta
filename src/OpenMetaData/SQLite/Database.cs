using System;
using System.Data;
using System.Data.OleDb;

using System.Data.SQLite;
 

namespace OMeta.SQLite
{
 
	public class SQLiteDatabase : Database
	{
		internal string _name = "";

		public SQLiteDatabase()
		{

		}

		override public string Name
		{
			get
			{
				return _name;
			}
		}

		override public string Alias
		{
			get
			{
				return _name;
			}
		}

		override public DataSet ExecuteSql(string sql)
		{
			SQLiteConnection cn = ConnectionHelper.CreateConnection(dbRoot);

			return this.ExecuteIntoRecordset(sql, cn);
		}
	}
}
