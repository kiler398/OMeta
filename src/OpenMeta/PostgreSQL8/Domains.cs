using System;
using System.Data;
using Npgsql;

namespace MyMeta.PostgreSQL8
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDomains))]
#endif 
	public class PostgreSQL8Domains : Domains
	{
		public PostgreSQL8Domains()
		{

		}

		internal DataColumn f_TypeNameComplete	= null;

		internal override void LoadAll()
		{
			string query = "select * from information_schema.domains where domain_catalog = '" + this.Database.Name + 
				"' and domain_schema = '" + this.Database.SchemaName + "'";

			NpgsqlConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Database.Name);

			DataTable metaData = new DataTable();
			NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);

			adapter.Fill(metaData);
			cn.Close();

			metaData.Columns["udt_name"].ColumnName = "DATA_TYPE";
			metaData.Columns["data_type"].ColumnName = "TYPE_NAMECOMPLETE";

			if(metaData.Columns.Contains("TYPE_NAMECOMPLETE"))
				f_TypeNameComplete = metaData.Columns["TYPE_NAMECOMPLETE"];
		
			PopulateArray(metaData);
		}
	}
}
