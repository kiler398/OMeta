using System;
using System.Data;
using System.Data.SQLite;

namespace MyMeta.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumns))]
#endif 
	public class SQLiteColumns : Columns
	{
		private MetaDataHelper _helper;
		
		internal DataColumn f_TypeName			= null;
		internal DataColumn f_TypeNameComplete	= null;

		public SQLiteColumns() {}
		
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

		override internal void LoadForTable()
		{
			DataTable metaData = Helper.LoadTableColumns(this.Table.Name);

			PopulateArray(metaData);

			if(metaData.Columns.Contains(MetaDataHelper.DATA_TYPE_NAME))	
				f_TypeName = metaData.Columns[MetaDataHelper.DATA_TYPE_NAME];
			if(metaData.Columns.Contains(MetaDataHelper.FULL_DATA_TYPE))	
				f_TypeNameComplete = metaData.Columns[MetaDataHelper.FULL_DATA_TYPE];
		}

		override internal void LoadForView()
		{
			DataTable metaData = Helper.LoadViewColumns(this.View.Name);

			PopulateArray(metaData);

			if(metaData.Columns.Contains(MetaDataHelper.DATA_TYPE_NAME))	
				f_TypeName = metaData.Columns[MetaDataHelper.DATA_TYPE_NAME];
			if(metaData.Columns.Contains(MetaDataHelper.FULL_DATA_TYPE))	
				f_TypeNameComplete = metaData.Columns[MetaDataHelper.FULL_DATA_TYPE];
		}
	}
}
