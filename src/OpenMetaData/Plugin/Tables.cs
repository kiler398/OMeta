using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
 
	public class PluginTables : Tables
    {
        private IOMetaPlugin plugin;

        public PluginTables(IOMetaPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetTables(this.Database.Name);
            PopulateArray(metaData);
		}
	}
}
