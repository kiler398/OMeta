using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Pervasive
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedures))]
#endif 
	public class PervasiveProcedures : Procedures
	{
		public PervasiveProcedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedures, null);

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
