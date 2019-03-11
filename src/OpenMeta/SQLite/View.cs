using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.SQLite
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
