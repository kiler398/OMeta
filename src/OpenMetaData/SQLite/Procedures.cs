using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedures))]
#endif 
	public class SQLiteProcedures : Procedures
	{
		public SQLiteProcedures()
		{

		}
	}
}
