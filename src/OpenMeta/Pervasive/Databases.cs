using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Pervasive
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabases))]
#endif 
	public class PervasiveDatabases : Databases
	{
		public PervasiveDatabases()
		{

		}

		internal override void LoadAll()
		{
			try
			{
				OleDbConnection cn = new OleDbConnection(this.dbRoot.ConnectionString); 

				string dbName = cn.DataSource;
				int index = cn.DataSource.LastIndexOfAny(new char[]{'\\'});

				if (index >= 0)
				{
					dbName = cn.DataSource.Substring(index + 1);
				}

				// We add our one and only Database
				PervasiveDatabase database = (PervasiveDatabase)this.dbRoot.ClassFactory.CreateDatabase();
				database._name = dbName;
				database.dbRoot = this.dbRoot;
				database.Databases = this;
				this._array.Add(database);
			}
			catch {}
		}
	}
}
