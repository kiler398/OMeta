using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.DB2
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameter))]
#endif 
	public class DB2Parameter : Parameter
	{
		public DB2Parameter()
		{

		}
	}
}
