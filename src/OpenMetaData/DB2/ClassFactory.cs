using System;

using OMeta;
using OpenMeta.Interfaces;

namespace OMeta.DB2
{
 
	public class ClassFactory : IClassFactory
	{
        public static void Register()
        {
            InternalDriver.Register("DB2",
                new InternalDriver
                (typeof(ClassFactory)
                , "Provider=IBMDADB2.1;Password=myPassword;User ID=myUser;Data Source=myDatasource;Persist Security Info=True"
                , true));
        }
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new DB2.DB2Tables();
		}

		public ITable CreateTable()
		{
			return new DB2.DB2Table();
		}

		public IColumn CreateColumn()
		{
			return new DB2.DB2Column();
		}

		public IColumns CreateColumns()
		{
			return new DB2.DB2Columns();
		}

		public IDatabase CreateDatabase()
		{
			return new DB2.DB2Database();
		}

		public IDatabases CreateDatabases()
		{
			return new DB2.DB2Databases();
		}

		public IProcedure CreateProcedure()
		{
			return new DB2.DB2Procedure();
		}

		public IProcedures CreateProcedures()
		{
			return new DB2.DB2Procedures();
		}

		public IView CreateView()
		{
			return new DB2.DB2View();
		}

		public IViews CreateViews()
		{
			return new DB2.DB2Views();
		}

		public IParameter CreateParameter()
		{
			return new DB2.DB2Parameter();
		}

		public IParameters CreateParameters()
		{
			return new DB2.DB2Parameters();
		}

		public IForeignKey CreateForeignKey()
		{
			return new DB2.DB2ForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new DB2.DB2ForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new DB2.DB2Index();
		}

		public IIndexes CreateIndexes()
		{
			return new DB2.DB2Indexes();
		}

		public IDomain CreateDomain()
		{
			return new DB2Domain();
		}

		public IDomains CreateDomains()
		{
			return new DB2Domains();
		}

		public IResultColumn CreateResultColumn()
		{
			return new DB2.DB2ResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new DB2.DB2ResultColumns();
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
