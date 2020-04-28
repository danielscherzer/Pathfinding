namespace Zenseless.PathFinder.Grid
{
	using System;
	using ElementType = System.UInt16;

	public struct Coord : IEquatable<Coord>
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

		public bool Equals(Coord other)
		{
			return other.Column == Column && other.Row == Row;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Coord)) return false;
			return Equals((Coord)obj);
		}

		public override int GetHashCode() => System.HashCode.Combine(Column, Row);

		public static bool operator ==(Coord left, Coord right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Coord left, Coord right)
		{
			return !(left == right);
		}
	}
}
