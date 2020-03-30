using OpenTK;

namespace Example
{
	public static class Viewport
	{
		public static Vector2 Transform(this Rectangle viewport, Point point)
		{
			var o = viewport.Location;
			var coord01 = new Vector2((point.X - o.X) / (viewport.Width - 1f), 1f - (point.Y - o.Y) / (viewport.Height - 1f));
			return coord01 * 2f - Vector2.One;
		}

		public static Vector2 Transform(this RectangleF viewport, Vector2 point)
		{
			var o = viewport.Location;
			var translate = new Vector2(o.X, o.Y);
			var scale = new Vector2(2 / viewport.Width, 2 / viewport.Height);
			return ((point - translate) * scale) + translate;
		}
	}
}
