using System;
using System.Data;
using System.Data.SQLite;

namespace OMeta.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabases))]
#endif 
	public class SQLiteDatabases : Databases
	{
		public SQLiteDatabases()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				SQLiteConnection cn = new SQLiteConnection(this.dbRoot.ConnectionString); 

				// We add our one and only Database
				SQLiteDatabase database = (SQLiteDatabase)this.dbRoot.ClassFactory.CreateDatabase();
				database.dbRoot = this.dbRoot;
				database.Databases = this;
				database._name = (cn.Database.Length == 0 ? "main" : cn.Database);
				this._array.Add(database);
			}
			catch {}
		}
	}
}
