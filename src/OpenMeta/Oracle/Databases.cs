using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Oracle
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabases))]
#endif 
	public class OracleDatabases : Databases
	{
		public OracleDatabases()
		{

		}

		override internal void LoadAll()
		{
			DataTable metaData = this.LoadData(OleDbSchemaGuid.Schemata, new Object[] {null});

			PopulateArray(metaData);
		}

		internal override void PopulateSchemaData()
		{
			// we do nothing here
		}
	}
}
