using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Oracle
{
 
	public class OracleIndexes : Indexes
	{
		public OracleIndexes()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Indexes, 
					new object[]{null, this.Table.Database.Name, null, null, this.Table.Name});

				PopulateArray(metaData);
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
		}
	}
}
