using System;
using System.Data;
using Npgsql;


namespace OMeta.PostgreSQL
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedures))]
#endif 
	public class PostgreSQLProcedures : Procedures
	{
		public PostgreSQLProcedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
//				string query = 
//					@"SELECT prosrc as PROCEDURE_DEFINITION,
//					proname as PROCEDURE_NAME, nspname as PROCEDURE_SCHEMA, t.typname as rettype,
//					p.proargtypes as args FROM pg_proc p, pg_type t, pg_namespace n, pg_language l
//					WHERE p.prorettype = t.oid AND p.pronamespace = n.oid AND p.prolang = l.oid
//					AND l.lanname <> 'c' AND l.lanname <> 'internal' and nspname = 'public';";
//
//				NpgsqlConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Database.Name);
//
//				DataTable metaData = new DataTable();
//				NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);
//
//				adapter.Fill(metaData);
//				cn.Close();
//		
//				PopulateArray(metaData);
//
//				DataRowCollection rows = metaData.Rows;
//
//				PostgreSQLProcedure p;
//
//				for(int i = 0; i < this.Count; i++)
//				{
//					p = this[i] as PostgreSQLProcedure;
//					if(rows[i]["args"] != DBNull.Value)
//					{
//						p.param_types = rows[i]["args"] as string;
//					}
//				}
			}
			catch {}
		}

		override public IProcedure this[object name]
		{
			get
			{
				return base[name];
			}
		}
	}
}
