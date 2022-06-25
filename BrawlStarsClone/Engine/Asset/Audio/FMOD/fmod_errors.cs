/* ==============================================================================================  */
/* FMOD Core / Studio API - Error string header file.                                              */
/* Copyright (c), Firelight Technologies Pty, Ltd. 2004-2022.                                      */
/*                                                                                                 */
/* Use this header if you want to store or display a string version / english explanation          */
/* of the FMOD error codes.                                                                        */
/*                                                                                                 */
/* For more detail visit:                                                                          */
/* https://fmod.com/resources/documentation-api?version=2.0&page=core-api-common.html#fmod_result  */
/* =============================================================================================== */

namespace FMOD
{
    public class Error
    {
        public static string String(FMOD.Result errcode)
        {
            switch (errcode)
            {
                case FMOD.Result.Ok:                            return "No errors.";
                case FMOD.Result.ErrBadcommand:                return "Tried to call a function on a data type that does not allow this type of functionality (ie calling Sound::lock on a streaming sound).";
                case FMOD.Result.ErrChannelAlloc:             return "Error trying to allocate a channel.";
                case FMOD.Result.ErrChannelStolen:            return "The specified channel has been reused to play another sound.";
                case FMOD.Result.ErrDma:                       return "DMA Failure.  See debug output for more information.";
                case FMOD.Result.ErrDspConnection:            return "DSP connection error.  Connection possibly caused a cyclic dependency or connected dsps with incompatible buffer counts.";
                case FMOD.Result.ErrDspDontprocess:           return "DSP return code from a DSP process query callback.  Tells mixer not to call the process callback and therefore not consume CPU.  Use this to optimize the DSP graph.";
                case FMOD.Result.ErrDspFormat:                return "DSP Format error.  A DSP unit may have attempted to connect to this network with the wrong format, or a matrix may have been set with the wrong size if the target unit has a specified channel map.";
                case FMOD.Result.ErrDspInuse:                 return "DSP is already in the mixer's DSP network. It must be removed before being reinserted or released.";
                case FMOD.Result.ErrDspNotfound:              return "DSP connection error.  Couldn't find the DSP unit specified.";
                case FMOD.Result.ErrDspReserved:              return "DSP operation error.  Cannot perform operation on this DSP as it is reserved by the system.";
                case FMOD.Result.ErrDspSilence:               return "DSP return code from a DSP process query callback.  Tells mixer silence would be produced from read, so go idle and not consume CPU.  Use this to optimize the DSP graph.";
                case FMOD.Result.ErrDspType:                  return "DSP operation cannot be performed on a DSP of this type.";
                case FMOD.Result.ErrFileBad:                  return "Error loading file.";
                case FMOD.Result.ErrFileCouldnotseek:         return "Couldn't perform seek operation.  This is a limitation of the medium (ie netstreams) or the file format.";
                case FMOD.Result.ErrFileDiskejected:          return "Media was ejected while reading.";
                case FMOD.Result.ErrFileEof:                  return "End of file unexpectedly reached while trying to read essential data (truncated?).";
                case FMOD.Result.ErrFileEndofdata:            return "End of current chunk reached while trying to read data.";
                case FMOD.Result.ErrFileNotfound:             return "File not found.";
                case FMOD.Result.ErrFormat:                    return "Unsupported file or audio format.";
                case FMOD.Result.ErrHeaderMismatch:           return "There is a version mismatch between the FMOD header and either the FMOD Studio library or the FMOD Low Level library.";
                case FMOD.Result.ErrHttp:                      return "A HTTP error occurred. This is a catch-all for HTTP errors not listed elsewhere.";
                case FMOD.Result.ErrHttpAccess:               return "The specified resource requires authentication or is forbidden.";
                case FMOD.Result.ErrHttpProxyAuth:           return "Proxy authentication is required to access the specified resource.";
                case FMOD.Result.ErrHttpServerError:         return "A HTTP server error occurred.";
                case FMOD.Result.ErrHttpTimeout:              return "The HTTP request timed out.";
                case FMOD.Result.ErrInitialization:            return "FMOD was not initialized correctly to support this function.";
                case FMOD.Result.ErrInitialized:               return "Cannot call this command after System::init.";
                case FMOD.Result.ErrInternal:                  return "An error occurred that wasn't supposed to.  Contact support.";
                case FMOD.Result.ErrInvalidFloat:             return "Value passed in was a NaN, Inf or denormalized float.";
                case FMOD.Result.ErrInvalidHandle:            return "An invalid object handle was used.";
                case FMOD.Result.ErrInvalidParam:             return "An invalid parameter was passed to this function.";
                case FMOD.Result.ErrInvalidPosition:          return "An invalid seek position was passed to this function.";
                case FMOD.Result.ErrInvalidSpeaker:           return "An invalid speaker was passed to this function based on the current speaker mode.";
                case FMOD.Result.ErrInvalidSyncpoint:         return "The syncpoint did not come from this sound handle.";
                case FMOD.Result.ErrInvalidThread:            return "Tried to call a function on a thread that is not supported.";
                case FMOD.Result.ErrInvalidVector:            return "The vectors passed in are not unit length, or perpendicular.";
                case FMOD.Result.ErrMaxaudible:                return "Reached maximum audible playback count for this sound's soundgroup.";
                case FMOD.Result.ErrMemory:                    return "Not enough memory or resources.";
                case FMOD.Result.ErrMemoryCantpoint:          return "Can't use FMOD_OPENMEMORY_POINT on non PCM source data, or non mp3/xma/adpcm data if FMOD_CREATECOMPRESSEDSAMPLE was used.";
                case FMOD.Result.ErrNeeds3D:                   return "Tried to call a command on a 2d sound when the command was meant for 3d sound.";
                case FMOD.Result.ErrNeedshardware:             return "Tried to use a feature that requires hardware support.";
                case FMOD.Result.ErrNetConnect:               return "Couldn't connect to the specified host.";
                case FMOD.Result.ErrNetSocketError:          return "A socket error occurred.  This is a catch-all for socket-related errors not listed elsewhere.";
                case FMOD.Result.ErrNetUrl:                   return "The specified URL couldn't be resolved.";
                case FMOD.Result.ErrNetWouldBlock:           return "Operation on a non-blocking socket could not complete immediately.";
                case FMOD.Result.ErrNotready:                  return "Operation could not be performed because specified sound/DSP connection is not ready.";
                case FMOD.Result.ErrOutputAllocated:          return "Error initializing output device, but more specifically, the output device is already in use and cannot be reused.";
                case FMOD.Result.ErrOutputCreatebuffer:       return "Error creating hardware sound buffer.";
                case FMOD.Result.ErrOutputDrivercall:         return "A call to a standard soundcard driver failed, which could possibly mean a bug in the driver or resources were missing or exhausted.";
                case FMOD.Result.ErrOutputFormat:             return "Soundcard does not support the specified format.";
                case FMOD.Result.ErrOutputInit:               return "Error initializing output device.";
                case FMOD.Result.ErrOutputNodrivers:          return "The output device has no drivers installed.  If pre-init, FMOD_OUTPUT_NOSOUND is selected as the output mode.  If post-init, the function just fails.";
                case FMOD.Result.ErrPlugin:                    return "An unspecified error has been returned from a plugin.";
                case FMOD.Result.ErrPluginMissing:            return "A requested output, dsp unit type or codec was not available.";
                case FMOD.Result.ErrPluginResource:           return "A resource that the plugin requires cannot be found. (ie the DLS file for MIDI playback)";
                case FMOD.Result.ErrPluginVersion:            return "A plugin was built with an unsupported SDK version.";
                case FMOD.Result.ErrRecord:                    return "An error occurred trying to initialize the recording device.";
                case FMOD.Result.ErrReverbChannelgroup:       return "Reverb properties cannot be set on this channel because a parent channelgroup owns the reverb connection.";
                case FMOD.Result.ErrReverbInstance:           return "Specified instance in FMOD_REVERB_PROPERTIES couldn't be set. Most likely because it is an invalid instance number or the reverb doesn't exist.";
                case FMOD.Result.ErrSubsounds:                 return "The error occurred because the sound referenced contains subsounds when it shouldn't have, or it doesn't contain subsounds when it should have.  The operation may also not be able to be performed on a parent sound.";
                case FMOD.Result.ErrSubsoundAllocated:        return "This subsound is already being used by another sound, you cannot have more than one parent to a sound.  Null out the other parent's entry first.";
                case FMOD.Result.ErrSubsoundCantmove:         return "Shared subsounds cannot be replaced or moved from their parent stream, such as when the parent stream is an FSB file.";
                case FMOD.Result.ErrTagnotfound:               return "The specified tag could not be found or there are no tags.";
                case FMOD.Result.ErrToomanychannels:           return "The sound created exceeds the allowable input channel count.  This can be increased using the 'maxinputchannels' parameter in System::setSoftwareFormat.";
                case FMOD.Result.ErrTruncated:                 return "The retrieved string is too long to fit in the supplied buffer and has been truncated.";
                case FMOD.Result.ErrUnimplemented:             return "Something in FMOD hasn't been implemented when it should be! contact support!";
                case FMOD.Result.ErrUninitialized:             return "This command failed because System::init or System::setDriver was not called.";
                case FMOD.Result.ErrUnsupported:               return "A command issued was not supported by this object.  Possibly a plugin without certain callbacks specified.";
                case FMOD.Result.ErrVersion:                   return "The version number of this file format is not supported.";
                case FMOD.Result.ErrEventAlreadyLoaded:      return "The specified bank has already been loaded.";
                case FMOD.Result.ErrEventLiveupdateBusy:     return "The live update connection failed due to the game already being connected.";
                case FMOD.Result.ErrEventLiveupdateMismatch: return "The live update connection failed due to the game data being out of sync with the tool.";
                case FMOD.Result.ErrEventLiveupdateTimeout:  return "The live update connection timed out.";
                case FMOD.Result.ErrEventNotfound:            return "The requested event, bus or vca could not be found.";
                case FMOD.Result.ErrStudioUninitialized:      return "The Studio::System object is not yet initialized.";
                case FMOD.Result.ErrStudioNotLoaded:         return "The specified resource is not loaded, so it can't be unloaded.";
                case FMOD.Result.ErrInvalidString:            return "An invalid string was passed to this function.";
                case FMOD.Result.ErrAlreadyLocked:            return "The specified resource is already locked.";
                case FMOD.Result.ErrNotLocked:                return "The specified resource is not locked, so it can't be unlocked.";
				case FMOD.Result.ErrRecordDisconnected:       return "The specified recording driver has been disconnected.";
				case FMOD.Result.ErrToomanysamples:            return "The length provided exceed the allowable limit.";
                default:                                        return "Unknown error.";
            }
        }
    }
}
