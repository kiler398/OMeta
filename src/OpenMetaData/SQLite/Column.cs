using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumn))]
#endif 
	public class SQLiteColumn : Column
	{
		public SQLiteColumn()
		{

		}

		override internal Column Clone()
		{
			Column c = base.Clone();

			return c;
		}

		public override Boolean IsAutoKey
		{
			get
			{
				if(this.IsInPrimaryKey && (this.DataTypeName == "INTEGER" || this.DataTypeName == "INT"))
					return true;
				else
					return false;
			}
		}


		override public bool IsNullable
		{
			get
			{
				return Convert.ToBoolean( this.GetString(Columns.f_IsNullable) );
			}
		}

		override public bool HasDefault
		{
			get
			{
				return Convert.ToBoolean( this.GetString(Columns.f_HasDefault) );
			}
		}

		
		override public string DataTypeName
		{
			get
			{
				SQLiteColumns cols = Columns as SQLiteColumns;
				return this.GetString(cols.f_TypeName);
			}
		}

		override public string DataTypeNameComplete
		{
			get
			{
				SQLiteColumns cols = Columns as SQLiteColumns;
				return this.GetString(cols.f_TypeNameComplete);
			}
		}
	}
}
