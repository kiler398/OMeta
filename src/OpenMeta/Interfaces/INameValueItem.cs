using System;
using System.Runtime.InteropServices;
using System.Collections;

namespace OMeta
{
    /// <summary>
    /// This interface allows all the collections here to be bound to 
    /// Name/Value collection type objects. with ease
    /// </summary>
    [Guid("6D15B5C1-1C0E-4AF7-9743-9791FF5DE774"),InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface INameValueItem
	{
		string ItemName{ get; }
		string ItemValue{ get; }
	}
}

