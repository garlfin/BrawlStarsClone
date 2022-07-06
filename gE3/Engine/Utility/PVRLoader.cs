using Silk.NET.OpenGL;

namespace gE3.Engine.Utility;

public static class PvrLoader
{
    // Loads a PVR file from a stream.
    public static PVRHeader LoadPVR(BinaryReader reader)
    {
        return new PVRHeader
        {
            Version = reader.ReadUInt32(), // Version
            Flags = (Flags)reader.ReadUInt32(),
            Format = (PvrPixelFormat)reader.ReadUInt64(),
            ColorSpace = (ColorSpace)reader.ReadUInt32(),
            ChannelType = (ChannelType)reader.ReadUInt32(),
            Width = reader.ReadUInt32(),
            Height = reader.ReadUInt32(),
            Depth = reader.ReadUInt32(),
            NumSurfaces = reader.ReadUInt32(),
            NumFaces = reader.ReadUInt32(),
            MipMapCount = reader.ReadUInt32(),
            MetaDataSize = reader.ReadUInt32()
        };
    }

    public static InternalFormat ToInternalFormat(this PvrPixelFormat format)
    {
        return format switch
        {
            PvrPixelFormat.Dxt1 => InternalFormat.CompressedRgbaS3TCDxt1Ext,
            PvrPixelFormat.Dxt3 => InternalFormat.CompressedRgbaS3TCDxt3Ext,
            PvrPixelFormat.Dxt5 => InternalFormat.CompressedRgbaS3TCDxt5Ext,
            PvrPixelFormat.Bc4 => InternalFormat.CompressedRedRgtc1,
            PvrPixelFormat.Bc5 => InternalFormat.CompressedRGRgtc2,
            PvrPixelFormat.Etc2Rgb => InternalFormat.CompressedRgb8Etc2,
            PvrPixelFormat.Etc2Rgba => InternalFormat.CompressedRgba8Etc2Eac,
            PvrPixelFormat.Etc2RgbA1 => InternalFormat.CompressedRgb8PunchthroughAlpha1Etc2,
            PvrPixelFormat.EacR11 => InternalFormat.CompressedR11Eac,
            PvrPixelFormat.EacRg11 => InternalFormat.CompressedRG11Eac,
            PvrPixelFormat.Astc4X4 => InternalFormat.CompressedRgbaAstc4x4,
            PvrPixelFormat.Astc5X4 => InternalFormat.CompressedRgbaAstc5x4,
            PvrPixelFormat.Astc5X5 => InternalFormat.CompressedRgbaAstc5x5,
            PvrPixelFormat.Astc6X5 => InternalFormat.CompressedRgbaAstc6x5,
            PvrPixelFormat.Astc6X6 => InternalFormat.CompressedRgbaAstc6x6,
            PvrPixelFormat.Astc8X5 => InternalFormat.CompressedRgbaAstc8x5,
            PvrPixelFormat.Astc8X6 => InternalFormat.CompressedRgbaAstc8x6,
            PvrPixelFormat.Astc8X8 => InternalFormat.CompressedRgbaAstc8x8,
            PvrPixelFormat.Astc10X5 => InternalFormat.CompressedRgbaAstc10x5,
            PvrPixelFormat.Astc10X6 => InternalFormat.CompressedRgbaAstc10x6,
            PvrPixelFormat.Astc10X8 => InternalFormat.CompressedRgbaAstc10x8,
            PvrPixelFormat.Astc10X10 => InternalFormat.CompressedRgbaAstc10x10,
            PvrPixelFormat.Astc12X10 => InternalFormat.CompressedRgbaAstc12x10,
            PvrPixelFormat.Astc12X12 => InternalFormat.CompressedRgbaAstc12x12,
            PvrPixelFormat.Astc3X3X3 => InternalFormat.CompressedRgbaAstc3x3x3Oes,
            PvrPixelFormat.Astc4X3X3 => InternalFormat.CompressedRgbaAstc4x3x3Oes,
            PvrPixelFormat.Astc4X4X3 => InternalFormat.CompressedRgbaAstc4x4x3Oes,
            PvrPixelFormat.Astc4X4X4 => InternalFormat.CompressedRgbaAstc4x4x4Oes,
            PvrPixelFormat.Astc5X4X4 => InternalFormat.CompressedRgbaAstc5x4x4Oes,
            PvrPixelFormat.Astc5X5X4 => InternalFormat.CompressedRgbaAstc5x5x4Oes,
            PvrPixelFormat.Astc5X5X5 => InternalFormat.CompressedRgbaAstc5x5x5Oes,
            PvrPixelFormat.Astc6X5X5 => InternalFormat.CompressedRgbaAstc6x5x5Oes,
            PvrPixelFormat.Astc6X6X5 => InternalFormat.CompressedRgbaAstc6x6x5Oes,
            PvrPixelFormat.Astc6X6X6 => InternalFormat.CompressedRgbaAstc6x6x6Oes,
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }
}

public struct PVRHeader
{
    public uint Version;
    public Flags Flags;
    public PvrPixelFormat Format;
    public ColorSpace ColorSpace;
    public ChannelType ChannelType;
    public uint Width;
    public uint Height;
    public uint Depth;
    public uint NumSurfaces;
    public uint NumFaces;
    public uint MipMapCount;
    public uint MetaDataSize;
}

public enum ColorSpace
{
    Linear = 0,
    sRGB = 1
}
public enum Flags
{
    None = 0,
    PreMultipled = 2
}
public enum PvrPixelFormat
{
    Pvrtc2BppRgb = 0,
    Pvrtc2BppRgba = 1,
    Pvrtc4BppRgb = 2,
    Pvrtc4BppRgba = 3,
    PvrtcIi2Bpp = 4,
    PvrtcIi4Bpp = 5,
    Etc1 = 6,
    Dxt1 = 7,
    Dxt2 = 8,
    Dxt3 = 9,
    Dxt4 = 10,
    Dxt5 = 11,
    Bc1 = 7,
    Bc2 = 9,
    Bc3 = 11,
    Bc4 = 12,
    Bc5 = 13,
    Bc6 = 14,
    Bc7 = 15,
    Uyvy = 16,
    Yuy2 = 17,
    Bw1Bpp = 18,
    R9G9B9E5SharedExponent = 19,
    Rgbg8888 = 20,
    Grgb8888 = 21,
    Etc2Rgb = 22,
    Etc2Rgba = 23,
    Etc2RgbA1 = 24,
    EacR11 = 25,
    EacRg11 = 26,
    Astc4X4 = 27,
    Astc5X4 = 28,
    Astc5X5 = 29,
    Astc6X5 = 30,
    Astc6X6 = 31,
    Astc8X5 = 32,
    Astc8X6 = 33,
    Astc8X8 = 34,
    Astc10X5 = 35,
    Astc10X6 = 36,
    Astc10X8 = 37,
    Astc10X10 = 38,
    Astc12X10 = 39,
    Astc12X12 = 40,
    Astc3X3X3 = 41,
    Astc4X3X3 = 42,
    Astc4X4X3 = 43,
    Astc4X4X4 = 44,
    Astc5X4X4 = 45,
    Astc5X5X4 = 46,
    Astc5X5X5 = 47,
    Astc6X5X5 = 48,
    Astc6X6X5 = 49,
    Astc6X6X6 = 50
}

public enum ChannelType
{
    UnsignedByteNormalised = 0,
    SignedByteNormalised = 1,
    UnsignedByte = 2,
    DataTypeValue = 3,
    SignedByte = 4,
    UnsignedShortNormalised = 5,
    SignedShortNormalised = 6,
    UnsignedShort = 7,
    SignedShort = 8,
    UnsignedIntegerNormalised = 9,
    SignedIntegerNormalised = 10,
    UnsignedInteger = 11,
    SignedInteger = 12,
    Float = 13
}