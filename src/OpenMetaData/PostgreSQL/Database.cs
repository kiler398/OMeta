using System;
using System.Data;
using System.Data.OleDb;

using Npgsql;
 

namespace OMeta.PostgreSQL
{
 
	public class PostgreSQLDatabase : Database
	{
		public PostgreSQLDatabase()
		{

		}

		override public DataSet ExecuteSql(string sql)
		{
			NpgsqlConnection cn = new NpgsqlConnection(dbRoot.ConnectionString);
			cn.Open();
			cn.ChangeDatabase(this.Name);

			return this.ExecuteIntoRecordset(sql, cn);
		}
	}
}
