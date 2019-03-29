# ADO.Net DbTarget 映射

DbTarget 映射允许你通过一个xml配置文件来定义数据库字段和ADO.Net DbTarget字段类型（SqlDbType.Int  SqlDbType.Text等）的对应关系。

OMeta中表或者视图的字段对象`IColumn`有一个属性`DbTargetType`,默认使用情况下访问这个属性会返回Unknown，如果给OMeta库指定一个特定的配置文件以后。这个属性就会返回数据库字段对应到ADO.Net DbTarget字段类型。

`DbTargetType` 主要常用于生成存储过程或者sql参数化查询的参数类型

映射文件如下

```xml
<?xml version="1.0" encoding="utf-8"?>
<DbTargets>
	<DbTarget From="SQL" To="SqlClient">
		<Type From="bigint" To="SqlDbType.BigInt" />
		<Type From="binary" To="SqlDbType.Binary" />
		<Type From="bit" To="SqlDbType.Bit" />
		<Type From="char" To="SqlDbType.Char" />
		<Type From="datetime" To="SqlDbType.DateTime" />
		<Type From="decimal" To="SqlDbType.Decimal" />
		<Type From="float" To="SqlDbType.Float" />
		<Type From="image" To="SqlDbType.Image" />
		<Type From="int" To="SqlDbType.Int" />
		<Type From="money" To="SqlDbType.Money" />
		<Type From="nchar" To="SqlDbType.NChar" />
		<Type From="ntext" To="SqlDbType.NText" />
		<Type From="numeric" To="SqlDbType.Decimal" />
		<Type From="nvarchar" To="SqlDbType.NVarChar" />
		<Type From="real" To="SqlDbType.Real" />
		<Type From="smalldatetime" To="SqlDbType.SmallDateTime" />
		<Type From="smallint" To="SqlDbType.SmallInt" />
		<Type From="smallmoney" To="SqlDbType.SmallMoney" />
		<Type From="text" To="SqlDbType.Text" />
		<Type From="timestamp" To="SqlDbType.Timestamp" />
		<Type From="tinyint" To="SqlDbType.TinyInt" />
		<Type From="uniqueidentifier" To="SqlDbType.UniqueIdentifier" />
		<Type From="varbinary" To="SqlDbType.VarBinary" />
		<Type From="varchar" To="SqlDbType.VarChar" />
		<Type From="xml" To="SqlDbType.Xml" />
		<Type From="sql_variant" To="SqlDbType.Variant" />
	</DbTarget>
	<DbTarget From="SQL" To="OleDb">
		<Type From="bigint" To="OleDbType.BigInt" />
		<Type From="binary" To="OleDbType.Binary" />
		<Type From="bit" To="OleDbType.Boolean" />
		<Type From="char" To="OleDbType.Char" />
		<Type From="datetime" To="OleDbType.DBTimeStamp" />
		<Type From="decimal" To="OleDbType.Numeric" />
		<Type From="float" To="OleDbType.Double" />
		<Type From="image" To="OleDbType.LongVarBinary" />
		<Type From="int" To="OleDbType.Integer" />
		<Type From="money" To="OleDbType.Currency" />
		<Type From="nchar" To="OleDbType.WChar" />
		<Type From="ntext" To="OleDbType.LongVarWChar" />
		<Type From="numeric" To="OleDbType.Numeric" />
		<Type From="nvarchar" To="OleDbType.VarWChar" />
		<Type From="real" To="OleDbType.Single" />
		<Type From="smalldatetime" To="OleDbType.DBTimeStamp" />
		<Type From="smallint" To="OleDbType.SmallInt" />
		<Type From="smallmoney" To="OleDbType.Currency" />
		<Type From="text" To="OleDbType.LongVarChar" />
		<Type From="timestamp" To="OleDbType.DBTimeStamp" />
		<Type From="tinyint" To="OleDbType.UnsignedTinyInt" />
		<Type From="uniqueidentifier" To="OleDbType.Guid" />
		<Type From="varbinary" To="OleDbType.VarBinary" />
		<Type From="varchar" To="OleDbType.VarChar" />
		<Type From="xml" To="OleDbType.Xml" />
		<Type From="sql_variant" To="OleDbType.Variant" />
	</DbTarget>
</DbTargets>
```

我们截取其中的一个片段来看看

```xml

...

	<DbTarget From="SQL" To="SqlClient">
		<Type From="bigint" To="SqlDbType.BigInt" />
		<Type From="binary" To="SqlDbType.Binary" />

...

```

` <DbTarget From="SQL" To="SqlClient"> ` 代表一个配置组 配置MSSQL（SqlServer）和SqlClient驱动类型的映射关系

` <Type From="bigint" To="SqlDbType.BigInt" /> ` 代表一个配置项 配置MSSQL（SqlServer）的bigint类型字段对应到SqlClient驱动的SqlDbType.BigInt类型

当指定了上述xml配置文件以后并且指明当前使用驱动类型为SqlClient，之前提到的`DbTargetType`属性就生效了。

我们来看下配置代码：

 ```c#
    //初始化元数据类
    var dbRoot = new dbRoot();
    //连接到SqlServer数据库，注意必须使用oledb连接字符串
    dbRoot.Connect(dbDriver.SQL, "Provider=sqloledb;Data Source=(local);Initial Catalog=Northwind;Integrated Security = SSPI; ");
    
    //指定当前驱动类型
    dbRoot.DbTarget	= "SqlClient";
    //指定当前DbTarget映射文件路径
    dbRoot.DbTargetMappingFileName = "D:\Settings\DbTargets.xml"
 
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
            //如果上面指定了驱动类型是SqlClient 数据库是SQL（SqlServer） 当前字段是bigint的话 这里的DbTarget属性会输出 SqlDbType.BigInt
            Console.Write("DbTarget类型：" + column.DbTarget);
            Console.WriteLine("");
            j++;
        }
        i++;
    }
```


