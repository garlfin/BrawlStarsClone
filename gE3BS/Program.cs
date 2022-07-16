using gE3BS.Engine;

namespace gE3BS;

public static class Program
{
    public static void Main(string[] args)
    {
        new BrawlWindow(1280, 720, "BrawlStars", true).Run();
    }
}