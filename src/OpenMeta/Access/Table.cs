using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Access
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
    /// <summary>
    /// Access数据库表元数据信息
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(ITable))]
#endif 
	public class AccessTable : Table
	{
		public AccessTable()
		{

		}

		public override IColumns PrimaryKeys
		{
			get
			{
				if(null == _primaryKeys)
				{
					DataTable metaData = this.LoadData(OleDbSchemaGuid.Primary_Keys, 
						new Object[] {null, null, this.Name});

					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryKeys.Table = this;
					_primaryKeys.dbRoot = this.dbRoot;

					string colName = "";

                    metaData.DefaultView.Sort = "ORDINAL";

                    foreach (DataRowView rv in metaData.DefaultView)
                    {
                        colName = rv.Row["COLUMN_NAME"] as string;
                        _primaryKeys.AddColumn((Column)this.Columns[colName]);

                    }

                    //int count = metaData.Rows.Count;
                    //for(int i = 0; i < count; i++)
                    //{
                    //    colName = metaData.Rows[i]["COLUMN_NAME"] as string;
                    //    _primaryKeys.AddColumn((Column)this.Columns[colName]);
                    //}
				}

				return _primaryKeys;
			}
		}

	}
}
