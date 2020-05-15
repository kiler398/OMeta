using System.Text;

namespace OMeta.Builders
{
    public enum SqlAuthType
    {
        UserNamePassword,
        Window
    }

    public class SqlConnectionStringBuilder
    {
        private string _host = "localhost";
        private int _port = 0;
        private SqlAuthType _authType = SqlAuthType.UserNamePassword;
        private string _dataBase;
        private string _userName;
        private string _pasword;

        public SqlConnectionStringBuilder SetHost(string host)
        {
            this._host = host;
            return this;
        }

        public SqlConnectionStringBuilder SetPort(int port)
        {
            this._port = port;
            return this;
        }

        public SqlConnectionStringBuilder SetAuthType(SqlAuthType authType)
        {
            this._authType = authType;
            return this;
        }
 
        public SqlConnectionStringBuilder SetDataBase(string dataBase)
        {
            this._dataBase = dataBase;
            return this;
        }

        public SqlConnectionStringBuilder SetUserName(string userName)
        {
            this._userName = userName;
            return this;
        }


        public SqlConnectionStringBuilder SetPasword(string pasword)
        {
            this._pasword = pasword;
            return this;
        }


        public string Builder(dbDriver dbDriver)
        {
            switch (dbDriver)
            {
                case dbDriver.SQL:
                    StringBuilder sbConnectionString = new StringBuilder("Provider=sqloledb;");

                    StringBuilder hostName = new StringBuilder(this._host);

                    if (this._port != 0 && this._port != 1433)
                    {
                        hostName.Append($",{this._port}");
                    }

                    sbConnectionString.Append($"Data Source={hostName.ToString()};");
                    sbConnectionString.Append($"Initial Catalog={this._dataBase};");
                    if (this._authType == SqlAuthType.UserNamePassword)
                    {
                        sbConnectionString.Append($"User Id={this._userName};");
                        sbConnectionString.Append($"Password={this._pasword};");
                    }
                    else
                    {
                        sbConnectionString.Append($"Integrated Security=SSPI;");
                    }
                    return sbConnectionString.ToString();
            }
            return "";
        }
    }
}
