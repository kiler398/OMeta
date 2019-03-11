using System;
using System.Data;
using Npgsql;

namespace MyMeta.PostgreSQL8
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedure))]
#endif 
	public class PostgreSQL8Procedure : Procedure
	{
		internal string _specific_name = "";

		public PostgreSQL8Procedure()
		{

		}

		public override IParameters Parameters
		{
			get
			{
				if(null == _parameters)
				{
					_parameters = (PostgreSQL8Parameters)this.dbRoot.ClassFactory.CreateParameters();
					_parameters.Procedure = this;
					_parameters.dbRoot = this.dbRoot;

					string query = "select * from information_schema.parameters where specific_schema = '" +
						this.Database.SchemaName + "' and specific_name = '" + (string)this._row["specific_name"] + "'";

					NpgsqlConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Database.Name);

					DataTable metaData = new DataTable();
					NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);

					adapter.Fill(metaData);
					cn.Close();

					metaData.Columns["udt_name"].ColumnName = "TYPE_NAME";
					metaData.Columns["data_type"].ColumnName = "TYPE_NAMECOMPLETE";

					if(metaData.Columns.Contains("TYPE_NAME"))
						_parameters.f_TypeName = metaData.Columns["TYPE_NAME"];

					if(metaData.Columns.Contains("TYPE_NAMECOMPLETE"))
						_parameters.f_TypeNameComplete = metaData.Columns["TYPE_NAMECOMPLETE"];
		
					_parameters.PopulateArray(metaData);

				}
				return _parameters;
			}
		}

		private PostgreSQL8Parameters _parameters = null;
	}
}
