using OpenTK;

namespace Example
{
	class Program
	{
		static void Main(string[] args)
		{
			var window = new MyWindow(); // create the example window
			window.WindowState = WindowState.Maximized; // render the window in maximized mode
			window.Run(); // start the game loop with 60Hz
		}
	}
}
