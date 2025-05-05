using System;
using System.Collections.Generic;
using System.Diagnostics;
using Zenseless.PathFinder;
using Zenseless.PathFinder.Grid;
using Zenseless.Spatial;
using static Example.Model.GridPathFinderAlgorithms;

namespace Example.Model;

internal class AlgorithmEvaluation(Algorithm algorithm, IReadOnlyGrid<bool> grid, Coord start, Coord goal)
{
	internal string AlgorithmName => algorithm.Method.Name;
	public PathInfo<Coord> Path { get; private set; } = PathInfo<Coord>.CreateEmpty();
	public bool StepMode => iterator != null;
	public TimeSpan EvaluationTime => stopWatch.Elapsed;
	public double Avg => sum / count;

	private IEnumerator<PathInfo<Coord>> iterator;
	private readonly Stopwatch stopWatch = new();
	private double sum;
	private int count;

	internal void FindNextStep()
	{
		iterator ??= StepFindPath().GetEnumerator();
		if (iterator.MoveNext())
		{
			Path = iterator.Current;
		}
		else
		{
			iterator = null;
		}
	}

	internal void FindPath()
	{
		if (StepMode) return;
		stopWatch.Restart();
		foreach (var step in StepFindPath())
		{
			Path = step;
		}
		stopWatch.Stop();
		var value = stopWatch.Elapsed.TotalMilliseconds;
		sum += value;
		++count;
	}

	private IEnumerable<PathInfo<Coord>> StepFindPath()
	{
		IEnumerable<Coord> WalkableNeighbors(Coord current)
		{
			foreach (var neighbor in current.Get8Neighbors(grid.Columns, grid.Rows))
			{
				// Test if location is walkable
				if (grid[neighbor.Column, neighbor.Row]) yield return neighbor;
			}
		}
		foreach (var step in algorithm(start, goal, WalkableNeighbors)) yield return step;
	}
}
