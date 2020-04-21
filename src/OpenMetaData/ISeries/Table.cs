using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.ISeries
{
 
	public class ISeriesTable : Table
	{
		public ISeriesTable()
		{

		}

		public override IColumns PrimaryKeys
		{
			get
			{
				if(null == _primaryKeys)
				{
					string colName = "";

					_primaryKeys = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryKeys.Table = this;
					_primaryKeys.dbRoot = this.dbRoot;

					OleDbConnection cn = new OleDbConnection(this.dbRoot.ConnectionString); 
					cn.Open();
					OleDbCommand cmd = cn.CreateCommand();
					cmd.CommandText = 
@"SELECT c.CONSTRAINT_SCHEMA, c.CONSTRAINT_NAME, 
	cpk.CONSTRAINT_NAME as PK_CONSTRAINT_NAME, 
	c.TABLE_SCHEMA, c.TABLE_NAME, col.COLUMN_NAME
FROM SYSCST c, SYSCST cpk, SYSCSTCOL col
WHERE c.CONSTRAINT_SCHEMA = cpk.CONSTRAINT_SCHEMA
	AND c.CONSTRAINT_NAME = cpk.CONSTRAINT_NAME
	AND col.CONSTRAINT_SCHEMA = c.CONSTRAINT_SCHEMA
	AND col.CONSTRAINT_NAME = c.CONSTRAINT_NAME
	AND c.CONSTRAINT_TYPE = 'PRIMARY KEY'
	AND c.TABLE_SCHEMA = '" + this.Schema + @"' 
	AND c.TABLE_NAME = '" + this.Name + @"'
ORDER BY c.CONSTRAINT_SCHEMA, c.CONSTRAINT_NAME";

					OleDbDataReader reader = cmd.ExecuteReader();
					while(reader.Read()) 
					{
						colName = reader["COLUMN_NAME"].ToString();
						Column column = (Column)this.Columns[colName];
						_primaryKeys.AddColumn(column);
					}
					reader.Close();
					cn.Close();
				}

				return _primaryKeys;
			}
		}
	}
}
