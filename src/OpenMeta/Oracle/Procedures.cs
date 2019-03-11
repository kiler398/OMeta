using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Oracle
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedures))]
#endif 
	public class OracleProcedures : Procedures
	{
		public OracleProcedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedures, 
					new Object[] {null, this.Database.Name});

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
