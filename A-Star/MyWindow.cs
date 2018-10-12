using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Example
{
	/// <summary>
	/// The exercises window code derived from <seealso cref="GameWindow"/>.
	/// </summary>
	internal class MyWindow : GameWindow
	{
		private readonly Model model;
		private readonly Visual visual;
		private IEnumerator<PathInfo<Coord>> iterator;
		private PathInfo<Coord> path;

		public bool ShowArrows { get; internal set; }

		internal MyWindow()
		{
			model = new Model();
			visual = new Visual(model.Grid);
		}

		internal void NewStartGoal()
		{
			model.NewStartGoal();
			iterator = null;
		}

		internal void ClickAt(int pixelX, int pixelY)
		{
			var coord = ConvertWindowPixelCoords(pixelX, pixelY); //convert pixel coordinates to [-1,1]²
			var x = coord.X * .5f + .5f;
			var y = coord.Y * .5f + .5f;
			var modelX = (ushort)(x * model.Grid.Width);
			var modelY = (ushort)(y * model.Grid.Height);

			model.ToggleElement(modelX, modelY);
			iterator = null;
		}

		internal void SolveMode()
		{
			iterator = null;
		}

		internal void NextAlgorithm()
		{
			model.NextAlgorithm();
			iterator = null;
		}

		internal void Step()
		{
			if (iterator is null)
			{
				iterator = model.FindPath().GetEnumerator();
			}
			if (iterator.MoveNext())
			{
				path = iterator.Current;
			}
			Title = $"grid size={model.Grid.Width}x{model.Grid.Height}; {model.AlgorithmName}; path length={path.Path.Count}";
		}

		/// <summary>
		/// Will be called each time the frame is rendered.
		/// </summary>
		/// <param name="arguments"></param>
		protected override void OnRenderFrame(FrameEventArgs arguments)
		{
			base.OnRenderFrame(arguments); // call the GameWindows implementation before executing the example code

			GL.Clear(ClearBufferMask.ColorBufferBit); // clear the screen
			GL.Color3(Color.Gray);
			GL.LineWidth(1.0f);
			visual.DrawGridLines(model.Grid.Width, model.Grid.Height);
			GL.Color3(Color.DarkGoldenrod);
			visual.DrawObstacles(model.Grid);
			var gray = new Vector3(0.3f);
			GL.Color3(gray);
			foreach (var v in path.Visited)
			{
				visual.DrawCell(v, 0.8f);
			}
			if (ShowArrows)
			{
				GL.Color3(Color.White);
				GL.LineWidth(2.0f);
				foreach (var line in path.CameFrom)
				{
					if (!line.Value.Equals(Coord.None)) visual.DrawArrow(line.Value, line.Key);
				}
			}
			GL.Color3(Color.LightBlue);
			foreach (var c in path.Path)
			{
				visual.DrawCell(c, 0.5f);
			}
			GL.Color3(Color.Green);
			visual.DrawCell(model.Start, 1.5f);
			GL.Color3(Color.Red);
			visual.DrawCell(model.Goal, 1.5f);
			SwapBuffers(); // buffer swap needed for double buffering
		}

		/// <summary>
		/// Will be called for each game loop iteration, so by default exactly 60 times a second
		/// </summary>
		/// <param name="arguments"></param>
		protected override void OnUpdateFrame(FrameEventArgs arguments)
		{
			base.OnUpdateFrame(arguments);
			if (!(iterator is null)) return;
			var sw = new Stopwatch();
			sw.Start();
			foreach (var step in model.FindPath())
			{
				path = step;
			}
			sw.Stop();
			Title = $"grid size={model.Grid.Width}x{model.Grid.Height}; {model.AlgorithmName}; path length={path.Path.Count} in {sw.ElapsedMilliseconds}ms";
		}

		/// <summary>
		/// Will be called each time the window is resized.
		/// </summary>
		/// <param name="arguments"></param>
		protected override void OnResize(EventArgs arguments)
		{
			base.OnResize(arguments); // call the GameWindows implementation before executing the example code
			
			GL.Viewport(0, 0, Width, Height);
		}

		private Vector2 ConvertWindowPixelCoords(int pixelX, int pixelY)
		{
			var coord01 = new Vector2(pixelX / (Width - 1f), 1f - pixelY / (Height - 1f));
			return coord01 * 2f - Vector2.One;
		}
	}
}