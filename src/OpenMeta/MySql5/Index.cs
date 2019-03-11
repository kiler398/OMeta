using System;
using System.Data;

namespace MyMeta.MySql5
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndex))]
#endif 
	public class MySql5Index : Index
	{
		public MySql5Index()
		{

		}

		public override string Type
		{
			get
			{
				return this.GetString(Indexes.f_Type);
			}
		}

		public override Boolean Unique
		{
			get
			{
				// We have to reverse the meaning
				return (base.Unique) ? false : true;
			}
		}

		public override string Collation
		{
			get
			{
				string s = this.GetString(Indexes.f_Collation);

				switch(s)
				{
					case "A":
						return "ASCENDING";
					case "D":
						return "DECENDING";
					default:
						return "UNKNOWN";
				}
			}
		}
	}
}
