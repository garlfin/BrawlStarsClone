/* ======================================================================================== */
/* FMOD Core API - C# wrapper.                                                              */
/* Copyright (c), Firelight Technologies Pty, Ltd. 2004-2022.                               */
/*                                                                                          */
/* For more detail visit:                                                                   */
/* https://fmod.com/resources/documentation-api?version=2.0&page=core-api.html              */
/* ======================================================================================== */

using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace FMOD
{
    /*
        FMOD version number.  Check this against FMOD::System::getVersion / System_GetVersion
        0xaaaabbcc -> aaaa = major version number.  bb = minor version number.  cc = development version number.
    */
    public partial class Version
    {
        public const int    Number = 0x00020207;
#if !UNITY_2019_4_OR_NEWER
        public const string Dll    = "fmod";
#endif
    }

    public class Constants
    {
        public const int MaxChannelWidth = 32;
        public const int MaxListeners = 8;
        public const int ReverbMaxinstances = 4;
        public const int MaxSystems = 8;
    }

    /*
        FMOD core types
    */
    public enum Result : int
    {
        Ok,
        ErrBadcommand,
        ErrChannelAlloc,
        ErrChannelStolen,
        ErrDma,
        ErrDspConnection,
        ErrDspDontprocess,
        ErrDspFormat,
        ErrDspInuse,
        ErrDspNotfound,
        ErrDspReserved,
        ErrDspSilence,
        ErrDspType,
        ErrFileBad,
        ErrFileCouldnotseek,
        ErrFileDiskejected,
        ErrFileEof,
        ErrFileEndofdata,
        ErrFileNotfound,
        ErrFormat,
        ErrHeaderMismatch,
        ErrHttp,
        ErrHttpAccess,
        ErrHttpProxyAuth,
        ErrHttpServerError,
        ErrHttpTimeout,
        ErrInitialization,
        ErrInitialized,
        ErrInternal,
        ErrInvalidFloat,
        ErrInvalidHandle,
        ErrInvalidParam,
        ErrInvalidPosition,
        ErrInvalidSpeaker,
        ErrInvalidSyncpoint,
        ErrInvalidThread,
        ErrInvalidVector,
        ErrMaxaudible,
        ErrMemory,
        ErrMemoryCantpoint,
        ErrNeeds3D,
        ErrNeedshardware,
        ErrNetConnect,
        ErrNetSocketError,
        ErrNetUrl,
        ErrNetWouldBlock,
        ErrNotready,
        ErrOutputAllocated,
        ErrOutputCreatebuffer,
        ErrOutputDrivercall,
        ErrOutputFormat,
        ErrOutputInit,
        ErrOutputNodrivers,
        ErrPlugin,
        ErrPluginMissing,
        ErrPluginResource,
        ErrPluginVersion,
        ErrRecord,
        ErrReverbChannelgroup,
        ErrReverbInstance,
        ErrSubsounds,
        ErrSubsoundAllocated,
        ErrSubsoundCantmove,
        ErrTagnotfound,
        ErrToomanychannels,
        ErrTruncated,
        ErrUnimplemented,
        ErrUninitialized,
        ErrUnsupported,
        ErrVersion,
        ErrEventAlreadyLoaded,
        ErrEventLiveupdateBusy,
        ErrEventLiveupdateMismatch,
        ErrEventLiveupdateTimeout,
        ErrEventNotfound,
        ErrStudioUninitialized,
        ErrStudioNotLoaded,
        ErrInvalidString,
        ErrAlreadyLocked,
        ErrNotLocked,
        ErrRecordDisconnected,
        ErrToomanysamples,
    }

    public enum ChannelcontrolType : int
    {
        Channel,
        Channelgroup,
        Max
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector
    {
        public float x;
        public float y;
        public float z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Attributes3D
    {
        public Vector position;
        public Vector velocity;
        public Vector forward;
        public Vector up;
    }

    [StructLayout(LayoutKind.Sequential)]
    public partial struct Guid
    {
        public int Data1;
        public int Data2;
        public int Data3;
        public int Data4;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Asyncreadinfo
    {
        public IntPtr   handle;
        public uint     offset;
        public uint     sizebytes;
        public int      priority;

        public IntPtr   userdata;
        public IntPtr   buffer;
        public uint     bytesread;
        public FileAsyncdoneFunc done;
    }

    public enum Outputtype : int
    {
        Autodetect,

        Unknown,
        Nosound,
        Wavwriter,
        NosoundNrt,
        WavwriterNrt,

        Wasapi,
        Asio,
        Pulseaudio,
        Alsa,
        Coreaudio,
        Audiotrack,
        Opensl,
        Audioout,
        Audio3D,
        Webaudio,
        Nnaudio,
        Winsonic,
        Aaudio,
        Audioworklet,

        Max,
    }

    public enum PortType : int
    {
        Music,
        CopyrightMusic,
        Voice,
        Controller,
        Personal,
        Vibration,
        Aux,

        Max
    }

    public enum DebugMode : int
    {
        Tty,
        File,
        Callback,
    }

    [Flags]
    public enum DebugFlags : uint
    {
        None                    = 0x00000000,
        Error                   = 0x00000001,
        Warning                 = 0x00000002,
        Log                     = 0x00000004,

        TypeMemory             = 0x00000100,
        TypeFile               = 0x00000200,
        TypeCodec              = 0x00000400,
        TypeTrace              = 0x00000800,

        DisplayTimestamps      = 0x00010000,
        DisplayLinenumbers     = 0x00020000,
        DisplayThread          = 0x00040000,
    }

    [Flags]
    public enum MemoryType : uint
    {
        Normal                  = 0x00000000,
        StreamFile             = 0x00000001,
        StreamDecode           = 0x00000002,
        Sampledata              = 0x00000004,
        DspBuffer              = 0x00000008,
        Plugin                  = 0x00000010,
        Persistent              = 0x00200000,
        All                     = 0xFFFFFFFF
    }

    public enum Speakermode : int
    {
        Default,
        Raw,
        Mono,
        Stereo,
        Quad,
        Surround,
        _5POINT1,
        _7POINT1,
        _7POINT1POINT4,

        Max,
    }
     
    public enum Speaker : int
    {
        None = -1,
        FrontLeft,
        FrontRight,
        FrontCenter,
        LowFrequency,
        SurroundLeft,
        SurroundRight,
        BackLeft,
        BackRight,
        TopFrontLeft,
        TopFrontRight,
        TopBackLeft,
        TopBackRight,

        Max,
    }

    [Flags]
    public enum Channelmask : uint
    {
        FrontLeft             = 0x00000001,
        FrontRight            = 0x00000002,
        FrontCenter           = 0x00000004,
        LowFrequency          = 0x00000008,
        SurroundLeft          = 0x00000010,
        SurroundRight         = 0x00000020,
        BackLeft              = 0x00000040,
        BackRight             = 0x00000080,
        BackCenter            = 0x00000100,

        Mono                   = (FrontLeft),
        Stereo                 = (FrontLeft | FrontRight),
        Lrc                    = (FrontLeft | FrontRight | FrontCenter),
        Quad                   = (FrontLeft | FrontRight | SurroundLeft | SurroundRight),
        Surround               = (FrontLeft | FrontRight | FrontCenter | SurroundLeft | SurroundRight),
        _5POINT1               = (FrontLeft | FrontRight | FrontCenter | LowFrequency | SurroundLeft | SurroundRight),
        _5POINT1_REARS         = (FrontLeft | FrontRight | FrontCenter | LowFrequency | BackLeft | BackRight),
        _7POINT0               = (FrontLeft | FrontRight | FrontCenter | SurroundLeft | SurroundRight | BackLeft | BackRight),
        _7POINT1               = (FrontLeft | FrontRight | FrontCenter | LowFrequency | SurroundLeft | SurroundRight | BackLeft | BackRight)
    }

    public enum Channelorder : int
    {
        Default,
        Waveformat,
        Protools,
        Allmono,
        Allstereo,
        Alsa,

        Max,
    }

    public enum Plugintype : int
    {
        Output,
        Codec,
        Dsp,

        Max,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Pluginlist
    {
        Plugintype type;
        IntPtr description;
    }

    [Flags]
    public enum Initflags : uint
    {
        Normal                     = 0x00000000,
        StreamFromUpdate         = 0x00000001,
        MixFromUpdate            = 0x00000002,
        _3D_RIGHTHANDED            = 0x00000004,
        ChannelLowpass            = 0x00000100,
        ChannelDistancefilter     = 0x00000200,
        ProfileEnable             = 0x00010000,
        Vol0BecomesVirtual       = 0x00020000,
        GeometryUseclosest        = 0x00040000,
        PreferDolbyDownmix       = 0x00080000,
        ThreadUnsafe              = 0x00100000,
        ProfileMeterAll          = 0x00200000,
        MemoryTracking            = 0x00400000,
    }

    public enum SoundType : int
    {
        Unknown,
        Aiff,
        Asf,
        Dls,
        Flac,
        Fsb,
        It,
        Midi,
        Mod,
        Mpeg,
        Oggvorbis,
        Playlist,
        Raw,
        S3M,
        User,
        Wav,
        Xm,
        Xma,
        Audioqueue,
        At9,
        Vorbis,
        MediaFoundation,
        Mediacodec,
        Fadpcm,
        Opus,

        Max,
    }

    public enum SoundFormat : int
    {
        None,
        Pcm8,
        Pcm16,
        Pcm24,
        Pcm32,
        Pcmfloat,
        Bitstream,

        Max
    }

    [Flags]
    public enum Mode : uint
    {
        Default                     = 0x00000000,
        LoopOff                    = 0x00000001,
        LoopNormal                 = 0x00000002,
        LoopBidi                   = 0x00000004,
        _2D                         = 0x00000008,
        _3D                         = 0x00000010,
        Createstream                = 0x00000080,
        Createsample                = 0x00000100,
        Createcompressedsample      = 0x00000200,
        Openuser                    = 0x00000400,
        Openmemory                  = 0x00000800,
        OpenmemoryPoint            = 0x10000000,
        Openraw                     = 0x00001000,
        Openonly                    = 0x00002000,
        Accuratetime                = 0x00004000,
        Mpegsearch                  = 0x00008000,
        Nonblocking                 = 0x00010000,
        Unique                      = 0x00020000,
        _3D_HEADRELATIVE            = 0x00040000,
        _3D_WORLDRELATIVE           = 0x00080000,
        _3D_INVERSEROLLOFF          = 0x00100000,
        _3D_LINEARROLLOFF           = 0x00200000,
        _3D_LINEARSQUAREROLLOFF     = 0x00400000,
        _3D_INVERSETAPEREDROLLOFF   = 0x00800000,
        _3D_CUSTOMROLLOFF           = 0x04000000,
        _3D_IGNOREGEOMETRY          = 0x40000000,
        Ignoretags                  = 0x02000000,
        Lowmem                      = 0x08000000,
        VirtualPlayfromstart       = 0x80000000
    }

    public enum Openstate : int
    {
        Ready = 0,
        Loading,
        Error,
        Connecting,
        Buffering,
        Seeking,
        Playing,
        Setposition,

        Max,
    }

    public enum SoundgroupBehavior : int
    {
        BehaviorFail,
        BehaviorMute,
        BehaviorSteallowest,

        Max,
    }

    public enum ChannelcontrolCallbackType : int
    {
        End,
        Virtualvoice,
        Syncpoint,
        Occlusion,

        Max,
    }

    public struct ChannelcontrolDspIndex
    {
        public const int Head    = -1;
        public const int Fader   = -2;
        public const int Tail    = -3;
    }

    public enum ErrorcallbackInstancetype : int
    {
        None,
        System,
        Channel,
        Channelgroup,
        Channelcontrol,
        Sound,
        Soundgroup,
        Dsp,
        Dspconnection,
        Geometry,
        Reverb3D,
        StudioSystem,
        StudioEventdescription,
        StudioEventinstance,
        StudioParameterinstance,
        StudioBus,
        StudioVca,
        StudioBank,
        StudioCommandreplay
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ErrorcallbackInfo
    {
        public  Result                      result;
        public  ErrorcallbackInstancetype  instancetype;
        public  IntPtr                      instance;
        public  StringWrapper               functionname;
        public  StringWrapper               functionparams;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CpuUsage
    {
        public float    dsp;                    /* DSP mixing CPU usage. */
        public float    stream;                 /* Streaming engine CPU usage. */
        public float    geometry;               /* Geometry engine CPU usage. */
        public float    update;                 /* System::update CPU usage. */
        public float    convolution1;           /* Convolution reverb processing thread #1 CPU usage */
        public float    convolution2;           /* Convolution reverb processing thread #2 CPU usage */ 
    }

    [Flags]
    public enum SystemCallbackType : uint
    {
        Devicelistchanged      = 0x00000001,
        Devicelost             = 0x00000002,
        Memoryallocationfailed = 0x00000004,
        Threadcreated          = 0x00000008,
        Baddspconnection       = 0x00000010,
        Premix                 = 0x00000020,
        Postmix                = 0x00000040,
        Error                  = 0x00000080,
        Midmix                 = 0x00000100,
        Threaddestroyed        = 0x00000200,
        Preupdate              = 0x00000400,
        Postupdate             = 0x00000800,
        Recordlistchanged      = 0x00001000,
        Bufferednomix          = 0x00002000,
        Devicereinitialize     = 0x00004000,
        Outputunderrun         = 0x00008000,
        All                    = 0xFFFFFFFF,
    }

    /*
        FMOD Callbacks
    */
    public delegate Result DebugCallback           (DebugFlags flags, IntPtr file, int line, IntPtr func, IntPtr message);
    public delegate Result SystemCallback          (IntPtr system, SystemCallbackType type, IntPtr commanddata1, IntPtr commanddata2, IntPtr userdata);
    public delegate Result ChannelcontrolCallback  (IntPtr channelcontrol, ChannelcontrolType controltype, ChannelcontrolCallbackType callbacktype, IntPtr commanddata1, IntPtr commanddata2);
    public delegate Result SoundNonblockCallback  (IntPtr sound, Result result);
    public delegate Result SoundPcmreadCallback   (IntPtr sound, IntPtr data, uint datalen);
    public delegate Result SoundPcmsetposCallback (IntPtr sound, int subsound, uint position, Timeunit postype);
    public delegate Result FileOpenCallback       (IntPtr name, ref uint filesize, ref IntPtr handle, IntPtr userdata);
    public delegate Result FileCloseCallback      (IntPtr handle, IntPtr userdata);
    public delegate Result FileReadCallback       (IntPtr handle, IntPtr buffer, uint sizebytes, ref uint bytesread, IntPtr userdata);
    public delegate Result FileSeekCallback       (IntPtr handle, uint pos, IntPtr userdata);
    public delegate Result FileAsyncreadCallback  (IntPtr info, IntPtr userdata);
    public delegate Result FileAsynccancelCallback(IntPtr info, IntPtr userdata);
    public delegate Result FileAsyncdoneFunc      (IntPtr info, Result result);
    public delegate IntPtr MemoryAllocCallback    (uint size, MemoryType type, IntPtr sourcestr);
    public delegate IntPtr MemoryReallocCallback  (IntPtr ptr, uint size, MemoryType type, IntPtr sourcestr);
    public delegate void   MemoryFreeCallback     (IntPtr ptr, MemoryType type, IntPtr sourcestr);
    public delegate float  Cb3DRolloffCallback   (IntPtr channelcontrol, float distance);

    public enum DspResampler : int
    {
        Default,
        Nointerp,
        Linear,
        Cubic,
        Spline,

        Max,
    }

    public enum DspconnectionType : int
    {
        Standard,
        Sidechain,
        Send,
        SendSidechain,

        Max,
    }

    public enum Tagtype : int
    {
        Unknown = 0,
        Id3V1,
        Id3V2,
        Vorbiscomment,
        Shoutcast,
        Icecast,
        Asf,
        Midi,
        Playlist,
        Fmod,
        User,

        Max
    }

    public enum Tagdatatype : int
    {
        Binary = 0,
        Int,
        Float,
        String,
        StringUtf16,
        StringUtf16Be,
        StringUtf8,

        Max
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Tag
    {
        public  Tagtype           type;
        public  Tagdatatype       datatype;
        public  StringWrapper     name;
        public  IntPtr            data;
        public  uint              datalen;
        public  bool              updated;
    }

    [Flags]
    public enum Timeunit : uint
    {
        Ms          = 0x00000001,
        Pcm         = 0x00000002,
        Pcmbytes    = 0x00000004,
        Rawbytes    = 0x00000008,
        Pcmfraction = 0x00000010,
        Modorder    = 0x00000100,
        Modrow      = 0x00000200,
        Modpattern  = 0x00000400,
    }

    public struct PortIndex
    {
        public const ulong None = 0xFFFFFFFFFFFFFFFF;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Createsoundexinfo
    {
        public int                         cbsize;
        public uint                        length;
        public uint                        fileoffset;
        public int                         numchannels;
        public int                         defaultfrequency;
        public SoundFormat                format;
        public uint                        decodebuffersize;
        public int                         initialsubsound;
        public int                         numsubsounds;
        public IntPtr                      inclusionlist;
        public int                         inclusionlistnum;
        public IntPtr                      pcmreadcallback_internal;
        public IntPtr                      pcmsetposcallback_internal;
        public IntPtr                      nonblockcallback_internal;
        public IntPtr                      dlsname;
        public IntPtr                      encryptionkey;
        public int                         maxpolyphony;
        public IntPtr                      userdata;
        public SoundType                  suggestedsoundtype;
        public IntPtr                      fileuseropen_internal;
        public IntPtr                      fileuserclose_internal;
        public IntPtr                      fileuserread_internal;
        public IntPtr                      fileuserseek_internal;
        public IntPtr                      fileuserasyncread_internal;
        public IntPtr                      fileuserasynccancel_internal;
        public IntPtr                      fileuserdata;
        public int                         filebuffersize;
        public Channelorder                channelorder;
        public IntPtr                      initialsoundgroup;
        public uint                        initialseekposition;
        public Timeunit                    initialseekpostype;
        public int                         ignoresetfilesystem;
        public uint                        audioqueuepolicy;
        public uint                        minmidigranularity;
        public int                         nonblockthreadid;
        public IntPtr                      fsbguid;

        public SoundPcmreadCallback Pcmreadcallback
        {
            set { pcmreadcallback_internal = (value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value)); }
            get { return pcmreadcallback_internal == IntPtr.Zero ? null : (SoundPcmreadCallback)Marshal.GetDelegateForFunctionPointer(pcmreadcallback_internal, typeof(SoundPcmreadCallback)); }
        }
        public SoundPcmsetposCallback Pcmsetposcallback
        {
            set { pcmsetposcallback_internal = (value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value)); }
            get { return pcmsetposcallback_internal == IntPtr.Zero ? null : (SoundPcmsetposCallback)Marshal.GetDelegateForFunctionPointer(pcmsetposcallback_internal, typeof(SoundPcmsetposCallback)); }
        }
        public SoundNonblockCallback Nonblockcallback
        {
            set { nonblockcallback_internal = (value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value)); }
            get { return nonblockcallback_internal == IntPtr.Zero ? null : (SoundNonblockCallback)Marshal.GetDelegateForFunctionPointer(nonblockcallback_internal, typeof(SoundNonblockCallback)); }
        }
        public FileOpenCallback Fileuseropen
        {
            set { fileuseropen_internal = (value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value)); }
            get { return fileuseropen_internal == IntPtr.Zero ? null : (FileOpenCallback)Marshal.GetDelegateForFunctionPointer(fileuseropen_internal, typeof(FileOpenCallback)); }
        }
        public FileCloseCallback Fileuserclose
        {
            set { fileuserclose_internal = (value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value)); }
            get { return fileuserclose_internal == IntPtr.Zero ? null : (FileCloseCallback)Marshal.GetDelegateForFunctionPointer(fileuserclose_internal, typeof(FileCloseCallback)); }
        }
        public FileReadCallback Fileuserread
        {
            set { fileuserread_internal = (value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value)); }
            get { return fileuserread_internal == IntPtr.Zero ? null : (FileReadCallback)Marshal.GetDelegateForFunctionPointer(fileuserread_internal, typeof(FileReadCallback)); }
        }
        public FileSeekCallback Fileuserseek
        {
            set { fileuserseek_internal = (value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value)); }
            get { return fileuserseek_internal == IntPtr.Zero ? null : (FileSeekCallback)Marshal.GetDelegateForFunctionPointer(fileuserseek_internal, typeof(FileSeekCallback)); }
        }
        public FileAsyncreadCallback Fileuserasyncread
        {
            set { fileuserasyncread_internal = (value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value)); }
            get { return fileuserasyncread_internal == IntPtr.Zero ? null : (FileAsyncreadCallback)Marshal.GetDelegateForFunctionPointer(fileuserasyncread_internal, typeof(FileAsyncreadCallback)); }
        }
        public FileAsynccancelCallback Fileuserasynccancel
        {
            set { fileuserasynccancel_internal = (value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(value)); }
            get { return fileuserasynccancel_internal == IntPtr.Zero ? null : (FileAsynccancelCallback)Marshal.GetDelegateForFunctionPointer(fileuserasynccancel_internal, typeof(FileAsynccancelCallback)); }
        }

    }

#pragma warning disable 414
    [StructLayout(LayoutKind.Sequential)]
    public struct ReverbProperties
    {
        public float DecayTime;
        public float EarlyDelay;
        public float LateDelay;
        public float HFReference;
        public float HFDecayRatio;
        public float Diffusion;
        public float Density;
        public float LowShelfFrequency;
        public float LowShelfGain;
        public float HighCut;
        public float EarlyLateMix;
        public float WetLevel;

        #region wrapperinternal
        public ReverbProperties(float decayTime, float earlyDelay, float lateDelay, float hfReference,
            float hfDecayRatio, float diffusion, float density, float lowShelfFrequency, float lowShelfGain,
            float highCut, float earlyLateMix, float wetLevel)
        {
            DecayTime = decayTime;
            EarlyDelay = earlyDelay;
            LateDelay = lateDelay;
            HFReference = hfReference;
            HFDecayRatio = hfDecayRatio;
            Diffusion = diffusion;
            Density = density;
            LowShelfFrequency = lowShelfFrequency;
            LowShelfGain = lowShelfGain;
            HighCut = highCut;
            EarlyLateMix = earlyLateMix;
            WetLevel = wetLevel;
        }
        #endregion
    }
#pragma warning restore 414

    public class Preset
    {
        public static ReverbProperties Off()                 { return new ReverbProperties(  1000,    7,  11, 5000, 100, 100, 100, 250, 0,    20,  96, -80.0f );}
        public static ReverbProperties Generic()             { return new ReverbProperties(  1500,    7,  11, 5000,  83, 100, 100, 250, 0, 14500,  96,  -8.0f );}
        public static ReverbProperties Paddedcell()          { return new ReverbProperties(   170,    1,   2, 5000,  10, 100, 100, 250, 0,   160,  84,  -7.8f );}
        public static ReverbProperties Room()                { return new ReverbProperties(   400,    2,   3, 5000,  83, 100, 100, 250, 0,  6050,  88,  -9.4f );}
        public static ReverbProperties Bathroom()            { return new ReverbProperties(  1500,    7,  11, 5000,  54, 100,  60, 250, 0,  2900,  83,   0.5f );}
        public static ReverbProperties Livingroom()          { return new ReverbProperties(   500,    3,   4, 5000,  10, 100, 100, 250, 0,   160,  58, -19.0f );}
        public static ReverbProperties Stoneroom()           { return new ReverbProperties(  2300,   12,  17, 5000,  64, 100, 100, 250, 0,  7800,  71,  -8.5f );}
        public static ReverbProperties Auditorium()          { return new ReverbProperties(  4300,   20,  30, 5000,  59, 100, 100, 250, 0,  5850,  64, -11.7f );}
        public static ReverbProperties Concerthall()         { return new ReverbProperties(  3900,   20,  29, 5000,  70, 100, 100, 250, 0,  5650,  80,  -9.8f );}
        public static ReverbProperties Cave()                { return new ReverbProperties(  2900,   15,  22, 5000, 100, 100, 100, 250, 0, 20000,  59, -11.3f );}
        public static ReverbProperties Arena()               { return new ReverbProperties(  7200,   20,  30, 5000,  33, 100, 100, 250, 0,  4500,  80,  -9.6f );}
        public static ReverbProperties Hangar()              { return new ReverbProperties( 10000,   20,  30, 5000,  23, 100, 100, 250, 0,  3400,  72,  -7.4f );}
        public static ReverbProperties Carpettedhallway()    { return new ReverbProperties(   300,    2,  30, 5000,  10, 100, 100, 250, 0,   500,  56, -24.0f );}
        public static ReverbProperties Hallway()             { return new ReverbProperties(  1500,    7,  11, 5000,  59, 100, 100, 250, 0,  7800,  87,  -5.5f );}
        public static ReverbProperties Stonecorridor()       { return new ReverbProperties(   270,   13,  20, 5000,  79, 100, 100, 250, 0,  9000,  86,  -6.0f );}
        public static ReverbProperties Alley()               { return new ReverbProperties(  1500,    7,  11, 5000,  86, 100, 100, 250, 0,  8300,  80,  -9.8f );}
        public static ReverbProperties Forest()              { return new ReverbProperties(  1500,  162,  88, 5000,  54,  79, 100, 250, 0,   760,  94, -12.3f );}
        public static ReverbProperties City()                { return new ReverbProperties(  1500,    7,  11, 5000,  67,  50, 100, 250, 0,  4050,  66, -26.0f );}
        public static ReverbProperties Mountains()           { return new ReverbProperties(  1500,  300, 100, 5000,  21,  27, 100, 250, 0,  1220,  82, -24.0f );}
        public static ReverbProperties Quarry()              { return new ReverbProperties(  1500,   61,  25, 5000,  83, 100, 100, 250, 0,  3400, 100,  -5.0f );}
        public static ReverbProperties Plain()               { return new ReverbProperties(  1500,  179, 100, 5000,  50,  21, 100, 250, 0,  1670,  65, -28.0f );}
        public static ReverbProperties Parkinglot()          { return new ReverbProperties(  1700,    8,  12, 5000, 100, 100, 100, 250, 0, 20000,  56, -19.5f );}
        public static ReverbProperties Sewerpipe()           { return new ReverbProperties(  2800,   14,  21, 5000,  14,  80,  60, 250, 0,  3400,  66,   1.2f );}
        public static ReverbProperties Underwater()          { return new ReverbProperties(  1500,    7,  11, 5000,  10, 100, 100, 250, 0,   500,  92,   7.0f );}
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Advancedsettings
    {
        public int                 cbSize;
        public int                 maxMPEGCodecs;
        public int                 maxADPCMCodecs;
        public int                 maxXMACodecs;
        public int                 maxVorbisCodecs;
        public int                 maxAT9Codecs;
        public int                 maxFADPCMCodecs;
        public int                 maxPCMCodecs;
        public int                 ASIONumChannels;
        public IntPtr              ASIOChannelList;
        public IntPtr              ASIOSpeakerList;
        public float               vol0virtualvol;
        public uint                defaultDecodeBufferSize;
        public ushort              profilePort;
        public uint                geometryMaxFadeTime;
        public float               distanceFilterCenterFreq;
        public int                 reverb3Dinstance;
        public int                 DSPBufferPoolSize;
        public DspResampler       resamplerMethod;
        public uint                randomSeed;
        public int                 maxConvolutionThreads;
        public int                 maxOpusCodecs;
    }

    [Flags]
    public enum DriverState : uint
    {
        Connected = 0x00000001,
        Default   = 0x00000002,
    }

    public enum ThreadPriority : int
    {
        /* Platform specific priority range */
        PlatformMin        = -32 * 1024,
        PlatformMax        =  32 * 1024,

        /* Platform agnostic priorities, maps internally to platform specific value */
        Default             = PlatformMin - 1,
        Low                 = PlatformMin - 2,
        Medium              = PlatformMin - 3,
        High                = PlatformMin - 4,
        VeryHigh           = PlatformMin - 5,
        Extreme             = PlatformMin - 6,
        Critical            = PlatformMin - 7,
        
        /* Thread defaults */
        Mixer               = Extreme,
        Feeder              = Critical,
        Stream              = VeryHigh,
        File                = High,
        Nonblocking         = High,
        Record              = High,
        Geometry            = Low,
        Profiler            = Medium,
        StudioUpdate       = Medium,
        StudioLoadBank    = Medium,
        StudioLoadSample  = Medium,
        Convolution1        = VeryHigh,
        Convolution2        = VeryHigh

    }

    public enum ThreadStackSize : uint
    {
        Default             = 0,
        Mixer               = 80  * 1024,
        Feeder              = 16  * 1024,
        Stream              = 96  * 1024,
        File                = 64  * 1024,
        Nonblocking         = 112 * 1024,
        Record              = 16  * 1024,
        Geometry            = 48  * 1024,
        Profiler            = 128 * 1024,
        StudioUpdate       = 96  * 1024,
        StudioLoadBank    = 96  * 1024,
        StudioLoadSample  = 96  * 1024,
        Convolution1        = 16  * 1024,
        Convolution2        = 16  * 1024
    }

    [Flags]
    public enum ThreadAffinity : long
    {
        /* Platform agnostic thread groupings */
        GroupDefault       = 0x4000000000000000,
        GroupA             = 0x4000000000000001,
        GroupB             = 0x4000000000000002,
        GroupC             = 0x4000000000000003,
        
        /* Thread defaults */
        Mixer               = GroupA,
        Feeder              = GroupC,
        Stream              = GroupC,
        File                = GroupC,
        Nonblocking         = GroupC,
        Record              = GroupC,
        Geometry            = GroupC,
        Profiler            = GroupC,
        StudioUpdate       = GroupB,
        StudioLoadBank    = GroupC,
        StudioLoadSample  = GroupC,
        Convolution1        = GroupC,
        Convolution2        = GroupC,
                
        /* Core mask, valid up to 1 << 61 */
        CoreAll            = 0,
        Core0              = 1 << 0,
        Core1              = 1 << 1,
        Core2              = 1 << 2,
        Core3              = 1 << 3,
        Core4              = 1 << 4,
        Core5              = 1 << 5,
        Core6              = 1 << 6,
        Core7              = 1 << 7,
        Core8              = 1 << 8,
        Core9              = 1 << 9,
        Core10             = 1 << 10,
        Core11             = 1 << 11,
        Core12             = 1 << 12,
        Core13             = 1 << 13,
        Core14             = 1 << 14,
        Core15             = 1 << 15
    }

    public enum ThreadType : int
    {
        Mixer,
        Feeder,
        Stream,
        File,
        Nonblocking,
        Record,
        Geometry,
        Profiler,
        StudioUpdate,
        StudioLoadBank,
        StudioLoadSample,
        Convolution1,
        Convolution2,

        Max
    }

    /*
        FMOD System factory functions.  Use this to create an FMOD System Instance.  below you will see System init/close to get started.
    */
    public struct Factory
    {
        public static Result System_Create(out System system)
        {
            return FMOD5_System_Create(out system.Handle, Version.Number);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Create(out IntPtr system, uint headerversion);

        #endregion
    }

    /*
        FMOD global system functions (optional).
    */
    public struct Memory
    {
        public static Result Initialize(IntPtr poolmem, int poollen, MemoryAllocCallback useralloc, MemoryReallocCallback userrealloc, MemoryFreeCallback userfree, MemoryType memtypeflags = MemoryType.All)
        {
            return FMOD5_Memory_Initialize(poolmem, poollen, useralloc, userrealloc, userfree, memtypeflags);
        }

        public static Result GetStats(out int currentalloced, out int maxalloced, bool blocking = true)
        {
            return FMOD5_Memory_GetStats(out currentalloced, out maxalloced, blocking);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Memory_Initialize(IntPtr poolmem, int poollen, MemoryAllocCallback useralloc, MemoryReallocCallback userrealloc, MemoryFreeCallback userfree, MemoryType memtypeflags);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Memory_GetStats  (out int currentalloced, out int maxalloced, bool blocking);

        #endregion
    }

    public struct Debug
    {
        public static Result Initialize(DebugFlags flags, DebugMode mode = DebugMode.Tty, DebugCallback callback = null, string filename = null)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD5_Debug_Initialize(flags, mode, callback, encoder.ByteFromStringUtf8(filename));
            }
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Debug_Initialize(DebugFlags flags, DebugMode mode, DebugCallback callback, byte[] filename);

        #endregion
    }

    public struct Thread
    {
        public static Result SetAttributes(ThreadType type, ThreadAffinity affinity = ThreadAffinity.GroupDefault, ThreadPriority priority = ThreadPriority.Default, ThreadStackSize stacksize = ThreadStackSize.Default)
        {
            return FMOD5_Thread_SetAttributes(type, affinity, priority, stacksize);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Thread_SetAttributes(ThreadType type, ThreadAffinity affinity, ThreadPriority priority, ThreadStackSize stacksize);
        #endregion
    }

    /*
        'System' API.
    */
    public struct System
    {
        public Result Release()
        {
            return FMOD5_System_Release(this.Handle);
        }

        // Setup functions.
        public Result SetOutput(Outputtype output)
        {
            return FMOD5_System_SetOutput(this.Handle, output);
        }
        public Result GetOutput(out Outputtype output)
        {
            return FMOD5_System_GetOutput(this.Handle, out output);
        }
        public Result GetNumDrivers(out int numdrivers)
        {
            return FMOD5_System_GetNumDrivers(this.Handle, out numdrivers);
        }
        public Result GetDriverInfo(int id, out string name, int namelen, out global::System.Guid guid, out int systemrate, out Speakermode speakermode, out int speakermodechannels)
        {
            IntPtr stringMem = Marshal.AllocHGlobal(namelen);

            Result result = FMOD5_System_GetDriverInfo(this.Handle, id, stringMem, namelen, out guid, out systemrate, out speakermode, out speakermodechannels);
            using (StringHelper.ThreadSafeEncoding encoding = StringHelper.GetFreeHelper())
            {
                name = encoding.StringFromNative(stringMem);
            }
            Marshal.FreeHGlobal(stringMem);

            return result;
        }
        public Result GetDriverInfo(int id, out global::System.Guid guid, out int systemrate, out Speakermode speakermode, out int speakermodechannels)
        {
            return FMOD5_System_GetDriverInfo(this.Handle, id, IntPtr.Zero, 0, out guid, out systemrate, out speakermode, out speakermodechannels);
        }
        public Result SetDriver(int driver)
        {
            return FMOD5_System_SetDriver(this.Handle, driver);
        }
        public Result GetDriver(out int driver)
        {
            return FMOD5_System_GetDriver(this.Handle, out driver);
        }
        public Result SetSoftwareChannels(int numsoftwarechannels)
        {
            return FMOD5_System_SetSoftwareChannels(this.Handle, numsoftwarechannels);
        }
        public Result GetSoftwareChannels(out int numsoftwarechannels)
        {
            return FMOD5_System_GetSoftwareChannels(this.Handle, out numsoftwarechannels);
        }
        public Result SetSoftwareFormat(int samplerate, Speakermode speakermode, int numrawspeakers)
        {
            return FMOD5_System_SetSoftwareFormat(this.Handle, samplerate, speakermode, numrawspeakers);
        }
        public Result GetSoftwareFormat(out int samplerate, out Speakermode speakermode, out int numrawspeakers)
        {
            return FMOD5_System_GetSoftwareFormat(this.Handle, out samplerate, out speakermode, out numrawspeakers);
        }
        public Result SetDspBufferSize(uint bufferlength, int numbuffers)
        {
            return FMOD5_System_SetDSPBufferSize(this.Handle, bufferlength, numbuffers);
        }
        public Result GetDspBufferSize(out uint bufferlength, out int numbuffers)
        {
            return FMOD5_System_GetDSPBufferSize(this.Handle, out bufferlength, out numbuffers);
        }
        public Result SetFileSystem(FileOpenCallback useropen, FileCloseCallback userclose, FileReadCallback userread, FileSeekCallback userseek, FileAsyncreadCallback userasyncread, FileAsynccancelCallback userasynccancel, int blockalign)
        {
            return FMOD5_System_SetFileSystem(this.Handle, useropen, userclose, userread, userseek, userasyncread, userasynccancel, blockalign);
        }
        public Result AttachFileSystem(FileOpenCallback useropen, FileCloseCallback userclose, FileReadCallback userread, FileSeekCallback userseek)
        {
            return FMOD5_System_AttachFileSystem(this.Handle, useropen, userclose, userread, userseek);
        }
        public Result SetAdvancedSettings(ref Advancedsettings settings)
        {
            settings.cbSize = MarshalHelper.SizeOf(typeof(Advancedsettings));
            return FMOD5_System_SetAdvancedSettings(this.Handle, ref settings);
        }
        public Result GetAdvancedSettings(ref Advancedsettings settings)
        {
            settings.cbSize = MarshalHelper.SizeOf(typeof(Advancedsettings));
            return FMOD5_System_GetAdvancedSettings(this.Handle, ref settings);
        }
        public Result SetCallback(SystemCallback callback, SystemCallbackType callbackmask = SystemCallbackType.All)
        {
            return FMOD5_System_SetCallback(this.Handle, callback, callbackmask);
        }

        // Plug-in support.
        public Result SetPluginPath(string path)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD5_System_SetPluginPath(this.Handle, encoder.ByteFromStringUtf8(path));
            }
        }
        public Result LoadPlugin(string filename, out uint handle, uint priority = 0)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD5_System_LoadPlugin(this.Handle, encoder.ByteFromStringUtf8(filename), out handle, priority);
            }
        }
        public Result UnloadPlugin(uint handle)
        {
            return FMOD5_System_UnloadPlugin(this.Handle, handle);
        }
        public Result GetNumNestedPlugins(uint handle, out int count)
        {
            return FMOD5_System_GetNumNestedPlugins(this.Handle, handle, out count);
        }
        public Result GetNestedPlugin(uint handle, int index, out uint nestedhandle)
        {
            return FMOD5_System_GetNestedPlugin(this.Handle, handle, index, out nestedhandle);
        }
        public Result GetNumPlugins(Plugintype plugintype, out int numplugins)
        {
            return FMOD5_System_GetNumPlugins(this.Handle, plugintype, out numplugins);
        }
        public Result GetPluginHandle(Plugintype plugintype, int index, out uint handle)
        {
            return FMOD5_System_GetPluginHandle(this.Handle, plugintype, index, out handle);
        }
        public Result GetPluginInfo(uint handle, out Plugintype plugintype, out string name, int namelen, out uint version)
        {
            IntPtr stringMem = Marshal.AllocHGlobal(namelen);

            Result result = FMOD5_System_GetPluginInfo(this.Handle, handle, out plugintype, stringMem, namelen, out version);
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                name = encoder.StringFromNative(stringMem);
            }
            Marshal.FreeHGlobal(stringMem);

            return result;
        }
        public Result GetPluginInfo(uint handle, out Plugintype plugintype, out uint version)
        {
            return FMOD5_System_GetPluginInfo(this.Handle, handle, out plugintype, IntPtr.Zero, 0, out version);
        }
        public Result SetOutputByPlugin(uint handle)
        {
            return FMOD5_System_SetOutputByPlugin(this.Handle, handle);
        }
        public Result GetOutputByPlugin(out uint handle)
        {
            return FMOD5_System_GetOutputByPlugin(this.Handle, out handle);
        }
        public Result CreateDspByPlugin(uint handle, out Dsp dsp)
        {
            return FMOD5_System_CreateDSPByPlugin(this.Handle, handle, out dsp.Handle);
        }
        public Result GetDspInfoByPlugin(uint handle, out IntPtr description)
        {
            return FMOD5_System_GetDSPInfoByPlugin(this.Handle, handle, out description);
        }
        public Result RegisterDsp(ref DspDescription description, out uint handle)
        {
            return FMOD5_System_RegisterDSP(this.Handle, ref description, out handle);
        }

        // Init/Close.
        public Result Init(int maxchannels, Initflags flags, IntPtr extradriverdata)
        {
            return FMOD5_System_Init(this.Handle, maxchannels, flags, extradriverdata);
        }
        public Result Close()
        {
            return FMOD5_System_Close(this.Handle);
        }

        // General post-init system functions.
        public Result Update()
        {
            return FMOD5_System_Update(this.Handle);
        }
        public Result SetSpeakerPosition(Speaker speaker, float x, float y, bool active)
        {
            return FMOD5_System_SetSpeakerPosition(this.Handle, speaker, x, y, active);
        }
        public Result GetSpeakerPosition(Speaker speaker, out float x, out float y, out bool active)
        {
            return FMOD5_System_GetSpeakerPosition(this.Handle, speaker, out x, out y, out active);
        }
        public Result SetStreamBufferSize(uint filebuffersize, Timeunit filebuffersizetype)
        {
            return FMOD5_System_SetStreamBufferSize(this.Handle, filebuffersize, filebuffersizetype);
        }
        public Result GetStreamBufferSize(out uint filebuffersize, out Timeunit filebuffersizetype)
        {
            return FMOD5_System_GetStreamBufferSize(this.Handle, out filebuffersize, out filebuffersizetype);
        }
        public Result Set3DSettings(float dopplerscale, float distancefactor, float rolloffscale)
        {
            return FMOD5_System_Set3DSettings(this.Handle, dopplerscale, distancefactor, rolloffscale);
        }
        public Result Get3DSettings(out float dopplerscale, out float distancefactor, out float rolloffscale)
        {
            return FMOD5_System_Get3DSettings(this.Handle, out dopplerscale, out distancefactor, out rolloffscale);
        }
        public Result Set3DNumListeners(int numlisteners)
        {
            return FMOD5_System_Set3DNumListeners(this.Handle, numlisteners);
        }
        public Result Get3DNumListeners(out int numlisteners)
        {
            return FMOD5_System_Get3DNumListeners(this.Handle, out numlisteners);
        }
        public Result Set3DListenerAttributes(int listener, ref Vector pos, ref Vector vel, ref Vector forward, ref Vector up)
        {
            return FMOD5_System_Set3DListenerAttributes(this.Handle, listener, ref pos, ref vel, ref forward, ref up);
        }
        public Result Get3DListenerAttributes(int listener, out Vector pos, out Vector vel, out Vector forward, out Vector up)
        {
            return FMOD5_System_Get3DListenerAttributes(this.Handle, listener, out pos, out vel, out forward, out up);
        }
        public Result Set3DRolloffCallback(Cb3DRolloffCallback callback)
        {
            return FMOD5_System_Set3DRolloffCallback(this.Handle, callback);
        }
        public Result MixerSuspend()
        {
            return FMOD5_System_MixerSuspend(this.Handle);
        }
        public Result MixerResume()
        {
            return FMOD5_System_MixerResume(this.Handle);
        }
        public Result GetDefaultMixMatrix(Speakermode sourcespeakermode, Speakermode targetspeakermode, float[] matrix, int matrixhop)
        {
            return FMOD5_System_GetDefaultMixMatrix(this.Handle, sourcespeakermode, targetspeakermode, matrix, matrixhop);
        }
        public Result GetSpeakerModeChannels(Speakermode mode, out int channels)
        {
            return FMOD5_System_GetSpeakerModeChannels(this.Handle, mode, out channels);
        }

        // System information functions.
        public Result GetVersion(out uint version)
        {
            return FMOD5_System_GetVersion(this.Handle, out version);
        }
        public Result GetOutputHandle(out IntPtr handle)
        {
            return FMOD5_System_GetOutputHandle(this.Handle, out handle);
        }
        public Result GetChannelsPlaying(out int channels)
        {
            return FMOD5_System_GetChannelsPlaying(this.Handle, out channels, IntPtr.Zero);
        }
        public Result GetChannelsPlaying(out int channels, out int realchannels)
        {
            return FMOD5_System_GetChannelsPlaying(this.Handle, out channels, out realchannels);
        }
        public Result GetCpuUsage(out CpuUsage usage)
        {
            return FMOD5_System_GetCPUUsage(this.Handle, out usage);
        }
        public Result GetFileUsage(out Int64 sampleBytesRead, out Int64 streamBytesRead, out Int64 otherBytesRead)
        {
            return FMOD5_System_GetFileUsage(this.Handle, out sampleBytesRead, out streamBytesRead, out otherBytesRead);
        }

        // Sound/DSP/Channel/FX creation and retrieval.
        public Result CreateSound(string name, Mode mode, ref Createsoundexinfo exinfo, out Sound sound)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                 return FMOD5_System_CreateSound(this.Handle, encoder.ByteFromStringUtf8(name), mode, ref exinfo, out sound.Handle);
            }
        }
        public Result CreateSound(byte[] data, Mode mode, ref Createsoundexinfo exinfo, out Sound sound)
        {
            return FMOD5_System_CreateSound(this.Handle, data, mode, ref exinfo, out sound.Handle);
        }
        public Result CreateSound(IntPtr nameOrData, Mode mode, ref Createsoundexinfo exinfo, out Sound sound)
        {
            return FMOD5_System_CreateSound(this.Handle, nameOrData, mode, ref exinfo, out sound.Handle);
        }
        public Result CreateSound(string name, Mode mode, out Sound sound)
        {
            Createsoundexinfo exinfo = new Createsoundexinfo();
            exinfo.cbsize = MarshalHelper.SizeOf(typeof(Createsoundexinfo));

            return CreateSound(name, mode, ref exinfo, out sound);
        }
        public Result CreateStream(string name, Mode mode, ref Createsoundexinfo exinfo, out Sound sound)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD5_System_CreateStream(this.Handle, encoder.ByteFromStringUtf8(name), mode, ref exinfo, out sound.Handle);
            }
        }
        public Result CreateStream(byte[] data, Mode mode, ref Createsoundexinfo exinfo, out Sound sound)
        {
            return FMOD5_System_CreateStream(this.Handle, data, mode, ref exinfo, out sound.Handle);
        }
        public Result CreateStream(IntPtr nameOrData, Mode mode, ref Createsoundexinfo exinfo, out Sound sound)
        {
            return FMOD5_System_CreateStream(this.Handle, nameOrData, mode, ref exinfo, out sound.Handle);
        }
        public Result CreateStream(string name, Mode mode, out Sound sound)
        {
            Createsoundexinfo exinfo = new Createsoundexinfo();
            exinfo.cbsize = MarshalHelper.SizeOf(typeof(Createsoundexinfo));

            return CreateStream(name, mode, ref exinfo, out sound);
        }
        public Result CreateDsp(ref DspDescription description, out Dsp dsp)
        {
            return FMOD5_System_CreateDSP(this.Handle, ref description, out dsp.Handle);
        }
        public Result CreateDspByType(DspType type, out Dsp dsp)
        {
            return FMOD5_System_CreateDSPByType(this.Handle, type, out dsp.Handle);
        }
        public Result CreateChannelGroup(string name, out ChannelGroup channelgroup)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD5_System_CreateChannelGroup(this.Handle, encoder.ByteFromStringUtf8(name), out channelgroup.Handle);
            }
        }
        public Result CreateSoundGroup(string name, out SoundGroup soundgroup)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD5_System_CreateSoundGroup(this.Handle, encoder.ByteFromStringUtf8(name), out soundgroup.Handle);
            }
        }
        public Result CreateReverb3D(out Reverb3D reverb)
        {
            return FMOD5_System_CreateReverb3D(this.Handle, out reverb.Handle);
        }
        public Result PlaySound(Sound sound, ChannelGroup channelgroup, bool paused, out Channel channel)
        {
            return FMOD5_System_PlaySound(this.Handle, sound.Handle, channelgroup.Handle, paused, out channel.Handle);
        }
        public Result PlayDsp(Dsp dsp, ChannelGroup channelgroup, bool paused, out Channel channel)
        {
            return FMOD5_System_PlayDSP(this.Handle, dsp.Handle, channelgroup.Handle, paused, out channel.Handle);
        }
        public Result GetChannel(int channelid, out Channel channel)
        {
            return FMOD5_System_GetChannel(this.Handle, channelid, out channel.Handle);
        }
        public Result GetDspInfoByType(DspType type, out IntPtr description)
        {
            return FMOD5_System_GetDSPInfoByType(this.Handle, type, out description);
        }
        public Result GetMasterChannelGroup(out ChannelGroup channelgroup)
        {
            return FMOD5_System_GetMasterChannelGroup(this.Handle, out channelgroup.Handle);
        }
        public Result GetMasterSoundGroup(out SoundGroup soundgroup)
        {
            return FMOD5_System_GetMasterSoundGroup(this.Handle, out soundgroup.Handle);
        }

        // Routing to ports.
        public Result AttachChannelGroupToPort(PortType portType, ulong portIndex, ChannelGroup channelgroup, bool passThru = false)
        {
            return FMOD5_System_AttachChannelGroupToPort(this.Handle, portType, portIndex, channelgroup.Handle, passThru);
        }
        public Result DetachChannelGroupFromPort(ChannelGroup channelgroup)
        {
            return FMOD5_System_DetachChannelGroupFromPort(this.Handle, channelgroup.Handle);
        }

        // Reverb api.
        public Result SetReverbProperties(int instance, ref ReverbProperties prop)
        {
            return FMOD5_System_SetReverbProperties(this.Handle, instance, ref prop);
        }
        public Result GetReverbProperties(int instance, out ReverbProperties prop)
        {
            return FMOD5_System_GetReverbProperties(this.Handle, instance, out prop);
        }

        // System level DSP functionality.
        public Result LockDsp()
        {
            return FMOD5_System_LockDSP(this.Handle);
        }
        public Result UnlockDsp()
        {
            return FMOD5_System_UnlockDSP(this.Handle);
        }

        // Recording api
        public Result GetRecordNumDrivers(out int numdrivers, out int numconnected)
        {
            return FMOD5_System_GetRecordNumDrivers(this.Handle, out numdrivers, out numconnected);
        }
        public Result GetRecordDriverInfo(int id, out string name, int namelen, out global::System.Guid guid, out int systemrate, out Speakermode speakermode, out int speakermodechannels, out DriverState state)
        {
            IntPtr stringMem = Marshal.AllocHGlobal(namelen);

            Result result = FMOD5_System_GetRecordDriverInfo(this.Handle, id, stringMem, namelen, out guid, out systemrate, out speakermode, out speakermodechannels, out state);

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                name = encoder.StringFromNative(stringMem);
            }
            Marshal.FreeHGlobal(stringMem);

            return result;
        }
        public Result GetRecordDriverInfo(int id, out global::System.Guid guid, out int systemrate, out Speakermode speakermode, out int speakermodechannels, out DriverState state)
        {
            return FMOD5_System_GetRecordDriverInfo(this.Handle, id, IntPtr.Zero, 0, out guid, out systemrate, out speakermode, out speakermodechannels, out state);
        }
        public Result GetRecordPosition(int id, out uint position)
        {
            return FMOD5_System_GetRecordPosition(this.Handle, id, out position);
        }
        public Result RecordStart(int id, Sound sound, bool loop)
        {
            return FMOD5_System_RecordStart(this.Handle, id, sound.Handle, loop);
        }
        public Result RecordStop(int id)
        {
            return FMOD5_System_RecordStop(this.Handle, id);
        }
        public Result IsRecording(int id, out bool recording)
        {
            return FMOD5_System_IsRecording(this.Handle, id, out recording);
        }

        // Geometry api
        public Result CreateGeometry(int maxpolygons, int maxvertices, out Geometry geometry)
        {
            return FMOD5_System_CreateGeometry(this.Handle, maxpolygons, maxvertices, out geometry.Handle);
        }
        public Result SetGeometrySettings(float maxworldsize)
        {
            return FMOD5_System_SetGeometrySettings(this.Handle, maxworldsize);
        }
        public Result GetGeometrySettings(out float maxworldsize)
        {
            return FMOD5_System_GetGeometrySettings(this.Handle, out maxworldsize);
        }
        public Result LoadGeometry(IntPtr data, int datasize, out Geometry geometry)
        {
            return FMOD5_System_LoadGeometry(this.Handle, data, datasize, out geometry.Handle);
        }
        public Result GetGeometryOcclusion(ref Vector listener, ref Vector source, out float direct, out float reverb)
        {
            return FMOD5_System_GetGeometryOcclusion(this.Handle, ref listener, ref source, out direct, out reverb);
        }

        // Network functions
        public Result SetNetworkProxy(string proxy)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD5_System_SetNetworkProxy(this.Handle, encoder.ByteFromStringUtf8(proxy));
            }
        }
        public Result GetNetworkProxy(out string proxy, int proxylen)
        {
            IntPtr stringMem = Marshal.AllocHGlobal(proxylen);

            Result result = FMOD5_System_GetNetworkProxy(this.Handle, stringMem, proxylen);
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                proxy = encoder.StringFromNative(stringMem);
            }
            Marshal.FreeHGlobal(stringMem);

            return result;
        }
        public Result SetNetworkTimeout(int timeout)
        {
            return FMOD5_System_SetNetworkTimeout(this.Handle, timeout);
        }
        public Result GetNetworkTimeout(out int timeout)
        {
            return FMOD5_System_GetNetworkTimeout(this.Handle, out timeout);
        }

        // Userdata set/get
        public Result SetUserData(IntPtr userdata)
        {
            return FMOD5_System_SetUserData(this.Handle, userdata);
        }
        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD5_System_GetUserData(this.Handle, out userdata);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Release                   (IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetOutput                 (IntPtr system, Outputtype output);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetOutput                 (IntPtr system, out Outputtype output);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetNumDrivers             (IntPtr system, out int numdrivers);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetDriverInfo             (IntPtr system, int id, IntPtr name, int namelen, out global::System.Guid guid, out int systemrate, out Speakermode speakermode, out int speakermodechannels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetDriver                 (IntPtr system, int driver);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetDriver                 (IntPtr system, out int driver);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetSoftwareChannels       (IntPtr system, int numsoftwarechannels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetSoftwareChannels       (IntPtr system, out int numsoftwarechannels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetSoftwareFormat         (IntPtr system, int samplerate, Speakermode speakermode, int numrawspeakers);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetSoftwareFormat         (IntPtr system, out int samplerate, out Speakermode speakermode, out int numrawspeakers);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetDSPBufferSize          (IntPtr system, uint bufferlength, int numbuffers);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetDSPBufferSize          (IntPtr system, out uint bufferlength, out int numbuffers);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetFileSystem             (IntPtr system, FileOpenCallback useropen, FileCloseCallback userclose, FileReadCallback userread, FileSeekCallback userseek, FileAsyncreadCallback userasyncread, FileAsynccancelCallback userasynccancel, int blockalign);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_AttachFileSystem          (IntPtr system, FileOpenCallback useropen, FileCloseCallback userclose, FileReadCallback userread, FileSeekCallback userseek);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetAdvancedSettings       (IntPtr system, ref Advancedsettings settings);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetAdvancedSettings       (IntPtr system, ref Advancedsettings settings);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetCallback               (IntPtr system, SystemCallback callback, SystemCallbackType callbackmask);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetPluginPath             (IntPtr system, byte[] path);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_LoadPlugin                (IntPtr system, byte[] filename, out uint handle, uint priority);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_UnloadPlugin              (IntPtr system, uint handle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetNumNestedPlugins       (IntPtr system, uint handle, out int count);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetNestedPlugin           (IntPtr system, uint handle, int index, out uint nestedhandle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetNumPlugins             (IntPtr system, Plugintype plugintype, out int numplugins);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetPluginHandle           (IntPtr system, Plugintype plugintype, int index, out uint handle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetPluginInfo             (IntPtr system, uint handle, out Plugintype plugintype, IntPtr name, int namelen, out uint version);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetOutputByPlugin         (IntPtr system, uint handle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetOutputByPlugin         (IntPtr system, out uint handle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_CreateDSPByPlugin         (IntPtr system, uint handle, out IntPtr dsp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetDSPInfoByPlugin        (IntPtr system, uint handle, out IntPtr description);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_RegisterDSP               (IntPtr system, ref DspDescription description, out uint handle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Init                      (IntPtr system, int maxchannels, Initflags flags, IntPtr extradriverdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Close                     (IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Update                    (IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetSpeakerPosition        (IntPtr system, Speaker speaker, float x, float y, bool active);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetSpeakerPosition        (IntPtr system, Speaker speaker, out float x, out float y, out bool active);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetStreamBufferSize       (IntPtr system, uint filebuffersize, Timeunit filebuffersizetype);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetStreamBufferSize       (IntPtr system, out uint filebuffersize, out Timeunit filebuffersizetype);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Set3DSettings             (IntPtr system, float dopplerscale, float distancefactor, float rolloffscale);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Get3DSettings             (IntPtr system, out float dopplerscale, out float distancefactor, out float rolloffscale);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Set3DNumListeners         (IntPtr system, int numlisteners);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Get3DNumListeners         (IntPtr system, out int numlisteners);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Set3DListenerAttributes   (IntPtr system, int listener, ref Vector pos, ref Vector vel, ref Vector forward, ref Vector up);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Get3DListenerAttributes   (IntPtr system, int listener, out Vector pos, out Vector vel, out Vector forward, out Vector up);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_Set3DRolloffCallback      (IntPtr system, Cb3DRolloffCallback callback);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_MixerSuspend              (IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_MixerResume               (IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetDefaultMixMatrix       (IntPtr system, Speakermode sourcespeakermode, Speakermode targetspeakermode, float[] matrix, int matrixhop);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetSpeakerModeChannels    (IntPtr system, Speakermode mode, out int channels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetVersion                (IntPtr system, out uint version);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetOutputHandle           (IntPtr system, out IntPtr handle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetChannelsPlaying        (IntPtr system, out int channels, IntPtr zero);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetChannelsPlaying        (IntPtr system, out int channels, out int realchannels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetCPUUsage               (IntPtr system, out CpuUsage usage);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetFileUsage              (IntPtr system, out Int64 sampleBytesRead, out Int64 streamBytesRead, out Int64 otherBytesRead);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_CreateSound               (IntPtr system, byte[] nameOrData, Mode mode, ref Createsoundexinfo exinfo, out IntPtr sound);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_CreateSound               (IntPtr system, IntPtr nameOrData, Mode mode, ref Createsoundexinfo exinfo, out IntPtr sound);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_CreateStream              (IntPtr system, byte[] nameOrData, Mode mode, ref Createsoundexinfo exinfo, out IntPtr sound);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_CreateStream              (IntPtr system, IntPtr nameOrData, Mode mode, ref Createsoundexinfo exinfo, out IntPtr sound);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_CreateDSP                 (IntPtr system, ref DspDescription description, out IntPtr dsp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_CreateDSPByType           (IntPtr system, DspType type, out IntPtr dsp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_CreateChannelGroup        (IntPtr system, byte[] name, out IntPtr channelgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_CreateSoundGroup          (IntPtr system, byte[] name, out IntPtr soundgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_CreateReverb3D            (IntPtr system, out IntPtr reverb);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_PlaySound                 (IntPtr system, IntPtr sound, IntPtr channelgroup, bool paused, out IntPtr channel);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_PlayDSP                   (IntPtr system, IntPtr dsp, IntPtr channelgroup, bool paused, out IntPtr channel);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetChannel                (IntPtr system, int channelid, out IntPtr channel);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetDSPInfoByType          (IntPtr system, DspType type, out IntPtr description);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetMasterChannelGroup     (IntPtr system, out IntPtr channelgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetMasterSoundGroup       (IntPtr system, out IntPtr soundgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_AttachChannelGroupToPort  (IntPtr system, PortType portType, ulong portIndex, IntPtr channelgroup, bool passThru);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_DetachChannelGroupFromPort(IntPtr system, IntPtr channelgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetReverbProperties       (IntPtr system, int instance, ref ReverbProperties prop);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetReverbProperties       (IntPtr system, int instance, out ReverbProperties prop);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_LockDSP                   (IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_UnlockDSP                 (IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetRecordNumDrivers       (IntPtr system, out int numdrivers, out int numconnected);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetRecordDriverInfo       (IntPtr system, int id, IntPtr name, int namelen, out global::System.Guid guid, out int systemrate, out Speakermode speakermode, out int speakermodechannels, out DriverState state);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetRecordPosition         (IntPtr system, int id, out uint position);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_RecordStart               (IntPtr system, int id, IntPtr sound, bool loop);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_RecordStop                (IntPtr system, int id);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_IsRecording               (IntPtr system, int id, out bool recording);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_CreateGeometry            (IntPtr system, int maxpolygons, int maxvertices, out IntPtr geometry);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetGeometrySettings       (IntPtr system, float maxworldsize);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetGeometrySettings       (IntPtr system, out float maxworldsize);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_LoadGeometry              (IntPtr system, IntPtr data, int datasize, out IntPtr geometry);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetGeometryOcclusion      (IntPtr system, ref Vector listener, ref Vector source, out float direct, out float reverb);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetNetworkProxy           (IntPtr system, byte[] proxy);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetNetworkProxy           (IntPtr system, IntPtr proxy, int proxylen);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetNetworkTimeout         (IntPtr system, int timeout);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetNetworkTimeout         (IntPtr system, out int timeout);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_SetUserData               (IntPtr system, IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_System_GetUserData               (IntPtr system, out IntPtr userdata);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public System(IntPtr ptr)   { this.Handle = ptr; }
        public bool HasHandle()     { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()   { this.Handle = IntPtr.Zero; }

        #endregion
    }


    /*
        'Sound' API.
    */
    public struct Sound
    {
        public Result Release()
        {
            return FMOD5_Sound_Release(this.Handle);
        }
        public Result GetSystemObject(out System system)
        {
            return FMOD5_Sound_GetSystemObject(this.Handle, out system.Handle);
        }

        // Standard sound manipulation functions.
        public Result Lock(uint offset, uint length, out IntPtr ptr1, out IntPtr ptr2, out uint len1, out uint len2)
        {
            return FMOD5_Sound_Lock(this.Handle, offset, length, out ptr1, out ptr2, out len1, out len2);
        }
        public Result Unlock(IntPtr ptr1, IntPtr ptr2, uint len1, uint len2)
        {
            return FMOD5_Sound_Unlock(this.Handle, ptr1, ptr2, len1, len2);
        }
        public Result SetDefaults(float frequency, int priority)
        {
            return FMOD5_Sound_SetDefaults(this.Handle, frequency, priority);
        }
        public Result GetDefaults(out float frequency, out int priority)
        {
            return FMOD5_Sound_GetDefaults(this.Handle, out frequency, out priority);
        }
        public Result Set3DMinMaxDistance(float min, float max)
        {
            return FMOD5_Sound_Set3DMinMaxDistance(this.Handle, min, max);
        }
        public Result Get3DMinMaxDistance(out float min, out float max)
        {
            return FMOD5_Sound_Get3DMinMaxDistance(this.Handle, out min, out max);
        }
        public Result Set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume)
        {
            return FMOD5_Sound_Set3DConeSettings(this.Handle, insideconeangle, outsideconeangle, outsidevolume);
        }
        public Result Get3DConeSettings(out float insideconeangle, out float outsideconeangle, out float outsidevolume)
        {
            return FMOD5_Sound_Get3DConeSettings(this.Handle, out insideconeangle, out outsideconeangle, out outsidevolume);
        }
        public Result Set3DCustomRolloff(ref Vector points, int numpoints)
        {
            return FMOD5_Sound_Set3DCustomRolloff(this.Handle, ref points, numpoints);
        }
        public Result Get3DCustomRolloff(out IntPtr points, out int numpoints)
        {
            return FMOD5_Sound_Get3DCustomRolloff(this.Handle, out points, out numpoints);
        }

        public Result GetSubSound(int index, out Sound subsound)
        {
            return FMOD5_Sound_GetSubSound(this.Handle, index, out subsound.Handle);
        }
        public Result GetSubSoundParent(out Sound parentsound)
        {
            return FMOD5_Sound_GetSubSoundParent(this.Handle, out parentsound.Handle);
        }
        public Result GetName(out string name, int namelen)
        {
            IntPtr stringMem = Marshal.AllocHGlobal(namelen);

            Result result = FMOD5_Sound_GetName(this.Handle, stringMem, namelen);
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                name = encoder.StringFromNative(stringMem);
            }
            Marshal.FreeHGlobal(stringMem);

            return result;
        }
        public Result GetLength(out uint length, Timeunit lengthtype)
        {
            return FMOD5_Sound_GetLength(this.Handle, out length, lengthtype);
        }
        public Result GetFormat(out SoundType type, out SoundFormat format, out int channels, out int bits)
        {
            return FMOD5_Sound_GetFormat(this.Handle, out type, out format, out channels, out bits);
        }
        public Result GetNumSubSounds(out int numsubsounds)
        {
            return FMOD5_Sound_GetNumSubSounds(this.Handle, out numsubsounds);
        }
        public Result GetNumTags(out int numtags, out int numtagsupdated)
        {
            return FMOD5_Sound_GetNumTags(this.Handle, out numtags, out numtagsupdated);
        }
        public Result GetTag(string name, int index, out Tag tag)
        {
             using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD5_Sound_GetTag(this.Handle, encoder.ByteFromStringUtf8(name), index, out tag);
            }
        }
        public Result GetOpenState(out Openstate openstate, out uint percentbuffered, out bool starving, out bool diskbusy)
        {
            return FMOD5_Sound_GetOpenState(this.Handle, out openstate, out percentbuffered, out starving, out diskbusy);
        }
        public Result ReadData(IntPtr buffer, uint length, out uint read)
        {
            return FMOD5_Sound_ReadData(this.Handle, buffer, length, out read);
        }
        public Result SeekData(uint pcm)
        {
            return FMOD5_Sound_SeekData(this.Handle, pcm);
        }
        public Result SetSoundGroup(SoundGroup soundgroup)
        {
            return FMOD5_Sound_SetSoundGroup(this.Handle, soundgroup.Handle);
        }
        public Result GetSoundGroup(out SoundGroup soundgroup)
        {
            return FMOD5_Sound_GetSoundGroup(this.Handle, out soundgroup.Handle);
        }

        // Synchronization point API.  These points can come from markers embedded in wav files, and can also generate channel callbacks.
        public Result GetNumSyncPoints(out int numsyncpoints)
        {
            return FMOD5_Sound_GetNumSyncPoints(this.Handle, out numsyncpoints);
        }
        public Result GetSyncPoint(int index, out IntPtr point)
        {
            return FMOD5_Sound_GetSyncPoint(this.Handle, index, out point);
        }
        public Result GetSyncPointInfo(IntPtr point, out string name, int namelen, out uint offset, Timeunit offsettype)
        {
            IntPtr stringMem = Marshal.AllocHGlobal(namelen);

            Result result = FMOD5_Sound_GetSyncPointInfo(this.Handle, point, stringMem, namelen, out offset, offsettype);
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                name = encoder.StringFromNative(stringMem);
            }
            Marshal.FreeHGlobal(stringMem);

            return result;
        }
        public Result GetSyncPointInfo(IntPtr point, out uint offset, Timeunit offsettype)
        {
            return FMOD5_Sound_GetSyncPointInfo(this.Handle, point, IntPtr.Zero, 0, out offset, offsettype);
        }
        public Result AddSyncPoint(uint offset, Timeunit offsettype, string name, out IntPtr point)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD5_Sound_AddSyncPoint(this.Handle, offset, offsettype, encoder.ByteFromStringUtf8(name), out point);
            }
        }
        public Result DeleteSyncPoint(IntPtr point)
        {
            return FMOD5_Sound_DeleteSyncPoint(this.Handle, point);
        }

        // Functions also in Channel class but here they are the 'default' to save having to change it in Channel all the time.
        public Result SetMode(Mode mode)
        {
            return FMOD5_Sound_SetMode(this.Handle, mode);
        }
        public Result GetMode(out Mode mode)
        {
            return FMOD5_Sound_GetMode(this.Handle, out mode);
        }
        public Result SetLoopCount(int loopcount)
        {
            return FMOD5_Sound_SetLoopCount(this.Handle, loopcount);
        }
        public Result GetLoopCount(out int loopcount)
        {
            return FMOD5_Sound_GetLoopCount(this.Handle, out loopcount);
        }
        public Result SetLoopPoints(uint loopstart, Timeunit loopstarttype, uint loopend, Timeunit loopendtype)
        {
            return FMOD5_Sound_SetLoopPoints(this.Handle, loopstart, loopstarttype, loopend, loopendtype);
        }
        public Result GetLoopPoints(out uint loopstart, Timeunit loopstarttype, out uint loopend, Timeunit loopendtype)
        {
            return FMOD5_Sound_GetLoopPoints(this.Handle, out loopstart, loopstarttype, out loopend, loopendtype);
        }

        // For MOD/S3M/XM/IT/MID sequenced formats only.
        public Result GetMusicNumChannels(out int numchannels)
        {
            return FMOD5_Sound_GetMusicNumChannels(this.Handle, out numchannels);
        }
        public Result SetMusicChannelVolume(int channel, float volume)
        {
            return FMOD5_Sound_SetMusicChannelVolume(this.Handle, channel, volume);
        }
        public Result GetMusicChannelVolume(int channel, out float volume)
        {
            return FMOD5_Sound_GetMusicChannelVolume(this.Handle, channel, out volume);
        }
        public Result SetMusicSpeed(float speed)
        {
            return FMOD5_Sound_SetMusicSpeed(this.Handle, speed);
        }
        public Result GetMusicSpeed(out float speed)
        {
            return FMOD5_Sound_GetMusicSpeed(this.Handle, out speed);
        }

        // Userdata set/get.
        public Result SetUserData(IntPtr userdata)
        {
            return FMOD5_Sound_SetUserData(this.Handle, userdata);
        }
        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD5_Sound_GetUserData(this.Handle, out userdata);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_Release                 (IntPtr sound);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetSystemObject         (IntPtr sound, out IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_Lock                    (IntPtr sound, uint offset, uint length, out IntPtr ptr1, out IntPtr ptr2, out uint len1, out uint len2);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_Unlock                  (IntPtr sound, IntPtr ptr1,  IntPtr ptr2, uint len1, uint len2);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_SetDefaults             (IntPtr sound, float frequency, int priority);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetDefaults             (IntPtr sound, out float frequency, out int priority);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_Set3DMinMaxDistance     (IntPtr sound, float min, float max);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_Get3DMinMaxDistance     (IntPtr sound, out float min, out float max);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_Set3DConeSettings       (IntPtr sound, float insideconeangle, float outsideconeangle, float outsidevolume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_Get3DConeSettings       (IntPtr sound, out float insideconeangle, out float outsideconeangle, out float outsidevolume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_Set3DCustomRolloff      (IntPtr sound, ref Vector points, int numpoints);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_Get3DCustomRolloff      (IntPtr sound, out IntPtr points, out int numpoints);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetSubSound             (IntPtr sound, int index, out IntPtr subsound);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetSubSoundParent       (IntPtr sound, out IntPtr parentsound);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetName                 (IntPtr sound, IntPtr name, int namelen);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetLength               (IntPtr sound, out uint length, Timeunit lengthtype);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetFormat               (IntPtr sound, out SoundType type, out SoundFormat format, out int channels, out int bits);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetNumSubSounds         (IntPtr sound, out int numsubsounds);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetNumTags              (IntPtr sound, out int numtags, out int numtagsupdated);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetTag                  (IntPtr sound, byte[] name, int index, out Tag tag);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetOpenState            (IntPtr sound, out Openstate openstate, out uint percentbuffered, out bool starving, out bool diskbusy);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_ReadData                (IntPtr sound, IntPtr buffer, uint length, out uint read);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_SeekData                (IntPtr sound, uint pcm);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_SetSoundGroup           (IntPtr sound, IntPtr soundgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetSoundGroup           (IntPtr sound, out IntPtr soundgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetNumSyncPoints        (IntPtr sound, out int numsyncpoints);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetSyncPoint            (IntPtr sound, int index, out IntPtr point);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetSyncPointInfo        (IntPtr sound, IntPtr point, IntPtr name, int namelen, out uint offset, Timeunit offsettype);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_AddSyncPoint            (IntPtr sound, uint offset, Timeunit offsettype, byte[] name, out IntPtr point);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_DeleteSyncPoint         (IntPtr sound, IntPtr point);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_SetMode                 (IntPtr sound, Mode mode);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetMode                 (IntPtr sound, out Mode mode);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_SetLoopCount            (IntPtr sound, int loopcount);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetLoopCount            (IntPtr sound, out int loopcount);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_SetLoopPoints           (IntPtr sound, uint loopstart, Timeunit loopstarttype, uint loopend, Timeunit loopendtype);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetLoopPoints           (IntPtr sound, out uint loopstart, Timeunit loopstarttype, out uint loopend, Timeunit loopendtype);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetMusicNumChannels     (IntPtr sound, out int numchannels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_SetMusicChannelVolume   (IntPtr sound, int channel, float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetMusicChannelVolume   (IntPtr sound, int channel, out float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_SetMusicSpeed           (IntPtr sound, float speed);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetMusicSpeed           (IntPtr sound, out float speed);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_SetUserData             (IntPtr sound, IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Sound_GetUserData             (IntPtr sound, out IntPtr userdata);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public Sound(IntPtr ptr)    { this.Handle = ptr; }
        public bool HasHandle()     { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()   { this.Handle = IntPtr.Zero; }

        #endregion
    }

    /*
        'ChannelControl' API
    */
    interface IChannelControl
    {
        Result GetSystemObject              (out System system);

        // General control functionality for Channels and ChannelGroups.
        Result Stop                         ();
        Result SetPaused                    (bool paused);
        Result GetPaused                    (out bool paused);
        Result SetVolume                    (float volume);
        Result GetVolume                    (out float volume);
        Result SetVolumeRamp                (bool ramp);
        Result GetVolumeRamp                (out bool ramp);
        Result GetAudibility                (out float audibility);
        Result SetPitch                     (float pitch);
        Result GetPitch                     (out float pitch);
        Result SetMute                      (bool mute);
        Result GetMute                      (out bool mute);
        Result SetReverbProperties          (int instance, float wet);
        Result GetReverbProperties          (int instance, out float wet);
        Result SetLowPassGain               (float gain);
        Result GetLowPassGain               (out float gain);
        Result SetMode                      (Mode mode);
        Result GetMode                      (out Mode mode);
        Result SetCallback                  (ChannelcontrolCallback callback);
        Result IsPlaying                    (out bool isplaying);

        // Note all 'set' functions alter a final matrix, this is why the only get function is getMixMatrix, to avoid other get functions returning incorrect/obsolete values.
        Result SetPan                       (float pan);
        Result SetMixLevelsOutput           (float frontleft, float frontright, float center, float lfe, float surroundleft, float surroundright, float backleft, float backright);
        Result SetMixLevelsInput            (float[] levels, int numlevels);
        Result SetMixMatrix                 (float[] matrix, int outchannels, int inchannels, int inchannelHop);
        Result GetMixMatrix                 (float[] matrix, out int outchannels, out int inchannels, int inchannelHop);

        // Clock based functionality.
        Result GetDspClock                  (out ulong dspclock, out ulong parentclock);
        Result SetDelay                     (ulong dspclockStart, ulong dspclockEnd, bool stopchannels);
        Result GetDelay                     (out ulong dspclockStart, out ulong dspclockEnd);
        Result GetDelay                     (out ulong dspclockStart, out ulong dspclockEnd, out bool stopchannels);
        Result AddFadePoint                 (ulong dspclock, float volume);
        Result SetFadePointRamp             (ulong dspclock, float volume);
        Result RemoveFadePoints             (ulong dspclockStart, ulong dspclockEnd);
        Result GetFadePoints                (ref uint numpoints, ulong[] pointDspclock, float[] pointVolume);

        // DSP effects.
        Result GetDsp                       (int index, out Dsp dsp);
        Result AddDsp                       (int index, Dsp dsp);
        Result RemoveDsp                    (Dsp dsp);
        Result GetNumDsPs                   (out int numdsps);
        Result SetDspIndex                  (Dsp dsp, int index);
        Result GetDspIndex                  (Dsp dsp, out int index);

        // 3D functionality.
        Result Set3DAttributes              (ref Vector pos, ref Vector vel);
        Result Get3DAttributes              (out Vector pos, out Vector vel);
        Result Set3DMinMaxDistance          (float mindistance, float maxdistance);
        Result Get3DMinMaxDistance          (out float mindistance, out float maxdistance);
        Result Set3DConeSettings            (float insideconeangle, float outsideconeangle, float outsidevolume);
        Result Get3DConeSettings            (out float insideconeangle, out float outsideconeangle, out float outsidevolume);
        Result Set3DConeOrientation         (ref Vector orientation);
        Result Get3DConeOrientation         (out Vector orientation);
        Result Set3DCustomRolloff           (ref Vector points, int numpoints);
        Result Get3DCustomRolloff           (out IntPtr points, out int numpoints);
        Result Set3DOcclusion               (float directocclusion, float reverbocclusion);
        Result Get3DOcclusion               (out float directocclusion, out float reverbocclusion);
        Result Set3DSpread                  (float angle);
        Result Get3DSpread                  (out float angle);
        Result Set3DLevel                   (float level);
        Result Get3DLevel                   (out float level);
        Result Set3DDopplerLevel            (float level);
        Result Get3DDopplerLevel            (out float level);
        Result Set3DDistanceFilter          (bool custom, float customLevel, float centerFreq);
        Result Get3DDistanceFilter          (out bool custom, out float customLevel, out float centerFreq);

        // Userdata set/get.
        Result SetUserData                  (IntPtr userdata);
        Result GetUserData                  (out IntPtr userdata);
    }

    /*
        'Channel' API
    */
    public struct Channel : IChannelControl
    {
        // Channel specific control functionality.
        public Result SetFrequency(float frequency)
        {
            return FMOD5_Channel_SetFrequency(this.Handle, frequency);
        }
        public Result GetFrequency(out float frequency)
        {
            return FMOD5_Channel_GetFrequency(this.Handle, out frequency);
        }
        public Result SetPriority(int priority)
        {
            return FMOD5_Channel_SetPriority(this.Handle, priority);
        }
        public Result GetPriority(out int priority)
        {
            return FMOD5_Channel_GetPriority(this.Handle, out priority);
        }
        public Result SetPosition(uint position, Timeunit postype)
        {
            return FMOD5_Channel_SetPosition(this.Handle, position, postype);
        }
        public Result GetPosition(out uint position, Timeunit postype)
        {
            return FMOD5_Channel_GetPosition(this.Handle, out position, postype);
        }
        public Result SetChannelGroup(ChannelGroup channelgroup)
        {
            return FMOD5_Channel_SetChannelGroup(this.Handle, channelgroup.Handle);
        }
        public Result GetChannelGroup(out ChannelGroup channelgroup)
        {
            return FMOD5_Channel_GetChannelGroup(this.Handle, out channelgroup.Handle);
        }
        public Result SetLoopCount(int loopcount)
        {
            return FMOD5_Channel_SetLoopCount(this.Handle, loopcount);
        }
        public Result GetLoopCount(out int loopcount)
        {
            return FMOD5_Channel_GetLoopCount(this.Handle, out loopcount);
        }
        public Result SetLoopPoints(uint loopstart, Timeunit loopstarttype, uint loopend, Timeunit loopendtype)
        {
            return FMOD5_Channel_SetLoopPoints(this.Handle, loopstart, loopstarttype, loopend, loopendtype);
        }
        public Result GetLoopPoints(out uint loopstart, Timeunit loopstarttype, out uint loopend, Timeunit loopendtype)
        {
            return FMOD5_Channel_GetLoopPoints(this.Handle, out loopstart, loopstarttype, out loopend, loopendtype);
        }

        // Information only functions.
        public Result IsVirtual(out bool isvirtual)
        {
            return FMOD5_Channel_IsVirtual(this.Handle, out isvirtual);
        }
        public Result GetCurrentSound(out Sound sound)
        {
            return FMOD5_Channel_GetCurrentSound(this.Handle, out sound.Handle);
        }
        public Result GetIndex(out int index)
        {
            return FMOD5_Channel_GetIndex(this.Handle, out index);
        }

        public Result GetSystemObject(out System system)
        {
            return FMOD5_Channel_GetSystemObject(this.Handle, out system.Handle);
        }

        // General control functionality for Channels and ChannelGroups.
        public Result Stop()
        {
            return FMOD5_Channel_Stop(this.Handle);
        }
        public Result SetPaused(bool paused)
        {
            return FMOD5_Channel_SetPaused(this.Handle, paused);
        }
        public Result GetPaused(out bool paused)
        {
            return FMOD5_Channel_GetPaused(this.Handle, out paused);
        }
        public Result SetVolume(float volume)
        {
            return FMOD5_Channel_SetVolume(this.Handle, volume);
        }
        public Result GetVolume(out float volume)
        {
            return FMOD5_Channel_GetVolume(this.Handle, out volume);
        }
        public Result SetVolumeRamp(bool ramp)
        {
            return FMOD5_Channel_SetVolumeRamp(this.Handle, ramp);
        }
        public Result GetVolumeRamp(out bool ramp)
        {
            return FMOD5_Channel_GetVolumeRamp(this.Handle, out ramp);
        }
        public Result GetAudibility(out float audibility)
        {
            return FMOD5_Channel_GetAudibility(this.Handle, out audibility);
        }
        public Result SetPitch(float pitch)
        {
            return FMOD5_Channel_SetPitch(this.Handle, pitch);
        }
        public Result GetPitch(out float pitch)
        {
            return FMOD5_Channel_GetPitch(this.Handle, out pitch);
        }
        public Result SetMute(bool mute)
        {
            return FMOD5_Channel_SetMute(this.Handle, mute);
        }
        public Result GetMute(out bool mute)
        {
            return FMOD5_Channel_GetMute(this.Handle, out mute);
        }
        public Result SetReverbProperties(int instance, float wet)
        {
            return FMOD5_Channel_SetReverbProperties(this.Handle, instance, wet);
        }
        public Result GetReverbProperties(int instance, out float wet)
        {
            return FMOD5_Channel_GetReverbProperties(this.Handle, instance, out wet);
        }
        public Result SetLowPassGain(float gain)
        {
            return FMOD5_Channel_SetLowPassGain(this.Handle, gain);
        }
        public Result GetLowPassGain(out float gain)
        {
            return FMOD5_Channel_GetLowPassGain(this.Handle, out gain);
        }
        public Result SetMode(Mode mode)
        {
            return FMOD5_Channel_SetMode(this.Handle, mode);
        }
        public Result GetMode(out Mode mode)
        {
            return FMOD5_Channel_GetMode(this.Handle, out mode);
        }
        public Result SetCallback(ChannelcontrolCallback callback)
        {
            return FMOD5_Channel_SetCallback(this.Handle, callback);
        }
        public Result IsPlaying(out bool isplaying)
        {
            return FMOD5_Channel_IsPlaying(this.Handle, out isplaying);
        }

        // Note all 'set' functions alter a final matrix, this is why the only get function is getMixMatrix, to avoid other get functions returning incorrect/obsolete values.
        public Result SetPan(float pan)
        {
            return FMOD5_Channel_SetPan(this.Handle, pan);
        }
        public Result SetMixLevelsOutput(float frontleft, float frontright, float center, float lfe, float surroundleft, float surroundright, float backleft, float backright)
        {
            return FMOD5_Channel_SetMixLevelsOutput(this.Handle, frontleft, frontright, center, lfe, surroundleft, surroundright, backleft, backright);
        }
        public Result SetMixLevelsInput(float[] levels, int numlevels)
        {
            return FMOD5_Channel_SetMixLevelsInput(this.Handle, levels, numlevels);
        }
        public Result SetMixMatrix(float[] matrix, int outchannels, int inchannels, int inchannelHop = 0)
        {
            return FMOD5_Channel_SetMixMatrix(this.Handle, matrix, outchannels, inchannels, inchannelHop);
        }
        public Result GetMixMatrix(float[] matrix, out int outchannels, out int inchannels, int inchannelHop = 0)
        {
            return FMOD5_Channel_GetMixMatrix(this.Handle, matrix, out outchannels, out inchannels, inchannelHop);
        }

        // Clock based functionality.
        public Result GetDspClock(out ulong dspclock, out ulong parentclock)
        {
            return FMOD5_Channel_GetDSPClock(this.Handle, out dspclock, out parentclock);
        }
        public Result SetDelay(ulong dspclockStart, ulong dspclockEnd, bool stopchannels = true)
        {
            return FMOD5_Channel_SetDelay(this.Handle, dspclockStart, dspclockEnd, stopchannels);
        }
        public Result GetDelay(out ulong dspclockStart, out ulong dspclockEnd)
        {
            return FMOD5_Channel_GetDelay(this.Handle, out dspclockStart, out dspclockEnd, IntPtr.Zero);
        }
        public Result GetDelay(out ulong dspclockStart, out ulong dspclockEnd, out bool stopchannels)
        {
            return FMOD5_Channel_GetDelay(this.Handle, out dspclockStart, out dspclockEnd, out stopchannels);
        }
        public Result AddFadePoint(ulong dspclock, float volume)
        {
            return FMOD5_Channel_AddFadePoint(this.Handle, dspclock, volume);
        }
        public Result SetFadePointRamp(ulong dspclock, float volume)
        {
            return FMOD5_Channel_SetFadePointRamp(this.Handle, dspclock, volume);
        }
        public Result RemoveFadePoints(ulong dspclockStart, ulong dspclockEnd)
        {
            return FMOD5_Channel_RemoveFadePoints(this.Handle, dspclockStart, dspclockEnd);
        }
        public Result GetFadePoints(ref uint numpoints, ulong[] pointDspclock, float[] pointVolume)
        {
            return FMOD5_Channel_GetFadePoints(this.Handle, ref numpoints, pointDspclock, pointVolume);
        }

        // DSP effects.
        public Result GetDsp(int index, out Dsp dsp)
        {
            return FMOD5_Channel_GetDSP(this.Handle, index, out dsp.Handle);
        }
        public Result AddDsp(int index, Dsp dsp)
        {
            return FMOD5_Channel_AddDSP(this.Handle, index, dsp.Handle);
        }
        public Result RemoveDsp(Dsp dsp)
        {
            return FMOD5_Channel_RemoveDSP(this.Handle, dsp.Handle);
        }
        public Result GetNumDsPs(out int numdsps)
        {
            return FMOD5_Channel_GetNumDSPs(this.Handle, out numdsps);
        }
        public Result SetDspIndex(Dsp dsp, int index)
        {
            return FMOD5_Channel_SetDSPIndex(this.Handle, dsp.Handle, index);
        }
        public Result GetDspIndex(Dsp dsp, out int index)
        {
            return FMOD5_Channel_GetDSPIndex(this.Handle, dsp.Handle, out index);
        }

        // 3D functionality.
        public Result Set3DAttributes(ref Vector pos, ref Vector vel)
        {
            return FMOD5_Channel_Set3DAttributes(this.Handle, ref pos, ref vel);
        }
        public Result Get3DAttributes(out Vector pos, out Vector vel)
        {
            return FMOD5_Channel_Get3DAttributes(this.Handle, out pos, out vel);
        }
        public Result Set3DMinMaxDistance(float mindistance, float maxdistance)
        {
            return FMOD5_Channel_Set3DMinMaxDistance(this.Handle, mindistance, maxdistance);
        }
        public Result Get3DMinMaxDistance(out float mindistance, out float maxdistance)
        {
            return FMOD5_Channel_Get3DMinMaxDistance(this.Handle, out mindistance, out maxdistance);
        }
        public Result Set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume)
        {
            return FMOD5_Channel_Set3DConeSettings(this.Handle, insideconeangle, outsideconeangle, outsidevolume);
        }
        public Result Get3DConeSettings(out float insideconeangle, out float outsideconeangle, out float outsidevolume)
        {
            return FMOD5_Channel_Get3DConeSettings(this.Handle, out insideconeangle, out outsideconeangle, out outsidevolume);
        }
        public Result Set3DConeOrientation(ref Vector orientation)
        {
            return FMOD5_Channel_Set3DConeOrientation(this.Handle, ref orientation);
        }
        public Result Get3DConeOrientation(out Vector orientation)
        {
            return FMOD5_Channel_Get3DConeOrientation(this.Handle, out orientation);
        }
        public Result Set3DCustomRolloff(ref Vector points, int numpoints)
        {
            return FMOD5_Channel_Set3DCustomRolloff(this.Handle, ref points, numpoints);
        }
        public Result Get3DCustomRolloff(out IntPtr points, out int numpoints)
        {
            return FMOD5_Channel_Get3DCustomRolloff(this.Handle, out points, out numpoints);
        }
        public Result Set3DOcclusion(float directocclusion, float reverbocclusion)
        {
            return FMOD5_Channel_Set3DOcclusion(this.Handle, directocclusion, reverbocclusion);
        }
        public Result Get3DOcclusion(out float directocclusion, out float reverbocclusion)
        {
            return FMOD5_Channel_Get3DOcclusion(this.Handle, out directocclusion, out reverbocclusion);
        }
        public Result Set3DSpread(float angle)
        {
            return FMOD5_Channel_Set3DSpread(this.Handle, angle);
        }
        public Result Get3DSpread(out float angle)
        {
            return FMOD5_Channel_Get3DSpread(this.Handle, out angle);
        }
        public Result Set3DLevel(float level)
        {
            return FMOD5_Channel_Set3DLevel(this.Handle, level);
        }
        public Result Get3DLevel(out float level)
        {
            return FMOD5_Channel_Get3DLevel(this.Handle, out level);
        }
        public Result Set3DDopplerLevel(float level)
        {
            return FMOD5_Channel_Set3DDopplerLevel(this.Handle, level);
        }
        public Result Get3DDopplerLevel(out float level)
        {
            return FMOD5_Channel_Get3DDopplerLevel(this.Handle, out level);
        }
        public Result Set3DDistanceFilter(bool custom, float customLevel, float centerFreq)
        {
            return FMOD5_Channel_Set3DDistanceFilter(this.Handle, custom, customLevel, centerFreq);
        }
        public Result Get3DDistanceFilter(out bool custom, out float customLevel, out float centerFreq)
        {
            return FMOD5_Channel_Get3DDistanceFilter(this.Handle, out custom, out customLevel, out centerFreq);
        }

        // Userdata set/get.
        public Result SetUserData(IntPtr userdata)
        {
            return FMOD5_Channel_SetUserData(this.Handle, userdata);
        }
        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD5_Channel_GetUserData(this.Handle, out userdata);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetFrequency         (IntPtr channel, float frequency);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetFrequency         (IntPtr channel, out float frequency);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetPriority          (IntPtr channel, int priority);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetPriority          (IntPtr channel, out int priority);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetPosition          (IntPtr channel, uint position, Timeunit postype);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetPosition          (IntPtr channel, out uint position, Timeunit postype);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetChannelGroup      (IntPtr channel, IntPtr channelgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetChannelGroup      (IntPtr channel, out IntPtr channelgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetLoopCount         (IntPtr channel, int loopcount);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetLoopCount         (IntPtr channel, out int loopcount);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetLoopPoints        (IntPtr channel, uint  loopstart, Timeunit loopstarttype, uint  loopend, Timeunit loopendtype);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetLoopPoints        (IntPtr channel, out uint loopstart, Timeunit loopstarttype, out uint loopend, Timeunit loopendtype);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_IsVirtual            (IntPtr channel, out bool isvirtual);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetCurrentSound      (IntPtr channel, out IntPtr sound);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetIndex             (IntPtr channel, out int index);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetSystemObject      (IntPtr channel, out IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Stop                 (IntPtr channel);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetPaused            (IntPtr channel, bool paused);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetPaused            (IntPtr channel, out bool paused);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetVolume            (IntPtr channel, float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetVolume            (IntPtr channel, out float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetVolumeRamp        (IntPtr channel, bool ramp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetVolumeRamp        (IntPtr channel, out bool ramp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetAudibility        (IntPtr channel, out float audibility);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetPitch             (IntPtr channel, float pitch);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetPitch             (IntPtr channel, out float pitch);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetMute              (IntPtr channel, bool mute);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetMute              (IntPtr channel, out bool mute);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetReverbProperties  (IntPtr channel, int instance, float wet);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetReverbProperties  (IntPtr channel, int instance, out float wet);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetLowPassGain       (IntPtr channel, float gain);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetLowPassGain       (IntPtr channel, out float gain);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetMode              (IntPtr channel, Mode mode);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetMode              (IntPtr channel, out Mode mode);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetCallback          (IntPtr channel, ChannelcontrolCallback callback);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_IsPlaying            (IntPtr channel, out bool isplaying);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetPan               (IntPtr channel, float pan);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetMixLevelsOutput   (IntPtr channel, float frontleft, float frontright, float center, float lfe, float surroundleft, float surroundright, float backleft, float backright);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetMixLevelsInput    (IntPtr channel, float[] levels, int numlevels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetMixMatrix         (IntPtr channel, float[] matrix, int outchannels, int inchannels, int inchannelHop);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetMixMatrix         (IntPtr channel, float[] matrix, out int outchannels, out int inchannels, int inchannelHop);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetDSPClock          (IntPtr channel, out ulong dspclock, out ulong parentclock);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetDelay             (IntPtr channel, ulong dspclockStart, ulong dspclockEnd, bool stopchannels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetDelay             (IntPtr channel, out ulong dspclockStart, out ulong dspclockEnd, IntPtr zero);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetDelay             (IntPtr channel, out ulong dspclockStart, out ulong dspclockEnd, out bool stopchannels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_AddFadePoint         (IntPtr channel, ulong dspclock, float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetFadePointRamp     (IntPtr channel, ulong dspclock, float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_RemoveFadePoints     (IntPtr channel, ulong dspclockStart, ulong dspclockEnd);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetFadePoints        (IntPtr channel, ref uint numpoints, ulong[] pointDspclock, float[] pointVolume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetDSP               (IntPtr channel, int index, out IntPtr dsp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_AddDSP               (IntPtr channel, int index, IntPtr dsp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_RemoveDSP            (IntPtr channel, IntPtr dsp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetNumDSPs           (IntPtr channel, out int numdsps);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetDSPIndex          (IntPtr channel, IntPtr dsp, int index);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetDSPIndex          (IntPtr channel, IntPtr dsp, out int index);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Set3DAttributes      (IntPtr channel, ref Vector pos, ref Vector vel);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Get3DAttributes      (IntPtr channel, out Vector pos, out Vector vel);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Set3DMinMaxDistance  (IntPtr channel, float mindistance, float maxdistance);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Get3DMinMaxDistance  (IntPtr channel, out float mindistance, out float maxdistance);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Set3DConeSettings    (IntPtr channel, float insideconeangle, float outsideconeangle, float outsidevolume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Get3DConeSettings    (IntPtr channel, out float insideconeangle, out float outsideconeangle, out float outsidevolume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Set3DConeOrientation (IntPtr channel, ref Vector orientation);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Get3DConeOrientation (IntPtr channel, out Vector orientation);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Set3DCustomRolloff   (IntPtr channel, ref Vector points, int numpoints);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Get3DCustomRolloff   (IntPtr channel, out IntPtr points, out int numpoints);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Set3DOcclusion       (IntPtr channel, float directocclusion, float reverbocclusion);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Get3DOcclusion       (IntPtr channel, out float directocclusion, out float reverbocclusion);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Set3DSpread          (IntPtr channel, float angle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Get3DSpread          (IntPtr channel, out float angle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Set3DLevel           (IntPtr channel, float level);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Get3DLevel           (IntPtr channel, out float level);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Set3DDopplerLevel    (IntPtr channel, float level);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Get3DDopplerLevel    (IntPtr channel, out float level);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Set3DDistanceFilter  (IntPtr channel, bool custom, float customLevel, float centerFreq);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_Get3DDistanceFilter  (IntPtr channel, out bool custom, out float customLevel, out float centerFreq);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_SetUserData          (IntPtr channel, IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Channel_GetUserData          (IntPtr channel, out IntPtr userdata);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public Channel(IntPtr ptr)  { this.Handle = ptr; }
        public bool HasHandle()     { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()   { this.Handle = IntPtr.Zero; }

        #endregion
    }

    /*
        'ChannelGroup' API
    */
    public struct ChannelGroup : IChannelControl
    {
        public Result Release()
        {
            return FMOD5_ChannelGroup_Release(this.Handle);
        }

        // Nested channel groups.
        public Result AddGroup(ChannelGroup group, bool propagatedspclock = true)
        {
            return FMOD5_ChannelGroup_AddGroup(this.Handle, group.Handle, propagatedspclock, IntPtr.Zero);
        }
        public Result AddGroup(ChannelGroup group, bool propagatedspclock, out DspConnection connection)
        {
            return FMOD5_ChannelGroup_AddGroup(this.Handle, group.Handle, propagatedspclock, out connection.Handle);
        }
        public Result GetNumGroups(out int numgroups)
        {
            return FMOD5_ChannelGroup_GetNumGroups(this.Handle, out numgroups);
        }
        public Result GetGroup(int index, out ChannelGroup group)
        {
            return FMOD5_ChannelGroup_GetGroup(this.Handle, index, out group.Handle);
        }
        public Result GetParentGroup(out ChannelGroup group)
        {
            return FMOD5_ChannelGroup_GetParentGroup(this.Handle, out group.Handle);
        }

        // Information only functions.
        public Result GetName(out string name, int namelen)
        {
            IntPtr stringMem = Marshal.AllocHGlobal(namelen);

            Result result = FMOD5_ChannelGroup_GetName(this.Handle, stringMem, namelen);
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                name = encoder.StringFromNative(stringMem);
            }
            Marshal.FreeHGlobal(stringMem);

            return result;
        }
        public Result GetNumChannels(out int numchannels)
        {
            return FMOD5_ChannelGroup_GetNumChannels(this.Handle, out numchannels);
        }
        public Result GetChannel(int index, out Channel channel)
        {
            return FMOD5_ChannelGroup_GetChannel(this.Handle, index, out channel.Handle);
        }

        public Result GetSystemObject(out System system)
        {
            return FMOD5_ChannelGroup_GetSystemObject(this.Handle, out system.Handle);
        }

        // General control functionality for Channels and ChannelGroups.
        public Result Stop()
        {
            return FMOD5_ChannelGroup_Stop(this.Handle);
        }
        public Result SetPaused(bool paused)
        {
            return FMOD5_ChannelGroup_SetPaused(this.Handle, paused);
        }
        public Result GetPaused(out bool paused)
        {
            return FMOD5_ChannelGroup_GetPaused(this.Handle, out paused);
        }
        public Result SetVolume(float volume)
        {
            return FMOD5_ChannelGroup_SetVolume(this.Handle, volume);
        }
        public Result GetVolume(out float volume)
        {
            return FMOD5_ChannelGroup_GetVolume(this.Handle, out volume);
        }
        public Result SetVolumeRamp(bool ramp)
        {
            return FMOD5_ChannelGroup_SetVolumeRamp(this.Handle, ramp);
        }
        public Result GetVolumeRamp(out bool ramp)
        {
            return FMOD5_ChannelGroup_GetVolumeRamp(this.Handle, out ramp);
        }
        public Result GetAudibility(out float audibility)
        {
            return FMOD5_ChannelGroup_GetAudibility(this.Handle, out audibility);
        }
        public Result SetPitch(float pitch)
        {
            return FMOD5_ChannelGroup_SetPitch(this.Handle, pitch);
        }
        public Result GetPitch(out float pitch)
        {
            return FMOD5_ChannelGroup_GetPitch(this.Handle, out pitch);
        }
        public Result SetMute(bool mute)
        {
            return FMOD5_ChannelGroup_SetMute(this.Handle, mute);
        }
        public Result GetMute(out bool mute)
        {
            return FMOD5_ChannelGroup_GetMute(this.Handle, out mute);
        }
        public Result SetReverbProperties(int instance, float wet)
        {
            return FMOD5_ChannelGroup_SetReverbProperties(this.Handle, instance, wet);
        }
        public Result GetReverbProperties(int instance, out float wet)
        {
            return FMOD5_ChannelGroup_GetReverbProperties(this.Handle, instance, out wet);
        }
        public Result SetLowPassGain(float gain)
        {
            return FMOD5_ChannelGroup_SetLowPassGain(this.Handle, gain);
        }
        public Result GetLowPassGain(out float gain)
        {
            return FMOD5_ChannelGroup_GetLowPassGain(this.Handle, out gain);
        }
        public Result SetMode(Mode mode)
        {
            return FMOD5_ChannelGroup_SetMode(this.Handle, mode);
        }
        public Result GetMode(out Mode mode)
        {
            return FMOD5_ChannelGroup_GetMode(this.Handle, out mode);
        }
        public Result SetCallback(ChannelcontrolCallback callback)
        {
            return FMOD5_ChannelGroup_SetCallback(this.Handle, callback);
        }
        public Result IsPlaying(out bool isplaying)
        {
            return FMOD5_ChannelGroup_IsPlaying(this.Handle, out isplaying);
        }

        // Note all 'set' functions alter a final matrix, this is why the only get function is getMixMatrix, to avoid other get functions returning incorrect/obsolete values.
        public Result SetPan(float pan)
        {
            return FMOD5_ChannelGroup_SetPan(this.Handle, pan);
        }
        public Result SetMixLevelsOutput(float frontleft, float frontright, float center, float lfe, float surroundleft, float surroundright, float backleft, float backright)
        {
            return FMOD5_ChannelGroup_SetMixLevelsOutput(this.Handle, frontleft, frontright, center, lfe, surroundleft, surroundright, backleft, backright);
        }
        public Result SetMixLevelsInput(float[] levels, int numlevels)
        {
            return FMOD5_ChannelGroup_SetMixLevelsInput(this.Handle, levels, numlevels);
        }
        public Result SetMixMatrix(float[] matrix, int outchannels, int inchannels, int inchannelHop)
        {
            return FMOD5_ChannelGroup_SetMixMatrix(this.Handle, matrix, outchannels, inchannels, inchannelHop);
        }
        public Result GetMixMatrix(float[] matrix, out int outchannels, out int inchannels, int inchannelHop)
        {
            return FMOD5_ChannelGroup_GetMixMatrix(this.Handle, matrix, out outchannels, out inchannels, inchannelHop);
        }

        // Clock based functionality.
        public Result GetDspClock(out ulong dspclock, out ulong parentclock)
        {
            return FMOD5_ChannelGroup_GetDSPClock(this.Handle, out dspclock, out parentclock);
        }
        public Result SetDelay(ulong dspclockStart, ulong dspclockEnd, bool stopchannels)
        {
            return FMOD5_ChannelGroup_SetDelay(this.Handle, dspclockStart, dspclockEnd, stopchannels);
        }
        public Result GetDelay(out ulong dspclockStart, out ulong dspclockEnd)
        {
            return FMOD5_ChannelGroup_GetDelay(this.Handle, out dspclockStart, out dspclockEnd, IntPtr.Zero);
        }
        public Result GetDelay(out ulong dspclockStart, out ulong dspclockEnd, out bool stopchannels)
        {
            return FMOD5_ChannelGroup_GetDelay(this.Handle, out dspclockStart, out dspclockEnd, out stopchannels);
        }
        public Result AddFadePoint(ulong dspclock, float volume)
        {
            return FMOD5_ChannelGroup_AddFadePoint(this.Handle, dspclock, volume);
        }
        public Result SetFadePointRamp(ulong dspclock, float volume)
        {
            return FMOD5_ChannelGroup_SetFadePointRamp(this.Handle, dspclock, volume);
        }
        public Result RemoveFadePoints(ulong dspclockStart, ulong dspclockEnd)
        {
            return FMOD5_ChannelGroup_RemoveFadePoints(this.Handle, dspclockStart, dspclockEnd);
        }
        public Result GetFadePoints(ref uint numpoints, ulong[] pointDspclock, float[] pointVolume)
        {
            return FMOD5_ChannelGroup_GetFadePoints(this.Handle, ref numpoints, pointDspclock, pointVolume);
        }

        // DSP effects.
        public Result GetDsp(int index, out Dsp dsp)
        {
            return FMOD5_ChannelGroup_GetDSP(this.Handle, index, out dsp.Handle);
        }
        public Result AddDsp(int index, Dsp dsp)
        {
            return FMOD5_ChannelGroup_AddDSP(this.Handle, index, dsp.Handle);
        }
        public Result RemoveDsp(Dsp dsp)
        {
            return FMOD5_ChannelGroup_RemoveDSP(this.Handle, dsp.Handle);
        }
        public Result GetNumDsPs(out int numdsps)
        {
            return FMOD5_ChannelGroup_GetNumDSPs(this.Handle, out numdsps);
        }
        public Result SetDspIndex(Dsp dsp, int index)
        {
            return FMOD5_ChannelGroup_SetDSPIndex(this.Handle, dsp.Handle, index);
        }
        public Result GetDspIndex(Dsp dsp, out int index)
        {
            return FMOD5_ChannelGroup_GetDSPIndex(this.Handle, dsp.Handle, out index);
        }

        // 3D functionality.
        public Result Set3DAttributes(ref Vector pos, ref Vector vel)
        {
            return FMOD5_ChannelGroup_Set3DAttributes(this.Handle, ref pos, ref vel);
        }
        public Result Get3DAttributes(out Vector pos, out Vector vel)
        {
            return FMOD5_ChannelGroup_Get3DAttributes(this.Handle, out pos, out vel);
        }
        public Result Set3DMinMaxDistance(float mindistance, float maxdistance)
        {
            return FMOD5_ChannelGroup_Set3DMinMaxDistance(this.Handle, mindistance, maxdistance);
        }
        public Result Get3DMinMaxDistance(out float mindistance, out float maxdistance)
        {
            return FMOD5_ChannelGroup_Get3DMinMaxDistance(this.Handle, out mindistance, out maxdistance);
        }
        public Result Set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume)
        {
            return FMOD5_ChannelGroup_Set3DConeSettings(this.Handle, insideconeangle, outsideconeangle, outsidevolume);
        }
        public Result Get3DConeSettings(out float insideconeangle, out float outsideconeangle, out float outsidevolume)
        {
            return FMOD5_ChannelGroup_Get3DConeSettings(this.Handle, out insideconeangle, out outsideconeangle, out outsidevolume);
        }
        public Result Set3DConeOrientation(ref Vector orientation)
        {
            return FMOD5_ChannelGroup_Set3DConeOrientation(this.Handle, ref orientation);
        }
        public Result Get3DConeOrientation(out Vector orientation)
        {
            return FMOD5_ChannelGroup_Get3DConeOrientation(this.Handle, out orientation);
        }
        public Result Set3DCustomRolloff(ref Vector points, int numpoints)
        {
            return FMOD5_ChannelGroup_Set3DCustomRolloff(this.Handle, ref points, numpoints);
        }
        public Result Get3DCustomRolloff(out IntPtr points, out int numpoints)
        {
            return FMOD5_ChannelGroup_Get3DCustomRolloff(this.Handle, out points, out numpoints);
        }
        public Result Set3DOcclusion(float directocclusion, float reverbocclusion)
        {
            return FMOD5_ChannelGroup_Set3DOcclusion(this.Handle, directocclusion, reverbocclusion);
        }
        public Result Get3DOcclusion(out float directocclusion, out float reverbocclusion)
        {
            return FMOD5_ChannelGroup_Get3DOcclusion(this.Handle, out directocclusion, out reverbocclusion);
        }
        public Result Set3DSpread(float angle)
        {
            return FMOD5_ChannelGroup_Set3DSpread(this.Handle, angle);
        }
        public Result Get3DSpread(out float angle)
        {
            return FMOD5_ChannelGroup_Get3DSpread(this.Handle, out angle);
        }
        public Result Set3DLevel(float level)
        {
            return FMOD5_ChannelGroup_Set3DLevel(this.Handle, level);
        }
        public Result Get3DLevel(out float level)
        {
            return FMOD5_ChannelGroup_Get3DLevel(this.Handle, out level);
        }
        public Result Set3DDopplerLevel(float level)
        {
            return FMOD5_ChannelGroup_Set3DDopplerLevel(this.Handle, level);
        }
        public Result Get3DDopplerLevel(out float level)
        {
            return FMOD5_ChannelGroup_Get3DDopplerLevel(this.Handle, out level);
        }
        public Result Set3DDistanceFilter(bool custom, float customLevel, float centerFreq)
        {
            return FMOD5_ChannelGroup_Set3DDistanceFilter(this.Handle, custom, customLevel, centerFreq);
        }
        public Result Get3DDistanceFilter(out bool custom, out float customLevel, out float centerFreq)
        {
            return FMOD5_ChannelGroup_Get3DDistanceFilter(this.Handle, out custom, out customLevel, out centerFreq);
        }

        // Userdata set/get.
        public Result SetUserData(IntPtr userdata)
        {
            return FMOD5_ChannelGroup_SetUserData(this.Handle, userdata);
        }
        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD5_ChannelGroup_GetUserData(this.Handle, out userdata);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Release             (IntPtr channelgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_AddGroup            (IntPtr channelgroup, IntPtr group, bool propagatedspclock, IntPtr zero);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_AddGroup            (IntPtr channelgroup, IntPtr group, bool propagatedspclock, out IntPtr connection);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetNumGroups        (IntPtr channelgroup, out int numgroups);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetGroup            (IntPtr channelgroup, int index, out IntPtr group);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetParentGroup      (IntPtr channelgroup, out IntPtr group);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetName             (IntPtr channelgroup, IntPtr name, int namelen);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetNumChannels      (IntPtr channelgroup, out int numchannels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetChannel          (IntPtr channelgroup, int index, out IntPtr channel);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetSystemObject     (IntPtr channelgroup, out IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Stop                (IntPtr channelgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetPaused           (IntPtr channelgroup, bool paused);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetPaused           (IntPtr channelgroup, out bool paused);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetVolume           (IntPtr channelgroup, float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetVolume           (IntPtr channelgroup, out float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetVolumeRamp       (IntPtr channelgroup, bool ramp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetVolumeRamp       (IntPtr channelgroup, out bool ramp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetAudibility       (IntPtr channelgroup, out float audibility);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetPitch            (IntPtr channelgroup, float pitch);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetPitch            (IntPtr channelgroup, out float pitch);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetMute             (IntPtr channelgroup, bool mute);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetMute             (IntPtr channelgroup, out bool mute);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetReverbProperties (IntPtr channelgroup, int instance, float wet);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetReverbProperties (IntPtr channelgroup, int instance, out float wet);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetLowPassGain      (IntPtr channelgroup, float gain);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetLowPassGain      (IntPtr channelgroup, out float gain);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetMode             (IntPtr channelgroup, Mode mode);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetMode             (IntPtr channelgroup, out Mode mode);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetCallback         (IntPtr channelgroup, ChannelcontrolCallback callback);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_IsPlaying           (IntPtr channelgroup, out bool isplaying);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetPan              (IntPtr channelgroup, float pan);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetMixLevelsOutput  (IntPtr channelgroup, float frontleft, float frontright, float center, float lfe, float surroundleft, float surroundright, float backleft, float backright);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetMixLevelsInput   (IntPtr channelgroup, float[] levels, int numlevels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetMixMatrix        (IntPtr channelgroup, float[] matrix, int outchannels, int inchannels, int inchannelHop);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetMixMatrix        (IntPtr channelgroup, float[] matrix, out int outchannels, out int inchannels, int inchannelHop);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetDSPClock         (IntPtr channelgroup, out ulong dspclock, out ulong parentclock);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetDelay            (IntPtr channelgroup, ulong dspclockStart, ulong dspclockEnd, bool stopchannels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetDelay            (IntPtr channelgroup, out ulong dspclockStart, out ulong dspclockEnd, IntPtr zero);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetDelay            (IntPtr channelgroup, out ulong dspclockStart, out ulong dspclockEnd, out bool stopchannels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_AddFadePoint        (IntPtr channelgroup, ulong dspclock, float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetFadePointRamp    (IntPtr channelgroup, ulong dspclock, float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_RemoveFadePoints    (IntPtr channelgroup, ulong dspclockStart, ulong dspclockEnd);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetFadePoints       (IntPtr channelgroup, ref uint numpoints, ulong[] pointDspclock, float[] pointVolume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetDSP              (IntPtr channelgroup, int index, out IntPtr dsp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_AddDSP              (IntPtr channelgroup, int index, IntPtr dsp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_RemoveDSP           (IntPtr channelgroup, IntPtr dsp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetNumDSPs          (IntPtr channelgroup, out int numdsps);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetDSPIndex         (IntPtr channelgroup, IntPtr dsp, int index);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetDSPIndex         (IntPtr channelgroup, IntPtr dsp, out int index);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Set3DAttributes     (IntPtr channelgroup, ref Vector pos, ref Vector vel);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Get3DAttributes     (IntPtr channelgroup, out Vector pos, out Vector vel);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Set3DMinMaxDistance (IntPtr channelgroup, float mindistance, float maxdistance);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Get3DMinMaxDistance (IntPtr channelgroup, out float mindistance, out float maxdistance);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Set3DConeSettings   (IntPtr channelgroup, float insideconeangle, float outsideconeangle, float outsidevolume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Get3DConeSettings   (IntPtr channelgroup, out float insideconeangle, out float outsideconeangle, out float outsidevolume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Set3DConeOrientation(IntPtr channelgroup, ref Vector orientation);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Get3DConeOrientation(IntPtr channelgroup, out Vector orientation);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Set3DCustomRolloff  (IntPtr channelgroup, ref Vector points, int numpoints);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Get3DCustomRolloff  (IntPtr channelgroup, out IntPtr points, out int numpoints);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Set3DOcclusion      (IntPtr channelgroup, float directocclusion, float reverbocclusion);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Get3DOcclusion      (IntPtr channelgroup, out float directocclusion, out float reverbocclusion);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Set3DSpread         (IntPtr channelgroup, float angle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Get3DSpread         (IntPtr channelgroup, out float angle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Set3DLevel          (IntPtr channelgroup, float level);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Get3DLevel          (IntPtr channelgroup, out float level);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Set3DDopplerLevel   (IntPtr channelgroup, float level);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Get3DDopplerLevel   (IntPtr channelgroup, out float level);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Set3DDistanceFilter (IntPtr channelgroup, bool custom, float customLevel, float centerFreq);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_Get3DDistanceFilter (IntPtr channelgroup, out bool custom, out float customLevel, out float centerFreq);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_SetUserData         (IntPtr channelgroup, IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_ChannelGroup_GetUserData         (IntPtr channelgroup, out IntPtr userdata);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public ChannelGroup(IntPtr ptr) { this.Handle = ptr; }
        public bool HasHandle()         { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()       { this.Handle = IntPtr.Zero; }

        #endregion
    }

    /*
        'SoundGroup' API
    */
    public struct SoundGroup
    {
        public Result Release()
        {
            return FMOD5_SoundGroup_Release(this.Handle);
        }

        public Result GetSystemObject(out System system)
        {
            return FMOD5_SoundGroup_GetSystemObject(this.Handle, out system.Handle);
        }

        // SoundGroup control functions.
        public Result SetMaxAudible(int maxaudible)
        {
            return FMOD5_SoundGroup_SetMaxAudible(this.Handle, maxaudible);
        }
        public Result GetMaxAudible(out int maxaudible)
        {
            return FMOD5_SoundGroup_GetMaxAudible(this.Handle, out maxaudible);
        }
        public Result SetMaxAudibleBehavior(SoundgroupBehavior behavior)
        {
            return FMOD5_SoundGroup_SetMaxAudibleBehavior(this.Handle, behavior);
        }
        public Result GetMaxAudibleBehavior(out SoundgroupBehavior behavior)
        {
            return FMOD5_SoundGroup_GetMaxAudibleBehavior(this.Handle, out behavior);
        }
        public Result SetMuteFadeSpeed(float speed)
        {
            return FMOD5_SoundGroup_SetMuteFadeSpeed(this.Handle, speed);
        }
        public Result GetMuteFadeSpeed(out float speed)
        {
            return FMOD5_SoundGroup_GetMuteFadeSpeed(this.Handle, out speed);
        }
        public Result SetVolume(float volume)
        {
            return FMOD5_SoundGroup_SetVolume(this.Handle, volume);
        }
        public Result GetVolume(out float volume)
        {
            return FMOD5_SoundGroup_GetVolume(this.Handle, out volume);
        }
        public Result Stop()
        {
            return FMOD5_SoundGroup_Stop(this.Handle);
        }

        // Information only functions.
        public Result GetName(out string name, int namelen)
        {
            IntPtr stringMem = Marshal.AllocHGlobal(namelen);

            Result result = FMOD5_SoundGroup_GetName(this.Handle, stringMem, namelen);
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                name = encoder.StringFromNative(stringMem);
            }
            Marshal.FreeHGlobal(stringMem);

            return result;
        }
        public Result GetNumSounds(out int numsounds)
        {
            return FMOD5_SoundGroup_GetNumSounds(this.Handle, out numsounds);
        }
        public Result GetSound(int index, out Sound sound)
        {
            return FMOD5_SoundGroup_GetSound(this.Handle, index, out sound.Handle);
        }
        public Result GetNumPlaying(out int numplaying)
        {
            return FMOD5_SoundGroup_GetNumPlaying(this.Handle, out numplaying);
        }

        // Userdata set/get.
        public Result SetUserData(IntPtr userdata)
        {
            return FMOD5_SoundGroup_SetUserData(this.Handle, userdata);
        }
        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD5_SoundGroup_GetUserData(this.Handle, out userdata);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_Release               (IntPtr soundgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_GetSystemObject       (IntPtr soundgroup, out IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_SetMaxAudible         (IntPtr soundgroup, int maxaudible);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_GetMaxAudible         (IntPtr soundgroup, out int maxaudible);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_SetMaxAudibleBehavior (IntPtr soundgroup, SoundgroupBehavior behavior);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_GetMaxAudibleBehavior (IntPtr soundgroup, out SoundgroupBehavior behavior);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_SetMuteFadeSpeed      (IntPtr soundgroup, float speed);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_GetMuteFadeSpeed      (IntPtr soundgroup, out float speed);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_SetVolume             (IntPtr soundgroup, float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_GetVolume             (IntPtr soundgroup, out float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_Stop                  (IntPtr soundgroup);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_GetName               (IntPtr soundgroup, IntPtr name, int namelen);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_GetNumSounds          (IntPtr soundgroup, out int numsounds);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_GetSound              (IntPtr soundgroup, int index, out IntPtr sound);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_GetNumPlaying         (IntPtr soundgroup, out int numplaying);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_SetUserData           (IntPtr soundgroup, IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_SoundGroup_GetUserData           (IntPtr soundgroup, out IntPtr userdata);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public SoundGroup(IntPtr ptr) { this.Handle = ptr; }
        public bool HasHandle()       { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()     { this.Handle = IntPtr.Zero; }

        #endregion
    }

    /*
        'DSP' API
    */
    public struct Dsp
    {
        public Result Release()
        {
            return FMOD5_DSP_Release(this.Handle);
        }
        public Result GetSystemObject(out System system)
        {
            return FMOD5_DSP_GetSystemObject(this.Handle, out system.Handle);
        }

        // Connection / disconnection / input and output enumeration.
        public Result AddInput(Dsp input)
        {
            return FMOD5_DSP_AddInput(this.Handle, input.Handle, IntPtr.Zero, DspconnectionType.Standard);
        }
        public Result AddInput(Dsp input, out DspConnection connection, DspconnectionType type = DspconnectionType.Standard)
        {
            return FMOD5_DSP_AddInput(this.Handle, input.Handle, out connection.Handle, type);
        }
        public Result DisconnectFrom(Dsp target, DspConnection connection)
        {
            return FMOD5_DSP_DisconnectFrom(this.Handle, target.Handle, connection.Handle);
        }
        public Result DisconnectAll(bool inputs, bool outputs)
        {
            return FMOD5_DSP_DisconnectAll(this.Handle, inputs, outputs);
        }
        public Result GetNumInputs(out int numinputs)
        {
            return FMOD5_DSP_GetNumInputs(this.Handle, out numinputs);
        }
        public Result GetNumOutputs(out int numoutputs)
        {
            return FMOD5_DSP_GetNumOutputs(this.Handle, out numoutputs);
        }
        public Result GetInput(int index, out Dsp input, out DspConnection inputconnection)
        {
            return FMOD5_DSP_GetInput(this.Handle, index, out input.Handle, out inputconnection.Handle);
        }
        public Result GetOutput(int index, out Dsp output, out DspConnection outputconnection)
        {
            return FMOD5_DSP_GetOutput(this.Handle, index, out output.Handle, out outputconnection.Handle);
        }

        // DSP unit control.
        public Result SetActive(bool active)
        {
            return FMOD5_DSP_SetActive(this.Handle, active);
        }
        public Result GetActive(out bool active)
        {
            return FMOD5_DSP_GetActive(this.Handle, out active);
        }
        public Result SetBypass(bool bypass)
        {
            return FMOD5_DSP_SetBypass(this.Handle, bypass);
        }
        public Result GetBypass(out bool bypass)
        {
            return FMOD5_DSP_GetBypass(this.Handle, out bypass);
        }
        public Result SetWetDryMix(float prewet, float postwet, float dry)
        {
            return FMOD5_DSP_SetWetDryMix(this.Handle, prewet, postwet, dry);
        }
        public Result GetWetDryMix(out float prewet, out float postwet, out float dry)
        {
            return FMOD5_DSP_GetWetDryMix(this.Handle, out prewet, out postwet, out dry);
        }
        public Result SetChannelFormat(Channelmask channelmask, int numchannels, Speakermode sourceSpeakermode)
        {
            return FMOD5_DSP_SetChannelFormat(this.Handle, channelmask, numchannels, sourceSpeakermode);
        }
        public Result GetChannelFormat(out Channelmask channelmask, out int numchannels, out Speakermode sourceSpeakermode)
        {
            return FMOD5_DSP_GetChannelFormat(this.Handle, out channelmask, out numchannels, out sourceSpeakermode);
        }
        public Result GetOutputChannelFormat(Channelmask inmask, int inchannels, Speakermode inspeakermode, out Channelmask outmask, out int outchannels, out Speakermode outspeakermode)
        {
            return FMOD5_DSP_GetOutputChannelFormat(this.Handle, inmask, inchannels, inspeakermode, out outmask, out outchannels, out outspeakermode);
        }
        public Result Reset()
        {
            return FMOD5_DSP_Reset(this.Handle);
        }

        // DSP parameter control.
        public Result SetParameterFloat(int index, float value)
        {
            return FMOD5_DSP_SetParameterFloat(this.Handle, index, value);
        }
        public Result SetParameterInt(int index, int value)
        {
            return FMOD5_DSP_SetParameterInt(this.Handle, index, value);
        }
        public Result SetParameterBool(int index, bool value)
        {
            return FMOD5_DSP_SetParameterBool(this.Handle, index, value);
        }
        public Result SetParameterData(int index, byte[] data)
        {
            return FMOD5_DSP_SetParameterData(this.Handle, index, Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), (uint)data.Length);
        }
        public Result GetParameterFloat(int index, out float value)
        {
            return FMOD5_DSP_GetParameterFloat(this.Handle, index, out value, IntPtr.Zero, 0);
        }
        public Result GetParameterInt(int index, out int value)
        {
            return FMOD5_DSP_GetParameterInt(this.Handle, index, out value, IntPtr.Zero, 0);
        }
        public Result GetParameterBool(int index, out bool value)
        {
            return FMOD5_DSP_GetParameterBool(this.Handle, index, out value, IntPtr.Zero, 0);
        }
        public Result GetParameterData(int index, out IntPtr data, out uint length)
        {
            return FMOD5_DSP_GetParameterData(this.Handle, index, out data, out length, IntPtr.Zero, 0);
        }
        public Result GetNumParameters(out int numparams)
        {
            return FMOD5_DSP_GetNumParameters(this.Handle, out numparams);
        }
        public Result GetParameterInfo(int index, out DspParameterDesc desc)
        {
            IntPtr descPtr;
            Result result = FMOD5_DSP_GetParameterInfo(this.Handle, index, out descPtr);
            desc = (DspParameterDesc)MarshalHelper.PtrToStructure(descPtr, typeof(DspParameterDesc));
            return result;
        }
        public Result GetDataParameterIndex(int datatype, out int index)
        {
            return FMOD5_DSP_GetDataParameterIndex(this.Handle, datatype, out index);
        }
        public Result ShowConfigDialog(IntPtr hwnd, bool show)
        {
            return FMOD5_DSP_ShowConfigDialog(this.Handle, hwnd, show);
        }

        //  DSP attributes.
        public Result GetInfo(out string name, out uint version, out int channels, out int configwidth, out int configheight)
        {
            IntPtr nameMem = Marshal.AllocHGlobal(32);

            Result result = FMOD5_DSP_GetInfo(this.Handle, nameMem, out version, out channels, out configwidth, out configheight);
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                name = encoder.StringFromNative(nameMem);
            }
            Marshal.FreeHGlobal(nameMem);
            return result;
        }
        public Result GetInfo(out uint version, out int channels, out int configwidth, out int configheight)
        {
            return FMOD5_DSP_GetInfo(this.Handle, IntPtr.Zero, out version, out channels, out configwidth, out configheight); ;
        }
        public Result GetType(out DspType type)
        {
            return FMOD5_DSP_GetType(this.Handle, out type);
        }
        public Result GetIdle(out bool idle)
        {
            return FMOD5_DSP_GetIdle(this.Handle, out idle);
        }

        // Userdata set/get.
        public Result SetUserData(IntPtr userdata)
        {
            return FMOD5_DSP_SetUserData(this.Handle, userdata);
        }
        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD5_DSP_GetUserData(this.Handle, out userdata);
        }

        // Metering.
        public Result SetMeteringEnabled(bool inputEnabled, bool outputEnabled)
        {
            return FMOD5_DSP_SetMeteringEnabled(this.Handle, inputEnabled, outputEnabled);
        }
        public Result GetMeteringEnabled(out bool inputEnabled, out bool outputEnabled)
        {
            return FMOD5_DSP_GetMeteringEnabled(this.Handle, out inputEnabled, out outputEnabled);
        }

        public Result GetMeteringInfo(IntPtr zero, out DspMeteringInfo outputInfo)
        {
            return FMOD5_DSP_GetMeteringInfo(this.Handle, zero, out outputInfo);
        }
        public Result GetMeteringInfo(out DspMeteringInfo inputInfo, IntPtr zero)
        {
            return FMOD5_DSP_GetMeteringInfo(this.Handle, out inputInfo, zero);
        }
        public Result GetMeteringInfo(out DspMeteringInfo inputInfo, out DspMeteringInfo outputInfo)
        {
            return FMOD5_DSP_GetMeteringInfo(this.Handle, out inputInfo, out outputInfo);
        }

        public Result GetCpuUsage(out uint exclusive, out uint inclusive)
        {
            return FMOD5_DSP_GetCPUUsage(this.Handle, out exclusive, out inclusive);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_Release                   (IntPtr dsp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetSystemObject           (IntPtr dsp, out IntPtr system);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_AddInput                  (IntPtr dsp, IntPtr input, IntPtr zero, DspconnectionType type);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_AddInput                  (IntPtr dsp, IntPtr input, out IntPtr connection, DspconnectionType type);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_DisconnectFrom            (IntPtr dsp, IntPtr target, IntPtr connection);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_DisconnectAll             (IntPtr dsp, bool inputs, bool outputs);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetNumInputs              (IntPtr dsp, out int numinputs);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetNumOutputs             (IntPtr dsp, out int numoutputs);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetInput                  (IntPtr dsp, int index, out IntPtr input, out IntPtr inputconnection);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetOutput                 (IntPtr dsp, int index, out IntPtr output, out IntPtr outputconnection);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_SetActive                 (IntPtr dsp, bool active);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetActive                 (IntPtr dsp, out bool active);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_SetBypass                 (IntPtr dsp, bool bypass);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetBypass                 (IntPtr dsp, out bool bypass);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_SetWetDryMix              (IntPtr dsp, float prewet, float postwet, float dry);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetWetDryMix              (IntPtr dsp, out float prewet, out float postwet, out float dry);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_SetChannelFormat          (IntPtr dsp, Channelmask channelmask, int numchannels, Speakermode sourceSpeakermode);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetChannelFormat          (IntPtr dsp, out Channelmask channelmask, out int numchannels, out Speakermode sourceSpeakermode);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetOutputChannelFormat    (IntPtr dsp, Channelmask inmask, int inchannels, Speakermode inspeakermode, out Channelmask outmask, out int outchannels, out Speakermode outspeakermode);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_Reset                     (IntPtr dsp);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_SetParameterFloat         (IntPtr dsp, int index, float value);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_SetParameterInt           (IntPtr dsp, int index, int value);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_SetParameterBool          (IntPtr dsp, int index, bool value);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_SetParameterData          (IntPtr dsp, int index, IntPtr data, uint length);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetParameterFloat         (IntPtr dsp, int index, out float value, IntPtr valuestr, int valuestrlen);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetParameterInt           (IntPtr dsp, int index, out int value, IntPtr valuestr, int valuestrlen);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetParameterBool          (IntPtr dsp, int index, out bool value, IntPtr valuestr, int valuestrlen);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetParameterData          (IntPtr dsp, int index, out IntPtr data, out uint length, IntPtr valuestr, int valuestrlen);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetNumParameters          (IntPtr dsp, out int numparams);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetParameterInfo          (IntPtr dsp, int index, out IntPtr desc);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetDataParameterIndex     (IntPtr dsp, int datatype, out int index);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_ShowConfigDialog          (IntPtr dsp, IntPtr hwnd, bool show);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetInfo                   (IntPtr dsp, IntPtr name, out uint version, out int channels, out int configwidth, out int configheight);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetType                   (IntPtr dsp, out DspType type);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetIdle                   (IntPtr dsp, out bool idle);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_SetUserData               (IntPtr dsp, IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSP_GetUserData               (IntPtr dsp, out IntPtr userdata);
        [DllImport(Version.Dll)]
        public static extern Result FMOD5_DSP_SetMeteringEnabled         (IntPtr dsp, bool inputEnabled, bool outputEnabled);
        [DllImport(Version.Dll)]
        public static extern Result FMOD5_DSP_GetMeteringEnabled         (IntPtr dsp, out bool inputEnabled, out bool outputEnabled);
        [DllImport(Version.Dll)]
        public static extern Result FMOD5_DSP_GetMeteringInfo            (IntPtr dsp, IntPtr zero, out DspMeteringInfo outputInfo);
        [DllImport(Version.Dll)]
        public static extern Result FMOD5_DSP_GetMeteringInfo            (IntPtr dsp, out DspMeteringInfo inputInfo, IntPtr zero);
        [DllImport(Version.Dll)]
        public static extern Result FMOD5_DSP_GetMeteringInfo            (IntPtr dsp, out DspMeteringInfo inputInfo, out DspMeteringInfo outputInfo);
        [DllImport(Version.Dll)]
        public static extern Result FMOD5_DSP_GetCPUUsage                (IntPtr dsp, out uint exclusive, out uint inclusive);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public Dsp(IntPtr ptr)      { this.Handle = ptr; }
        public bool HasHandle()     { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()   { this.Handle = IntPtr.Zero; }

        #endregion
    }

    /*
        'DSPConnection' API
    */
    public struct DspConnection
    {
        public Result GetInput(out Dsp input)
        {
            return FMOD5_DSPConnection_GetInput(this.Handle, out input.Handle);
        }
        public Result GetOutput(out Dsp output)
        {
            return FMOD5_DSPConnection_GetOutput(this.Handle, out output.Handle);
        }
        public Result SetMix(float volume)
        {
            return FMOD5_DSPConnection_SetMix(this.Handle, volume);
        }
        public Result GetMix(out float volume)
        {
            return FMOD5_DSPConnection_GetMix(this.Handle, out volume);
        }
        public Result SetMixMatrix(float[] matrix, int outchannels, int inchannels, int inchannelHop = 0)
        {
            return FMOD5_DSPConnection_SetMixMatrix(this.Handle, matrix, outchannels, inchannels, inchannelHop);
        }
        public Result GetMixMatrix(float[] matrix, out int outchannels, out int inchannels, int inchannelHop = 0)
        {
            return FMOD5_DSPConnection_GetMixMatrix(this.Handle, matrix, out outchannels, out inchannels, inchannelHop);
        }
        public Result GetType(out DspconnectionType type)
        {
            return FMOD5_DSPConnection_GetType(this.Handle, out type);
        }

        // Userdata set/get.
        public Result SetUserData(IntPtr userdata)
        {
            return FMOD5_DSPConnection_SetUserData(this.Handle, userdata);
        }
        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD5_DSPConnection_GetUserData(this.Handle, out userdata);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSPConnection_GetInput        (IntPtr dspconnection, out IntPtr input);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSPConnection_GetOutput       (IntPtr dspconnection, out IntPtr output);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSPConnection_SetMix          (IntPtr dspconnection, float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSPConnection_GetMix          (IntPtr dspconnection, out float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSPConnection_SetMixMatrix    (IntPtr dspconnection, float[] matrix, int outchannels, int inchannels, int inchannelHop);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSPConnection_GetMixMatrix    (IntPtr dspconnection, float[] matrix, out int outchannels, out int inchannels, int inchannelHop);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSPConnection_GetType         (IntPtr dspconnection, out DspconnectionType type);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSPConnection_SetUserData     (IntPtr dspconnection, IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_DSPConnection_GetUserData     (IntPtr dspconnection, out IntPtr userdata);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public DspConnection(IntPtr ptr) { this.Handle = ptr; }
        public bool HasHandle()          { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()        { this.Handle = IntPtr.Zero; }

        #endregion
    }

    /*
        'Geometry' API
    */
    public struct Geometry
    {
        public Result Release()
        {
            return FMOD5_Geometry_Release(this.Handle);
        }

        // Polygon manipulation.
        public Result AddPolygon(float directocclusion, float reverbocclusion, bool doublesided, int numvertices, Vector[] vertices, out int polygonindex)
        {
            return FMOD5_Geometry_AddPolygon(this.Handle, directocclusion, reverbocclusion, doublesided, numvertices, vertices, out polygonindex);
        }
        public Result GetNumPolygons(out int numpolygons)
        {
            return FMOD5_Geometry_GetNumPolygons(this.Handle, out numpolygons);
        }
        public Result GetMaxPolygons(out int maxpolygons, out int maxvertices)
        {
            return FMOD5_Geometry_GetMaxPolygons(this.Handle, out maxpolygons, out maxvertices);
        }
        public Result GetPolygonNumVertices(int index, out int numvertices)
        {
            return FMOD5_Geometry_GetPolygonNumVertices(this.Handle, index, out numvertices);
        }
        public Result SetPolygonVertex(int index, int vertexindex, ref Vector vertex)
        {
            return FMOD5_Geometry_SetPolygonVertex(this.Handle, index, vertexindex, ref vertex);
        }
        public Result GetPolygonVertex(int index, int vertexindex, out Vector vertex)
        {
            return FMOD5_Geometry_GetPolygonVertex(this.Handle, index, vertexindex, out vertex);
        }
        public Result SetPolygonAttributes(int index, float directocclusion, float reverbocclusion, bool doublesided)
        {
            return FMOD5_Geometry_SetPolygonAttributes(this.Handle, index, directocclusion, reverbocclusion, doublesided);
        }
        public Result GetPolygonAttributes(int index, out float directocclusion, out float reverbocclusion, out bool doublesided)
        {
            return FMOD5_Geometry_GetPolygonAttributes(this.Handle, index, out directocclusion, out reverbocclusion, out doublesided);
        }

        // Object manipulation.
        public Result SetActive(bool active)
        {
            return FMOD5_Geometry_SetActive(this.Handle, active);
        }
        public Result GetActive(out bool active)
        {
            return FMOD5_Geometry_GetActive(this.Handle, out active);
        }
        public Result SetRotation(ref Vector forward, ref Vector up)
        {
            return FMOD5_Geometry_SetRotation(this.Handle, ref forward, ref up);
        }
        public Result GetRotation(out Vector forward, out Vector up)
        {
            return FMOD5_Geometry_GetRotation(this.Handle, out forward, out up);
        }
        public Result SetPosition(ref Vector position)
        {
            return FMOD5_Geometry_SetPosition(this.Handle, ref position);
        }
        public Result GetPosition(out Vector position)
        {
            return FMOD5_Geometry_GetPosition(this.Handle, out position);
        }
        public Result SetScale(ref Vector scale)
        {
            return FMOD5_Geometry_SetScale(this.Handle, ref scale);
        }
        public Result GetScale(out Vector scale)
        {
            return FMOD5_Geometry_GetScale(this.Handle, out scale);
        }
        public Result Save(IntPtr data, out int datasize)
        {
            return FMOD5_Geometry_Save(this.Handle, data, out datasize);
        }

        // Userdata set/get.
        public Result SetUserData(IntPtr userdata)
        {
            return FMOD5_Geometry_SetUserData(this.Handle, userdata);
        }
        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD5_Geometry_GetUserData(this.Handle, out userdata);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_Release              (IntPtr geometry);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_AddPolygon           (IntPtr geometry, float directocclusion, float reverbocclusion, bool doublesided, int numvertices, Vector[] vertices, out int polygonindex);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_GetNumPolygons       (IntPtr geometry, out int numpolygons);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_GetMaxPolygons       (IntPtr geometry, out int maxpolygons, out int maxvertices);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_GetPolygonNumVertices(IntPtr geometry, int index, out int numvertices);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_SetPolygonVertex     (IntPtr geometry, int index, int vertexindex, ref Vector vertex);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_GetPolygonVertex     (IntPtr geometry, int index, int vertexindex, out Vector vertex);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_SetPolygonAttributes (IntPtr geometry, int index, float directocclusion, float reverbocclusion, bool doublesided);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_GetPolygonAttributes (IntPtr geometry, int index, out float directocclusion, out float reverbocclusion, out bool doublesided);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_SetActive            (IntPtr geometry, bool active);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_GetActive            (IntPtr geometry, out bool active);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_SetRotation          (IntPtr geometry, ref Vector forward, ref Vector up);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_GetRotation          (IntPtr geometry, out Vector forward, out Vector up);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_SetPosition          (IntPtr geometry, ref Vector position);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_GetPosition          (IntPtr geometry, out Vector position);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_SetScale             (IntPtr geometry, ref Vector scale);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_GetScale             (IntPtr geometry, out Vector scale);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_Save                 (IntPtr geometry, IntPtr data, out int datasize);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_SetUserData          (IntPtr geometry, IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Geometry_GetUserData          (IntPtr geometry, out IntPtr userdata);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public Geometry(IntPtr ptr) { this.Handle = ptr; }
        public bool HasHandle()     { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()   { this.Handle = IntPtr.Zero; }

        #endregion
    }

    /*
        'Reverb3D' API
    */
    public struct Reverb3D
    {
        public Result Release()
        {
            return FMOD5_Reverb3D_Release(this.Handle);
        }

        // Reverb manipulation.
        public Result Set3DAttributes(ref Vector position, float mindistance, float maxdistance)
        {
            return FMOD5_Reverb3D_Set3DAttributes(this.Handle, ref position, mindistance, maxdistance);
        }
        public Result Get3DAttributes(ref Vector position, ref float mindistance, ref float maxdistance)
        {
            return FMOD5_Reverb3D_Get3DAttributes(this.Handle, ref position, ref mindistance, ref maxdistance);
        }
        public Result SetProperties(ref ReverbProperties properties)
        {
            return FMOD5_Reverb3D_SetProperties(this.Handle, ref properties);
        }
        public Result GetProperties(ref ReverbProperties properties)
        {
            return FMOD5_Reverb3D_GetProperties(this.Handle, ref properties);
        }
        public Result SetActive(bool active)
        {
            return FMOD5_Reverb3D_SetActive(this.Handle, active);
        }
        public Result GetActive(out bool active)
        {
            return FMOD5_Reverb3D_GetActive(this.Handle, out active);
        }

        // Userdata set/get.
        public Result SetUserData(IntPtr userdata)
        {
            return FMOD5_Reverb3D_SetUserData(this.Handle, userdata);
        }
        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD5_Reverb3D_GetUserData(this.Handle, out userdata);
        }

        #region importfunctions
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Reverb3D_Release             (IntPtr reverb3d);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Reverb3D_Set3DAttributes     (IntPtr reverb3d, ref Vector position, float mindistance, float maxdistance);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Reverb3D_Get3DAttributes     (IntPtr reverb3d, ref Vector position, ref float mindistance, ref float maxdistance);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Reverb3D_SetProperties       (IntPtr reverb3d, ref ReverbProperties properties);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Reverb3D_GetProperties       (IntPtr reverb3d, ref ReverbProperties properties);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Reverb3D_SetActive           (IntPtr reverb3d, bool active);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Reverb3D_GetActive           (IntPtr reverb3d, out bool active);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Reverb3D_SetUserData         (IntPtr reverb3d, IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD5_Reverb3D_GetUserData         (IntPtr reverb3d, out IntPtr userdata);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public Reverb3D(IntPtr ptr) { this.Handle = ptr; }
        public bool HasHandle()     { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()   { this.Handle = IntPtr.Zero; }

        #endregion
    }

    #region Helper Functions
    [StructLayout(LayoutKind.Sequential)]
    public struct StringWrapper
    {
        IntPtr nativeUtf8Ptr;

        public StringWrapper(IntPtr ptr)
        {
            nativeUtf8Ptr = ptr;
        }

        public static implicit operator string(StringWrapper fstring)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return encoder.StringFromNative(fstring.nativeUtf8Ptr);
            }
        }
    }

    static class StringHelper
    {
        public class ThreadSafeEncoding : IDisposable
        {
            UTF8Encoding _encoding = new UTF8Encoding();
            byte[] _encodedBuffer = new byte[128];
            char[] _decodedBuffer = new char[128];
            bool _inUse;
            GCHandle _gcHandle;

            public bool InUse()    { return _inUse; }
            public void SetInUse() { _inUse = true; }

            private int RoundUpPowerTwo(int number)
            {
                int newNumber = 1;
                while (newNumber <= number)
                {
                    newNumber *= 2;
                }

                return newNumber;
            }

            public byte[] ByteFromStringUtf8(string s)
            {
                if (s == null)
                {
                    return null;
                }

                int maximumLength = _encoding.GetMaxByteCount(s.Length) + 1; // +1 for null terminator
                if (maximumLength > _encodedBuffer.Length)
                {
                    int encodedLength = _encoding.GetByteCount(s) + 1; // +1 for null terminator
                    if (encodedLength > _encodedBuffer.Length)
                    {
                        _encodedBuffer = new byte[RoundUpPowerTwo(encodedLength)];
                    }
                }

                int byteCount = _encoding.GetBytes(s, 0, s.Length, _encodedBuffer, 0);
                _encodedBuffer[byteCount] = 0; // Apply null terminator

                return _encodedBuffer;
            }

            public IntPtr IntptrFromStringUtf8(string s)
            {
                if (s == null)
                {
                    return IntPtr.Zero;
                }

                _gcHandle = GCHandle.Alloc(ByteFromStringUtf8(s), GCHandleType.Pinned);
                return _gcHandle.AddrOfPinnedObject();
            }

            public string StringFromNative(IntPtr nativePtr)
            {
                if (nativePtr == IntPtr.Zero)
                {
                    return "";
                }

                int nativeLen = 0;
                while (Marshal.ReadByte(nativePtr, nativeLen) != 0)
                {
                    nativeLen++;
                }

                if (nativeLen == 0)
                {
                    return "";
                }

                if (nativeLen > _encodedBuffer.Length)
                {
                    _encodedBuffer = new byte[RoundUpPowerTwo(nativeLen)];
                }

                Marshal.Copy(nativePtr, _encodedBuffer, 0, nativeLen);

                int maximumLength = _encoding.GetMaxCharCount(nativeLen);
                if (maximumLength > _decodedBuffer.Length)
                {
                    int decodedLength = _encoding.GetCharCount(_encodedBuffer, 0, nativeLen);
                    if (decodedLength > _decodedBuffer.Length)
                    {
                        _decodedBuffer = new char[RoundUpPowerTwo(decodedLength)];
                    }
                }

                int charCount = _encoding.GetChars(_encodedBuffer, 0, nativeLen, _decodedBuffer, 0);

                return new String(_decodedBuffer, 0, charCount);
            }

            public void Dispose()
            {
                if (_gcHandle.IsAllocated)
                {
                    _gcHandle.Free();
                }
                lock (_encoders)
                {
                    _inUse = false;
                }
            }
        }

        static List<ThreadSafeEncoding> _encoders = new List<ThreadSafeEncoding>(1);

        public static ThreadSafeEncoding GetFreeHelper()
        {
            lock (_encoders)
            {
                ThreadSafeEncoding helper = null;
                // Search for not in use helper
                for (int i = 0; i < _encoders.Count; i++)
                {
                    if (!_encoders[i].InUse())
                    {
                        helper = _encoders[i];
                        break;
                    }
                }
                // Otherwise create another helper
                if (helper == null)
                {
                    helper = new ThreadSafeEncoding();
                    _encoders.Add(helper);
                }
                helper.SetInUse();
                return helper;
            }
        }
    }

    // Some of the Marshal functions were marked as deprecated / obsolete, however that decision was reversed: https://github.com/dotnet/corefx/pull/10541
    // Use the old syntax (non-generic) to ensure maximum compatibility (especially with Unity) ignoring the warnings
    public static class MarshalHelper
    {
#pragma warning disable 618
        public static int SizeOf(Type t)
        {
            return Marshal.SizeOf(t); // Always use Type version, never Object version as it boxes causes GC allocations
        }

        public static object PtrToStructure(IntPtr ptr, Type structureType)
        {
            return Marshal.PtrToStructure(ptr, structureType);
        }
#pragma warning restore 618
    }

    #endregion
}
