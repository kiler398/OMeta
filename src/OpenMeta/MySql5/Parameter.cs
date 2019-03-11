using System;
using System.Data;

namespace MyMeta.MySql5
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameter))]
#endif 
	public class MySql5Parameter : Parameter
	{
		public MySql5Parameter()
		{

		}
	}
}
