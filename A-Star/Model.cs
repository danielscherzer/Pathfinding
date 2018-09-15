using System;
using System.Collections.Generic;

namespace Example
{
	class Model
	{
		const int scale = 8;
		private readonly Grid grid = new Grid(32 * scale, 18 * scale);
		private readonly Random rnd;

		private delegate PathInfo Algorithm(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors);
		private readonly IReadOnlyList<Algorithm> algorithms;
		private int algorithmIndex = 0;

		public string AlgorithmName => algorithms[algorithmIndex].Method.Name;
		public IGrid Grid => grid;
		public Coord Start { get; private set; } = new Coord(13, 10);
		public Coord Goal { get; private set; } = new Coord(10, 5);

		public Model()
		{
			rnd = new Random(12);
			var obstacles = Grid.Width * Grid.Height / 20;
			for (int i = 0; i < obstacles; ++i)
			{
				var x = rnd.Next(Grid.Width);
				var y = rnd.Next(Grid.Height);
				grid[x, y] = false;
			}
			NewStartGoal();

			algorithms = new List<Algorithm>
			{
				PathFinding.BreathFirstSearch,
				UniformCostSearch
			};
		}

		internal void NextAlgorithm()
		{
			algorithmIndex = (algorithmIndex + 1) % algorithms.Count;
		}

		internal void ToggleElement(int x, int y)
		{
			grid[x, y] = !grid[x, y];
		}

		internal PathInfo FindPath()
		{
			var algorithm = algorithms[algorithmIndex];

			IEnumerable<Coord> Walkables(IEnumerable<Coord> points)
			{
				foreach (var p in points) if (grid.IsPassable(p.X, p.Y)) yield return p;
			}

			IEnumerable<Coord> WalkableNeighbors(Coord current) => Walkables(Get8Neighbors(current));

			return algorithm(Start, Goal, WalkableNeighbors);
		}

		internal void NewStartGoal()
		{
			Start = new Coord(rnd.Next(Grid.Width), rnd.Next(Grid.Height));
			Goal = new Coord(rnd.Next(Grid.Width), rnd.Next(Grid.Height));
		}

		private PathInfo UniformCostSearch(Coord start, Coord goal, Func<Coord, IEnumerable<Coord>> walkableNeighbors)
		{
			return PathFinding.UniformCostSearch(start, goal, walkableNeighbors, (a, b) => 1);
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
