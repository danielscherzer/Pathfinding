using OpenTK;

namespace Example
{
	public static class Viewport
	{
		public static Point FromWindow(this Rectangle viewport, Point point)
		{
			var o = viewport.Location;
			return new Point(point.X - o.X, point.Y - o.Y);
		}

		public static Vector2 FromWindowF(this Rectangle viewport, Point point)
		{
			point = FromWindow(viewport, point);
			var coord01 = new Vector2(point.X / (viewport.Width - 1f), 1f - point.Y / (viewport.Height - 1f));
			return coord01 * 2f - Vector2.One;
		}
	}
}
