using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.DB2
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumn))]
#endif 
	public class DB2Column : Column
	{
		public DB2Column()
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
				if(null == this.Columns.Table) return false;

				Columns cols = this.Columns.Table.PrimaryKeys as Columns;
				Column col = cols.GetByPhysicalName(this.Name);

				return (null == col) ? false : true;
			}
		}

		override public System.Boolean IsAutoKey
		{
			get
			{
				DB2Columns cols = Columns as DB2Columns;
				return this.GetBool(cols.f_AutoKey);
			}
		}

		override public string DataTypeName
		{
			get
			{
				DB2Columns cols = Columns as DB2Columns;
				return this.GetString(cols.f_TypeName);
			}
		}

		override public string DataTypeNameComplete
		{
			get
			{
				return "Unknown";
//				DB2Columns cols = Columns as DB2Columns;
//				return this.GetString(cols.f_TypeName);
			}
		}
	}
}
