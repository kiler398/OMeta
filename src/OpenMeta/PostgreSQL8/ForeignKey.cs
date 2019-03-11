using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.PostgreSQL8
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IForeignKey))]
#endif 
	public class PostgreSQL8ForeignKey : ForeignKey
	{
		public PostgreSQL8ForeignKey()
		{

		}
	}
}
