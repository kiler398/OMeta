using System;

using OMeta;

namespace OMeta.PostgreSQL8
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
            InternalDriver.Register("POSTGRESQL8",
                new InternalDriver
                (typeof(ClassFactory)
                , "Server=127.0.0.1;Port=5432;User Id=myUser;Password=myPasswordt;Database=test;"
                , false));
        }
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new PostgreSQL8.PostgreSQL8Tables();
		}

		public ITable CreateTable()
		{
			return new PostgreSQL8.PostgreSQL8Table();
		}

		public IColumn CreateColumn()
		{
			return new PostgreSQL8.PostgreSQL8Column();
		}

		public IColumns CreateColumns()
		{
			return new PostgreSQL8.PostgreSQL8Columns();
		}

		public IDatabase CreateDatabase()
		{
			return new PostgreSQL8.PostgreSQL8Database();
		}

		public IDatabases CreateDatabases()
		{
			return new PostgreSQL8.PostgreSQL8Databases();
		}

		public IProcedure CreateProcedure()
		{
			return new PostgreSQL8.PostgreSQL8Procedure();
		}

		public IProcedures CreateProcedures()
		{
			return new PostgreSQL8.PostgreSQL8Procedures();
		}

		public IView CreateView()
		{
			return new PostgreSQL8.PostgreSQL8View();
		}

		public IViews CreateViews()
		{
			return new PostgreSQL8.PostgreSQL8Views();
		}

		public IParameter CreateParameter()
		{
			return new PostgreSQL8.PostgreSQL8Parameter();
		}

		public IParameters CreateParameters()
		{
			return new PostgreSQL8.PostgreSQL8Parameters();
		}

		public IForeignKey  CreateForeignKey()
		{
			return new PostgreSQL8.PostgreSQL8ForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new PostgreSQL8.PostgreSQL8ForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new PostgreSQL8.PostgreSQL8Index();
		}

		public IIndexes CreateIndexes()
		{
			return new PostgreSQL8.PostgreSQL8Indexes();
		}

		public IResultColumn CreateResultColumn()
		{
			return new PostgreSQL8.PostgreSQL8ResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new PostgreSQL8.PostgreSQL8ResultColumns();
		}

		public IDomain CreateDomain()
		{
			return new PostgreSQL8Domain();
		}

		public IDomains CreateDomains()
		{
			return new PostgreSQL8Domains();
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
            return new Npgsql.NpgsqlConnection();
        }

        public void ChangeDatabase(System.Data.IDbConnection connection, string database)
        {
            connection.ChangeDatabase(database);
        }
    }
}
