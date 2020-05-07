using System;
using System.Data;
using System.Collections;

using System.Data.SQLite;

namespace OMeta.SQLite
{
	/// <summary>
	/// Summary description for SQLiteMetaData.
	/// </summary>
	public class MetaDataHelper
	{
		public const string COLUMN_IS_KEY = "IS_KEY";
		public const string FULL_DATA_TYPE = "FULL_DATA_TYPE";
		public const string DATA_TYPE_NAME = "DATA_TYPE_NAME";

		private ArrayList _tables = new ArrayList();
		private ArrayList _indeces = new ArrayList();
		private ArrayList _views = new ArrayList();
		private ArrayList _triggers = new ArrayList();

		private DataTable _metaTables;
		private DataTable _metaViews;
		private DataTable _metaIndexes;
		private DataTable _metaFKs;

		private Hashtable _indexColumns = new Hashtable();
		private Hashtable _tableColumns = new Hashtable();
		private Hashtable _viewColumns = new Hashtable();
		private Hashtable _fkColumns = new Hashtable();

		public MetaDataHelper(SQLiteConnection cn)
		{
			DataTable metaTableColumns;
			DataTable metaViewColumns;

			SQLiteCommand command = cn.CreateCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = @"SELECT [type], [name], [tbl_name], [rootpage], [sql] FROM [sqlite_master]";

			var reader = command.ExecuteReader() as SQLiteDataReader;

			string[] items;
			while (reader.Read()) 
			{
				items = new string[4];
				string type = reader.GetString(0).ToLower();

				object v;

				v = reader.GetValue(1);
				items[0] = Convert.ToString(v);

				v = reader.GetValue(2);
				items[1] = Convert.ToString(v);

				v = reader.GetValue(3);
				items[2] = Convert.ToString(v);

				v = reader.GetValue(4);
				items[3] = Convert.ToString(v);

				switch (type) 
				{
					case "trigger":
						_triggers.Add(items);
						break;
					case "view":
						_views.Add(items);
						break;
					case "index":
						_indeces.Add(items);
						break;
					case "table":
						_tables.Add(items);
						break;
				}
			}

			reader.Close();

			string fullname, name;
			int precision, scale;

			_metaTables = this.CreateEmptyMeta_Entities(false);
			DataTable tempIndexTable = CreateTempIndexTable();
			DataTable schemaTable;
			DataTable pragmaTable;
			Hashtable rowMap = new Hashtable();
			foreach (string[] data in _tables) 
			{
				metaTableColumns = CreateEmptyMeta_Columns(false);

				string tableName = data[0];
				string rawSQL = data[3];

				this.FillMetaTables(cn, tableName, out schemaTable, out pragmaTable);
				this.CheckForIndexes(cn, tableName, tempIndexTable);
								
				// First, Add the table record:
				DataRow row = _metaTables.NewRow();
				row["TABLE_NAME"] = tableName;
				row["TABLE_TYPE"] = "TABLE";
				row["IS_UPDATABLE"] = true;
				row["TABLE_DEFINITION"] = rawSQL;
				_metaTables.Rows.Add(row);

				int count = 0;
				foreach (DataRow pragmaRow in pragmaTable.Rows) 
				{
					row = metaTableColumns.NewRow();
					
					row["TABLE_NAME"] = tableName;
					row["COLUMN_NAME"] = pragmaRow["name"];
					row["COLUMN_PROPID"] =  Convert.ToInt32(pragmaRow["cid"]);
					row["IS_NULLABLE"] = pragmaRow["notnull"].ToString() == "0";
					row["ORDINAL_POSITION"] = ++count;
					row["COLUMN_HASDEFAULT"] = (pragmaRow["dflt_value"] != DBNull.Value);		
					row["COLUMN_DEFAULT"] = pragmaRow["dflt_value"];
					row[COLUMN_IS_KEY] = (pragmaRow["pk"].ToString() == "1");
					
					fullname = pragmaRow["type"].ToString().ToUpper();
					this.ParseDataType(fullname, out name, out precision, out scale);
					row[FULL_DATA_TYPE] = fullname;
					row[DATA_TYPE_NAME] = name;
					if (scale != Int32.MinValue) 
					{
						row["NUMERIC_PRECISION"] = precision;
						row["NUMERIC_SCALE"] = scale;
					}
					else if (precision != Int32.MinValue) 
					{
						row["CHARACTER_MAXIMUM_LENGTH"] = precision;
					}
					
					metaTableColumns.Rows.Add(row);
				}

				this._tableColumns.Add(tableName, metaTableColumns);
			}

			_metaViews = this.CreateEmptyMeta_Entities(true);
			rowMap = new Hashtable();
			foreach (string[] data in _views) 
			{
				rowMap.Clear();
				metaViewColumns = CreateEmptyMeta_Columns(true);

				string viewName = data[0];
				string rawSQL = data[3];

				this.FillMetaTables(cn, viewName, out schemaTable, out pragmaTable);
								
				// First, Add the table record:
				DataRow row = _metaViews.NewRow();
				row["TABLE_NAME"] = viewName;
				row["TABLE_TYPE"] = "VIEW";
				row["IS_UPDATABLE"] = false;
				row["VIEW_DEFINITION"] = rawSQL;
				_metaViews.Rows.Add(row);

				int count = 0;
				foreach (DataRow pragmaRow in pragmaTable.Rows) 
				{
					row = metaViewColumns.NewRow();
					
					row["TABLE_NAME"] = viewName;
					row["COLUMN_NAME"] = pragmaRow["name"];
					row["COLUMN_PROPID"] = Convert.ToInt32(pragmaRow["cid"]);
					row["IS_NULLABLE"] = (pragmaRow["notnull"].ToString() == "0");
					row["ORDINAL_POSITION"] = ++count;
					row["COLUMN_HASDEFAULT"] = (pragmaRow["dflt_value"] != DBNull.Value);		
					row["COLUMN_DEFAULT"] = pragmaRow["dflt_value"];				
					row[COLUMN_IS_KEY] = false;
						
					metaViewColumns.Rows.Add(row);
					rowMap.Add(pragmaRow["name"], row);
				}

				foreach (DataRow schemaRow in schemaTable.Rows) 
				{
					row = rowMap[ schemaRow["ColumnName"] ] as DataRow;
					if (row != null) 
					{	
						//This is currently being converted backwards? ARGH!
						fullname = LousyBackwardsTypeConvert(schemaRow["DataType"].ToString());
						this.ParseDataType(fullname, out name, out precision, out scale);

						row[FULL_DATA_TYPE] = fullname;
						row[DATA_TYPE_NAME] = name;
						if (scale != Int32.MinValue) 
						{
							row["NUMERIC_PRECISION"] = precision;
							row["NUMERIC_SCALE"] = scale;
						}
						else if (precision != Int32.MinValue) 
						{
							row["CHARACTER_MAXIMUM_LENGTH"] = precision;
						}
					}
				}

				this._viewColumns.Add(viewName, metaViewColumns);
			}

			_metaIndexes = null;
			string lastTableName = string.Empty;
			string tblName = string.Empty, indexName = string.Empty;
			foreach (DataRow data in tempIndexTable.Rows) 
			{
				indexName = data["INDEX_NAME"].ToString();
				tblName = data["TABLE_NAME"].ToString();

				if (tblName != lastTableName) 
				{
					if (_metaIndexes != null) 
					{
						this._indexColumns[lastTableName] = _metaIndexes;
					}
					_metaIndexes = CreateEmptyMeta_Indexes();
				}

				this.FillMetaTables(cn, indexName, out pragmaTable);

				foreach (DataRow pragmaRow in pragmaTable.Rows) 
				{
					DataRow row = _metaIndexes.NewRow();
					
					row["TABLE_NAME"] = tblName;
					row["INDEX_NAME"] = indexName;
					row["UNIQUE"] = data["UNIQUE"];
					row["COLUMN_NAME"] = pragmaRow["name"];
					
					_metaIndexes.Rows.Add(row);
				}

				lastTableName = tblName;
			}
			if (_metaIndexes != null) 
			{
				this._indexColumns[tblName] = _metaIndexes;
			}
			
			foreach (string[] data in this._tables)
			{
				string tableName = data[0];
				_metaFKs = new DataTable();

				if (_fkColumns.Contains(tableName)) 
				{
					_metaFKs = (DataTable)_fkColumns[tableName]; 
				}
				else 
				{
					_metaFKs = CreateEmptyMeta_FKs();
					_fkColumns[tableName] = _metaFKs;
				}
								
				//id, seq, table, from, to,
				this.FillFKMetaTables(cn, tableName, out pragmaTable);
				foreach (DataRow pragmaRow in pragmaTable.Rows) 
				{
					string fkTable = pragmaRow["table"].ToString();
					string fkName = tableName + "_" + fkTable + ( (pragmaRow["id"].ToString() == "0") ? string.Empty : (pragmaRow["id"]) );

					DataRow row = _metaFKs.NewRow();
					row["FK_NAME"] = fkName;
					row["PK_TABLE_NAME"] = fkTable;
					row["FK_TABLE_NAME"] = tableName;
					row["PK_COLUMN_NAME"] = pragmaRow["to"];
					row["FK_COLUMN_NAME"] = pragmaRow["from"];
					row["ORDINAL"] = pragmaRow["seq"];
					_metaFKs.Rows.Add(row);

					object[] rowitems = (object[])row.ItemArray.Clone();
										
					DataTable tmpMetaFKs;
					if (_fkColumns.Contains(fkTable)) 
					{
						tmpMetaFKs = (DataTable)_fkColumns[fkTable]; 
					}
					else 
					{
						tmpMetaFKs = CreateEmptyMeta_FKs();
						_fkColumns[fkTable] = tmpMetaFKs;
					}
					DataRow tmpRow = tmpMetaFKs.NewRow();
					tmpRow.ItemArray = rowitems;
					tmpMetaFKs.Rows.Add(tmpRow);
				}
			}
		}

		public DataTable LoadIndexColumns(string tableName) 
		{
			return this._indexColumns[tableName] as DataTable;
		}

		public DataTable LoadTableColumns(string tableName) 
		{
			return this._tableColumns[tableName] as DataTable;
		}

		public DataTable LoadViewColumns(string viewName) 
		{
			return this._viewColumns[viewName] as DataTable;
		}

		public DataTable LoadFKColumns(string tableName) 
		{
			return this._fkColumns[tableName] as DataTable;
		}

		public DataTable Tables 
		{
			get { return _metaTables; }
		}

		public DataTable Views 
		{
			get { return _metaViews; }
		}

        private DataTable FillPragmaTable(SQLiteConnection cn, string sql)
        {
            DataTable pragma = new DataTable();

            SQLiteCommand command = cn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql;

            SQLiteDataReader reader;
            using (reader = command.ExecuteReader() as SQLiteDataReader)
            {
                int rowindex = 0;
                while (reader.Read())
                {
                    DataRow row = pragma.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        object o = reader.GetValue(i);
                        if (rowindex == 0)
                        {
                            string field = reader.GetName(i);
                            string dtype = reader.GetDataTypeName(i);
                            if (o != DBNull.Value)
                            {
                                pragma.Columns.Add(field, o.GetType());
                            }
                            else
                            {
                                pragma.Columns.Add(field);
                            }
                        }
                        row[i] = o;
                    }
                    pragma.Rows.Add(row);
                    rowindex++;
                }
            }
            return pragma;
        }

		private void FillFKMetaTables(SQLiteConnection cn, string tableName, out DataTable pragma) 
		{
			pragma = FillPragmaTable(cn, "PRAGMA foreign_key_list('" + tableName + "');");
		}

		private void FillMetaTables(SQLiteConnection cn, string indexName, out DataTable pragma)
        {
            pragma = FillPragmaTable(cn, "PRAGMA index_info('" + indexName + "');");
		}

		private void FillMetaTables(SQLiteConnection cn, string tableOrView, out DataTable schema, out DataTable pragma) 
		{
			SQLiteCommand command = cn.CreateCommand();
			SQLiteDataAdapter adapter = new SQLiteDataAdapter();
			SQLiteDataReader reader;

			schema = new DataTable();
			command = cn.CreateCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = "SELECT * FROM [" + tableOrView + "];";
			reader = command.ExecuteReader() as SQLiteDataReader;
			schema = reader.GetSchemaTable();
			reader.Close();

            pragma = FillPragmaTable(cn, "PRAGMA table_info('" + tableOrView + "');");
		}

		private void CheckForIndexes(SQLiteConnection cn, string tableName, DataTable tempIndexTable) 
		{
			SQLiteCommand command = cn.CreateCommand();
			SQLiteDataAdapter adapter = new SQLiteDataAdapter();

			DataTable tmp = new DataTable();
			command = cn.CreateCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = "PRAGMA index_list('" + tableName + "');";
			adapter.SelectCommand = command;
			adapter.Fill(tmp);

			//tempIndexTable
			foreach (DataRow row in tmp.Rows) 
			{
				DataRow newRow = tempIndexTable.NewRow();

				newRow["TABLE_NAME"] = tableName;
				newRow["INDEX_NAME"] = row["name"];
				newRow["UNIQUE"] = row["unique"];

				tempIndexTable.Rows.Add(newRow);
			}
		}

		private DataTable CreateTempIndexTable() 
		{
			DataTable dt = new DataTable();

			dt.Columns.Add("TABLE_NAME");
			dt.Columns.Add("INDEX_NAME");
			dt.Columns.Add("UNIQUE");
			
			return dt;
		}

		private DataTable CreateEmptyMeta_Indexes()
		{
			DataTable dt = new DataTable();

			dt.Columns.Add("COLUMN_NAME");			
			dt.Columns.Add("INDEX_NAME");
			dt.Columns.Add("TABLE_NAME");
			dt.Columns.Add("UNIQUE");
						
			return dt;
		}

		private DataTable CreateEmptyMeta_FKs()
		{
			DataTable dt = new DataTable();

			dt.Columns.Add("PK_TABLE_CATALOG");			
			dt.Columns.Add("PK_TABLE_SCHEMA");
			dt.Columns.Add("PK_TABLE_NAME");			
			dt.Columns.Add("FK_TABLE_CATALOG");			
			dt.Columns.Add("FK_TABLE_SCHEMA");
			dt.Columns.Add("FK_TABLE_NAME");
			dt.Columns.Add("PK_NAME");
			dt.Columns.Add("FK_NAME");
			dt.Columns.Add("PK_COLUMN_NAME");
			dt.Columns.Add("FK_COLUMN_NAME");
			dt.Columns.Add("ORDINAL");
						
			return dt;
		}

		private DataTable CreateEmptyMeta_Entities(bool isView)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("TABLE_NAME");
			dt.Columns.Add("TABLE_TYPE");
			dt.Columns.Add("IS_UPDATABLE");
			dt.Columns.Add("TABLE_PROPID");

			if (isView) 
			{
				dt.Columns.Add("VIEW_DEFINITION");
			}
			else 
			{
				dt.Columns.Add("TABLE_DEFINITION");
			}

			return dt;
		}

		private DataTable CreateEmptyMeta_Columns(bool isView)
		{
			DataTable dt = new DataTable();

			dt.Columns.Add("TABLE_NAME");					
			dt.Columns.Add("COLUMN_NAME");			
			dt.Columns.Add("COLUMN_PROPID");			
			dt.Columns.Add("ORDINAL_POSITION");		
			dt.Columns.Add("COLUMN_HASDEFAULT");			
			dt.Columns.Add("COLUMN_DEFAULT");			
			dt.Columns.Add("IS_NULLABLE");
			dt.Columns.Add("CHARACTER_MAXIMUM_LENGTH");	
			dt.Columns.Add("NUMERIC_PRECISION");			
			dt.Columns.Add("NUMERIC_SCALE");

			dt.Columns.Add(COLUMN_IS_KEY);
			dt.Columns.Add(FULL_DATA_TYPE);
			dt.Columns.Add(DATA_TYPE_NAME);

			return dt;
		}

		private string LousyBackwardsTypeConvert(string systemType) 
		{
			string type = "TEXT";
			switch (systemType) 
			{
				case "System.Int32":
				case "System.Int64":
					type = "INTEGER";
					break;
				case "System.DateTime":
					type = "BLOB";
					break;
				case "System.Byte[]":
					type = "TIMESTAMP";
					break;
				case "System.Single":
					type = "INTEGER";
					break;
				case "System.Boolean":
					type = "BOOLEAN";
					break;
				case "System.Decimal":
					type = "NUMERIC(10,5)";
					break;
			}
			return type;
		}

		private void ParseDataType(string datatype, out string newtype, out int precision, out int scale)
		{
			precision = Int32.MinValue;
			scale = Int32.MinValue;

			int x = datatype.IndexOf("("), 
				y = datatype.IndexOf(")");
			if (x == -1 || y == -1) 
			{
				newtype = datatype;
			}
			else //test(12, 3); 4-10
			{
				string[] tmpNumbers = datatype.Substring(x+1, y-x-1).Split(',');
				precision = Int32.Parse(tmpNumbers[0].Trim());
				
				if (tmpNumbers.Length == 2) 
				{
					scale = Int32.Parse(tmpNumbers[1].Trim());
				}

				newtype = datatype.Substring(0, x);
			}
		}
	}
}