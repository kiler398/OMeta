using System;
using System.Data;

namespace OMeta.VistaDB
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IView))]
#endif 
	public class VistaDBView : View
	{
		public VistaDBView()
		{

		}
	}
}
