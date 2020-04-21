using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Pervasive
{
 
	public class PervasiveTables : Tables
	{
		public PervasiveTables()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string type = this.dbRoot.ShowSystemData ? "SYSTEM TABLE" : "TABLE";
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Tables, new Object[] {null, null, null, type});

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
