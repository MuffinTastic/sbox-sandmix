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
	private static bool Debug { get; set; } = false;

	private const string VSND_EXT = ".vsnd";
	private const string VSND_C_EXT = ".vsnd_c";
	private const string WAV_EXT = ".wav";

	private static Dictionary<string, SoundData> LoadedSoundData = new();

	[Event.Hotload]
	public static void OnHotload()
	{
		//LoadedSoundData.Clear();
	}

	/// <summary>
	/// Get samples from a sound file.
	/// </summary>
	/// <param name="file">Path to a sound file</param>
	public static SoundData LoadSamples( string file )
	{
		SoundData soundData = null;

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
					soundData = LoadFromVsnd( stream );
					break;
				}

				vsndFile = Path.ChangeExtension( file, WAV_EXT );
				if ( FileSystem.Mounted.FileExists( vsndFile ) )
				{
					stream = FileSystem.Mounted.OpenRead( vsndFile );
					soundData = LoadFromWav( stream );
					break;
				}

				throw new FileNotFoundException( $"No supported sound file format was found for {file}" );

			case VSND_C_EXT: // let's trust the user, try to load data based off the extension they give us
				stream = FileSystem.Mounted.OpenRead( file );
				soundData = LoadFromVsnd( stream );
				break;

			case WAV_EXT:
				stream = FileSystem.Mounted.OpenRead( file );
				soundData = LoadFromWav( stream );
				break;

			default:
				throw new NotSupportedException( $"Extension '{ext}' is not supported" );
		}

		if ( soundData is null )
			throw new InvalidSoundDataException( "No sound data was loaded" );

		if ( SoundLoader.Debug )
		{
			Log.Info( $"Size: {soundData.Samples.Length}" );
			Log.Info( $"SampleSize: {soundData.SampleSize}" );
			Log.Info( $"SampleRate: {soundData.SampleRate}" );
			Log.Info( $"BitsPerSample: {soundData.BitsPerSample}" );
			Log.Info( $"Channels: {soundData.Channels}" );
			Log.Info( $"LoopStart: {soundData.LoopStart}" );
			Log.Info( $"LoopEnd: {soundData.LoopEnd}" );
		}

		soundData.File = file;
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
