using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace OMeta
{
    [Guid("9E3B6733-D96C-48C5-820B-DA3C4AEE6E31"),InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IProviderTypes
	{
		[DispId(0)]
		IProviderType this[object index] { get; }

		// ICollection
		bool IsSynchronized { get; }
		int Count { get; }
		void CopyTo(System.Array array, int index);
		object SyncRoot { get; }

		// IEnumerable
		[DispId(-4)]
		IEnumerator GetEnumerator();
	}
}


