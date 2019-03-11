using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.DB2
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumns))]
#endif 
	public class DB2Columns : Columns
	{
		public DB2Columns()
		{

		}

		internal DataColumn f_TypeName		= null;
		internal DataColumn f_AutoKey		= null;
		internal DataColumn f_InPrimaryKey	= null;


		override internal void LoadForTable()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Columns, new Object[] {null, null, this.Table.Name});

			PopulateArray(metaData);

			LoadExtraData("T");
		}

		override internal void LoadForView()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Columns, new Object[] {null, null, this.View.Name});

			PopulateArray(metaData);

			LoadExtraData("V");
		}

		private void LoadExtraData(string type)
		{
			try
			{
				string name = ("T" == type) ? this.Table.Name :  this.View.Name;
				string select = "SELECT COLNAME, TYPENAME, CODEPAGE, IDENTITY FROM SYSCAT.COLUMNS WHERE TABNAME = '" + name + "' ORDER BY COLNO";

				OleDbDataAdapter adapter = new OleDbDataAdapter(select, this.dbRoot.ConnectionString);
				DataTable dataTable = new DataTable();

				adapter.Fill(dataTable);

				if(this._array.Count > 0)
				{
					Column col = this._array[0] as Column;

					f_TypeName = new DataColumn("TYPE_NAME", typeof(string));
					col._row.Table.Columns.Add(f_TypeName);

					f_AutoKey  = new DataColumn("AUTO_INCREMENT", typeof(Boolean));
					col._row.Table.Columns.Add(f_AutoKey);

					DataRowCollection rows = dataTable.Rows;
					string identity = "";
					string colName = "";

					int count = this._array.Count;
					Column c = null;

					foreach(DataRow row in rows)
					{
						colName = row["COLNAME"] as string;

						c = this[colName] as Column;

						identity = row["IDENTITY"] as string;
						c._row["AUTO_INCREMENT"] = identity == "Y" ? true : false;
						c._row["TYPE_NAME"]      = (row["TYPENAME"] as string).Trim();

						int codepage = -1;
						try
						{
							codepage = (short)row["CODEPAGE"];
						}
						catch{}

						if(codepage == 0)
						{
							// Check for "bit data"
							switch(c.DataTypeName)
							{
								case "CHARACTER":
								case "VARCHAR":
								case "LONG VARCHAR":

									c._row["TYPE_NAME"] = c.DataTypeName + " FOR BIT DATA";
									break;
							}
						}
					}
				}
			}
			catch {}
		}
	}
}
