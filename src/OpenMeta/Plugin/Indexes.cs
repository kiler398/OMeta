using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Plugin
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndexes))]
#endif 
	public class PluginIndexes : Indexes
    {
        private IMyMetaPlugin plugin;

        public PluginIndexes(IMyMetaPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetTableIndexes(this.Table.Database.Name, this.Table.Name);
            PopulateArray(metaData);
		}
	}
}
