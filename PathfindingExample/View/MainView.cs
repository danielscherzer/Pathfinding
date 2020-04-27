using Example.Grid;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PathFinder;

namespace Example.View
{
	internal class MainView : Bounded
	{
		private readonly Style style;
		private readonly Model.Model model;
		private readonly GridVisual gridVisual;

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

		internal void InputDown(Point point)
		{
			var coord = Bounds.Transform(point);
			if (gridVisual.Contains(coord))
			{
				var (column, row) = gridVisual.TransformToGrid(point);
				model.ToggleElement(column, row);
			}
		}

		internal void Resize(int width, int height)
		{
			var startGrid = 0;
			Bounds = new Rectangle(0, 0, width, height);
			gridVisual.Bounds = new Rectangle(startGrid, 0, width - startGrid, height);

			var deltaF = 2f * startGrid / (float)width;
			gridVisual.BoundsF = new RectangleF(deltaF - 1f, -1f, 2f, 2f);
		}
	}
}