using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Advantage
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IForeignKey))]
#endif 
	public class AdvantageForeignKey : ForeignKey
	{
		public AdvantageForeignKey()
		{

		}

		public override string DeleteRule
		{
			get
			{
				System.Int16 i = this.GetInt16(ForeignKeys.f_DeleteRule);

				switch(i)
				{
					case 1:
						return "CASCASE";
					case 2:
						return "RESTRICT";
					case 3:
						return "SET NULL";
					case 4:
						return "SET DEFAULT";
					default:
						return "UNKNOWN";
				}
			}
		}

		public override string UpdateRule
		{
			get
			{
				System.Int16 i = this.GetInt16(ForeignKeys.f_UpdateRule);

				switch(i)
				{
					case 1:
						return "CASCASE";
					case 2:
						return "RESTRICT";
					case 3:
						return "SET NULL";
					case 4:
						return "SET DEFAULT";
					default:
						return "UNKNOWN";
				}
			}
		}
	}
}
