using System;

namespace Example
{
	struct PriorityNode : IComparable<PriorityNode>
	{
		public PriorityNode(int priority, Coord coord) : this()
		{
			Priority = priority;
			Coord = coord;
		}

		public int Priority { get; }
		public Coord Coord { get; }

		public int CompareTo(PriorityNode other) => Priority.CompareTo(other.Priority);
	}
}
