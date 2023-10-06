using System;
using System.Collections.Generic;
using System.Text;

namespace Zenseless.PathFinder
{
	/// <summary>
	/// A priority queue based on a heap using a <seealso cref="List{T}"/> as data container.
	/// Inspired from https://visualstudiomagazine.com/Articles/2012/11/01/Priority-Queues-with-C.aspx?Page=1
	/// Smaller priority value means higher priority
	/// </summary>
	/// <typeparam name="T">Is required to implement <seealso cref="IComparable{T}"/>. All value types already do so.</typeparam>
	public class PriorityQueue<T> where T : IComparable<T>
	{
		/// <summary>
		/// Removes all elements from the <see cref="PriorityQueue{T}"/> 
		/// </summary>
		public void Clear() => _data.Clear();

		/// <summary>
		/// Gets the number of elements contained in the <see cref="PriorityQueue{T}"/> 
		/// </summary>
		public int Count => _data.Count;

		/// <summary>
		/// Returns a read-only <see cref="IReadOnlyCollection{T}"/> wrapper for the underlying data collection.
		/// </summary>
		public IReadOnlyList<T> Data => _data.AsReadOnly();

		/// <summary>
		/// Add an item to the <see cref="PriorityQueue{T}"/>. This is a O(log(n)) operation.
		/// </summary>
		/// <param name="item">The object to be added.</param>
		public void Enqueue(T item)
		{
			_data.Add(item);
			int ci = _data.Count - 1;
			while (ci > 0)
			{
				int pi = (ci - 1) / 2;
				if (_data[ci].CompareTo(_data[pi]) >= 0)
					break;
				//Switch index ci and pi
				(_data[pi], _data[ci]) = (_data[ci], _data[pi]);
				ci = pi;
			}
		}

		/// <summary>
		/// Return and remove the item with the highest priority from the <see cref="PriorityQueue{T}"/>. This is a O(log(n)) operation.
		/// </summary>
		/// <returns></returns>
		public T Dequeue()
		{
			// Assumes pq isn't empty
			int li = _data.Count - 1;
			T frontItem = _data[0];
			_data[0] = _data[li];
			_data.RemoveAt(li);

			--li;
			int pi = 0;
			while (true)
			{
				int ci = pi * 2 + 1;
				if (ci > li) break;
				int rc = ci + 1;
				if (rc <= li && _data[rc].CompareTo(_data[ci]) < 0)
					ci = rc;
				if (_data[pi].CompareTo(_data[ci]) <= 0) break;
				(_data[ci], _data[pi]) = (_data[pi], _data[ci]);
				pi = ci;
			}
			return frontItem;
		}

		/// <summary>
		/// Checks if the <see cref="PriorityQueue{T}"/> is in a consistent state.
		/// </summary>
		/// <returns></returns>
		public bool IsConsistent()
		{
			if (_data.Count == 0) return true;
			int li = _data.Count - 1; // last index
			for (int pi = 0; pi < _data.Count; ++pi) // each parent index
			{
				int lci = 2 * pi + 1; // left child index
				int rci = 2 * pi + 2; // right child index
				if (lci <= li && _data[pi].CompareTo(_data[lci]) > 0) return false;
				if (rci <= li && _data[pi].CompareTo(_data[rci]) > 0) return false;
			}
			return true; // Passed all checks
		}

		/// <summary>
		/// Return the item with the highest priority from the <see cref="PriorityQueue{T}"/>. This is a O(1) operation.
		/// </summary>
		/// <returns></returns>
		public T Peek() => _data[0];

		/// <summary>
		/// Converts the value of this instance to a <see cref="string"/>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var s = new StringBuilder();
			for (int i = 0; i < _data.Count; ++i)
			{
				s.Append(_data[i]);
				s.Append(' ');
			}
			s.Append($"count = {_data.Count}");
			return s.ToString();
		}

		private readonly List<T> _data = new List<T>();
	}
}