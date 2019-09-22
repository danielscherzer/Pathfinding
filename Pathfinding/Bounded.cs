using OpenTK;

namespace Example
{
	class Bounded
	{
		public Rectangle Bounds { get; set; }
		public RectangleF BoundsF { get; set; }
		public bool Contains(Vector2 point)
		{
			return BoundsF.Contains(new PointF(point.X, point.Y));
		}
	}
}
