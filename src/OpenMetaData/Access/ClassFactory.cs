using System.Data;
using OMeta;
using OMeta.Access;
using OpenMeta;
using OpenMeta.Access;
using OpenMeta.Interfaces;

namespace OpenMeta.Access
{
 
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
			return new OMeta.Access.AccessTables();
		}

		public ITable CreateTable()
		{
			return new OMeta.Access.AccessTable();
		}

		public IColumn CreateColumn()
		{
			return new OMeta.Access.AccessColumn();
		}

		public IColumns CreateColumns()
		{
			return new OMeta.Access.AccessColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new OMeta.Access.AccessDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new OMeta.Access.AccessDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new OMeta.Access.AccessProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new OMeta.Access.AccessProcedures();
		}

		public IView CreateView()
		{
			return new OMeta.Access.AccessView();
		}

		public IViews CreateViews()
		{
			return new OMeta.Access.AccessViews();
		}

		public IParameter CreateParameter()
		{
			return new OMeta.Access.AccessParameter();
		}

		public IParameters CreateParameters()
		{
			return new OMeta.Access.AccessParameters();
		}

		public IForeignKey CreateForeignKey()
		{
			return new OMeta.Access.AccessForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new OMeta.Access.AccessForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new OMeta.Access.AccessIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new OMeta.Access.AccessIndexes();
		}

		public IResultColumn CreateResultColumn()
		{
			return new OMeta.Access.AccessResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new OMeta.Access.AccessResultColumns();
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
        public IDbConnection CreateConnection()
        {
            return new System.Data.OleDb.OleDbConnection();
        }

        public void ChangeDatabase(System.Data.IDbConnection connection, string database)
        {
            connection.ChangeDatabase(database);
        }
    }
}
