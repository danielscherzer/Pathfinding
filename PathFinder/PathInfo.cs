using System.Collections.Generic;

namespace PathFinder
{
	public struct PathInfo<NODE>
	{
		public IReadOnlyList<NODE> Path;
		public IEnumerable<NODE> Visited => CameFrom.Keys;
		public IReadOnlyDictionary<NODE, NODE> CameFrom;
	}
}
