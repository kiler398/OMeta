using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.MySql
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IResultColumn))]
#endif 
	public class MySqlResultColumn : ResultColumn
	{
		public MySqlResultColumn()
		{

		}
	}
}
