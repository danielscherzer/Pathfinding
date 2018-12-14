namespace Example
{
	using Type = System.UInt16;

	struct Coord 
	{
		public Coord(int x, int y) : this()
		{
			X = (Type)x;
			Y = (Type)y;
		}

		public Coord(Type x, Type y) : this()
		{
			X = x;
			Y = y;
		}

		public Type X { get; }
		public Type Y { get; }

		public static Coord None => new Coord(Type.MaxValue, Type.MaxValue);

		public override string ToString()
		{
			return $"({X}, {Y})";
		}
	}
}
