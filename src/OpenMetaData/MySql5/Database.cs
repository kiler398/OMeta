using System;
using System.Data;

namespace OMeta.MySql5
{
 
	public class MySql5Database : Database
	{
		public MySql5Database()
		{

		}

		override public string Alias
		{
			get
			{
				return _name;
			}
		}

		override public string Name
		{
			get
			{
				return _name;
			}
		}

		override public string Description
		{
			get
			{
				return _desc;
			}
		}

		internal string _name = "";
		internal string _desc = "";

		internal bool _FKsInLoad = false;
	}
}
