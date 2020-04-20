using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameter))]
#endif 
	public class SQLiteParameter : Parameter
	{
		public SQLiteParameter()
		{

		}

		override public string DataTypeNameComplete
		{
			get
			{
				/*switch(this.TypeName)
				{
					case "binary":
					case "char":
					case "nchar":
					case "nvarchar":
					case "varchar":
					case "varbinary":

						return this.TypeName + "(" + this.CharacterMaxLength + ")";

					case "decimal":
					case "numeric":

						return this.TypeName + "(" + this.NumericPrecision + "," + this.NumericScale + ")";

					default:*/

						return this.TypeName;
				//}
			}
		}
	}
}
