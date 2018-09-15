namespace Example
{
	public interface IGrid
	{
		bool IsPassable(int x, int y);

		int Height { get; }
		int Width { get; }
	}
}
