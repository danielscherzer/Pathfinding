using System.Collections.Generic;

namespace Example
{
	struct PathInfo
	{
		public IReadOnlyList<Coord> Path;
		public IEnumerable<Coord> Visited;
		public IReadOnlyDictionary<Coord, Coord> CameFrom;
	}
}
