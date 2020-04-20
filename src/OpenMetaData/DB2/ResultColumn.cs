using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.DB2
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IResultColumn))]
#endif 
	public class DB2ResultColumn : ResultColumn
	{
		public DB2ResultColumn()
		{

		}

		#region Properties

		override public string Alias
		{
			get
			{
				return name;
			}
		}

		override public string DataTypeName
		{
			get
			{
				return typeName;
			}
		}

		override public System.Int32 Ordinal
		{
			get
			{
				return ordinal;
			}
		}

		internal string name = "";
		internal string typeName = "";
		internal System.Int32 ordinal = 0;

		#endregion
	}
}
