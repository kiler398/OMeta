using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.ISeries
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedure))]
#endif 
	public class ISeriesProcedure : Procedure
	{
		public ISeriesProcedure()
		{

		}
	}
}
