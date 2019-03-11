using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.ISeries
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumns))]
#endif 
	public class ISeriesColumns : Columns
	{
		public ISeriesColumns()
		{

		}

		internal DataColumn f_TypeName		= null;
		internal DataColumn f_AutoKey		= null;
		internal DataColumn f_InPrimaryKey	= null;
		internal DataColumn f_FullTypeName	= null;


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
				string schema = ("T" == type) ? this.Table.Schema :  this.View.Schema;
				//string select = "SELECT COLNAME, TYPENAME, CODEPAGE, IDENTITY FROM SYSCOLUMNS WHERE TABNAME = '" + name + "' ORDER BY COLNO";
				string select = "SELECT COLUMN_NAME, DATA_TYPE, CCSID, IS_IDENTITY FROM SYSCOLUMNS WHERE TABLE_SCHEMA = '" + schema + "' AND TABLE_NAME = '" + name + "' ORDER BY ORDINAL_POSITION";

				OleDbDataAdapter adapter = new OleDbDataAdapter(select, this.dbRoot.ConnectionString);
				DataTable dataTable = new DataTable();

				adapter.Fill(dataTable);

				if(this._array.Count > 0)
				{
					Column col = this._array[0] as Column;

					if (col._row.Table.Columns.IndexOf("TYPE_NAME") == -1) 
					{
						f_TypeName = new DataColumn("TYPE_NAME", typeof(string));
						col._row.Table.Columns.Add(f_TypeName);
					}

					if (col._row.Table.Columns.IndexOf("FULL_TYPE_NAME") == -1) 
					{
						f_FullTypeName = new DataColumn("FULL_TYPE_NAME", typeof(string));
						col._row.Table.Columns.Add(f_FullTypeName);
					}

					if (col._row.Table.Columns.IndexOf("AUTO_INCREMENT") == -1) 
					{
						f_AutoKey  = new DataColumn("AUTO_INCREMENT", typeof(Boolean));
						col._row.Table.Columns.Add(f_AutoKey);
					}

					DataRowCollection rows = dataTable.Rows;
					string identity = "";
					string colName = "";

					int count = this._array.Count;
					Column c = null;

					foreach(DataRow row in rows)
					{
						colName = row["COLUMN_NAME"] as string;

						c = this[colName] as Column;

						identity = row["IS_IDENTITY"] as string;
						c._row["AUTO_INCREMENT"] = identity[0] == 'Y' ? true : false;
						c._row["TYPE_NAME"]      = (row["DATA_TYPE"] as string).Trim();

						int codepage = -1;
						try
						{
							codepage = (int)row["CCSID"];
						}
						catch{}

						
						string fulltypename = c.DataTypeName;
						switch (fulltypename) 
						{
							case "CHAR":
							case "VARCHAR":
							case "GRAPHIC":
							case "VARGRAPHIC":
							case "BINARY":
							case "VARBINARY":
							case "CLOB":
							case "BLOB":
							case "DBCLOB":
							case "DATALINK":
								int tmp = Int32.Parse(c._row[this.f_MaxLength].ToString());
								fulltypename += "(" + tmp + ")";
								break;
							case "DECIMAL":
							case "NUMERIC":
								int tmp1 = Int32.Parse(c._row[this.f_NumericPrecision].ToString());
								int tmp2 = Int32.Parse(c._row[this.f_NumericScale].ToString());
								fulltypename += "(" + tmp1 + "," + tmp2 + ")";
								break;
						}

						if(codepage == 65535)
						{
							// Check for "bit data"
							switch(c.DataTypeName)
							{
								case "CHAR":
								case "VARCHAR":
									c._row["TYPE_NAME"] = c.DataTypeName + " FOR BIT DATA";
									fulltypename += " FOR BIT DATA";
									break;
							}
						}
						else 
						{
							// Check for "bit data"
							switch(c.DataTypeName)
							{
								case "CHAR":
								case "VARCHAR":
									fulltypename += " CCSID " + codepage;
									break;
							}
						}

						c._row["FULL_TYPE_NAME"] = fulltypename;
					}
				}
			}
			catch {}
		}
	}
}
