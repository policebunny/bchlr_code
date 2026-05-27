using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using impl_search.SetUp;

namespace impl_search.Search
{
    public class JPS
    {
        public Dictionary<Location, Location> cameFrom = new Dictionary<Location, Location>();
        public Dictionary<Location, double> costSoFar = new Dictionary<Location, double>();
        public int nodestouched = 0;

        private SquareGridDiagonal grid;
        private Location goal;        

        public JPS(SquareGridDiagonal graph, Location start, Location end)
        {
            this.grid = graph;
            this.goal = end;

            var frontier = new PriorityQueueA<Location, double>();
            frontier.Enqueue(start, 0);

            cameFrom[start] = start;
            costSoFar[start] = 0;
            nodestouched++;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                // nodestouched++;

                if (current.Equals(goal))
                    break;

                foreach (var dir in GetDirections(current))
                {
                    var jp = Jump(current, dir.x, dir.y);
                    if (jp == null) continue;

                    double newCost = costSoFar[current] + Distance(current, jp.Value);

                    if (!costSoFar.ContainsKey(jp.Value) || newCost < costSoFar[jp.Value])
                    {
                        costSoFar[jp.Value] = newCost;
                        double priority = newCost + Heuristic(jp.Value, goal);
                        frontier.Enqueue(jp.Value, priority);
                        cameFrom[jp.Value] = current;
                    }
                }
            }

        }


        // ------------------------
        // Heuristic (Octile for uniform diag)
        // ------------------------

        static double Heuristic(Location a, Location b)
        {
            int dx = Math.Abs(a.x - b.x);
            int dy = Math.Abs(a.y - b.y);
            return Math.Max(dx, dy);
        }

        static double Distance(Location a, Location b)
        {
            int dx = Math.Abs(a.x - b.x);
            int dy = Math.Abs(a.y - b.y);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // ------------------------
        // Direction pruning
        // ------------------------
        private IEnumerable<Location> GetDirections(Location current)
        {
            // Start node → all directions
            if (!cameFrom.ContainsKey(current) || cameFrom[current].Equals(current))
                return SquareGridDiagonal.DIRS;

            var parent = cameFrom[current];

            int dx = Math.Sign(current.x - parent.x);
            int dy = Math.Sign(current.y - parent.y);

            var dirs = new List<Location>();

            // Diagonal movement
            if (dx != 0 && dy != 0)
            {
                // natural neighbors
                dirs.Add(new Location(dx, dy));
                dirs.Add(new Location(dx, 0));
                dirs.Add(new Location(0, dy));

                // forced neighbors
                if (!IsWalkable(current.x - dx, current.y))
                    dirs.Add(new Location(-dx, dy));

                if (!IsWalkable(current.x, current.y - dy))
                    dirs.Add(new Location(dx, -dy));
            }
            // Horizontal
            else if (dx != 0)
            {
                dirs.Add(new Location(dx, 0));

                // forced neighbors
                if (!IsWalkable(current.x, current.y + 1))
                    dirs.Add(new Location(dx, 1));

                if (!IsWalkable(current.x, current.y - 1))
                    dirs.Add(new Location(dx, -1));
            }
            // Vertical
            else if (dy != 0)
            {
                dirs.Add(new Location(0, dy));

                // forced neighbors
                if (!IsWalkable(current.x + 1, current.y))
                    dirs.Add(new Location(1, dy));

                if (!IsWalkable(current.x - 1, current.y))
                    dirs.Add(new Location(-1, dy));
            }

            return dirs;
        }

        // ------------------------
        // Jump function (core of JPS)
        // ------------------------
        private Location? Jump(Location current, int dx, int dy)
        {
            int nx = current.x + dx;
            int ny = current.y + dy;
            nodestouched++;

            if (!IsWalkable(nx, ny))
                return null;

            var next = new Location(nx, ny);

            if (next.Equals(goal))
                return next;

            // Forced neighbor
            if (HasForcedNeighbor(next, dx, dy))
                return next;

            // Diagonal case
            if (dx != 0 && dy != 0)
            {
                // must check horizontal and vertical jumps
                if (Jump(next, dx, 0) != null || Jump(next, 0, dy) != null)
                    return next;
            }

            return Jump(next, dx, dy);
        }

        // ------------------------
        // Forced neighbor detection
        // ------------------------
        private bool HasForcedNeighbor(Location p, int dx, int dy)
        {
            // Diagonal
            if (dx != 0 && dy != 0)
            {
                if (!IsWalkable(p.x - dx, p.y) && IsWalkable(p.x - dx, p.y + dy))
                    return true;

                if (!IsWalkable(p.x, p.y - dy) && IsWalkable(p.x + dx, p.y - dy))
                    return true;
            }
            // Horizontal
            else if (dx != 0)
            {
                if (!IsWalkable(p.x, p.y + 1) && IsWalkable(p.x + dx, p.y + 1))
                    return true;

                if (!IsWalkable(p.x, p.y - 1) && IsWalkable(p.x + dx, p.y - 1))
                    return true;
            }
            // Vertical
            else if (dy != 0)
            {
                if (!IsWalkable(p.x + 1, p.y) && IsWalkable(p.x + 1, p.y + dy))
                    return true;

                if (!IsWalkable(p.x - 1, p.y) && IsWalkable(p.x - 1, p.y + dy))
                    return true;
            }

            return false;
        }

        private bool IsWalkable(int x, int y)
        {
            var loc = new Location(x, y);
            return grid.InBounds(loc) && grid.Passable(loc);
        }

    }
}

