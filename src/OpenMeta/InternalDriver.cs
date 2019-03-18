using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

using ADODB;

namespace OMeta
{
    public delegate string ShowOleDbDialogHandler(string connstring);
    public class InternalDriver
    {
        internal InternalDriver(Type factory, string connString, bool isOleDB)
        {
            this.factory = factory;
            this.IsOleDB = isOleDB;
            this.ConnectString = connString;
        }

        public ShowOleDbDialogHandler ShowOLEDBDialog;

        public string OnShowOLEDBDialog(string connstring)
        {
            if (ShowOLEDBDialog != null) return ShowOLEDBDialog(connstring);
            return connstring;
        }

        #region driver properties
        private bool isOleDB;

        private string connectString; // last connect string

        private string driverId;

        private Type factory;

        private bool stripTrailingNulls = false;

        private bool requiredDatabaseName = false;

        public bool RequiredDatabaseName
        {
            get { return requiredDatabaseName; }
            set { requiredDatabaseName = value; }
        }
	

        public bool StripTrailingNulls
        {
            get { return stripTrailingNulls; }
            set { stripTrailingNulls = value; }
        }
	

        public IClassFactory CreateBuildInClass()
        {
            if (factory.IsSubclassOf(typeof(IClassFactory)))
                return factory.Assembly.CreateInstance(factory.Name) as IClassFactory;
            return null;
        }

        public IOMetaPlugin CreateMyMetaPluginClass()
        {
            if (factory.IsSubclassOf(typeof(IOMetaPlugin)))
                return factory.Assembly.CreateInstance(factory.Name) as IOMetaPlugin;
            return null;
        }

        public string DriverId
        {
            get { return driverId; }
            private set { driverId = value; }
        }

        public virtual string ConnectString
        {
            get { return connectString; }
            set { connectString = value; }
        }
	
        public bool IsOleDB
        {
            get { return isOleDB; }
            protected set { isOleDB = value; }
        }

        public virtual string GetDataBaseName(IDbConnection con)
        {
            return con.Database;
        }
        #endregion

        public virtual string BrowseConnectionString(string connstr)
        {
            if (this.IsOleDB)
                return OnShowOLEDBDialog(connstr);
            return null;
        }

        #region driver mapping
        public static InternalDriver Register(string driverId, InternalDriver driver)
        {
            internalDrivers[driverId] = driver;
            driver.driverId = driverId;
            return driver;
        }

        private static Dictionary<string, InternalDriver> internalDrivers = new Dictionary<string, InternalDriver>();

        public static InternalDriver Get(string driverId)
        {
            InternalDriver result;
            if (internalDrivers.TryGetValue(driverId,out result))
                return result;
            return null;
        }
        #endregion
    }
}
