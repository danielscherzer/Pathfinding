using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Zenseless.PathFinder;
using Zenseless.PathFinder.Grid;

namespace Example.View
{
	internal class MainView
	{
		private readonly Style style;
		private readonly Model.Model model;
		private readonly GridVisual gridVisual;
		private Box2 bounds;

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
			gridVisual.Draw(style, ShowArrows, path, model.Start, model.Goal);
		}

		internal void InputDown(Vector2 point)
		{
			var coord = bounds.Transform(point); //convert pixel coordinates to [-1,1]²
			var (column, row) = gridVisual.TransformToGrid(coord);
			model.ToggleElement(column, row);
		}

		internal void Resize(int width, int height)
		{
			bounds = new Box2(0, 0, width, height);
			GL.Viewport(0, 0, width, height);
		}
	}
}