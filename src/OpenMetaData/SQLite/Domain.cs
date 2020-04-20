using System;
using System.Data;
using System.Data.SQLite;

namespace OMeta.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDomain))]
#endif 
	public class SQLiteDomain : Domain
	{
		public SQLiteDomain()
		{

		}
	}
}
