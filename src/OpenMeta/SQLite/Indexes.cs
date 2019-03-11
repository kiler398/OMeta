using System;
using System.Data;
using System.Data.SQLite;

namespace MyMeta.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndexes))]
#endif 
	public class SQLiteIndexes : Indexes
	{
		private MetaDataHelper _helper;

		public SQLiteIndexes() {}
		
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
			DataTable metaData = Helper.LoadIndexColumns(this.Table.Name);

			if (metaData != null) 
			{
				PopulateArray(metaData);
			}
		}
	}
}
