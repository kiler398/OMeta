using System;
using System.Data;
using System.Data.OleDb;

namespace OMeta.Access
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
    /// <summary>
    /// Access数据库Domain信息
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IDomain))]
#endif 
	public class AccessDomain : Domain
	{
		public AccessDomain()
		{

		}
	}
}
