using System;
using System.Data;
using System.Data.Common;

namespace OMeta.MySql5
{
 
	public class MySql5Views : Views
	{
		public MySql5Views()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				MySql5Databases db = this.Database.Databases as MySql5Databases;
				if(db.Version.StartsWith("5"))
				{
					string query = @"SHOW FULL TABLES WHERE Table_type = 'VIEW'";

					DataTable metaData = new DataTable();
					DbDataAdapter adapter = MySql5Databases.CreateAdapter(query, this.dbRoot.ConnectionString);

					adapter.Fill(metaData);

					metaData.Columns[0].ColumnName = "TABLE_NAME";

					PopulateArray(metaData);
				}
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
		}
	}
}
