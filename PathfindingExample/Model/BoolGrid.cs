namespace Example.Model
{
	public class BoolGrid : IReadOnlyGrid
	{
		private readonly bool[,] grid;

		public BoolGrid(ushort columns, ushort rows)
		{
			grid = new bool[columns, rows];
			for (int x = 0; x < Columns; ++x)
			{
				for (int y = 0; y < Rows; ++y)
				{
					grid[x, y] = true;
				}
			}
		}

		public bool this[ushort x, ushort y]
		{
			get { return grid[x, y]; }
			set { grid[x, y] = value; }
		}

		public ushort Rows => (ushort)grid.GetLength(1);

		public ushort Columns => (ushort)grid.GetLength(0);

		public bool IsPassable(ushort x, ushort y) => grid[x, y];
	}
}
