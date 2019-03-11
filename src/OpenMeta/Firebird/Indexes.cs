using System;
using System.Data;

using FirebirdSql.Data.FirebirdClient;

namespace MyMeta.Firebird
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndexes))]
#endif 
	public class FirebirdIndexes : Indexes
	{
		public FirebirdIndexes()
		{

		}

		override internal void LoadAll()
		{
			try
			{
                using (FbConnection cn = new FirebirdSql.Data.FirebirdClient.FbConnection(this._dbRoot.ConnectionString))
                {
                    cn.Open();
                    
                    DataTable idxMetaData = cn.GetSchema("Indexes", new string[] { null, null, this.Table.Name });

                    if (!idxMetaData.Columns.Contains("CARDINALITY")) idxMetaData.Columns.Add("CARDINALITY");
                    if (!idxMetaData.Columns.Contains("COLUMN_NAME")) idxMetaData.Columns.Add("COLUMN_NAME");

                    idxMetaData.Columns["IS_UNIQUE"].ColumnName = "UNIQUE";
                    idxMetaData.Columns["INDEX_TYPE"].ColumnName = "TYPE";

                    DataTable metaData = idxMetaData.Clone();
                    metaData.Clear();
                    foreach (DataRow row in idxMetaData.Rows)
                    {
                        string indexName = (string)row["INDEX_NAME"];
                        DataTable metaDataColumns = cn.GetSchema("IndexColumns", new string[] { null, null, null, indexName });
                        metaDataColumns.DefaultView.Sort = "ORDINAL_POSITION ASC";
                        foreach (DataRowView vrow in metaDataColumns.DefaultView)
                        {
                            DataRow newrow = metaData.Rows.Add(row.ItemArray);
                            newrow["CARDINALITY"] = vrow["ORDINAL_POSITION"];
                            newrow["COLUMN_NAME"] = vrow["COLUMN_NAME"];
                        }
                    }
                    cn.Close();
                 
                    PopulateArray(metaData);
                }
			}
			catch(Exception ex)
			{
				string m = ex.Message;
			}
		}
	}
}
