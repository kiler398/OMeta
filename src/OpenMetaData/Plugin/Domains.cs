using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
 
	public class PluginDomains : Domains
	{
        private IOMetaPlugin plugin;

        public PluginDomains(IOMetaPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetDomains(this.Database.Name);
            PopulateArray(metaData);
		}
	}
}
