using System;
using System.Data;

namespace OMeta.MySql5
{
 
	public class MySql5Procedures : Procedures
	{
		public MySql5Procedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
//				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedures, 
//					new Object[] {this.Database.Name, null, null});
//
//				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
