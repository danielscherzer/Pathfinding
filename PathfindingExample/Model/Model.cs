using System;
using System.Collections.Generic;
using Zenseless.PathFinder.Grid;
using Zenseless.Spatial;
using static Example.Model.GridPathFinderAlgorithms;

namespace Example.Model;

internal class Model
{
	private Grid<bool> grid;
	private readonly Random rnd;
	private int _algorithmIndex = 0;
	private readonly List<AlgorithmEvaluation> algorithmEvaluations = new();

	internal IReadOnlyList<AlgorithmEvaluation> AlgorithmEvaluations => algorithmEvaluations;
	internal AlgorithmEvaluation CurrentEvaluation => AlgorithmEvaluations[AlgorithmIndex];
	internal IReadOnlyGrid<bool> Grid => grid;

	internal Coord Start { get; private set; } = new Coord(0, 0);
	internal Coord Goal { get; private set; } = new Coord(254, 140);
	public int AlgorithmIndex
	{
		get => _algorithmIndex;
		set
		{
			if (value >= AlgorithmEvaluations.Count) return;
			if (value < 0) return;
			_algorithmIndex = value;
			InvalidateAlgorithms();
		}
	}

	public Model()
	{
		rnd = new Random(24);

		NewGrid(100, 100);
	}

	internal void Exchange()
	{
		(Start, Goal) = (Goal, Start);
		InvalidateAlgorithms();
	}

	internal void NewGrid(int columns, int rows)
	{
		grid = new(columns, rows);
		grid.CreateRandomWalkObstacles();
		Start = FindRandomPassablePosition();
		Goal = FindRandomPassablePosition();
		InvalidateAlgorithms();
		Update();
	}

	internal void NewGoal()
	{
		Goal = FindRandomPassablePosition();
		InvalidateAlgorithms();
	}

	internal void NewStart()
	{
		Start = FindRandomPassablePosition();
		InvalidateAlgorithms();
	}

	internal void Step()
	{
		AlgorithmEvaluations[AlgorithmIndex].FindNextStep();
	}

	internal void Update()
	{
		if (AlgorithmEvaluations[AlgorithmIndex].StepMode) return;
		var nextId = rnd.Next(AlgorithmEvaluations.Count);
		AlgorithmEvaluations[nextId].FindPath();
	}

	internal void ToggleElement(ushort x, ushort y)
	{
		grid[x, y] = !grid[x, y];
		InvalidateAlgorithms();
	}

	private Coord FindRandomPassablePosition()
	{
		Coord pos;
		do
		{
			pos = new Coord(rnd.Next(Grid.Columns), rnd.Next(Grid.Rows));
		} while (!Grid[pos.Column, pos.Row]);
		return pos;
	}

	private void InvalidateAlgorithms()
	{
		algorithmEvaluations.Clear();
		var algorithms = new List<Algorithm>
		{
			BreathFirstSearch,
			DijkstraSearch,
			AStarSearch,
			AStarSearchStraight,
			GreedyBestFirstSearch,
		};
		foreach (var algo in algorithms)
		{
			algorithmEvaluations.Add(new AlgorithmEvaluation(algo, Grid, Start, Goal));
		};
	}
}
