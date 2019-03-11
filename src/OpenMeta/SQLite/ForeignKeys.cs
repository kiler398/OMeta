using System;
using System.Data;
using System.Data.SQLite;

namespace MyMeta.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IForeignKeys))]
#endif 
	public class SQLiteForeignKeys : ForeignKeys
	{
		private MetaDataHelper _helper;

		public SQLiteForeignKeys() {}
		
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
			DataTable metaData = Helper.LoadFKColumns(this.Table.Name);

			if (metaData != null) 
			{
				PopulateArray(metaData);
			}
		}
	}
}
