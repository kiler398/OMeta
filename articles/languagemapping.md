# 语言映射

语言映射允许你通过一个xml配置文件来定义数据库字段和语言（C#，VB.Net等）的对应关系。

OMeta中表或者视图的字段对象`IColumn`有一个属性`LanguageType`,默认使用情况下访问这个属性会返回Unknown，如果给OMeta库指定一个特定的配置文件以后。这个属性就会返回数据库字段对应到编程语言类型。

`LanguageType`主要用于生成实体类的属性类型

映射文件如下

```xml
<?xml version="1.0" encoding="utf-8"?>
<Languages>
	<Language From="SQL" To="C#">
		<Type From="bigint" To="long" />
		<Type From="binary" To="object" />
		<Type From="bit" To="bool" />
		<Type From="char" To="string" />
		<Type From="datetime" To="DateTime" />
		<Type From="datetime2" To="DateTime" />
		<Type From="date" To="DateTime" />
		<Type From="time" To="DateTime" />
		<Type From="timestamp" To="DateTime" />
		<Type From="decimal" To="decimal" />
		<Type From="float" To="double" />
		<Type From="image" To="byte[]" />
		<Type From="int" To="int" />
		<Type From="money" To="decimal" />
		<Type From="nchar" To="string" />
		<Type From="ntext" To="string" />
		<Type From="numeric" To="decimal" />
		<Type From="nvarchar" To="string" />
		<Type From="real" To="float" />
		<Type From="smalldatetime" To="DateTime" />
		<Type From="smallint" To="short" />
		<Type From="smallmoney" To="decimal" />
		<Type From="text" To="string" />
		<Type From="tinyint" To="byte" />
		<Type From="uniqueidentifier" To="Guid" />
		<Type From="varbinary" To="byte[]" />
		<Type From="varchar" To="string" />
		<Type From="xml" To="string" />
		<Type From="sql_variant" To="object" />
	</Language>
    <Language From="SQL" To="VB.NET">
		<Type From="bigint" To="Long" />
		<Type From="binary" To="Object" />
		<Type From="bit" To="Boolean" />
		<Type From="char" To="String" />
		<Type From="datetime" To="DateTime" />
		<Type From="decimal" To="Decimal" />
		<Type From="float" To="Double" />
		<Type From="image" To="Byte()" />
		<Type From="int" To="Integer" />
		<Type From="money" To="Decimal" />
		<Type From="nchar" To="String" />
		<Type From="ntext" To="String" />
		<Type From="numeric" To="Decimal" />
		<Type From="nvarchar" To="String" />
		<Type From="real" To="Single" />
		<Type From="smalldatetime" To="DateTime" />
		<Type From="smallint" To="Short" />
		<Type From="smallmoney" To="Decimal" />
		<Type From="text" To="String" />
		<Type From="timestamp" To="Byte()" />
		<Type From="tinyint" To="Byte" />
		<Type From="uniqueidentifier" To="Guid" />
		<Type From="varbinary" To="Byte()" />
		<Type From="varchar" To="String" />
		<Type From="xml" To="String" />
		<Type From="sql_variant" To="Object" />
	</Language>
</Languages>
```

我们截取其中的一个片段来看看

```xml

...

	<Language From="SQL" To="C#">
		<Type From="bigint" To="long" />
		<Type From="binary" To="object" />

...

```

` <Language From="SQL" To="C#"> ` 代表一个配置组 配置MSSQL（SqlServer）和C#语言的映射关系

` <Type From="bigint" To="long" /> ` 代表一个配置项 配置MSSQL（SqlServer）的bigint类型字段对应到C#语言的long数据类型

当指定了上述xml配置文件以后并且指明当前使用语言为C#，之前提到的`LanguageType`属性就生效了。

我们来看下配置代码：

 ```c#
    //初始化元数据类
    var dbRoot = new dbRoot();
    //连接到SqlServer数据库，注意必须使用oledb连接字符串
    dbRoot.Connect(dbDriver.SQL, "Provider=sqloledb;Data Source=(local);Initial Catalog=Northwind;Integrated Security = SSPI; ");
    
    //指定当前编程语言
    dbRoot.Language = "C#"
    //指定当前语言映射文件路径
    dbRoot.LanguageMappingFileName = "D:\Settings\Languages.xml"

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
            //如果上面指定了编程语言是C# 数据库是SQL（SqlServer） 当前字段是bigint的话 这里的LanguageType属性会输出 long
            Console.Write("字段语言类型：" + column.LanguageType);
            Console.WriteLine("");
            j++;
        }
        i++;
    }
```


