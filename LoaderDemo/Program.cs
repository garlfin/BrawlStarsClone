using LoaderDemo.Engine;

public static class Program
{
    public static void Main(string[] args)
    {
        DemoWindow window = new DemoWindow(1280, 720, "Mesh Loader Demo"
#if DEBUG
            , true
#endif
            );
        window.Run();
    }
}