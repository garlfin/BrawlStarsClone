using gE3.Engine.Component;

namespace gE3.Engine.Asset.Mesh;

public struct Animation
{
    public int FPS;
    public int FrameCount;

    public Channel[] ChannelFrames;

    public double Time => (double)FrameCount / FPS;

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

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        return obj.GetHashCode() == GetHashCode();
    }

    public Channel this[int index] => ChannelFrames[index];
}

public struct Channel
{
    public string BoneName;
    public TransformationQuaternion[] Frames;

    public override string ToString()
    {
        return BoneName;
    }
}