using System;
using System.Data;

using FirebirdSql.Data.FirebirdClient;

namespace OMeta.Firebird
{
 
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
