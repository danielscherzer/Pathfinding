using OpenTK;

namespace Example
{
	struct Style
	{
		public Color ArrowColor { get; set; }
		public Color Background { get; set; }
		public Color LineColor { get; set; }
		public float LineWidth { get; set; }
		public Color ObstacleColor { get; set; }
		public Color PathColor { get; set; }
		public float PathPointSize { get; set; }
		public Color StartPointColor { get; set; }
		public Color EndPointColor { get; set; }
		public float StartEndPointSize { get; set; }
		public float VisitedCellSize { get; set; }
		public Color VisitedColor { get; set; }

		public static Style darkStyle = new Style
		{
			ArrowColor = Color.White,
			Background = Color.Black,
			LineColor = Color.Gray,
			LineWidth = 1.0f,
			ObstacleColor = Color.DarkGoldenrod,
			PathColor = Color.LightBlue,
			PathPointSize = 0.5f,
			StartPointColor = Color.Green,
			EndPointColor = Color.Red,
			StartEndPointSize = 0.8f,
			VisitedCellSize = 0.8f,
			VisitedColor = Color.FromArgb(255, 50, 50, 50),
		};

		public static Style lightStyle = new Style
		{
			ArrowColor = Color.Black,
			Background = Color.White,
			LineColor = Color.LightGray,
			LineWidth = 1.0f,
			ObstacleColor = Color.DarkGoldenrod,
			PathColor = Color.CornflowerBlue,
			PathPointSize = 0.5f,
			StartPointColor = Color.Green,
			EndPointColor = Color.Red,
			StartEndPointSize = 0.8f,
			VisitedCellSize = 0.8f,
			VisitedColor = Color.FromArgb(255, 235, 235, 255),
		};
	}
}
