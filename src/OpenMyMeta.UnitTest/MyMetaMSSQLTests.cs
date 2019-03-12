using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMeta;
using Xunit;

namespace MyGeneration.UnitTests
{
    public class MyMetaMSSQLTests
    {

        private dbRoot dbRoot;

        public MyMetaMSSQLTests()
        {
            string basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            //Console.WriteLine(str);

            dbRoot = new dbRoot();
            dbRoot.DbTarget = "SqlClient";
            dbRoot.DbTargetMappingFileName = Path.Combine(basePath,@"cfg\DbTargets.xml");

            dbRoot.Language = "C#";
            dbRoot.LanguageMappingFileName = Path.Combine(basePath, @"cfg\Languages.xml");  

            //dbRoot.UserMetaDataFileName = @"C:\Program Files\MyGeneration\Settings\UserMetaData.xml";
            dbRoot.Connect(dbDriver.SQL, "Provider=sqloledb;Data Source=(local);Initial Catalog=LL;Integrated Security = SSPI; ");



        }

        [Fact]
        public void TestDefaultDatabaseName()
        {
            Assert.Equal("LL", dbRoot.DefaultDatabase.Alias);

            Console.WriteLine(dbRoot.DefaultDatabase.Alias);
        }
    }
}
