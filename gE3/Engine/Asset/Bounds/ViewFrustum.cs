using Silk.NET.Maths;

namespace gE3.Engine.Asset.Bounds;

public struct ViewFrustum
{
    public FrustumPlane TopFace;
    public FrustumPlane BottomFace;

    public FrustumPlane RightFace;
    public FrustumPlane LeftFace;

    public FrustumPlane FarFace;
    public FrustumPlane NearFace;
    
    public ViewFrustum(FrustumPlane topFace, FrustumPlane bottomFace, FrustumPlane rightFace, FrustumPlane leftFace, FrustumPlane farFace, FrustumPlane nearFace)
    {
        TopFace = topFace;
        BottomFace = bottomFace;
        RightFace = rightFace;
        LeftFace = leftFace;
        FarFace = farFace;
        NearFace = nearFace;
    }
}