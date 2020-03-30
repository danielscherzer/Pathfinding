using OpenTK;
using OpenTK.Graphics.OpenGL;
using PathFinder;

namespace Example
{
	class GridVisual : Bounded
	{
		private readonly Vector2 gridDelta;
		private readonly IGrid grid;

		public GridVisual(IGrid grid)
		{
			this.grid = grid;
			gridDelta = new Vector2(2.0f / grid.Columns, 2.0f / grid.Rows);
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

		internal void Draw(Style style, bool showArrows, PathInfo<Coord> path, Coord start, Coord goal)
		{
			GL.Viewport(Bounds);
			GL.Color3(style.LineColor);
			GL.LineWidth(style.LineWidth);
			DrawGridLines();
			GL.Color3(style.ObstacleColor);
			DrawObstacles();
			GL.Color3(style.VisitedColor);
			foreach (var v in path.Visited)
			{
				DrawCell(v, style.VisitedCellSize);
			}
			if (showArrows)
			{
				GL.Color3(style.ArrowColor);
				foreach (var line in path.CameFrom)
				{
					if (!line.Value.Equals(GridPathFinder.NullCoord)) DrawArrow(line.Value, line.Key);
				}
			}
			GL.Color3(style.PathColor);
			foreach (var c in path.Path)
			{
				DrawCell(c, style.PathPointSize);
			}
			GL.Color3(style.StartPointColor);
			DrawCell(start, style.StartEndPointSize);
			GL.Color3(style.EndPointColor);
			DrawCell(goal, style.StartEndPointSize);
		}

		public void DrawCell(Coord start, float scale)
		{
			var scaled = gridDelta * scale;
			var min = Convert(start) + 0.5f * (gridDelta - gridDelta * scale);
			DrawQuad(min, scaled);
		}

		private Vector2 Convert(Coord p)
		{
			return gridDelta * new Vector2(p.Column, p.Row) - Vector2.One;
		}

		public void DrawGridLines()
		{
			GL.Begin(PrimitiveType.Lines);
			for (int i = 1; i < grid.Columns; ++i)
			{
				var x = gridDelta.X * i - 1.0f;
				GL.Vertex2(x, -1.0);
				GL.Vertex2(x, 1.0);
			}
			for (int i = 1; i < grid.Rows; ++i)
			{
				var y = gridDelta.Y * i - 1.0f;
				GL.Vertex2(-1.0, y);
				GL.Vertex2(1.0, y);
			}
			GL.End();
		}

		public void DrawObstacles()
		{
			for (ushort u = 0; u < grid.Columns; ++u)
			{
				for (ushort v = 0; v < grid.Rows; ++v)
				{
					var min = Convert(new Coord(u, v));
					if (!grid.IsPassable(u, v))
					{
						DrawQuad(min, gridDelta);
					}
				}
			}
		}

		internal (ushort column, ushort row) TransformToGrid(Point point)
		{
			var coord = Bounds.Transform(point); //convert pixel coordinates to [-1,1]²
			var rect = new RectangleF(-1, -1, 2, 2);
			var b = rect.Transform(coord);
			var x = coord.X * .5f + .5f;
			var y = coord.Y * .5f + .5f;
			var column = (ushort)MathHelper.Clamp(x * grid.Columns, 0, grid.Columns - 1);
			var row = (ushort)MathHelper.Clamp(y * grid.Rows, 0, grid.Rows - 1);
			return (column, row);
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
