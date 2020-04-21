using System;
using System.Data;

namespace OMeta.MySql5
{
 
	public class MySql5Parameters : Parameters
	{
		public MySql5Parameters()
		{

		}

		override internal void LoadAll()
		{
			try
			{
//				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedure_Parameters, 
//					new object[]{this.Procedure.Database.Name, null, this.Procedure.Name});
//
//				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
