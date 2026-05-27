using impl_search.SetUp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using impl_search.SetUp;
using impl_search.Logging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impl_search.Search
{
    public class RunJPS
    {
        static SquareGridDiagonal grid = new SquareGridDiagonal(10, 10);
        static JPS Jps = new JPS(grid, new Location(1, 4), new Location(8, 5));
        static Location start = new Location(0, 0);
        static Location end = new Location(0, 0);
        static string focusT = "testTicks";
        static string focusM = "testMemory";
        static List<Location> rec_path = new List<Location>();
        static void DrawGrid(SquareGridDiagonal grid, JPS jps)
        {
            Console.WriteLine("--Touched all Nodes drawing--");


            // Print out the cameFrom array
            for (var y = 0; y < grid.height; y++)
            {
                for (var x = 0; x < grid.width; x++)
                {
                    Location id = new Location(x, y);
                    Location ptr = id;
                    if (!jps.cameFrom.TryGetValue(id, out ptr))
                    {
                        ptr = id;
                    }
                    if (grid.walls.Contains(id)) { Console.Write("##"); }
                    else if (ptr.x == x + 1 && ptr.y == y) { Console.Write("> "); } //u2192 = B rechts
                    else if (ptr.x == x - 1 && ptr.y == y) { Console.Write("< "); } // u2190 = C links
                    else if (ptr.y == y + 1 && ptr.x == x) { Console.Write("v "); } // u2193 = D   runter
                    else if (ptr.y == y - 1 && ptr.x == x) { Console.Write("^ "); } //u2191 = A hoch
                    else if (ptr.x == x + 1 && ptr.y == y + 1) { Console.Write("so"); } // S-O
                    else if (ptr.y == y + 1 && ptr.x == x - 1) { Console.Write("sw"); } // S-W
                    else if (ptr.y == y - 1 && ptr.x == x - 1) { Console.Write("nw"); } // N-W
                    else if (ptr.y == y - 1 && ptr.x == x + 1) { Console.Write("no"); } // N-O
                    else { Console.Write("* "); }
                }
                Console.WriteLine();
            }
            Console.WriteLine("--Only highlighted path--");
            var path = ReconstructPath(jps.cameFrom, start, end);
            rec_path = path;
            var pathSet = new HashSet<Location>(path);
            for (var y = 0; y < grid.height; y++)
            {
                for (var x = 0; x < grid.width; x++)
                {
                    Location id = new Location(x, y);
                    Location ptr = id;
                    if (!jps.cameFrom.TryGetValue(id, out ptr))
                    {
                        ptr = id;
                    }
                    if (grid.walls.Contains(id)) { Console.Write("##"); }
                    else if (pathSet.Contains(id)) { Console.Write("OO"); } // path
                    else { Console.Write(". "); }
                }
                Console.WriteLine();
            }
        }

        static void SetUpJPSGrid()
        {
            // Make "diagram 4" from main article
            grid = new SquareGridDiagonal(10, 10);


            for (int y = 3; y <= 6; y++)
            {
                int x = 4;
                grid.walls.Add(new Location(x, y));
            }

        }

        public static void SetUpJPS()
        {
            SetUpJPSGrid();
            start = new Location(1, 4);
            end = new Location(8, 5);
        }

        public static void SetUpJPSBig(int which)
        {
            string big = "_BigGrid";
            focusT = focusT + big + which;
            focusM = focusM + big + which;
            start = BigGrid.bigStart(which);
            end = BigGrid.bigGoal(which);
            switch (which)
            {
                case 0:
                    SetUpJPS();
                    break;
                case 1:
                    grid = BigGrid.makeGridDia1();
                    break;
                case 2:
                    grid = BigGrid.makeGridDia2();
                    break;
                case 3:
                    grid = BigGrid.makeGridDia3();
                    break;
                case 4:
                    grid = BigGrid.makeGridDia4();
                    break;
                case 5:
                    grid = BigGrid.makeGrid5_Maze();
                    break;
                case 6:
                    grid = BigGrid.makeGrid6_Maze_noCorners();
                    break;
                case 7:
                    grid = BigGrid.makeGrid7_CnCMap();
                    break;
            }
            
        }

        public static void GoJPS_B()
        {
            Jps = new JPS(grid, start, end);
        }

        public static void GoJPS(bool logging)
        {

            if (logging)
            {
                Logger.writeEmptyResult();
                Console.WriteLine("Loggin hier, zuerst ticks");
                for (int i = 0; i < 5; i++)
                {
                    Information testinfo = testTicks();
                    Logger.writeResults(testinfo);
                }
                Logger.writeEmptyResult();
                Console.WriteLine("Nun die Memory Tests");
                for (int i = 0; i < 5; i++)
                {
                    Information testinfo = testMemory();
                    Logger.writeResults(testinfo);
                }
                Console.WriteLine("Logging fertig");
            }

            Jps = new JPS(grid, start, end);
            DrawGrid(grid, Jps);

        }

        static Information testTicks()
        {
            var timer = new Stopwatch();
            timer.Start();
            Jps = new JPS(grid, start, end);
            timer.Stop();
            TimeSpan timeTakenT = timer.Elapsed;
            Console.WriteLine("frequency: " + Stopwatch.Frequency);
            string algA = "JPS";
            var path = ReconstructPath(Jps.cameFrom, start, end);
            int pLength = path.Count;
            Information info = new Information
            {
                whichAlg = algA,
                focusTest = focusT,
                Date = DateTime.Parse("2026-04-1"),
                nodesTouched = Jps.nodestouched,
                pathLength = pLength,
                memoryUsed = 0,
                ticksTaken = timeTakenT.Ticks
            };
            return info;
        }

        static Information testMemory()
        {
            long dif = 0;
            long mem1 = GC.GetTotalMemory(false);
            Jps = new JPS(grid, start, end);
            long mem2 = GC.GetTotalMemory(false);
            dif = mem2 - mem1;
            string algA = "JPS";
            var path = ReconstructPath(Jps.cameFrom, start, end);
            int pLength = path.Count;
            Information info = new Information
            {
                whichAlg = algA,
                focusTest = focusM,
                Date = DateTime.Parse("2026-04-01"),
                nodesTouched = Jps.nodestouched,
                pathLength = pLength,
                memoryUsed = dif,
                ticksTaken = 0
            };
            return info;
        }

        public static List<Location> ReconstructPath(Dictionary<Location, Location> cameFrom, Location start, Location goal)
        {
            var path = new List<Location>();

            if (!cameFrom.ContainsKey(goal))
                return path;

            Location current = goal;

            while (!current.Equals(start))
            {
                var prev = cameFrom[current];

                // expand segment from prev → current
                int dx = Math.Sign(current.x - prev.x);
                int dy = Math.Sign(current.y - prev.y);

                var temp = current;

                while (!temp.Equals(prev))
                {
                    path.Add(temp);
                    temp = new Location(temp.x - dx, temp.y - dy);
                }

                current = prev;
            }

            path.Add(start);
            path.Reverse();

            return path;
        }

        public static SquareGridDiagonal getAGrid()
        {
            return grid;
        }

        public static Location getStart()
        {
            return start;
        }

        public static Location getEnd()
        {
            return end;
        }

        public static List<Location> getPath()
        {
            return rec_path;
        }

    }
}
