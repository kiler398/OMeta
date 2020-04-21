using System;
using System.Data;
using Npgsql;

namespace OMeta.PostgreSQL8
{
 
	public class PostgreSQL8Domain : Domain
	{
		public PostgreSQL8Domain()
		{

		}

		public override string DataTypeNameComplete
		{
			get
			{
				PostgreSQL8Domains domains = this.Domains as PostgreSQL8Domains;
				return this.GetString(domains.f_TypeNameComplete);
			}
		}

	}
}
