using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Pervasive
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IForeignKeys))]
#endif 
	public class PervasiveForeignKeys : ForeignKeys
	{
		public PervasiveForeignKeys()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string sql = "SELECT DATABASE() AS PK_TABLE_CATALOG, DATABASE() AS FK_TABLE_CATALOG, R.Xr$Name AS FK_NAME, PT.Xf$Name AS PK_TABLE_NAME, FT.Xf$Name AS FK_TABLE_NAME, " +
					         "PIF.Xe$Name AS PK_COLUMN_NAME, FIF.Xe$Name AS FK_COLUMN_NAME, NULL AS PK_TABLE_SCHEMA, NULL AS FK_TABLE_SCHEMA, '' AS PK_NAME," +
							 "CASE R.Xr$UpdateRule WHEN 1 THEN 'RESTRICT' ELSE 'UNKNOWN' END AS UPDATE_RULE, " + 
							 "CASE R.Xr$DeleteRule WHEN 1 THEN 'RESTRICT' WHEN 2 THEN 'CASCADE' ELSE 'UNKNOWN' END AS DELETE_RULE " +
							 "FROM \"X$Relate\" \"R\" "  +
							 "JOIN \"X$File\" \"PT\" ON R.Xr$Pid=PT.Xf$Id " +
							 "JOIN \"X$File\" \"FT\" ON R.Xr$FId=FT.Xf$Id " +
							 "JOIN \"X$Index\" \"PI\" ON R.Xr$Index=\"PI\".Xi$Number AND PT.Xf$Id=\"PI\".Xi$File " +
							 "JOIN \"X$Field\" \"PIF\" ON \"PI\".Xi$Field=PIF.Xe$Id " +
							 "JOIN \"X$Index\" \"FI\" ON R.Xr$FIndex=\"FI\".Xi$Number AND FT.Xf$Id=\"FI\".Xi$File " +
							 "JOIN \"X$Field\" \"FIF\" ON \"FI\".Xi$Field=FIF.Xe$Id " +
							 "WHERE PK_TABLE_NAME = '" + this.Table.Name + "' OR FK_TABLE_NAME = '" +  this.Table.Name + "'";

				OleDbDataAdapter adapter = new OleDbDataAdapter(sql, this.dbRoot.ConnectionString);
				DataTable dataTable = new DataTable();

				adapter.Fill(dataTable);

				// Getting the primary key name in that view above believe it or not is quite difficult 
				OleDbCommand cmd = new OleDbCommand("call psp_pkeys(null,'" + this.Table.Name + "')");
				cmd.Connection = new OleDbConnection(this.dbRoot.ConnectionString);
				
				OleDbDataAdapter adapter1 = new OleDbDataAdapter(cmd);
				DataTable pkName = new DataTable();
				adapter1.Fill(pkName);

				if(pkName.Rows.Count > 0 && dataTable.Rows.Count > 0)
				{
					string PK_NAME = (pkName.Rows[0]["PK_NAME"] as String).Trim();

					DataRowCollection rows = dataTable.Rows;
					DataRow row;
					for(int i = 0; i < dataTable.Rows.Count; i++)
					{
						row = rows[i];
						row["PK_NAME"] = PK_NAME;
					}
				}

				PopulateArray(dataTable);
			}
			catch {}
//
//				DataTable metaData1 = this.LoadData(OleDbSchemaGuid.Foreign_Keys, 
//					new object[]{this.Table.Database.Name, null, this.Table.Name});
//
//				DataTable metaData2 = this.LoadData(OleDbSchemaGuid.Foreign_Keys, 
//					new object[]{null, null, null, null, null, this.Table.Name});
//
//				DataRowCollection rows = metaData2.Rows;
//				int count = rows.Count;
//				for(int i = 0; i < count; i++)
//				{
//					metaData1.ImportRow(rows[i]);
//				}
//
//				PopulateArray(metaData1);
//			}
//			catch {}
		}
	}
}
