using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.PostgreSQL8
{
 
	public class PostgreSQL8Index : Index
	{
		public PostgreSQL8Index()
		{

		}

		public override string Type
		{
			get
			{
				string type = this.GetString(Indexes.f_Type);
				return type.ToUpper();
			}
		}
	}
}
