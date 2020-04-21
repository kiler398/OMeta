using System;
using System.Data;
using System.Data.SQLite;

namespace OMeta.SQLite
{
  
	public class SQLiteTables : Tables
	{
		private MetaDataHelper _helper;

		public SQLiteTables() {}
		
		private MetaDataHelper Helper
		{
			get 
			{
				if (_helper == null) 
				{
					SQLiteConnection connection = ConnectionHelper.CreateConnection(dbRoot);
					_helper = new MetaDataHelper( connection );
					connection.Close();
				}
				return _helper;
			}
		}

		override internal void LoadAll()
		{
			DataTable metaData = Helper.Tables;

			PopulateArray(metaData);
		}
	}
}
