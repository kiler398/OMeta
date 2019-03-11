using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.DB2
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabase))]
#endif 
	public class DB2Database : Database
	{
		public DB2Database()
		{

		}

		override public string Alias
		{
			get
			{
				return _name;
			}
		}

		override public string Name
		{
			get
			{
				return _name;
			}
		}

		override public string Description
		{
			get
			{
				return _desc;
			}
		}

		internal string _name = "";
		internal string _desc = "";
	}
}
