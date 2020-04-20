using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;

namespace OMeta
{
 
    public interface IOMetaPluginExt : IOMetaPlugin
    {
        void ChangeDatabase(IDbConnection connection, string database);

        DataTable GetProviderTypes(string database);
    }
}