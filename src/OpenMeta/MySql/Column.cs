using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.MySql
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumn))]
#endif 
	public class MySqlColumn : Column
	{
		public MySqlColumn()
		{

		}

		override internal Column Clone()
		{
			Column c = base.Clone();

			return c;
		}

		override public System.Boolean IsInPrimaryKey
		{
			get
			{
				MySqlColumns cols = Columns as MySqlColumns;
				return this.GetBool(cols.f_InPrimaryKey);
			}
		}

		override public System.Boolean IsAutoKey
		{
			get
			{
				MySqlColumns cols = Columns as MySqlColumns;
				return this.GetBool(cols.f_AutoKey);
			}
		}

		override public string DataTypeName
		{
			get
			{
				MySqlColumns cols = Columns as MySqlColumns;
				string type = this.GetString(cols.f_TypeName);
				int startIndex = type.IndexOf("(");
				if(-1 != startIndex)
				{
					int endIndex = type.IndexOf(")");
					type = type.Remove(startIndex, (endIndex - startIndex) + 1);
				}
				return type;
			}
		}

		override public string DataTypeNameComplete
		{
			get
			{
				MySqlColumns cols = Columns as MySqlColumns;
				return this.GetString(cols.f_TypeName);
			}
		}
	}
}
