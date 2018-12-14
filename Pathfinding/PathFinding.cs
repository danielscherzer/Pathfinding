using System;
using System.Collections.Generic;

namespace Example
{
	class PathFinding
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
		
		public static IEnumerable<PathInfo<NODE>> BreathFirstSearch<NODE>(NODE start, NODE goal, Func<NODE, IEnumerable<NODE>> walkableNeighbors, Func<NODE> emptyNode)
		{
			if (walkableNeighbors is null) throw new ArgumentNullException(nameof(walkableNeighbors));
			var path = new List<NODE>();

			var frontier = new Queue<NODE>();
			var cameFrom = new Dictionary<NODE, NODE>();
			frontier.Enqueue(start);
			cameFrom[start] = emptyNode();
			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				if (current.Equals(goal))
				{
					frontier.Clear();
					path = CreatePath(start, goal, cameFrom);
					break;
				}
				var neighbors = walkableNeighbors(current);
				foreach (var next in neighbors)
				{
					if (!cameFrom.ContainsKey(next))
					{
						frontier.Enqueue(next);
						cameFrom[next] = current;
					}
				}
				yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
			}
			yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
		}

		public static IEnumerable<PathInfo<NODE>> UniformCostSearch<NODE>(NODE start, NODE goal, Func<NODE, IEnumerable<NODE>> walkableNeighbors
			, Func<NODE, NODE, float> cost, Func<NODE> emptyNode)
		{
			if (walkableNeighbors is null) throw new ArgumentNullException(nameof(walkableNeighbors));
			if (cost == null) throw new ArgumentNullException(nameof(cost));
			var path = new List<NODE>();
			var frontier = new PriorityQueue<PriorityPair<NODE>>();
			var cameFrom = new Dictionary<NODE, NODE>();
			var costSoFar = new Dictionary<NODE, float>();
			frontier.Enqueue(new PriorityPair<NODE>(0, start));
			cameFrom[start] = emptyNode();
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
				var neighbors = walkableNeighbors(current.Node);
				foreach (var next in neighbors)
				{
					var newCost = costSoFar[current.Node] + cost(current.Node, next);
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

		public static IEnumerable<PathInfo<NODE>> GreedyBestFirstSearch<NODE>(NODE start, NODE goal, Func<NODE, IEnumerable<NODE>> walkableNeighbors
			, Func<NODE, float> costToGoal, Func<NODE> emptyNode)
		{
			if (walkableNeighbors is null) throw new ArgumentNullException(nameof(walkableNeighbors));
			if (costToGoal == null) throw new ArgumentNullException(nameof(costToGoal));
			var path = new List<NODE>();
			var frontier = new PriorityQueue<PriorityPair<NODE>>();
			var cameFrom = new Dictionary<NODE, NODE>();
			frontier.Enqueue(new PriorityPair<NODE>(0f, start));
			cameFrom[start] = emptyNode();
			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				if (current.Node.Equals(goal))
				{
					frontier.Clear();
					path = CreatePath(start, goal, cameFrom);
					break;
				}
				var neighbors = walkableNeighbors(current.Node);
				foreach (var next in neighbors)
				{
					var newCost = costToGoal(next);
					if (!cameFrom.ContainsKey(next))
					{
						frontier.Enqueue(new PriorityPair<NODE>(newCost, next));
						cameFrom[next] = current.Node;
					}
				}
				yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
			}
			yield return new PathInfo<NODE>() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
		}

		public static IEnumerable<PathInfo<NODE>> AStarSearch<NODE>(NODE start, NODE goal, Func<NODE, IEnumerable<NODE>> walkableNeighbors
			, Func<NODE, NODE, float> cost, Func<NODE, float> costToGoal, Func<NODE> emptyNode) where NODE: struct
		{
			if (walkableNeighbors is null) throw new ArgumentNullException(nameof(walkableNeighbors));
			if (cost == null) throw new ArgumentNullException(nameof(cost));
			if (emptyNode == null) throw new ArgumentNullException(nameof(emptyNode));

			var path = new List<NODE>();
			var frontier = new PriorityQueue<PriorityPair<NODE>>();
			var cameFrom = new Dictionary<NODE, NODE>();
			var costSoFar = new Dictionary<NODE, float>();
			frontier.Enqueue(new PriorityPair<NODE>(0f, start));
			cameFrom[start] = emptyNode();
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
				var neighbors = walkableNeighbors(current.Node);
				foreach (var next in neighbors)
				{
					var newCost = costSoFar[current.Node] + cost(current.Node, next);
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
