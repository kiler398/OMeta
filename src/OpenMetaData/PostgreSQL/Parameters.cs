using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.PostgreSQL
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameters))]
#endif 
	public class PostgreSQLParameters : Parameters
	{
		public PostgreSQLParameters()
		{

		}

		override internal void LoadAll()
		{
			try
			{
//				DataTable metaData = this.LoadData(OleDbSchemaGuid.Procedure_Parameters, 
//					new object[]{this.Procedure.Database.Name, this.Procedure.Schema, this.Procedure.Name});
//
//				PopulateArray(metaData);
			}
			catch {}
		}
	}
}
