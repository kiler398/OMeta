using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.PostgreSQL
{
 
	public class PostgreSQLIndex : Index
	{
		public PostgreSQLIndex()
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
