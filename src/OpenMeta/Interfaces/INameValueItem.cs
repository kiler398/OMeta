using System;
using System.Runtime.InteropServices;
using System.Collections;

namespace MyMeta
{
    /// <summary>
    /// This interface allows all the collections here to be bound to 
    /// Name/Value collection type objects. with ease
    /// </summary>
    [Guid("DB486BAA-45A9-4D74-AF23-35F2FC38477D"),InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface INameValueItem
	{
		string ItemName{ get; }
		string ItemValue{ get; }
	}
}

