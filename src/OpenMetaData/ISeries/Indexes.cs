using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.ISeries
{
 
	public class ISeriesIndexes : Indexes
	{
		public ISeriesIndexes()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = new DataTable();

				OleDbConnection cn = new OleDbConnection(this.dbRoot.ConnectionString); 
				cn.Open();
				OleDbCommand cmd = cn.CreateCommand();
				cmd.CommandText = 
@"SELECT i.TABLE_SCHEMA, i.TABLE_NAME, i.INDEX_NAME, 
	i.IS_UNIQUE, i.COLUMN_COUNT, k.COLUMN_NAME,
	i.LONG_COMMENT, i.INDEX_TEXT, i.IS_SPANNING_INDEX
FROM SYSINDEXES i, SYSKEYS k
WHERE i.TABLE_SCHEMA = k.INDEX_SCHEMA
	AND i.INDEX_NAME = k.INDEX_NAME
	AND i.TABLE_SCHEMA = '" + this.Table.Schema + @"' 
	AND i.TABLE_NAME = '" + this.Table.Name + @"'";

				if(!metaData.Columns.Contains("TABLE_CATALOG"))
					f_Catalog = metaData.Columns.Add("TABLE_CATALOG");
				if(!metaData.Columns.Contains("TABLE_SCHEMA"))	
					f_Schema = metaData.Columns.Add("TABLE_SCHEMA");
				if(!metaData.Columns.Contains("TABLE_NAME"))			
					f_TableName = metaData.Columns.Add("TABLE_NAME");
				if(!metaData.Columns.Contains("INDEX_CATALOG"))			
					f_IndexCatalog = metaData.Columns.Add("INDEX_CATALOG");
				if(!metaData.Columns.Contains("INDEX_SCHEMA"))			
					f_IndexSchema = metaData.Columns.Add("INDEX_SCHEMA");
				if(!metaData.Columns.Contains("INDEX_NAME"))			
					f_IndexName = metaData.Columns.Add("INDEX_NAME");
				if(!metaData.Columns.Contains("COLUMN_NAME"))			
					f_IndexName = metaData.Columns.Add("COLUMN_NAME");
				if(!metaData.Columns.Contains("UNIQUE"))				
					f_Unique = metaData.Columns.Add("UNIQUE");
				if(!metaData.Columns.Contains("CLUSTERED"))				
					f_Clustered = metaData.Columns.Add("CLUSTERED");
				if(!metaData.Columns.Contains("TYPE"))					
					f_Type = metaData.Columns.Add("TYPE");
				if(!metaData.Columns.Contains("FILL_FACTOR"))			
					f_FillFactor = metaData.Columns.Add("FILL_FACTOR");
				if(!metaData.Columns.Contains("INITIAL_SIZE"))			
					f_InitializeSize = metaData.Columns.Add("INITIAL_SIZE");
				if(!metaData.Columns.Contains("NULLS"))					
					f_Nulls = metaData.Columns.Add("NULLS");
				if(!metaData.Columns.Contains("SORT_BOOKMARKS"))		
					f_SortBookmarks = metaData.Columns.Add("SORT_BOOKMARKS");
				if(!metaData.Columns.Contains("AUTO_UPDATE"))			
					f_AutoUpdate = metaData.Columns.Add("AUTO_UPDATE");
				if(!metaData.Columns.Contains("NULL_COLLATION"))		
					f_NullCollation = metaData.Columns.Add("NULL_COLLATION");
				if(!metaData.Columns.Contains("COLLATION"))				
					f_Collation = metaData.Columns.Add("COLLATION");
				if(!metaData.Columns.Contains("CARDINALITY"))			
					f_Cardinality = metaData.Columns.Add("CARDINALITY");
				if(!metaData.Columns.Contains("PAGES"))					
					f_Pages = metaData.Columns.Add("PAGES");
				if(!metaData.Columns.Contains("FILTER_CONDITION"))		
					f_FilterCondition = metaData.Columns.Add("FILTER_CONDITION");
				if(!metaData.Columns.Contains("INTEGRATED"))			
					f_Integrated = metaData.Columns.Add("INTEGRATED");

				OleDbDataReader reader = cmd.ExecuteReader();
				while(reader.Read()) 
				{
					DataRow row = metaData.NewRow();

					string uniqueCode = reader["IS_UNIQUE"].ToString();
					bool allowNulls = false;

					row["TABLE_CATALOG"] = reader["TABLE_SCHEMA"].ToString();
					row["TABLE_SCHEMA"] = reader["TABLE_SCHEMA"].ToString();
					row["TABLE_NAME"] = reader["TABLE_NAME"].ToString();
					row["INDEX_CATALOG"] = reader["TABLE_SCHEMA"].ToString();
					row["INDEX_SCHEMA"] = reader["TABLE_SCHEMA"].ToString();
					row["INDEX_NAME"] = reader["INDEX_NAME"].ToString();
					row["COLUMN_NAME"] = reader["COLUMN_NAME"].ToString();
					row["UNIQUE"] = ((uniqueCode == "U") || 
						(uniqueCode == "V")) ? 1 : 0;
					row["TYPE"] = 0;
					row["FILL_FACTOR"] = 0;
					row["INITIAL_SIZE"] = 0;
					row["NULLS"] = allowNulls ? 1 : 0;
					row["SORT_BOOKMARKS"] = 0;
					row["AUTO_UPDATE"] = 0;
					row["NULL_COLLATION"] = 0;
					row["COLLATION"] = 0;
					row["CARDINALITY"] = 0;
					row["PAGES"] = 0;
					row["FILTER_CONDITION"] = string.Empty;
					row["INTEGRATED"] = 0;

					metaData.Rows.Add(row);
				}
				reader.Close();
				cn.Close();

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
