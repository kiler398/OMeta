using System;
using System.Data;

namespace MyMeta.VistaDB
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameter))]
#endif 
	public class VistaDBParameter : Parameter
	{
		public VistaDBParameter()
		{

		}
	}
}
