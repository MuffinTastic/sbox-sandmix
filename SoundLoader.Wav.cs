using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SandMix;

public static partial class SoundLoader
{
	private static class Wav
	{
		public const string ChunkID = "RIFF";
		public const string Format = "WAVE";

		public enum AudioFormat
		{
			Unknown = 0,
			PCM = 1,
			ADPCM = 2,
		}

		public enum SubchunkType
		{
			Format,
			Fact,
			Data,
			List,
			None
		}

		public struct Subchunk
		{
			public SubchunkType Type;
			public uint Offset;
			public uint Size;
		}

		public static SubchunkType GetSubchunkType( char[] id )
		{
			var idStr = new string( id ).ToLower();

			return idStr switch
			{
				"fmt " => SubchunkType.Format,
				"fact" => SubchunkType.Fact,
				"list" => SubchunkType.List,
				"data" => SubchunkType.Data,
				_ => throw new NotSupportedException( $"Wave Subchunk ID {idStr}" )
			};
		}
	}

	private static SoundData LoadFromWav( Stream stream, SoundData vsndData = null )
	{
		SoundData soundData;
		var reader = new BinaryReader( stream );

		if ( vsndData is null )
		{
			soundData = new SoundData();
			var subchunks = new List<Wav.Subchunk>();


			// main chunk - add all subchunks to a list


			var chunkId = reader.ReadChars( 4 );
			if ( !chunkId.SequenceEqual( Wav.ChunkID ) )
				throw new InvalidSoundDataException( "Bad WAVE chunk ID" );

			reader.BaseStream.Position += sizeof( uint );
			//var chunkSize = reader.ReadUInt32();

			var format = reader.ReadChars( 4 );
			if ( !format.SequenceEqual( Wav.Format ) )
				throw new InvalidSoundDataException( "Bad WAVE format" );

			var lastType = Wav.SubchunkType.None;
			
			while ( lastType != Wav.SubchunkType.Data ) // presumably the last subchunk
			{
				var subchunkID = reader.ReadChars( 4 );
				var subchunkSize = reader.ReadUInt32();

				var subchunk = new Wav.Subchunk
				{
					Type = Wav.GetSubchunkType( subchunkID ),
					Size = subchunkSize,
					Offset = (uint) reader.BaseStream.Position
				};

				subchunks.Add( subchunk );
				lastType = subchunk.Type;

				reader.BaseStream.Position += subchunkSize;
			}
			

			// read subchunks


			{	// format subchunk
				var subchunk = subchunks.Where( c => c.Type == Wav.SubchunkType.Format ).First();
				reader.BaseStream.Position = subchunk.Offset;

				var audioFormat = (Wav.AudioFormat)reader.ReadUInt16();
				if ( audioFormat != Wav.AudioFormat.PCM )
					throw new NotSupportedException( $"WAVE audio format {audioFormat} is not supported - please use Integer PCM" );

				soundData.Channels = reader.ReadUInt16();
				soundData.SampleRate = reader.ReadUInt32();

				reader.BaseStream.Position += sizeof( uint ) + sizeof( ushort );
				//var byteRate = reader.ReadUInt32();
				//var blockAlign = reader.ReadUInt16();

				soundData.BitsPerSample = reader.ReadUInt16();

				// usually doesn't exist for PCM
				// var extraParamSize = reader.ReadUInt16();
			}


			{   // data subchunk - read last so we can leave the reader untouched and load samples with the rest of the function
				var subchunk = subchunks.Where( c => c.Type == Wav.SubchunkType.Data ).First();
				reader.BaseStream.Position = subchunk.Offset;

				soundData.Size = subchunk.Size;
				soundData.SampleSize = soundData.BitsPerSample / 8;
				soundData.SampleCount = soundData.Size / soundData.SampleSize;
				soundData.Duration = (float)soundData.SampleCount / (float)soundData.SampleRate;
			}

		}
		else
		{
			soundData = vsndData;
		}

		var remaining = reader.BaseStream.Length - reader.BaseStream.Position;
		var actualCount = (remaining / soundData.SampleSize);

		soundData.Samples = new float[actualCount];

		if (soundData.SampleSize == sizeof( short ) )
		{
			for ( var i = 0; i < actualCount; i++ )
			{
				soundData.Samples[i] = Sample.ToFloat( reader.ReadInt16() );
			}
		}
		else if (soundData.SampleSize == sizeof( byte ) )
		{
			for ( var i = 0; i < actualCount; i++ )
			{
				soundData.Samples[i] = Sample.ToFloat( reader.ReadByte() );
			}
		}
		else
		{
			throw new NotSupportedException( $"Wave sound sample size {soundData.SampleSize} is not supported" );
		}

		return soundData;
	}
}
