using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.MySql
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameters))]
#endif 
	public class MySqlParameters : Parameters
	{
		public MySqlParameters()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedure_Parameters, 
					new object[]{this.Procedure.Database.Name, null, this.Procedure.Name});

				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
