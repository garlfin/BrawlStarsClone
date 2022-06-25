/* ======================================================================================== */
/* FMOD Studio API - C# wrapper.                                                            */
/* Copyright (c), Firelight Technologies Pty, Ltd. 2004-2022.                               */
/*                                                                                          */
/* For more detail visit:                                                                   */
/* https://fmod.com/resources/documentation-api?version=2.0&page=page=studio-api.html       */
/* ======================================================================================== */

using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace FMOD.Studio
{
    public partial class StudioVersion
    {
#if !UNITY_2019_4_OR_NEWER
        public const string Dll     = "fmodstudio";
#endif
    }

    public enum StopMode : int
    {
        Allowfadeout,
        Immediate,
    }

    public enum LoadingState : int
    {
        Unloading,
        Unloaded,
        Loading,
        Loaded,
        Error,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ProgrammerSoundProperties
    {
        public StringWrapper name;
        public IntPtr sound;
        public int subsoundIndex;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TimelineMarkerProperties
    {
        public StringWrapper name;
        public int position;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TimelineBeatProperties
    {
        public int bar;
        public int beat;
        public int position;
        public float tempo;
        public int timesignatureupper;
        public int timesignaturelower;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TimelineNestedBeatProperties
    {
        public Guid eventid;
        public TimelineBeatProperties properties;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Advancedsettings
    {
        public int cbsize;
        public int commandqueuesize;
        public int handleinitialsize;
        public int studioupdateperiod;
        public int idlesampledatapoolsize;
        public int streamingscheduledelay;
        public IntPtr encryptionkey;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CpuUsage
    {
        public float update;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BufferInfo
    {
        public int currentusage;
        public int peakusage;
        public int capacity;
        public int stallcount;
        public float stalltime;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BufferUsage
    {
        public BufferInfo studiocommandqueue;
        public BufferInfo studiohandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BankInfo
    {
        public int size;
        public IntPtr userdata;
        public int userdatalength;
        public FileOpenCallback opencallback;
        public FileCloseCallback closecallback;
        public FileReadCallback readcallback;
        public FileSeekCallback seekcallback;
    }

    [Flags]
    public enum SystemCallbackType : uint
    {
        Preupdate = 0x00000001,
        Postupdate = 0x00000002,
        BankUnload = 0x00000004,
        LiveupdateConnected = 0x00000008,
        LiveupdateDisconnected = 0x00000010,
        All = 0xFFFFFFFF,
    }

    public delegate Result SystemCallback(IntPtr system, SystemCallbackType type, IntPtr commanddata, IntPtr userdata);

    public enum ParameterType : int
    {
        GameControlled,
        AutomaticDistance,
        AutomaticEventConeAngle,
        AutomaticEventOrientation,
        AutomaticDirection,
        AutomaticElevation,
        AutomaticListenerOrientation,
        AutomaticSpeed,
        AutomaticSpeedAbsolute,
        AutomaticDistanceNormalized,
        Max
    }

    [Flags]
    public enum ParameterFlags : uint
    {
        Readonly      = 0x00000001,
        Automatic     = 0x00000002,
        Global        = 0x00000004,
        Discrete      = 0x00000008,
        Labeled       = 0x00000010,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ParameterId
    {
        public uint data1;
        public uint data2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ParameterDescription
    {
        public StringWrapper name;
        public ParameterId id;
        public float minimum;
        public float maximum;
        public float defaultvalue;
        public ParameterType type;
        public ParameterFlags flags;
        public Guid guid;
    }

    // This is only need for loading memory and given our C# wrapper LOAD_MEMORY_POINT isn't feasible anyway
    enum LoadMemoryMode : int
    {
        LoadMemory,
        LoadMemoryPoint,
    }

    enum LoadMemoryAlignment : int
    {
        Value = 32
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SoundInfo
    {
        public IntPtr name_or_data;
        public Mode mode;
        public Createsoundexinfo exinfo;
        public int subsoundindex;

        public string Name
        {
            get
            {
                using (StringHelper.ThreadSafeEncoding encoding = StringHelper.GetFreeHelper())
                {
                    return ((mode & (Mode.Openmemory | Mode.OpenmemoryPoint)) == 0) ? encoding.StringFromNative(name_or_data) : String.Empty;
                }
            }
        }
    }

    public enum UserPropertyType : int
    {
        Integer,
        Boolean,
        Float,
        String,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct UserProperty
    {
        public StringWrapper name;
        public UserPropertyType type;
        private UnionIntBoolFloatString value;

        public int IntValue()       {   return (type == UserPropertyType.Integer) ? value.intvalue : -1;      }
        public bool BoolValue()     {   return (type == UserPropertyType.Boolean) ? value.boolvalue : false;  }
        public float FloatValue()   {   return (type == UserPropertyType.Float)   ? value.floatvalue : -1;    }
        public string StringValue() {   return (type == UserPropertyType.String)  ? value.stringvalue : "";   }
    };

    [StructLayout(LayoutKind.Explicit)]
    struct UnionIntBoolFloatString
    {
        [FieldOffset(0)]
        public int intvalue;
        [FieldOffset(0)]
        public bool boolvalue;
        [FieldOffset(0)]
        public float floatvalue;
        [FieldOffset(0)]
        public StringWrapper stringvalue;
    }

    [Flags]
    public enum Initflags : uint
    {
        Normal                  = 0x00000000,
        Liveupdate              = 0x00000001,
        AllowMissingPlugins   = 0x00000002,
        SynchronousUpdate      = 0x00000004,
        DeferredCallbacks      = 0x00000008,
        LoadFromUpdate        = 0x00000010,
        MemoryTracking         = 0x00000020,
    }

    [Flags]
    public enum LoadBankFlags : uint
    {
        Normal                  = 0x00000000,
        Nonblocking             = 0x00000001,
        DecompressSamples      = 0x00000002,
        Unencrypted             = 0x00000004,
    }

    [Flags]
    public enum CommandcaptureFlags : uint
    {
        Normal                  = 0x00000000,
        Fileflush               = 0x00000001,
        SkipInitialState      = 0x00000002,
    }

    [Flags]
    public enum CommandreplayFlags : uint
    {
        Normal                  = 0x00000000,
        SkipCleanup            = 0x00000001,
        FastForward            = 0x00000002,
        SkipBankLoad          = 0x00000004,
    }

    public enum PlaybackState : int
    {
        Playing,
        Sustaining,
        Stopped,
        Starting,
        Stopping,
    }

    public enum EventProperty : int
    {
        Channelpriority,
        ScheduleDelay,
        ScheduleLookahead,
        MinimumDistance,
        MaximumDistance,
        Cooldown,
        Max
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct PluginInstanceProperties
    {
        public IntPtr name;
        public IntPtr dsp;
    }

    [Flags]
    public enum EventCallbackType : uint
    {
        Created                  = 0x00000001,
        Destroyed                = 0x00000002,
        Starting                 = 0x00000004,
        Started                  = 0x00000008,
        Restarted                = 0x00000010,
        Stopped                  = 0x00000020,
        StartFailed             = 0x00000040,
        CreateProgrammerSound  = 0x00000080,
        DestroyProgrammerSound = 0x00000100,
        PluginCreated           = 0x00000200,
        PluginDestroyed         = 0x00000400,
        TimelineMarker          = 0x00000800,
        TimelineBeat            = 0x00001000,
        SoundPlayed             = 0x00002000,
        SoundStopped            = 0x00004000,
        RealToVirtual          = 0x00008000,
        VirtualToReal          = 0x00010000,
        StartEventCommand      = 0x00020000,
        NestedTimelineBeat     = 0x00040000,

        All                      = 0xFFFFFFFF,
    }

    public delegate Result EventCallback(EventCallbackType type, IntPtr @event, IntPtr parameters);

    public delegate Result CommandreplayFrameCallback(IntPtr replay, int commandindex, float currenttime, IntPtr userdata);
    public delegate Result CommandreplayLoadBankCallback(IntPtr replay, int commandindex, Guid bankguid, IntPtr bankfilename, LoadBankFlags flags, out IntPtr bank, IntPtr userdata);
    public delegate Result CommandreplayCreateInstanceCallback(IntPtr replay, int commandindex, IntPtr eventdescription, out IntPtr instance, IntPtr userdata);

    public enum Instancetype : int
    {
        None,
        System,
        Eventdescription,
        Eventinstance,
        Parameterinstance,
        Bus,
        Vca,
        Bank,
        Commandreplay,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CommandInfo
    {
        public StringWrapper commandname;
        public int parentcommandindex;
        public int framenumber;
        public float frametime;
        public Instancetype instancetype;
        public Instancetype outputtype;
        public UInt32 instancehandle;
        public UInt32 outputhandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryUsage
    {
        public int exclusive;
        public int inclusive;
        public int sampledata;
    }

    public struct Util
    {
        public static Result ParseId(string idString, out Guid id)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_ParseID(encoder.ByteFromStringUtf8(idString), out id);
            }
        }

        #region importfunctions
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_ParseID(byte[] idString, out Guid id);
        #endregion
    }

    public struct System
    {
        // Initialization / system functions.
        public static Result Create(out System system)
        {
            return FMOD_Studio_System_Create(out system.Handle, Version.Number);
        }
        public Result SetAdvancedSettings(Advancedsettings settings)
        {
            settings.cbsize = MarshalHelper.SizeOf(typeof(Advancedsettings));
            return FMOD_Studio_System_SetAdvancedSettings(this.Handle, ref settings);
        }
        public Result SetAdvancedSettings(Advancedsettings settings, string encryptionKey)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr userKey = settings.encryptionkey;
                settings.encryptionkey = encoder.IntptrFromStringUtf8(encryptionKey);
                FMOD.Result result = SetAdvancedSettings(settings);
                settings.encryptionkey = userKey;
                return result;
            }
        }
        public Result GetAdvancedSettings(out Advancedsettings settings)
        {
            settings.cbsize = MarshalHelper.SizeOf(typeof(Advancedsettings));
            return FMOD_Studio_System_GetAdvancedSettings(this.Handle, out settings);
        }
        public Result Initialize(int maxchannels, Initflags studioflags, FMOD.Initflags flags, IntPtr extradriverdata)
        {
            return FMOD_Studio_System_Initialize(this.Handle, maxchannels, studioflags, flags, extradriverdata);
        }
        public Result Release()
        {
            return FMOD_Studio_System_Release(this.Handle);
        }
        public Result Update()
        {
            return FMOD_Studio_System_Update(this.Handle);
        }
        public Result GetCoreSystem(out FMOD.System coresystem)
        {
            return FMOD_Studio_System_GetCoreSystem(this.Handle, out coresystem.Handle);
        }
        public Result GetEvent(string path, out EventDescription @event)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_GetEvent(this.Handle, encoder.ByteFromStringUtf8(path), out @event.Handle);
            }
        }
        public Result GetBus(string path, out Bus bus)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_GetBus(this.Handle, encoder.ByteFromStringUtf8(path), out bus.Handle);
            }
        }
        public Result GetVca(string path, out Vca vca)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_GetVCA(this.Handle, encoder.ByteFromStringUtf8(path), out vca.Handle);
            }
        }
        public Result GetBank(string path, out Bank bank)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_GetBank(this.Handle, encoder.ByteFromStringUtf8(path), out bank.Handle);
            }
        }

        public Result GetEventById(Guid id, out EventDescription @event)
        {
            return FMOD_Studio_System_GetEventByID(this.Handle, ref id, out @event.Handle);
        }
        public Result GetBusById(Guid id, out Bus bus)
        {
            return FMOD_Studio_System_GetBusByID(this.Handle, ref id, out bus.Handle);
        }
        public Result GetVcaById(Guid id, out Vca vca)
        {
            return FMOD_Studio_System_GetVCAByID(this.Handle, ref id, out vca.Handle);
        }
        public Result GetBankById(Guid id, out Bank bank)
        {
            return FMOD_Studio_System_GetBankByID(this.Handle, ref id, out bank.Handle);
        }
        public Result GetSoundInfo(string key, out SoundInfo info)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_GetSoundInfo(this.Handle, encoder.ByteFromStringUtf8(key), out info);
            }
        }
        public Result GetParameterDescriptionByName(string name, out ParameterDescription parameter)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_GetParameterDescriptionByName(this.Handle, encoder.ByteFromStringUtf8(name), out parameter);
            }
        }
        public Result GetParameterDescriptionById(ParameterId id, out ParameterDescription parameter)
        {
            return FMOD_Studio_System_GetParameterDescriptionByID(this.Handle, id, out parameter);
        }
        public Result GetParameterLabelByName(string name, int labelindex, out string label)
        {
            label = null;

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                int retrieved = 0;
                byte[] nameBytes = encoder.ByteFromStringUtf8(name);
                Result result = FMOD_Studio_System_GetParameterLabelByName(this.Handle, nameBytes, labelindex, stringMem, 256, out retrieved);

                if (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    result = FMOD_Studio_System_GetParameterLabelByName(this.Handle, nameBytes, labelindex, IntPtr.Zero, 0, out retrieved);
                    stringMem = Marshal.AllocHGlobal(retrieved);
                    result = FMOD_Studio_System_GetParameterLabelByName(this.Handle, nameBytes, labelindex, stringMem, retrieved, out retrieved);
                }

                if (result == Result.Ok)
                {
                    label = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }
        }
        public Result GetParameterLabelById(ParameterId id, int labelindex, out string label)
        {
            label = null;

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                int retrieved = 0;
                Result result = FMOD_Studio_System_GetParameterLabelByID(this.Handle, id, labelindex, stringMem, 256, out retrieved);

                if (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    result = FMOD_Studio_System_GetParameterLabelByID(this.Handle, id, labelindex, IntPtr.Zero, 0, out retrieved);
                    stringMem = Marshal.AllocHGlobal(retrieved);
                    result = FMOD_Studio_System_GetParameterLabelByID(this.Handle, id, labelindex, stringMem, retrieved, out retrieved);
                }

                if (result == Result.Ok)
                {
                    label = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }
        }
        public Result GetParameterById(ParameterId id, out float value)
        {
            float finalValue;
            return GetParameterById(id, out value, out finalValue);
        }
        public Result GetParameterById(ParameterId id, out float value, out float finalvalue)
        {
            return FMOD_Studio_System_GetParameterByID(this.Handle, id, out value, out finalvalue);
        }
        public Result SetParameterById(ParameterId id, float value, bool ignoreseekspeed = false)
        {
            return FMOD_Studio_System_SetParameterByID(this.Handle, id, value, ignoreseekspeed);
        }
        public Result SetParameterByIdWithLabel(ParameterId id, string label, bool ignoreseekspeed = false)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_SetParameterByIDWithLabel(this.Handle, id, encoder.ByteFromStringUtf8(label), ignoreseekspeed);
            }
        }
        public Result SetParametersByIDs(ParameterId[] ids, float[] values, int count, bool ignoreseekspeed = false)
        {
            return FMOD_Studio_System_SetParametersByIDs(this.Handle, ids, values, count, ignoreseekspeed);
        }
        public Result GetParameterByName(string name, out float value)
        {
            float finalValue;
            return GetParameterByName(name, out value, out finalValue);
        }
        public Result GetParameterByName(string name, out float value, out float finalvalue)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_GetParameterByName(this.Handle, encoder.ByteFromStringUtf8(name), out value, out finalvalue);
            }
        }
        public Result SetParameterByName(string name, float value, bool ignoreseekspeed = false)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_SetParameterByName(this.Handle, encoder.ByteFromStringUtf8(name), value, ignoreseekspeed);
            }
        }
        public Result SetParameterByNameWithLabel(string name, string label, bool ignoreseekspeed = false)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper(),
                                                   labelEncoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_SetParameterByNameWithLabel(this.Handle, encoder.ByteFromStringUtf8(name), labelEncoder.ByteFromStringUtf8(label), ignoreseekspeed);
            }
        }
        public Result LookupId(string path, out Guid id)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_LookupID(this.Handle, encoder.ByteFromStringUtf8(path), out id);
            }
        }
        public Result LookupPath(Guid id, out string path)
        {
            path = null;

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                int retrieved = 0;
                Result result = FMOD_Studio_System_LookupPath(this.Handle, ref id, stringMem, 256, out retrieved);

                if (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    stringMem = Marshal.AllocHGlobal(retrieved);
                    result = FMOD_Studio_System_LookupPath(this.Handle, ref id, stringMem, retrieved, out retrieved);
                }

                if (result == Result.Ok)
                {
                    path = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }
        }
        public Result GetNumListeners(out int numlisteners)
        {
            return FMOD_Studio_System_GetNumListeners(this.Handle, out numlisteners);
        }
        public Result SetNumListeners(int numlisteners)
        {
            return FMOD_Studio_System_SetNumListeners(this.Handle, numlisteners);
        }
        public Result GetListenerAttributes(int listener, out Attributes3D attributes)
        {
            return FMOD_Studio_System_GetListenerAttributes(this.Handle, listener, out attributes, IntPtr.Zero);
        }
        public Result GetListenerAttributes(int listener, out Attributes3D attributes, out Vector attenuationposition)
        {
            return FMOD_Studio_System_GetListenerAttributes(this.Handle, listener, out attributes, out attenuationposition);
        }
        public Result SetListenerAttributes(int listener, Attributes3D attributes)
        {
            return FMOD_Studio_System_SetListenerAttributes(this.Handle, listener, ref attributes, IntPtr.Zero);
        }
        public Result SetListenerAttributes(int listener, Attributes3D attributes, Vector attenuationposition)
        {
            return FMOD_Studio_System_SetListenerAttributes(this.Handle, listener, ref attributes, ref attenuationposition);
        }
        public Result GetListenerWeight(int listener, out float weight)
        {
            return FMOD_Studio_System_GetListenerWeight(this.Handle, listener, out weight);
        }
        public Result SetListenerWeight(int listener, float weight)
        {
            return FMOD_Studio_System_SetListenerWeight(this.Handle, listener, weight);
        }
        public Result LoadBankFile(string filename, LoadBankFlags flags, out Bank bank)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_LoadBankFile(this.Handle, encoder.ByteFromStringUtf8(filename), flags, out bank.Handle);
            }
        }
        public Result LoadBankMemory(byte[] buffer, LoadBankFlags flags, out Bank bank)
        {
            // Manually pin the byte array. It's what the marshaller should do anyway but don't leave it to chance.
            GCHandle pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            Result result = FMOD_Studio_System_LoadBankMemory(this.Handle, pointer, buffer.Length, LoadMemoryMode.LoadMemory, flags, out bank.Handle);
            pinnedArray.Free();
            return result;
        }
        public Result LoadBankCustom(BankInfo info, LoadBankFlags flags, out Bank bank)
        {
            info.size = MarshalHelper.SizeOf(typeof(BankInfo));
            return FMOD_Studio_System_LoadBankCustom(this.Handle, ref info, flags, out bank.Handle);
        }
        public Result UnloadAll()
        {
            return FMOD_Studio_System_UnloadAll(this.Handle);
        }
        public Result FlushCommands()
        {
            return FMOD_Studio_System_FlushCommands(this.Handle);
        }
        public Result FlushSampleLoading()
        {
            return FMOD_Studio_System_FlushSampleLoading(this.Handle);
        }
        public Result StartCommandCapture(string filename, CommandcaptureFlags flags)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_StartCommandCapture(this.Handle, encoder.ByteFromStringUtf8(filename), flags);
            }
        }
        public Result StopCommandCapture()
        {
            return FMOD_Studio_System_StopCommandCapture(this.Handle);
        }
        public Result LoadCommandReplay(string filename, CommandreplayFlags flags, out CommandReplay replay)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_System_LoadCommandReplay(this.Handle, encoder.ByteFromStringUtf8(filename), flags, out replay.Handle);
            }
        }
        public Result GetBankCount(out int count)
        {
            return FMOD_Studio_System_GetBankCount(this.Handle, out count);
        }
        public Result GetBankList(out Bank[] array)
        {
            array = null;

            Result result;
            int capacity;
            result = FMOD_Studio_System_GetBankCount(this.Handle, out capacity);
            if (result != Result.Ok)
            {
                return result;
            }
            if (capacity == 0)
            {
                array = new Bank[0];
                return result;
            }

            IntPtr[] rawArray = new IntPtr[capacity];
            int actualCount;
            result = FMOD_Studio_System_GetBankList(this.Handle, rawArray, capacity, out actualCount);
            if (result != Result.Ok)
            {
                return result;
            }
            if (actualCount > capacity) // More items added since we queried just now?
            {
                actualCount = capacity;
            }
            array = new Bank[actualCount];
            for (int i = 0; i < actualCount; ++i)
            {
                array[i].Handle = rawArray[i];
            }
            return Result.Ok;
        }
        public Result GetParameterDescriptionCount(out int count)
        {
            return FMOD_Studio_System_GetParameterDescriptionCount(this.Handle, out count);
        }
        public Result GetParameterDescriptionList(out ParameterDescription[] array)
        {
            array = null;

            int capacity;
            Result result = FMOD_Studio_System_GetParameterDescriptionCount(this.Handle, out capacity);
            if (result != Result.Ok)
            {
                return result;
            }
            if (capacity == 0)
            {
                array = new ParameterDescription[0];
                return Result.Ok;
            }

            ParameterDescription[] tempArray = new ParameterDescription[capacity];
            int actualCount;
            result = FMOD_Studio_System_GetParameterDescriptionList(this.Handle, tempArray, capacity, out actualCount);
            if (result != Result.Ok)
            {
                return result;
            }

            if (actualCount != capacity)
            {
                Array.Resize(ref tempArray, actualCount);
            }

            array = tempArray;

            return Result.Ok;
        }
        public Result GetCpuUsage(out CpuUsage usage, out FMOD.CpuUsage usageCore)
        {
            return FMOD_Studio_System_GetCPUUsage(this.Handle, out usage, out usageCore);
        }
        public Result GetBufferUsage(out BufferUsage usage)
        {
            return FMOD_Studio_System_GetBufferUsage(this.Handle, out usage);
        }
        public Result ResetBufferUsage()
        {
            return FMOD_Studio_System_ResetBufferUsage(this.Handle);
        }

        public Result SetCallback(SystemCallback callback, SystemCallbackType callbackmask = SystemCallbackType.All)
        {
            return FMOD_Studio_System_SetCallback(this.Handle, callback, callbackmask);
        }

        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD_Studio_System_GetUserData(this.Handle, out userdata);
        }

        public Result SetUserData(IntPtr userdata)
        {
            return FMOD_Studio_System_SetUserData(this.Handle, userdata);
        }

        public Result GetMemoryUsage(out MemoryUsage memoryusage)
        {
            return FMOD_Studio_System_GetMemoryUsage(this.Handle, out memoryusage);
        }

        #region importfunctions
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_Create                  (out IntPtr system, uint headerversion);
        [DllImport(StudioVersion.Dll)]
        private static extern bool   FMOD_Studio_System_IsValid                 (IntPtr system);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetAdvancedSettings     (IntPtr system, ref Advancedsettings settings);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetAdvancedSettings     (IntPtr system, out Advancedsettings settings);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_Initialize              (IntPtr system, int maxchannels, Initflags studioflags, FMOD.Initflags flags, IntPtr extradriverdata);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_Release                 (IntPtr system);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_Update                  (IntPtr system);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetCoreSystem           (IntPtr system, out IntPtr coresystem);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetEvent                (IntPtr system, byte[] path, out IntPtr @event);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetBus                  (IntPtr system, byte[] path, out IntPtr bus);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetVCA                  (IntPtr system, byte[] path, out IntPtr vca);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetBank                 (IntPtr system, byte[] path, out IntPtr bank);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetEventByID            (IntPtr system, ref Guid id, out IntPtr @event);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetBusByID              (IntPtr system, ref Guid id, out IntPtr bus);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetVCAByID              (IntPtr system, ref Guid id, out IntPtr vca);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetBankByID             (IntPtr system, ref Guid id, out IntPtr bank);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetSoundInfo            (IntPtr system, byte[] key, out SoundInfo info);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetParameterDescriptionByName(IntPtr system, byte[] name, out ParameterDescription parameter);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetParameterDescriptionByID(IntPtr system, ParameterId id, out ParameterDescription parameter);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetParameterLabelByName (IntPtr system, byte[] name, int labelindex, IntPtr label, int size, out int retrieved);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetParameterLabelByID   (IntPtr system, ParameterId id, int labelindex, IntPtr label, int size, out int retrieved);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetParameterByID        (IntPtr system, ParameterId id, out float value, out float finalvalue);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetParameterByID        (IntPtr system, ParameterId id, float value, bool ignoreseekspeed);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetParameterByIDWithLabel   (IntPtr system, ParameterId id, byte[] label, bool ignoreseekspeed);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetParametersByIDs      (IntPtr system, ParameterId[] ids, float[] values, int count, bool ignoreseekspeed);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetParameterByName      (IntPtr system, byte[] name, out float value, out float finalvalue);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetParameterByName      (IntPtr system, byte[] name, float value, bool ignoreseekspeed);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetParameterByNameWithLabel (IntPtr system, byte[] name, byte[] label, bool ignoreseekspeed);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_LookupID                (IntPtr system, byte[] path, out Guid id);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_LookupPath              (IntPtr system, ref Guid id, IntPtr path, int size, out int retrieved);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetNumListeners         (IntPtr system, out int numlisteners);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetNumListeners         (IntPtr system, int numlisteners);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetListenerAttributes   (IntPtr system, int listener, out Attributes3D attributes, IntPtr zero);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetListenerAttributes   (IntPtr system, int listener, out Attributes3D attributes, out Vector attenuationposition);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetListenerAttributes   (IntPtr system, int listener, ref Attributes3D attributes, IntPtr zero);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetListenerAttributes   (IntPtr system, int listener, ref Attributes3D attributes, ref Vector attenuationposition);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetListenerWeight       (IntPtr system, int listener, out float weight);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetListenerWeight       (IntPtr system, int listener, float weight);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_LoadBankFile            (IntPtr system, byte[] filename, LoadBankFlags flags, out IntPtr bank);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_LoadBankMemory          (IntPtr system, IntPtr buffer, int length, LoadMemoryMode mode, LoadBankFlags flags, out IntPtr bank);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_LoadBankCustom          (IntPtr system, ref BankInfo info, LoadBankFlags flags, out IntPtr bank);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_UnloadAll               (IntPtr system);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_FlushCommands           (IntPtr system);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_FlushSampleLoading      (IntPtr system);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_StartCommandCapture     (IntPtr system, byte[] filename, CommandcaptureFlags flags);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_StopCommandCapture      (IntPtr system);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_LoadCommandReplay       (IntPtr system, byte[] filename, CommandreplayFlags flags, out IntPtr replay);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetBankCount            (IntPtr system, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetBankList             (IntPtr system, IntPtr[] array, int capacity, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetParameterDescriptionCount(IntPtr system, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetParameterDescriptionList(IntPtr system, [Out] ParameterDescription[] array, int capacity, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetCPUUsage             (IntPtr system, out CpuUsage usage, out FMOD.CpuUsage usageCore);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetBufferUsage          (IntPtr system, out BufferUsage usage);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_ResetBufferUsage        (IntPtr system);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetCallback             (IntPtr system, SystemCallback callback, SystemCallbackType callbackmask);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetUserData             (IntPtr system, out IntPtr userdata);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_SetUserData             (IntPtr system, IntPtr userdata);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_System_GetMemoryUsage          (IntPtr system, out MemoryUsage memoryusage);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public System(IntPtr ptr)   { this.Handle = ptr; }
        public bool HasHandle()     { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()   { this.Handle = IntPtr.Zero; }

        public bool IsValid()
        {
            return HasHandle() && FMOD_Studio_System_IsValid(this.Handle);
        }

        #endregion
    }

    public struct EventDescription
    {
        public Result GetId(out Guid id)
        {
            return FMOD_Studio_EventDescription_GetID(this.Handle, out id);
        }
        public Result GetPath(out string path)
        {
            path = null;

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                int retrieved = 0;
                Result result = FMOD_Studio_EventDescription_GetPath(this.Handle, stringMem, 256, out retrieved);

                if (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    stringMem = Marshal.AllocHGlobal(retrieved);
                    result = FMOD_Studio_EventDescription_GetPath(this.Handle, stringMem, retrieved, out retrieved);
                }

                if (result == Result.Ok)
                {
                    path = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }
        }
        public Result GetParameterDescriptionCount(out int count)
        {
            return FMOD_Studio_EventDescription_GetParameterDescriptionCount(this.Handle, out count);
        }
        public Result GetParameterDescriptionByIndex(int index, out ParameterDescription parameter)
        {
            return FMOD_Studio_EventDescription_GetParameterDescriptionByIndex(this.Handle, index, out parameter);
        }
        public Result GetParameterDescriptionByName(string name, out ParameterDescription parameter)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_EventDescription_GetParameterDescriptionByName(this.Handle, encoder.ByteFromStringUtf8(name), out parameter);
            }
        }
        public Result GetParameterDescriptionById(ParameterId id, out ParameterDescription parameter)
        {
            return FMOD_Studio_EventDescription_GetParameterDescriptionByID(this.Handle, id, out parameter);
        }
        public Result GetParameterLabelByIndex(int index, int labelindex, out string label)
        {
            label = null;

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                int retrieved = 0;
                Result result = FMOD_Studio_EventDescription_GetParameterLabelByIndex(this.Handle, index, labelindex, stringMem, 256, out retrieved);

                if (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    result = FMOD_Studio_EventDescription_GetParameterLabelByIndex(this.Handle, index, labelindex, IntPtr.Zero, 0, out retrieved);
                    stringMem = Marshal.AllocHGlobal(retrieved);
                    result = FMOD_Studio_EventDescription_GetParameterLabelByIndex(this.Handle, index, labelindex, stringMem, retrieved, out retrieved);
                }

                if (result == Result.Ok)
                {
                    label = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }
        }
        public Result GetParameterLabelByName(string name, int labelindex, out string label)
        {
            label = null;

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                int retrieved = 0;
                byte[] nameBytes = encoder.ByteFromStringUtf8(name);
                Result result = FMOD_Studio_EventDescription_GetParameterLabelByName(this.Handle, nameBytes, labelindex, stringMem, 256, out retrieved);

                if (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    result = FMOD_Studio_EventDescription_GetParameterLabelByName(this.Handle, nameBytes, labelindex, IntPtr.Zero, 0, out retrieved);
                    stringMem = Marshal.AllocHGlobal(retrieved);
                    result = FMOD_Studio_EventDescription_GetParameterLabelByName(this.Handle, nameBytes, labelindex, stringMem, retrieved, out retrieved);
                }

                if (result == Result.Ok)
                {
                    label = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }
        }
        public Result GetParameterLabelById(ParameterId id, int labelindex, out string label)
        {
            label = null;

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                int retrieved = 0;
                Result result = FMOD_Studio_EventDescription_GetParameterLabelByID(this.Handle, id, labelindex, stringMem, 256, out retrieved);

                if (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    result = FMOD_Studio_EventDescription_GetParameterLabelByID(this.Handle, id, labelindex, IntPtr.Zero, 0, out retrieved);
                    stringMem = Marshal.AllocHGlobal(retrieved);
                    result = FMOD_Studio_EventDescription_GetParameterLabelByID(this.Handle, id, labelindex, stringMem, retrieved, out retrieved);
                }

                if (result == Result.Ok)
                {
                    label = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }
        }
        public Result GetUserPropertyCount(out int count)
        {
            return FMOD_Studio_EventDescription_GetUserPropertyCount(this.Handle, out count);
        }
        public Result GetUserPropertyByIndex(int index, out UserProperty property)
        {
            return FMOD_Studio_EventDescription_GetUserPropertyByIndex(this.Handle, index, out property);
        }
        public Result GetUserProperty(string name, out UserProperty property)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_EventDescription_GetUserProperty(this.Handle, encoder.ByteFromStringUtf8(name), out property);
            }
        }
        public Result GetLength(out int length)
        {
            return FMOD_Studio_EventDescription_GetLength(this.Handle, out length);
        }
        public Result GetMinMaxDistance(out float min, out float max)
        {
            return FMOD_Studio_EventDescription_GetMinMaxDistance(this.Handle, out min, out max);
        }
        public Result GetSoundSize(out float size)
        {
            return FMOD_Studio_EventDescription_GetSoundSize(this.Handle, out size);
        }
        public Result IsSnapshot(out bool snapshot)
        {
            return FMOD_Studio_EventDescription_IsSnapshot(this.Handle, out snapshot);
        }
        public Result isOneshot(out bool oneshot)
        {
            return FMOD_Studio_EventDescription_IsOneshot(this.Handle, out oneshot);
        }
        public Result IsStream(out bool isStream)
        {
            return FMOD_Studio_EventDescription_IsStream(this.Handle, out isStream);
        }
        public Result Is3D(out bool is3D)
        {
            return FMOD_Studio_EventDescription_Is3D(this.Handle, out is3D);
        }
        public Result IsDopplerEnabled(out bool doppler)
        {
            return FMOD_Studio_EventDescription_IsDopplerEnabled(this.Handle, out doppler);
        }
        public Result HasSustainPoint(out bool sustainPoint)
        {
            return FMOD_Studio_EventDescription_HasSustainPoint(this.Handle, out sustainPoint);
        }

        public Result CreateInstance(out EventInstance instance)
        {
            return FMOD_Studio_EventDescription_CreateInstance(this.Handle, out instance.Handle);
        }

        public Result GetInstanceCount(out int count)
        {
            return FMOD_Studio_EventDescription_GetInstanceCount(this.Handle, out count);
        }
        public Result GetInstanceList(out EventInstance[] array)
        {
            array = null;

            Result result;
            int capacity;
            result = FMOD_Studio_EventDescription_GetInstanceCount(this.Handle, out capacity);
            if (result != Result.Ok)
            {
                return result;
            }
            if (capacity == 0)
            {
                array = new EventInstance[0];
                return result;
            }

            IntPtr[] rawArray = new IntPtr[capacity];
            int actualCount;
            result = FMOD_Studio_EventDescription_GetInstanceList(this.Handle, rawArray, capacity, out actualCount);
            if (result != Result.Ok)
            {
                return result;
            }
            if (actualCount > capacity) // More items added since we queried just now?
            {
                actualCount = capacity;
            }
            array = new EventInstance[actualCount];
            for (int i = 0; i < actualCount; ++i)
            {
                array[i].Handle = rawArray[i];
            }
            return Result.Ok;
        }

        public Result LoadSampleData()
        {
            return FMOD_Studio_EventDescription_LoadSampleData(this.Handle);
        }

        public Result UnloadSampleData()
        {
            return FMOD_Studio_EventDescription_UnloadSampleData(this.Handle);
        }

        public Result GetSampleLoadingState(out LoadingState state)
        {
            return FMOD_Studio_EventDescription_GetSampleLoadingState(this.Handle, out state);
        }

        public Result ReleaseAllInstances()
        {
            return FMOD_Studio_EventDescription_ReleaseAllInstances(this.Handle);
        }
        public Result SetCallback(EventCallback callback, EventCallbackType callbackmask = EventCallbackType.All)
        {
            return FMOD_Studio_EventDescription_SetCallback(this.Handle, callback, callbackmask);
        }

        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD_Studio_EventDescription_GetUserData(this.Handle, out userdata);
        }

        public Result SetUserData(IntPtr userdata)
        {
            return FMOD_Studio_EventDescription_SetUserData(this.Handle, userdata);
        }

        #region importfunctions
        [DllImport(StudioVersion.Dll)]
        private static extern bool FMOD_Studio_EventDescription_IsValid                 (IntPtr eventdescription);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetID                 (IntPtr eventdescription, out Guid id);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetPath               (IntPtr eventdescription, IntPtr path, int size, out int retrieved);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetParameterDescriptionCount(IntPtr eventdescription, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetParameterDescriptionByIndex(IntPtr eventdescription, int index, out ParameterDescription parameter);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetParameterDescriptionByName(IntPtr eventdescription, byte[] name, out ParameterDescription parameter);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetParameterDescriptionByID(IntPtr eventdescription, ParameterId id, out ParameterDescription parameter);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetParameterLabelByIndex(IntPtr eventdescription, int index, int labelindex, IntPtr label, int size, out int retrieved);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetParameterLabelByName(IntPtr eventdescription, byte[] name, int labelindex, IntPtr label, int size, out int retrieved);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetParameterLabelByID (IntPtr eventdescription, ParameterId id, int labelindex, IntPtr label, int size, out int retrieved);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetUserPropertyCount  (IntPtr eventdescription, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetUserPropertyByIndex(IntPtr eventdescription, int index, out UserProperty property);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetUserProperty       (IntPtr eventdescription, byte[] name, out UserProperty property);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetLength             (IntPtr eventdescription, out int length);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetMinMaxDistance     (IntPtr eventdescription, out float min, out float max);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetSoundSize          (IntPtr eventdescription, out float size);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_IsSnapshot            (IntPtr eventdescription, out bool snapshot);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_IsOneshot             (IntPtr eventdescription, out bool oneshot);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_IsStream              (IntPtr eventdescription, out bool isStream);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_Is3D                  (IntPtr eventdescription, out bool is3D);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_IsDopplerEnabled      (IntPtr eventdescription, out bool doppler);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_HasSustainPoint       (IntPtr eventdescription, out bool sustainPoint);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_CreateInstance        (IntPtr eventdescription, out IntPtr instance);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetInstanceCount      (IntPtr eventdescription, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetInstanceList       (IntPtr eventdescription, IntPtr[] array, int capacity, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_LoadSampleData        (IntPtr eventdescription);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_UnloadSampleData      (IntPtr eventdescription);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetSampleLoadingState (IntPtr eventdescription, out LoadingState state);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_ReleaseAllInstances   (IntPtr eventdescription);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_SetCallback           (IntPtr eventdescription, EventCallback callback, EventCallbackType callbackmask);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_GetUserData           (IntPtr eventdescription, out IntPtr userdata);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventDescription_SetUserData           (IntPtr eventdescription, IntPtr userdata);
        #endregion
        #region wrapperinternal

        public IntPtr Handle;

        public EventDescription(IntPtr ptr) { this.Handle = ptr; }
        public bool HasHandle()             { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()           { this.Handle = IntPtr.Zero; }

        public bool IsValid()
        {
            return HasHandle() && FMOD_Studio_EventDescription_IsValid(this.Handle);
        }

        #endregion
    }

    public struct EventInstance
    {
        public Result GetDescription(out EventDescription description)
        {
            return FMOD_Studio_EventInstance_GetDescription(this.Handle, out description.Handle);
        }
        public Result GetVolume(out float volume)
        {
            return FMOD_Studio_EventInstance_GetVolume(this.Handle, out volume, IntPtr.Zero);
        }
        public Result GetVolume(out float volume, out float finalvolume)
        {
            return FMOD_Studio_EventInstance_GetVolume(this.Handle, out volume, out finalvolume);
        }
        public Result SetVolume(float volume)
        {
            return FMOD_Studio_EventInstance_SetVolume(this.Handle, volume);
        }
        public Result GetPitch(out float pitch)
        {
            return FMOD_Studio_EventInstance_GetPitch(this.Handle, out pitch, IntPtr.Zero);
        }
        public Result GetPitch(out float pitch, out float finalpitch)
        {
            return FMOD_Studio_EventInstance_GetPitch(this.Handle, out pitch, out finalpitch);
        }
        public Result SetPitch(float pitch)
        {
            return FMOD_Studio_EventInstance_SetPitch(this.Handle, pitch);
        }
        public Result Get3DAttributes(out Attributes3D attributes)
        {
            return FMOD_Studio_EventInstance_Get3DAttributes(this.Handle, out attributes);
        }
        public Result Set3DAttributes(Attributes3D attributes)
        {
            return FMOD_Studio_EventInstance_Set3DAttributes(this.Handle, ref attributes);
        }
        public Result GetListenerMask(out uint mask)
        {
            return FMOD_Studio_EventInstance_GetListenerMask(this.Handle, out mask);
        }
        public Result SetListenerMask(uint mask)
        {
            return FMOD_Studio_EventInstance_SetListenerMask(this.Handle, mask);
        }
        public Result GetProperty(EventProperty index, out float value)
        {
            return FMOD_Studio_EventInstance_GetProperty(this.Handle, index, out value);
        }
        public Result SetProperty(EventProperty index, float value)
        {
            return FMOD_Studio_EventInstance_SetProperty(this.Handle, index, value);
        }
        public Result GetReverbLevel(int index, out float level)
        {
            return FMOD_Studio_EventInstance_GetReverbLevel(this.Handle, index, out level);
        }
        public Result SetReverbLevel(int index, float level)
        {
            return FMOD_Studio_EventInstance_SetReverbLevel(this.Handle, index, level);
        }
        public Result GetPaused(out bool paused)
        {
            return FMOD_Studio_EventInstance_GetPaused(this.Handle, out paused);
        }
        public Result SetPaused(bool paused)
        {
            return FMOD_Studio_EventInstance_SetPaused(this.Handle, paused);
        }
        public Result Start()
        {
            return FMOD_Studio_EventInstance_Start(this.Handle);
        }
        public Result Stop(StopMode mode)
        {
            return FMOD_Studio_EventInstance_Stop(this.Handle, mode);
        }
        public Result GetTimelinePosition(out int position)
        {
            return FMOD_Studio_EventInstance_GetTimelinePosition(this.Handle, out position);
        }
        public Result SetTimelinePosition(int position)
        {
            return FMOD_Studio_EventInstance_SetTimelinePosition(this.Handle, position);
        }
        public Result GetPlaybackState(out PlaybackState state)
        {
            return FMOD_Studio_EventInstance_GetPlaybackState(this.Handle, out state);
        }
        public Result GetChannelGroup(out FMOD.ChannelGroup group)
        {
            return FMOD_Studio_EventInstance_GetChannelGroup(this.Handle, out group.Handle);
        }
        public Result GetMinMaxDistance(out float min, out float max)
        {
            return FMOD_Studio_EventInstance_GetMinMaxDistance(this.Handle, out min, out max);
        }
        public Result Release()
        {
            return FMOD_Studio_EventInstance_Release(this.Handle);
        }
        public Result IsVirtual(out bool virtualstate)
        {
            return FMOD_Studio_EventInstance_IsVirtual(this.Handle, out virtualstate);
        }
        public Result GetParameterById(ParameterId id, out float value)
        {
            float finalvalue;
            return GetParameterById(id, out value, out finalvalue);
        }
        public Result GetParameterById(ParameterId id, out float value, out float finalvalue)
        {
            return FMOD_Studio_EventInstance_GetParameterByID(this.Handle, id, out value, out finalvalue);
        }
        public Result SetParameterById(ParameterId id, float value, bool ignoreseekspeed = false)
        {
            return FMOD_Studio_EventInstance_SetParameterByID(this.Handle, id, value, ignoreseekspeed);
        }
        public Result SetParameterByIdWithLabel(ParameterId id, string label, bool ignoreseekspeed = false)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_EventInstance_SetParameterByIDWithLabel(this.Handle, id, encoder.ByteFromStringUtf8(label), ignoreseekspeed);
            }
        }
        public Result SetParametersByIDs(ParameterId[] ids, float[] values, int count, bool ignoreseekspeed = false)
        {
            return FMOD_Studio_EventInstance_SetParametersByIDs(this.Handle, ids, values, count, ignoreseekspeed);
        }
        public Result GetParameterByName(string name, out float value)
        {
            float finalValue;
            return GetParameterByName(name, out value, out finalValue);
        }
        public Result GetParameterByName(string name, out float value, out float finalvalue)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_EventInstance_GetParameterByName(this.Handle, encoder.ByteFromStringUtf8(name), out value, out finalvalue);
            }
        }
        public Result SetParameterByName(string name, float value, bool ignoreseekspeed = false)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_EventInstance_SetParameterByName(this.Handle, encoder.ByteFromStringUtf8(name), value, ignoreseekspeed);
            }
        }
        public Result SetParameterByNameWithLabel(string name, string label, bool ignoreseekspeed = false)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper(),
                                                   labelEncoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_EventInstance_SetParameterByNameWithLabel(this.Handle, encoder.ByteFromStringUtf8(name), labelEncoder.ByteFromStringUtf8(label), ignoreseekspeed);
            }
        }
        public Result KeyOff()
        {
            return FMOD_Studio_EventInstance_KeyOff(this.Handle);
        }
        public Result SetCallback(EventCallback callback, EventCallbackType callbackmask = EventCallbackType.All)
        {
            return FMOD_Studio_EventInstance_SetCallback(this.Handle, callback, callbackmask);
        }
        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD_Studio_EventInstance_GetUserData(this.Handle, out userdata);
        }
        public Result SetUserData(IntPtr userdata)
        {
            return FMOD_Studio_EventInstance_SetUserData(this.Handle, userdata);
        }
        public Result GetCpuUsage(out uint exclusive, out uint inclusive)
        {
            return FMOD_Studio_EventInstance_GetCPUUsage(this.Handle, out exclusive, out inclusive);
        }
        public Result GetMemoryUsage(out MemoryUsage memoryusage)
        {
            return FMOD_Studio_EventInstance_GetMemoryUsage(this.Handle, out memoryusage);
        }
        #region importfunctions
        [DllImport(StudioVersion.Dll)]
        private static extern bool   FMOD_Studio_EventInstance_IsValid                     (IntPtr @event);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetDescription              (IntPtr @event, out IntPtr description);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetVolume                   (IntPtr @event, out float volume, IntPtr zero);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetVolume                   (IntPtr @event, out float volume, out float finalvolume);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetVolume                   (IntPtr @event, float volume);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetPitch                    (IntPtr @event, out float pitch, IntPtr zero);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetPitch                    (IntPtr @event, out float pitch, out float finalpitch);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetPitch                    (IntPtr @event, float pitch);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_Get3DAttributes             (IntPtr @event, out Attributes3D attributes);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_Set3DAttributes             (IntPtr @event, ref Attributes3D attributes);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetListenerMask             (IntPtr @event, out uint mask);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetListenerMask             (IntPtr @event, uint mask);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetProperty                 (IntPtr @event, EventProperty index, out float value);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetProperty                 (IntPtr @event, EventProperty index, float value);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetReverbLevel              (IntPtr @event, int index, out float level);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetReverbLevel              (IntPtr @event, int index, float level);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetPaused                   (IntPtr @event, out bool paused);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetPaused                   (IntPtr @event, bool paused);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_Start                       (IntPtr @event);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_Stop                        (IntPtr @event, StopMode mode);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetTimelinePosition         (IntPtr @event, out int position);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetTimelinePosition         (IntPtr @event, int position);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetPlaybackState            (IntPtr @event, out PlaybackState state);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetChannelGroup             (IntPtr @event, out IntPtr group);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetMinMaxDistance           (IntPtr @event, out float min, out float max);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_Release                     (IntPtr @event);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_IsVirtual                   (IntPtr @event, out bool virtualstate);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetParameterByName          (IntPtr @event, byte[] name, out float value, out float finalvalue);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetParameterByName          (IntPtr @event, byte[] name, float value, bool ignoreseekspeed);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetParameterByNameWithLabel (IntPtr @event, byte[] name, byte[] label, bool ignoreseekspeed);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetParameterByID            (IntPtr @event, ParameterId id, out float value, out float finalvalue);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetParameterByID            (IntPtr @event, ParameterId id, float value, bool ignoreseekspeed);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetParameterByIDWithLabel   (IntPtr @event, ParameterId id, byte[] label, bool ignoreseekspeed);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetParametersByIDs          (IntPtr @event, ParameterId[] ids, float[] values, int count, bool ignoreseekspeed);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_KeyOff                      (IntPtr @event);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetCallback                 (IntPtr @event, EventCallback callback, EventCallbackType callbackmask);
        [DllImport (StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetUserData                 (IntPtr @event, out IntPtr userdata);
        [DllImport (StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_SetUserData                 (IntPtr @event, IntPtr userdata);
        [DllImport (StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetCPUUsage                 (IntPtr @event, out uint exclusive, out uint inclusive);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_EventInstance_GetMemoryUsage              (IntPtr @event, out MemoryUsage memoryusage);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public EventInstance(IntPtr ptr) { this.Handle = ptr; }
        public bool HasHandle()          { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()        { this.Handle = IntPtr.Zero; }

        public bool IsValid()
        {
            return HasHandle() && FMOD_Studio_EventInstance_IsValid(this.Handle);
        }

        #endregion
    }

    public struct Bus
    {
        public Result GetId(out Guid id)
        {
            return FMOD_Studio_Bus_GetID(this.Handle, out id);
        }
        public Result GetPath(out string path)
        {
            path = null;

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                int retrieved = 0;
                Result result = FMOD_Studio_Bus_GetPath(this.Handle, stringMem, 256, out retrieved);

                if (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    stringMem = Marshal.AllocHGlobal(retrieved);
                    result = FMOD_Studio_Bus_GetPath(this.Handle, stringMem, retrieved, out retrieved);
                }

                if (result == Result.Ok)
                {
                    path = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }

        }
        public Result GetVolume(out float volume)
        {
            float finalVolume;
            return GetVolume(out volume, out finalVolume);
        }
        public Result GetVolume(out float volume, out float finalvolume)
        {
            return FMOD_Studio_Bus_GetVolume(this.Handle, out volume, out finalvolume);
        }
        public Result SetVolume(float volume)
        {
            return FMOD_Studio_Bus_SetVolume(this.Handle, volume);
        }
        public Result GetPaused(out bool paused)
        {
            return FMOD_Studio_Bus_GetPaused(this.Handle, out paused);
        }
        public Result SetPaused(bool paused)
        {
            return FMOD_Studio_Bus_SetPaused(this.Handle, paused);
        }
        public Result GetMute(out bool mute)
        {
            return FMOD_Studio_Bus_GetMute(this.Handle, out mute);
        }
        public Result SetMute(bool mute)
        {
            return FMOD_Studio_Bus_SetMute(this.Handle, mute);
        }
        public Result StopAllEvents(StopMode mode)
        {
            return FMOD_Studio_Bus_StopAllEvents(this.Handle, mode);
        }
        public Result LockChannelGroup()
        {
            return FMOD_Studio_Bus_LockChannelGroup(this.Handle);
        }
        public Result UnlockChannelGroup()
        {
            return FMOD_Studio_Bus_UnlockChannelGroup(this.Handle);
        }
        public Result GetChannelGroup(out FMOD.ChannelGroup group)
        {
            return FMOD_Studio_Bus_GetChannelGroup(this.Handle, out group.Handle);
        }
        public Result GetCpuUsage(out uint exclusive, out uint inclusive)
        {
            return FMOD_Studio_Bus_GetCPUUsage(this.Handle, out exclusive, out inclusive);
        }
        public Result GetMemoryUsage(out MemoryUsage memoryusage)
        {
            return FMOD_Studio_Bus_GetMemoryUsage(this.Handle, out memoryusage);
        }
        public Result GetPortIndex(out ulong index)
        {
            return FMOD_Studio_Bus_GetPortIndex(this.Handle, out index);
        }
        public Result SetPortIndex(ulong index)
        {
            return FMOD_Studio_Bus_SetPortIndex(this.Handle, index);
        }

        #region importfunctions
        [DllImport(StudioVersion.Dll)]
        private static extern bool   FMOD_Studio_Bus_IsValid              (IntPtr bus);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_GetID                (IntPtr bus, out Guid id);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_GetPath              (IntPtr bus, IntPtr path, int size, out int retrieved);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_GetVolume            (IntPtr bus, out float volume, out float finalvolume);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_SetVolume            (IntPtr bus, float volume);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_GetPaused            (IntPtr bus, out bool paused);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_SetPaused            (IntPtr bus, bool paused);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_GetMute              (IntPtr bus, out bool mute);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_SetMute              (IntPtr bus, bool mute);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_StopAllEvents        (IntPtr bus, StopMode mode);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_LockChannelGroup     (IntPtr bus);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_UnlockChannelGroup   (IntPtr bus);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_GetChannelGroup      (IntPtr bus, out IntPtr group);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_GetCPUUsage          (IntPtr bus, out uint exclusive, out uint inclusive);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_GetMemoryUsage       (IntPtr bus, out MemoryUsage memoryusage);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_GetPortIndex         (IntPtr bus, out ulong index);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bus_SetPortIndex         (IntPtr bus, ulong index);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public Bus(IntPtr ptr)      { this.Handle = ptr; }
        public bool HasHandle()     { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()   { this.Handle = IntPtr.Zero; }

        public bool IsValid()
        {
            return HasHandle() && FMOD_Studio_Bus_IsValid(this.Handle);
        }

        #endregion
    }

    public struct Vca
    {
        public Result GetId(out Guid id)
        {
            return FMOD_Studio_VCA_GetID(this.Handle, out id);
        }
        public Result GetPath(out string path)
        {
            path = null;

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                int retrieved = 0;
                Result result = FMOD_Studio_VCA_GetPath(this.Handle, stringMem, 256, out retrieved);

                if (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    stringMem = Marshal.AllocHGlobal(retrieved);
                    result = FMOD_Studio_VCA_GetPath(this.Handle, stringMem, retrieved, out retrieved);
                }

                if (result == Result.Ok)
                {
                    path = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }
        }
        public Result GetVolume(out float volume)
        {
            float finalVolume;
            return GetVolume(out volume, out finalVolume);
        }
        public Result GetVolume(out float volume, out float finalvolume)
        {
            return FMOD_Studio_VCA_GetVolume(this.Handle, out volume, out finalvolume);
        }
        public Result SetVolume(float volume)
        {
            return FMOD_Studio_VCA_SetVolume(this.Handle, volume);
        }

        #region importfunctions
        [DllImport(StudioVersion.Dll)]
        private static extern bool   FMOD_Studio_VCA_IsValid       (IntPtr vca);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_VCA_GetID         (IntPtr vca, out Guid id);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_VCA_GetPath       (IntPtr vca, IntPtr path, int size, out int retrieved);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_VCA_GetVolume     (IntPtr vca, out float volume, out float finalvolume);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_VCA_SetVolume     (IntPtr vca, float volume);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public Vca(IntPtr ptr)      { this.Handle = ptr; }
        public bool HasHandle()     { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()   { this.Handle = IntPtr.Zero; }

        public bool IsValid()
        {
            return HasHandle() && FMOD_Studio_VCA_IsValid(this.Handle);
        }

        #endregion
    }

    public struct Bank
    {
        // Property access

        public Result GetId(out Guid id)
        {
            return FMOD_Studio_Bank_GetID(this.Handle, out id);
        }
        public Result GetPath(out string path)
        {
            path = null;

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                int retrieved = 0;
                Result result = FMOD_Studio_Bank_GetPath(this.Handle, stringMem, 256, out retrieved);

                if (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    stringMem = Marshal.AllocHGlobal(retrieved);
                    result = FMOD_Studio_Bank_GetPath(this.Handle, stringMem, retrieved, out retrieved);
                }

                if (result == Result.Ok)
                {
                    path = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }
        }
        public Result Unload()
        {
            return FMOD_Studio_Bank_Unload(this.Handle);
        }
        public Result LoadSampleData()
        {
            return FMOD_Studio_Bank_LoadSampleData(this.Handle);
        }
        public Result UnloadSampleData()
        {
            return FMOD_Studio_Bank_UnloadSampleData(this.Handle);
        }
        public Result GetLoadingState(out LoadingState state)
        {
            return FMOD_Studio_Bank_GetLoadingState(this.Handle, out state);
        }
        public Result GetSampleLoadingState(out LoadingState state)
        {
            return FMOD_Studio_Bank_GetSampleLoadingState(this.Handle, out state);
        }

        // Enumeration
        public Result GetStringCount(out int count)
        {
            return FMOD_Studio_Bank_GetStringCount(this.Handle, out count);
        }
        public Result GetStringInfo(int index, out Guid id, out string path)
        {
            path = null;
            id = new Guid();

            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                int retrieved = 0;
                Result result = FMOD_Studio_Bank_GetStringInfo(this.Handle, index, out id, stringMem, 256, out retrieved);

                if (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    stringMem = Marshal.AllocHGlobal(retrieved);
                    result = FMOD_Studio_Bank_GetStringInfo(this.Handle, index, out id, stringMem, retrieved, out retrieved);
                }

                if (result == Result.Ok)
                {
                    path = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }
        }

        public Result GetEventCount(out int count)
        {
            return FMOD_Studio_Bank_GetEventCount(this.Handle, out count);
        }
        public Result GetEventList(out EventDescription[] array)
        {
            array = null;

            Result result;
            int capacity;
            result = FMOD_Studio_Bank_GetEventCount(this.Handle, out capacity);
            if (result != Result.Ok)
            {
                return result;
            }
            if (capacity == 0)
            {
                array = new EventDescription[0];
                return result;
            }

            IntPtr[] rawArray = new IntPtr[capacity];
            int actualCount;
            result = FMOD_Studio_Bank_GetEventList(this.Handle, rawArray, capacity, out actualCount);
            if (result != Result.Ok)
            {
                return result;
            }
            if (actualCount > capacity) // More items added since we queried just now?
            {
                actualCount = capacity;
            }
            array = new EventDescription[actualCount];
            for (int i = 0; i < actualCount; ++i)
            {
                array[i].Handle = rawArray[i];
            }
            return Result.Ok;
        }
        public Result GetBusCount(out int count)
        {
            return FMOD_Studio_Bank_GetBusCount(this.Handle, out count);
        }
        public Result GetBusList(out Bus[] array)
        {
            array = null;

            Result result;
            int capacity;
            result = FMOD_Studio_Bank_GetBusCount(this.Handle, out capacity);
            if (result != Result.Ok)
            {
                return result;
            }
            if (capacity == 0)
            {
                array = new Bus[0];
                return result;
            }

            IntPtr[] rawArray = new IntPtr[capacity];
            int actualCount;
            result = FMOD_Studio_Bank_GetBusList(this.Handle, rawArray, capacity, out actualCount);
            if (result != Result.Ok)
            {
                return result;
            }
            if (actualCount > capacity) // More items added since we queried just now?
            {
                actualCount = capacity;
            }
            array = new Bus[actualCount];
            for (int i = 0; i < actualCount; ++i)
            {
                array[i].Handle = rawArray[i];
            }
            return Result.Ok;
        }
        public Result GetVcaCount(out int count)
        {
            return FMOD_Studio_Bank_GetVCACount(this.Handle, out count);
        }
        public Result GetVcaList(out Vca[] array)
        {
            array = null;

            Result result;
            int capacity;
            result = FMOD_Studio_Bank_GetVCACount(this.Handle, out capacity);
            if (result != Result.Ok)
            {
                return result;
            }
            if (capacity == 0)
            {
                array = new Vca[0];
                return result;
            }

            IntPtr[] rawArray = new IntPtr[capacity];
            int actualCount;
            result = FMOD_Studio_Bank_GetVCAList(this.Handle, rawArray, capacity, out actualCount);
            if (result != Result.Ok)
            {
                return result;
            }
            if (actualCount > capacity) // More items added since we queried just now?
            {
                actualCount = capacity;
            }
            array = new Vca[actualCount];
            for (int i = 0; i < actualCount; ++i)
            {
                array[i].Handle = rawArray[i];
            }
            return Result.Ok;
        }

        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD_Studio_Bank_GetUserData(this.Handle, out userdata);
        }

        public Result SetUserData(IntPtr userdata)
        {
            return FMOD_Studio_Bank_SetUserData(this.Handle, userdata);
        }

        #region importfunctions
        [DllImport(StudioVersion.Dll)]
        private static extern bool   FMOD_Studio_Bank_IsValid                   (IntPtr bank);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetID                     (IntPtr bank, out Guid id);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetPath                   (IntPtr bank, IntPtr path, int size, out int retrieved);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_Unload                    (IntPtr bank);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_LoadSampleData            (IntPtr bank);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_UnloadSampleData          (IntPtr bank);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetLoadingState           (IntPtr bank, out LoadingState state);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetSampleLoadingState     (IntPtr bank, out LoadingState state);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetStringCount            (IntPtr bank, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetStringInfo             (IntPtr bank, int index, out Guid id, IntPtr path, int size, out int retrieved);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetEventCount             (IntPtr bank, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetEventList              (IntPtr bank, IntPtr[] array, int capacity, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetBusCount               (IntPtr bank, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetBusList                (IntPtr bank, IntPtr[] array, int capacity, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetVCACount               (IntPtr bank, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetVCAList                (IntPtr bank, IntPtr[] array, int capacity, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_GetUserData               (IntPtr bank, out IntPtr userdata);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_Bank_SetUserData               (IntPtr bank, IntPtr userdata);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public Bank(IntPtr ptr)     { this.Handle = ptr; }
        public bool HasHandle()     { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()   { this.Handle = IntPtr.Zero; }

        public bool IsValid()
        {
            return HasHandle() && FMOD_Studio_Bank_IsValid(this.Handle);
        }

        #endregion
    }

    public struct CommandReplay
    {
        // Information query
        public Result GetSystem(out System system)
        {
            return FMOD_Studio_CommandReplay_GetSystem(this.Handle, out system.Handle);
        }

        public Result GetLength(out float length)
        {
            return FMOD_Studio_CommandReplay_GetLength(this.Handle, out length);
        }
        public Result GetCommandCount(out int count)
        {
            return FMOD_Studio_CommandReplay_GetCommandCount(this.Handle, out count);
        }
        public Result GetCommandInfo(int commandIndex, out CommandInfo info)
        {
            return FMOD_Studio_CommandReplay_GetCommandInfo(this.Handle, commandIndex, out info);
        }

        public Result GetCommandString(int commandIndex, out string buffer)
        {
            buffer = null;
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                int stringLength = 256;
                IntPtr stringMem = Marshal.AllocHGlobal(256);
                Result result = FMOD_Studio_CommandReplay_GetCommandString(this.Handle, commandIndex, stringMem, stringLength);

                while (result == Result.ErrTruncated)
                {
                    Marshal.FreeHGlobal(stringMem);
                    stringLength *= 2;
                    stringMem = Marshal.AllocHGlobal(stringLength);
                    result = FMOD_Studio_CommandReplay_GetCommandString(this.Handle, commandIndex, stringMem, stringLength);
                }

                if (result == Result.Ok)
                {
                    buffer = encoder.StringFromNative(stringMem);
                }
                Marshal.FreeHGlobal(stringMem);
                return result;
            }
        }
        public Result GetCommandAtTime(float time, out int commandIndex)
        {
            return FMOD_Studio_CommandReplay_GetCommandAtTime(this.Handle, time, out commandIndex);
        }
        // Playback
        public Result SetBankPath(string bankPath)
        {
            using (StringHelper.ThreadSafeEncoding encoder = StringHelper.GetFreeHelper())
            {
                return FMOD_Studio_CommandReplay_SetBankPath(this.Handle, encoder.ByteFromStringUtf8(bankPath));
            }
        }
        public Result Start()
        {
            return FMOD_Studio_CommandReplay_Start(this.Handle);
        }
        public Result Stop()
        {
            return FMOD_Studio_CommandReplay_Stop(this.Handle);
        }
        public Result SeekToTime(float time)
        {
            return FMOD_Studio_CommandReplay_SeekToTime(this.Handle, time);
        }
        public Result SeekToCommand(int commandIndex)
        {
            return FMOD_Studio_CommandReplay_SeekToCommand(this.Handle, commandIndex);
        }
        public Result GetPaused(out bool paused)
        {
            return FMOD_Studio_CommandReplay_GetPaused(this.Handle, out paused);
        }
        public Result SetPaused(bool paused)
        {
            return FMOD_Studio_CommandReplay_SetPaused(this.Handle, paused);
        }
        public Result GetPlaybackState(out PlaybackState state)
        {
            return FMOD_Studio_CommandReplay_GetPlaybackState(this.Handle, out state);
        }
        public Result GetCurrentCommand(out int commandIndex, out float currentTime)
        {
            return FMOD_Studio_CommandReplay_GetCurrentCommand(this.Handle, out commandIndex, out currentTime);
        }
        // Release
        public Result Release()
        {
            return FMOD_Studio_CommandReplay_Release(this.Handle);
        }
        // Callbacks
        public Result SetFrameCallback(CommandreplayFrameCallback callback)
        {
            return FMOD_Studio_CommandReplay_SetFrameCallback(this.Handle, callback);
        }
        public Result SetLoadBankCallback(CommandreplayLoadBankCallback callback)
        {
            return FMOD_Studio_CommandReplay_SetLoadBankCallback(this.Handle, callback);
        }
        public Result SetCreateInstanceCallback(CommandreplayCreateInstanceCallback callback)
        {
            return FMOD_Studio_CommandReplay_SetCreateInstanceCallback(this.Handle, callback);
        }
        public Result GetUserData(out IntPtr userdata)
        {
            return FMOD_Studio_CommandReplay_GetUserData(this.Handle, out userdata);
        }
        public Result SetUserData(IntPtr userdata)
        {
            return FMOD_Studio_CommandReplay_SetUserData(this.Handle, userdata);
        }

        #region importfunctions
        [DllImport(StudioVersion.Dll)]
        private static extern bool FMOD_Studio_CommandReplay_IsValid                    (IntPtr replay);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_GetSystem                (IntPtr replay, out IntPtr system);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_GetLength                (IntPtr replay, out float length);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_GetCommandCount          (IntPtr replay, out int count);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_GetCommandInfo           (IntPtr replay, int commandindex, out CommandInfo info);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_GetCommandString         (IntPtr replay, int commandIndex, IntPtr buffer, int length);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_GetCommandAtTime         (IntPtr replay, float time, out int commandIndex);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_SetBankPath              (IntPtr replay, byte[] bankPath);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_Start                    (IntPtr replay);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_Stop                     (IntPtr replay);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_SeekToTime               (IntPtr replay, float time);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_SeekToCommand            (IntPtr replay, int commandIndex);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_GetPaused                (IntPtr replay, out bool paused);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_SetPaused                (IntPtr replay, bool paused);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_GetPlaybackState         (IntPtr replay, out PlaybackState state);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_GetCurrentCommand        (IntPtr replay, out int commandIndex, out float currentTime);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_Release                  (IntPtr replay);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_SetFrameCallback         (IntPtr replay, CommandreplayFrameCallback callback);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_SetLoadBankCallback      (IntPtr replay, CommandreplayLoadBankCallback callback);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_SetCreateInstanceCallback(IntPtr replay, CommandreplayCreateInstanceCallback callback);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_GetUserData              (IntPtr replay, out IntPtr userdata);
        [DllImport(StudioVersion.Dll)]
        private static extern Result FMOD_Studio_CommandReplay_SetUserData              (IntPtr replay, IntPtr userdata);
        #endregion

        #region wrapperinternal

        public IntPtr Handle;

        public CommandReplay(IntPtr ptr) { this.Handle = ptr; }
        public bool HasHandle()          { return this.Handle != IntPtr.Zero; }
        public void ClearHandle()        { this.Handle = IntPtr.Zero; }

        public bool IsValid()
        {
            return HasHandle() && FMOD_Studio_CommandReplay_IsValid(this.Handle);
        }

        #endregion
    }
} // FMOD
