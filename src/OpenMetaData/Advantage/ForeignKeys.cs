using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Advantage
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IForeignKeys))]
#endif 
	public class AdvantageForeignKeys : ForeignKeys
	{
		public AdvantageForeignKeys()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string select = @"SELECT Name as FK_NAME, RI_Primary_Table as PK_TABLE_NAME, " +
					"RI_Foreign_Table as FK_TABLE_NAME, RI_UpdateRule as UPDATE_RULE, " +
					"RI_DeleteRule as DELETE_RULE, '" + this.Table.Database.Name + "' as PK_TABLE_CATALOG, '" +
					this.Table.Database.Name + "' as FK_TABLE_CATALOG, RI_Primary_Index as PK_COLUMN_NAME, " +
					"RI_Foreign_Index as FK_COLUMN_NAME, '' as PK_TABLE_SCHEMA, '' as FK_TABLE_SCHEMA, " +
					"RI_Primary_Index as PK_NAME FROM system.relations WHERE (RI_Primary_Table = '" + this.Table.Name + "' OR " +
					"RI_Foreign_Table = '" + this.Table.Name + "')"; 

				OleDbConnection cn = new OleDbConnection(dbRoot.ConnectionString);
				cn.Open();

				OleDbDataAdapter adapter = new OleDbDataAdapter(select, cn);
				DataTable metaData = new DataTable();

				adapter.Fill(metaData);

				cn.Close();

				PopulateArrayNoHookup(metaData);

				string tableName = "";
				string indexName = "";
				Index index;

				foreach(ForeignKey key in this)
				{
					tableName = key._row["PK_TABLE_NAME"] as string;
					indexName = key._row["PK_COLUMN_NAME"] as string;

					index = this.Table.Tables[tableName].Indexes[indexName] as Index;
					if(index != null)
					{
						foreach(Column col in index.Columns)
						{
							key.AddForeignColumn(index.Table.Database.Name, "", tableName, col.Name, true);
						}
					}

					tableName = key._row["FK_TABLE_NAME"] as string;
					indexName = key._row["FK_COLUMN_NAME"] as string;

					index = this.Table.Tables[tableName].Indexes[indexName] as Index;
					if(index != null)
					{
						foreach(Column col in index.Columns)
						{
							key.AddForeignColumn(index.Table.Database.Name, "", tableName, col.Name, false);
						}
					}
				}
			}
			catch {}
		}
	}
}



//		if(metaData.Columns.Contains("PK_TABLE_CATALOG"))    f_PKTableCatalog = metaData.Columns["PK_TABLE_CATALOG"];
//		if(metaData.Columns.Contains("PK_TABLE_SCHEMA"))	 f_PKTableSchema = metaData.Columns["PK_TABLE_SCHEMA"];
//		if(metaData.Columns.Contains("PK_TABLE_NAME"))		 f_PKTableName = metaData.Columns["PK_TABLE_NAME"];
//		if(metaData.Columns.Contains("FK_TABLE_CATALOG"))	 f_FKTableCatalog = metaData.Columns["FK_TABLE_CATALOG"];
//		if(metaData.Columns.Contains("FK_TABLE_SCHEMA"))	 f_FKTableSchema = metaData.Columns["FK_TABLE_SCHEMA"];
//		if(metaData.Columns.Contains("FK_TABLE_NAME"))		 f_FKTableName = metaData.Columns["FK_TABLE_NAME"];
//		if(metaData.Columns.Contains("ORDINAL"))			 f_Ordinal = metaData.Columns["ORDINAL"];
//		if(metaData.Columns.Contains("UPDATE_RULE"))		 f_UpdateRule = metaData.Columns["UPDATE_RULE"];
//		if(metaData.Columns.Contains("DELETE_RULE"))		 f_DeleteRule = metaData.Columns["DELETE_RULE"];
//		if(metaData.Columns.Contains("PK_NAME"))			 f_PKName = metaData.Columns["PK_NAME"];
//		if(metaData.Columns.Contains("FK_NAME"))			 f_FKName= metaData.Columns["FK_NAME"];
//		if(metaData.Columns.Contains("DEFERRABILITY"))		 f_Deferrability = metaData.Columns["DEFERRABILITY"];
//
//		Name	Character	200	Referential integrity constraint name.
//		RI_Primary_Table	Character	200	The name of the primary table.
//		RI_Primary_Index	Character	200	The name of the primary key used in the referential integrity constraint.
//		RI_Foreign_Table	Character	200	The name of the foreign table.
//		RI_Foreign_Index	Character	200	The name of the foreign key used in the referential integrity constraint.
//		RI_UpdateRule	ShortInt	2	The numeric representation of the update rule used by the referential integrity constraint.
//		RI_DeleteRule	ShortInt	2	The numeric representation of the deletion rule used by the referential integrity constraint.
//		RI_No_Pkey_Error	Memo	variable	The custom error message returned when a new foreign key value does not exist in the primary key.
//		RI_Cascade_Error	Memo	variable	The custom error message returned when a cascading update or delete fails.
