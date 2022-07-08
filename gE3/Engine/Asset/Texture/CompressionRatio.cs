namespace gE3.Engine.Asset.Texture;

public struct CompressionRatio
{
    public uint Pixels;
    public uint Bytes;
    
    public CompressionRatio(uint pixels, uint bytes)
    {
        Pixels = pixels;
        Bytes = bytes;
    }
}