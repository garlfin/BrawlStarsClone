using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace BrawlStarsClone.Engine.Utility;

internal static class Debug
{
    private static readonly DebugProc _dp = Log;

    private static void Log(DebugSource source, DebugType type, int id, DebugSeverity severity, int length,
        IntPtr message, IntPtr userparam)
    {
        if (severity is DebugSeverity.DebugSeverityNotification) return;
        if (severity is DebugSeverity.DebugSeverityHigh) throw new Exception(Marshal.PtrToStringAnsi(message, length));
        Console.WriteLine($"SEVERITY: {severity}; MESSAGE: {Marshal.PtrToStringAnsi(message, length)}");
    }

    public static void Init()
    {
        Console.WriteLine("DEBUG: Debug initialize");
        GL.Enable(EnableCap.DebugOutput);
        GL.DebugMessageCallback(_dp, (IntPtr) 0);
    }
}