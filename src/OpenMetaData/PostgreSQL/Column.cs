using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.PostgreSQL
{
 
	public class PostgreSQLColumn : Column
	{
		public PostgreSQLColumn()
		{

		}

		override internal Column Clone()
		{
			Column c = base.Clone();

			return c;
		}

		override public System.Boolean IsNullable
		{
			get
			{
				string s = this.GetString(Columns.f_IsNullable);

				if(s == "YES") 
					return true;
				else
					return false;
			}
		}

		override public System.Boolean HasDefault
		{
			get
			{
				object o = this._row[Columns.f_Default];

				if(o == DBNull.Value)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}


		override public System.Boolean IsAutoKey
		{
			get
			{
				PostgreSQLColumns cols = Columns as PostgreSQLColumns;
				return this.GetBool(cols.f_AutoKey);
			}
		}

		override public System.Boolean IsComputed
		{
			get
			{
				return this.GetBool(Columns.f_IsComputed);
			}
		}


		override public string DataTypeName
		{
			get
			{
				PostgreSQLColumns cols = Columns as PostgreSQLColumns;
				return this.GetString(cols.f_TypeName);
			}
		}

		override public string DataTypeNameComplete
		{
			get
			{
				PostgreSQLColumns cols = Columns as PostgreSQLColumns;
				return this.GetString(cols.f_TypeNameComplete);
			}
		}
	}
}
