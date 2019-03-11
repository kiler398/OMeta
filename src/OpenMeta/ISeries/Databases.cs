using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.ISeries
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabases))]
#endif 
	public class ISeriesDatabases : Databases
	{
		public ISeriesDatabases()
		{

		}

		internal override void LoadAll()
		{
			try
			{
				ISeriesDatabase database = (ISeriesDatabase)this.dbRoot.ClassFactory.CreateDatabase();
				//database._name = cn.DataSource;

				OleDbConnection cn = new OleDbConnection(this.dbRoot.ConnectionString); 
				cn.Open();
				OleDbCommand cmd = cn.CreateCommand();
				cmd.CommandText = "SELECT c.CATALOG_NAME, t.TABLE_SCHEMA FROM SYSTABLES t, QSYS2.SYSCATALOGS c";
				OleDbDataReader reader = cmd.ExecuteReader();
				while(reader.Read()) 
				{
					//database. = reader["CATALOG_NAME"].ToString();
					database._name = reader["TABLE_SCHEMA"].ToString();
					break;
				}
				reader.Close();
				cn.Close();
	
				database.dbRoot = this.dbRoot;
				database.Databases = this;
				this._array.Add(database);
			}
			catch (Exception ex)
			{
				string message = ex.Message;
			}
		}
	}
}
