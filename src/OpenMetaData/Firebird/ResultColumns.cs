using System;
using System.Data;

namespace OMeta.Firebird
{
 
	public class FirebirdResultColumns : ResultColumns
	{
		public FirebirdResultColumns()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				//DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedure_Columns, 
				//	new object[]{null, null, this.Procedure.Name, null});

				//PopulateArray(metaData);
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
		}
	}
}
