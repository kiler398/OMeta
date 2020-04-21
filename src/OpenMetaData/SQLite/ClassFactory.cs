using System;

using OMeta;
using OpenMeta.Interfaces;

namespace OMeta.SQLite
{
 
	public class ClassFactory : IClassFactory
	{
        public static void Register()
        {
            InternalDriver driver = new FileDbDriver
                (typeof(ClassFactory)
                , "Data Source=","database.db",";New=False;Compress=True;Synchronous=Off"
                , "SqlLiteDB (*.db)|*.db|all files (*.*)|*.*");
            InternalDriver.Register("SQLITE",
                driver);
        }
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new SQLite.SQLiteTables();
		}

		public ITable CreateTable()
		{
			return new SQLite.SQLiteTable();
		}

		public IColumn CreateColumn()
		{
			return new SQLite.SQLiteColumn();
		}

		public IColumns CreateColumns()
		{
			return new SQLite.SQLiteColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new SQLite.SQLiteDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new SQLite.SQLiteDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new SQLite.SQLiteProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new SQLite.SQLiteProcedures();
		}

		public IView CreateView()
		{
			return new SQLite.SQLiteView();
		}

		public IViews CreateViews()
		{
			return new SQLite.SQLiteViews();
		}

		public IParameter CreateParameter()
		{
			return new SQLite.SQLiteParameter();
		}

		public IParameters CreateParameters()
		{
			return new SQLite.SQLiteParameters();
		}

		public IForeignKey  CreateForeignKey()
		{
			return new SQLite.SQLiteForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new SQLite.SQLiteForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new SQLite.SQLiteIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new SQLite.SQLiteIndexes();
		}

		public IResultColumn CreateResultColumn()
		{
			return new SQLite.SQLiteResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new SQLite.SQLiteResultColumns();
		}

		public IDomain CreateDomain()
		{
			return new SQLiteDomain();
		}

		public IDomains CreateDomains()
		{
			return new SQLiteDomains();
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
            return new System.Data.SQLite.SQLiteConnection();
        }

        public void ChangeDatabase(System.Data.IDbConnection connection, string database)
        {
            connection.ChangeDatabase(database);
        }
    }
}
