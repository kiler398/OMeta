using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Pervasive
{
  
	public class PervasiveProcedures : Procedures
	{
		public PervasiveProcedures()
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
