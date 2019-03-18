using System;

using OMeta;

namespace OMeta.VistaDB
{
#if ENTERPRISE
	using System.EnterpriseServices;
	using System.Runtime.InteropServices;
    /// <summary>
    /// VistaDB 数据库 ClassFactory
    /// </summary>
    [ComVisible(false)]
#endif
	public class ClassFactory : IClassFactory
	{
        public static void Register()
        {
            InternalDriver.Register("VISTADB",
                new FileDbDriver
                (typeof(ClassFactory)
                , @"DataSource=", @"C:\Program Files\VistaDB 2.0\Data\Northwind.vdb", @";Cypher= None;Password=;Exclusive=False;Readonly=False;"
                , "VistaDB (*.vbd)|*.vbd|all files (*.*)|*.*"));
        }
        /// <summary>
        /// 构造器
        /// </summary>
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new VistaDB.VistaDBTables();
		}

		public ITable CreateTable()
		{
			return new VistaDB.VistaDBTable();
		}

		public IColumn CreateColumn()
		{
			return new VistaDB.VistaDBColumn();
		}

		public IColumns CreateColumns()
		{
			return new VistaDB.VistaDBColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new VistaDB.VistaDBDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new VistaDB.VistaDBDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new VistaDB.VistaDBProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new VistaDB.VistaDBProcedures();
		}

		public IView CreateView()
		{
			return new VistaDB.VistaDBView();
		}

		public IViews CreateViews()
		{
			return new VistaDB.VistaDBViews();
		}

		public IParameter CreateParameter()
		{
			return new VistaDB.VistaDBParameter();
		}

		public IParameters CreateParameters()
		{
			return new VistaDB.VistaDBParameters();
		}

		public IForeignKey CreateForeignKey()
		{
			return new VistaDB.VistaDBForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new VistaDB.VistaDBForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new VistaDB.VistaDBIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new VistaDB.VistaDBIndexes();
		}

		public IDomain CreateDomain()
		{
			return new VistaDBDomain();
		}

		public IDomains CreateDomains()
		{
			return new VistaDBDomains();
		}

		public IResultColumn CreateResultColumn()
		{
			return new VistaDB.VistaDBResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new VistaDB.VistaDBResultColumns();
		}


		public IProviderType CreateProviderType()
		{
			return new ProviderType();
		}

		public IProviderTypes CreateProviderTypes()
		{
			return new ProviderTypes();
        }

        public void ChangeDatabase(System.Data.IDbConnection connection, string database)
        {
            connection.ChangeDatabase(database);
        }

        #region IClassFactory Members

        public System.Data.IDbConnection CreateConnection()
        {
            return new Provider.VistaDB.VistaDBConnection();
        }

        #endregion
    }
}
