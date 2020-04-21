using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Advantage
{
 
	public class AdvantageColumns : Columns
	{
		public AdvantageColumns()
		{

		}

		internal DataColumn f_TypeName	= null;
		internal DataColumn f_AutoKey	= null;

		override internal void LoadForTable()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Columns, new Object[] {null, null, this.Table.Name});
			PopulateArray(metaData);
			LoadExtraData(this.Table.Name, "T");
	   }

		override internal void LoadForView()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Columns, new Object[] {null, null, this.View.Name});
			PopulateArray(metaData);
			LoadExtraData(this.View.Name, "V");
		}

		private void LoadExtraData(string name, string type)
		{
			try
			{
				string select = "SELECT Name, Field_Type FROM system.columns WHERE Parent = '" + name + "'";

				OleDbConnection cn = new OleDbConnection(dbRoot.ConnectionString);
				cn.Open();
	
				OleDbDataAdapter adapter = new OleDbDataAdapter(select, cn);
				DataTable dataTable = new DataTable();

				adapter.Fill(dataTable);
				cn.Close();

				if(this._array.Count > 0)
				{
					Column col = this._array[0] as Column;

					f_TypeName = new DataColumn("TYPE_NAME", typeof(string));
					col._row.Table.Columns.Add(f_TypeName);

					string typeName = "";
					DataRowCollection rows = dataTable.Rows;

					int count = this._array.Count;
					Column c = null;

					for( int index = 0; index < count; index++)
					{
						c = (Column)this.GetByPhysicalName((string)(rows[index]["Name"]));

						switch((System.Int16)rows[index]["Field_Type"])
						{
							case 1: typeName = "Logical"; break;
							case 2: typeName = "Numeric"; break;
							case 3:	typeName = "Date"; break;
							case 4:	typeName = "String"; break;
							case 5:	typeName = "Memo"; break;
							case 6: typeName = "Binary"; break;
							case 7:	typeName = "Image"; break;
							case 8:	typeName = "Varchar"; break;
							case 9:	typeName = "Compactdate"; break;
							case 10: typeName = "Double"; break;
							case 11: typeName = "Integer"; break;
							case 12: typeName = "ShortInt"; break;
							case 13: typeName = "Time"; break;
							case 14: typeName = "TimeStamp"; break;
							case 15: typeName = "AutoInc"; break;
							case 16: typeName = "Raw"; break;
							case 17: typeName = "CurDouble"; break;
							case 18: typeName = "Money"; break;
							case 19: typeName = "LongLong"; break;
							case 20: typeName = "CIString"; break;
							default: typeName = "Uknown"; break;
						}

						c._row["TYPE_NAME"] = typeName;
					}
				}
			}
			catch {}
		}
	}
}
