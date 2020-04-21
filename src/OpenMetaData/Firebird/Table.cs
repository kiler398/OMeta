using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;


namespace OMeta.Firebird
{
 
	public class FirebirdTable : Table
	{
		public FirebirdTable()
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
						FbConnection cn = new FirebirdSql.Data.FirebirdClient.FbConnection(this._dbRoot.ConnectionString);
						cn.Open();
						DataTable metaData = cn.GetSchema("PrimaryKeys", new string[] {null, null, this.Name});
						cn.Close();

						string colName;
						Column c;
						foreach(DataRow row in metaData.Rows)
						{
							colName = row["COLUMN_NAME"] as string;

							c = this.Columns[colName] as Column;

							_primaryKeys.AddColumn(c);
						}
					}
					catch(Exception ex)
					{
						string m = ex.Message;
					}
				}

				return _primaryKeys;
			}
		}
	}
}
