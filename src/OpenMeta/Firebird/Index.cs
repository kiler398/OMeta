using System;
using System.Data;

using FirebirdSql.Data.FirebirdClient;

namespace OMeta.Firebird
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndex))]
#endif 
	public class FirebirdIndex : Index
	{
		public FirebirdIndex()
		{

		}
	}
}
