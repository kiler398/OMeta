using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(ITables))]
#endif 
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
