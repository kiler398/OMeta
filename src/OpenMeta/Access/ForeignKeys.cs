using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Access
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
    /// <summary>
    /// Access数据库外键集合元数据信息
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IForeignKeys))]
#endif 
	public class AccessForeignKeys : ForeignKeys
	{
		public AccessForeignKeys()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData1 = this.LoadData(OleDbSchemaGuid.Foreign_Keys, 
					new object[]{null, null, this.Table.Name});

				DataTable metaData2 = this.LoadData(OleDbSchemaGuid.Foreign_Keys, 
					new object[]{null, null, null, null, null, this.Table.Name});

				DataRowCollection rows = metaData2.Rows;
				int count = rows.Count;
				for(int i = 0; i < count; i++)
				{
					metaData1.ImportRow(rows[i]);
				}

				PopulateArray(metaData1);
			}
			catch {}
		}
	}
}
