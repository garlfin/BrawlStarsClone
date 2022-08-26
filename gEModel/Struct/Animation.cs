using gEModel.Struct;

namespace gEModel.Struct;

public class Animation
{
    public static Animation FromgETF(ref gETF file)
    {
        FrameHolder[] frames = new FrameHolder[file.Nodes.Length * file.FrameCount];

        for (int i = 0; i < file.Body.NodeCount; i++)
        {
            for (int f = 0; f < file.FrameCount; f++)
            {
                frames[i + f] = new FrameHolder(ref file.Nodes[i].Frames[f], file.Nodes[i].Name);
            }
        }
        
        return new Animation(frames, file.FPS, file.Nodes.Length);
    }
    
    public float FPS { get; }
    public int FrameCount => Frames.Length;
    public int BoneCount { get; }
    public FrameHolder[] Frames { get; }

    public Animation(FrameHolder[] frames, float fps, int boneCount)
    {
        Frames = frames;
        FPS = fps;
        BoneCount = boneCount;
    }

    public Frame? this[string boneName, int frame]
    {
        get
        {
            for (int i = 0; i < FrameCount; i++)
            {
                if (Frames[i * BoneCount].BoneName == boneName)
                {
                    return Frames[i * BoneCount + frame].Frame;
                }
            }

            return null;
        }
    }
}

public struct FrameHolder
{
    public string BoneName;
    public Frame Frame;

    public FrameHolder(ref Frame frame, string boneName)
    {
        BoneName = boneName;
        Frame = frame;
    }
}