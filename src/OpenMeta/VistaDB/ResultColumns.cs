using System;
using System.Data;

namespace MyMeta.VistaDB
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IResultColumns))]
#endif 
	public class VistaDBResultColumns : ResultColumns
	{
		public VistaDBResultColumns()
		{

		}
	}
}
