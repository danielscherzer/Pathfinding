using Example.View;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Diagnostics;
using System.Drawing;
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

			if (ImGui.Button("Random grid"))
			{
				model.NewGrid(model.Grid.Columns, model.Grid.Rows);
			}
			ImGui.SameLine();
			if (ImGui.Button("Random start"))
			{
				model.Start = model.FindRandomPassablePosition();
			}
			ImGui.SameLine();
			if (ImGui.Button("Random goal"))
			{
				model.Goal = model.FindRandomPassablePosition();
			}
			if (ImGui.Button("Exchange start and goal"))
			{
				(model.Start, model.Goal) = (model.Goal, model.Start);
			}

			if (ImGui.Button($"Step ({keyStopMode})") || window.IsKeyDown(keyStopMode))
			{
				model.CurrentEvaluation.FindNextStep();
			}
			ImGui.Checkbox("Search for difference in path length", ref search);
			Search();

			var showArrows = view.ShowArrows;
			if (ImGui.Checkbox("Show arrows", ref showArrows))
			{
				view.ShowArrows = showArrows;
			}

			var value = (int)editMode;
			if(ImGui.ListBox("Edit mode", ref value, Enum.GetNames(typeof(EditMode)), 3))
			{
				editMode = (EditMode)value;
			}

			GridMouse(model, view);
			ImGui.End();
		}
		facade.Render(clientSize);

		void Search()
		{
			if (!search) return;
			var reference = model.AlgorithmEvaluations[0];
			//var time = Stopwatch.StartNew();
			//do
			//{
				reference.FindPath();
				model.CurrentEvaluation.FindPath();
				if (reference.Path.Path.Count == model.CurrentEvaluation.Path.Path.Count)
				{
					model.Start = model.FindRandomPassablePosition();
					model.Goal = model.FindRandomPassablePosition();
				}
				else
				{
					search = false;
					//break;
				}
			//} while (true);
		}
	}

	private readonly ImGuiFacade facade;
	private readonly GameWindow window;
	private readonly Keys keyStopMode = Keys.Space;
	private bool search = false;
	enum EditMode { ToggleCell = 0, Start = 1, Goal = 2 };
	private EditMode editMode = EditMode.ToggleCell;

	private void GridMouse(Model.Model model, MainView view)
	{
		var io = ImGui.GetIO();
		if (!io.WantCaptureMouse && window.IsMouseButtonPressed(MouseButton.Left))
		{
			var pos = view.Convert(io.MousePos.ToOpenTK());
			var (column, row) = model.Grid.TransformToGrid(pos);
			switch (editMode)
			{
				case EditMode.Start:
					model.Start = new Zenseless.PathFinder.Grid.Coord(column, row);
					break;
				case EditMode.ToggleCell:
					model.ToggleElement(column, row);
					break;
				case EditMode.Goal:
					model.Goal = new Zenseless.PathFinder.Grid.Coord(column, row);
					break;
			}
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