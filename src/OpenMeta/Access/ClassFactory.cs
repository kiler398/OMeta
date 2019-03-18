using System;

using OMeta;

namespace OMeta.Access
{
#if ENTERPRISE
	using System.EnterpriseServices;
	using System.Runtime.InteropServices;
	[ComVisible(false)]
#endif
	public class ClassFactory : IClassFactory
	{
        public static void Register()
        {
            InternalDriver.Register("ACCESS",
                new InternalDriver
                (typeof(ClassFactory)
                , @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\access\Northwind.mdb;User Id=;Password="
                , true));
        }
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new Access.AccessTables();
		}

		public ITable CreateTable()
		{
			return new Access.AccessTable();
		}

		public IColumn CreateColumn()
		{
			return new Access.AccessColumn();
		}

		public IColumns CreateColumns()
		{
			return new Access.AccessColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new Access.AccessDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new Access.AccessDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new Access.AccessProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new Access.AccessProcedures();
		}

		public IView CreateView()
		{
			return new Access.AccessView();
		}

		public IViews CreateViews()
		{
			return new Access.AccessViews();
		}

		public IParameter CreateParameter()
		{
			return new Access.AccessParameter();
		}

		public IParameters CreateParameters()
		{
			return new Access.AccessParameters();
		}

		public IForeignKey CreateForeignKey()
		{
			return new Access.AccessForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new Access.AccessForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new Access.AccessIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new Access.AccessIndexes();
		}

		public IResultColumn CreateResultColumn()
		{
			return new Access.AccessResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new Access.AccessResultColumns();
		}

		public IDomain CreateDomain()
		{
			return new AccessDomain();
		}

		public IDomains CreateDomains()
		{
			return new AccessDomains();;
		}

		public IProviderType CreateProviderType()
		{
			return new ProviderType();
		}

		public IProviderTypes CreateProviderTypes()
		{
			return new ProviderTypes();
		}
        public System.Data.IDbConnection CreateConnection()
        {
            return new System.Data.OleDb.OleDbConnection();
        }

        public void ChangeDatabase(System.Data.IDbConnection connection, string database)
        {
            connection.ChangeDatabase(database);
        }
    }
}
