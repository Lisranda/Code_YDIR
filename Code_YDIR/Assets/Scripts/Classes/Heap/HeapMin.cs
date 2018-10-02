using System;
using System.Collections.Generic;

public class HeapMin<T> where T : IComparable {
	T[] array;
	int count;

	public HeapMin (int minSize = 10) {
		array = new T[minSize];
	}

	void ExpandHeap () {
		T[] temp = new T[array.Length * 2];
		for (int i = 0; i < array.Length; i++) {
			temp [i] = array [i];
		}
		array = temp;
	}

	public int Count { get { return count; } }

	public int Size { get { return array.Length; } }

	public bool IsEmpty { get { return count == 0; } }

	public bool Contains (T item) {
		for (int i = 0; i < count; i++) {
			if (Equals (array [i], item))
				return true;
		}
		return false;
	}

	public void Add (T item) {
		if (count == array.Length)
			ExpandHeap ();
		array [count] = item;
		SortUp (count);
		count++;
	}

	public T Peek () {
		if (count < 1)
			throw new ArgumentOutOfRangeException ("Heap is empty");
		return array [0];
	}

	public T Remove () {
		T temp = Peek ();
		count--;
		array [0] = array [count];
		SortDown (0);
		return temp;
	}

	void Swap (int index1, int index2) {
		T temp = array [index1];
		array [index1] = array [index2];
		array [index2] = temp;
	}

	void SortUp (int index) {
		if (index == 0)
			return;
		int pIndex = (index - 1) / 2;
		if (!(array [pIndex].CompareTo (array [index]) > 0))
			return;
		Swap (pIndex, index);
		SortUp (pIndex);
	}

	void SortDown (int index) {
		int cLIndex = index * 2 + 1;
		if (cLIndex >= count)
			return;
		int cRIndex = cLIndex + 1;
		int cIndex = (cRIndex >= count || array [cLIndex].CompareTo (array [cRIndex]) <= 0) ? cLIndex : cRIndex;
		if (array [cIndex].CompareTo (array [index]) > 0)
			return;
		Swap (cIndex, index);
		SortDown (cIndex);
	}
}
