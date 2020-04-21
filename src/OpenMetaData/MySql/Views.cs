using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.MySql
{
 
	public class MySqlViews : Views
	{
		public MySqlViews()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string type = this.dbRoot.ShowSystemData ? "SYSTEM VIEW" : "VIEW";
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Tables, new Object[] {this.Database.Name, null, null, type});

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
