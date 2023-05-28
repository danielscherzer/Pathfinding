using OpenTK.Mathematics;

namespace Example.View;

internal struct Style
{
	public Color4 ArrowColor { get; set; }
	public Color4 Background { get; set; }
	public Color4 LineColor { get; set; }
	public float LineWidth { get; set; }
	public Color4 ObstacleColor { get; set; }
	public Color4 PathColor { get; set; }
	public float PathPointSize { get; set; }
	public Color4 StartPointColor { get; set; }
	public Color4 EndPointColor { get; set; }
	public float StartEndPointSize { get; set; }
	public float VisitedCellSize { get; set; }
	public Color4 VisitedColor { get; set; }

	public static Style darkStyle = new()
	{
		ArrowColor = Color4.White,
		Background = Color4.Black,
		LineColor = Color4.Gray,
		LineWidth = 1.0f,
		ObstacleColor = Color4.DarkGoldenrod,
		PathColor = Color4.LightBlue,
		PathPointSize = 0.5f,
		StartPointColor = Color4.ForestGreen,
		EndPointColor = Color4.OrangeRed,
		StartEndPointSize = 0.8f,
		VisitedCellSize = 0.8f,
		VisitedColor = Color4.DarkSlateBlue //new Color4(50, 50, 50, 255),
	};

	public static Style lightStyle = new()
	{
		ArrowColor = Color4.Black,
		Background = Color4.White,
		LineColor = Color4.LightGray,
		LineWidth = 1.0f,
		ObstacleColor = Color4.DarkGoldenrod,
		PathColor = Color4.CornflowerBlue,
		PathPointSize = 0.5f,
		StartPointColor = Color4.Green,
		EndPointColor = Color4.Red,
		StartEndPointSize = 0.8f,
		VisitedCellSize = 0.8f,
		VisitedColor = new Color4(235, 235, 255, 255),
	};
}
