using System;
using System.Data;
using System.Data.SQLite;

namespace OMeta.SQLite
{
 
	public class SQLiteViews : Views
	{
		private MetaDataHelper _helper;

		public SQLiteViews() {}
		
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
			DataTable metaData = Helper.Views;

			PopulateArray(metaData);
		}

	}
}
