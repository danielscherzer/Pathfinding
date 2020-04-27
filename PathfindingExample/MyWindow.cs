using System;
using System.Collections.Generic;
using System.Diagnostics;
using Example.View;
using OpenTK;
using OpenTK.Input;

namespace Example
{
	/// <summary>
	/// The exercises window code derived from <seealso cref="GameWindow"/>.
	/// </summary>
	internal class MyWindow : GameWindow
	{
		private readonly Model.Model model;
		private readonly MainView view;

		internal MyWindow()
		{
			model = new Model.Model();
			view = new MainView(model);
			var keyBindings = new Dictionary<Key, Tuple<Action, string>>
			{
				[Key.Escape] = new Tuple<Action, string>(() => Close(), "closes application"),
				[Key.Space] = new Tuple<Action, string>(() => model.NewStartGoal(), "creates new random start and goal positions"),
				[Key.A] = new Tuple<Action, string>(() => view.ShowArrows = !view.ShowArrows, "toggle arrows"),
				[Key.Tab] = new Tuple<Action, string>(() => model.NextAlgorithm(), "cycles algorithm"),
				[Key.S] = new Tuple<Action, string>(() => Step(), "activates step mode for algorithm"),
			};

			foreach (var keyBinding in keyBindings)
			{
				Console.WriteLine($"{keyBinding.Key.ToString().PadRight(10, '.')} {keyBinding.Value.Item2}");
			}
			KeyDown += (s, a) =>
			{
				if (keyBindings.TryGetValue(a.Key, out var data))
				{
					data.Item1();
				}
			};

			MouseDown += (s, e) => view.InputDown(e.Position);
		}

		private void Step()
		{
			model.Step();
			Title = $"grid size={model.Grid.Columns}x{model.Grid.Rows}; {model.AlgorithmName}; path length={model.Path.Path.Count}";
		}

		/// <summary>
		/// Will be called each time the frame is rendered.
		/// </summary>
		/// <param name="arguments"></param>
		protected override void OnRenderFrame(FrameEventArgs arguments)
		{
			base.OnRenderFrame(arguments); // call the GameWindows implementation before executing the example code
			view.Draw(model.Path);
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
			model.Update();
			sw.Stop();
			Title = $"grid size={model.Grid.Columns}x{model.Grid.Rows}; {model.AlgorithmName}; path length={model.Path.Path.Count} in {sw.ElapsedMilliseconds}ms";
		}

		/// <summary>
		/// Will be called each time the window is resized.
		/// </summary>
		/// <param name="arguments"></param>
		protected override void OnResize(EventArgs arguments)
		{
			base.OnResize(arguments); // call the GameWindows implementation before executing the example code
			view.Resize(Width, Height);
		}
	}
}