using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.PostgreSQL
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedure))]
#endif 
	public class PostgreSQLProcedure : Procedure
	{
		internal string param_types = "";

		public PostgreSQLProcedure()
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

		public override IParameters Parameters
		{
			get
			{
				if(null == _parameters)
				{
					_parameters = (Parameters)this.dbRoot.ClassFactory.CreateParameters();
					_parameters.Procedure = this;
					_parameters.dbRoot = this.dbRoot;

//					if(this.param_types.Length > 0)
//					{
//						string query = "SELECT typname::text FROM pg_type WHERE oid IN(";
//						query += this.param_types.Replace(" ", ",");
//						query += ");";
//					}
				}
				return _parameters;
			}
		}

		private Parameters _parameters = null;
	}
}
