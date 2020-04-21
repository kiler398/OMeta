using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Advantage
{
 
	public class AdvantageDatabases : Databases
	{
		public AdvantageDatabases()
		{

		}

		override internal void LoadAll()
		{
			DataTable metaData  = this.LoadData(OleDbSchemaGuid.Catalogs, null);
		
			PopulateArray(metaData);
		}
	}
}
