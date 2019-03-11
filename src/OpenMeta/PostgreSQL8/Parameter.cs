using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.PostgreSQL8
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameter))]
#endif 
	public class PostgreSQL8Parameter : Parameter
	{
		public PostgreSQL8Parameter()
		{

		}

		public override string Name
		{
			get
			{
				string n = base.Name;

				if(n == string.Empty)
				{
					n = "[" + this.Parameters.Procedure.Name + ":" + this.Ordinal.ToString() + "]";
				}

				return n;
			}
		}

		public override ParamDirection Direction
		{
			get
			{
				return ParamDirection.Input;
			}
		}



		override public string DataTypeNameComplete
		{
			get
			{
				PostgreSQL8Parameters parameters = this.Parameters as PostgreSQL8Parameters;
				return this.GetString(parameters.f_TypeNameComplete);
			}
		}
	}
}
