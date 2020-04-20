using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.DB2
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedure))]
#endif 
	public class DB2Procedure : Procedure
	{
		public DB2Procedure()
		{

		}
	}
}
