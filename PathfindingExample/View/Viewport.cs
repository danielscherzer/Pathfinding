using OpenTK.Mathematics;

namespace Example.View
{
	public static class Viewport
	{
		public static Vector2 Transform(this Box2 viewport, Vector2 point)
		{
			var o = viewport.Min;
			var coord01 = new Vector2((point.X - o.X) / (viewport.Size.X - 1f), 1f - (point.Y - o.Y) / (viewport.Size.Y - 1f));
			return coord01 * 2f - Vector2.One;
		}
	}
}
