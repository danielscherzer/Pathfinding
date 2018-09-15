using System;
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
		private PathInfo path;

		public bool ShowArrows { get; internal set; }

		internal MyWindow()
		{
			model = new Model();
			visual = new Visual(model.Grid);
		}

		internal void NewStartGoal() => model.NewStartGoal();

		internal void ToggleElement(Vector2 coord)
		{
			var x = coord.X * .5f + .5f;
			var y = coord.Y * .5f + .5f;
			var modelX = (int)(x * model.Grid.Width);
			var modelY = (int)(y * model.Grid.Height);

			model.ToggleElement(modelX, modelY);
		}

		internal void NextAlgorithm()
		{
			model.NextAlgorithm();
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
			visual.DrawCell(model.Start, .5f);
			GL.Color3(Color.Red);
			visual.DrawCell(model.Goal, .5f);
			SwapBuffers(); // buffer swap needed for double buffering
		}

		/// <summary>
		/// Will be called for each game loop iteration, so by default exactly 60 times a second
		/// </summary>
		/// <param name="arguments"></param>
		protected override void OnUpdateFrame(FrameEventArgs arguments)
		{
			base.OnUpdateFrame(arguments);
			var sw = new Stopwatch();
			sw.Start();
			path = model.FindPath();
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

			GL.Viewport(0, 0, Width, Height); // tell OpenGL to use the whole window for drawing
		}
	}
}