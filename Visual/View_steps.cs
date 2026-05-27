using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;
using impl_search.SetUp;
using impl_search.Visual;

// snake rendering example, draw the grid in a simple way
internal class View_steps
{
    static Location startYellow = new Location(1, 4);
    static Location goalRed = new Location(8, 5);
    static CellType cellType;
    static int tmpx = 0;
    static int tmpy = 0;
    static float variety = 10f;
    static SquareGridDiagonal tmpGrid = new SquareGridDiagonal(tmpx, tmpy);
    static List<Location> path = new List<Location>();
    public View_steps() { }

    internal static void Draw(SquareGrid grid)
    {
        //
        tmpx = grid.width;
        tmpy = grid.height;
        GL.ClearColor(Color4.LightGray);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        DrawGrid(tmpx, tmpy);
    }

    internal static void Draw(SquareGridDiagonal grid)
    {
        //
        tmpx = grid.width;
        tmpy = grid.height;
        if (tmpx > 10)
        {
            variety = 100f;
        }
        else if (tmpx <= 10)
        {
            variety = 10f;
        }
        // Console.WriteLine("tmpx: " + tmpx + ", tmpy: " + tmpy);
        // tmpGrid = new SquareGridDiagonal(tmpx, tmpy);
        tmpGrid = grid;
        tmpGrid.walls = grid.walls;

        GL.ClearColor(Color4.White);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        DrawGrid(tmpx, tmpy);

    }

    private static void DrawGrid(int row, int column)
    {
        var pathSet = new HashSet<Location>(path);
        DrawGridLines(row, column);

        // GL.Clear(ClearBufferMask.ColorBufferBit);
        for (int i = 0; i < row; i++)
        {
            for (int t = 0; t < column; t++)
            {
                // Console.WriteLine("i: " + i + "t: " + t);
                // Console.WriteLine("goalx: " + goalRed.x + "goaly: " + goalRed.y);

                // GL.Clear(ClearBufferMask.ColorBufferBit);
                // if() which color
                cellType = CellType.Normal;

                Location check = new Location(i, t);
                if (!tmpGrid.Passable(check))
                {
                    // Console.WriteLine("Is a Wall");
                    cellType = CellType.Wall;
                }
                else if (pathSet.Contains(check))
                {
                    // cellType = CellType.Move;
                    if (stepTypes.ContainsKey(check))
                    {
                        int type = stepTypes[check];
                        if (type == 0)
                            cellType = CellType.Final;      // exploring
                        else
                            cellType = CellType.Move;     // final path
                    }
                }
                if (i == startYellow.x && t == startYellow.y)
                {
                    // GL.ClearColor(Color4.Yellow);
                    // Console.WriteLine("Green");
                    cellType = CellType.Start;
                }
                if (i == goalRed.x && t == goalRed.y)
                {
                    // GL.ClearColor(Color4.Red);
                    // Console.WriteLine("Make Red");
                    cellType = CellType.End;
                }

                switch (cellType)
                {
                    case CellType.Normal:
                        continue;
                    case CellType.Start:
                        GL.Color4(Color4.Green);
                        break;
                    case CellType.End:
                        GL.Color4(Color4.Red);
                        break;
                    case CellType.Wall:
                        GL.Color4(Color4.Black);
                        break;
                    case CellType.Move:
                        GL.Color4(Color4.Gray);
                        break;
                    case CellType.Final:
                        GL.Color4(Color4.Blue);
                        break;
                }

                Vector2 tmpLocation = new Vector2(i, t);
                MakeCell(row, column, i, t);

            }

        }
        DrawGridLines(row, column);
    }

    private static void MakeCell(int row, int column, int i, int t)
    {
        float cellWidth = 2f / row;
        float cellHeight = 2f / column;

        var size = new Vector2(cellWidth, cellHeight);
        var min = new Vector2(-1, -1) + new Vector2(i * cellWidth, t * cellHeight);

        DrawCell(min, size);
    }

    private static void DrawCell(Vector2 min, Vector2 size)
    {

        var max = min + size;
        GL.Begin(PrimitiveType.Quads);
        GL.Vertex2(min);
        GL.Vertex2(min.X, max.Y);
        GL.Vertex2(max);
        GL.Vertex2(max.X, min.Y);
        GL.End();
    }

    private static void DrawGridLines(int rows, int columns)
    {
        GL.Color4(Color4.LightGray);
        GL.LineWidth(1.5f);
        GL.Begin(PrimitiveType.Lines);

        float cellWidth = 2f / columns;
        float cellHeight = 2f / rows;

        // Vertical lines
        for (int x = 0; x <= columns; x++)
        {
            float xpos = -1f + x * cellWidth;
            GL.Vertex2(xpos, -1f);
            GL.Vertex2(xpos, 1f);
        }

        // Horizontal lines
        for (int y = 0; y <= rows; y++)
        {
            float ypos = -1f + y * cellHeight;
            GL.Vertex2(-1f, ypos);
            GL.Vertex2(1f, ypos);
        }

        GL.End();
    }

    internal static void ResetDraw()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
    }

    internal static void setStart(Location newstart)
    {
        startYellow = newstart;
    }

    internal static void setGoal(Location newgoal)
    {
        goalRed = newgoal;
    }

    internal static void setPath(List<Location> rec_path)
    {
        path = rec_path;
    }

    static Dictionary<Location, int> stepTypes = new Dictionary<Location, int>();

    internal static void setSteps(List<Location> locs, int[] types, int count)
    {
        stepTypes.Clear();
        for (int i = 0; i < count; i++)
        {
            stepTypes[locs[i]] = types[i];
        }
    }
}

