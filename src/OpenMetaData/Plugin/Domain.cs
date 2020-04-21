using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
 
	public class PluginDomain : Domain	
	{
        private IOMetaPlugin plugin;

        public PluginDomain(IOMetaPlugin plugin)
        {
            this.plugin = plugin;
        }

        public override object DatabaseSpecificMetaData(string key)
        {
            return this.plugin.GetDatabaseSpecificMetaData(this, key);
        }
	}
}
