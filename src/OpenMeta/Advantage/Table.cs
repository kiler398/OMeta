using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Advantage
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(ITable))]
#endif 
	public class AdvantageTable : Table
	{
		public AdvantageTable()
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

					string data = "";

					int count = metaData.Rows.Count;
					if(count > 0)
					{
						data = metaData.Rows[0]["COLUMN_NAME"] as string;
						string[] pks = data.Split(';');
						foreach(string colName in pks)
						{
							_primaryKeys.AddColumn((Column)this.Columns[colName]);
						}
					}
				}

				return _primaryKeys;
			}
		}
	}
}
