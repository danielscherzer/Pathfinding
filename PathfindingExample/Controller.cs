using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace Example
{
	internal class Controller
	{
		private readonly Dictionary<Keys, Tuple<Action, string>> keyBindings;
		private readonly Model.Model model;
		public event Action Close;

		public Controller(Model.Model model, View.MainView view)
		{
			keyBindings = new Dictionary<Keys, Tuple<Action, string>>
			{
				[Keys.Enter] = new Tuple<Action, string>(() => model.NewStartGoal(), "creates new random start and goal positions"),
				[Keys.A] = new Tuple<Action, string>(() => view.ShowArrows = !view.ShowArrows, "toggle arrows"),
				[Keys.S] = new Tuple<Action, string>(() => model.Step(), "activates step mode for algorithm"),
				[Keys.Escape] = new Tuple<Action, string>(() => Close?.Invoke(), "closes application"),
			};
			for (int i = 0; i < model.AlgorithmEvaluations.Count; ++i)
			{
				int copy = i; // for lambda capturing
				keyBindings[Keys.D1 + i] = new Tuple<Action, string>(() => { model.AlgorithmIndex = copy; }, $"Use {model.AlgorithmEvaluations[i].AlgorithmName} algorithm");
			}
			this.model = model;
		}

		internal void Handle(Keys key)
		{
			if (keyBindings.TryGetValue(key, out var data))
			{
				data.Item1();
			}
		}

		internal string ShowInfo()
		{
			Console.SetCursorPosition(0, 0);
			Console.ForegroundColor = ConsoleColor.White;
			foreach (var keyBinding in keyBindings)
			{
				LogFullLine($"{keyBinding.Key.ToString().PadRight(10, '.')} {keyBinding.Value.Item2}");
			}

			Console.WriteLine();
			LogFullLine($"grid size={model.Grid.Columns}x{model.Grid.Rows}");
			for (int i = 0; i < model.AlgorithmEvaluations.Count; ++i)
			{
				var eval = model.AlgorithmEvaluations[i];
				Console.ForegroundColor = model.AlgorithmIndex == i ? ConsoleColor.Red : ConsoleColor.White;
				LogFullLine($"{eval.AlgorithmName,-25} path length={eval.Path.Path.Count,-4} avg={eval.Avg,-8:F2}ms value={eval.EvaluationTime.TotalMilliseconds}ms");
			}
			return model.CurrentEvaluation.AlgorithmName;
		}

		private static void LogFullLine(string message)
		{
			var lines = message.Split(Environment.NewLine);
			foreach (var line in lines)
			{
				var fill = new string(' ', Console.BufferWidth - line.Length - 1);
				Console.WriteLine($"{line}{fill}");
			}
		}
	}
}
