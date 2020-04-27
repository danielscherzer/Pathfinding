using System;
using System.Collections.Generic;

namespace PathFinder
{
	public static class Algorithms
	{
		/// <summary>
		/// Create a path from <paramref name="start"/> to <paramref name="goal"/> from the cameForm dictionary
		/// </summary>
		/// <typeparam name="NODE">Type of a graph node</typeparam>
		/// <param name="start">node of the path</param>
		/// <param name="goal">node of the path</param>
		/// <param name="cameFrom">contains for each node the "best" choice of predecessor for a path from <paramref name="start"/> to <paramref name="goal"/>.</param>
		/// <returns>A list of NODE elements. Starting with <paramref name="start"/> and then each a single step on the path to <paramref name="goal"/>.</returns>
		public static List<NODE> CreatePath<NODE>(NODE start, NODE goal, IReadOnlyDictionary<NODE, NODE> cameFrom)
		{
			var path = new List<NODE>();
			var current = goal;
			while (!current.Equals(start))
			{
				path.Add(current);
				if (!cameFrom.ContainsKey(current))
				{
					break;
				}
				current = cameFrom[current];
			}
			path.Add(start);
			path.Reverse();
			return path;
		}

		/// <summary>
		/// Breadth first a.k.a. flood fill search with early exit
		/// </summary>
		/// <typeparam name="NODE">Type of a graph node</typeparam>
		/// <param name="start">node of the path</param>
		/// <param name="goal">node of the path</param>
		/// <param name="neighborNodes">functor that returns all neighbors for a given node</param>
		/// <param name="nullNode">is an empty, non existing node. Needed to signify the predecessor of <paramref name="start"/></param>
		/// <returns></returns>
		public static IEnumerable<PathInfo<NODE>> BreadthFirst<NODE>(NODE start, NODE goal
			, Func<NODE, IEnumerable<NODE>> neighborNodes, NODE nullNode)
		{
			if (neighborNodes is null) throw new ArgumentNullException(nameof(neighborNodes));
			var path = new List<NODE>();

			var frontier = new Queue<NODE>();
			var cameFrom = new Dictionary<NODE, NODE>();
			frontier.Enqueue(start);
			cameFrom[start] = nullNode;
			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				if (current.Equals(goal))
				{
					frontier.Clear();
					path = CreatePath(start, goal, cameFrom);
					break;
				}
				var neighbors = neighborNodes(current);
				foreach (var neighbor in neighbors)
				{
					if (!cameFrom.ContainsKey(neighbor))
					{
						frontier.Enqueue(neighbor);
						cameFrom[neighbor] = current;
					}
				}
				yield return new PathInfo<NODE>() { Path = path, CameFrom = cameFrom };
			}
			yield return new PathInfo<NODE>() { Path = path, CameFrom = cameFrom };
		}

		/// <summary>
		/// Dijkstra’s Algorithm (a.k.a. Uniform Cost Search
		/// </summary>
		/// <typeparam name="NODE">Type of a graph node</typeparam>
		/// <param name="start">node of the path</param>
		/// <param name="goal">node of the path</param>
		/// <param name="neighborNodes">functor that returns all neighbors for a given node</param>
		/// <param name="edgeCost">returns the edge cost netween two nodes</param>
		/// <param name="nullNode">is an empty, non existing node. Needed to signify the predecessor of <paramref name="start"/></param>
		/// <returns></returns>
		public static IEnumerable<PathInfo<NODE>> Dijkstra<NODE>(NODE start, NODE goal, Func<NODE, IEnumerable<NODE>> neighborNodes
			, Func<NODE, NODE, float> edgeCost, NODE nullNode)
		{
			if (neighborNodes is null) throw new ArgumentNullException(nameof(neighborNodes));
			if (edgeCost == null) throw new ArgumentNullException(nameof(edgeCost));
			var path = new List<NODE>();
			var frontier = new PriorityQueue<PriorityQueueElement<NODE>>();
			frontier.Enqueue(new PriorityQueueElement<NODE>(0, start));
			var cameFrom = new Dictionary<NODE, NODE>();
			var costSoFar = new Dictionary<NODE, float>();
			cameFrom[start] = nullNode;
			costSoFar[start] = 0;
			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				if (current.Node.Equals(goal))
				{
					frontier.Clear();
					path = CreatePath(start, goal, cameFrom);
					break;
				}
				var neighbors = neighborNodes(current.Node);
				foreach (var next in neighbors)
				{
					var newCost = costSoFar[current.Node] + edgeCost(current.Node, next);
					if (!cameFrom.ContainsKey(next) || newCost < costSoFar[next])
					{
						frontier.Enqueue(new PriorityQueueElement<NODE>(newCost, next));
						cameFrom[next] = current.Node;
						costSoFar[next] = newCost;
					}
				}
				yield return new PathInfo<NODE>() { Path = path, CameFrom = cameFrom };
			}
			yield return new PathInfo<NODE>() { Path = path, CameFrom = cameFrom };
		}

		/// <summary>
		/// Greedy Best First Search uses estimated cost to goal for prioritizing expansion of neighbors.
		/// </summary>
		/// <typeparam name="NODE">Type of a graph node</typeparam>
		/// <param name="start">node of the path</param>
		/// <param name="goal">node of the path</param>
		/// <param name="neighborNodes">functor that returns all neighbors for a given node</param>
		/// <param name="costToGoal">functor that returns the estimated cost to the goal for a given node</param>
		/// <param name="nullNode">is an empty, non existing node. Needed to signify the predecessor of <paramref name="start"/></param>
		/// <returns></returns>
		public static IEnumerable<PathInfo<NODE>> GreedyBestFirstSearch<NODE>(NODE start, NODE goal, Func<NODE, IEnumerable<NODE>> neighborNodes
			, Func<NODE, float> costToGoal, NODE nullNode)
		{
			if (neighborNodes is null) throw new ArgumentNullException(nameof(neighborNodes));
			if (costToGoal == null) throw new ArgumentNullException(nameof(costToGoal));
			var path = new List<NODE>();
			var frontier = new PriorityQueue<PriorityQueueElement<NODE>>();
			var cameFrom = new Dictionary<NODE, NODE>();
			frontier.Enqueue(new PriorityQueueElement<NODE>(0f, start));
			cameFrom[start] = nullNode;
			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				if (current.Node.Equals(goal))
				{
					frontier.Clear();
					path = CreatePath(start, goal, cameFrom);
					break;
				}
				var neighbors = neighborNodes(current.Node);
				foreach (var neighbor in neighbors)
				{
					var newCost = costToGoal(neighbor);
					if (!cameFrom.ContainsKey(neighbor))
					{
						frontier.Enqueue(new PriorityQueueElement<NODE>(newCost, neighbor));
						cameFrom[neighbor] = current.Node;
					}
				}
				yield return new PathInfo<NODE>() { Path = path, CameFrom = cameFrom };
			}
			yield return new PathInfo<NODE>() { Path = path, CameFrom = cameFrom };
		}

		/// <summary>
		/// A* search
		/// </summary>
		/// <typeparam name="NODE"></typeparam>
		/// <typeparam name="NODE">Type of a graph node</typeparam>
		/// <param name="start">node of the path</param>
		/// <param name="goal">node of the path</param>
		/// <param name="neighborNodes">functor that returns all neighbors for a given node</param>
		/// <param name="edgeCost">returns the edge cost netween two nodes</param>
		/// <param name="costToGoal">functor that returns the estimated cost to the goal for a given node</param>
		/// <param name="nullNode">is an empty, non existing node. Needed to signify the predecessor of <paramref name="start"/></param>
		/// <returns></returns>
		public static IEnumerable<PathInfo<NODE>> AStarSearch<NODE>(NODE start, NODE goal, Func<NODE, IEnumerable<NODE>> neighborNodes
			, Func<NODE, NODE, float> edgeCost, Func<NODE, float> costToGoal, NODE nullNode)
		{
			if (neighborNodes is null) throw new ArgumentNullException(nameof(neighborNodes));
			if (edgeCost == null) throw new ArgumentNullException(nameof(edgeCost));

			var path = new List<NODE>();
			var frontier = new PriorityQueue<PriorityQueueElement<NODE>>();
			var cameFrom = new Dictionary<NODE, NODE>();
			var costSoFar = new Dictionary<NODE, float>();
			frontier.Enqueue(new PriorityQueueElement<NODE>(0f, start));
			cameFrom[start] = nullNode;
			costSoFar[start] = 0;
			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				if (current.Node.Equals(goal))
				{
					frontier.Clear();
					path = CreatePath(start, goal, cameFrom);
					break;
				}
				var neighbors = neighborNodes(current.Node);
				foreach (var next in neighbors)
				{
					var newCost = costSoFar[current.Node] + edgeCost(current.Node, next);
					if (!cameFrom.ContainsKey(next) || newCost < costSoFar[next])
					{
						var priority = newCost + costToGoal(next);
						frontier.Enqueue(new PriorityQueueElement<NODE>(priority, next));
						cameFrom[next] = current.Node;
						costSoFar[next] = newCost;
					}
				}
				yield return new PathInfo<NODE>() { Path = path, CameFrom = cameFrom };
			}
			yield return new PathInfo<NODE>() { Path = path, CameFrom = cameFrom };
		}
	}
}
