using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impl_search.SetUp
{

    // A* needs only a WeightedGraph and a location type L, and does *not*
    // have to be a grid. However, in the example code I am using a grid.
    public interface IWeightedGraph<L>
    {
        double Cost(Location a, Location b);
        IEnumerable<Location> Neighbors(Location id);
    }


    public struct Location
    {
        // Implementation notes: I am using the default Equals but it can
        // be slow. You'll probably want to override both Equals and
        // GetHashCode in a real project.

        public readonly int x, y;
        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

}
