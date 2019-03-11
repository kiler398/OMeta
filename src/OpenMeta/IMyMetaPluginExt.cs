using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;

namespace MyMeta
{
	[ComVisible(false)]
    public interface IMyMetaPluginExt : IMyMetaPlugin
    {
        void ChangeDatabase(IDbConnection connection, string database);

        DataTable GetProviderTypes(string database);
    }
}