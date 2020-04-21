using System;
using OMeta;
using MySql.Data.MySqlClient;
using OMeta.Interfaces;

namespace OMeta.MySql5
{
 
	public class ClassFactory : IClassFactory
	{
        public static void Register()
        {
            InternalDriver drv = new InternalDriver
                (typeof(ClassFactory)
                , "Database=Test;Data Source=Griffo;User Id=anonymous;Password=;"
                , false);
            drv.StripTrailingNulls = true;
            drv.RequiredDatabaseName = true;

            InternalDriver.Register("MYSQL2", drv);
        }
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new MySql5.MySql5Tables();
		}

		public ITable CreateTable()
		{
			return new MySql5.MySql5Table();
		}

		public IColumn CreateColumn()
		{
			return new MySql5.MySql5Column();
		}

		public IColumns CreateColumns()
		{
			return new MySql5.MySql5Columns();
		}

		public IDatabase CreateDatabase()
		{
			return new MySql5.MySql5Database();
		}

		public IDatabases CreateDatabases()
		{
			return new MySql5.MySql5Databases();
		}

		public IProcedure CreateProcedure()
		{
			return new MySql5.MySql5Procedure();
		}

		public IProcedures CreateProcedures()
		{
			return new MySql5.MySql5Procedures();
		}

		public IView CreateView()
		{
			return new MySql5.MySql5View();
		}

		public IViews CreateViews()
		{
			return new MySql5.MySql5Views();
		}

		public IParameter CreateParameter()
		{
			return new MySql5.MySql5Parameter();
		}

		public IParameters CreateParameters()
		{
			return new MySql5.MySql5Parameters();
		}

		public IForeignKey CreateForeignKey()
		{
			return new MySql5.MySql5ForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new MySql5.MySql5ForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new MySql5.MySql5Index();
		}

		public IIndexes CreateIndexes()
		{
			return new MySql5.MySql5Indexes();
		}

		public IResultColumn CreateResultColumn()
		{
			return new MySql5.MySql5ResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new MySql5.MySql5ResultColumns();
		}

		public IDomain CreateDomain()
		{
			return new MySql5Domain();
		}

		public IDomains CreateDomains()
		{
			return new MySql5Domains();
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
            return new MySqlConnection();
        }

        #endregion
    }
}
