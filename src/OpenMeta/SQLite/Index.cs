using System;
using System.Data;
using System.Collections;
using System.Data.OleDb;

using System.Data.SQLite;

namespace MyMeta.SQLite
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndex))]
#endif 
	public class SQLiteIndex : Index
	{
		//private SQLiteColumns _indexColumns;
		//private MetaDataHelper _helper;

		public SQLiteIndex() {}
		
		/*private MetaDataHelper Helper
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

		public override IColumns Columns
		{
			get
			{
				if(null == _indexColumns)
				{
	
					_indexColumns = (SQLiteColumns)this.dbRoot.ClassFactory.CreateColumns();
					_indexColumns.Index = this;
					_indexColumns.dbRoot = this.dbRoot;

					ArrayList indexColumns = Helper.LoadIndexColumns(this.Table.Name);
					string colName;

					foreach (IColumn col in this.Table.Columns)
					{
						colName = col.Name;
						if (indexColumns.Contains(colName)) 
						{
							_indexColumns.AddColumn((Column)this.Columns[colName]);
						}
					}
				}

				return _indexColumns;
			}
		}*/
	}
}
