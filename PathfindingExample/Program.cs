using Example;
using Example.Model;
using Example.View;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Zenseless.OpenTK;

Model model = new();
GameWindow window = new(GameWindowSettings.Default, ImmediateMode.NativeWindowSettings);
var monitor = Monitors.GetMonitorFromWindow(window);
window.Size = new Vector2i(monitor.HorizontalResolution, monitor.VerticalResolution) / 2; // set window to halve monitor size
window.VSync = VSyncMode.On;
window.WindowState = WindowState.Maximized;

MainView view = new(model);
Gui gui = new(window);

window.RenderFrame += _ =>
{
	view.Draw(model.CurrentEvaluation.Path);
	gui.Render(model, view, window.ClientSize);
	window.SwapBuffers();
};
window.Resize += args => view.Resize(args.Width, args.Height);
window.UpdateFrame += _ => model.Update();

window.Run();
