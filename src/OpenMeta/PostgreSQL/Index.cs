using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.PostgreSQL
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndex))]
#endif 
	public class PostgreSQLIndex : Index
	{
		public PostgreSQLIndex()
		{

		}

		public override string Type
		{
			get
			{
				string type = this.GetString(Indexes.f_Type);
				return type.ToUpper();
			}
		}
	}
}
