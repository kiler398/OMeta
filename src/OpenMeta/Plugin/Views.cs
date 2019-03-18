using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Plugin
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IViews))]
#endif 
	public class PluginViews : Views
	{
        private IOMetaPlugin plugin;

		#region DataColumn Binding Stuff

		// Added for 3rd party providers
		internal DataColumn f_viewText = null;	

		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("VIEW_TEXT"))
					f_viewText = metaData.Columns["VIEW_TEXT"];
			}																		
		}
		#endregion

        public PluginViews(IOMetaPlugin plugin)
        {
            this.plugin = plugin;
		}

		override internal void LoadAll()
        {
            DataTable metaData = this.plugin.GetViews(this.Database.Name);
			BindToColumns(metaData);
            PopulateArray(metaData);
		}
	}
}
