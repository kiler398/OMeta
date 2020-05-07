using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Collections;
using System.Reflection;

using System.Diagnostics;

using Npgsql;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SQLite;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using MySql.Data.MySqlClient;
using OMeta.Interfaces;
using ClassFactory = OMeta.Sql.ClassFactory;

namespace OMeta
{
 

    /// <summary>
    /// OMeta is the root of the OMeta meta-data. OMeta is an intrinsic object available to your script and configured based on the settings
    /// you have entered in the Default Settings dialog. It is already connected before you script execution begins.
    /// </summary>
    /// <remarks>
    ///	OMeta has 1 Collection:
    /// <list type="table">
    ///		<item><term>Databases</term><description>Contains a collection of all of the databases in your system</description></item>
    ///	</list>
    /// There is a property collection on every entity in your database, you can add key/value
    /// pairs to the User Meta Data either through the user interface of MyGeneration or 
    /// programmatically in your scripts.  User meta data is stored in XML and never writes to your database.
    ///
    /// This can be very useful, you might need more meta data than OMeta supplies, in fact,
    /// OMeta will eventually offer extended meta data using this feature as well. The current plan
    /// is that any extended data added via MyGeneration will have a key that beings with "OMeta.Something"
    /// where 'Something' equals the description. 
    /// </remarks>
    /// <example>
    ///	VBScript - ****** NOTE ****** You never have to actually write this code, this is for education purposes only.
    ///	<code>
    ///	OMeta.Connect "SQL", "Provider=SQLOLEDB.1;Persist Security Info=True;User ID=sa;Data Source=localhost"
    ///	
    ///	OMeta.DbTarget	= "SqlClient"
    ///	OMeta.DbTargetMappingFileName = "C:\Program Files\MyGeneration\Settings\DbTargets.xml"
    ///	
    /// OMeta.Language = "VB.NET"
    /// OMeta.LanguageMappingFileName = "C:\Program Files\MyGeneration\Settings\Languages.xml"
    /// 
    /// OMeta.UserMetaDataFileName = "C:\Program Files\MyGeneration\Settings\UserMetaData.xml"
    /// </code>
    ///	JScript - ****** NOTE ****** You never have to actually write this code, this is for education purposes only.
    ///	<code>
    ///	OMeta.Connect("SQL", "Provider=SQLOLEDB.1;Persist Security Info=True;User ID=sa;Data Source=localhost")
    ///	
    ///	OMeta.DbTarget	= "SqlClient";
    ///	OMeta.DbTargetMappingFileName = "C:\Program Files\MyGeneration\Settings\DbTargets.xml";
    ///	
    /// OMeta.Language = "VB.NET";
    /// OMeta.LanguageMappingFileName = "C:\Program Files\MyGeneration\Settings\Languages.xml";
    /// 
    /// OMeta.UserMetaDataFileName = "C:\Program Files\MyGeneration\Settings\UserMetaData.xml";
    /// </code>
    /// The above code is done for you long before you execute your script and the values come from the Default Settings Dialog.
    /// However, you can override these defaults as many of the sample scripts do. For instance, if you have a script that is for SqlClient
    /// only go ahead and set the OMeta.DbTarget in your script thus overriding the Default Settings.
    /// </example>
 
	public class dbRoot
    {
		public dbRoot()
		{
            Advantage.ClassFactory.Register();
            DB2.ClassFactory.Register();
            Firebird.ClassFactory.Register();
            ISeries.ClassFactory.Register();
            MySql.ClassFactory.Register();
            MySql5.ClassFactory.Register();
            Oracle.ClassFactory.Register();
            //Pervasive.ClassFactory.Register();
            PostgreSQL.ClassFactory.Register();
            PostgreSQL8.ClassFactory.Register();
            Sql.ClassFactory.Register();
            SQLite.ClassFactory.Register();
 
			Reset();
        }

        public void SetDbTarget(string dbTarget,string dbTargetMappingFileName = null)
        {
            if (string.IsNullOrEmpty(dbTargetMappingFileName))
            {
                var embeddedFileProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
                var fileInfo = embeddedFileProvider.GetFileInfo("Config/DbTargets.xml");
                _dbTargetDoc = new XmlDocument();
                _dbTargetDoc.Load(fileInfo.CreateReadStream());
                _dbTarget = string.Empty; ;
                _dbTargetNode = null;
            }
            else
            {
                this.DbTargetMappingFileName = dbTargetMappingFileName;
            }
            this.DbTarget = dbTarget;
        }
 
        public void SetCodeLanguage(string language, string languageMappingFileName = null)
        {
            if (string.IsNullOrEmpty(languageMappingFileName))
            {
                var embeddedFileProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
                var fileInfo = embeddedFileProvider.GetFileInfo("Config/Languages.xml");
                _languageDoc = new XmlDocument();
                _languageDoc.Load(fileInfo.CreateReadStream());
                _language = string.Empty; ;
                _languageNode = null;
            }
            else
            {
                this.LanguageMappingFileName = languageMappingFileName;
            }
            this.Language = language;
        }

 

        private void Reset()
		{
			UserData = null;

			IgnoreCase = true;
			requiredDatabaseName = false;
			requiresSchemaName = false;
			StripTrailingNulls = false;
			TrailingNull = ((char)0x0).ToString();

			ClassFactory = null;

            _showSystemData = false;
            _showDefaultDatabaseOnly = false;

			_driver = dbDriver.None;
			_driverString = "NONE";
			_databases = null;
			_connectionString = "";
			_theConnection = new OleDbConnection();
			_isConnected = false;
			_parsedConnectionString = null;
            _defaultDatabase = "";
            _lastConnectionException = null;
            _lastConnectionError = string.Empty;

			// Language
			_languageMappingFileName = string.Empty;
			_language = string.Empty;
			_languageDoc = null;
			_languageNode = null;

			UserData = new XmlDocument();
			UserData.AppendChild(UserData.CreateNode(XmlNodeType.Element, "OMeta", null));

			// DbTarget
			_dbTargetMappingFileName = string.Empty;
			_dbTarget = string.Empty;
			_dbTargetDoc = null;
			_dbTargetNode = null;
		}

        public object DriverSpecificData(string providerName, string key)
        {
            object o = null;
            if (Plugins.ContainsKey(providerName))
            {
                o = (Plugins[providerName] as IOMetaPlugin).GetDatabaseSpecificMetaData(null, key);
            }
            return o;
        }

		#region Properties

		/// <summary>
		/// Contains all of the databases in your DBMS system.
		/// </summary>
		public IDatabases Databases
		{
			get
			{
				if(null == _databases)
				{
					if(this.ClassFactory != null)
                    {
                        _databases = ClassFactory.CreateDatabases() as Databases;
                        _databases.dbRoot = this;

                        if (this.ShowDefaultDatabaseOnly)
                        {
                            _databases.LoadDefault();
                        }
                        else
                        {
                            _databases.LoadAll();
                        }
					}
				}

				return _databases;
			}
		}

        public string DefaultDatabaseName
        {
            get
            {
                return _defaultDatabase;
            }
        }

		/// <summary>
		/// This is the default database as defined in your connection string, or if not provided your DBMS system may provide one.
		/// Finally, for single database systems like Microsoft Access it will be the default database.
		/// </summary>
        public IDatabase DefaultDatabase
        {
            get
            {
                IDatabase defDatabase = null;
                Databases dbases = this.Databases as Databases;

                if (this._defaultDatabase != null && this._defaultDatabase != "")
                {
                    try
                    {
                        defDatabase = dbases.GetByName(this._defaultDatabase);
                    }
                    catch { }

                    if (defDatabase == null)
                    {
                        try
                        {
                            defDatabase = dbases.GetByPhysicalName(this._defaultDatabase);
                        }
                        catch { }
                    }
                }

                if (defDatabase == null)
                {
                    if (dbases.Count == 1)
                    {
                        defDatabase = dbases[0];
                    }
                }

                return defDatabase;
            }
        }

		public IProviderTypes ProviderTypes
		{
			get
			{
				if(null == _providerTypes)
				{
					_providerTypes = (ProviderTypes)ClassFactory.CreateProviderTypes();
					_providerTypes.dbRoot = this;
					_providerTypes.LoadAll();
				}

				return _providerTypes;
			}
		}

		#endregion

		#region Connection 
 
		public IDbConnection BuildConnection(string driver, string connectionString) 
		{
			IDbConnection conn = null;
			switch(driver.ToUpper())
			{
                case OMetaDrivers.MySql2:
                    conn = new MySqlConnection(connectionString);
					break;

                case OMetaDrivers.PostgreSQL:
                case OMetaDrivers.PostgreSQL8:
					conn = new Npgsql.NpgsqlConnection(connectionString);
					break;

                case OMetaDrivers.Firebird:
                case OMetaDrivers.Interbase:
					conn = new FirebirdSql.Data.FirebirdClient.FbConnection(connectionString);
                    break;

                case OMetaDrivers.SQLite:
                    conn = new SQLiteConnection(connectionString);
                    break;
                default:
                    if (Plugins.ContainsKey(driver))
                    {
                        conn = this.GetConnectionFromPlugin(driver, connectionString);
                    }
                    else
                    {
                        conn = new OleDbConnection(connectionString);
                    }
					break;
			}
			return conn;
        }

		/// <summary>
		/// This is how you connect to your DBMS system using OMeta. This is already called for you before your script beings execution.
		/// </summary>
		/// <param name="driver">A string as defined in the remarks below</param>
		/// <param name="connectionString">A valid connection string for you DBMS</param>
		/// <returns>True if connected, False if not</returns>
		/// <remarks>
		/// These are the supported "drivers".
		/// <list type="table">
		///		<item><term>"ACCESS"</term><description>Microsoft Access 97 and higher</description></item>
		///		<item><term>"DB2"</term><description>IBM DB2</description></item>	
		///		<item><term>"MYSQL"</term><description>Currently limited to only MySQL running on Microsoft Operating Systems</description></item>
		///		<item><term>"MYSQL2"</term><description>Uses MySQL Connector/Net, Supports 4.x schema info on Windows or Linux</description></item>
		///		<item><term>"ORACLE"</term><description>Oracle 8i - 9</description></item>
		///		<item><term>"SQL"</term><description>Microsoft SQL Server 2000 and higher</description></item>	
		///		<item><term>"PERVASIVE"</term><description>Pervasive 9.00+ (might work on lower but untested)</description></item>		
		///		<item><term>"POSTGRESQL"</term><description>PostgreSQL 7.3+ (might work on lower but untested)</description></item>		
		///		<item><term>"POSTGRESQL8"</term><description>PostgreSQL 8.0+</description></item>	
		///		<item><term>"FIREBIRD"</term><description>Firebird</description></item>		
		///		<item><term>"INTERBASE"</term><description>Borland's InterBase</description></item>		
		///		<item><term>"SQLITE"</term><description>SQLite</description></item>		
		///		<item><term>"VISTADB"</term><description>VistaDB Database</description></item>		
        ///		<item><term>"ADVANTAGE"</term><description>Advantage Database Server</description></item>	
        ///		<item><term>"ISERIES"</term><description>iSeries (AS400)</description></item>	
		///	</list>
		/// Below are some sample connection strings. However, the "Data Link" dialog available on the Default Settings dialog can help you.
		/// <list type="table">
		///		<item><term>"ACCESS"</term><description>Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\access\newnorthwind.mdb;User Id=;Password=</description></item>
		///		<item><term>"DB2"</term><description>Provider=IBMDADB2.1;Password=sa;User ID=DB2Admin;Data Source=OMeta;Persist Security Info=True</description></item>	
		///		<item><term>"MYSQL"</term><description>Provider=MySQLProv;Persist Security Info=True;Data Source=test;UID=griffo;PWD=;PORT=3306</description></item>
		///		<item><term>"MYSQL2"</term><description>Uses Database=Test;Data Source=Griffo;User Id=anonymous;</description></item>
		///		<item><term>"ORACLE"</term><description>Provider=OraOLEDB.Oracle.1;Password=sa;Persist Security Info=True;User ID=GRIFFO;Data Source=dbMeta</description></item>
		///		<item><term>"SQL"</term><description>Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=Northwind;Data Source=localhost</description></item>
		///		<item><term>"PERVASIVE"</term><description>Provider=PervasiveOLEDB.8.60;Data Source=demodata;Location=Griffo;Persist Security Info=False</description></item>		
		///		<item><term>"POSTGRESQL"</term><description>Server=www.myserver.com;Port=5432;User Id=myuser;Password=aaa;Database=mygeneration;</description></item>		
		///		<item><term>"POSTGRESQL8"</term><description>Server=www.myserver.com;Port=5432;User Id=myuser;Password=aaa;Database=mygeneration;</description></item>		
		///		<item><term>"FIREBIRD"</term><description>Database=C:\firebird\EMPLOYEE.GDB;User=SYSDBA;Password=wow;Dialect=3;Server=localhost</description></item>		
		///		<item><term>"INTERBASE"</term><description>Database=C:\interbase\EMPLOYEE.GDB;User=SYSDBA;Password=wow;Dialect=3;Server=localhost</description></item>		
		///		<item><term>"SQLITE"</term><description>Data Source=C:\SQLite\employee.db;New=False;Compress=True;Synchronous=Off;Version=3</description></item>		
		///		<item><term>"VISTADB"</term><description>DataSource=C:\Program Files\VistaDB 2.0\Data\Northwind.vdb</description></item>		
		///		<item><term>"ADVANTAGE"</term><description>Provider=Advantage.OLEDB.1;Password="";User ID=AdsSys;Data Source=C:\task1;Initial Catalog=aep_tutorial.add;Persist Security Info=True;Advantage Server Type=ADS_LOCAL_SERVER;Trim Trailing Spaces=TRUE</description></item>		
        ///		<item><term>"ISERIES"</term><description>PROVIDER=IBMDA400; DATA SOURCE=MY_SYSTEM_NAME;USER ID=myUserName;PASSWORD=myPwd;DEFAULT COLLECTION=MY_LIBRARY;</description></item>		
		///	</list>
		///	</remarks>
        public bool Connect(string driverIn, string connectionString)
        {
            string driver = driverIn.ToUpper();
            switch (driver)
            {
                case OMetaDrivers.None:
                    return true;
                case OMetaDrivers.SQL:
                case OMetaDrivers.Oracle:
                //case OMetaDrivers.Access:
                case OMetaDrivers.MySql:
                case OMetaDrivers.MySql2:
                case OMetaDrivers.DB2:
                case OMetaDrivers.ISeries:
                //case OMetaDrivers.Pervasive:
                case OMetaDrivers.PostgreSQL:
                case OMetaDrivers.PostgreSQL8:
                case OMetaDrivers.Firebird:
                case OMetaDrivers.Interbase:
                case OMetaDrivers.SQLite:
                case OMetaDrivers.Advantage:
                    return this.Connect(
                        OMetaDrivers.GetDbDriverFromName(driver),
                        driver,
                        connectionString);
                default:
                    if (Plugins.ContainsKey(driver))
                    {
                        return this.Connect(dbDriver.Plugin, driver, connectionString);
                    }
                    else
                    {
                        return false;
                    }
            }
        }

        /// <summary>
		/// Same as <see cref="Connect(string, string)"/>(string, string) only this uses an enumeration.  
		/// </summary>
		/// <param name="driver">The driver enumeration for you DBMS system</param>
		/// <param name="connectionString">A valid connection string for you DBMS</param>
		/// <returns></returns>
        public bool Connect(dbDriver driver, string connectionString)
        {
            return Connect(driver, string.Empty, connectionString);
        }

		/// <summary>
		/// Same as <see cref="Connect(string, string)"/>(string, string) only this uses an enumeration.  
        /// </summary>
        /// <param name="driver">The driver enumeration for you DBMS system</param>
        /// <param name="pluginName">The name of the plugin</param>
		/// <param name="connectionString">A valid connection string for you DBMS</param>
		/// <returns></returns>
		public bool Connect(dbDriver driver, string pluginName, string connectionString)
		{
			Reset();

            try
            {
                string dbName;
                int index;

                this._connectionString = connectionString.Replace("\"", "");
                this._driver = driver;

                switch (_driver)
                {
                    case dbDriver.SQL:

                        ConnectUsingOleDb(_driver, _connectionString);
                        this._driverString = OMetaDrivers.SQL;
                        this.StripTrailingNulls = false;
                        this.requiredDatabaseName = true;
                        ClassFactory = new Sql.ClassFactory();
                        break;

                    case dbDriver.Oracle:

                        ConnectUsingOleDb(_driver, _connectionString);
                        this._driverString = OMetaDrivers.Oracle;
                        this.StripTrailingNulls = false;
                        this.requiredDatabaseName = true;
                        ClassFactory = new Oracle.ClassFactory();
                        break;
 
                    case dbDriver.MySql:

                        ConnectUsingOleDb(_driver, _connectionString);
                        this._driverString = OMetaDrivers.MySql;
                        this.StripTrailingNulls = true;
                        this.requiredDatabaseName = true;
                        ClassFactory = new MySql.ClassFactory();
                        break;

                    case dbDriver.MySql2:

                        using (MySqlConnection mysqlconn = new MySqlConnection(_connectionString))
                        {
                            mysqlconn.Close();
                            mysqlconn.Open();
                            this._defaultDatabase = mysqlconn.Database;
                        }

                        this._driverString = OMetaDrivers.MySql2;
                        this.StripTrailingNulls = true;
                        this.requiredDatabaseName = true;
                        ClassFactory = new MySql5.ClassFactory();
                        break;

                    case dbDriver.DB2:

                        ConnectUsingOleDb(_driver, _connectionString);
                        this._driverString = OMetaDrivers.DB2;
                        this.StripTrailingNulls = false;
                        this.requiredDatabaseName = false;
                        ClassFactory = new DB2.ClassFactory();
                        break;

                    case dbDriver.ISeries:

                        ConnectUsingOleDb(_driver, _connectionString);
                        this._driverString = OMetaDrivers.ISeries;
                        this.StripTrailingNulls = false;
                        this.requiredDatabaseName = false;
                        ClassFactory = new ISeries.ClassFactory();
                        break;
 
                    case dbDriver.PostgreSQL:

                        using (NpgsqlConnection cn = new Npgsql.NpgsqlConnection(_connectionString))
                        {
                            cn.Open();
                            this._defaultDatabase = cn.Database;
                        }

                        this._driverString = OMetaDrivers.PostgreSQL;
                        this.StripTrailingNulls = false;
                        this.requiredDatabaseName = false;
                        ClassFactory = new PostgreSQL.ClassFactory();
                        break;

                    case dbDriver.PostgreSQL8:

                        using (NpgsqlConnection cn8 = new Npgsql.NpgsqlConnection(_connectionString))
                        {
                            cn8.Open();
                            this._defaultDatabase = cn8.Database;
                        }

                        this._driverString = OMetaDrivers.PostgreSQL8;
                        this.StripTrailingNulls = false;
                        this.requiredDatabaseName = false;
                        ClassFactory = new PostgreSQL8.ClassFactory();
                        break;

                    case dbDriver.Firebird:

                        using (FbConnection cn1 = new FirebirdSql.Data.FirebirdClient.FbConnection(_connectionString))
                        {
                            cn1.Open();
                            dbName = cn1.Database;
                        }

                        try
                        {
                            index = dbName.LastIndexOfAny(new char[] { '\\' });
                            if (index >= 0)
                            {
                                this._defaultDatabase = dbName.Substring(index + 1);
                            }
                        }
                        catch { }

                        this._driverString = OMetaDrivers.Firebird;
                        this.StripTrailingNulls = false;
                        this.requiredDatabaseName = false;
                        ClassFactory = new Firebird.ClassFactory();
                        break;

                    case dbDriver.Interbase:

                        using (FbConnection cn2 = new FirebirdSql.Data.FirebirdClient.FbConnection(_connectionString))
                        {
                            cn2.Open();
                            this._defaultDatabase = cn2.Database;
                        }

                        this._driverString = OMetaDrivers.Interbase;
                        this.StripTrailingNulls = false;
                        this.requiredDatabaseName = false;
                        ClassFactory = new Firebird.ClassFactory();
                        break;

                    case dbDriver.SQLite:

                        using (SQLiteConnection sqliteConn = new SQLiteConnection(_connectionString))
                        {
                            sqliteConn.Open();
                            dbName = sqliteConn.Database;

                            if (!string.IsNullOrEmpty(dbName)) this._defaultDatabase = dbName;
                        }
                        this._driverString = OMetaDrivers.SQLite;
                        this.StripTrailingNulls = false;
                        this.requiredDatabaseName = false;
                        ClassFactory = new SQLite.ClassFactory();
                        break;
                    case dbDriver.Advantage:

                        ConnectUsingOleDb(_driver, _connectionString);
                        this._driverString = OMetaDrivers.Advantage;
                        this.StripTrailingNulls = false;
                        this.requiredDatabaseName = false;
                        ClassFactory = new Advantage.ClassFactory();
                        string[] s = this._defaultDatabase.Split('.');
                        this._defaultDatabase = s[0];
                        break;

                    case dbDriver.Plugin:

                        IOMetaPlugin plugin;
                        using (IDbConnection connection = this.GetConnectionFromPlugin(pluginName, _connectionString, out plugin))
                        {
                            if (connection != null)
                                connection.Open();
                            dbName = connection.Database;
                            if (!string.IsNullOrEmpty(plugin.DefaultDatabase) || string.IsNullOrEmpty(dbName)) dbName = plugin.DefaultDatabase;
                            if (!string.IsNullOrEmpty(dbName)) this._defaultDatabase = dbName;
                        }
                        this._driverString = pluginName;
                        //this.StripTrailingNulls = plugin.StripTrailingNulls;
                        //this.requiredDatabaseName = plugin.RequiredDatabaseName;
                        ClassFactory = new Plugin.ClassFactory(plugin);
                        break;

                    case dbDriver.None:

                        this._driverString = OMetaDrivers.None;
                        break;
                }
            }
            catch (OleDbException ex)
            {
                this._lastConnectionException = ex;
                foreach (OleDbError error in ex.Errors)
                {
                    if (this._lastConnectionError != string.Empty) this._lastConnectionError += Environment.NewLine;
                    this._lastConnectionError += ex;
                }
            }
            catch (Exception ex)
            {
                this._lastConnectionException = ex;
                this._lastConnectionError = ex.Message;
            }

			_isConnected = true;
			return true;
		}

        private void ConnectUsingOleDb(dbDriver driver, string connectionString)
        {
            OleDbConnection cn = new OleDbConnection(connectionString.Replace("\"", ""));
            cn.Open();
            this._defaultDatabase = GetDefaultDatabase(cn, driver);
            cn.Close();
        }

        public string LastConnectionError
        {
            get
            {
                return _lastConnectionError;
            }
        }

 
        public Exception LastConnectionException
        {
            get
            {
                return _lastConnectionException;
            }
        }
 
        public System.Collections.Generic.Dictionary<string, string> UserDataDatabaseMappings
        {
            get
            {
                if (_userDataDatabaseMappings == null) 
                    _userDataDatabaseMappings = new System.Collections.Generic.Dictionary<string,string>();

                return _userDataDatabaseMappings;
            }
        }

        [ComVisible(false)]
        public void ChangeDatabase(IDbConnection connection, string database)
        {
            if (this.ClassFactory != null)
                this.ClassFactory.ChangeDatabase(connection, database);
        }

		internal OleDbConnection TheConnection
		{
			get
			{
				if(this._theConnection.State != ConnectionState.Open)
				{
					this._theConnection.ConnectionString = this._connectionString;
					this._theConnection.Open();
				}

				return this._theConnection;
			}
		}

		private string GetDefaultDatabase(OleDbConnection cn, dbDriver driver)
		{
			string databaseName = string.Empty;

			switch(driver)
			{
				//case dbDriver.Access:

				//	int i = cn.DataSource.LastIndexOf(@"\");

				//	if(i == -1) 
				//		databaseName = cn.DataSource;
				//	else
				//		databaseName = cn.DataSource.Substring(++i);

				//	break;

				default:

					databaseName = cn.Database;
					break;
			}

			return databaseName;
		}

		/// <summary>
		/// True if OMeta has been successfully connected to your DBMS, False if not.
		/// </summary>
		public bool IsConnected
		{
			get
			{
				return  _isConnected;
			}
		}

		/// <summary>
		/// Returns OMeta's current dbDriver enumeration value as defined by its current connection.
		/// </summary>
		public dbDriver Driver
		{
			get
			{
				return _driver;
			}
		}

		/// <summary>
		/// Returns OMeta's current DriverString as defined by its current connection.
		/// </summary>
		/// <remarks>
		/// These are the current possible values.
		/// <list type="table">
		///		<item><term>"ACCESS"</term><description>Microsoft Access 97 and higher</description></item>
		///		<item><term>"DB2"</term><description>IBM DB2</description></item>	
		///		<item><term>"MYSQL"</term><description>Currently limited to only MySQL running on Microsoft Operating Systems</description></item>
		///		<item><term>"ORACLE"</term><description>Oracle 8i - 9</description></item>
		///		<item><term>"SQL"</term><description>Microsoft SQL Server 2000 and higher</description></item>	
		///		<item><term>"PostgreSQL"</term><description>PostgreSQL</description></item>	///		
		///	</list>
		///	</remarks>
		public string DriverString
		{
			get
			{
				return _driverString;
			}
		}

		/// <summary>
		/// Returns the current connection string. ** WARNING ** Currently the password is returned, the password will be stripped from this
		/// property in the very near future.
		/// </summary>
		public string ConnectionString
		{
			get
			{
				return _connectionString;
			}
		}

		internal Hashtable ParsedConnectionString
		{
			get
			{
                //Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=Northwind;Data Source=localhost
				if(null == _parsedConnectionString)
				{
					string[] tokens = ConnectionString.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

                    _parsedConnectionString = new Hashtable(tokens.Length);

					string[] kv = null;

                    for (int i = 0; i < tokens.Length; i++)
					{
                        kv = tokens[i].Split('=');

						if (kv.Length == 2)
						{
							_parsedConnectionString.Add(kv[0].ToUpper(), kv[1]);
						}
						else if (kv.Length == 1)
						{
							_parsedConnectionString.Add(kv[0].ToUpper(), string.Empty);
						}
					}
				}

				return _parsedConnectionString;
			}
		}

		#endregion

		#region Settings

		/// <summary>
		/// Determines whether system tables and views and alike are shown, the default is False. If True, ONLY system data is shown.
		/// </summary>
		public bool ShowSystemData
		{
			get	{ return _showSystemData;   }
			set	{ _showSystemData = value ; }
        }

        /// <summary>
        /// Only show the default database in the databases collection.
        /// </summary>
        public bool ShowDefaultDatabaseOnly
        {
            get { return _showDefaultDatabaseOnly; }
            set 
            {
                if (_showDefaultDatabaseOnly != value)
                {
                    this._databases = null;
                }
                _showDefaultDatabaseOnly = value; 
            }
        }

		/// <summary>
		/// If this is true then four IColumn properties are actually supplied by the Domain, if the Column has an IDomain. 
		/// The four properties are DataTypeName, DataTypeNameComplete, LanguageType, and DbTargetType.
		/// </summary>
		public bool DomainOverride
		{
			get	{ return _domainOverride;   }
			set	{ _domainOverride = value ; }
		}

		#endregion

        #region Plugin Members
        /// <summary>
        /// If you want to fetch data specific to a given plugin, you can get generic info here.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object PluginSpecificData(string providerName, string key)
        {
            object o = null;
            try
            {
                if (Plugins.ContainsKey(providerName))
                {
                    o = (Plugins[providerName] as IOMetaPlugin).GetDatabaseSpecificMetaData(null, key);
                }
            }
            catch { }
            return o;
        }

        /// <summary>
        /// A Plugin ConnectionString is a special feature for external assemblies.
        /// </summary>
        /// <param name="connectionString">Sample: PluginName;Provider=SQLOLEDB.1;Persist Security Info=True;User ID=sa;Data Source=localhost</param>
        /// <returns></returns>
        private IDbConnection GetConnectionFromPlugin(string providerName, string pluginConnectionString)
        {
            IOMetaPlugin plugin;

            return GetConnectionFromPlugin(providerName, pluginConnectionString, out plugin);
        }

        /// <summary>
        /// A Plugin ConnectionString is a special feature for external assemblies.
        /// </summary>
        /// <param name="pluginConnectionString">Sample: PluginName;Provider=SQLOLEDB.1;Persist Security Info=True;User ID=sa;Data Source=localhost</param>
        /// <param name="plugin">Returns the plugin object.</param>
        /// <returns></returns>
        private IDbConnection GetConnectionFromPlugin(string providerName, string pluginConnectionString, out IOMetaPlugin plugin)
        {
            OMetaPluginContext pluginContext = new OMetaPluginContext(providerName, pluginConnectionString);

            IDbConnection connection = null;
            if (!Plugins.ContainsKey(providerName))
            {
                throw new Exception("OMeta Plugin \"" + providerName + "\" not registered.");
            }
            else
            {
                plugin = Plugins[providerName] as IOMetaPlugin;
                plugin.Initialize(pluginContext);

                connection = plugin.NewConnection;
            }

            return connection;
        }

        private static Hashtable plugins;

        [ComVisible(false)]
        public static Hashtable Plugins
        {
            get
            {
                if (plugins == null)
                {
                    plugins = new Hashtable();
                    FileInfo info = new FileInfo(Assembly.GetCallingAssembly().Location);
                    if (info.Exists)
                    {
                    	StringBuilder fileNames = new StringBuilder();
                    	Exception err = null;

#if PLUGINS_FROM_SUBDIRS
                        // k3b allow plugins to be in its own directory
                        foreach (FileInfo dllFile in info.Directory.GetFiles("OMeta.Plugins.*.dll",SearchOption.AllDirectories))
#else                        
                        foreach (FileInfo dllFile in info.Directory.GetFiles("OMeta.Plugins.*.dll"))
#endif                        
                        {
                        	try
                        	{
                        		loadPlugin(dllFile.FullName, plugins);
                        	} catch(Exception ex) {
                        		// Fix K3b 2007-06-27 if the current plugin cannot be loaded ignore it.
                        		//			i got the exception when loading a plugin that was linked against an old Interface
                        		//			the chatch ensures that the rest of the application-initialisation continues ...
                        		fileNames.AppendLine(dllFile.FullName);
                        		err = ex;
                        	}
                        }
                        
                        //TODO How to tell the caller that something is not ok. A Exception would result in a incomplete initialisation
//                        if (err != null)
//                        	throw new ApplicationException("Cannot load Plugin(s) " + fileNames.ToString(), err);
                    }
                    
                }

                return plugins;
            }
        }
        
		private static void loadPlugin(string filename, Hashtable plugins)
		{
#if PLUGINS_FROM_SUBDIRS
            if (System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) != System.IO.Path.GetDirectoryName(filename))
                AppDomain.CurrentDomain.AppendPrivatePath(System.IO.Path.GetDirectoryName(filename));
#endif

            Assembly assembly = Assembly.LoadFile(filename);

            foreach (Type type in assembly.GetTypes())
            {
                Type[] interfaces = type.GetInterfaces();
                foreach (Type iface in interfaces)
                {
                    if (iface == typeof(IOMetaPlugin))
                    {
                        try
                        {
                            ConstructorInfo[] constructors = type.GetConstructors();
                            ConstructorInfo constructor = constructors[0];

                            IOMetaPlugin plugin = constructor.Invoke(BindingFlags.CreateInstance, null, new object[] { }, null) as IOMetaPlugin;
                            InternalDriver.Register(plugin.ProviderUniqueKey,
                                                    new PluginDriver(plugin));

                            plugins[plugin.ProviderUniqueKey] = plugin; // after register because if exception in register, donot remeber plugin
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine("Cannot load plugin " + filename);
                            while (ex != null)
                            {
                                System.Diagnostics.Trace.WriteLine(ex.Message);
                                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                                ex = ex.InnerException;
                            }
                        }
                    }
                }
            }
		}

        static Module assembly_ModuleResolve(object sender, ResolveEventArgs e)
        {
            // throw new Exception("The method or operation is not implemented.");
            return null;
        }

        #endregion
        
        #region XML User Data

		public string UserDataXPath
		{ 
			get
			{
				return @"//OMeta";
			} 
		}

		internal bool GetXmlNode(out XmlNode node, bool forceCreate)
		{
			node = null;
			bool success = false;

			if(null == _xmlNode)
			{
				if(!UserData.HasChildNodes)
				{
					_xmlNode = UserData.CreateNode(XmlNodeType.Element, "OMeta", null);
					UserData.AppendChild(_xmlNode);
				}
				else
				{
					_xmlNode = UserData.SelectSingleNode("./OMeta");
				}
			}

			if(null != _xmlNode)
			{
				node = _xmlNode;
				success = true;
			}

			return success;
		}

		/// <summary>
		/// The full path of the XML file that contains the user defined meta data. See IPropertyCollection
		/// </summary>
		public string UserMetaDataFileName
		{
			get	{ return _userMetaDataFileName; }
			set
			{
				_userMetaDataFileName = value;

				try
				{
					UserData = new XmlDocument();
					UserData.Load(_userMetaDataFileName);
				}
				catch 
				{
					UserData = new XmlDocument();
				}
			}
		}

		/// <summary>
		/// Call this method to save any user defined meta data that you may have modified. See <see cref="UserMetaDataFileName"/>
		/// </summary>
		/// <returns>True if saved, False if not</returns>
		public bool SaveUserMetaData()
		{
            if (null != UserData && string.Empty != _userMetaDataFileName)
            {
                FileInfo f = new FileInfo(_userMetaDataFileName);
                if (!f.Exists)
                {
                    if (!f.Directory.Exists) f.Directory.Create();
                }

                UserData.Save(_userMetaDataFileName);
                return true;
            }

			return false;
		}

		private string _userMetaDataFileName = "";

        public static void MergeUserMetaDataFiles(string f1, string db1, string f2, string db2, string fm) 
        {
            FileInfo f1Inf = new FileInfo(f1);
            FileInfo f2Inf = new FileInfo(f2);
            FileInfo fmInf = new FileInfo(fm);
            if (fmInf.Exists) fmInf.Delete();
            fmInf = f1Inf.CopyTo(fmInf.FullName);

            XmlDocument newDoc = new XmlDocument();
            newDoc.Load(fmInf.FullName);
            XmlNode newDocRoot = newDoc.SelectSingleNode("//OMeta/Databases/Database[@p='" + db1 + "']");

            XmlDocument otherDoc = new XmlDocument();
            otherDoc.Load(f2Inf.FullName);
            XmlNode otherDocRoot = otherDoc.SelectSingleNode("//OMeta/Databases/Database[@p='" + db2 + "']");

            MergeXml(newDoc, newDocRoot, "//OMeta/Databases/Database[@p='" + db1 + "']", otherDoc, otherDocRoot, "//OMeta/Databases/Database[@p='" + db2 + "']");

            newDoc.Save(fmInf.FullName);
        }

        private static void MergeXml(XmlDocument d1, XmlNode n1, string xpath1, XmlDocument d2, XmlNode n2, string xpath2)
        {
            System.Collections.Generic.Dictionary<string, XmlNode> ch1nodes = new System.Collections.Generic.Dictionary<string,XmlNode>();
            foreach (XmlNode ch1 in n1.ChildNodes)
            {
                ch1nodes[GetXmlNodeKey(ch1)] = ch1;
            }

            foreach (XmlNode ch2 in n2.ChildNodes)
            {
                string ch2Key = GetXmlNodeKey(ch2);

                XmlNode ch1 = null;
                if (ch1nodes.ContainsKey(ch2Key))
                {
                    ch1 = ch1nodes[ch2Key];

                    if (ch2.Attributes["n"] != null && ch1.Attributes["n"] == null)
                        ch1.Attributes.Append(ch2.Attributes["n"].Clone() as XmlAttribute);

                    else if (ch2.Attributes["v"] != null && ch1.Attributes["v"] == null)
                        ch1.Attributes.Append(ch2.Attributes["v"].Clone() as XmlAttribute);

                    if (ch2.HasChildNodes)
                    {
                        MergeXml(d1, ch1, xpath1 + ch2Key, d2, ch2, xpath2 + ch2Key);
                    }
                }
                else 
                {
                    //ch1 = ch2.CloneNode(true);
                    ch1 = d1.ImportNode(ch2, true);
                    n1.AppendChild(ch1);
                }
            }
        }

        private static string GetXmlNodeKey(XmlNode n)
        {
            string xpathKey = "/" + n.Name;
            if (n.Attributes["p"] != null)
            {
                xpathKey += "[@p='" + n.Attributes["p"].Value + "']";
            }
            else if (n.Attributes["k"] != null)
            {
                xpathKey += "[@k='" + n.Attributes["k"].Value + "']";
            }
            return xpathKey;
        }

		#endregion

		#region XML Language Mapping

		/// <summary>
		/// The full path of the XML file that contains the language mappings. The data in this file plus the value you provide 
		/// to <see cref="Language"/> determine the value of IColumn.Language.
		/// </summary>
		public string LanguageMappingFileName
		{
			get { return _languageMappingFileName;	}
			set
			{
				try
				{
					_languageMappingFileName = value;

					_languageDoc = new XmlDocument();
					_languageDoc.Load(_languageMappingFileName);
					_language = string.Empty;;
					_languageNode = null;
				}
				catch {}
			}
		}

		/// <summary>
		/// Returns all of the languages currently configured for the DBMS set when Connect was called.
		/// </summary>
		/// <returns>An array with all of the possible languages.</returns>
		public string[] GetLanguageMappings()
		{
			return GetLanguageMappings(_driverString);
		}

		/// <summary>
		/// Returns all of the languages for a given driver, regardless of OMeta's current connection
		/// </summary>
		/// <returns>An array with all of the possible languages.</returns>
		public string[] GetLanguageMappings(string driverString)
		{
			

			string[] mappings = null;

			if ((null != _languageDoc) && (driverString != null))
			{
                driverString = driverString.ToUpper();
				string xPath = @"//Languages/Language[@From='" + driverString + "']";
				XmlNodeList nodes = _languageDoc.SelectNodes(xPath, null);

				if ((null != nodes) && (nodes.Count > 0))
				{
					int nodeCount = nodes.Count;
					mappings = new string[nodeCount];

					for(int i = 0; i < nodeCount; i++)
					{
						mappings[i] = nodes[i].Attributes["To"].Value;
					}
				}
			}

			return mappings;
		}

		/// <summary>
		/// Use this to choose your Language, for example, "C#". See <see cref="LanguageMappingFileName"/> for more information
		/// </summary>
		public string Language
		{
			get
			{
				return _language;
			}

			set
			{
				if(null != _languageDoc)
				{
					_language = value;
					string xPath = @"//Languages/Language[@From='" + _driverString + "' and @To='" + _language + "']";
					_languageNode = _languageDoc.SelectSingleNode(xPath, null);
				}
			}
        }

        [ComVisible(false)]
        public XmlNode LanguageNode
        {
            get
            {
                return _languageNode;
            }
        }

		private string _languageMappingFileName = string.Empty;
		private string _language = string.Empty;
		private XmlDocument _languageDoc;
        private XmlNode _languageNode = null;

		#endregion

		#region XML DbTarget Mapping

		/// <summary>
		/// The full path of the XML file that contains the DbTarget mappings. The data in this file plus the value you provide 
		/// to <see cref="DbTarget"/> determine the value of IColumn.DbTarget.
		/// </summary>
		public string DbTargetMappingFileName
		{
			get	{ return _dbTargetMappingFileName; }
			set
			{
				try
				{
					_dbTargetMappingFileName = value;

					_dbTargetDoc = new XmlDocument();
					_dbTargetDoc.Load(_dbTargetMappingFileName);
					_dbTarget = string.Empty;;
					_dbTargetNode = null;
				}
				catch {}
			}
		}

		/// <summary>
		/// Returns all of the dbTargets currently configured for the DBMS set when Connect was called.
		/// </summary>
		/// <returns>An array with all of the possible dbTargets.</returns>
		public string[] GetDbTargetMappings()
		{
			return GetDbTargetMappings(_driverString);
		}

		/// <summary>
		/// Returns all of the dbTargets for a given driver, regardless of OMeta's current connection
		/// </summary>
		/// <returns>An array with all of the possible dbTargets.</returns>
		public string[] GetDbTargetMappings(string driverString)
		{
			

			string[] mappings = null;

			if ((null != _dbTargetDoc) && (driverString != null))
			{
                driverString = driverString.ToUpper();
				string xPath = @"//DbTargets/DbTarget[@From='" + driverString + "']";
				XmlNodeList nodes = _dbTargetDoc.SelectNodes(xPath, null);

				if(null != nodes && nodes.Count > 0)
				{
					int nodeCount = nodes.Count;
					mappings = new string[nodeCount];

					for(int i = 0; i < nodeCount; i++)
					{
						mappings[i] = nodes[i].Attributes["To"].Value;
					}
				}
			}

			return mappings;
		}

		/// <summary>
		/// Use this to choose your DbTarget, for example, "SqlClient". See <see cref="DbTargetMappingFileName"/>  for more information
		/// </summary>
		public string DbTarget
		{
			get
			{
				return _dbTarget;
			}

			set
			{
				if(null != _dbTargetDoc)
				{
					_dbTarget = value;
					string xPath = @"//DbTargets/DbTarget[@From='" + _driverString + "' and @To='" + _dbTarget + "']";
					_dbTargetNode = _dbTargetDoc.SelectSingleNode(xPath, null);
				}
			}
        }

        public XmlNode DbTargetNode
        {
            get
            {
                return _dbTargetNode;
            }
        }

		private string _dbTargetMappingFileName = string.Empty;
		private string _dbTarget = string.Empty;
		private XmlDocument _dbTargetDoc;
        private XmlNode _dbTargetNode = null;
		#endregion

        public XmlDocument UserData = new XmlDocument();

		public bool IgnoreCase = true;
        public bool requiredDatabaseName = false;
        public bool requiresSchemaName = false;
        public bool StripTrailingNulls = false;

        public string TrailingNull = null;

        public IClassFactory ClassFactory = null;

        private bool _showSystemData = false;
        private bool _showDefaultDatabaseOnly = false;

		private dbDriver _driver = dbDriver.None;
		private string _driverString = "NONE";
		private string _defaultDatabase = "";
		private Databases _databases = null;
		private ProviderTypes _providerTypes = null;
		private string _connectionString = "";
		private bool _isConnected = false;
		private Hashtable _parsedConnectionString = null;
        private bool _domainOverride = true;
        private string _lastConnectionError = string.Empty;
        private Exception _lastConnectionException = null;
        private XmlNode _xmlNode = null;
        private System.Collections.Generic.Dictionary<string, string> _userDataDatabaseMappings = null;

		private OleDbConnection _theConnection = new OleDbConnection();
	}

    /// <summary>
    /// The current list of support dbDrivers. Typically VBScript and JScript use the string version as defined by OMeta.DriverString.
    /// </summary>
 
    public enum dbDriver
	{
		/// <summary>
		/// String form is "SQL" for DriverString property
		/// </summary>
		SQL,

		/// <summary>
		/// String form is "ORACLE" for DriverString property
		/// </summary>
		Oracle,

		///// <summary>
		///// String form is "ACCESS" for DriverString property
		///// </summary>
		//Access,

		/// <summary>
		/// String form is "MYSQL" for DriverString property
		/// </summary>
		MySql,

		/// <summary>
		/// String form is "MYSQL" for DriverString property
		/// </summary>
		MySql2,

		/// <summary>
		/// String form is "DB2" for DriverString property
		/// </summary>
		DB2,

		/// <summary>
		/// String form is "ISeries" for DriverString property
		/// </summary>
		ISeries,

		///// <summary>
		///// String form is "PERVASIVE" for DriverString property
		///// </summary>
		//Pervasive,

		/// <summary>
		/// String form is "POSTGRESQL" for DriverString property
		/// </summary>
		PostgreSQL,

		/// <summary>
		/// String form is "POSTGRESQL8" for DriverString property
		/// </summary>
		PostgreSQL8,

		/// <summary>
		/// String form is "FIREBIRD" for DriverString property
		/// </summary>
		Firebird,

		/// <summary>
		/// String form is "INTERBASE" for DriverString property
		/// </summary>
		Interbase,

		/// <summary>
		/// String form is "SQLITE" for DriverString property
		/// </summary>
		SQLite,

 

		/// <summary>
		/// String form is "ADVANTAGE" for DriverString property
		/// </summary>
        Advantage,

        /// <summary>
        /// This is a placeholder for plugin providers
        /// </summary>
        Plugin,

		/// <summary>
		/// Use this if you want know connection at all
		/// </summary>
		None
    }

    #region OMetaDrivers string Constants
    public static class OMetaDrivers
    {
        //public const string Access = "ACCESS";
        public const string Advantage = "ADVANTAGE";
        public const string DB2 = "DB2";
        public const string Firebird = "FIREBIRD";
        public const string Interbase = "INTERBASE";
        public const string ISeries = "ISERIES";
        public const string MySql = "MYSQL";
        public const string MySql2 = "MYSQL2";
        public const string None = "NONE";
        public const string Oracle = "ORACLE";
        //public const string Pervasive = "PERVASIVE";
        public const string PostgreSQL = "POSTGRESQL";
        public const string PostgreSQL8 = "POSTGRESQL8";
        public const string SQLite = "SQLITE";
        public const string SQL = "SQL";
 
        public static dbDriver GetDbDriverFromName(string name)
        {
            switch (name)
            {
                case OMetaDrivers.SQL:
                    return dbDriver.SQL;
                case OMetaDrivers.Oracle:
                    return dbDriver.Oracle;
                case OMetaDrivers.MySql:
                    return dbDriver.MySql;
                case OMetaDrivers.MySql2:
                    return dbDriver.MySql2;
                case OMetaDrivers.DB2:
                    return dbDriver.DB2;
                case OMetaDrivers.ISeries:
                    return dbDriver.ISeries;
                case OMetaDrivers.PostgreSQL:
                    return dbDriver.PostgreSQL;
                case OMetaDrivers.PostgreSQL8:
                    return dbDriver.PostgreSQL8;
                case OMetaDrivers.Firebird:
                    return dbDriver.Firebird;
                case OMetaDrivers.Interbase:
                    return dbDriver.Interbase;
                case OMetaDrivers.SQLite:
                    return dbDriver.SQLite;
 
                case OMetaDrivers.Advantage:
                    return dbDriver.Advantage;
                case OMetaDrivers.None:
                    return dbDriver.None;
                default:
                    return dbDriver.Plugin;
            }
        }
    }
    #endregion
}

