using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMeta;
using Xunit;

namespace OMeta.UnitTests
{
    public class OMetaMSSQLTests
    {

        private dbRoot dbRoot;

        public OMetaMSSQLTests()
        {
            string basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            dbRoot = new dbRoot();
            dbRoot.DbTarget = "SqlClient";
            dbRoot.DbTargetMappingFileName = Path.Combine(basePath,@"cfg\DbTargets.xml");

            dbRoot.Language = "C#";
            dbRoot.LanguageMappingFileName = Path.Combine(basePath, @"cfg\Languages.xml");  

            //dbRoot.UserMetaDataFileName = @"C:\Program Files\MyGeneration\Settings\UserMetaData.xml";
            dbRoot.Connect(dbDriver.SQL, "Provider=sqloledb;Data Source=(local);Initial Catalog=Northwind;Integrated Security = SSPI; ");



        }

        /// <summary>
        /// 测试默认数据库名
        /// </summary>
        [Fact(DisplayName = "测试默认数据库名")]
        public void TestDefaultDatabaseName()
        {
            Assert.Equal("Northwind", dbRoot.DefaultDatabase.Alias);
            //Console.WriteLine(dbRoot.DefaultDatabase.Alias);
        }

        /// <summary>
        /// 测试默认数据库表数量
        /// </summary>
        [Fact(DisplayName = "测试默认数据库表视图存储过程数量")]
        public void TestDefaultDatabaseTableCount()
        {
            Assert.Equal(13, dbRoot.DefaultDatabase.Tables.Count);

            Assert.Equal(16, dbRoot.DefaultDatabase.Views.Count);

            var sps = dbRoot.DefaultDatabase.Procedures.ToList().FindAll(p =>
                !p.Alias.StartsWith("dm_") && !p.Alias.StartsWith("sp_") && !p.Alias.StartsWith("xp_") &&
                !p.Alias.StartsWith("fn_"));

            Assert.Equal(7, sps.Count);
        }

        [Theory(DisplayName = "测试默认数据库所有表名")]
        [InlineData("Categories")]
        [InlineData("CustomerCustomerDemo")]
        [InlineData("CustomerDemographics")]
        [InlineData("Customers")]
        [InlineData("Employees")]
        [InlineData("EmployeeTerritories")]
        [InlineData("OrderDetails")]
        [InlineData("Orders")]
        [InlineData("Products")]
        [InlineData("Region")]
        [InlineData("Shippers")]
        [InlineData("Suppliers")]
        [InlineData("Territories")]
        public void TestDefaultDatabaseAllTableName(string tableName)
        {
            Assert.True(CheckDbHasTableName(dbRoot.DefaultDatabase, tableName));
        }


        [Theory(DisplayName = "测试默认数据库所有视图名")]
        [InlineData("AlphabeticalListOfProducts")]
        [InlineData("CategorySalesFor1997")]
        [InlineData("CurrentProductList")]
        [InlineData("CustomerAndSuppliersByCity")]
        [InlineData("Invoices")]
        [InlineData("OrderDetailsExtended")]
        [InlineData("OrdersQry")]
        [InlineData("OrderSubtotals")]
        [InlineData("ProductsAboveAveragePrice")]
        [InlineData("ProductSalesFor1997")]
        [InlineData("ProductsByCategory")]
        [InlineData("QuarterlyOrders")]
        [InlineData("SalesByCategories")]
        [InlineData("SalesTotalsByAmount")]
        [InlineData("SummaryOfSalesByQuarter")]
        [InlineData("SummaryOfSalesByYear")]
        public void TestDefaultDatabaseAllViewName(string viewName)
        {
            Assert.True(CheckDbHasViewName(dbRoot.DefaultDatabase, viewName));
        }


        [Theory(DisplayName = "测试默认数据库所有存储过程名")]
        [InlineData("CustOrderHist")]
        [InlineData("CustOrdersDetail")]
        [InlineData("CustOrdersOrders")]
        [InlineData("EmployeeSalesByCountry")]
        [InlineData("SalesByCategory")]
        [InlineData("SalesByYear")]
        [InlineData("TenMostExpensiveProducts")]
        public void TestDefaultDatabaseAllProcedureName(string procedureName)
        {
            Assert.True(CheckDbHasProcedureName(dbRoot.DefaultDatabase, procedureName));
        }


        private bool CheckDbHasTableName(IDatabase database,string tableName)
        {
            return database.Tables.ToList().Exists(p => p.Alias.Equals(tableName));
        }

        private bool CheckDbHasViewName(IDatabase database, string viewName)
        {
            return database.Views.ToList().Exists(p => p.Alias.Equals(viewName));
        }

        private bool CheckDbHasProcedureName(IDatabase database, string procedureName)
        {
            return database.Procedures.ToList().Exists(p => p.Alias.Equals(procedureName));
        }
    }
}
