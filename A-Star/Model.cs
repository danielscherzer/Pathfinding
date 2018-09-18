using System;
using System.Collections.Generic;

namespace Example
{
	class Model
	{
		const int scale = 8;
		private readonly Grid grid = new Grid(32 * scale, 18 * scale);
		private readonly Random rnd;

		private delegate IEnumerable<PathInfo<Coord>> Algorithm(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors);
		private readonly IReadOnlyList<Algorithm> algorithms;
		private int algorithmIndex = 0;

		public string AlgorithmName => algorithms[algorithmIndex].Method.Name;
		public IGrid Grid => grid;
		public Coord Start { get; private set; } = new Coord(0, 0);
		public Coord Goal { get; private set; } = new Coord(254, 140);

		public Model()
		{
			rnd = new Random(12);

			CreateRandomWalkObstacles();
			//NewStartGoal();

			algorithms = new List<Algorithm>
			{
				AStarSearch,
				AStarSearchStraight,
				GreedyBestFirstSearch,
				BreathFirstSearch,
				UniformCostSearch,
			};
		}

		private void CreateRandomWalkObstacles()
		{
			var obstacles = Grid.Width * Grid.Height / 2;
			var walkLen = 4 * Math.Min(Grid.Width, Grid.Height);
			for (int i = 0; i < obstacles; i += walkLen)
			{
				//do a random walk with length walkLen
				var x = (ushort)rnd.Next(Grid.Width);
				var y = (ushort)rnd.Next(Grid.Height);
				grid[x, y] = false;
				for (int j = 0; j < walkLen; ++j)
				{
					//choose a random direction
					var dir = rnd.Next(4);
					switch (dir)
					{
						case 0: ++x; break;
						case 1: ++y; break;
						case 2: --x; break;
						case 3: --y; break;
							//case 4: ++x; ++y; break;
					}
					grid[x, y] = false;
				}
			}
		}

		internal void NextAlgorithm()
		{
			algorithmIndex = (algorithmIndex + 1) % algorithms.Count;
		}

		internal void ToggleElement(ushort x, ushort y)
		{
			grid[x, y] = !grid[x, y];
		}

		internal IEnumerable<PathInfo<Coord>> FindPath()
		{
			var algorithm = algorithms[algorithmIndex];

			IEnumerable<Coord> Walkables(IEnumerable<Coord> points)
			{
				foreach (var p in points) if (grid.IsPassable(p.X, p.Y)) yield return p;
			}

			IEnumerable<Coord> WalkableNeighbors(Coord current) => Walkables(Get8Neighbors(current));

			foreach(var step in algorithm(Start, Goal, WalkableNeighbors)) yield return step;
		}

		internal void NewStartGoal()
		{
			Coord FindRandomPassablePosition()
			{
				Coord pos;
				do
				{
					pos = new Coord(rnd.Next(Grid.Width), rnd.Next(Grid.Height));
				} while (!Grid.IsPassable(pos.X, pos.Y));
				return pos;
			}

			Start = FindRandomPassablePosition();
			Goal = FindRandomPassablePosition();
		}

		private float GridCost(Coord a, Coord b)
		{
			var dx = Math.Abs(a.X - b.X);
			var dy = Math.Abs(a.Y - b.Y);
			var D2 = MathF.Sqrt(2f);
			return dx + dy > 1 ? D2: 1f;
		}

		private static float ManhattanDistance(Coord a, Coord b)
		{
			var dx = Math.Abs(a.X - b.X);
			var dy = Math.Abs(a.Y - b.Y);
			return dx + dy;
		}

		private static float DiagonalDistance(Coord a, Coord b)
		{
			var dx = Math.Abs(a.X - b.X);
			var dy = Math.Abs(a.Y - b.Y);
			var D = 1f;
			var D2 = MathF.Sqrt(2f);
			return D * (dx + dy) + (D2 - 2f * D) * Math.Min(dx, dy);
		}

		private static float Straightness(in Coord start, in Coord current, in Coord goal)
		{
			var dx1 = current.X - goal.X;
			var dy1 = current.Y - goal.Y;
			var dx2 = start.X - goal.X;
			var dy2 = start.Y - goal.Y;
			return Math.Abs(dx1 * dy2 - dx2 * dy1);
		}

		private IEnumerable<PathInfo<Coord>> BreathFirstSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			return PathFinding.BreathFirstSearch(start, goal, walkableNeighbors, () => Coord.None);
		}

		private IEnumerable<PathInfo<Coord>> UniformCostSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			return PathFinding.UniformCostSearch(start, goal, walkableNeighbors, GridCost, () => Coord.None);
		}

		private IEnumerable<PathInfo<Coord>> GreedyBestFirstSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			return PathFinding.GreedyBestFirstSearch(start, goal, walkableNeighbors, (a) => DiagonalDistance(a, goal), () => Coord.None);
		}

		private IEnumerable<PathInfo<Coord>> AStarSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			float h(Coord a) => DiagonalDistance(a, goal);
			return PathFinding.AStarSearch(start, goal, walkableNeighbors, GridCost, h, () => Coord.None);
		}

		private IEnumerable<PathInfo<Coord>> AStarSearchStraight(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			//use straightness as tie breaker
			float h(Coord a) => DiagonalDistance(a, goal) + Straightness(start, a, goal) * 0.001f;
			return PathFinding.AStarSearch(start, goal, walkableNeighbors, GridCost, h, () => Coord.None);
		}

		private IEnumerable<Coord> Get4Neighbors(Coord pos)
		{
			var top = pos.Y < grid.Height - 1;
			var bottom = 0 < pos.Y;
			if (top) yield return new Coord(pos.X, pos.Y + 1);
			if (bottom) yield return new Coord(pos.X, pos.Y - 1);
			var left = 0 < pos.X;
			if (left) yield return new Coord(pos.X - 1, pos.Y);
			var right = pos.X < grid.Width - 1;
			if (right) yield return new Coord(pos.X + 1, pos.Y);
		}

		private IEnumerable<Coord> Get8Neighbors(Coord pos)
		{
			var top = pos.Y < grid.Height - 1;
			var bottom = 0 < pos.Y;
			if (top) yield return new Coord(pos.X, pos.Y + 1);
			if (bottom) yield return new Coord(pos.X, pos.Y - 1);
			var left = 0 < pos.X;
			if (left)
			{
				yield return new Coord(pos.X - 1, pos.Y);
				if (top) yield return new Coord(pos.X - 1, pos.Y + 1);
				if (bottom) yield return new Coord(pos.X - 1, pos.Y - 1);
			}
			var right = pos.X < grid.Width - 1;
			if (right)
			{
				yield return new Coord(pos.X + 1, pos.Y);
				if (top) yield return new Coord(pos.X + 1, pos.Y + 1);
				if (bottom) yield return new Coord(pos.X + 1, pos.Y - 1);
			}
		}
	}
}
