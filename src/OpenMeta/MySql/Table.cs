using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.MySql
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(ITable))]
#endif 
	public class MySqlTable : Table
	{
		public MySqlTable()
		{

		}

		public override IColumns PrimaryKeys
		{
			get
			{
				if(null == _primaryKeys)
				{
					DataTable metaData = this.LoadData(OleDbSchemaGuid.Primary_Keys, 
						new Object[] {this.Tables.Database.Name, null, this.Name});

					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryKeys.Table = this;
					_primaryKeys.dbRoot = this.dbRoot;

					string colName = "";

					int count = metaData.Rows.Count;
					for(int i = 0; i < count; i++)
					{
						colName = metaData.Rows[i]["COLUMN_NAME"] as string;
						_primaryKeys.AddColumn((Column)this.Columns[colName]);
					}
				}

				return _primaryKeys;
			}
		}

//		public override IColumns PrimaryKeys
//		{
//			get
//			{
//				if(null == _primaryKeys)
//				{
//					OleDbConnection cn = new OleDbConnection(this.dbRoot.ConnectionString); 
//					cn.Open(); 
//					DataTable metaData = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, 
//						new Object[] {null, this.Tables.Database.Name, this.Name});
//					cn.Close();
//
//					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
//
//					Columns cols = (Columns)this.Columns;
//
//					string colName = "";
//
//					int count = metaData.Rows.Count;
//					for(int i = 0; i < count; i++)
//					{
//						colName = metaData.Rows[i]["COLUMN_NAME"] as string;
//						_primaryKeys.AddColumn((Column)cols[colName]);
//					}
//				}
//
//				return _primaryKeys;
//			}
//		}
	}
}

