using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Access
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndexes))]
#endif 
	public class AccessIndexes : Indexes
	{
		public AccessIndexes()
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
