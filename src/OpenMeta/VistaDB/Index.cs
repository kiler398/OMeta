using System;
using System.Data;

namespace OMeta.VistaDB
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndex))]
#endif 
	public class VistaDBIndex : Index
	{
		public VistaDBIndex()
		{

		}
	}
}
