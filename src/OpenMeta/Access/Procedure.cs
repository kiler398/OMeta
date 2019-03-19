using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Access
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
    /// <summary>
    /// Access���ݿ�洢����Ԫ������Ϣ
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedure))]
#endif 
	public class AccessProcedure : Procedure
	{
		public AccessProcedure()
		{

		}
	}
}
