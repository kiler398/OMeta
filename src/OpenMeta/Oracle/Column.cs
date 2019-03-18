using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Oracle
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumn))]
#endif 
	public class OracleColumn : Column
	{
		public OracleColumn()
		{

		}

		override internal Column Clone()
		{
			Column c = base.Clone();

			return c;
		}

		public override string DataTypeName
		{
			get
			{
				OracleColumns cols = Columns as OracleColumns;
				return this.GetString(cols.f_TypeName);
			}
		}

//		override public string DataTypeName
//		{
//			get
//			{
//				MySqlColumns cols = Columns as MySqlColumns;
//				string type = this.GetString(cols.f_TypeName);
//				int index = type.IndexOf("(");
//				if(-1 != index)
//				{
//					type = type.Substring(0, index);
//				}
//				return type;
//			}
//		}

		override public string DataTypeNameComplete
		{
			get
			{
				OracleColumns cols = Columns as OracleColumns;
				return this.GetString(cols.f_TypeName);
			}
		}
	}
}
