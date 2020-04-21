using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Pervasive
{
 
	public class PervasiveIndexes : Indexes
	{
		public PervasiveIndexes()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Indexes, 
					new object[]{null, null, null, null, this.Table.Name});

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
