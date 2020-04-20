using System;
using System.Data;

namespace OMeta.MySql5
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumn))]
#endif 
	public class MySql5Column : Column
	{
		static char[] chars = new char[] {' ', '('};

		private int numericScale = 0;
		private int precision = 0;
		private int characterLength = 0;
		private string dataType = "";

		public MySql5Column()
		{

		}

		override internal Column Clone()
		{
			Column c = base.Clone();

			return c;
		}

		public override Boolean IsNullable
		{
			get
			{
				MySql5Columns cols = Columns as MySql5Columns;
				string s = this.GetString(cols.f_IsNullable);
				return (s == "YES") ? true : false;
			}
		}

		public override Boolean HasDefault
		{
			get
			{
				return (this.Default == "") ? false : true;
			}
		}

		override public System.Boolean IsAutoKey
		{
			get
			{
				bool isAutoKey = false;

				MySql5Columns cols = Columns as MySql5Columns;
				string s = this.GetString(cols.f_Extra);

				if(-1 != s.IndexOf("auto_increment"))
				{
					isAutoKey = true;
				}

				return isAutoKey;
			}
		}

		override public string DataTypeName
		{
			get
			{
				if(dataType == "")
				{
					MySql5Columns cols = Columns as MySql5Columns;
					string type = this.GetString(cols.f_DataType).ToUpper();

					string[] data = type.Split(new char[]{' '});
					string[] typeandsize = data[0].Split(new char[]{'(',')',','});

					dataType = typeandsize[0];

					if(dataType != "ENUM")
					{
						if(-1 != type.IndexOf("UNSIGNED"))
						{
							dataType += " UNSIGNED";
						}

						int parts = typeandsize.Length;

						if(parts >= 2)
						{
							if(dataType == "VARCHAR" || dataType == "CHAR")
							{
								this.characterLength = Convert.ToInt32(typeandsize[1]);
							}
							else
							{
								this.precision = Convert.ToInt32(typeandsize[1]);
							}
						}

						if(parts >= 3)
						{
							if(typeandsize[2].Length > 0)
							{
								this.numericScale = Convert.ToInt32(typeandsize[2]);
							}
						}
					}
				}

				return dataType;
			}
		}

		override public string DataTypeNameComplete
		{
			get
			{
				try
				{
					MySql5Columns cols = Columns as MySql5Columns;
					string origType = GetString(cols.f_DataType);
					string type = origType.ToUpper();

					string[] data = type.Split(new char[]{' '});

					if(data[0].StartsWith("ENUM"))
					{
						return "ENUM" + origType.Substring(4, origType.Length - 4);
					}
					else
					{
						if(-1 != type.IndexOf("UNSIGNED"))
						{
							return data[0] + " UNSIGNED";
						}
						else
						{
							return data[0];
						}
					}
				}
				catch
				{
					return "ERROR";
				}
			}
		}

		public override Int32 NumericPrecision
		{
			get
			{
				return this.precision;
			}
		}

		public override Int32 NumericScale
		{
			get
			{
				return this.numericScale;
			}
		}

		public override Int32 CharacterMaxLength
		{
			get
			{
				return this.characterLength;
			}
		}
	}
}
