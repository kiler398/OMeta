using System;
using System.Data;
using Npgsql;

namespace MyMeta.PostgreSQL8
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameters))]
#endif 
	public class PostgreSQL8Parameters : Parameters
	{
		public PostgreSQL8Parameters()
		{

		}

		internal DataColumn f_TypeNameComplete	= null;

		override internal void LoadAll()
		{
			try
			{

			}
			catch {}
		}
	}
}
