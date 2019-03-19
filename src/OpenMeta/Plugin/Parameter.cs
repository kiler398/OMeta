using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IParameter))]
#endif 
	public class PluginParameter : Parameter
    {
        private IOMetaPlugin plugin;

        public PluginParameter(IOMetaPlugin plugin)
        {
            this.plugin = plugin;
        }

        public override object DatabaseSpecificMetaData(string key)
        {
            return this.plugin.GetDatabaseSpecificMetaData(this, key);
        }
	}
}
