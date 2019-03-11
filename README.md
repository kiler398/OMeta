# OpenMyMeta
OpenMyMeta 是一个关系型数据库元数据获取工具，把数据库->schema->表->列，主键、外键、索引，触发器、存储过程、函数等抽象为对象，易于供代码生成工具使用。

1、提供丰富的接口，能够获取常见的所有数据库元数据。能很好的获取索引、存储过程、函数、视图的定义。

2、使用简单，只需要一个链接字符串并指定数据库类型连接到数据库即可获取该数据库的元数据信息。

3、丰富的数据库类型支持，支持MSSQL Oracle MySQL postgresql Access SQLLite等主流数据库。

4、有很强的扩展性，可以通过外置XML映射文件对各种编程语言（C# VB.NET Java）以及数据新字段类型提供映射操作。

