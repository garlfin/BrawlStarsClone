using System.Runtime.InteropServices;
using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Utility;

internal static class Debug
{
    private static GameWindow _window;
    private static readonly DebugProc _dp = Log;
    private static void Log(GLEnum source, GLEnum type, int id, GLEnum severity, int length,
        nint message, nint userParam)
    {
        if (severity is GLEnum.DebugSeverityNotification) return;
        //if (severity is GLEnum.DebugSeverityHigh)
        //    throw new System.Exception(Marshal.PtrToStringAnsi(message, length));
        Console.WriteLine($"SEVERITY: {severity}; MESSAGE: {Marshal.PtrToStringAnsi(message, length)}");
    }

    public static void Init(GameWindow window)
    {
        _window = window;
        Console.WriteLine("DEBUG: Debug initialize");
        _window.GL.Enable(EnableCap.DebugOutput);
        _window.GL.DebugMessageCallback(_dp, (IntPtr)0);
    }
    
    public static void CheckError()
    {
        if (_window == null) return;
        GLEnum error = _window.GL.GetError();
        if (error != GLEnum.NoError)
            Console.WriteLine($"GL ERROR: {error}");
    }
}