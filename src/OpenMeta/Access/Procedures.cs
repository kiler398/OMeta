using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Access
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
    /// <summary>
    /// Access数据库存储过程集合元数据信息
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedures))]
#endif 
	public class AccessProcedures : Procedures
	{
		public AccessProcedures()
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
