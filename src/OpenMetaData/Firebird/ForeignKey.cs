using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace OMeta.Firebird
{
 
	public class FirebirdForeignKey : ForeignKey
	{
		public FirebirdForeignKey()
		{

		}

		override public ITable ForeignTable
		{
			get
			{
				string catalog = this.ForeignKeys.Table.Database.Name;
				string schema  = this.GetString(ForeignKeys.f_FKTableSchema);

				return this.dbRoot.Databases[0].Tables[this.GetString(ForeignKeys.f_FKTableName)];
			}
		}
	}
}
