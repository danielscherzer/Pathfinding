using PathFinder;
using System;
using System.Collections.Generic;

namespace Example
{
	public static class GridPathFinder
	{
		public delegate IEnumerable<PathInfo<Coord>> Algorithm(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> nodeNeighbors);

		public static readonly Coord NullCoord = new Coord(ushort.MaxValue, ushort.MaxValue);

		public static IEnumerable<PathInfo<Coord>> BreathFirstSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			return PathFinding.BreathFirstSearch(start, goal, walkableNeighbors, NullCoord);
		}

		public static IEnumerable<PathInfo<Coord>> UniformCostSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			return PathFinding.UniformCostSearch(start, goal, walkableNeighbors, IGridExtensions.GridStepCost, NullCoord);
		}

		public static IEnumerable<PathInfo<Coord>> GreedyBestFirstSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			return PathFinding.GreedyBestFirstSearch(start, goal, walkableNeighbors, (a) => GridCostFunctions.DiagonalDistance(a, goal), NullCoord);
		}

		public static IEnumerable<PathInfo<Coord>> AStarSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			float h(Coord a) => GridCostFunctions.DiagonalDistance(a, goal);
			return PathFinding.AStarSearch(start, goal, walkableNeighbors, IGridExtensions.GridStepCost, h, NullCoord);
		}

		public static IEnumerable<PathInfo<Coord>> AStarSearchStraight(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			//use straightness as tie breaker
			float h(Coord a) => GridCostFunctions.DiagonalDistance(a, goal) + GridCostFunctions.Straightness(start, a, goal) * 0.001f;
			return PathFinding.AStarSearch(start, goal, walkableNeighbors, IGridExtensions.GridStepCost, h, NullCoord);
		}
	}
}
