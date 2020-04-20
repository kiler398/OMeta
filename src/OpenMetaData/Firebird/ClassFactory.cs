using System;
using System.Data;
using OMeta;
using OpenMeta.Interfaces;

namespace OMeta.Firebird
{
 
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
                string result = GetDataBaseName(con);
                int index = result.LastIndexOf("\\");
                if (index >= 0)
                {
                    result = result.Substring(index + 1);
                }
                return result;
            }
        }
        public static void Register()
        {
            InternalDriver.Register("FIREBIRD",
                new FileDbDriver
                (typeof(ClassFactory)
                , @"Database=", @"C:\firebird\EMPLOYEE.GDB", @";User=SYSDBA;Password=wow;Dialect=3;Server=localhost"
                , "FirebirdDB (*.GDB)|*.GDB|all files (*.*|*.*"));
            InternalDriver.Register("INTERBASE",
                new FileDbDriver
                (typeof(ClassFactory)
                , @"Database=", @"C:\firebird\EMPLOYEE.GDB", @";User=SYSDBA;Password=wow;Server=localhost"
                , "InterbaseDB (*.GDB)|*.GDB|all files (*.*)|*.*"));

        }
        public ClassFactory()
		{

		}

		public ITables CreateTables()
		{
			return new Firebird.FirebirdTables();
		}

		public ITable CreateTable()
		{
			return new Firebird.FirebirdTable();
		}

		public IColumn CreateColumn()
		{
			return new Firebird.FirebirdColumn();
		}

		public IColumns CreateColumns()
		{
			return new Firebird.FirebirdColumns();
		}

		public IDatabase CreateDatabase()
		{
			return new Firebird.FirebirdDatabase();
		}

		public IDatabases CreateDatabases()
		{
			return new Firebird.FirebirdDatabases();
		}

		public IProcedure CreateProcedure()
		{
			return new Firebird.FirebirdProcedure();
		}

		public IProcedures CreateProcedures()
		{
			return new Firebird.FirebirdProcedures();
		}

		public IView CreateView()
		{
			return new Firebird.FirebirdView();
		}

		public IViews CreateViews()
		{
			return new Firebird.FirebirdViews();
		}

		public IParameter CreateParameter()
		{
			return new Firebird.FirebirdParameter();
		}

		public IParameters CreateParameters()
		{
			return new Firebird.FirebirdParameters();
		}

		public IForeignKey  CreateForeignKey()
		{
			return new Firebird.FirebirdForeignKey();
		}

		public IForeignKeys CreateForeignKeys()
		{
			return new Firebird.FirebirdForeignKeys();
		}

		public IIndex CreateIndex()
		{
			return new Firebird.FirebirdIndex();
		}

		public IIndexes CreateIndexes()
		{
			return new Firebird.FirebirdIndexes();
		}

		public IResultColumn CreateResultColumn()
		{
			return new Firebird.FirebirdResultColumn();
		}

		public IResultColumns CreateResultColumns()
		{
			return new Firebird.FirebirdResultColumns();
		}

		public IDomain CreateDomain()
		{
			return new Firebird.FirebirdDomain();
		}

		public IDomains CreateDomains()
		{
			return new Firebird.FirebirdDomains();
		}

		public IProviderType CreateProviderType()
		{
			return new ProviderType();
		}

		public IProviderTypes CreateProviderTypes()
		{
			return new ProviderTypes();
        }

        public void ChangeDatabase(IDbConnection connection, string database)
        {
            connection.ChangeDatabase(database);
        }

        #region IClassFactory Members

        public System.Data.IDbConnection CreateConnection()
        {
            return new FirebirdSql.Data.FirebirdClient.FbConnection();
        }

        #endregion
    }
}
