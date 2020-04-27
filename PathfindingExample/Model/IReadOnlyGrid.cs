namespace Example.Model
{
	public interface IReadOnlyGrid
	{
		bool IsPassable(ushort column, ushort row);

		ushort Columns { get; }
		ushort Rows { get; }
	}
}
