using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Access
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
    /// <summary>
    /// Access数据库元数据信息
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabase))]
#endif 
	public class AccessDatabase : Database
    {
        internal string _name = "";
        internal string _desc = "";

		public AccessDatabase()
		{

		}

		override public string Name
		{
			get
			{
				return _name;
			}
		}

		override public string Alias
		{
			get
			{
				return _name;
			}
		}

		override public string Description
		{
			get
			{
				return _desc;
			}
		}

        protected override bool GetNativeType(OleDbType oledbType, int providerTypeInt, string dataType, int length, int numericPrecision, int numericScale, bool isLong, out string dbTypeName, out string dbTypeNameComplete)
        {
            bool rval = base.GetNativeType(oledbType, providerTypeInt, dataType, length, numericPrecision, numericScale, isLong, out dbTypeName, out dbTypeNameComplete);
            
            if (!rval)
            {
                if ((oledbType == OleDbType.VarChar) ||
                    (oledbType == OleDbType.VarWChar) ||
                    (oledbType == OleDbType.Char) ||
                    (oledbType == OleDbType.WChar) ||
                    (oledbType == OleDbType.VarWChar) ||
                    (oledbType == OleDbType.VarWChar) || 
                    (oledbType == OleDbType.BSTR))
                {
                    dbTypeName = "Text";
                    dbTypeNameComplete = dbTypeName + "(" + length + ")";
                }
                else if ((oledbType == OleDbType.LongVarChar) || (oledbType == OleDbType.LongVarWChar))
                {
                    dbTypeName = "Memo";
                    dbTypeNameComplete = dbTypeName;
                }
                else if (oledbType == OleDbType.LongVarBinary)
                {
                    dbTypeName = "LongBinary";
                    dbTypeNameComplete = dbTypeName;
                }
                else
                {
                    dbTypeName = "Variant";
                    dbTypeNameComplete = dbTypeName;
                }
                rval = true;

                foreach (IProviderType ptypeLoop in dbRoot.ProviderTypes)
                {
                    if (ptypeLoop.LocalType.Equals(dbTypeName, StringComparison.CurrentCultureIgnoreCase))
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
