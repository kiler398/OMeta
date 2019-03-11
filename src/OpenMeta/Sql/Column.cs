using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Sql
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumn))]
#endif 
	public class SqlColumn : Column
	{
		public SqlColumn()
		{

		}

		override internal Column Clone()
		{
			Column c = base.Clone();

			return c;
		}

		override public System.Boolean IsAutoKey
		{
			get
			{
				SqlColumns cols = Columns as SqlColumns;
				return this.GetBool(cols.f_AutoKey);
			}
		}

		override public System.Boolean IsComputed
		{
			get
			{
				if(this.DataTypeName == "timestamp") return true;

				return this.GetBool(Columns.f_IsComputed);
			}
		}


		override public string DataTypeName
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DataTypeName;
						}
					}
				}

				SqlColumns cols = Columns as SqlColumns;
				return this.GetString(cols.f_TypeName);
			}
		}

		override public string DataTypeNameComplete
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DataTypeNameComplete;
						}
					}
				}

                string dtnf = GetFullDataTypeName(DataTypeName, CharacterMaxLength, NumericPrecision, NumericScale);
                /*switch(this.DataTypeName)
				{
                    case "varchar":
                    case "nvarchar":
                    case "varbinary":
                        if (this.CharacterMaxLength > 1000000)
                            dtnf = this.DataTypeName + "(MAX)";
                        else
                            dtnf = this.DataTypeName + "(" + this.CharacterMaxLength + ")";
                        break;
					case "binary":
					case "char":
					case "nchar":

                        dtnf = this.DataTypeName + "(" + this.CharacterMaxLength + ")";
                        break;

					case "decimal":
					case "numeric":

                        dtnf = this.DataTypeName + "(" + this.NumericPrecision + "," + this.NumericScale + ")";
                        break;

					default:

                        dtnf = this.DataTypeName;
                        break;
				}*/

                return dtnf;
			}
		}

		public override object DatabaseSpecificMetaData(string key)
		{
			return SqlDatabase.DBSpecific(key, this);
		}

        internal static string GetFullDataTypeName(string name, int charMaxLen, int precision, int scale)
        {
            string dtnf = null;
            switch (name)
            {
                case "varchar":
                case "nvarchar":
                case "varbinary":
                    if (charMaxLen > 1000000)
                        dtnf = name + "(MAX)";
                    else
                        dtnf = name + "(" + charMaxLen + ")";
                    break;
                case "binary":
                case "char":
                case "nchar":

                    dtnf = name + "(" + charMaxLen + ")";
                    break;

                case "decimal":
                case "numeric":

                    dtnf = name + "(" + precision + "," + scale + ")";
                    break;

                default:

                    dtnf = name;
                    break;
            }

            return dtnf;
        }
	}
}
