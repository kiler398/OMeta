using System;
using System.Text;
using System.Data;

using FirebirdSql.Data.FirebirdClient;

namespace OMeta.Firebird
{
 
	public class FirebirdParameters : Parameters
	{
		internal DataColumn f_TypeNameComplete	= null;

		public FirebirdParameters()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				FbConnection cn = new FirebirdSql.Data.FirebirdClient.FbConnection(this._dbRoot.ConnectionString);

				FbDataAdapter adapter = new FbDataAdapter(BuildSql(this.Procedure.Name), cn);
				DataTable metaData = new DataTable();
				adapter.Fill(metaData);

				metaData.Columns["PARAMETER_DIRECTION"].ColumnName = "PARAMETER_TYPE";

				PopulateArray(metaData);
				
				FixupDataTypes(metaData);
			}
			catch(Exception ex)
			{
				string m = ex.Message;
			}
		}

		private void FixupDataTypes(DataTable metaData)
		{
			FbConnection cn = null;

			try
			{
				int dialect = 1;

				cn = new FirebirdSql.Data.FirebirdClient.FbConnection(this._dbRoot.ConnectionString);
				cn.Open();

				try
				{

					FbConnectionStringBuilder cnString = new FbConnectionStringBuilder(cn.ConnectionString);
					dialect = cnString.Dialect;
				}
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }

				int count = this._array.Count;
				Parameter p = null;

				if(count > 0)
				{
					// Dimension Data
					string dimSelect = "select r.rdb$field_name AS Name , d.rdb$dimension as DIM, d.rdb$lower_bound as L, d.rdb$upper_bound as U from rdb$fields f, rdb$field_dimensions d, rdb$relation_fields r where r.rdb$relation_name='" + this.Procedure.Name + "' and f.rdb$field_name = d.rdb$field_name and f.rdb$field_name=r.rdb$field_source order by d.rdb$dimension;";

					FbDataAdapter dimAdapter = new FbDataAdapter(dimSelect, cn);
					DataTable dimTable = new DataTable();
					dimAdapter.Fill(dimTable);

					p = this._array[0] as Parameter;

					f_TypeName = new DataColumn("TYPE_NAME", typeof(string));
					p._row.Table.Columns.Add(f_TypeName);

					f_TypeNameComplete = new DataColumn("TYPE_NAME_COMPLETE", typeof(string));
					p._row.Table.Columns.Add(f_TypeNameComplete);

					short ftype = 0;
					short dim = 0;
					DataRowCollection rows = metaData.Rows;

					for( int index = 0; index < count; index++)
					{
						p = (Parameter)this[index];

						if(p._row["NUMERIC_PRECISION"] == System.DBNull.Value)
						{
							p._row["NUMERIC_PRECISION"] = p._row["PARAMETER_SIZE"];
						}

						p._row["PARAMETER_NAME"] = (p._row["PARAMETER_NAME"] as String).Trim();

						int dir = (int)p._row["PARAMETER_TYPE"];


						p._row["PARAMETER_TYPE"] = dir == 0 ? 1 : 3;


						// Step 1: DataTypeName
						ftype = (short)rows[index]["FIELD_TYPE"];

						switch(ftype)
						{
							case 7:
								p._row["TYPE_NAME"] = "SMALLINT";
								break;

							case 8:
								p._row["TYPE_NAME"] = "INTEGER";
								break;

							case 9:
								p._row["TYPE_NAME"] = "QUAD";
								break;

							case 10:
								p._row["TYPE_NAME"] = "FLOAT";
								break;

							case 11:
								p._row["TYPE_NAME"] = "DOUBLE PRECISION";
								break;

							case 12:
								p._row["TYPE_NAME"] = "DATE";
								break;

							case 13:
								p._row["TYPE_NAME"] = "TIME";
								break;

							case 14:
								p._row["TYPE_NAME"] = "CHAR";
								break;

							case 16:
								if(p.NumericScale < 0)
									p._row["TYPE_NAME"] = "NUMERIC";
								else
									p._row["TYPE_NAME"] = "BIGINT";
								break;

							case 27:
								p._row["TYPE_NAME"] = "DOUBLE PRECISION";
								break;

							case 35:

								if(dialect > 2)
								{
									p._row["TYPE_NAME"] = "TIMESTAMP";
								}
								else
								{
									p._row["TYPE_NAME"] = "DATE";
								}
								break;

							case 37:
								p._row["TYPE_NAME"] = "VARCHAR";
								break;

							case 40:
								p._row["TYPE_NAME"] = "CSTRING";
								break;

							case 261:
								short subtype = (short)rows[index]["PARAMETER_SUB_TYPE"];
								
								switch(subtype)
								{
									case 0:
										p._row["TYPE_NAME"] = "BLOB(BINARY)";
										break;
									case 1:
										p._row["TYPE_NAME"] = "BLOB(TEXT)";
										break;
									default:
										p._row["TYPE_NAME"] = "BLOB(UNKNOWN)";
										break;
								}
								break;
						}

						int scale = p.NumericScale;
						if(scale < 0)
						{
							p._row["TYPE_NAME"]     = "NUMERIC";
							p._row["NUMERIC_SCALE"] = Math.Abs(scale);
						}


						// Step 2: DataTypeNameComplete
						string s = p._row["TYPE_NAME"] as string;
						switch(s)
						{
							case "VARCHAR":
							case "CHAR":
								p._row["TYPE_NAME_COMPLETE"] = s + "(" + p.CharacterMaxLength + ")";
								break;

							case "NUMERIC":

							switch((int)p._row["PARAMETER_SIZE"])
							{
								case 2:
									p._row["TYPE_NAME_COMPLETE"] = s + "(4, " + p.NumericScale.ToString() + ")";
									break;
								case 4:
									p._row["TYPE_NAME_COMPLETE"] = s + "(9, " + p.NumericScale.ToString() + ")";
									break;
								case 8:
									p._row["TYPE_NAME_COMPLETE"] = s + "(15, " + p.NumericScale.ToString() + ")";
									break;
								default:
									p._row["TYPE_NAME_COMPLETE"] = "NUMERIC(18,0)";
									break;
							}
								break;

							case "BLOB(TEXT)":
							case "BLOB(BINARY)":
								p._row["TYPE_NAME_COMPLETE"] = "BLOB";
								break;

							default:
								p._row["TYPE_NAME_COMPLETE"] = s;
								break;
						}

						s = p._row["TYPE_NAME_COMPLETE"] as string;

						dim = 0;
						object o = rows[index]["DIM"];
						if(o != DBNull.Value)
						{
							dim = (short)o;
						}

						if(dim > 0)
						{
							dimTable.DefaultView.RowFilter = "Name = '" + p.Name + "'";
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

							p._row["TYPE_NAME_COMPLETE"] = s + a;

							p._row["TYPE_NAME"] = p._row["TYPE_NAME"] + ":A";
						}
					}
				}
			}
			catch(Exception ex)
			{
				string e = ex.Message;
			}

			if(cn != null)
			{
				cn.Close();
			}
		}

		private string BuildSql(string procedureName)
		{
			StringBuilder sql = new StringBuilder();

			sql.Append(
				@"SELECT " +
				"pp.rdb$procedure_name AS PROCEDURE_NAME, " +
				"pp.rdb$parameter_name AS PARAMETER_NAME, " +
				"fld.rdb$field_sub_type AS PARAMETER_SUB_TYPE, " +
				"pp.rdb$parameter_number AS ORDINAL_POSITION, " +
				"cast(pp.rdb$parameter_type AS integer) AS PARAMETER_DIRECTION, " +
				"cast(fld.rdb$field_length AS integer) AS PARAMETER_SIZE, " +
				"cast(fld.rdb$field_precision AS integer) AS NUMERIC_PRECISION, " +
				"cast(fld.rdb$field_scale AS integer) AS NUMERIC_SCALE, " +
				"cs.rdb$character_set_name AS CHARACTER_SET_NAME, " +
				"coll.rdb$collation_name AS COLLATION_NAME, " +
				"pp.rdb$description AS DESCRIPTION, " +
				"fld.rdb$field_type AS FIELD_TYPE, " +
				"d.rdb$dimension AS DIM " +
				"FROM " +
				"rdb$procedure_parameters pp " +
				"left join rdb$fields fld ON pp.rdb$field_source = fld.rdb$field_name " +
				"left join rdb$field_dimensions d ON fld.rdb$field_name = d.rdb$field_name " +
				"left join rdb$character_sets cs ON cs.rdb$character_set_id = fld.rdb$character_set_id " +
				"left join rdb$collations coll ON (coll.rdb$collation_id = fld.rdb$collation_id AND coll.rdb$character_set_id = fld.rdb$character_set_id) " +
				"WHERE pp.rdb$procedure_name = '" + procedureName + "'" +
				" ORDER BY pp.rdb$procedure_name, pp.rdb$parameter_type, pp.rdb$parameter_number");

			return sql.ToString();
		}
	}
}
