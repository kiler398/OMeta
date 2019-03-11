using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Sql
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabase))]
#endif 
	public class SqlDatabase : Database
	{
		public SqlDatabase() {}

		internal static object DBSpecific(string key, Single single)
		{
			object retVal = null;
			DatabaseSpecific dext = new DatabaseSpecific();

			if (key == DatabaseSpecific.EXTENDED_PROPERTIES) 
			{
				if (single is IColumn) 
				{
					retVal = dext.ExtendedProperties(single as IColumn);
				}
				else if (single is ITable) 
				{
					retVal = dext.ExtendedProperties(single as ITable);
				}
				else if (single is IProcedure) 
				{
					retVal = dext.ExtendedProperties(single as IProcedure);
				}
				else if (single is IView) 
				{
					retVal = dext.ExtendedProperties(single as IView);
				}
			}
			
			return retVal;
        }
        protected override bool GetNativeType(OleDbType oledbType, int providerTypeInt, string dataType, int length, int numericPrecision, int numericScale, bool isLong, out string dbTypeName, out string dbTypeNameComplete)
        {
            bool rval = base.GetNativeType(oledbType, providerTypeInt, dataType, length, numericPrecision, numericScale, isLong, out dbTypeName, out dbTypeNameComplete);

            if (!rval || (oledbType == OleDbType.Char || oledbType == OleDbType.WChar))
            {
                if (oledbType == OleDbType.VarChar)
                {
                    dbTypeName = "varchar";
                    dbTypeNameComplete = dbTypeName + "(" + length + ")";
                }
                else if ((oledbType == OleDbType.VarWChar) || (oledbType == OleDbType.BSTR))
                {
                    dbTypeName = "nvarchar";
                    dbTypeNameComplete = dbTypeName + "(" + length + ")";
                }
                else if (oledbType == OleDbType.Char)
                {
                    dbTypeName = "char";
                    dbTypeNameComplete = dbTypeName + "(" + length + ")";
                }
                else if (oledbType == OleDbType.WChar)
                {
                    dbTypeName = "nchar";
                    dbTypeNameComplete = dbTypeName + "(" + length + ")";
                }
                else if (oledbType == OleDbType.LongVarChar)
                {
                    dbTypeName = "text";
                    dbTypeNameComplete = dbTypeName;
                }
                else if (oledbType == OleDbType.LongVarWChar)
                {
                    dbTypeName = "ntext";
                    dbTypeNameComplete = dbTypeName;
                }
                else if (oledbType == OleDbType.LongVarBinary)
                {
                    dbTypeName = "image";
                    dbTypeNameComplete = dbTypeName;
                }
                else if ((oledbType == OleDbType.LongVarBinary) || (oledbType == OleDbType.VarBinary))
                {
                    dbTypeName = "varbinary";
                    dbTypeNameComplete = dbTypeName;
                }
                else
                {
                    dbTypeName = "sql_variant";
                    dbTypeNameComplete = dbTypeName;
                }
                rval = true;

                foreach (IProviderType ptypeLoop in dbRoot.ProviderTypes)
                {
                    if (ptypeLoop.Type.Equals(dbTypeName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        dataTypeTables[providerTypeInt] = ptypeLoop;
                        break;
                    }
                }
            }

            return rval;
        }
	}
}
