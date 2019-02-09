using System.Collections.Generic;

namespace PathFinder
{
	public struct PathInfo<NODE>
	{
		public IReadOnlyList<NODE> Path;
		public IEnumerable<NODE> Visited;
		public IReadOnlyDictionary<NODE, NODE> CameFrom;
	}
}
