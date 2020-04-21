using System;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Collections;

namespace OMeta.MySql
{
 
	public class MySqlColumns : Columns
	{
		public MySqlColumns()
		{

		}

		internal DataColumn f_TypeName		= null;
		internal DataColumn f_AutoKey		= null;
		internal DataColumn f_InPrimaryKey	= null;

		override internal void LoadForTable()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Columns, new Object[] {this.Table.Database.Name, null, this.Table.Name});

			PopulateArray(metaData);

			LoadExtraData(this.Table.Name);
		}

		override internal void LoadForView()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Columns, new Object[] {this.View.Database.Name, null, this.View.Name});

			PopulateArray(metaData);

			LoadExtraData(this.View.Name);
		}

		private void LoadExtraData(string name)
		{
			try
			{
				#region ConvertConnectionString
				Hashtable conn = dbRoot.ParsedConnectionString;

				string select = "show fields from " + name;

				string MyConString = "DRIVER={MySQL ODBC 3.51 Driver}";

				MyConString += ";SERVER=";
				if(conn.ContainsKey("LOCATION"))
				{
					if(conn["LOCATION"].ToString().Length > 0)
						MyConString += conn["LOCATION"];
					else
						MyConString += "localhost";
				}
				else
				{
					if(conn.ContainsKey("SERVER"))
					{
						if(conn["SERVER"].ToString().Length > 0)
							MyConString += conn["SERVER"];
						else
							MyConString += "localhost";
					}
					else
					{
						MyConString += "localhost";
					}
				}

				MyConString += ";DATABASE=";
				if(conn.ContainsKey("DB"))
				{
					MyConString += conn["DB"];
				}
				else
				{
					if(conn.ContainsKey("DATA SOURCE"))
					{
						MyConString += conn["DATA SOURCE"];
					}
				}

				MyConString += ";UID=";
				if(conn.ContainsKey("UID"))
				{
					MyConString += conn["UID"];
				}
				else
				{
					if(conn.ContainsKey("USER"))
					{
						MyConString += conn["USER"];
					}
				}

				MyConString += ";PWD=";
				if(conn.ContainsKey("PWD"))
				{
					MyConString += conn["PWD"];
				}
				else
				{
					if(conn.ContainsKey("PASSWORD"))
					{
						MyConString += conn["PASSWORD"];
					}
				}

				MyConString += ";OPTION=3";
				#endregion

				OdbcDataAdapter adapter = new OdbcDataAdapter(select, MyConString);
				DataTable dataTable = new DataTable();

				adapter.Fill(dataTable);

				if(this._array.Count > 0)
				{
					Column col = this._array[0] as Column;

					f_TypeName = new DataColumn("TYPE_NAME", typeof(string));
					col._row.Table.Columns.Add(f_TypeName);

					f_AutoKey  = new DataColumn("AUTO_INCREMENT", typeof(Boolean));
					col._row.Table.Columns.Add(f_AutoKey);

					f_InPrimaryKey  = new DataColumn("IN_PRIMARY_KEY", typeof(Boolean));
					col._row.Table.Columns.Add(f_InPrimaryKey);

					DataRowCollection rows = dataTable.Rows;

					int count = this._array.Count;
					Column c = null;

					for( int index = 0; index < count; index++)
					{
						c = (Column)this[index];

						// TypeName
						c._row["TYPE_NAME"] = rows[index]["Type"] as string;

						// IsAutoKey
						string extra =  rows[index]["Extra"] as string;

						if(0 == extra.IndexOf("auto_increment"))
						{
							c._row["AUTO_INCREMENT"] = true;
						}
						else
						{
							c._row["AUTO_INCREMENT"] = false;
						}

						// IsInPrimaryKey
						string key =  rows[index]["Key"] as string;

						if(0 == key.IndexOf("PRI"))
						{
							c._row["IN_PRIMARY_KEY"] = true;
						}
						else
						{
							c._row["IN_PRIMARY_KEY"] = false;
						}
					}
				}
			}
			catch {}
		}
	}
}
