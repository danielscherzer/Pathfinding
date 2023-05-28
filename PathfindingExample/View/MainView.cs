using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Zenseless.OpenTK;
using Zenseless.PathFinder;
using Zenseless.PathFinder.Grid;
using static Zenseless.OpenTK.Transformation2d;

namespace Example.View;

internal class MainView
{
	private readonly Style style;
	private readonly Model.Model model;
	private readonly Viewport viewport = new();
	private Matrix4 mtxAspect;
	private Matrix4 fromViewportToWorld;

	public MainView(Model.Model model)
	{
		style = Style.darkStyle;
		GL.ClearColor(style.Background);
		this.model = model;
	}

	public bool ShowArrows { get; set; } = false;

	public void Draw(PathInfo<Coord> path)
	{
		GL.Clear(ClearBufferMask.ColorBufferBit); // clear the screen
		GL.LoadMatrix(ref mtxAspect);
		model.Grid.Draw(style, ShowArrows, path, model.Start, model.Goal);
	}

	internal void InputDown(Vector2 point)
	{
		var coord = point.Transform(fromViewportToWorld);
		var (column, row) = model.Grid.TransformToGrid(coord);
		model.ToggleElement(column, row);
	}

	internal void Resize(int width, int height)
	{
		viewport.Resize(width, height);
		mtxAspect = Translate(width/(float)height - 1f, 0f);
		mtxAspect *= Scale(viewport.InvAspectRatio, 1f);
		fromViewportToWorld = Combine(viewport.InvViewportMatrix, mtxAspect.Inverted());
	}
}