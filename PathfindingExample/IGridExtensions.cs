using System;
using System.Collections.Generic;

namespace Example
{
	public static class IGridExtensions
	{
		public static IEnumerable<Coord> Get4Neighbors(this IGrid grid, Coord pos)
		{
			var top = pos.Row < grid.Rows - 1;
			var bottom = 0 < pos.Row;
			if (top) yield return new Coord(pos.Column, pos.Row + 1);
			if (bottom) yield return new Coord(pos.Column, pos.Row - 1);
			var left = 0 < pos.Column;
			if (left) yield return new Coord(pos.Column - 1, pos.Row);
			var right = pos.Column < grid.Columns - 1;
			if (right) yield return new Coord(pos.Column + 1, pos.Row);
		}

		public static IEnumerable<Coord> Get8Neighbors(this IGrid grid, Coord pos)
		{
			var top = pos.Row < grid.Rows - 1;
			var bottom = 0 < pos.Row;
			if (top) yield return new Coord(pos.Column, pos.Row + 1);
			if (bottom) yield return new Coord(pos.Column, pos.Row - 1);
			var left = 0 < pos.Column;
			if (left)
			{
				yield return new Coord(pos.Column - 1, pos.Row);
				if (top) yield return new Coord(pos.Column - 1, pos.Row + 1);
				if (bottom) yield return new Coord(pos.Column - 1, pos.Row - 1);
			}
			var right = pos.Column < grid.Columns - 1;
			if (right)
			{
				yield return new Coord(pos.Column + 1, pos.Row);
				if (top) yield return new Coord(pos.Column + 1, pos.Row + 1);
				if (bottom) yield return new Coord(pos.Column + 1, pos.Row - 1);
			}
		}

		public static float GridStepCost(Coord a, Coord b)
		{
			var dx = Math.Abs(a.Column - b.Column);
			var dy = Math.Abs(a.Row - b.Row);
			var D2 = MathF.Sqrt(2f);
			return dx + dy > 1 ? D2 : 1f;
		}
	}
}
