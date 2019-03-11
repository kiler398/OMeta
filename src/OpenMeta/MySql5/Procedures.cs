using System;
using System.Data;

namespace MyMeta.MySql5
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedures))]
#endif 
	public class MySql5Procedures : Procedures
	{
		public MySql5Procedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
//				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedures, 
//					new Object[] {this.Database.Name, null, null});
//
//				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
