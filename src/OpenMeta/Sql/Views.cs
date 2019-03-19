using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Sql
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IViews))]
#endif 
	public class SqlViews : Views
	{
		public SqlViews()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string type = this.dbRoot.ShowSystemData ? "SYSTEM VIEW" : "VIEW";
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Tables, new Object[] {this.Database.Name, null, null, type});

				PopulateArray(metaData);

				LoadDescriptions();
			}
			catch {}
		}

		private void LoadDescriptions()
		{
			try
			{
				string select = @"SELECT objName, value FROM ::fn_listextendedproperty ('MS_Description', 'user', 'dbo', 'view', null, null, null)";

				OleDbConnection cn = new OleDbConnection(dbRoot.ConnectionString);
				cn.Open();
				cn.ChangeDatabase("[" + this.Database.Name + "]");

				OleDbDataAdapter adapter = new OleDbDataAdapter(select, cn);
				DataTable dataTable = new DataTable();

				adapter.Fill(dataTable);

				cn.Close();

				View v;

				foreach(DataRow row in dataTable.Rows)
				{
					v = this[row["objName"] as string] as View;

					if(null != v)
					{
						v._row["DESCRIPTION"] = row["value"] as string;
					}
				}
			}
			catch
			{
			
			}
		}
	}
}
