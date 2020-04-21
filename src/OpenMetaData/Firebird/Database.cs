using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

 


namespace OMeta.Firebird
{
 
	public class FirebirdDatabase : Database
	{
		public FirebirdDatabase()
		{

		}

        override public IResultColumns ResultColumnsFromSQL(string sql)
        {
            IResultColumns columns = null;
            using (FbConnection cn = new FbConnection(dbRoot.ConnectionString))
            {
                cn.Open();
                columns = ResultColumnsFromSQL(sql, cn);
            }
            return columns;
        }

		override public DataSet ExecuteSql(string sql)
		{
			FbConnection cn = new FbConnection(dbRoot.ConnectionString);
			cn.Open();
			//cn.ChangeDatabase(this.Name);

			return this.ExecuteIntoRecordset(sql, cn);
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

		override public string Description
		{
			get
			{
				return _desc;
			}
		}

		internal string _name = "";
		internal string _desc = "";
	}
}
