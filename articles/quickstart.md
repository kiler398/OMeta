# 快速入门

OMeta使用涉及第三方组件库比较多，建议使用Nuget方式安装使用。

### NuGet 安装

你可以运行以下下命令在你的项目中安装 OMeta。

```
PM> Install-Package OMeta
```

### 使用数据库脚本创建MSSQL数据库

使用脚本创建MSSQL数据库

### 初始化OMeta对象

首先需要初始化OMeta对象并进行配置：

```c#
static void Main(string[] args)
{
    //初始化元数据类
    var dbRoot = new dbRoot();
    //连接到SqlServer数据库，注意必须使用oledb连接字符串
    dbRoot.Connect(dbDriver.SQL, "Provider=sqloledb;Data Source=(local);Initial Catalog=Northwind;Integrated Security = SSPI; ");
}

```

### 读取数据库信息

 ```c#
    //获取当前连接默认数据库
    var database = dbRoot.DefaultDatabase;
    //输出当前默认数据库名
    Console.WriteLine("当前默认数据库名:"+database.Name);
    Console.WriteLine("----------------------------------------");
```

### 读取指定名称数据库信息

 ```c#
    //获取指定名称数据库（当前用户有权访问的数据库）
    var database2 = dbRoot.Databases["Northwind"];
    //输出当前默认数据库名
    Console.WriteLine("数据库名:" + database2.Name);
    Console.WriteLine("----------------------------------------");
```

### 读取表 字段信息

 ```c#
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
```


### 读取视图 字段信息

 ```c#
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
```


### 读取存储过程 参数 结果字段信息

 ```c#
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
```