namespace Example
{
	public interface IGrid
	{
		bool IsPassable(ushort column, ushort row);

		ushort Columns { get; }
		ushort Rows { get; }
	}
}
