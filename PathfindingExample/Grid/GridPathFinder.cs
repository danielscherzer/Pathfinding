using PathFinder;
using System;
using System.Collections.Generic;

namespace Example.Grid
{
	public static class GridPathFinder
	{
		public delegate IEnumerable<PathInfo<Coord>> Algorithm(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> nodeNeighbors);

		public static readonly Coord NullCoord = new Coord(ushort.MaxValue, ushort.MaxValue);

		public static float CostToNeighbor(Coord a, Coord b)
		{
			var dx = Math.Abs(a.Column - b.Column);
			var dy = Math.Abs(a.Row - b.Row);
			var D2 = MathF.Sqrt(2f);
			return dx + dy > 1 ? D2 : 1f;
		}

		public static IEnumerable<PathInfo<Coord>> BreathFirstSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			return PathFinding.BreadthFirst(start, goal, walkableNeighbors, NullCoord);
		}

		public static IEnumerable<PathInfo<Coord>> DijkstraSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			return PathFinding.Dijkstra(start, goal, walkableNeighbors, CostToNeighbor, NullCoord);
		}

		public static IEnumerable<PathInfo<Coord>> GreedyBestFirstSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			return PathFinding.GreedyBestFirstSearch(start, goal, walkableNeighbors, (a) => GridCostFunctions.DiagonalDistance(a, goal), NullCoord);
		}

		public static IEnumerable<PathInfo<Coord>> AStarSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			float h(Coord a) => GridCostFunctions.DiagonalDistance(a, goal);
			return PathFinding.AStarSearch(start, goal, walkableNeighbors, CostToNeighbor, h, NullCoord);
		}

		public static IEnumerable<PathInfo<Coord>> AStarSearchStraight(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			//use straightness as tie breaker
			float h(Coord a) => GridCostFunctions.DiagonalDistance(a, goal) + GridCostFunctions.Straightness(start, a, goal) * 0.001f;
			return PathFinding.AStarSearch(start, goal, walkableNeighbors, CostToNeighbor, h, NullCoord);
		}
	}
}