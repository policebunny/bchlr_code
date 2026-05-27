using impl_search.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impl_search.SetUp
{
    public class BigGrid
    {
        // start ca 50, 20
        // end ca 50, 80
        // hindernisse ab ca 40-60
        static SquareGridDiagonal tmpgridDia = new SquareGridDiagonal(100, 100);
        static SquareGrid tmpgrid = new SquareGrid(100, 100);

        // map.end = new Location(33,51);
        // map.start = new Location(68,48);

        public static Location bigStart(int which)
        {
            if(which != 5 && which != 6 && which != 7)
            {
                return new Location(50, 20);
            } else
            {
                if(which == 7)
                {
                    // return new Location(68,48); // CHANGE TO NEW START POINT IN GRID 7
                    return new Location(60, 60);
                } else
                {
                    return new Location(3,3); // CHANGE TO NEW START POINT IN GRID 5 & 6
                }
            }

        }
        public static Location bigGoal(int which)
        {
            if (which != 5 && which != 6 && which != 7)
            {
                return new Location(50, 80);
            }
            else
            {
                if (which == 7)
                {
                    // return new Location(33, 51); // CHANGE TO NEW START POINT IN GRID 7
                    return new Location(40, 40);
                }
                else
                {
                    return new Location(98, 98); // CHANGE TO NEW START POINT IN GRID 5 & 6
                }
            }
        }
        public static SquareGridDiagonal makeGridDia1()
        {
            // fill grid type 1 here
            // is empty just line
            tmpgridDia = new SquareGridDiagonal(100, 100);
            return tmpgridDia;
        }
        /*
            grid = new SquareGrid(10, 10);
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
        public static SquareGridDiagonal makeGridDia2()
        {
            // fill grid type 2 here
            // x 30 y90 bis x30 y40
            // x31 y40 bis x35 y40
            tmpgridDia = new SquareGridDiagonal(100, 100);

            for (int x = 30, y = 40; y <= 90; y++)
            {
                tmpgridDia.walls.Add(new Location(x,y));
            }
            for(int x = 31, y = 40; x <= 55; x++)
            {
                tmpgridDia.walls.Add(new Location(x,y));
            }

            return tmpgridDia;
        }

        public static SquareGridDiagonal makeGridDia3()
        {
            // fill grid type 3 here
            // not done
            tmpgridDia = new SquareGridDiagonal(100, 100);

            // x50 y30 is oberhalb und nochmal bei x50 y40
            // x5 y15 bis x5 y30 -> x6 y30 bis x60 y30
            // x60 y25 bis x65 y25 -> x65 y26 bis x65 y40 -> x20 y40 bis x64 y40
            // 
            for (int x = 5, y = 15; y <= 30; y++)
            {
                tmpgridDia.walls.Add(new Location(x,y));
            }
            for (int x = 6, y = 30; x <= 60; x++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }

            for (int x = 60, y = 25; x <= 65; x++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }
            for (int x = 65, y = 26; y <= 40; y++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }
            for (int x = 20, y = 40; x <= 64; x++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }
            return tmpgridDia;
        }

        public static SquareGridDiagonal makeGridDia4()
        {
            // fill grid type 4 here
            // kringel
            tmpgridDia = new SquareGridDiagonal(100, 100);

            // x45, y25 bis x45, y70 -> x46, y70 bis x70, y70 -> x70, y71 bis x70 y90 -> x69, y90 bis x45, y90 -> x45, y89 bis x45, y80

            for (int x = 45, y = 25; y <= 70; y++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }
            for (int x = 46, y = 70; x <= 70; x++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }
            for (int x = 70, y = 71; y <= 90; y++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }
            for (int x = 45, y = 90; x <= 69; x++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }
            for (int x = 45, y = 80; y <= 89; y++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }

            // x55, y25 bis x55, y60 -> x56, y60 bis x80, y60 -> x80, y61 bis x80, y95 -> x79, y95 bis x35, y95 -> x35, y94 bis x35, y70

            for (int x = 55, y = 25; y <= 60; y++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }
            for (int x = 56, y = 60; x <= 80; x++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }
            for (int x = 80, y = 61; y <= 95; y++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }
            for (int x = 35, y = 95; x <= 79; x++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }
            for (int x = 35, y = 70; y <= 94; y++)
            {
                tmpgridDia.walls.Add(new Location(x, y));
            }

            return tmpgridDia;
        }

        /// ---------------------------------------------
        public static SquareGridDiagonal makeGrid5_Maze()
        {
            tmpgridDia = new SquareGridDiagonal(100, 100);

            CustomMap map = Logger.loadGrid5();

            tmpgridDia = map.editedGrid;

            return tmpgridDia;
        }

        public static SquareGridDiagonal makeGrid6_Maze_noCorners()
        {
            tmpgridDia = new SquareGridDiagonal(100, 100);

            CustomMap map = Logger.loadGrid6();

            tmpgridDia = map.editedGrid;

            return tmpgridDia;
        }

        public static SquareGridDiagonal makeGrid7_CnCMap()
        {
            tmpgridDia = new SquareGridDiagonal(100, 100);

            CustomMap map = Logger.loadGrid7();

            tmpgridDia = map.editedGrid;


            return tmpgridDia;
        }


        /// ---------------------------------------------- 
        public static SquareGrid makeGrid1()
        {
            return tmpgrid;
        }

        public static SquareGrid makeGrid2()
        {
            for (int x = 30, y = 40; y <= 90; y++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }
            for (int x = 31, y = 40; x <= 35; x++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }
            return tmpgrid;
        }

        public static SquareGrid makeGrid3()
        {
            // not done

            return tmpgrid;
        }

        public static SquareGrid makeGrid4()
        {
            for (int x = 45, y = 25; y <= 70; y++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }
            for (int x = 46, y = 70; x <= 70; x++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }
            for (int x = 70, y = 71; y <= 90; y++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }
            for (int x = 45, y = 90; x <= 69; x++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }
            for (int x = 45, y = 80; y <= 89; y++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }

            // x55, y25 bis x55, y60 -> x56, y60 bis x80, y60 -> x80, y61 bis x80, y95 -> x79, y95 bis x35, y95 -> x35, y94 bis x35, y70

            for (int x = 55, y = 25; y <= 60; y++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }
            for (int x = 56, y = 60; x <= 80; x++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }
            for (int x = 80, y = 61; y <= 95; y++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }
            for (int x = 35, y = 95; x <= 79; x++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }
            for (int x = 35, y = 70; y <= 94; y++)
            {
                tmpgrid.walls.Add(new Location(x, y));
            }

            return tmpgrid;
        }
    }
}
