using System;

namespace PathFinder.Grid
{
	public static class GridCostFunctions
	{
		public static float DiagonalDistance(Coord a, Coord b, float avgCost = 1f)
		{
			var dx = Math.Abs(a.Column - b.Column);
			var dy = Math.Abs(a.Row - b.Row);
			var D2 = MathF.Sqrt(2f);
			return avgCost * (dx + dy) + (D2 - 2f * avgCost) * Math.Min(dx, dy);
		}

		public static float ManhattanDistance(Coord a, Coord b, float avgCost = 1f)
		{
			var dx = Math.Abs(a.Column - b.Column);
			var dy = Math.Abs(a.Row - b.Row);
			return avgCost * (dx + dy);
		}

		public static float Straightness(in Coord start, in Coord current, in Coord goal)
		{
			var dx1 = current.Column - goal.Column;
			var dy1 = current.Row - goal.Row;
			var dx2 = start.Column - goal.Column;
			var dy2 = start.Row - goal.Row;
			return Math.Abs(dx1 * dy2 - dx2 * dy1);
		}
	}
}
