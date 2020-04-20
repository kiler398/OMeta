using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Access
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
    /// <summary>
    /// Access���ݿ⼯��Ԫ������Ϣ
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabases))]
#endif 
	public class AccessDatabases : Databases
	{
		public AccessDatabases()
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
				AccessDatabase database = (AccessDatabase)this.dbRoot.ClassFactory.CreateDatabase();
				database._name = dbName;
				database.dbRoot = this.dbRoot;
				database.Databases = this;
				this._array.Add(database);
			}
			catch {}
		}
	}
}
