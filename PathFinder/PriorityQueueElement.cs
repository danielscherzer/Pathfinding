using System;

namespace Zenseless.PathFinder
{
	public struct PriorityQueueElement<NODE> : IComparable<PriorityQueueElement<NODE>>
	{
		public PriorityQueueElement(float priority, NODE coord) : this()
		{
			Priority = priority;
			Node = coord;
		}

		public float Priority { get; }
		public NODE Node { get; }

		public int CompareTo(PriorityQueueElement<NODE> other) => Priority.CompareTo(other.Priority);
	}
}
