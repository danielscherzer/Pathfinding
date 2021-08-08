using System;
using Zenseless.Spatial;

namespace Example.Model
{
	public static class GridInitializers
	{
		public static void CreateMazeObstacles(this Grid<bool> grid)
		{
			var rnd = new Random(24);
			//full of obstacles
			for (ushort x = 0; x < grid.Columns; ++x)
			{
				for (ushort y = 0; y < grid.Rows; ++y)
				{
					grid[x, y] = false;
				}
			}
			var openSpace = grid.Columns * grid.Rows / 2;
			var walkLen = Math.Min(grid.Columns, grid.Rows) / 4;
			for (int i = 0; i < openSpace; i += walkLen)
			{
				//do a random walk with length walkLen
				var x = (ushort)rnd.Next(grid.Columns);
				var y = (ushort)rnd.Next(grid.Rows);
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
					grid.SetModulo(x, y, true);
				}
			}
		}
		public static void CreateRandomWalkObstacles(this Grid<bool> grid)
		{
			var rnd = new Random(24);
			//start with no obstacles
			for (ushort x = 0; x < grid.Columns; ++x)
			{
				for (ushort y = 0; y < grid.Rows; ++y)
				{
					grid[x, y] = true;
				}
			}
			var obstacles = grid.Columns * grid.Rows / 2;
			var walkLen = 4 * Math.Min(grid.Columns, grid.Rows);
			for (int i = 0; i < obstacles; i += walkLen)
			{
				//do a random walk with length walkLen
				var x = (ushort)rnd.Next(grid.Columns);
				var y = (ushort)rnd.Next(grid.Rows);
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
					grid.SetModulo(x, y, false);
				}
			}
		}

		private static void SetModulo(this Grid<bool> grid, ushort x, ushort y, bool value)
		{
			grid[(ushort)(x % grid.Columns), (ushort)(y % grid.Rows)] = value;
		}
	}
}
