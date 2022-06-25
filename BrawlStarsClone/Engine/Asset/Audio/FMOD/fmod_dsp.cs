/* ======================================================================================== */
/* FMOD Core API - DSP header file.                                                         */
/* Copyright (c), Firelight Technologies Pty, Ltd. 2004-2022.                               */
/*                                                                                          */
/* Use this header if you are wanting to develop your own DSP plugin to use with FMODs      */
/* dsp system.  With this header you can make your own DSP plugin that FMOD can             */
/* register and use.  See the documentation and examples on how to make a working plugin.   */
/*                                                                                          */
/* For more detail visit:                                                                   */
/* https://fmod.com/resources/documentation-api?version=2.0&page=plugin-api-dsp.html        */
/* =========================================================================================*/

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace FMOD
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DspBufferArray
    {
        public int              numbuffers;
        public int[]            buffernumchannels;
        public Channelmask[]    bufferchannelmask;
        public IntPtr[]         buffers;
        public Speakermode      speakermode;
    }

    public enum DspProcessOperation
    {
        ProcessPerform = 0,
        ProcessQuery
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Complex
    {
        public float real;
        public float imag;
    }

    public enum DspPanSurroundFlags
    {
        Default = 0,
        RotationNotBiased = 1,
    }


    /*
        DSP callbacks
    */
    public delegate Result DspCreatecallback                   (ref DspState dspState);
    public delegate Result DspReleasecallback                  (ref DspState dspState);
    public delegate Result DspResetcallback                    (ref DspState dspState);
    public delegate Result DspSetpositioncallback              (ref DspState dspState, uint pos);
    public delegate Result DspReadcallback                     (ref DspState dspState, IntPtr inbuffer, IntPtr outbuffer, uint length, int inchannels, ref int outchannels);
    public delegate Result DspShouldiprocessCallback          (ref DspState dspState, bool inputsidle, uint length, Channelmask inmask, int inchannels, Speakermode speakermode);
    public delegate Result DspProcessCallback                 (ref DspState dspState, uint length, ref DspBufferArray inbufferarray, ref DspBufferArray outbufferarray, bool inputsidle, DspProcessOperation op);
    public delegate Result DspSetparamFloatCallback          (ref DspState dspState, int index, float value);
    public delegate Result DspSetparamIntCallback            (ref DspState dspState, int index, int value);
    public delegate Result DspSetparamBoolCallback           (ref DspState dspState, int index, bool value);
    public delegate Result DspSetparamDataCallback           (ref DspState dspState, int index, IntPtr data, uint length);
    public delegate Result DspGetparamFloatCallback          (ref DspState dspState, int index, ref float value, IntPtr valuestr);
    public delegate Result DspGetparamIntCallback            (ref DspState dspState, int index, ref int value, IntPtr valuestr);
    public delegate Result DspGetparamBoolCallback           (ref DspState dspState, int index, ref bool value, IntPtr valuestr);
    public delegate Result DspGetparamDataCallback           (ref DspState dspState, int index, ref IntPtr data, ref uint length, IntPtr valuestr);
    public delegate Result DspSystemRegisterCallback         (ref DspState dspState);
    public delegate Result DspSystemDeregisterCallback       (ref DspState dspState);
    public delegate Result DspSystemMixCallback              (ref DspState dspState, int stage);


    /*
        DSP functions
    */
    public delegate IntPtr DspAllocFunc                         (uint size, MemoryType type, IntPtr sourcestr);
    public delegate IntPtr DspReallocFunc                       (IntPtr ptr, uint size, MemoryType type, IntPtr sourcestr);
    public delegate void   DspFreeFunc                          (IntPtr ptr, MemoryType type, IntPtr sourcestr);
    public delegate void   DspLogFunc                           (DebugFlags level, IntPtr file, int line, IntPtr function, IntPtr format);
    public delegate Result DspGetsamplerateFunc                 (ref DspState dspState, ref int rate);
    public delegate Result DspGetblocksizeFunc                  (ref DspState dspState, ref uint blocksize);
    public delegate Result DspGetspeakermodeFunc                (ref DspState dspState, ref int speakermodeMixer, ref int speakermodeOutput);
    public delegate Result DspGetclockFunc                      (ref DspState dspState, out ulong clock, out uint offset, out uint length);
    public delegate Result DspGetlistenerattributesFunc         (ref DspState dspState, ref int numlisteners, IntPtr attributes);
    public delegate Result DspGetuserdataFunc                   (ref DspState dspState, out IntPtr userdata);
    public delegate Result DspDftFftrealFunc                   (ref DspState dspState, int size, IntPtr signal, IntPtr dft, IntPtr window, int signalhop);
    public delegate Result DspDftIfftrealFunc                  (ref DspState dspState, int size, IntPtr dft, IntPtr signal, IntPtr window, int signalhop);
    public delegate Result DspPanSummonomatrixFunc             (ref DspState dspState, int sourceSpeakerMode, float lowFrequencyGain, float overallGain, IntPtr matrix);
    public delegate Result DspPanSumstereomatrixFunc           (ref DspState dspState, int sourceSpeakerMode, float pan, float lowFrequencyGain, float overallGain, int matrixHop, IntPtr matrix);
    public delegate Result DspPanSumsurroundmatrixFunc         (ref DspState dspState, int sourceSpeakerMode, int targetSpeakerMode, float direction, float extent, float rotation, float lowFrequencyGain, float overallGain, int matrixHop, IntPtr matrix, DspPanSurroundFlags flags);
    public delegate Result DspPanSummonotosurroundmatrixFunc   (ref DspState dspState, int targetSpeakerMode, float direction, float extent, float lowFrequencyGain, float overallGain, int matrixHop, IntPtr matrix);
    public delegate Result DspPanSumstereotosurroundmatrixFunc (ref DspState dspState, int targetSpeakerMode, float direction, float extent, float rotation, float lowFrequencyGain, float overallGain, int matrixHop, IntPtr matrix);
    public delegate Result DspPanGetrolloffgainFunc            (ref DspState dspState, DspPan3DRolloffType rolloff, float distance, float mindistance, float maxdistance, out float gain);


    public enum DspType : int
    {
        Unknown,
        Mixer,
        Oscillator,
        Lowpass,
        Itlowpass,
        Highpass,
        Echo,
        Fader,
        Flange,
        Distortion,
        Normalize,
        Limiter,
        Parameq,
        Pitchshift,
        Chorus,
        Vstplugin,
        Winampplugin,
        Itecho,
        Compressor,
        Sfxreverb,
        LowpassSimple,
        Delay,
        Tremolo,
        Ladspaplugin,
        Send,
        Return,
        HighpassSimple,
        Pan,
        ThreeEq,
        Fft,
        LoudnessMeter,
        Envelopefollower,
        Convolutionreverb,
        Channelmix,
        Transceiver,
        Objectpan,
        MultibandEq,
        Max
    }

    public enum DspParameterType
    {
        Float = 0,
        Int,
        Bool,
        Data,
        Max
    }

    public enum DspParameterFloatMappingType
    {
        DspParameterFloatMappingTypeLinear = 0,
        DspParameterFloatMappingTypeAuto,
        DspParameterFloatMappingTypePiecewiseLinear,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameterFloatMappingPiecewiseLinear
    {
        public int numpoints;
        public IntPtr pointparamvalues;
        public IntPtr pointpositions;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameterFloatMapping
    {
        public DspParameterFloatMappingType type;
        public DspParameterFloatMappingPiecewiseLinear piecewiselinearmapping;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameterDescFloat
    {
        public float                     min;
        public float                     max;
        public float                     defaultval;
        public DspParameterFloatMapping mapping;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameterDescInt
    {
        public int                       min;
        public int                       max;
        public int                       defaultval;
        public bool                      goestoinf;
        public IntPtr                    valuenames;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameterDescBool
    {
        public bool                      defaultval;
        public IntPtr                    valuenames;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameterDescData
    {
        public int                       datatype;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct DspParameterDescUnion
    {
        [FieldOffset(0)]
        public DspParameterDescFloat   floatdesc;
        [FieldOffset(0)]
        public DspParameterDescInt     intdesc;
        [FieldOffset(0)]
        public DspParameterDescBool    booldesc;
        [FieldOffset(0)]
        public DspParameterDescData    datadesc;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameterDesc
    {
        public DspParameterType   type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[]               name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[]               label;
        public string               description;

        public DspParameterDescUnion desc;
    }

    public enum DspParameterDataType
    {
        DspParameterDataTypeUser =                       0,
        DspParameterDataTypeOverallgain =               -1,
        DspParameterDataType3Dattributes =              -2,
        DspParameterDataTypeSidechain =                 -3,
        DspParameterDataTypeFft =                       -4,
        DspParameterDataType3DattributesMulti =        -5,
        DspParameterDataTypeAttenuationRange =         -6
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameterOverallgain
    {
        public float linear_gain;
        public float linear_gain_additive;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameter3Dattributes
    {
        public Attributes3D relative;
        public Attributes3D absolute;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameter3DattributesMulti
    {
        public int            numlisteners;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Attributes3D[] relative;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public float[] weight;
        public Attributes3D absolute;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameterSidechain
    {
        public int sidechainenable;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameterFft
    {
        public int     length;
        public int     numchannels;
        
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=32)]
        private IntPtr[] spectrum_internal;

        public float[][] Spectrum
        {
            get
            {
                var buffer = new float[numchannels][];
                
                for (int i = 0; i < numchannels; ++i)
                {
                    buffer[i] = new float[length];
                    Marshal.Copy(spectrum_internal[i], buffer[i], 0, length);
                }
                
                return buffer;
            }
        }

        public void GetSpectrum(ref float[][] buffer)
        {
            int bufferLength = Math.Min(buffer.Length, numchannels);
            for (int i = 0; i < bufferLength; ++i)
            {
                GetSpectrum(i, ref buffer[i]);
            }
        }

        public void GetSpectrum(int channel, ref float[] buffer)
        {
            int bufferLength = Math.Min(buffer.Length, length);
            Marshal.Copy(spectrum_internal[channel], buffer, 0, bufferLength);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspLoudnessMeterInfoType
    {
        public float momentaryloudness;
        public float shorttermloudness;
        public float integratedloudness;
        public float loudness10thpercentile;
        public float loudness95thpercentile;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
        public float[] loudnesshistogram;
        public float maxtruepeak;
        public float maxmomentaryloudness;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspLoudnessMeterWeightingType
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public float[] channelweight;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspParameterAttenuationRange
    {
        public float min;
        public float max;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspDescription
    {
        public uint                           pluginsdkversion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[]                         name;
        public uint                           version;
        public int                            numinputbuffers;
        public int                            numoutputbuffers;
        public DspCreatecallback             create;
        public DspReleasecallback            release;
        public DspResetcallback              reset;
        public DspReadcallback               read;
        public DspProcessCallback           process;
        public DspSetpositioncallback        setposition;

        public int                            numparameters;
        public IntPtr                         paramdesc;
        public DspSetparamFloatCallback    setparameterfloat;
        public DspSetparamIntCallback      setparameterint;
        public DspSetparamBoolCallback     setparameterbool;
        public DspSetparamDataCallback     setparameterdata;
        public DspGetparamFloatCallback    getparameterfloat;
        public DspGetparamIntCallback      getparameterint;
        public DspGetparamBoolCallback     getparameterbool;
        public DspGetparamDataCallback     getparameterdata;
        public DspShouldiprocessCallback    shouldiprocess;
        public IntPtr                         userdata;

        public DspSystemRegisterCallback   sys_register;
        public DspSystemDeregisterCallback sys_deregister;
        public DspSystemMixCallback        sys_mix;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspStateDftFunctions
    {
        public DspDftFftrealFunc  fftreal;
        public DspDftIfftrealFunc inversefftreal;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspStatePanFunctions
    {
        public DspPanSummonomatrixFunc             summonomatrix;
        public DspPanSumstereomatrixFunc           sumstereomatrix;
        public DspPanSumsurroundmatrixFunc         sumsurroundmatrix;
        public DspPanSummonotosurroundmatrixFunc   summonotosurroundmatrix;
        public DspPanSumstereotosurroundmatrixFunc sumstereotosurroundmatrix;
        public DspPanGetrolloffgainFunc            getrolloffgain;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspStateFunctions
    {
        public DspAllocFunc                  alloc;
        public DspReallocFunc                realloc;
        public DspFreeFunc                   free;
        public DspGetsamplerateFunc          getsamplerate;
        public DspGetblocksizeFunc           getblocksize;
        public IntPtr                          dft;
        public IntPtr                          pan;
        public DspGetspeakermodeFunc         getspeakermode;
        public DspGetclockFunc               getclock;
        public DspGetlistenerattributesFunc  getlistenerattributes;
        public DspLogFunc                    log;
        public DspGetuserdataFunc            getuserdata;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspState
    {
        public IntPtr     instance;
        public IntPtr     plugindata;
        public uint       channelmask;
        public int        source_speakermode;
        public IntPtr     sidechaindata;
        public int        sidechainchannels;
        public IntPtr     functions;
        public int        systemobject;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DspMeteringInfo
    {
        public int   numsamples;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
        public float[] peaklevel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
        public float[] rmslevel;
        public short numchannels;
    }

    /*
        ==============================================================================================================

        FMOD built in effect parameters.
        Use DSP::setParameter with these enums for the 'index' parameter.

        ==============================================================================================================
    */

    public enum DspOscillator : int
    {
        Type,
        Rate
    }

    public enum DspLowpass : int
    {
        Cutoff,
        Resonance
    }

    public enum DspItlowpass : int
    {
        Cutoff,
        Resonance
    }

    public enum DspHighpass : int
    {
        Cutoff,
        Resonance
    }

    public enum DspEcho : int
    {
        Delay,
        Feedback,
        Drylevel,
        Wetlevel
    }

    public enum DspFader : int
    {
        Gain,
        OverallGain,
    }

    public enum DspDelay : int
    {
        Ch0,
        Ch1,
        Ch2,
        Ch3,
        Ch4,
        Ch5,
        Ch6,
        Ch7,
        Ch8,
        Ch9,
        Ch10,
        Ch11,
        Ch12,
        Ch13,
        Ch14,
        Ch15,
        Maxdelay,
    }

    public enum DspFlange : int
    {
        Mix,
        Depth,
        Rate
    }

    public enum DspTremolo : int
    {
        Frequency,
        Depth,
        Shape,
        Skew,
        Duty,
        Square,
        Phase,
        Spread
    }

    public enum DspDistortion : int
    {
        Level
    }

    public enum DspNormalize : int
    {
        Fadetime,
        Threshold,
        Maxamp
    }

    public enum DspLimiter : int
    {
        Releasetime,
        Ceiling,
        Maximizergain,
        Mode,
    }

    public enum DspParameq : int
    {
        Center,
        Bandwidth,
        Gain
    }

    public enum DspMultibandEq : int
    {
        AFilter,
        AFrequency,
        AQ,
        AGain,
        BFilter,
        BFrequency,
        BQ,
        BGain,
        CFilter,
        CFrequency,
        CQ,
        CGain,
        DFilter,
        DFrequency,
        DQ,
        DGain,
        EFilter,
        EFrequency,
        EQ,
        EGain,
    }

    public enum DspMultibandEqFilterType : int
    {
        Disabled,
        Lowpass12Db,
        Lowpass24Db,
        Lowpass48Db,
        Highpass12Db,
        Highpass24Db,
        Highpass48Db,
        Lowshelf,
        Highshelf,
        Peaking,
        Bandpass,
        Notch,
        Allpass,
    }

    public enum DspPitchshift : int
    {
        Pitch,
        Fftsize,
        Overlap,
        Maxchannels
    }

    public enum DspChorus : int
    {
        Mix,
        Rate,
        Depth,
    }

    public enum DspItecho : int
    {
        Wetdrymix,
        Feedback,
        Leftdelay,
        Rightdelay,
        Pandelay
    }

    public enum DspCompressor : int
    {
        Threshold,
        Ratio,
        Attack,
        Release,
        Gainmakeup,
        Usesidechain,
        Linked
    }

    public enum DspSfxreverb : int
    {
        Decaytime,
        Earlydelay,
        Latedelay,
        Hfreference,
        Hfdecayratio,
        Diffusion,
        Density,
        Lowshelffrequency,
        Lowshelfgain,
        Highcut,
        Earlylatemix,
        Wetlevel,
        Drylevel
    }

    public enum DspLowpassSimple : int
    {
        Cutoff
    }

    public enum DspSend : int
    {
        Returnid,
        Level,
    }

    public enum DspReturn : int
    {
        Id,
        InputSpeakerMode
    }

    public enum DspHighpassSimple : int
    {
        Cutoff
    }

    public enum DspPan2DStereoModeType : int
    {
        Distributed,
        Discrete
    }

    public enum DspPanModeType : int
    {
        Mono,
        Stereo,
        Surround
    }

    public enum DspPan3DRolloffType : int
    {
        Linearsquared,
        Linear,
        Inverse,
        Inversetapered,
        Custom
    }

    public enum DspPan3DExtentModeType : int
    {
        Auto,
        User,
        Off
    }

    public enum DspPan : int
    {
        Mode,
        _2D_STEREO_POSITION,
        _2D_DIRECTION,
        _2D_EXTENT,
        _2D_ROTATION,
        _2D_LFE_LEVEL,
        _2D_STEREO_MODE,
        _2D_STEREO_SEPARATION,
        _2D_STEREO_AXIS,
        EnabledSpeakers,
        _3D_POSITION,
        _3D_ROLLOFF,
        _3D_MIN_DISTANCE,
        _3D_MAX_DISTANCE,
        _3D_EXTENT_MODE,
        _3D_SOUND_SIZE,
        _3D_MIN_EXTENT,
        _3D_PAN_BLEND,
        LfeUpmixEnabled,
        OverallGain,
        SurroundSpeakerMode,
        _2D_HEIGHT_BLEND,
        AttenuationRange,
        OverrideRange
    }

    public enum DspThreeEqCrossoverslopeType : int
    {
        _12DB,
        _24DB,
        _48DB
    }

    public enum DspThreeEq : int
    {
        Lowgain,
        Midgain,
        Highgain,
        Lowcrossover,
        Highcrossover,
        Crossoverslope
    }

    public enum DspFftWindow : int
    {
        Rect,
        Triangle,
        Hamming,
        Hanning,
        Blackman,
        Blackmanharris
    }

    public enum DspFft : int
    {
        Windowsize,
        Windowtype,
        Spectrumdata,
        DominantFreq
    }


    public enum DspLoudnessMeter : int
    {
        State,
        Weighting,
        Info
    }


    public enum DspLoudnessMeterStateType : int
    {
        ResetIntegrated = -3,
        ResetMaxpeak = -2,
        ResetAll = -1,
        Paused = 0,
        Analyzing = 1
    }

    public enum DspEnvelopefollower : int
    {
        Attack,
        Release,
        Envelope,
        Usesidechain
    }

    public enum DspConvolutionReverb : int
    {
        Ir,
        Wet,
        Dry,
        Linked
    }

    public enum DspChannelmixOutput : int
    {
        Default,
        Allmono,
        Allstereo,
        Allquad,
        All5Point1,
        All7Point1,
        Alllfe,
        All7Point1Point4
    }

    public enum DspChannelmix : int
    {
        Outputgrouping,
        GainCh0,
        GainCh1,
        GainCh2,
        GainCh3,
        GainCh4,
        GainCh5,
        GainCh6,
        GainCh7,
        GainCh8,
        GainCh9,
        GainCh10,
        GainCh11,
        GainCh12,
        GainCh13,
        GainCh14,
        GainCh15,
        GainCh16,
        GainCh17,
        GainCh18,
        GainCh19,
        GainCh20,
        GainCh21,
        GainCh22,
        GainCh23,
        GainCh24,
        GainCh25,
        GainCh26,
        GainCh27,
        GainCh28,
        GainCh29,
        GainCh30,
        GainCh31,
        OutputCh0,
        OutputCh1,
        OutputCh2,
        OutputCh3,
        OutputCh4,
        OutputCh5,
        OutputCh6,
        OutputCh7,
        OutputCh8,
        OutputCh9,
        OutputCh10,
        OutputCh11,
        OutputCh12,
        OutputCh13,
        OutputCh14,
        OutputCh15,
        OutputCh16,
        OutputCh17,
        OutputCh18,
        OutputCh19,
        OutputCh20,
        OutputCh21,
        OutputCh22,
        OutputCh23,
        OutputCh24,
        OutputCh25,
        OutputCh26,
        OutputCh27,
        OutputCh28,
        OutputCh29,
        OutputCh30,
        OutputCh31,
    }

    public enum DspTransceiverSpeakermode : int
    {
        Auto = -1,
        Mono = 0,
        Stereo,
        Surround,
    }

    public enum DspTransceiver : int
    {
        Transmit,
        Gain,
        Channel,
        Transmitspeakermode
    }

    public enum DspObjectpan : int
    {
        _3D_POSITION,
        _3D_ROLLOFF,
        _3D_MIN_DISTANCE,
        _3D_MAX_DISTANCE,
        _3D_EXTENT_MODE,
        _3D_SOUND_SIZE,
        _3D_MIN_EXTENT,
        OverallGain,
        Outputgain,
        AttenuationRange,
        OverrideRange
    }
}
