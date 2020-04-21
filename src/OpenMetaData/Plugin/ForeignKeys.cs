using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
 
	public class PluginForeignKeys : ForeignKeys
    {
        private IOMetaPlugin plugin;

        public PluginForeignKeys(IOMetaPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetForeignKeys(this.Table.Database.Name, this.Table.Name);
            PopulateArray(metaData);
		}
	}
}
