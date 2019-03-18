using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.MySql
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabases))]
#endif 
	public class MySqlDatabases : Databases
	{
		public MySqlDatabases()
		{

		}

		internal override void LoadAll()
		{
			try
			{
				OleDbConnection cn = new OleDbConnection(this.dbRoot.ConnectionString); 

				// We add our one and only Database
				MySqlDatabase database = (MySqlDatabase)this.dbRoot.ClassFactory.CreateDatabase();
				database._name = cn.DataSource;
				database.dbRoot = this.dbRoot;
				database.Databases = this;
				this._array.Add(database);
			}
			catch {}
		}
	}
}
