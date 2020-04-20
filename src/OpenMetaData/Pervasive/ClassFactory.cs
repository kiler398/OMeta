using System;

using OMeta;
using OpenMeta.Interfaces;

namespace OMeta.Pervasive
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
            InternalDriver.Register("PERVASIVE",
                new InternalDriver
                (typeof(ClassFactory)
                ,"Provider=PervasiveOLEDB.8.60;Data Source=demodata;Location=Griffo;Persist Security Info=False"
                , true));
        }
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new Pervasive.PervasiveTables();
		}

		public ITable CreateTable()
		{
			return new Pervasive.PervasiveTable();
		}

		public IColumn CreateColumn()
		{
			return new Pervasive.PervasiveColumn();
		}

		public IColumns CreateColumns()
		{
			return new Pervasive.PervasiveColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new Pervasive.PervasiveDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new Pervasive.PervasiveDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new Pervasive.PervasiveProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new Pervasive.PervasiveProcedures();
		}

		public IView CreateView()
		{
			return new Pervasive.PervasiveView();
		}

		public IViews CreateViews()
		{
			return new Pervasive.PervasiveViews();
		}

		public IParameter CreateParameter()
		{
			return new Pervasive.PervasiveParameter();
		}

		public IParameters CreateParameters()
		{
			return new Pervasive.PervasiveParameters();
		}

		public IForeignKey CreateForeignKey()
		{
			return new Pervasive.PervasiveForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new Pervasive.PervasiveForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new Pervasive.PervasiveIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new Pervasive.PervasiveIndexes();
		}

		public IResultColumn CreateResultColumn()
		{
			return new Pervasive.PervasiveResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new Pervasive.PervasiveResultColumns();
		}

		public IDomain CreateDomain()
		{
			return new Pervasive.PervasiveDomain();
		}

		public IDomains CreateDomains()
		{
			return new Pervasive.PervasiveDomains();
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
