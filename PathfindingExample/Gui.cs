using Example.View;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Linq;
using Zenseless.OpenTK;
using Zenseless.OpenTK.GUI;

namespace Example;

internal class Gui
{
	public Gui(GameWindow window)
	{
		facade = new ImGuiFacade(window);
		facade.LoadFontDroidSans(24f);
		window.KeyDown += args =>
		{
			switch (args.Key)
			{
				case Keys.Escape: window.Close(); break;
			}
		};
		this.window = window;
		var style = ImGui.GetStyle();
		style.ItemSpacing.Y = 10f;
		style.CellPadding.Y = 5f;
	}

	internal void Render(Model.Model model, MainView view, Vector2i clientSize)
	{
		ImGui.NewFrame();

		if(ImGui.Begin("stats", ImGuiWindowFlags.AlwaysAutoResize))
		{
			Resolution(model);
			Table(model);

			if (ImGui.Button("New grid"))
			{
				model.NewGrid(model.Grid.Columns, model.Grid.Rows);
			}
			ImGui.SameLine();
			if (ImGui.Button("New start"))
			{
				model.Start = model.FindRandomPassablePosition();
			}
			ImGui.SameLine();
			if (ImGui.Button("New goal"))
			{
				model.Goal = model.FindRandomPassablePosition();
			}
			ImGui.SameLine();
			if (ImGui.Button("Exchange"))
			{
				(model.Start, model.Goal) = (model.Goal, model.Start);
			}

			if (ImGui.Button($"Step ({keyStopMode})") || window.IsKeyDown(keyStopMode))
			{
				model.CurrentEvaluation.FindNextStep();
			}
			ImGui.Checkbox("Search for difference in path length", ref search);
			if(search)
			{
				var reference = model.AlgorithmEvaluations[0];
				reference.FindPath();
				model.CurrentEvaluation.FindPath();
				if(reference.Path.Path.Count == model.CurrentEvaluation.Path.Path.Count)
				{
					model.Start = model.FindRandomPassablePosition();
					model.Goal = model.FindRandomPassablePosition();
				}
				else
				{
					search = false;
				}
			}

			var showArrows = view.ShowArrows;
			if (ImGui.Checkbox("Show arrows", ref showArrows))
			{
				view.ShowArrows = showArrows;
			}

			GridMouse(view);
			ImGui.End();
		}
		facade.Render(clientSize);	
	}

	private readonly ImGuiFacade facade;
	private readonly GameWindow window;
	private readonly Keys keyStopMode = Keys.Space;
	private bool search = false;

	private void GridMouse(MainView view)
	{
		var io = ImGui.GetIO();
		if (!io.WantCaptureMouse && window.IsMouseButtonPressed(MouseButton.Left))
		{
			view.InputDown(io.MousePos.ToOpenTK());
		}
	}

	private static void Resolution(Model.Model model)
	{
		ImGui.SetNextItemWidth(11.5f * ImGui.GetFontSize());
		var resolution = new int[] { model.Grid.Columns, model.Grid.Rows };
		if (ImGui.DragInt2("Grid resolution", ref resolution[0], 1f, 10, 512, "%i", ImGuiSliderFlags.Logarithmic))
		{
			model.NewGrid(resolution[0], resolution[1]);
		}
	}

	private static void Table(Model.Model model)
	{
		ImGui.BeginTable("table1", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.NoHostExtendX);
		ImGui.TableSetupColumn("Algorithm name");
		ImGui.TableSetupColumn("Path");
		ImGui.TableSetupColumn("Cells");
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
			ImGui.Text($"{row.Path.Visited.Count()}");
			ImGui.TableNextColumn();
			ImGui.Text($"{row.Avg:F2}ms");
		}
		ImGui.EndTable();
	}
}