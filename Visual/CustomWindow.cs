using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace impl_search.Visual
{
    public static class CustomWindow
    {
        public static GameWindow Create()
        {
            var info = Monitors.GetPrimaryMonitor();
            Vector2i res = new(info.HorizontalResolution, info.VerticalResolution);
            Vector2i winRes = res / 2;
            Vector2i pos = (res - winRes) / 2;

            // window with immediate mode rendering enabled
            var window = new GameWindow(GameWindowSettings.Default, new NativeWindowSettings
            {
                Profile = ContextProfile.Compatability,
                Flags = ContextFlags.Default // For none NVIDIA drivers
            });

            window.Resize += args => GL.Viewport(0, 0, args.Width, args.Height); // resize viewport on window resize
            window.Title = Assembly.GetEntryAssembly()?.GetName().Name;

            window.KeyDown += args =>
            {
                if (Keys.Escape == args.Key)
                {
                    window.Close(); // close the application on pressing the escape key
                }
            };
#if SCREEN_CAPTURE
		Console.SetWindowSize(150, 60);
		window.WindowState = WindowState.Normal;
		window.WindowBorder = WindowBorder.Hidden;
		Vector2i size = new(256);
		window.Bounds = new Box2i(pos, pos + size);
#endif
            window.ResetTimeSinceLastUpdate(); // reset time to avoid huge time step on first frame
            var s = window.ClientSize;
            GL.Viewport(0, 0, s.X, s.Y); // If we use our own game loop, we need to set the viewport manually
            return window;
        }

        public static float DeltaTime(this GameWindow gameWindow) => (float)gameWindow.TimeSinceLastUpdate();

        /// <summary>
        /// This routine is for a user defined game loop. Wait for the next frame, do double buffering, check if any events, like user input was called and return true until the window is closed.
        /// </summary>
        /// <param name="gameWindow"></param>
        /// <returns>Return true until the window is closed.</returns>
        public static bool NextFrame(this GameWindow gameWindow)
        {
            gameWindow.ResetTimeSinceLastUpdate();
            gameWindow.SwapBuffers(); //buffer swap for double/tripple buffering
            NativeWindow.ProcessWindowEvents(false); //handle all events that are sent to the window (user inputs, operating system stuff); this call could destroy window, so check immediately after this call if window still exists, otherwise GL calls will fail.
            return !gameWindow.IsExiting;
        }
    }
}
