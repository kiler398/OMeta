using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace OMeta.Firebird
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabases))]
#endif 
	public class FirebirdDatabases : Databases
	{
		public FirebirdDatabases()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				FbConnection cn = new FirebirdSql.Data.FirebirdClient.FbConnection(this._dbRoot.ConnectionString);
				cn.Open();
				string dbName = cn.Database;
				cn.Close();

				int index = dbName.LastIndexOfAny(new char[]{'\\'});

				if (index >= 0)
				{
					dbName = dbName.Substring(index + 1);
				}

				// We add our one and only Database
				FirebirdDatabase database = (FirebirdDatabase)this.dbRoot.ClassFactory.CreateDatabase();
				database._name = dbName;
				database.dbRoot = this.dbRoot;
				database.Databases = this;
				this._array.Add(database);
			}
			catch {}
		}
	}
}
