using System;
using System.Data;
using System.Data.SQLite;

namespace OMeta.SQLite
{
 
	public class SQLiteTable : Table
	{
		private MetaDataHelper _helper;

		public SQLiteTable() {}
		
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

		public override IColumns PrimaryKeys
		{
			get
			{
				if(null == _primaryKeys)
				{
					DataTable metaData = Helper.LoadTableColumns(this.Name);

					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryKeys.Table = this;
					_primaryKeys.dbRoot = this.dbRoot;

					string colName = "";
					bool isKey = false;

					foreach (DataRow row in metaData.Rows)
					{
						if (!row.IsNull(MetaDataHelper.COLUMN_IS_KEY)) 
						{
							isKey = Convert.ToBoolean(row[MetaDataHelper.COLUMN_IS_KEY]);
							if (isKey)
							{
								colName = row["COLUMN_NAME"] as String;
								_primaryKeys.AddColumn((Column)this.Columns[colName]);
							}
						}
					}
				}

				return _primaryKeys;
			}
		}
	}
}
