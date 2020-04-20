using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Oracle
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndex))]
#endif 
	public class OracleIndex : Index
	{
		public OracleIndex()
		{

		}
	}
}
