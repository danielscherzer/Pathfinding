using System.Collections.Generic;

namespace Example
{
	struct PathInfo<NODE>
	{
		public IReadOnlyList<NODE> Path;
		public IEnumerable<NODE> Visited;
		public IReadOnlyDictionary<NODE, NODE> CameFrom;
	}
}
