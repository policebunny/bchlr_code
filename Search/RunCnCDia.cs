using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using impl_search.SetUp;
using impl_search.Logging;

namespace impl_search.Search
{
    public class RunCnCDia
    {
        static SquareGridDiagonal grid = new SquareGridDiagonal(10, 10);
        static CnCOriginal cncOG = new CnCOriginal(grid, new Location(1, 4), new Location(8, 5));
        
        static CnCCrippled cnc_steps = new CnCCrippled(grid, new Location(1,4), new Location(8,5));
        static List<Location> listedSteps = new List<Location>();
        static int[] steps = new int[cnc_steps.steps.Count];
        
        static Location start = new Location(0, 0);
        static Location end = new Location(0, 0);
        static string focusT = "testTicks";
        static string focusM = "testMemory";
        static List<Location> rec_path = new List<Location>();
        static void DrawGrid(SquareGridDiagonal grid, CnCOriginal cncOG)
        {

            Console.WriteLine("--cnc path--");

            // Print out the cameFrom array
            for (var y = 0; y < grid.height; y++)
            {
                for (var x = 0; x < grid.width; x++)
                {
                    Location id = new Location(x, y);
                    Location ptr = id;
                    if (cncOG.cameFrom.TryGetValue(id, out ptr))
                    {
                        ptr = id;
                    }
                    else
                    {
                        ptr = new Location(-1, -1);
                    }

                    // Console.WriteLine(ptr.x + ", !!!" + ptr.y );
                    if (grid.walls.Contains(id)) { Console.Write("#"); }
                    else if (ptr.x == x && ptr.y == y) { Console.Write("p"); }
                    /*
                    else if (ptr.x == x + 1 && ptr.y == y) { Console.Write("> "); } //u2192 = B rechts
                    else if (ptr.x == x - 1 && ptr.y == y) { Console.Write("< "); } // u2190 = C links
                    else if (ptr.y == y + 1 && ptr.x == x) { Console.Write("v "); } // u2193 = D   runter
                    else if (ptr.y == y - 1 && ptr.x == x) { Console.Write("^ "); } //u2191 = A hoch
                    else if (ptr.x == x + 1 && ptr.y == y + 1) { Console.Write("so"); } // S-O
                    else if (ptr.y == y + 1 && ptr.x == x - 1) { Console.Write("sw"); } // S-W
                    else if (ptr.y == y - 1 && ptr.x == x - 1) { Console.Write("nw"); } // N-W
                    else if (ptr.y == y + 1 && ptr.x == x + 1) { Console.Write("no"); } // N-O
                    */
                    else { Console.Write("*"); }
                }
                Console.WriteLine();
            }
            rec_path = new List<Location>();
            rec_path = compressPath(cncOG.cameFrom);

            // Requires nodestouched as dictionary with saved nodes and not as integer just counted
            // Console.WriteLine("--Touched all Nodes cnc--");
            /*
            for (var y = 0; y < grid.height; y++)
            {
                for (var x = 0; x < grid.width; x++)
                {
                    Location id = new Location(x, y);
                    Location ptr = id;
                    if (cncOG.nodesTouched.TryGetValue(id, out ptr))
                    {
                        ptr = id;
                    }
                    else
                    {
                        ptr = new Location(-1, -1);
                    }

                    // Console.WriteLine(ptr.x + ", !!!" + ptr.y );
                    if (grid.walls.Contains(id)) { Console.Write("#"); }
                    else if (ptr.x == x && ptr.y == y) { Console.Write("p"); }
                    else { Console.Write("*"); }
                }
                Console.WriteLine();
            }
            */
        }

        public static List<Location> compressPath(Dictionary<Location, Location> cameFrom)
        {
            List<Location> listedPath = new List<Location>();
            foreach(var loc in cameFrom)
            {
                listedPath.Add(loc.Key);
            }

            return listedPath;
        }

        public static void SetUpCnCGrid()
        {
            // /*
            grid = new SquareGridDiagonal(10, 10);

            for (int y = 3; y <= 6; y++)
            {
                int x = 4;
                grid.walls.Add(new Location(x, y));
            }
            // */



            /*
            // the test grid
            // Make "diagram 4" from main article
            grid = new SquareGridDiagonal(10, 10);
            for (var x = 1; x < 4; x++)
            {
                for (var y = 7; y < 9; y++)
                {
                    grid.walls.Add(new Location(x, y));
                }
            }
            grid.forests = new HashSet<Location>
                {
                    new Location(3, 4), new Location(3, 5),
                    new Location(4, 1), new Location(4, 2),
                    new Location(4, 3), new Location(4, 4),
                    new Location(4, 5), new Location(4, 6),
                    new Location(4, 7), new Location(4, 8),
                    new Location(5, 1), new Location(5, 2),
                    new Location(5, 3), new Location(5, 4),
                    new Location(5, 5), new Location(5, 6),
                    new Location(5, 7), new Location(5, 8),
                    new Location(6, 2), new Location(6, 3),
                    new Location(6, 4), new Location(6, 5),
                    new Location(6, 6), new Location(6, 7),
                    new Location(7, 3), new Location(7, 4),
                    new Location(7, 5)
                };
            */
        }



        public static void SetUpCnCOG()
        {

            SetUpCnCGrid();
            start = new Location(1, 4);
            end = new Location(8, 5);
            // USE GENERATED INSTEAD
        }

        public static void SetUpCnCBig(int which)
        {
            string big = "_BigGrid";
            focusT = focusT + big + which;
            focusM = focusM + big + which;
            start = BigGrid.bigStart(which);
            end = BigGrid.bigGoal(which);
            switch (which)
            {
                case 0:
                    SetUpCnCOG();
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

        public static void GoCnCOG_B()
        {
            cncOG = new CnCOriginal(grid, start, end);
        }
        
        public static void CnC_steps()
        {
            cnc_steps = new CnCCrippled(grid, start, end);
            /*
            List<Location> listedPath = new List<Location>();
            int[] step_type = new int[cnc_steps.steps.Count];
            int x = 0;
            foreach (var loc in cnc_steps.steps)
            {
                listedPath.Add(loc.Key);
                step_type[x] = loc.Value;
                x++;
            }
            listedSteps = listedPath;
            steps = step_type;
            */

            List<Location> listedPath = new List<Location>();
            List<int> stepTypes = new List<int>();

            foreach (var step in cnc_steps.steps)
            {
                listedPath.Add(step.loc);
                stepTypes.Add(step.type);
            }

            listedSteps = listedPath;
            steps = stepTypes.ToArray();
        }
        

        public static void GoCnCOG(bool logging)
        {
            if(logging)
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

            // VISUALOZER
            cncOG = new CnCOriginal(grid, start, end);
            DrawGrid(grid, cncOG);

        }

        static Information testTicks()
        {
            var timer = new Stopwatch();
            timer.Start();
            cncOG = new CnCOriginal(grid, start, end);
            timer.Stop();
            TimeSpan timeTakenT = timer.Elapsed;
            Console.WriteLine("frequency: " + Stopwatch.Frequency);
            string algA = "cncOGOG";
            Information info = new Information
            {
                whichAlg = algA,
                focusTest = focusT,
                Date = DateTime.Parse("2026-04-01"),
                nodesTouched = cncOG.nodesTouched,
                pathLength = cncOG.cameFrom.Count,
                memoryUsed = 0,
                ticksTaken = timeTakenT.Ticks
            };
            return info;
        }

        static Information testMemory()
        {
            long dif = 0;
            long mem1 = GC.GetTotalMemory(false);
            cncOG = new CnCOriginal(grid, start, end);
            long mem2 = GC.GetTotalMemory(false);
            dif = mem2 - mem1;
            string algA = "cncOGOG";
            Information info = new Information
            {
                whichAlg = algA,
                focusTest = focusM,
                Date = DateTime.Parse("2026-04-01"),
                nodesTouched = cncOG.nodesTouched,
                pathLength = cncOG.cameFrom.Count,
                memoryUsed = dif,
                ticksTaken = 0
            };
            return info;
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
        
        public static List<Location> getStepsPath()
        {
            return listedSteps;
        }

        public static int[] getStepsType()
        {
            return steps;
        }
        
    }
}
