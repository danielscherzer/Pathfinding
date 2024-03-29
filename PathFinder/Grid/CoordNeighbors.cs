﻿using System.Collections.Generic;

namespace Zenseless.PathFinder.Grid
{
	public static class CoordNeighbors
	{
		public static IEnumerable<Coord> Get4Neighbors(this Coord pos, int columns, int rows)
		{
			var top = pos.Row < rows - 1;
			var bottom = 0 < pos.Row;
			if (top) yield return new Coord(pos.Column, pos.Row + 1);
			if (bottom) yield return new Coord(pos.Column, pos.Row - 1);
			var left = 0 < pos.Column;
			if (left) yield return new Coord(pos.Column - 1, pos.Row);
			var right = pos.Column < columns - 1;
			if (right) yield return new Coord(pos.Column + 1, pos.Row);
		}

		public static IEnumerable<Coord> Get8Neighbors(this Coord pos, int columns, int rows)
		{
			var top = pos.Row < rows - 1;
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
			var right = pos.Column < columns - 1;
			if (right)
			{
				yield return new Coord(pos.Column + 1, pos.Row);
				if (top) yield return new Coord(pos.Column + 1, pos.Row + 1);
				if (bottom) yield return new Coord(pos.Column + 1, pos.Row - 1);
			}
		}
	}
}
