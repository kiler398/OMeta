using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Plugin
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IForeignKey))]
#endif 
	public class PluginForeignKey : ForeignKey
	{
        private IMyMetaPlugin plugin;

        public PluginForeignKey(IMyMetaPlugin plugin)
        {
            this.plugin = plugin;
        }

        public override object DatabaseSpecificMetaData(string key)
        {
            return this.plugin.GetDatabaseSpecificMetaData(this, key);
        }
	}
}
