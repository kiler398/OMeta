using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.ISeries
{
 
	public class ISeriesProcedures : Procedures
	{
		public ISeriesProcedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedures, null);

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
