using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.DB2
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabases))]
#endif 
	public class DB2Databases : Databases
	{
		public DB2Databases()
		{

		}

		internal override void LoadAll()
		{
			try
			{
				OleDbConnection cn = new OleDbConnection(this.dbRoot.ConnectionString); 

				// We add our one and only Database
				DB2Database database = (DB2Database)this.dbRoot.ClassFactory.CreateDatabase();
				database._name = cn.DataSource;
				database.dbRoot = this.dbRoot;
				database.Databases = this;
				this._array.Add(database);
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
		}
	}
}
