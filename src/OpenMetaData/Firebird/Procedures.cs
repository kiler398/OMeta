using System;
using System.Data;

using FirebirdSql.Data.FirebirdClient;

namespace OMeta.Firebird
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedures))]
#endif 
	public class FirebirdProcedures : Procedures
	{
		public FirebirdProcedures()
		{

		}

		override internal void LoadAll()
		{
			try
			{
				FbConnection cn = new FirebirdSql.Data.FirebirdClient.FbConnection(this._dbRoot.ConnectionString);
				cn.Open();
				DataTable metaData = cn.GetSchema("Procedures", new string[] {this.Database.Name});
				cn.Close();

				PopulateArray(metaData);
			}
			catch(Exception ex)
			{
				string m = ex.Message;
			}
		}

		override public IProcedure this[object name]
		{
			get
			{
				return base[name];
			}
		}
	}
}
