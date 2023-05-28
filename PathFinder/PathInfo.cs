using System.Collections.Generic;

namespace Zenseless.PathFinder
{
	public struct PathInfo<NODE>
	{
		public IReadOnlyDictionary<NODE, NODE> CameFrom;
		public IReadOnlyList<NODE> Path;
		public readonly IEnumerable<NODE> Visited => CameFrom.Keys;

		public static PathInfo<NODE> CreateEmpty()
		{
			return new PathInfo<NODE>
			{
				CameFrom = new Dictionary<NODE, NODE>(),
				Path = new List<NODE>(),
			};
		}
	}
}
