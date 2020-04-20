using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Sql
{
 
	public class SqlDatabases : Databases
	{
		public SqlDatabases()
		{

		}

		override internal void LoadAll()
		{
			DataTable metaData  = this.LoadData(OleDbSchemaGuid.Catalogs, null);
		
			PopulateArray(metaData);
		}
	}
}
