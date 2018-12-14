using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Example
{
	class Visual
	{
		private readonly Vector2 gridDelta;

		public Visual(IGrid grid)
		{
			gridDelta = new Vector2(2.0f / grid.Width, 2.0f / grid.Height);
		}

		public void DrawArrow(Coord a, Coord b)
		{
			var p1 = Convert(a) + 0.5f * gridDelta;
			var p2 = Convert(b) + 0.5f * gridDelta;
			var t1 = Vector2.Lerp(p1, p2, 0.2f);
			p2 = Vector2.Lerp(p1, p2, 0.8f);
			var dir = p2 - t1;
			var n = 0.1f * dir.PerpendicularLeft.Normalized() * gridDelta;
			var t2 = p2 + n;
			var t3 = p2 - n;
			GL.Begin(PrimitiveType.Triangles);
			GL.Vertex2(t1);
			GL.Vertex2(t2);
			GL.Vertex2(t3);
			GL.End();
		}

		public void DrawCell(Coord start, float scale)
		{
			var scaled = gridDelta * scale;
			var min = Convert(start) + 0.5f * (gridDelta - gridDelta * scale);
			DrawQuad(min, scaled);
		}

		private Vector2 Convert(Coord p)
		{
			return gridDelta * new Vector2(p.X, p.Y) - Vector2.One;
		}

		public void DrawGridLines(int width, int height)
		{
			GL.Begin(PrimitiveType.Lines);
			for (int i = 1; i < width; ++i)
			{
				var x = gridDelta.X * i - 1.0f;
				GL.Vertex2(x, -1.0);
				GL.Vertex2(x, 1.0);
			}
			for (int i = 1; i < height; ++i)
			{
				var y = gridDelta.Y * i - 1.0f;
				GL.Vertex2(-1.0, y);
				GL.Vertex2(1.0, y);
			}
			GL.End();
		}

		public void DrawObstacles(IGrid grid)
		{
			for (ushort u = 0; u < grid.Width; ++u)
			{
				for (ushort v = 0; v < grid.Height; ++v)
				{
					var min = Convert(new Coord(u, v));
					if (!grid.IsPassable(u, v))
					{
						DrawQuad(min, gridDelta);
					}
				}
			}
		}

		private static void DrawQuad(Vector2 min, Vector2 size)
		{
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex2(min.X, min.Y);
			GL.Vertex2(min.X + size.X, min.Y);
			GL.Vertex2(min.X + size.X, min.Y + size.Y);
			GL.Vertex2(min.X, min.Y + size.Y);
			GL.End();
		}
	}
}
