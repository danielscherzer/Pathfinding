using OpenTK;
using OpenTK.Graphics.OpenGL;
using PathFinder;

namespace Example
{
	internal class View : Bounded
	{
		private readonly Style style;
		private readonly Model model;
		private GridVisual gridVisual;
		private readonly UI ui;
		//private readonly List<(Bounded, Action)> bounded = new List<(Bounded, Action)>();

		public View(Model model)
		{
			style = Style.darkStyle;
			gridVisual = new GridVisual(model.Grid);
			//bounded.Add(gridVisual, () => ());
			ui = new UI();
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
			//if (gridVisual.Bounds.Contains(point))
			//{
			//	var (column, row) = gridVisual.TransformToGrid(point);
			//	model.ToggleElement(column, row);
			//}
			//else if(ui.Bounds.Contains(point))
			//{
			//	ShowArrows = !ShowArrows;
			//}
			var coord = Bounds.Transform(point);
			if (gridVisual.Contains(coord))
			{
				var (column, row) = gridVisual.TransformToGrid(point);
				model.ToggleElement(column, row);
			}
			else if (ui.Contains(coord))
			{
				ShowArrows = !ShowArrows;
			}
		}

		internal void InputUp(Point position)
		{
		}

		internal void Resize(int width, int height)
		{
			var startGrid = 0;
			Bounds = new Rectangle(0, 0, width, height);
			gridVisual.Bounds = new Rectangle(startGrid, 0, width - startGrid, height);
			ui.Bounds = new Rectangle(0, 0, startGrid, height);

			var deltaF = 2f * startGrid / (float)width;
			gridVisual.BoundsF = new RectangleF(deltaF - 1f, -1f, 2f, 2f);
			ui.BoundsF = new RectangleF(-1f, -1f, deltaF, 2f);
		}
	}
}