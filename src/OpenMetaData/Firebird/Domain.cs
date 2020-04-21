using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Firebird
{
 
	public class FirebirdDomain : Domain
	{
		public FirebirdDomain()
		{

		}

		override public string DataTypeNameComplete
		{
			get
			{
				FirebirdDomains domains = Domains as FirebirdDomains;
				return this.GetString(domains.f_TypeNameComplete);
			}
		}

		override public System.Int32 CharacterMaxLength
		{
			get
			{
				switch(DataTypeName)
				{
					case "VARCHAR":
					case "CHAR":
						return (int)this._row["DOMAIN_SIZE"];

					default:
						return this.GetInt32(Domains.f_MaxLength);
				}
			}
		}

		public override Int32 NumericPrecision
		{
			get
			{
				switch(DataTypeName)
				{
					case "VARCHAR":
					case "CHAR":
						return 0;

					default:
						return base.NumericPrecision;
				}
			}
		}
	}
}
