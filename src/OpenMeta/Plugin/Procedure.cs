using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Plugin
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IProcedure))]
#endif 
	public class PluginProcedure : Procedure
    {
        private IMyMetaPlugin plugin;

        public PluginProcedure(IMyMetaPlugin plugin)
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
