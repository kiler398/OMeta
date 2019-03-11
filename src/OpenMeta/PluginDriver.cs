using System;
using System.Collections.Generic;
using System.Text;

namespace MyMeta
{
    public class PluginDriver : InternalDriver
    {
        private IMyMetaPlugin plugin;
        internal PluginDriver(IMyMetaPlugin plugin)
            : base(plugin.GetType(),"", false)
        {
            this.plugin = plugin;

            this.IsOleDB = canBrowseDatabase();
        }

        public override string ConnectString
        {
            get 
            {
                string result = base.ConnectString;
                if (result.Length == 0)
                    return plugin.SampleConnectionString;
                return result;
            }
            set { base.ConnectString = value; }
        }

        public override string BrowseConnectionString(string connstr)
        {
            MyMetaPluginContext pluginContext = new MyMetaPluginContext(this.DriverId, connstr);
            plugin.Initialize(pluginContext);
            return plugin.GetDatabaseSpecificMetaData(null, "BrowseDatabase") as string;
        }

        private bool canBrowseDatabase()
        {
            return (null != plugin.GetDatabaseSpecificMetaData(null, "CanBrowseDatabase"));
        }
    }
}
