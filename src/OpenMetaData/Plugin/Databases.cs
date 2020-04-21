using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
 
	public class PluginDatabases : Databases
	{
        private IOMetaPlugin plugin;

        public PluginDatabases(IOMetaPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
		{
			DataTable metaData  = this.plugin.Databases;
			PopulateArray(metaData);
		}
	}
}
