using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Oracle
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameter))]
#endif 
	public class OracleParameter : Parameter
	{
		public OracleParameter()
		{

		}
	}
}
