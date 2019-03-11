using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace MyMeta
{
    [Guid("A62F62DA-81F0-4FB2-83A9-F1F84CB203DD"),InterfaceType(ComInterfaceType.InterfaceIsDual)]
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


