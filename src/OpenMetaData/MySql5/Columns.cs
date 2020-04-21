using System;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace OMeta.MySql5
{
 
	public class MySql5Columns : Columns
	{
		public MySql5Columns()
		{

		}

		internal DataColumn f_Extra	= null;

		override internal void LoadForTable()
		{
			string query = @"SHOW COLUMNS FROM `" + this.Table.Name + "`";

			DataTable metaData = new DataTable();
			DbDataAdapter adapter = MySql5Databases.CreateAdapter(query, this.dbRoot.ConnectionString);

			adapter.Fill(metaData);

			if(metaData.Columns.Contains("Extra"))	f_Extra = metaData.Columns["Extra"];

			metaData.Columns["Field"].ColumnName   = "COLUMN_NAME";
			metaData.Columns["Type"].ColumnName    = "DATA_TYPE";
			metaData.Columns["Null"].ColumnName    = "IS_NULLABLE";
			metaData.Columns["Default"].ColumnName = "COLUMN_DEFAULT";
			
			PopulateArray(metaData);

			LoadTableColumnDescriptions();
		}

		override internal void LoadForView()
		{
			MySql5Database db   = this.View.Database as MySql5Database;
			MySql5Databases dbs = db.Databases as MySql5Databases;
			if(dbs.Version.StartsWith("5"))
			{
				string query = @"SHOW COLUMNS FROM `" + this.View.Name + "`";

				DataTable metaData = new DataTable();
				DbDataAdapter adapter = MySql5Databases.CreateAdapter(query, this.dbRoot.ConnectionString);

				adapter.Fill(metaData);

				if(metaData.Columns.Contains("Extra"))	f_Extra = metaData.Columns["Extra"];

				metaData.Columns["Field"].ColumnName   = "COLUMN_NAME";
				metaData.Columns["Type"].ColumnName    = "DATA_TYPE";
				metaData.Columns["Null"].ColumnName    = "IS_NULLABLE";
				metaData.Columns["Default"].ColumnName = "COLUMN_DEFAULT";
			
				PopulateArray(metaData);
			}
		}

		private void LoadTableColumnDescriptions()
		{
			try
			{
				string query = @"SELECT TABLE_NAME, COLUMN_NAME, COLUMN_COMMENT FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = '" + 
					this.Table.Database.Name + "' AND TABLE_NAME ='" + this.Table.Name + "'";

				DataTable metaData = new DataTable();
				DbDataAdapter adapter = MySql5Databases.CreateAdapter(query, this.dbRoot.ConnectionString);

				adapter.Fill(metaData);

				if(metaData.Rows.Count > 0)
				{
					foreach(DataRow row in metaData.Rows)
					{
						Column c = this[row["COLUMN_NAME"] as string] as Column;

						if(!c._row.Table.Columns.Contains("DESCRIPTION"))
						{
							c._row.Table.Columns.Add("DESCRIPTION", Type.GetType("System.String"));
							this.f_Description = c._row.Table.Columns["DESCRIPTION"];
						}

						c._row["DESCRIPTION"] = row["COLUMN_COMMENT"] as string;
					}
				}
			}
			catch {}
		}
	}
}
