using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace OMeta
{
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


