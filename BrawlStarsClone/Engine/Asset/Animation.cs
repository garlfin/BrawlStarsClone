using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset;

public struct Animation
{
    public int FPS;
    public int FrameCount;

    public Channel[] ChannelFrames;
}

public struct Channel
{
    public string BoneName;
    public Matrix4X4<float>[] Frames;
}