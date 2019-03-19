using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Advantage
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabase))]
#endif 
	public class AdvantageDatabase : Database
	{
		public AdvantageDatabase()
		{

		}
	}
}
