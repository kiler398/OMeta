using System;
using System.Collections.Generic;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace OMeta.Firebird
{
 
	public class FirebirdColumns : Columns
	{
		public FirebirdColumns()
		{

		}

		internal DataColumn f_TypeName			= null;
		internal DataColumn f_TypeNameComplete	= null;

		override internal void LoadForTable()
		{
			try
			{
				FbConnection cn = new FirebirdSql.Data.FirebirdClient.FbConnection(this._dbRoot.ConnectionString);
				cn.Open();
                DataTable metaData = cn.GetSchema("Columns", new string[] { null, null, this.Table.Name });

                DataColumn c;
                if (!metaData.Columns.Contains("IS_AUTO_KEY")) { c = metaData.Columns.Add("IS_AUTO_KEY", typeof(Boolean)); c.DefaultValue = false; }
                if (!metaData.Columns.Contains("AUTO_KEY_SEED")) { c = metaData.Columns.Add("AUTO_KEY_SEED"); c.DefaultValue = 0; }
                if (!metaData.Columns.Contains("AUTO_KEY_INCREMENT")) { c = metaData.Columns.Add("AUTO_KEY_INCREMENT"); c.DefaultValue = 0; }
                if (!metaData.Columns.Contains("AUTO_KEY_SEQUENCE")) { c = metaData.Columns.Add("AUTO_KEY_SEQUENCE"); c.DefaultValue = string.Empty; }

				PopulateArray(metaData);
				LoadExtraData(cn, this.Table.Name, "T");
				cn.Close();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
            }
		}

		override internal void LoadForView()
		{
			try
			{
				FbConnection cn = new FirebirdSql.Data.FirebirdClient.FbConnection(this._dbRoot.ConnectionString);
				cn.Open();

				DataTable metaData = cn.GetSchema("Columns", new string[] {null, null, this.View.Name});

                PopulateArray(metaData);
				LoadExtraData(cn, this.View.Name, "V");
				cn.Close();				
			}
			catch(Exception ex)
			{
                Console.WriteLine(ex.StackTrace);
			}
		}

		private void LoadExtraData(FbConnection cn, string name, string type)
		{
			try
			{
				int dialect = 1;
				object o = null;
				short scale = 0;

				try
				{
					FbConnectionStringBuilder cnString = new FbConnectionStringBuilder(cn.ConnectionString);
					dialect = cnString.Dialect;
				}
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }

				// AutoKey Data
				Dictionary<string, object[]> autoKeyFields = new Dictionary<string, object[]>();
                DataTable triggers = cn.GetSchema("Triggers", new string[] {null, null, name});
                foreach (DataRow row in triggers.Rows)
                {
                    int isSystemTrigger = Convert.ToInt32(row["IS_SYSTEM_TRIGGER"]);
                    int isInactive = Convert.ToInt32(row["IS_INACTIVE"]);
                    int triggerType = Convert.ToInt32(row["TRIGGER_TYPE"]);

                    if ((isSystemTrigger == 0) && (isInactive == 0) && (triggerType == 1))
                    {
                        string source = row["SOURCE"].ToString();
                        int end = 0;
                        do
                        {
                            string field = null, generatorName = string.Empty;
                            int tmp, increment = 1, seed = 0;

                            end = source.IndexOf("gen_id(", end, StringComparison.CurrentCultureIgnoreCase);
                            if (end >= 0)
                            {
                                string s = source.Substring(0, end);
                                int start = s.LastIndexOf(".");
                                if (start >= 0 && start < end)
                                {
                                    field = s.Substring(start).Trim(' ', '.', '=').ToUpper();
                                }

                                int end2 = source.IndexOf(")", end);
                                string s2 = source.Substring(0, end2);
                                int start2 = s2.LastIndexOf(",");
                                if (start2 >= 0 && start2 < end2)
                                {
                                    if (int.TryParse(s2.Substring(start2 + 1).Trim(' ', ','), out tmp))
                                        increment = tmp;
                                    
                                    s2 = s2.Substring(0, start2);
                                    generatorName = s2.Substring(end + 7).Trim();
                                }


                                if (field != null)
                                {
                                    autoKeyFields[field] = new object[] { increment, seed, generatorName };
                                }

                                end += 7;
                            }
                        } while (end != -1);
                    }
                }

                string select = "select r.rdb$field_name, f.rdb$field_scale AS SCALE, f.rdb$computed_source AS IsComputed, f.rdb$field_type as FTYPE, f.rdb$field_sub_type AS SUBTYPE, f.rdb$dimensions AS DIM from rdb$relation_fields r, rdb$types t, rdb$fields f where r.rdb$relation_name='" + name + "' and f.rdb$field_name=r.rdb$field_source and t.rdb$field_name='RDB$FIELD_TYPE' and f.rdb$field_type=t.rdb$type order by r.rdb$field_position;";

                // Column Data
				FbDataAdapter adapter = new FbDataAdapter(select, cn);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);

				// Dimension Data
				string dimSelect = "select r.rdb$field_name AS Name , d.rdb$dimension as DIM, d.rdb$lower_bound as L, d.rdb$upper_bound as U from rdb$fields f, rdb$field_dimensions d, rdb$relation_fields r where r.rdb$relation_name='" + name + "' and f.rdb$field_name = d.rdb$field_name and f.rdb$field_name=r.rdb$field_source order by d.rdb$dimension;";

				FbDataAdapter dimAdapter = new FbDataAdapter(dimSelect, cn);
				DataTable dimTable = new DataTable();
				dimAdapter.Fill(dimTable);

				if(this._array.Count > 0)
				{
					Column col = this._array[0] as Column;

					f_TypeName = new DataColumn("TYPE_NAME", typeof(string));
					col._row.Table.Columns.Add(f_TypeName);

					f_TypeNameComplete = new DataColumn("TYPE_NAME_COMPLETE", typeof(string));
					col._row.Table.Columns.Add(f_TypeNameComplete);

					this.f_IsComputed = new DataColumn("IS_COMPUTED", typeof(bool));
					col._row.Table.Columns.Add(f_IsComputed);

					short ftype = 0;
					short dim = 0;
					DataRowCollection rows = dataTable.Rows;

					int count = this._array.Count;
					Column c = null;

					for( int index = 0; index < count; index++)
					{
						c = (Column)this[index];
                        if (autoKeyFields.ContainsKey(c.Name.ToUpper()))
                        {
                            c._row["IS_AUTO_KEY"] = true;
                            c._row["AUTO_KEY_INCREMENT"] = autoKeyFields[c.Name][0];
                            c._row["AUTO_KEY_SEED"] = autoKeyFields[c.Name][1];
                            c._row["AUTO_KEY_SEQUENCE"] = autoKeyFields[c.Name][2];
                        }
						if(!c._row.IsNull("DOMAIN_NAME"))
						{
							// Special Hack, if there is a domain 
							c._row["TYPE_NAME"]			 = c._row["DOMAIN_NAME"] as string;
							c._row["TYPE_NAME_COMPLETE"] = c._row["DOMAIN_NAME"] as string;

							o = rows[index]["IsComputed"];
							if(o != DBNull.Value)
							{
								c._row["IS_COMPUTED"] = true;
							}

							scale = (short)rows[index]["SCALE"];
							if(scale < 0)
							{
								c._row["NUMERIC_SCALE"] = Math.Abs(scale);
							}
							continue;
						}

						try
						{
							string bigint = c._row["COLUMN_DATA_TYPE"] as string;
							if(bigint.ToUpper() == "BIGINT")
							{
								c._row["TYPE_NAME"]			 = "BIGINT";
								c._row["TYPE_NAME_COMPLETE"] = "BIGINT";
								continue;
							}
						}
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                        }

						//		public const int blr_text		= 14;
						//		public const int blr_text2		= 15;
						//		public const int blr_short		= 7;
						//		public const int blr_long		= 8;
						//		public const int blr_quad		= 9;
						//		public const int blr_int64		= 16;
						//		public const int blr_float		= 10;
						//		public const int blr_double		= 27;
						//		public const int blr_d_float	= 11;
						//		public const int blr_timestamp	= 35;
						//		public const int blr_varying	= 37;
						//		public const int blr_varying2	= 38;
						//		public const int blr_blob		= 261;
						//		public const int blr_cstring	= 40;
						//		public const int blr_cstring2	= 41;
						//		public const int blr_blob_id	= 45;
						//		public const int blr_sql_date	= 12;
						//		public const int blr_sql_time	= 13;

						// Step 1: DataTypeName
						ftype = (short)rows[index]["FTYPE"];

						switch(ftype)
						{
							case 7:
								c._row["TYPE_NAME"] = "SMALLINT";
								break;

							case 8:
								c._row["TYPE_NAME"] = "INTEGER";
								break;

							case 9:
								c._row["TYPE_NAME"] = "QUAD";
								break;

							case 10:
								c._row["TYPE_NAME"] = "FLOAT";
								break;

							case 11:
								c._row["TYPE_NAME"] = "DOUBLE PRECISION";
								break;

							case 12:
								c._row["TYPE_NAME"] = "DATE";
								break;

							case 13:
								c._row["TYPE_NAME"] = "TIME";
								break;

							case 14:
								c._row["TYPE_NAME"] = "CHAR";
								break;

							case 16:
								c._row["TYPE_NAME"] = "NUMERIC";
								break;

							case 27:
								c._row["TYPE_NAME"] = "DOUBLE PRECISION";
								break;

							case 35:

								if(dialect > 2)
								{
									c._row["TYPE_NAME"] = "TIMESTAMP";
								}
								else
								{
									c._row["TYPE_NAME"] = "DATE";
								}
								break;

							case 37:
								c._row["TYPE_NAME"] = "VARCHAR";
								break;

							case 40:
								c._row["TYPE_NAME"] = "CSTRING";
								break;

							case 261:
								short subtype = (short)rows[index]["SUBTYPE"];
								
								switch(subtype)
								{
									case 0:
										c._row["TYPE_NAME"] = "BLOB(BINARY)";
										break;
									case 1:
										c._row["TYPE_NAME"] = "BLOB(TEXT)";
										break;
									default:
										c._row["TYPE_NAME"] = "BLOB(UNKNOWN)";
										break;
								}
								break;
						}

						scale = (short)rows[index]["SCALE"];
						if(scale < 0)
						{
							c._row["TYPE_NAME"]     = "NUMERIC";
							c._row["NUMERIC_SCALE"] = Math.Abs(scale);
						}

						o = rows[index]["IsComputed"];
						if(o != DBNull.Value)
						{
							c._row["IS_COMPUTED"] = true;
						}

						// Step 2: DataTypeNameComplete
						string s = c._row["TYPE_NAME"] as string;
						switch(s)
						{
							case "VARCHAR":
							case "CHAR":
								c._row["TYPE_NAME_COMPLETE"] = s + "(" + c.CharacterMaxLength + ")";
								break;

							case "NUMERIC":

							switch((int)c._row["COLUMN_SIZE"])
							{
								case 2:
									c._row["TYPE_NAME_COMPLETE"] = s + "(4, " + c.NumericScale.ToString() + ")";
									break;
								case 4:
									c._row["TYPE_NAME_COMPLETE"] = s + "(9, " + c.NumericScale.ToString() + ")";
									break;
								case 8:
									c._row["TYPE_NAME_COMPLETE"] = s + "(15, " + c.NumericScale.ToString() + ")";
									break;
								default:
									c._row["TYPE_NAME_COMPLETE"] = "NUMERIC(18,0)";
									break;
							}
								break;

							case "BLOB(TEXT)":
							case "BLOB(BINARY)":
								c._row["TYPE_NAME_COMPLETE"] = "BLOB";
								break;

							default:
								c._row["TYPE_NAME_COMPLETE"] = s;
								break;
						}

						s = c._row["TYPE_NAME_COMPLETE"] as string;

						dim = 0;
						o = rows[index]["DIM"];
						if(o != DBNull.Value)
						{
							dim = (short)o;
						}

						if(dim > 0)
						{
							dimTable.DefaultView.RowFilter = "Name = '" + c.Name + "'";
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

							c._row["TYPE_NAME_COMPLETE"] = s + a;

							c._row["TYPE_NAME"] = c._row["TYPE_NAME"] + ":A";
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

