using OpenTK;

namespace Example.View
{
	public static class Viewport
	{
		public static Vector2 Transform(this Rectangle viewport, Point point)
		{
			var o = viewport.Location;
			var coord01 = new Vector2((point.X - o.X) / (viewport.Width - 1f), 1f - (point.Y - o.Y) / (viewport.Height - 1f));
			return coord01 * 2f - Vector2.One;
		}
	}
}
