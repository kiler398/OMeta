using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IForeignKey))]
#endif 
	public class SQLiteForeignKey : ForeignKey
	{
		public SQLiteForeignKey()
		{

		}
	}
}
