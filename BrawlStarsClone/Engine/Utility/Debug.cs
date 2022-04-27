using System.Runtime.InteropServices;
using BrawlStarsClone.Engine.Windowing;
using Silk.NET.OpenGL;

namespace BrawlStarsClone.Engine.Utility
{
    internal static class Debug
    {
        private static DebugProc _dp = Log;

        public static unsafe void Init(GameWindow window)
        {
            Console.Write("DEBUG: Debug initialize");
            window.gl.Enable(EnableCap.DebugOutput);
            window.gl.DebugMessageCallback(_dp, (void*) 0);
        }
        
        private static void Log(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userparam)
        {
            Console.WriteLine(Marshal.PtrToStringAnsi(message, length));
        }
    }
}