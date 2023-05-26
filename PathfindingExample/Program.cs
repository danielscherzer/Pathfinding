// window with immediate mode rendering enabled
using Example;
using Example.Model;
using Example.View;
using OpenTK.Windowing.Desktop;
using Zenseless.OpenTK;

Model model = new();
GameWindow window = new(GameWindowSettings.Default, ImmediateMode.NativeWindowSettings);
MainView view = new(model);
Controller controller = new(model, view);
controller.Close += window.Close;

window.KeyDown += args => controller.Handle(args.Key);
window.MouseDown += args => view.InputDown(window.MousePosition);
window.RenderFrame += _ => view.Draw(model.CurrentEvaluation.Path);
window.RenderFrame += _ => window.SwapBuffers();
window.Resize += args => view.Resize(args.Width, args.Height);
window.UpdateFrame += _ => model.Update();
window.UpdateFrame += _ => window.Title = controller.ShowInfo();

window.Run(); // start the game loop with 60Hz
