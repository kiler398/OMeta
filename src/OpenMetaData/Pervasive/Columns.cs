using System;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;

namespace OMeta.Pervasive
{
 
	public class PervasiveColumns : Columns
	{
		public PervasiveColumns()
		{

		}

		internal DataColumn f_TypeName			= null;
		internal DataColumn f_AutoKey			= null;

		override internal void LoadForTable()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Columns, new Object[] {null, null, this.Table.Name});

			metaData.DefaultView.Sort = "ORDINAL_POSITION";

			PopulateArray(metaData);

			LoadExtraDataForTable();
		}

		override internal void LoadForView()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Columns, new Object[] {null, null, this.View.Name});

			metaData.DefaultView.Sort = "ORDINAL_POSITION";

			PopulateArray(metaData);

			LoadExtraDataForView();
		}

		private void LoadExtraDataForTable()
		{
			try
			{
				string select = "SELECT \"X$Field\".Xe$Name AS Name, \"X$Field\".Xe$DataType as Type, \"X$Field\".Xe$Size AS Size, \"X$Field\".Xe$Flags AS Flags " +
								"FROM X$File,X$Field WHERE Xf$Id=Xe$File AND Xf$Name = '" + this.Table.Name + "' " +
								"AND \"X$Field\".Xe$DataType < 27 " +
								"ORDER BY Xe$Offset";

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

					string typeName = "";
					DataRowCollection rows = dataTable.Rows;

					int type  = 0;
					int size  = 0;
					int flags = 0;
	
					int index = 0;
					foreach(Column c in this)
					{
						DataRow row = rows[index++];

						type  = Convert.ToInt32(row["Type"]);
						size  = Convert.ToInt32(row["Size"]);
						flags = Convert.ToInt32(row["Flags"]);
						

						switch(type)
						{
							case 0:

								if(flags == 4100)
								{
									typeName = "BINARY";
								}
								else
								{
									typeName = "CHAR";
								}
								break;


							case 1:

							switch(size)
							{
								case 1:
									typeName = "TINYINT";
									break;
								case 2:
									typeName = "SMALLINT";
									break;
								case 4:
									typeName = "INTEGER";
									break;
								case 8:
									typeName = "BIGINT";
									break;
							}
								break;

							case 2:

							switch(size)
							{
								case 4:
									typeName = "REAL";
									break;
								case 8:
									typeName = "DOUBLE";
									break;
							}
								break;

							case 3:

								typeName = "DATE";
								break;

							case 4:

								typeName = "TIME";
								break;

							case 5:

								typeName = "DECIMAL";
								break;

							case 6:

								typeName = "MONEY";
								break;

							case 8:

								typeName = "NUMERIC";
								break;

							case 9:

							switch(size)
							{
								case 4:
									typeName = "BFLOAT4";
									break;
								case 8:
									typeName = "BFLOAT8";
									break;
							}
								break;

							case 11:

								typeName = "VARCHAR";
								break;

							case 14:

							switch(size)
							{
								case 1:
									typeName = "UTINYINT";
									break;
								case 2:
									typeName = "USMALLINT";
									break;
								case 4:
									typeName = "UINTEGER";
									break;
								case 8:
									typeName = "UBIGINT";
									break;
							}
								break;

							case 15:

							switch(size)
							{
								case 2:
									typeName = "SMALLIDENTITY";
									c._row["AUTO_INCREMENT"] = true;
									break;
								case 4:
									typeName = "IDENTITY";
									c._row["AUTO_INCREMENT"] = true;
									break;
							}
								break;

							case 16:

								typeName = "BIT";
								break;

							case 17:

								typeName = "NUMERICSTS";
								break;

							case 18:

								typeName = "NUMERICSA";
								break;

							case 19:

								typeName = "CURRENCY";
								break;

							case 20:

								typeName = "TIMESTAMP";
								break;

							case 21:

								if(flags == 4100)
								{
									typeName = "LONGVARBINARY";
								}
								else
								{
									typeName = "LONGVARCHAR";
								}
								break;

							case 22:

								typeName = "LONGVARBINARY";
								break;

							case 25:

								typeName = "WSTRING";
								break;

							case 26:

								typeName = "WSZSTRING";
								break;
						}

						c._row["TYPE_NAME"] = typeName;
					}
				}
			}
			catch {}
		}

		private void LoadExtraDataForView()
		{
			try
			{
				if(this._array.Count > 0)
				{
					ADODB.Connection cnn = new ADODB.Connection();
					ADODB.Recordset rs = new ADODB.Recordset();
					ADOX.Catalog cat = new ADOX.Catalog();
    
					// Open the Connection
					cnn.Open(dbRoot.ConnectionString, null, null, 0);
					cat.ActiveConnection = cnn;

					rs.Source = cat.Views[this.View.Name].Command;
					rs.Fields.Refresh();
					ADODB.Fields flds = rs.Fields;

					Column col = this._array[0] as Column;

					f_TypeName = new DataColumn("TYPE_NAME", typeof(string));
					col._row.Table.Columns.Add(f_TypeName);

					f_AutoKey  = new DataColumn("AUTO_INCREMENT", typeof(Boolean));
					col._row.Table.Columns.Add(f_AutoKey);

					Column c = null;
					ADODB.Field fld = null;

					int count = this._array.Count;
					for( int index = 0; index < count; index++)
					{
						fld = flds[index];
						c   = (Column)this[fld.Name];

						c._row["TYPE_NAME"]      = fld.Type.ToString();
						c._row["AUTO_INCREMENT"] = false;
					}

					rs.Close();
					cnn.Close();
				}
			}
			catch {}
		}
	}
}
