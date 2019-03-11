using System;
using System.Data;


namespace MyMeta.Firebird
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedure))]
#endif 
	public class FirebirdProcedure : Procedure
	{
		public FirebirdProcedure()
		{

		}

		override public string Alias
		{
			get
			{
				string[] name = base.Name.Split(';');

				return name[0];
			}
		}

		override public string Name
		{
			get
			{
				string[] name = base.Name.Split(';');

				return name[0];
			}
		}
	}
}
