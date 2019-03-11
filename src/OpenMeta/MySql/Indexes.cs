using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.MySql
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndexes))]
#endif 
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
