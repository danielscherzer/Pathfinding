using System;
using System.Collections.Generic;
using System.Linq;
using Zenseless.PathFinder.Grid;
using Zenseless.Patterns;
using Zenseless.Spatial;
using static Example.Model.GridPathFinderAlgorithms;

namespace Example.Model;

internal class Model
{
	public Model()
	{
		rnd = new Random(24);
		List<AlgorithmEvaluation> InvalidateAlgorithms()
		{
			var algorithms = new Algorithm[]
			{
			BreathFirstSearch,
			DijkstraSearch,
			AStarSearch,
			AStarSearchStraight,
			GreedyBestFirstSearch,
			};
			return algorithms.Select(algo => new AlgorithmEvaluation(algo, Grid, Start, Goal)).ToList();
		}
		algorithmEvaluations = new(InvalidateAlgorithms);
		NewGrid(100, 100);
	}

	internal IReadOnlyList<AlgorithmEvaluation> AlgorithmEvaluations => algorithmEvaluations.Value;
	
	internal AlgorithmEvaluation CurrentEvaluation => AlgorithmEvaluations[AlgorithmIndex];
	
	internal IReadOnlyGrid<bool> Grid => grid;

	internal Coord Start
	{
		get => start; 
		set
		{
			start = value;
			algorithmEvaluations.Invalidate();
		}
	}
	
	internal Coord Goal
	{
		get => goal;
		set
		{
			goal = value;
			algorithmEvaluations.Invalidate();
		}
	}

	public int AlgorithmIndex
	{
		get => _algorithmIndex;
		set
		{
			if (value >= AlgorithmEvaluations.Count) return;
			if (value < 0) return;
			_algorithmIndex = value;
			algorithmEvaluations.Invalidate();
		}
	}

	internal Coord FindRandomPassablePosition()
	{
		Coord pos;
		do
		{
			pos = new Coord(rnd.Next(Grid.Columns), rnd.Next(Grid.Rows));
		} while (!Grid[pos.Column, pos.Row]);
		return pos;
	}

	internal void NewGrid(int columns, int rows)
	{
		grid = new(columns, rows);
		grid.CreateRandomWalkObstacles(rnd.Next());
		//grid.CreateMazeObstacles(rnd.Next());
		Start = FindRandomPassablePosition();
		Goal = FindRandomPassablePosition();
	}

	internal void Update()
	{
		if (CurrentEvaluation.StepMode) return;
		var nextId = rnd.Next(AlgorithmEvaluations.Count);
		AlgorithmEvaluations[nextId].FindPath();
	}

	internal void ToggleElement(ushort x, ushort y)
	{
		grid[x, y] = !grid[x, y];
		algorithmEvaluations.Invalidate();
	}

	private Grid<bool> grid;
	private readonly Random rnd;
	private int _algorithmIndex = 0;
	private Coord start = new(0, 0);
	private Coord goal = new(254, 140);
	private readonly DirtyFlag<List<AlgorithmEvaluation>> algorithmEvaluations;
}
