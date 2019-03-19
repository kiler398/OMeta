using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumn))]
#endif 
	public class PluginColumn : Column
	{
        private IOMetaPlugin plugin;

        public PluginColumn(IOMetaPlugin plugin)
        {
            this.plugin = plugin;
        }

		public override string DataTypeName
		{
			get
			{
				PluginColumns cols = Columns as PluginColumns;
				return this.GetString(cols.f_extTypeName);
			}
		}

		public override string DataTypeNameComplete
		{
			get
			{
				PluginColumns cols = Columns as PluginColumns;
				return this.GetString(cols.f_extTypeNameComplete);
			}
        }

        public override object DatabaseSpecificMetaData(string key)
        {
            return this.plugin.GetDatabaseSpecificMetaData(this, key);
        }
	}
}
