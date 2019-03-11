using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Plugin
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(ITables))]
#endif 
	public class PluginTables : Tables
    {
        private IMyMetaPlugin plugin;

        public PluginTables(IMyMetaPlugin plugin)
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
