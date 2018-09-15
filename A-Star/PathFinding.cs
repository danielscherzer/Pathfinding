using System;
using System.Collections.Generic;

namespace Example
{
	class PathFinding
	{
		public static List<Coord> CreatePath(Coord start, Coord goal, IReadOnlyDictionary<Coord, Coord> cameFrom)
		{
			var path = new List<Coord>();
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

		public static PathInfo BreathFirstSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			if (walkableNeighbors is null) throw new ArgumentNullException(nameof(walkableNeighbors));
			var path = new List<Coord>();

			var frontier = new Queue<Coord>();
			var cameFrom = new Dictionary<Coord, Coord>();
			frontier.Enqueue(start);
			cameFrom[start] = new Coord(-1, -1);
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
			}
			return new PathInfo() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
		}

		public static PathInfo UniformCostSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors, Func<Coord, Coord, int> cost)
		{
			if (walkableNeighbors is null) throw new ArgumentNullException(nameof(walkableNeighbors));
			if (cost == null) throw new ArgumentNullException(nameof(cost));
			var path = new List<Coord>();
			var frontier = new PriorityQueue<PriorityNode>();
			var cameFrom = new Dictionary<Coord, Coord>();
			var costSoFar = new Dictionary<Coord, int>();
			frontier.Enqueue(new PriorityNode(0, start));
			cameFrom[start] = Coord.None;
			costSoFar[start] = 0;
			while (frontier.Count > 0)
			{
				var current = frontier.Dequeue();
				if (current.Equals(goal))
				{
					frontier.Clear();
					path = CreatePath(start, goal, cameFrom);
					break;
				}
				var neighbors = walkableNeighbors(current.Coord);
				foreach (var next in neighbors)
				{
					var newCost = costSoFar[current.Coord] + cost(current.Coord, next);
					if (!cameFrom.ContainsKey(next) || newCost < costSoFar[next])
					{
						frontier.Enqueue(new PriorityNode(newCost, next));
						cameFrom[next] = current.Coord;
						costSoFar[next] = newCost;
					}
				}
			}
			return new PathInfo() { Path = path, Visited = cameFrom.Keys, CameFrom = cameFrom };
		}
	}
}
