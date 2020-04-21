using System;
using System.Data;


namespace OMeta.Firebird
{
 
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
