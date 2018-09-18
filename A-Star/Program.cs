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
				[Key.N] = new Tuple<Action, string>(() => window.SolveMode(), "solves algorithm in one step."),
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

			window.MouseDown += (s, e) =>
			{
				var coord = ConvertWindowPixelCoords(window.Width, window.Height, e.X, e.Y); //convert pixel coordinates to [-1,1]²
				window.ToggleElement(coord);
			};

			window.Run(); // start the game loop with 60Hz
		}

		static Vector2 ConvertWindowPixelCoords(int winWidth, int winHeight, int pixelX, int pixelY)
		{
			var coord01 = new Vector2(pixelX / (winWidth - 1f), 1f - pixelY / (winHeight - 1f));
			return coord01 * 2f - Vector2.One;
		}
	}
}
