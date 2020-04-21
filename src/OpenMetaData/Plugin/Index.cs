using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
 
	public class PluginIndex : Index
    {
        private IOMetaPlugin plugin;

        public PluginIndex(IOMetaPlugin plugin)
        {
            this.plugin = plugin;
        }

        public override object DatabaseSpecificMetaData(string key)
        {
            return this.plugin.GetDatabaseSpecificMetaData(this, key);
        }
	}
}
