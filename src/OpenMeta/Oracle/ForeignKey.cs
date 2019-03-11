using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Oracle
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IForeignKey))]
#endif 
	public class OracleForeignKey : ForeignKey
	{
		public OracleForeignKey()
		{

		}

		override public ITable ForeignTable
		{
			get
			{
				string catalog = this.ForeignKeys.Table.Database.Name;
				string schema  = this.GetString(ForeignKeys.f_FKTableSchema);

				return this.dbRoot.Databases[schema].Tables[this.GetString(ForeignKeys.f_FKTableName)];
			}
		}
	}
}
