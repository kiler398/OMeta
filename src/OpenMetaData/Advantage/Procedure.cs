using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Advantage
{
 
	public class AdvantageProcedure : Procedure
	{
		public AdvantageProcedure()
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
