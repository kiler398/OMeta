using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
 
	public class PluginProcedure : Procedure
    {
        private IOMetaPlugin plugin;

        public PluginProcedure(IOMetaPlugin plugin)
        {
            this.plugin = plugin;
		}

		public override string ProcedureText
		{
			get
			{
				PluginProcedures procs = this.Procedures as PluginProcedures;
				return this.GetString(procs.f_procText);
			}
        }

        public override object DatabaseSpecificMetaData(string key)
        {
            return this.plugin.GetDatabaseSpecificMetaData(this, key);
        }
	}
}
