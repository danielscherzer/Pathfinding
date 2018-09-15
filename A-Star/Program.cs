using OpenTK;
using OpenTK.Input;
using System;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			var window = new MyWindow(); // create the example window
			window.WindowState = WindowState.Maximized; // render the window in maximized mode

			Console.WriteLine($"{Key.Space} creates new random start and goal positions.");
			Console.WriteLine($"{Key.H} toggles helpers.");
			Console.WriteLine($"{Key.A} cycles algorithm.");
			window.KeyDown += (s, a) =>
			{
				switch(a.Key)
				{
					case Key.Escape:
						window.Close();
						break;
					case Key.Space:
						window.NewStartGoal();
						break;
					case Key.H:
						window.ShowArrows = !window.ShowArrows;
						break;
					case Key.A:
						window.NextAlgorithm();
						break;
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
