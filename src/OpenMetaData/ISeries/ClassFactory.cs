using System;

using OMeta;
using OpenMeta.Interfaces;

namespace OMeta.ISeries
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
            InternalDriver.Register("ISERIES",
                new InternalDriver
                (typeof(ClassFactory)
                , "PROVIDER=IBMDA400; DATA SOURCE=MY_SYSTEM_NAME;USER ID=myUserName;PASSWORD=myPwd;DEFAULT COLLECTION=MY_LIBRARY;"
                , true));
        }
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new ISeries.ISeriesTables();
		}

		public ITable CreateTable()
		{
			return new ISeries.ISeriesTable();
		}

		public IColumn CreateColumn()
		{
			return new ISeries.ISeriesColumn();
		}

		public IColumns CreateColumns()
		{
			return new ISeries.ISeriesColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new ISeries.ISeriesDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new ISeries.ISeriesDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new ISeries.ISeriesProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new ISeries.ISeriesProcedures();
		}

		public IView CreateView()
		{
			return new ISeries.ISeriesView();
		}

		public IViews CreateViews()
		{
			return new ISeries.ISeriesViews();
		}

		public IParameter CreateParameter()
		{
			return new ISeries.ISeriesParameter();
		}

		public IParameters CreateParameters()
		{
			return new ISeries.ISeriesParameters();
		}

		public IForeignKey CreateForeignKey()
		{
			return new ISeries.ISeriesForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new ISeries.ISeriesForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new ISeries.ISeriesIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new ISeries.ISeriesIndexes();
		}

		public IDomain CreateDomain()
		{
			return new ISeriesDomain();
		}

		public IDomains CreateDomains()
		{
			return new ISeriesDomains();
		}

		public IResultColumn CreateResultColumn()
		{
			return new ISeries.ISeriesResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new ISeries.ISeriesResultColumns();
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
