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

		public bool this[ushort x, ushort y]
		{
			get { return grid[x, y]; }
			set { grid[x % Width, y % Height] = value; }
		}

		public ushort Height => (ushort)grid.GetLength(1);

		public ushort Width => (ushort)grid.GetLength(0);

		public bool IsPassable(ushort x, ushort y) => grid[x, y];
	}
}
