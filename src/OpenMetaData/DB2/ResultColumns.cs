using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.DB2
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IResultColumns))]
#endif 
	public class DB2ResultColumns : ResultColumns
	{
		public DB2ResultColumns()
		{

		}
	}
}
