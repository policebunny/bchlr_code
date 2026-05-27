using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impl_search.SetUp
{
    public class SquareGridDiagonal : IWeightedGraph<Location>
    {


        public static readonly Location[] DIRS = new[]
            {
            new Location(1, 0), // East
            new Location(0, -1), // South
            new Location(-1, 0), // West
            new Location(0, 1), // North
            new Location(1, 1), // North-East diagonal
            new Location (-1, 1), // North-West diagonal
            new Location(1, -1), // South-East diagonal
            new Location(-1, -1) // South-West diagonal
        };

        public int width, height;
        public HashSet<Location> walls = new HashSet<Location>();
        public HashSet<Location> forests = new HashSet<Location>();

        public SquareGridDiagonal(int width, int height)
        {
            this.width = width;
            this.height = height;
        }


        public bool InBounds(Location id)
        {
            return 0 <= id.x && id.x < width
                && 0 <= id.y && id.y < height;
        }

        public bool Passable(Location id)
        {
            return !walls.Contains(id);
        }

        public double Cost(Location a, Location b)
        {
            return forests.Contains(b) ? 5 : 1;
        }

        public IEnumerable<Location> Neighbors(Location id)
        {

            foreach (var dir in DIRS)
            {
                Location next = new Location(id.x + dir.x, id.y + dir.y);
                // if (InBounds(next) && Passable(next))
                if (InBounds(next))
                {
                    yield return next;
                }
            }
        }
    }
}
