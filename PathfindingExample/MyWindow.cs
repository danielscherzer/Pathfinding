using Example.View;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace Example
{
	/// <summary>
	/// The exercises window code derived from <seealso cref="GameWindow"/>.
	/// </summary>
	internal class MyWindow : GameWindow
	{
		private readonly Model.Model model;
		private readonly MainView view;
		private readonly Dictionary<Key, Tuple<Action, string>> keyBindings;

		internal MyWindow()
		{
			model = new Model.Model();
			view = new MainView(model);
			keyBindings = new Dictionary<Key, Tuple<Action, string>>
			{
				[Key.Enter] = new Tuple<Action, string>(() => model.NewStartGoal(), "creates new random start and goal positions"),
				[Key.A] = new Tuple<Action, string>(() => view.ShowArrows = !view.ShowArrows, "toggle arrows"),
				[Key.S] = new Tuple<Action, string>(() => Step(), "activates step mode for algorithm"),
				[Key.Escape] = new Tuple<Action, string>(() => Close(), "closes application"),
			};
			for(int i = 0; i < model.AlgorithmEvaluations.Count; ++i)
			{
				int copy = i; // for lambda capturing
				keyBindings[Key.Number1 + i] = new Tuple<Action, string>(() => { model.AlgorithmIndex = copy; }, $"Use {model.AlgorithmEvaluations[i].AlgorithmName} algorithm");
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
		}

		/// <summary>
		/// Will be called each time the frame is rendered.
		/// </summary>
		/// <param name="arguments"></param>
		protected override void OnRenderFrame(FrameEventArgs arguments)
		{
			base.OnRenderFrame(arguments); // call the GameWindows implementation before executing the example code
			view.Draw(model.CurrentEvaluation.Path);
			SwapBuffers(); // buffer swap needed for double buffering
		}

		/// <summary>
		/// Will be called for each game loop iteration, so by default exactly 60 times a second
		/// </summary>
		/// <param name="arguments"></param>
		protected override void OnUpdateFrame(FrameEventArgs arguments)
		{
			base.OnUpdateFrame(arguments);
			Console.SetCursorPosition(0, 0);
			Console.ForegroundColor = ConsoleColor.White;
			foreach (var keyBinding in keyBindings)
			{
				LogFullLine($"{keyBinding.Key.ToString().PadRight(10, '.')} {keyBinding.Value.Item2}");
			}
			Title = model.CurrentEvaluation.AlgorithmName;

			model.Update();
			LogFullLine($"grid size={model.Grid.Columns}x{model.Grid.Rows}");
			for (int i = 0; i < model.AlgorithmEvaluations.Count; ++i)
			{
				var eval = model.AlgorithmEvaluations[i];
				Console.ForegroundColor = model.AlgorithmIndex == i ? ConsoleColor.Red : ConsoleColor.White;
				LogFullLine($"{eval.AlgorithmName,-25} path length={eval.Path.Path.Count, -4} avg={eval.Avg,-8:F2}ms value={eval.EvaluationTime.TotalMilliseconds}ms");
			}
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

		public static void LogFullLine(string message)
		{
			var lines = message.Split(Environment.NewLine);
			foreach(var line in lines)
			{
				var fill = new string(' ', Console.BufferWidth - line.Length - 1);
				Console.WriteLine($"{line}{fill}");
			}
		}
	}
}