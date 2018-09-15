namespace Example
{
	internal class Grid : IGrid
	{
		private bool[,] grid;

		public Grid(int width, int height)
		{
			grid = new bool[width, height];
			for (int x = 0; x < Width; ++x)
			{
				for (int y = 0; y < Height; ++y)
				{
					grid[x, y] = true;
				}
			}
		}

		public bool this[int x, int y]
		{
			get { return grid[x, y]; }
			set { grid[x, y] = value; }
		}

		public int Height => grid.GetLength(1);

		public int Width => grid.GetLength(0);

		public bool IsPassable(int x, int y) => grid[x, y];
	}
}
