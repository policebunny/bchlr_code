using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using impl_search.SetUp;

namespace impl_search.Search
{

    public class AStarSearch
    {
        public Dictionary<Location, Location> cameFrom
            = new Dictionary<Location, Location>();
        public Dictionary<Location, double> costSoFar
            = new Dictionary<Location, double>();
        public int nodesTouched = 0;

        // Note: a generic version of A* would abstract over Location and
        // also Heuristic
        static public double Heuristic(Location a, Location b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public AStarSearch(SquareGridDiagonal graph, Location start, Location goal)
        {
            var frontier = new PriorityQueueA<Location, double>();
            frontier.Enqueue(start, 0);

            cameFrom[start] = start;
            costSoFar[start] = 0;
            nodesTouched++;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(goal))
                {
                    break;
                }

                foreach (var next in graph.Neighbors(current))
                {
                    nodesTouched++;
                    if(graph.Passable(next))
                    {
                        double newCost = costSoFar[current]
                        + graph.Cost(current, next);
                        if (!costSoFar.ContainsKey(next)
                            || newCost < costSoFar[next])
                        {
                            costSoFar[next] = newCost;
                            double priority = newCost + Heuristic(next, goal);
                            frontier.Enqueue(next, priority);
                            cameFrom[next] = current;
                        }
                    }
                    
                }
            }
        }
    }

}
