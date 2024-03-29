using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Zenseless.PathFinder;
using Zenseless.PathFinder.Grid;
using Zenseless.Spatial;

namespace Example.View
{
	internal static class GridVisual
	{
		internal static (ushort column, ushort row) TransformToGrid(this IReadOnlyGrid<bool> grid, Vector2 coord)
		{
			var x = coord.X * .5f + .5f;
			var y = coord.Y * .5f + .5f;
			var column = (ushort)MathHelper.Clamp(x * grid.Columns, 0, grid.Columns - 1);
			var row = (ushort)MathHelper.Clamp(y * grid.Rows, 0, grid.Rows - 1);
			return (column, row);
		}

		internal static void Draw(this IReadOnlyGrid<bool> grid, Style style, bool showArrows, PathInfo<Coord> path, Coord start, Coord goal)
		{
			var gridDelta = new Vector2(2.0f / grid.Columns, 2.0f / grid.Rows);

			Vector2 Convert(Coord p)
			{
				return gridDelta * new Vector2(p.Column, p.Row) - Vector2.One;
			}

			void DrawArrow(Coord a, Coord b)
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

			void DrawCell(Coord start, float scale)
			{
				var scaled = gridDelta * scale;
				var min = Convert(start) + 0.5f * (gridDelta - gridDelta * scale);
				DrawQuad(min, scaled);
			}

			void DrawGridLines()
			{
				GL.Begin(PrimitiveType.Lines);
				for (int i = 0; i <= grid.Columns; ++i)
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

			void DrawObstacles()
			{
				for (ushort u = 0; u < grid.Columns; ++u)
				{
					for (ushort v = 0; v < grid.Rows; ++v)
					{
						var min = Convert(new Coord(u, v));
						if (!grid[u, v])
						{
							DrawQuad(min, gridDelta);
						}
					}
				}
			}

			GL.Color4(style.LineColor);
			GL.LineWidth(style.LineWidth);
			DrawGridLines();
			GL.Color4(style.ObstacleColor);
			DrawObstacles();
			GL.Color4(style.VisitedColor);
			foreach (var v in path.Visited)
			{
				DrawCell(v, style.VisitedCellSize);
			}
			if (showArrows)
			{
				GL.Color4(style.ArrowColor);
				foreach (var line in path.CameFrom)
				{
					if (!line.Value.Equals(Coord.Null)) DrawArrow(line.Value, line.Key);
				}
			}
			GL.Color4(style.PathColor);
			foreach (var c in path.Path)
			{
				DrawCell(c, style.PathPointSize);
			}
			GL.Color4(style.StartPointColor);
			DrawCell(start, style.StartEndPointSize);
			GL.Color4(style.EndPointColor);
			DrawCell(goal, style.StartEndPointSize);
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
