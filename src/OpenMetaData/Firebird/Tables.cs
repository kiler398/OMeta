using System;
using System.Data;

using FirebirdSql.Data.FirebirdClient;

namespace OMeta.Firebird
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(ITables))]
#endif 
	public class FirebirdTables : Tables
	{
		public FirebirdTables()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string type = this.dbRoot.ShowSystemData ? "SYSTEM TABLE" : "TABLE";

				FbConnection cn = new FirebirdSql.Data.FirebirdClient.FbConnection(this._dbRoot.ConnectionString);
				cn.Open();
				DataTable metaData = cn.GetSchema("Tables", new string[] {null, null, null, type});
				cn.Close();

				PopulateArray(metaData);
			}
			catch(Exception ex)
			{
				string m = ex.Message;
			}
		}
	}
}
