using System;

using OMeta;

namespace OMeta.Advantage
{
#if ENTERPRISE
	using System.EnterpriseServices;
	using System.Runtime.InteropServices;
	[ComVisible(false)]
#endif
	public class ClassFactory : IClassFactory
	{
        internal class MyInternalDriver : InternalDriver
        {
            internal MyInternalDriver(Type factory, string connString, bool isOleDB)
                : base(factory, connString, isOleDB)
            {
            }

            public override string GetDataBaseName(System.Data.IDbConnection con)
            {
                string result = GetDataBaseName(con).Split('.')[0];
                return result;
            }
        }

        public static void Register()
        {
            InternalDriver.Register("ADVANTAGE",
                new MyInternalDriver
                (typeof(ClassFactory)
                , @"Provider=Advantage.OLEDB.1;Password="";User ID=AdsSys;Data Source=C:\Program Files\Extended Systems\Advantage\Help\examples\aep_tutorial\task1;Initial Catalog=aep_tutorial.add;Persist Security Info=True;Advantage Server Type=ADS_LOCAL_SERVER;Trim Trailing Spaces=TRUE"
                , true));
        }
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new Advantage.AdvantageTables();
		}

		public ITable CreateTable()
		{
			return new Advantage.AdvantageTable();
		}

		public IColumn CreateColumn()
		{
			return new Advantage.AdvantageColumn();
		}

		public IColumns CreateColumns()
		{
			return new Advantage.AdvantageColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new Advantage.AdvantageDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new Advantage.AdvantageDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new Advantage.AdvantageProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new Advantage.AdvantageProcedures();
		}

		public IView CreateView()
		{
			return new Advantage.AdvantageView();
		}

		public IViews CreateViews()
		{
			return new Advantage.AdvantageViews();
		}

		public IParameter CreateParameter()
		{
			return new Advantage.AdvantageParameter();
		}

		public IParameters CreateParameters()
		{
			return new Advantage.AdvantageParameters();
		}

		public IForeignKey  CreateForeignKey()
		{
			return new Advantage.AdvantageForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new Advantage.AdvantageForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new Advantage.AdvantageIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new Advantage.AdvantageIndexes();
		}

		public IResultColumn CreateResultColumn()
		{
			return new Advantage.AdvantageResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new Advantage.AdvantageResultColumns();
		}

		public IDomain CreateDomain()
		{
			return new Advantage.AdvantageDomain();
		}

		public IDomains CreateDomains()
		{
			return new Advantage.AdvantageDomains();
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
            return null;
        }

        public void ChangeDatabase(System.Data.IDbConnection connection, string database)
        {
            connection.ChangeDatabase(database);
        }
    }
}
