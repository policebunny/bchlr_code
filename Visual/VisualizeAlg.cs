using impl_search.Logging;
using impl_search.Search;
using impl_search.SetUp;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace impl_search.Visual
{
    
    internal class VisualizeAlg
    {
        GameWindow viswindow;
        bool go;
        
        public VisualizeAlg()
        {
            viswindow = CustomWindow.Create();
            go = true;
        }

        public void visualizeAStarDia()
        {
            SquareGridDiagonal grid = RunAStarDia.getAGrid();
            Location start = RunAStarDia.getStart();
            View.setStart(start);
            Location goal = RunAStarDia.getEnd();
            View.setGoal(goal);
            List<Location> path = RunAStarDia.getPath();
            View.setPath(path);

            viswindow.KeyDown += args =>
            {
                if (Keys.Escape == args.Key)
                {
                    viswindow.Close();
                }
                if(Keys.Space == args.Key)
                {
                    go = false;
                }
            };
            // we implemented our own game loop
            // View currentAlg = new View(start, end);
            float time = 0f;
            do
            {
                time += viswindow.DeltaTime();

                View.Draw(grid); // called once each frame; callback should contain drawing code
            } while (viswindow.NextFrame() && go); // wait for next frame and return true until window is closed
            View.ResetDraw();
            go = true;
        }


        public void visualizeCnCOG()
        {
            SquareGridDiagonal grid = RunCnCDia.getAGrid();
            Location start = RunCnCDia.getStart();
            View.setStart(start);
            Location goal = RunCnCDia.getEnd();
            View.setGoal(goal);
            List<Location> path = RunCnCDia.getPath();
            View.setPath(path);

            viswindow.KeyDown += args =>
            {
                if (Keys.Escape == args.Key)
                {
                    viswindow.Close();
                }
                if (Keys.Space == args.Key)
                {
                    go = false;
                }
            };
            // we implemented our own game loop
            // View currentAlg = new View(start, end);
            float time = 0f;
            do
            {
                time += viswindow.DeltaTime();

                View.Draw(grid); // called once each frame; callback should contain drawing code
            } while (viswindow.NextFrame() && go); // wait for next frame and return true until window is closed
            View.ResetDraw();
            go = true;
        }

        public void visualizeJPS()
        {
            SquareGridDiagonal grid = RunJPS.getAGrid();
            Location start = RunJPS.getStart();
            View.setStart(start);
            Location goal = RunJPS.getEnd();
            View.setGoal(goal);
            List<Location> path = RunJPS.getPath();
            View.setPath(path);

            viswindow.KeyDown += args =>
            {
                if (Keys.Escape == args.Key)
                {
                    viswindow.Close();
                }
                if (Keys.Space == args.Key)
                {
                    go = false;
                }
            };
            // we implemented our own game loop
            // View currentAlg = new View(start, end);
            float time = 0f;
            do
            {
                time += viswindow.DeltaTime();

                View.Draw(grid); // called once each frame; callback should contain drawing code
            } while (viswindow.NextFrame() && go); // wait for next frame and return true until window is closed
            View.ResetDraw();
            go = true;
        }

        public void visualizeEditable()
        {
            CustomMap map = Logger.loadGrid();

            SquareGridDiagonal grid = map.editedGrid;

            Location start = map.start;
            View.setStart(start);

            Location goal = map.end;
            View.setGoal(goal);
            // List<Location> path = RunCnCDia.getPath();
            // View.setPath(path);

            viswindow.KeyDown += args =>
            {
                if (Keys.Escape == args.Key)
                {
                    viswindow.Close();
                }
                if (Keys.Space == args.Key)
                {
                    go = false;
                }
            };

            viswindow.MouseDown += args =>
            {
                int gridWidth = grid.width;
                int gridHeight = grid.height;

                float normX = (viswindow.MousePosition.X / viswindow.Size.X) * 2f - 1f;
                float normY = 1f - (viswindow.MousePosition.Y / viswindow.Size.Y) * 2f;

                float shiftedX = normX + 1f;
                float shiftedY = normY + 1f;

                int gridX = (int)(shiftedX / (2f / gridWidth));
                int gridY = (int)(shiftedY / (2f / gridHeight));

                Location edit = new Location(gridX, gridY);

                Console.WriteLine($"editx: {edit.x}, y: {edit.y}");

                if (grid.Passable(edit))
                {
                    grid.walls.Add(edit);
                } else
                {
                    grid.walls.Remove(edit);
                }
            };



            // we implemented our own game loop
            // View currentAlg = new View(start, end);
            float time = 0f;
            do
            {
                time += viswindow.DeltaTime();

                View.Draw(grid); // called once each frame; callback should contain drawing code
            } while (viswindow.NextFrame() && go); // wait for next frame and return true until window is closed

            CustomMap edited = CustomMap.makeToMap(grid, start, goal);
            Logger.saveGrid(edited);

        }

        public void visualizeCnCOG_steps()
        {
            SquareGridDiagonal grid = RunCnCDia.getAGrid();
            Location start = RunCnCDia.getStart();
            View_steps.setStart(start);
            Location goal = RunCnCDia.getEnd();
            View.setGoal(goal);
            List<Location> path = RunCnCDia.getStepsPath();
            View_steps.setPath(path);
            int[] steps = RunCnCDia.getStepsType();

            int currentStep = 0;
            bool advanceStep = false;

            viswindow.KeyDown += args =>
            {
                if (Keys.Escape == args.Key)
                {
                    viswindow.Close();
                }
                if (Keys.Space == args.Key)
                {
                    advanceStep = true;
                }
            };
            // we implemented our own game loop
            // View currentAlg = new View(start, end);
            float time = 0f;
            do
            {
                if (advanceStep && currentStep < path.Count)
                {
                    currentStep++;
                    advanceStep = false;
                }

                // pass only the visible steps
                var partialPath = path.Take(currentStep).ToList();
                View_steps.setSteps(path, steps, currentStep);
                View_steps.setPath(partialPath);

                View_steps.Draw(grid);

            } while (viswindow.NextFrame());

        }

    }
    
}
