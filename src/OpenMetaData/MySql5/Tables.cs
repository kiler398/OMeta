using System;
using System.Data;
using System.Data.Common;

namespace OMeta.MySql5
{
 
	public class MySql5Tables : Tables
	{
		public MySql5Tables()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				MySql5Databases db = this.Database.Databases as MySql5Databases;

				string query = "SHOW TABLES";
				if(db.Version.StartsWith("5"))
				{
					query = @"SHOW FULL TABLES WHERE Table_type = 'BASE TABLE'";
				}

				DataTable metaData = new DataTable();
				DbDataAdapter adapter = MySql5Databases.CreateAdapter(query, this.dbRoot.ConnectionString);

				adapter.Fill(metaData);

				metaData.Columns[0].ColumnName = "TABLE_NAME";

				PopulateArray(metaData);

				if(db.Version.StartsWith("5"))
				{
					LoadTableDescriptions();
				}
			}
			catch {	}
		}

		private void LoadTableDescriptions()
		{
			try
			{
				string query = @"SELECT TABLE_NAME, TABLE_COMMENT, CREATE_TIME, UPDATE_TIME FROM information_schema.TABLES WHERE TABLE_SCHEMA = '" + this.Database.Name + "'";

				DataTable metaData = new DataTable();
				DbDataAdapter adapter = MySql5Databases.CreateAdapter(query, this.dbRoot.ConnectionString);

				adapter.Fill(metaData);

				if(this.Database.Tables.Count > 0)
				{
					Table t = this.Database.Tables[0] as Table;

					if(!t._row.Table.Columns.Contains("DESCRIPTION"))
					{
						t._row.Table.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
						this.f_Description = t._row.Table.Columns["DESCRIPTION"];
					}

					if(!t._row.Table.Columns.Contains("TABLE_SCHEMA"))
					{
						t._row.Table.Columns.Add("TABLE_SCHEMA", Type.GetType("System.String"));
						this.f_Schema = t._row.Table.Columns["TABLE_SCHEMA"];
					}

					if(!t._row.Table.Columns.Contains("DATE_CREATED"))
					{
						t._row.Table.Columns.Add("DATE_CREATED", Type.GetType("System.DateTime"));
						this.f_DateCreated = t._row.Table.Columns["DATE_CREATED"];
					}

					if(!t._row.Table.Columns.Contains("DATE_MODIFIED"))
					{
						t._row.Table.Columns.Add("DATE_MODIFIED", Type.GetType("System.DateTime"));
						this.f_DateModified = t._row.Table.Columns["DATE_MODIFIED"];
					}
				}

				if(metaData.Rows.Count > 0)
				{
					foreach(DataRow row in metaData.Rows)
					{
						Table t = this[row["TABLE_NAME"] as string] as Table;

						t._row["DESCRIPTION"] = row["TABLE_COMMENT"] as string;
						t._row["TABLE_SCHEMA"] = this.Database.Name;

						if(row["CREATE_TIME"] != DBNull.Value)
						{
							t._row["DATE_CREATED"] = (DateTime)row["CREATE_TIME"];
						}

						if(row["UPDATE_TIME"] != DBNull.Value)
						{
							t._row["DATE_MODIFIED"] = (DateTime)row["UPDATE_TIME"];
						}
					}
				}
			}
			catch {}
		}
	}
}
