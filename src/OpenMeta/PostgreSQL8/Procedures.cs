using System;
using System.Data;
using Npgsql;


namespace MyMeta.PostgreSQL8
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedures))]
#endif 
	public class PostgreSQL8Procedures : Procedures
	{
		internal string _specific_name = "";

		public PostgreSQL8Procedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string query = "SELECT routine_definition as PROCEDURE_DEFINITION, specific_name, " +
					"routine_name as PROCEDURE_NAME, routine_schema as PROCEDURE_SCHEMA, routine_catalog as PROCEDURE_CATALOG " +
					"from information_schema.routines where routine_schema = '" + this.Database.SchemaName + 
					"' and routine_catalog = '" + this.Database.Name + "'";

				NpgsqlConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Database.Name);

				DataTable metaData = new DataTable();
				NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);

				adapter.Fill(metaData);
				cn.Close();
		
				PopulateArray(metaData);
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
