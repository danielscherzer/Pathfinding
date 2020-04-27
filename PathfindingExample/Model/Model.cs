using PathFinder;
using PathFinder.Grid;
using System;
using System.Collections.Generic;
using static Example.Model.GridPathFinderAlgorithms;

namespace Example.Model
{
	class Model
	{
		const ushort scale = 8;
		private readonly BoolGrid grid = new BoolGrid(32 * scale, 18 * scale);
		private readonly Random rnd;

		private IEnumerator<PathInfo<Coord>> iterator;
		private readonly IReadOnlyList<Algorithm> algorithms;
		private int algorithmIndex = 0;

		public string AlgorithmName => algorithms[algorithmIndex].Method.Name;
		public IReadOnlyGrid Grid => grid;

		public Coord Start { get; private set; } = new Coord(0, 0);
		public Coord Goal { get; private set; } = new Coord(254, 140);
		public PathInfo<Coord> Path { get; private set; }

		public Model()
		{
			rnd = new Random(24);

			grid.CreateRandomWalkObstacles();
			//grid.CreateMazeObstacles();
			//NewStartGoal();

			algorithms = new List<Algorithm>
			{
				AStarSearch,
				AStarSearchStraight,
				GreedyBestFirstSearch,
				BreathFirstSearch,
				DijkstraSearch,
			};

			NewStartGoal();
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

			IEnumerable<Coord> WalkableNeighbors(Coord current)
			{
				foreach(var neighbor in current.Get8Neighbors(grid.Columns, grid.Rows))
				{
					if (grid.IsPassable(neighbor.Column, neighbor.Row)) yield return neighbor;
				}
			}

			foreach (var step in algorithm(Start, Goal, WalkableNeighbors)) yield return step;
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
