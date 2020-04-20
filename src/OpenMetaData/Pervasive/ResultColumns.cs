using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Pervasive
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IResultColumns))]
#endif 
	public class PervasiveResultColumns : ResultColumns
	{
		public PervasiveResultColumns()
		{

		}

		override internal void LoadAll()
		{
			ADODB.Connection cnn = new ADODB.Connection();
			ADODB.Recordset rs = new ADODB.Recordset();
			ADOX.Catalog cat = new ADOX.Catalog();
    
			// Open the Connection
			cnn.Open(dbRoot.ConnectionString, null, null, 0);
			cat.ActiveConnection = cnn;

			ADOX.Procedure proc = cat.Procedures[this.Procedure.Name];
       
			// Retrieve Parameter information
			rs.Source = proc.Command as ADODB.Command;
			rs.Fields.Refresh();

			Pervasive.PervasiveResultColumn resultColumn;

			if(rs.Fields.Count > 0)
			{
				int ordinal = 0;
			
				foreach(ADODB.Field field in rs.Fields)
				{
					resultColumn = this.dbRoot.ClassFactory.CreateResultColumn() as Pervasive.PervasiveResultColumn;
					resultColumn.dbRoot = this.dbRoot;
					resultColumn.ResultColumns = this;

					resultColumn.name = field.Name;
					resultColumn.ordinal = ordinal++;
					resultColumn.typeName = field.Type.ToString();

					this._array.Add(resultColumn);
				}
			}

			cnn.Close();
		}
	}
}
