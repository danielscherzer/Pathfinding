namespace Example
{
	using ElementType = System.UInt16;

	public struct Coord
	{
		public Coord(int column, int row)
		{
			Column = (ElementType)column;
			Row = (ElementType)row;
		}

		public Coord(ElementType column, ElementType row) : this()
		{
			Column = column;
			Row = row;
		}

		public ElementType Column { get; }
		public ElementType Row { get; }

		public override string ToString()
		{
			return $"({Column}, {Row})";
		}
	}
}
