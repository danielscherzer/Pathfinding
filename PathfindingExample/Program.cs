using OpenTK;

namespace Example
{
	class Program
	{
		static void Main(string[] _)
		{
			var window = new MyWindow
			{
				WindowState = WindowState.Maximized, // render the window in maximized mode
				VSync = VSyncMode.Adaptive,
			}; // create the example window
			window.Run(60.0); // start the game loop with 60Hz
		}
	}
}
