using System;
using System.Collections.Generic;
using System.Text;

namespace OMeta.Meta
{
    public enum AuthType 
    {
        UserPWD,
        LocalAccount
    }


    /// <summary>
    /// 构造OleDb连接字符串
    /// </summary>
    public class OleDbConnectionStringBuilder
    {
        private dbDriver _DbType;
        private string _Host;
        private string _Port;
        private string _UserID;
        private string _Password;
        private string _DataBase;
        private AuthType _AuthType;
 



        public OleDbConnectionStringBuilder()
        {

        }

        public OleDbConnectionStringBuilder SetDbType(dbDriver dbType)
        {
            this._DbType = dbType;
            return this;
        }

        public OleDbConnectionStringBuilder SetHost(string host)
        {
            this._Host = host;
            return this;
        }

        public OleDbConnectionStringBuilder SetPort(string port)
        {
            this._Port = port;
            return this;
        }

        public OleDbConnectionStringBuilder SetUserID(string userId)
        {
            this._UserID = userId;
            return this;
        }

        public OleDbConnectionStringBuilder SetPassword(string password)
        {
            this._Password = password;
            return this;
        }

        public OleDbConnectionStringBuilder SetDataBase(string dataBase)
        {
            this._DataBase = dataBase;
            return this;
        }

        public OleDbConnectionStringBuilder SetAuthType(AuthType authType)
        {
            this._AuthType = authType;
            return this;
        }

        public string Build()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("Provider=SQLOLEDB.1;");
            sbSql.Append("Data Source={};");
            sbSql.Append("Initial Catalog={};");
            return "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=Northwind;Data Source=localhost";
        }
    }
}
