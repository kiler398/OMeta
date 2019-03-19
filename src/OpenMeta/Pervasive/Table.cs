using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Pervasive
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(ITable))]
#endif 
	public class PervasiveTable : Table
	{
		public PervasiveTable()
		{

		}

		public override IColumns PrimaryKeys
		{
			get
			{
				if(null == _primaryKeys)
				{
					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryKeys.Table = this;
					_primaryKeys.dbRoot = this.dbRoot;

					try
					{
						string select = "SELECT Xe$Name AS COLUMN_NAME FROM X$File,X$Index,X$Field " +
							            "WHERE Xf$Id=Xi$File and Xi$Field=Xe$Id and Xf$Name = '" + this.Name + "' AND Xi$Flags > 16384 " +
										"ORDER BY Xi$Number,Xi$Part";

						OleDbDataAdapter adapter = new OleDbDataAdapter(select, this.dbRoot.ConnectionString);
						DataTable dataTable = new DataTable();

						adapter.Fill(dataTable);

						string colName = "";

						int count = dataTable.Rows.Count;
						for(int i = 0; i < count; i++)
						{
							colName = dataTable.Rows[i]["COLUMN_NAME"] as string;
							_primaryKeys.AddColumn((Column)this.Columns[colName.Trim()]);
						}
					}
					catch {}
				}

				return _primaryKeys;
			}
		}

	}
}
