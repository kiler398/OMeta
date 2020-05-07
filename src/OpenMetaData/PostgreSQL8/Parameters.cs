using System;
using System.Data;
using Npgsql;

namespace OMeta.PostgreSQL8
{
 
	public class PostgreSQL8Parameters : Parameters
	{
		public PostgreSQL8Parameters()
		{

		}

		internal DataColumn f_TypeNameComplete	= null;

		override internal void LoadAll()
		{
        }
	}
}
