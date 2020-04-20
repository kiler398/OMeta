using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.ISeries
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameters))]
#endif 
	public class ISeriesParameters : Parameters
	{
		public ISeriesParameters()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedure_Parameters, new object[]{null, null, this.Procedure.Name});

				PopulateArray(metaData);

				LoadExtraData();
			}
			catch {}
		}

		private void LoadExtraData()
		{
			try
			{
				if(this._array.Count > 0)
				{
					//string select = "SELECT PARMNAME, TYPENAME, CODEPAGE FROM SYSCAT.PROCPARMS WHERE PROCNAME = '" + this.Procedure.Name + "' ORDER BY ORDINAL";
					string select = "SELECT PARAMETER_NAME, DATA_TYPE, CCSID, PARAMETER_MODE FROM QSYS2.SYSPARMS WHERE SPECIFIC_SCHEMA = '" + this.Procedure.Schema + "' AND SPECIFIC_NAME = '" + this.Procedure.Name + "' ORDER BY ORDINAL_POSITION";

					OleDbDataAdapter adapter = new OleDbDataAdapter(select, this.dbRoot.ConnectionString);
					DataTable dataTable = new DataTable();

					adapter.Fill(dataTable);
				
					Parameter pa = this._array[0] as Parameter;

					if (pa._row.Table.Columns.IndexOf("TYPE_NAME") == -1) 
					{
						f_TypeName = new DataColumn("TYPE_NAME", typeof(string));
						pa._row.Table.Columns.Add(f_TypeName);
					}

					if (pa._row.Table.Columns.IndexOf("FULL_TYPE_NAME") == -1) 
					{
						f_FullTypeName = new DataColumn("FULL_TYPE_NAME", typeof(string));
						pa._row.Table.Columns.Add(f_FullTypeName);
					}

					DataRowCollection rows = dataTable.Rows;
					string paramName = "";

					int count = this._array.Count;
					Parameter p = null;

					foreach(DataRow row in rows)
					{
						paramName = row["PARAMETER_NAME"] as string;

						p = this[paramName] as Parameter;

						string tn = row["DATA_TYPE"].ToString().Trim();
						if (tn == "CHARACTER") tn = "CHAR";
						if (tn == "CHARACTER VARYING") tn = "VARCHAR";

						p._row["TYPE_NAME"] = tn;

						int codepage = -1;
						try
						{
							codepage = (int)row["CCSID"];
						}
						catch{}

						
						string fulltypename = p.TypeName;
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
								int tmp = Int32.Parse(p._row[this.f_CharMaxLength].ToString());
								fulltypename += "(" + tmp + ")";
								break;
							case "DECIMAL":
							case "NUMERIC":
								int tmp1 = Int32.Parse(p._row[this.f_NumericPrecision].ToString());
								int tmp2 = Int32.Parse(p._row[this.f_NumericScale].ToString());
								fulltypename += "(" + tmp1 + "," + tmp2 + ")";
								break;
						}

						if(codepage == 65535)
						{
							// Check for "bit data"
							switch(p.TypeName)
							{
								case "CHAR":
								case "VARCHAR":
									p._row["TYPE_NAME"] = p.TypeName + " FOR BIT DATA";
									fulltypename += " FOR BIT DATA";
									break;
							}
						}
						else 
						{
							// Check for "bit data"
							switch(p.TypeName)
							{
								case "CHAR":
								case "VARCHAR":
									fulltypename += " CCSID " + codepage;
									break;
							}
						}

						p._row["FULL_TYPE_NAME"] = fulltypename;
					}
				}
			}
			catch {}
		}
	}
}
