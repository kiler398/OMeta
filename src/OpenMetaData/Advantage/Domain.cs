using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Advantage
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDomain))]
#endif 
	public class AdvantageDomain : Domain	
	{
		public AdvantageDomain()
		{

		}

		override public string DataTypeNameComplete
		{
			get
			{
				switch(this.DataTypeName)
				{
					case "binary":
					case "char":
					case "nchar":
					case "nvarchar":
					case "varchar":
					case "varbinary":

						return this.DataTypeName + "(" + this.CharacterMaxLength + ")";

					case "decimal":
					case "numeric":

						return this.DataTypeName + "(" + this.NumericPrecision + "," + this.NumericScale + ")";

					default:

						return this.DataTypeName;
				}
			}
		}
	}
}
