using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Zenseless.OpenTK;
using Zenseless.OpenTK.GUI;

namespace Example;
internal class Gui
{
	private readonly ImGuiFacade facade;
	private readonly GameWindow window;

	public Gui(GameWindow window)
	{
		facade = new ImGuiFacade(window);
		facade.LoadFontDroidSans(32f);
		window.KeyDown += args =>
		{
			switch (args.Key)
			{
				case Keys.Escape: window.Close(); break;
			}
		};
		this.window = window;
	}

	internal void Render(Model.Model model, View.MainView view, Vector2i clientSize)
	{
		ImGui.NewFrame();

		if(ImGui.Begin("stats", ImGuiWindowFlags.AlwaysAutoResize))
		{
			ImGui.Text($"grid size={model.Grid.Columns}x{model.Grid.Rows}");

			Table(model);

			var showArrows = view.ShowArrows;
			if (ImGui.Checkbox("Show arrows", ref showArrows))
			{
				view.ShowArrows = showArrows;
			}

			if (ImGui.Button("New start and goal position"))
			{
				model.NewStartGoal();
			}
			if (ImGui.Button("Step mode"))
			{
				model.Step();
			}

			var io = ImGui.GetIO();
			if (!io.WantCaptureMouse && window.IsMouseButtonPressed(MouseButton.Left))
			{
				view.InputDown(io.MousePos.ToOpenTK());
			}
			ImGui.End();
		}
		facade.Render(clientSize);	
	}

	private static void Table(Model.Model model)
	{
		ImGui.BeginTable("table1", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.NoHostExtendX);
		ImGui.TableSetupColumn("Algorithm name");
		ImGui.TableSetupColumn("Path length");
		ImGui.TableSetupColumn("AVG time");
		ImGui.TableHeadersRow();

		for (int i = 0; i < model.AlgorithmEvaluations.Count; ++i)
		{
			var row = model.AlgorithmEvaluations[i];
			ImGui.TableNextColumn();
			if (ImGui.Selectable(row.AlgorithmName, model.AlgorithmIndex == i, ImGuiSelectableFlags.SpanAllColumns))
			{
				model.AlgorithmIndex = i;
			}
			ImGui.TableNextColumn();
			ImGui.Text($"{row.Path.Path.Count}");
			ImGui.TableNextColumn();
			ImGui.Text($"{row.Avg:F2}ms");
		}
		ImGui.EndTable();
	}
}