using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
 
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
