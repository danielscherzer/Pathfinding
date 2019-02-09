using System;

namespace PathFinder
{
	public struct PriorityPair<NODE> : IComparable<PriorityPair<NODE>>
	{
		public PriorityPair(float priority, NODE coord) : this()
		{
			Priority = priority;
			Node = coord;
		}

		public float Priority { get; }
		public NODE Node { get; }

		public int CompareTo(PriorityPair<NODE> other) => Priority.CompareTo(other.Priority);
	}
}
