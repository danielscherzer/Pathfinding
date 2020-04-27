using System;

namespace PathFinder
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1036:Override methods on comparable types", Justification = "Only used inside PriorityQueue with IComparable interface")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Only used inside PriorityQueue with IComparable interface")]
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
