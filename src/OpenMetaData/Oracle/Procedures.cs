using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Oracle
{
 
	public class OracleProcedures : Procedures
	{
		public OracleProcedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedures, 
					new Object[] {null, this.Database.Name});

				PopulateArray(metaData);
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
		}
	}
}
