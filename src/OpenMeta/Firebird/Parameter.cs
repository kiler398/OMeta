using System;
using System.Data;


namespace MyMeta.Firebird
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameter))]
#endif 
	public class FirebirdParameter : Parameter
	{
		public FirebirdParameter()
		{

		}

		public override string DataTypeNameComplete
		{
			get
			{
				FirebirdParameters parameters = this.Parameters as FirebirdParameters;
				return this.GetString(parameters.f_TypeNameComplete);
			}
		}


		override public System.Int32 CharacterMaxLength
		{
			get
			{
				switch(TypeName)
				{
					case "VARCHAR":
					case "CHAR":
						return (System.Int32)this._row["PARAMETER_SIZE"];

					default:
						return this.GetInt32(Parameters.f_CharMaxLength);
				}
			}
		}

		public override Int32 CharacterOctetLength
		{
			get
			{
				return (System.Int32)this._row["PARAMETER_SIZE"];
			}
		}

		override public System.Int32 NumericPrecision
		{
			get
			{
				if(this.TypeName == "NUMERIC")
				{
					switch((int)this._row["PARAMETER_SIZE"])
					{
						case 2:
							return 4;
						case 4:
							return 9;
						case 8:
							return 15;
						default:
							return 18;
					}
				}
				return this.GetInt32(Parameters.f_NumericScale);
			}
		}
	}
}
