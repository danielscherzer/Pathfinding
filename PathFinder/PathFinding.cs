using System;
using System.Collections.Generic;

namespace PathFinder
{
	public class PathFinding
	{
		public static List<NODE> CreatePath<NODE>(NODE start, NODE goal, IReadOnlyDictionary<NODE, NODE> cameFrom)
		{
			var path = new List<NODE>();
			var current = goal;
			while (!current.Equals(start))
			{
				path.Add(current);
				if (!cameFrom.TryGetValue(current, out var value))
				{
					break;
				}
				current = cameFrom[current];
			}
			path.Add(start);
			path.Reverse();
			return path;
		}
		
		public static IEnumerable<PathInfo<NODE>> BreathFirstSearch<NODE>(NODE start, NODE goal
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
				yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
			}
			yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
		}

		public static IEnumerable<PathInfo<NODE>> UniformCostSearch<NODE>(NODE start, NODE goal, Func<NODE, IEnumerable<NODE>> neighborNodes
			, Func<NODE, NODE, float> edgeCost, NODE nullNode)
		{
			if (neighborNodes is null) throw new ArgumentNullException(nameof(neighborNodes));
			if (edgeCost == null) throw new ArgumentNullException(nameof(edgeCost));
			var path = new List<NODE>();
			var frontier = new PriorityQueue<PriorityPair<NODE>>();
			var cameFrom = new Dictionary<NODE, NODE>();
			var costSoFar = new Dictionary<NODE, float>();
			frontier.Enqueue(new PriorityPair<NODE>(0, start));
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
						frontier.Enqueue(new PriorityPair<NODE>(newCost, next));
						cameFrom[next] = current.Node;
						costSoFar[next] = newCost;
					}
				}
				yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
			}
			yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
		}

		public static IEnumerable<PathInfo<NODE>> GreedyBestFirstSearch<NODE>(NODE start, NODE goal, Func<NODE, IEnumerable<NODE>> neighborNodes
			, Func<NODE, float> costToGoal, NODE nullNode)
		{
			if (neighborNodes is null) throw new ArgumentNullException(nameof(neighborNodes));
			if (costToGoal == null) throw new ArgumentNullException(nameof(costToGoal));
			var path = new List<NODE>();
			var frontier = new PriorityQueue<PriorityPair<NODE>>();
			var cameFrom = new Dictionary<NODE, NODE>();
			frontier.Enqueue(new PriorityPair<NODE>(0f, start));
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
						frontier.Enqueue(new PriorityPair<NODE>(newCost, neighbor));
						cameFrom[neighbor] = current.Node;
					}
				}
				yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
			}
			yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
		}

		public static IEnumerable<PathInfo<NODE>> AStarSearch<NODE>(NODE start, NODE goal, Func<NODE, IEnumerable<NODE>> neighborNodes
			, Func<NODE, NODE, float> edgeCost, Func<NODE, float> costToGoal, NODE nullNode)
		{
			if (neighborNodes is null) throw new ArgumentNullException(nameof(neighborNodes));
			if (edgeCost == null) throw new ArgumentNullException(nameof(edgeCost));

			var path = new List<NODE>();
			var frontier = new PriorityQueue<PriorityPair<NODE>>();
			var cameFrom = new Dictionary<NODE, NODE>();
			var costSoFar = new Dictionary<NODE, float>();
			frontier.Enqueue(new PriorityPair<NODE>(0f, start));
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
						frontier.Enqueue(new PriorityPair<NODE>(priority, next));
						cameFrom[next] = current.Node;
						costSoFar[next] = newCost;
					}
				}
				yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
			}
			yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
		}
	}
}
