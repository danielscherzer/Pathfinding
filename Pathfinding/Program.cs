using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			var window = new MyWindow(); // create the example window
			window.WindowState = WindowState.Maximized; // render the window in maximized mode
			var keyBindings = new Dictionary<Key, Tuple<Action, string>>
			{
				[Key.Escape] = new Tuple<Action, string>(() => window.Close(), "closes application."),
				[Key.Space] = new Tuple<Action, string>(() => window.NewStartGoal(), "creates new random start and goal positions."),
				[Key.H] = new Tuple<Action, string>(() => window.ShowArrows = !window.ShowArrows, "toggles helpers."),
				[Key.Tab] = new Tuple<Action, string>(() => window.NextAlgorithm(), "cycles algorithm."),
				[Key.S] = new Tuple<Action, string>(() => window.Step(), "activates step mode for algorithm."),
			};

			foreach(var keyBinding in keyBindings)
			{
				Console.WriteLine($"{keyBinding.Key.ToString().PadRight(10,'.')} {keyBinding.Value.Item2}");
			}
			window.KeyDown += (s, a) =>
			{
				if(keyBindings.TryGetValue(a.Key, out var data))
				{
					data.Item1();
				}
			};

			window.MouseDown += (s, e) => window.ClickAt(e.X, e.Y);

			window.Run(); // start the game loop with 60Hz
		}
	}
}
