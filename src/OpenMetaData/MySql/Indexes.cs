using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.MySql
{
 
	public class MySqlIndexes : Indexes
	{
		public MySqlIndexes()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Indexes, 
					new object[]{this.Table.Database.Name, null, null, null, this.Table.Name});

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
