using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace OMeta.Firebird
{
 
	public class FirebirdDomains : Domains
	{
		public FirebirdDomains()
		{

		}

		internal DataColumn f_TypeNameComplete	= null;

		override internal void LoadAll()
		{
			try
			{
				FbConnection cn = new FirebirdSql.Data.FirebirdClient.FbConnection(this._dbRoot.ConnectionString);
				cn.Open();
				DataTable metaData = cn.GetSchema("Domains", null);
				cn.Close();

				if(metaData.Columns.Contains("DOMAIN_DATA_TYPE"))
				{
					metaData.Columns["DOMAIN_DATA_TYPE"].ColumnName = "DATA_TYPE";
				}

				PopulateArray(metaData);
				
				LoadExtraData(cn);
			}
			catch(Exception ex)
			{
                Console.WriteLine(ex.StackTrace);
			}
		}

		private void LoadExtraData(FbConnection cn)
		{
			try
			{
				int dialect = 1;
				try
				{
					FbConnectionStringBuilder cnString = new FbConnectionStringBuilder(cn.ConnectionString);
					dialect = cnString.Dialect;
				}
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }

				string select = "select f.rdb$field_name, f.rdb$field_scale AS SCALE, f.rdb$field_type as FTYPE, f.rdb$field_sub_type AS SUBTYPE, f.rdb$dimensions AS DIM from rdb$fields f, rdb$types t where t.rdb$field_name='RDB$FIELD_TYPE' and f.rdb$field_type=t.rdb$type and not f.rdb$field_name starting with 'RDB$' ORDER BY f.rdb$field_name;";

				// Column Data
				FbDataAdapter adapter = new FbDataAdapter(select, cn);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);

				// Dimension Data
				string dimSelect = "select r.rdb$field_name AS Name, d.rdb$dimension as DIM, d.rdb$lower_bound as L, d.rdb$upper_bound as U from rdb$fields f, rdb$field_dimensions d, rdb$relation_fields r where f.rdb$field_name = d.rdb$field_name and f.rdb$field_name=r.rdb$field_source and not f.rdb$field_name starting with 'RDB$' order by d.rdb$dimension;";

				FbDataAdapter dimAdapter = new FbDataAdapter(dimSelect, cn);
				DataTable dimTable = new DataTable();
				dimAdapter.Fill(dimTable);

				if(this._array.Count > 0)
				{
					Domain dom = this._array[0] as Domain;

					f_TypeNameComplete = new DataColumn("TYPE_NAME_COMPLETE", typeof(string));
					dom._row.Table.Columns.Add(f_TypeNameComplete);

					short ftype = 0;
					short dim = 0;
					DataRowCollection rows = dataTable.Rows;

					int count = this._array.Count;
					Domain d = null;

					for( int index = 0; index < count; index++)
					{
						d = (Domain)this[index];

						if(d.DataTypeName == "bigint")
						{
							d._row["DATA_TYPE"] = "BIGINT";
							d._row["TYPE_NAME_COMPLETE"] = "BIGINT";
							continue;
						}

						// Step 1: DataTypeName
						ftype = (short)rows[index]["FTYPE"];

						switch(ftype)
						{
							case 7:
								d._row["DATA_TYPE"] = "SMALLINT";
								break;

							case 8:
								d._row["DATA_TYPE"] = "INTEGER";
								break;

							case 9:
								d._row["DATA_TYPE"] = "QUAD";
								break;

							case 10:
								d._row["DATA_TYPE"] = "FLOAT";
								break;

							case 11:
								d._row["DATA_TYPE"] = "DOUBLE PRECISION";
								break;

							case 12:
								d._row["DATA_TYPE"] = "DATE";
								break;

							case 13:
								d._row["DATA_TYPE"] = "TIME";
								break;

							case 14:
								d._row["DATA_TYPE"] = "CHAR";
								break;

							case 16:
								d._row["DATA_TYPE"] = "NUMERIC";
								break;

							case 27:
								d._row["DATA_TYPE"] = "DOUBLE PRECISION";
								break;

//							case 35:
//								d._row["DATA_TYPE"] = "DATE";
//								break;

							case 35:

								if(dialect > 2)
								{
									d._row["DATA_TYPE"] = "TIMESTAMP";
								}
								else
								{
									d._row["DATA_TYPE"] = "DATE";
								}
								break;

							case 37:
								d._row["DATA_TYPE"] = "VARCHAR";
								break;

							case 40:
								d._row["DATA_TYPE"] = "CSTRING";
								break;

							case 261:
								short subtype = (short)rows[index]["SUBTYPE"];
								
							switch(subtype)
							{
								case 0:
									d._row["DATA_TYPE"] = "BLOB(BINARY)";
									break;
								case 1:
									d._row["DATA_TYPE"] = "BLOB(TEXT)";
									break;
								default:
									d._row["DATA_TYPE"] = "BLOB(UNKNOWN)";
									break;
							}
								break;
						}

						short scale = (short)rows[index]["SCALE"];
						if(scale < 0)
						{
							d._row["DATA_TYPE"]     = "NUMERIC";
							d._row["NUMERIC_SCALE"] = Math.Abs(scale);
						}

						object o = null;

						// Step 2: DataTypeNameComplete
						string s = d._row["DATA_TYPE"] as string;
						switch(s)
						{
							case "VARCHAR":
							case "CHAR":
								d._row["TYPE_NAME_COMPLETE"] = s + "(" + d.CharacterMaxLength + ")";
								break;

							case "NUMERIC":

							switch((int)d._row["DOMAIN_SIZE"])
							{
								case 2:
									d._row["TYPE_NAME_COMPLETE"] = s + "(4, " + d.NumericScale.ToString() + ")";
									break;
								case 4:
									d._row["TYPE_NAME_COMPLETE"] = s + "(9, " + d.NumericScale.ToString() + ")";
									break;
								case 8:
									d._row["TYPE_NAME_COMPLETE"] = s + "(15, " + d.NumericScale.ToString() + ")";
									break;
								default:
									d._row["TYPE_NAME_COMPLETE"] = "NUMERIC(18,0)";
									break;
							}
								break;

							case "BLOB(TEXT)":
							case "BLOB(BINARY)":
								d._row["TYPE_NAME_COMPLETE"] = "BLOB";
								break;

							default:
								d._row["TYPE_NAME_COMPLETE"] = s;
								break;
						}

						s = d._row["TYPE_NAME_COMPLETE"] as string;

						dim = 0;
						o = rows[index]["DIM"];
						if(o != DBNull.Value)
						{
							dim = (short)o;
						}

						if(dim > 0)
						{
							dimTable.DefaultView.RowFilter = "Name = '" + d.Name + "'";
							dimTable.DefaultView.Sort = "DIM";

							string a = "[";
							bool bFirst = true;

							foreach(DataRowView vrow in dimTable.DefaultView)
							{
								DataRow row = vrow.Row;

								if(!bFirst)	a += ",";

								a += row["L"].ToString() + ":" + row["U"].ToString();

								bFirst = false;
							}

							a += "]";

							d._row["TYPE_NAME_COMPLETE"] = s + a;

							d._row["DATA_TYPE"] = d._row["DATA_TYPE"] + ":A";
						}
					}
				}
			}
			catch(Exception ex)
			{
                Console.WriteLine(ex.StackTrace);
			}
		}
	}
}
