using System;
using System.Data;

namespace OMeta.MySql5
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedure))]
#endif 
	public class MySql5Procedure : Procedure
	{
		public MySql5Procedure()
		{

		}
	}
}
