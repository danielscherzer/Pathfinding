using OpenTK;
using OpenTK.Graphics.OpenGL;
using PathFinder;
using System;
using System.Collections.Generic;

namespace Example
{
	internal class View
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
			if (gridVisual.Bounds.Contains(point))
			{
				var (column, row) = gridVisual.TransformToGrid(point);
				model.ToggleElement(column, row);
			}
			else if(ui.Bounds.Contains(point))
			{
				ShowArrows = !ShowArrows;
			}
		}

		internal void InputUp(Point position)
		{
		}

		internal void Resize(int width, int height)
		{
			gridVisual.Bounds = new Rectangle(50, 0, width - 50, height);
			ui.Bounds = new Rectangle(0, 0, 50, height);
		}
	}
}