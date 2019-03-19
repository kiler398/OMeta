using System;
using System.Data;
using System.Data.OleDb;

using Npgsql;
using ADODB;

namespace OMeta.PostgreSQL8
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabase))]
#endif 
	public class PostgreSQL8Database : Database
	{
		public PostgreSQL8Database()
		{

		}

		override public ADODB.Recordset ExecuteSql(string sql)
		{
			NpgsqlConnection cn = new NpgsqlConnection(dbRoot.ConnectionString);
			cn.Open();
			cn.ChangeDatabase(this.Name);

			return this.ExecuteIntoRecordset(sql, cn);
		}
	}
}
