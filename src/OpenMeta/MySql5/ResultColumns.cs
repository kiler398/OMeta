using System;
using System.Data;

namespace OMeta.MySql5
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IResultColumns))]
#endif 
	public class MySql5ResultColumns : ResultColumns
	{
		public MySql5ResultColumns()
		{

		}
	}
}
