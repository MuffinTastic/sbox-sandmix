using System;
using System.Collections.Generic;
using System.IO;
using Sandbox;

namespace SandMix;

/// <summary>
/// Sample loader that buffers via filename.
/// Unique sound files only ever get loaded once, they're then handed out again and again to voices that request them
/// </summary>
public static partial class SoundLoader
{
	private const string VSND_EXT = ".vsnd";
	private const string VSND_C_EXT = ".vsnd_c";
	private const string WAV_EXT = ".wav";
	private const string MP3_EXT = ".mp3";

	private static Dictionary<string, SoundData> LoadedSoundData = new();

	[Event.Hotload]
	public static void aaaa()
	{
		LoadedSoundData.Clear();
	}

	/// <summary>
	/// Get samples from a file.
	/// </summary>
	/// <param name="file">Name of a sound file</param>
	public static SoundData LoadSamples( string file )
	{
		SoundData soundData;

		var soundName = Path.GetFileNameWithoutExtension( file ).ToLower();

		if ( LoadedSoundData.TryGetValue( soundName, out soundData ) )
		{
			return soundData;
		}

		var ext = Path.GetExtension( file ).ToLower();
		Stream stream;

		switch ( ext )
		{
			case VSND_EXT:   // could be anything, try a bunch of shit
				var vsndFile = Path.ChangeExtension( file, VSND_C_EXT );
				if ( FileSystem.Mounted.FileExists( vsndFile ) )
				{
					stream = FileSystem.Mounted.OpenRead( vsndFile );
					LoadFromVsnd( stream, out soundData );
					break;
				}

				vsndFile = Path.ChangeExtension( file, WAV_EXT );
				if ( FileSystem.Mounted.FileExists( vsndFile ) )
				{
					//LoadFromWav( stream, out soundData );

					break;
				}

				vsndFile = Path.ChangeExtension( file, MP3_EXT );
				if ( FileSystem.Mounted.FileExists( vsndFile ) )
				{
					//LoadFromMp3( stream, out soundData );
				}


				break;

			case VSND_C_EXT: // let's trust the user, try to load data based off the extension they give us
				stream = FileSystem.Mounted.OpenRead( file );
				LoadFromVsnd( stream, out soundData );
				break;

			case WAV_EXT:
				stream = FileSystem.Mounted.OpenRead( file );

				break;

			case MP3_EXT:
				stream = FileSystem.Mounted.OpenRead( file );

				break;

			default:
				
				break;
		}

		LoadedSoundData.Add( soundName, soundData );
		return soundData;
	}

	public class InvalidSoundDataException : Exception
	{
		public InvalidSoundDataException() : base() { }
		public InvalidSoundDataException( string message ) : base( message ) { }
		public InvalidSoundDataException( string message, Exception inner ) : base( message, inner ) { }
	}
}

public class SoundData
{
	public uint Size;
	public uint SampleRate;
	public uint SampleCount;
	public uint Bits;
	public uint Channels;
	public int LoopStart;
	public int LoopEnd;
	public short[] Samples;

	// do we need these?
	// public uint SampleSize;
	// public float Duration;
}
