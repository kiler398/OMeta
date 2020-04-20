using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IIndexes))]
#endif 
	public class PluginIndexes : Indexes
    {
        private IOMetaPlugin plugin;

        public PluginIndexes(IOMetaPlugin plugin)
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
