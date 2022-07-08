using gE3.Engine.Asset.Texture;
using Silk.NET.OpenGL;

namespace gE3.Engine.Utility;

public static class PvrLoader
{
    // Loads a PVR file from a stream.
    public static PVRHeader LoadPVR(BinaryReader reader)
    {
        var pvr = new PVRHeader();
        
        pvr.Version = reader.ReadUInt32(); // Version
        pvr.Flags = (Flags)reader.ReadUInt32();

        var preFormat = reader.ReadInt32();
        if (preFormat == 0) pvr.Format = (PvrPixelFormat)reader.ReadUInt32();
        else
        {
            reader.ReadUInt32();
            pvr.Format = (PvrPixelFormat)preFormat;
        }
        pvr.ColorSpace = (ColorSpace)reader.ReadUInt32();
        pvr.ChannelType = (ChannelType)reader.ReadUInt32();
        pvr.Width = reader.ReadUInt32();
        pvr.Height = reader.ReadUInt32();
        pvr.Depth = reader.ReadUInt32();
        pvr.NumSurfaces = reader.ReadUInt32();
        pvr.NumFaces = reader.ReadUInt32();
        pvr.MipMapCount = reader.ReadUInt32();
        pvr.MetaDataSize = reader.ReadUInt32();
        
        return pvr;
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
            PvrPixelFormat.RGB => InternalFormat.Rgb32f,
            PvrPixelFormat.RGBA => InternalFormat.Rgba,
            
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    public static PixelType ToPixelType(this ChannelType format)
    {
        return format switch
        {
            ChannelType.Float => PixelType.Float,
            ChannelType.UnsignedByte => PixelType.UnsignedByte,
            ChannelType.UnsignedShort => PixelType.UnsignedShort,
            ChannelType.UnsignedInteger => PixelType.UnsignedInt,
            ChannelType.SignedByte => PixelType.Byte,
            ChannelType.SignedShort => PixelType.Short,
            ChannelType.SignedInteger => PixelType.Int,
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    public static PixelFormat ToPixelFormat(this PvrPixelFormat format)
    {
        return format switch
        {
            PvrPixelFormat.Bc4 => PixelFormat.Red,
            PvrPixelFormat.Bc5 => PixelFormat.RG,
            PvrPixelFormat.RGB => PixelFormat.Rgb,
            _ => PixelFormat.Rgba
        };
    }

    public static CompressionRatio Compression(this PvrPixelFormat format)
    {
        return format switch
        {
            PvrPixelFormat.Dxt1 => Ratios[0],
            _ => Ratios[1]
        };
    }

    private static readonly CompressionRatio[] Ratios =
    {
        new(16, 8), // DXT1
        new(16, 16), // DXT2 & 3, BC4 & 5
    };
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
    
    public CompressionRatio CompressionRatio => Format.Compression();
    public bool Compressed => Format is PvrPixelFormat.Dxt1 or PvrPixelFormat.Dxt3 or PvrPixelFormat.Dxt5 or PvrPixelFormat.Bc4 or PvrPixelFormat.Bc5;
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
    Astc6X6X6 = 50,
    R = 114,
    RG = 26482,
    RGB = 6449010,
    RGBA = 1633838962
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