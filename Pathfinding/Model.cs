using PathFinder;
using System;
using System.Collections.Generic;
using static Example.GridPathFinder;

namespace Example
{
	class Model
	{
		const ushort scale = 4;
		private readonly Grid grid = new Grid(32 * scale, 18 * scale);
		private readonly Random rnd;

		private IEnumerator<PathInfo<Coord>> iterator;
		private readonly IReadOnlyList<Algorithm> algorithms;
		private int algorithmIndex = 0;

		public string AlgorithmName => algorithms[algorithmIndex].Method.Name;
		public IGrid Grid => grid;

		public Coord Start { get; private set; } = new Coord(0, 0);
		public Coord Goal { get; private set; } = new Coord(254, 140);
		public PathInfo<Coord> Path { get; private set; }

		public Model()
		{
			rnd = new Random(12);

			CreateRandomWalkObstacles();
			//CreateMazeObstacles();
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

		private void CreateMazeObstacles()
		{
			//full of obstacles
			for(ushort x = 0; x < Grid.Columns; ++x)
			{
				for (ushort y = 0; y < Grid.Rows; ++y)
				{
					grid[x, y] = false;
				}
			}
			var openSpace = Grid.Columns * Grid.Rows / 2;
			var walkLen = Math.Min(Grid.Columns, Grid.Rows) / 4;
			for (int i = 0; i < openSpace; i += walkLen)
			{
				//do a random walk with length walkLen
				var x = (ushort)rnd.Next(Grid.Columns);
				var y = (ushort)rnd.Next(Grid.Rows);
				grid[x, y] = true;
				//choose a random direction
				var dir = rnd.Next(4);
				for (int j = 0; j < walkLen; ++j)
				{
					switch (dir)
					{
						case 0: ++x; break;
						case 1: ++y; break;
						case 2: --x; break;
						case 3: --y; break;
					}
					SetGridModulo(x, y, true);
				}
			}
		}

		internal void SolveMode() => iterator = null;

		internal void Step()
		{
			if (iterator is null)
			{
				iterator = FindPath().GetEnumerator();
			}
			if (iterator.MoveNext())
			{
				Path = iterator.Current;
			}
		}

		internal void Update()
		{
			if (!(iterator is null)) return;
			foreach (var step in FindPath())
			{
				Path = step;
			}
		}

		private void CreateRandomWalkObstacles()
		{
			var obstacles = Grid.Columns * Grid.Rows / 2;
			var walkLen = 4 * Math.Min(Grid.Columns, Grid.Rows);
			for (int i = 0; i < obstacles; i += walkLen)
			{
				//do a random walk with length walkLen
				var x = (ushort)rnd.Next(Grid.Columns);
				var y = (ushort)rnd.Next(Grid.Rows);
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
					SetGridModulo(x, y, false);
				}
			}
		}

		private void SetGridModulo(ushort x, ushort y, bool value)
		{
			grid[(ushort)(x % Grid.Columns), (ushort)(y % Grid.Rows)] = value;
		}

		internal void NextAlgorithm()
		{
			algorithmIndex = (algorithmIndex + 1) % algorithms.Count;
			iterator = null;
		}

		internal void ToggleElement(ushort x, ushort y)
		{
			grid[x, y] = !grid[x, y];
			iterator = null;
		}

		internal IEnumerable<PathInfo<Coord>> FindPath()
		{
			var algorithm = algorithms[algorithmIndex];

			IEnumerable<Coord> Walkables(IEnumerable<Coord> points)
			{
				foreach (var p in points) if (grid.IsPassable(p.Column, p.Row)) yield return p;
			}

			IEnumerable<Coord> WalkableNeighbors(Coord current) => Walkables(grid.Get8Neighbors(current));

			foreach(var step in algorithm(Start, Goal, WalkableNeighbors)) yield return step;
		}

		internal void NewStartGoal()
		{
			Coord FindRandomPassablePosition()
			{
				Coord pos;
				do
				{
					pos = new Coord(rnd.Next(Grid.Columns), rnd.Next(Grid.Rows));
				} while (!Grid.IsPassable(pos.Column, pos.Row));
				return pos;
			}

			Start = FindRandomPassablePosition();
			Goal = FindRandomPassablePosition();
			iterator = null;
		}
	}
}
