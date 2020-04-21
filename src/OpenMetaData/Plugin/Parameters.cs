using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
 
	public class PluginParameters : Parameters
    {
        private IOMetaPlugin plugin;

        public PluginParameters(IOMetaPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetProcedureParameters(this.Procedure.Database.Name, this.Procedure.Name);
            PopulateArray(metaData);
		}
	}
}
