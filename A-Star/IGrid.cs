namespace Example
{
	public interface IGrid
	{
		bool IsPassable(ushort x, ushort y);

		ushort Height { get; }
		ushort Width { get; }
	}
}
