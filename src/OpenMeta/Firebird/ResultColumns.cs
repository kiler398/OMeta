using System;
using System.Data;

namespace MyMeta.Firebird
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IResultColumns))]
#endif 
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
			catch {}
		}
	}
}
