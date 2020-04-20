using System;
using System.Data;

namespace OMeta.MySql5
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IView))]
#endif 
	public class MySql5View : View
	{
		public MySql5View()
		{

		}
	}
}
