using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using FirebirdSql.Data.FirebirdClient;

namespace OMeta.Firebird
{
 
	public class FirebirdForeignKeys : ForeignKeys
    {
        private static DataTable allFkData = null;
        private static NameValueCollection mappingHash = null;

        public FirebirdForeignKeys()
        {
        }

		override internal void LoadAll()
		{
			try
			{
                using (FbConnection cn = new FirebirdSql.Data.FirebirdClient.FbConnection(this._dbRoot.ConnectionString))
                {
                    cn.Open();
                    if (allFkData == null)
                    {
                        allFkData = cn.GetSchema("ForeignKeys");
                        allFkData.Columns.Add("COLUMN_NAME");
                        allFkData.Columns.Add("REFERENCED_COLUMN_NAME");
                        allFkData.Columns.Add("ORDINAL_POSITION");
                        allFkData.Columns.Add("DEFERRABILITY");

                        mappingHash = new NameValueCollection();
                        mappingHash["FK_TABLE_CATALOG"] = "TABLE_CATALOG";
                        mappingHash["FK_TABLE_SCHEMA"] = "TABLE_SCHEMA";
                        mappingHash["FK_TABLE_NAME"] = "TABLE_NAME";
                        mappingHash["PK_TABLE_CATALOG"] = "REFERENCED_TABLE_CATALOG";
                        mappingHash["PK_TABLE_SCHEMA"] = "REFERENCED_TABLE_SCHEMA";
                        mappingHash["PK_TABLE_NAME"] = "REFERENCED_TABLE_NAME";
                        mappingHash["ORDINAL"] = "ORDINAL_POSITION";
                        mappingHash["UPDATE_RULE"] = "UPDATE_RULE";
                        mappingHash["DELETE_RULE"] = "DELETE_RULE";
                        mappingHash["PK_NAME"] = "INDEX_NAME";
                        mappingHash["FK_NAME"] = "CONSTRAINT_NAME";
                        mappingHash["DEFERRABILITY"] = "DEFERRABILITY";
                    }

                    DataTable metaData = allFkData.Clone();
                    metaData.Clear();
                    foreach (DataRow row in allFkData.Rows)
                    {
                        if ((this.Table.Name == (string)row["TABLE_NAME"]) ||
                            (this.Table.Name == (string)row["REFERENCED_TABLE_NAME"]))
                        {
                            string indexName = (string)row["INDEX_NAME"];
                            string refTableName = (string)row["REFERENCED_TABLE_NAME"];
                            string isDef = (string)row["IS_DEFERRABLE"];
                            string initDef = (string)row["INITIALLY_DEFERRED"];


                            row["DEFERRABILITY"] = (isDef == "NO" ? 3 : (initDef == "YES" ? 2 : 1));
                            DataTable metaDataColumns = cn.GetSchema("IndexColumns", new string[] { null, null, null, indexName });
                            metaDataColumns.DefaultView.Sort = "ORDINAL_POSITION ASC";

                            DataTable metaDataPKIndex = cn.GetSchema("Indexes", new string[] { null, null, refTableName });
                            metaDataPKIndex.DefaultView.RowFilter = "IS_PRIMARY = True";

                            string refPkIndexName = (string)metaDataPKIndex.DefaultView[0]["INDEX_NAME"];

                            DataTable metaDataColumnsRefPk = cn.GetSchema("IndexColumns", new string[] { null, null, null, refPkIndexName });
                            metaDataColumnsRefPk.DefaultView.Sort = "ORDINAL_POSITION ASC";

                            if (metaDataColumnsRefPk.Rows.Count == metaDataColumns.Rows.Count)
                            {
                                for (int i = 0; i < metaDataColumnsRefPk.Rows.Count; i++)
                                {
                                    DataRow newrow = metaData.Rows.Add(row.ItemArray);
                                    newrow["ORDINAL_POSITION"] = metaDataColumnsRefPk.DefaultView[i]["ORDINAL_POSITION"];
                                    newrow["COLUMN_NAME"] = metaDataColumns.DefaultView[i]["COLUMN_NAME"];
                                    newrow["REFERENCED_COLUMN_NAME"] = metaDataColumnsRefPk.DefaultView[i]["COLUMN_NAME"];
                                }
                            }
                        }
                    }
                    cn.Close();

                    PopulateArrayNoHookup(metaData, mappingHash);

                    ForeignKey key = null;
                    string keyName = "";

                    foreach (DataRow row in metaData.Rows)
                    {
                        keyName = row["CONSTRAINT_NAME"] as string;

                        key = this.GetByName(keyName);

                        key.AddForeignColumn(null, null, (string)row["TABLE_NAME"], (string)row["COLUMN_NAME"], false);
                        key.AddForeignColumn(null, null, (string)row["REFERENCED_TABLE_NAME"], (string)row["REFERENCED_COLUMN_NAME"], true);
                    }
                }
            }
			catch(Exception ex)
			{
				string m = ex.Message;
			}
		}
	}
}
