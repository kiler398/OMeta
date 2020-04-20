using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IView))]
#endif 
	public class SQLiteView : View
	{
		public SQLiteView()
		{

		}
	}
}
