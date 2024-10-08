using System;
using System.Collections.Generic;
using Zenseless.PathFinder;
using Zenseless.PathFinder.Grid;

namespace Example.Model;

public static class GridPathFinderAlgorithms
{
	public delegate IEnumerable<PathInfo<Coord>> Algorithm(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> nodeNeighbors);

	public static float CostToNeighbor(Coord a, Coord b)
	{
		return 1f; //TODO: More complicated CostToNeighbor can cause paths to be "longer" (disregarding the cost) for A* than for Breath-First. We need to take cost into account for path length.
		var dx = Math.Abs(a.Column - b.Column);
		var dy = Math.Abs(a.Row - b.Row);
		var D2 = MathF.Sqrt(2f);
		return dx + dy > 1 ? D2 : 1f;
	}

	public static IEnumerable<PathInfo<Coord>> BreathFirstSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
	{
		return Algorithms.BreadthFirst(start, goal, walkableNeighbors, Coord.Null);
	}

	public static IEnumerable<PathInfo<Coord>> DijkstraSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
	{
		return Algorithms.Dijkstra(start, goal, walkableNeighbors, CostToNeighbor, Coord.Null);
	}

	public static IEnumerable<PathInfo<Coord>> GreedyBestFirstSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
	{
		return Algorithms.GreedyBestFirstSearch(start, goal, walkableNeighbors, (a) => GridCostFunctions.DiagonalDistance(a, goal), Coord.Null);
	}

	public static IEnumerable<PathInfo<Coord>> AStarSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
	{
		float h(Coord a) => GridCostFunctions.DiagonalDistance(a, goal);
		return Algorithms.AStarSearch(start, goal, walkableNeighbors, CostToNeighbor, h, Coord.Null);
	}

	public static IEnumerable<PathInfo<Coord>> AStarSearchStraight(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
	{
		//use straightness as tie breaker
		float h(Coord a) => GridCostFunctions.DiagonalDistance(a, goal) + GridCostFunctions.Straightness(start, a, goal) * 0.001f;
		return Algorithms.AStarSearch(start, goal, walkableNeighbors, CostToNeighbor, h, Coord.Null);
	}
}
