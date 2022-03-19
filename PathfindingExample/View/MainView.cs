using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Zenseless.OpenTK;
using Zenseless.PathFinder;
using Zenseless.PathFinder.Grid;

namespace Example.View
{
	internal class MainView
	{
		private readonly Style style;
		private readonly Model.Model model;
		private readonly GridVisual gridVisual;
		private readonly Viewport viewport = new();
		private Matrix4 mtxAspect;
		private Matrix4 fromViewportToWorld;

		public MainView(Model.Model model)
		{
			style = Style.darkStyle;
			gridVisual = new GridVisual(model.Grid);
			GL.ClearColor(style.Background);
			this.model = model;
		}

		public bool ShowArrows { get; set; } = false;

		public void Draw(PathInfo<Coord> path)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit); // clear the screen
			GL.LoadMatrix(ref mtxAspect);
			gridVisual.Draw(style, ShowArrows, path, model.Start, model.Goal);
		}

		internal void InputDown(Vector2 point)
		{
			var coord = point.Transform(fromViewportToWorld);
			var (column, row) = gridVisual.TransformToGrid(coord);
			model.ToggleElement(column, row);
		}

		internal void Resize(int width, int height)
		{
			viewport.Resize(width, height);
			mtxAspect = Transformation2d.Scale(viewport.InvAspectRatio, 1f);
			fromViewportToWorld = Transformation2d.Combine(viewport.InvViewportMatrix, mtxAspect.Inverted());
		}
	}
}