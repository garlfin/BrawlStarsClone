using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset;

public struct Animation
{
    public int FPS;
    public int FrameCount;

    public Channel[] ChannelFrames;

    public Channel? this[string name]
    {
        get
        {
            for (ushort i = 0; i < ChannelFrames.Length; i++)
                if (ChannelFrames[i].BoneName == name)
                    return ChannelFrames[i];
            return null;
        }
    }

    public Channel this[int index] => ChannelFrames[index];
}

public struct Channel
{
    public string BoneName;
    public Matrix4X4<float>[] Frames;
}