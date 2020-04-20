using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDatabases))]
#endif 
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
