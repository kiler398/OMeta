using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.ISeries
{
 
	public class ISeriesForeignKeys : ForeignKeys
	{
		public ISeriesForeignKeys()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				string sql1 = @"SELECT c.CONSTRAINT_SCHEMA, c.CONSTRAINT_NAME, cpk.CONSTRAINT_NAME as PK_CONSTRAINT_NAME, c.CONSTRAINT_TYPE, c.TABLE_SCHEMA, c.TABLE_NAME, col.COLUMN_NAME
FROM SYSCST c, SYSCST cpk, QSYS2.SYSREFCST rf, SYSCSTCOL col
WHERE c.CONSTRAINT_SCHEMA = rf.CONSTRAINT_SCHEMA
AND c.CONSTRAINT_NAME = rf.CONSTRAINT_NAME
AND rf.UNIQUE_CONSTRAINT_SCHEMA = cpk.CONSTRAINT_SCHEMA
AND rf.UNIQUE_CONSTRAINT_NAME = cpk.CONSTRAINT_NAME
AND col.CONSTRAINT_SCHEMA = c.CONSTRAINT_SCHEMA
AND col.CONSTRAINT_NAME = c.CONSTRAINT_NAME
AND c.CONSTRAINT_TYPE = 'FOREIGN KEY'
AND c.TABLE_SCHEMA = '" + this.Table.Schema + @"' 
AND c.TABLE_NAME = '" + this.Table.Name + @"'
ORDER BY c.CONSTRAINT_SCHEMA, c.CONSTRAINT_NAME";

				string sql2 = @"SELECT c.CONSTRAINT_SCHEMA, c.CONSTRAINT_NAME, cpk.CONSTRAINT_NAME as PK_CONSTRAINT_NAME, c.CONSTRAINT_TYPE, cpk.TABLE_SCHEMA, cpk.TABLE_NAME, col.COLUMN_NAME
FROM SYSCST c, SYSCST cpk, QSYS2.SYSREFCST rf, SYSCSTCOL col
WHERE c.CONSTRAINT_SCHEMA = rf.CONSTRAINT_SCHEMA
AND c.CONSTRAINT_NAME = rf.CONSTRAINT_NAME
AND rf.UNIQUE_CONSTRAINT_SCHEMA = cpk.CONSTRAINT_SCHEMA
AND rf.UNIQUE_CONSTRAINT_NAME = cpk.CONSTRAINT_NAME
AND col.CONSTRAINT_SCHEMA = cpk.CONSTRAINT_SCHEMA
AND col.CONSTRAINT_NAME = cpk.CONSTRAINT_NAME
AND c.CONSTRAINT_TYPE = 'FOREIGN KEY'
AND c.TABLE_SCHEMA = '" + this.Table.Schema + @"' 
AND c.TABLE_NAME = '" + this.Table.Name + @"'
ORDER BY c.CONSTRAINT_SCHEMA, c.CONSTRAINT_NAME";

				OleDbDataAdapter adapter1 = new OleDbDataAdapter(sql1, this.dbRoot.ConnectionString);
				DataTable dataTableFk = new DataTable();
				adapter1.Fill(dataTableFk);
					
				OleDbDataAdapter adapter2 = new OleDbDataAdapter(sql2, this.dbRoot.ConnectionString);
				DataTable dataTablePk = new DataTable();
				adapter2.Fill(dataTablePk);
					
				PopulateArray(dataTableFk, dataTablePk);
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
		}
		
		internal void PopulateArray(DataTable dataTableFk, DataTable dataTablePk)
		{
			DataTable metaData = new DataTable();
			metaData.Columns.AddRange(
				new DataColumn[] {
										new DataColumn("PK_TABLE_CATALOG"),
										new DataColumn("PK_TABLE_SCHEMA"),
										new DataColumn("PK_TABLE_NAME"),
										new DataColumn("FK_TABLE_CATALOG"),
										new DataColumn("FK_TABLE_SCHEMA"),
										new DataColumn("FK_TABLE_NAME"),
										new DataColumn("ORDINAL"),
										new DataColumn("PK_NAME"),
									 new DataColumn("FK_NAME"),
									 new DataColumn("UPDATE_RULE"),
									 new DataColumn("DELETE_RULE"),
									 new DataColumn("DEFERRABILITY")
								}
				);
			this.BindToColumns(metaData);

			ForeignKey key  = null;

			int j = 0;
			for (int i=0; i < dataTableFk.Rows.Count; i++)
			{
				DataRow rowPk = dataTablePk.Rows[i];
				DataRow rowFk = dataTableFk.Rows[i];
				try
				{
					string pkschema = rowPk["TABLE_SCHEMA"].ToString();
					string pktable = rowPk["TABLE_NAME"].ToString();
					string pkcolumn = rowPk["COLUMN_NAME"].ToString();
					string fkschema = rowPk["TABLE_SCHEMA"].ToString();
					string fktable = rowFk["TABLE_NAME"].ToString();
					string fkcolumn = rowFk["COLUMN_NAME"].ToString();
					string fkKeyName = rowFk["CONSTRAINT_NAME"].ToString();
					string pkKeyName = rowFk["PK_CONSTRAINT_NAME"].ToString();

					//DataRow row = rowView.Row;
					key = this.GetByName(fkKeyName);

					if(null == key)
					{
						DataRow row = metaData.NewRow();
		
						row["PK_TABLE_CATALOG"] = pkschema;
						row["PK_TABLE_SCHEMA"] = pkschema;
						row["PK_TABLE_NAME"] = pktable;
						row["FK_TABLE_CATALOG"] = fkschema;
						row["FK_TABLE_SCHEMA"] = fkschema;
						row["FK_TABLE_NAME"] = fktable;
						row["ORDINAL"] = j++.ToString();
						row["PK_NAME"] = pkKeyName;
						row["FK_NAME"] = fkKeyName;
						row["UPDATE_RULE"] = "";
						row["DELETE_RULE"] = "";
						row["DEFERRABILITY"] = 0;
				
						metaData.Rows.Add(row);
				
						key = (ForeignKey)this.dbRoot.ClassFactory.CreateForeignKey();
						key.dbRoot = this.dbRoot;
						key.ForeignKeys = this;
						key.Row = row;
						this._array.Add(key);
					}

					key.AddForeignColumn(pkschema, pkschema, pktable, pkcolumn, true);
					key.AddForeignColumn(fkschema, fkschema, fktable, fkcolumn, false);
				}
				catch (Exception ex)
				{
					string tmp = ex.Message;
				}
			}
		}
		
		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("PK_TABLE_CATALOG"))    f_PKTableCatalog = metaData.Columns["PK_TABLE_CATALOG"];
				if(metaData.Columns.Contains("PK_TABLE_SCHEMA"))	 f_PKTableSchema = metaData.Columns["PK_TABLE_SCHEMA"];
				if(metaData.Columns.Contains("PK_TABLE_NAME"))		 f_PKTableName = metaData.Columns["PK_TABLE_NAME"];
				if(metaData.Columns.Contains("FK_TABLE_CATALOG"))	 f_FKTableCatalog = metaData.Columns["FK_TABLE_CATALOG"];
				if(metaData.Columns.Contains("FK_TABLE_SCHEMA"))	 f_FKTableSchema = metaData.Columns["FK_TABLE_SCHEMA"];
				if(metaData.Columns.Contains("FK_TABLE_NAME"))		 f_FKTableName = metaData.Columns["FK_TABLE_NAME"];
				if(metaData.Columns.Contains("ORDINAL"))			 f_Ordinal = metaData.Columns["ORDINAL"];
				if(metaData.Columns.Contains("UPDATE_RULE"))		 f_UpdateRule = metaData.Columns["UPDATE_RULE"];
				if(metaData.Columns.Contains("DELETE_RULE"))		 f_DeleteRule = metaData.Columns["DELETE_RULE"];
				if(metaData.Columns.Contains("PK_NAME"))			 f_PKName = metaData.Columns["PK_NAME"];
				if(metaData.Columns.Contains("FK_NAME"))			 f_FKName= metaData.Columns["FK_NAME"];
				if(metaData.Columns.Contains("DEFERRABILITY"))		 f_Deferrability = metaData.Columns["DEFERRABILITY"];

				_fieldsBound = true;
			}
		}
	}
}
