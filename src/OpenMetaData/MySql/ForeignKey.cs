using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.MySql
{
 
	public class MySqlForeignKey : ForeignKey
	{
		public MySqlForeignKey()
		{

		}

		override public ITable ForeignTable
		{
			get
			{
				string catalog = this.ForeignKeys.Table.Database.Name;
				string schema  = this.GetString(ForeignKeys.f_FKTableSchema);

				return this.dbRoot.Databases[catalog].Tables[this.GetString(ForeignKeys.f_FKTableName)];
			}
		}
	}
}
