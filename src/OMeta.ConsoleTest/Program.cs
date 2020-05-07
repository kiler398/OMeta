using System;

namespace OMeta.ConsoleTest
{
    class Program
    {
        private static dbRoot dbRoot;

        static void Main(string[] args)
        {
            //初始化元数据类
            dbRoot = new dbRoot();
            //连接到SqlServer数据库，注意必须使用oledb连接字符串
            dbRoot.Connect(dbDriver.SQL, "Provider=sqloledb;Data Source=(local);Initial Catalog=Northwind;Integrated Security = SSPI; ");

            //设置内置的数据库Ado.Net驱动，以及输出语言
            dbRoot.SetDbTarget("SqlClient");
            dbRoot.SetCodeLanguage("C#");

            ////指定当前编程语言
            //dbRoot.Language = "C#";
            ////指定当前语言映射文件路径
            //dbRoot.LanguageMappingFileName = @"F:\Projects\OMeta\src\OpenMetaData\Config\Languages.xml";

            //获取当前连接默认数据库
            var database = dbRoot.DefaultDatabase;

            //输出当前默认数据库名
            Console.WriteLine("当前默认数据库名:" + database.Name);
            Console.WriteLine("当前默认数据库架构名:" + database.SchemaName);
            Console.WriteLine("----------------------------------------");
            //遍历循环当前数据库所有的表
            int i = 1;
            foreach (var table in database.Tables)
            {
                //输出表名和备注
                Console.WriteLine("表" + i.ToString("D2") + ":" + table.Name + ",备注：" + table.Description);
                Console.WriteLine("---------------");
                int j = 1;
                //遍历循环表所有的字段
                foreach (var column in table.Columns)
                {
                    //输出字段名和字段类型
                    Console.Write("字段" + j.ToString("D2") + ":" + column.Name + "," + column.DataTypeNameComplete);
                    Console.WriteLine("");
                    Console.Write("语言类型："  +  column.LanguageType);
                    j++;
                }
                i++;
            }
        }
    }
}
