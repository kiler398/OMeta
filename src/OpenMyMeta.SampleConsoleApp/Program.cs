using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMeta;

namespace OMeta.SampleConsoleApp
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
            //获取当前连接默认数据库
            var database = dbRoot.DefaultDatabase;
            //输出当前默认数据库名
            Console.WriteLine("当前默认数据库名:"+database.Name);
            Console.WriteLine("----------------------------------------");
            //遍历循环当前数据库所有的表
            int i = 1;
            foreach (var table in database.Tables)
            {
                //输出表名和备注
                Console.WriteLine("表"+ i.ToString("D2") + ":" + table.Name +",备注："+ table.Description);
                Console.WriteLine("---------------");
                int j = 1;
                //遍历循环表所有的字段
                foreach (var column in table.Columns)
                {
                    //输出字段名和字段类型
                    Console.Write("字段" + j.ToString("D2") + ":" + column.Name + "," + column.DataTypeNameComplete);
                    Console.WriteLine("");
                    j++;
                }
                i++;
            }

            //获取指定名称数据库（当前用户有权访问的数据库）
            var database2 = dbRoot.Databases["Northwind"];
            //输出当前默认数据库名
            Console.WriteLine("数据库名:" + database2.Name);
            Console.WriteLine("----------------------------------------");


            int k = 1;
            foreach (var view in database.Views)
            {
                //输出视图名和备注
                Console.WriteLine("视图" + i.ToString("D2") + ":" + view.Name + ",备注：" + view.Description);
                Console.WriteLine("---------------");
                //输出存储过程脚本
                Console.WriteLine("视图过程脚本:" + view.ViewText);
                Console.WriteLine("---------------");
                int j = 1;
                //遍历循环视图所有的字段
                foreach (var column in view.Columns)
                {
                    //输出字段名和字段类型
                    Console.Write("字段" + j.ToString("D2") + ":" + column.Name + "," + column.DataTypeNameComplete);
                    Console.WriteLine("");
                    j++;
                }
                k++;
            }


            int l = 1;
            foreach (var procedure in database.Procedures)
            {
                //输出存储过程名和备注
                Console.WriteLine("存储过程" + i.ToString("D2") + ":" + procedure.Name + ",备注：" + procedure.Description);
                Console.WriteLine("---------------");
                //输出存储过程脚本
                Console.WriteLine("存储过程脚本:" + procedure.ProcedureText);
                Console.WriteLine("---------------");
                int j = 1;
                //遍历循环存储过程所有的参数
                foreach (var parameter in procedure.Parameters)
                {
                    //输出参数名和参数类型
                    Console.Write("参数" + j.ToString("D2") + ":" + parameter.Name + "," + parameter.DataTypeNameComplete);
                    Console.WriteLine("");
                    j++;
                }

                j = 1;
                //遍历循环存储过程所有的结果
                foreach (IResultColumn column in procedure.ResultColumns)
                {
                    //输出字段名和字段类型
                    Console.Write("字段" + j.ToString("D2") + ":" + column.Name + "," + column.DataTypeNameComplete);
                    Console.WriteLine("");
                    j++;
                }
                l++;
            }


            Console.ReadKey();


            //Code1();
        }

        private static void Code1()
        {
            string basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            dbRoot = new dbRoot();
            dbRoot.DbTarget = "SqlClient";
            dbRoot.DbTargetMappingFileName = Path.Combine(basePath, @"cfg\DbTargets.xml");

            dbRoot.Language = "C#";
            dbRoot.LanguageMappingFileName = Path.Combine(basePath, @"cfg\Languages.xml");

            //dbRoot.UserMetaDataFileName = @"C:\Program Files\MyGeneration\Settings\UserMetaData.xml";
            dbRoot.Connect(dbDriver.SQL,
                "Provider=sqloledb;Data Source=(local);Initial Catalog=Northwind;Integrated Security = SSPI; ");

            var database = dbRoot.DefaultDatabase;

            var sps = database.Procedures.ToList();

            var sps1 = sps.FindAll(p => p.Alias.StartsWith("sp"));

            var sps2 = sps.FindAll(p => p.Alias.StartsWith("dm_"));

            var sps3 = sps.FindAll(p =>
                !p.Alias.StartsWith("dm_") && !p.Alias.StartsWith("sp_") && !p.Alias.StartsWith("xp_") &&
                !p.Alias.StartsWith("fn_"));

            Console.WriteLine(sps.Count);
        }
    }
}
