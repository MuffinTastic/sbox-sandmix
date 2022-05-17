using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ValveResourceFormat;

namespace SandMix;

public static partial class SoundLoader
{
	private static class Vsnd
	{
		public const uint VPKMagic = 0x55AA1234;
		public const ushort HeaderVersion = 12;
		public const ushort VsndVersion = 4;

		public enum BlockType
		{
			REDI, // will probably never do anything with this one
			DATA
		}

		public struct Block
		{
			public BlockType Type;
			public uint Offset;
			public uint Size;
		}

		public enum AudioFileType
		{
			AAC = 0,
			WAV = 1,
			MP3 = 2,
		}

		public enum AudioFormatV4
		{
			PCM16 = 0,
			PCM8 = 1,
			MP3 = 2,
			ADPCM = 3,
		}

		public enum WaveAudioFormat
		{
			Unknown = 0,
			PCM = 1,
			ADPCM = 2,
		}
	}

	private static void LoadFromVsnd( Stream stream, out SoundData soundData )
	{
		soundData = new SoundData();
		var reader = new BinaryReader( stream );


		// load generic valve resource gunk


		var fileSize = reader.ReadUInt32();
		if ( fileSize == Vsnd.VPKMagic )
		{
			throw new InvalidSoundDataException( "VPKs are not supported" );
		}
		Log.Info( $"File size: {fileSize}" );

		var headerVersion = reader.ReadUInt16();
		if ( headerVersion != Vsnd.HeaderVersion )
		{
			throw new NotSupportedException( $"Header version {headerVersion}" );
		}
		Log.Info( $"Header version: {headerVersion}" );

		var version = reader.ReadUInt16();
		Log.Info( $"Version: {version}" );

		var blockOffset = reader.ReadUInt32();
		var blockCount = reader.ReadUInt32();

		reader.BaseStream.Position += blockOffset - 8; // anchor is before block offset

		var blocks = new List<Vsnd.Block>();

		for ( var i = 0; i < blockCount; i++ )
		{
			var blockTypeStr = Encoding.UTF8.GetString( reader.ReadBytes( 4 ) );
			var position = reader.BaseStream.Position;
			var offset = (uint)position + reader.ReadUInt32();
			var size = reader.ReadUInt32();

			Log.Info( blockTypeStr );

			var blockType = blockTypeStr switch
			{
				nameof( Vsnd.BlockType.DATA ) => Vsnd.BlockType.DATA,
				nameof( Vsnd.BlockType.REDI ) => Vsnd.BlockType.REDI,
				_ => throw new NotSupportedException( blockTypeStr )
			};

			var block = new Vsnd.Block
			{
				Type = blockType,
				Offset = offset,
				Size = size
			};

			blocks.Add( block );
		}

		Log.Info( $"{blocks.Count} block(s)" );

		var dataBlocks = blocks.Where( b => b.Type == Vsnd.BlockType.DATA );
		if ( dataBlocks.Count() != 1 )
			throw new InvalidSoundDataException( "Data block count != 1" );

		var dataBlock = dataBlocks.First();


		// load vsnd metadata


		reader.BaseStream.Position = dataBlock.Offset;

		if ( version != Vsnd.VsndVersion )
			throw new NotSupportedException( $"Vsnd version {version}" );

		soundData.SampleRate = reader.ReadUInt16();
		var soundFormat = (Vsnd.AudioFormatV4) reader.ReadByte();
		soundData.Channels = reader.ReadByte();

		soundData.LoopStart = reader.ReadInt32();
		soundData.SampleCount = reader.ReadUInt32();
		reader.BaseStream.Position += sizeof( float ); //soundData.Duration = reader.ReadSingle();

		// skip m_Sentence and m_pHeader
		reader.BaseStream.Position += 12;

		soundData.Size = reader.ReadUInt32();

		// v4
		if ( reader.ReadUInt32() != 0 ) throw new InvalidSoundDataException();
		if ( reader.ReadUInt32() != 0 ) throw new InvalidSoundDataException();
		if ( reader.ReadUInt32() != 0 ) throw new InvalidSoundDataException();

		soundData.LoopEnd = reader.ReadInt32();


		// we've got all our data, now the touchups


		Vsnd.AudioFileType fileType;
		var waveFormat = Vsnd.WaveAudioFormat.Unknown;

		switch ( soundFormat )
		{
			case Vsnd.AudioFormatV4.PCM8:
				fileType = Vsnd.AudioFileType.WAV;
				soundData.Bits = 8;
				//soundData.SampleSize = 1;
				waveFormat = Vsnd.WaveAudioFormat.PCM;
				break;

			case Vsnd.AudioFormatV4.PCM16:
				fileType = Vsnd.AudioFileType.WAV;
				soundData.Bits = 16;
				//soundData.SampleSize = 2;
				waveFormat = Vsnd.WaveAudioFormat.PCM;
				break;

			case Vsnd.AudioFormatV4.MP3:
				fileType = Vsnd.AudioFileType.MP3;
				break;

			case Vsnd.AudioFormatV4.ADPCM:
				//fileType = Vsnd.AudioFileType.WAV;
				//soundData.Bits = 4;
				//soundData.SampleSize = 1;
				//waveFormat = Vsnd.WaveAudioFormat.ADPCM;
				throw new NotImplementedException( "ADPCM" );

			default:
				throw new InvalidSoundDataException( $"Sound format {(int)soundFormat}" );
		}

		Log.Info( "Vsnd data block:" );
		Log.Info( $"Size: {soundData.Size}" );
		//Log.Info( $"SampleSize: {soundData.SampleSize}" );
		Log.Info( $"SampleRate: {soundData.SampleRate}" );
		Log.Info( $"SampleCount: {soundData.SampleCount}" );
		Log.Info( $"Bits: {soundData.Bits}" );
		Log.Info( $"Channels: {soundData.Channels}" );
		Log.Info( $"LoopStart: {soundData.LoopStart}" );
		Log.Info( $"LoopEnd: {soundData.LoopEnd}" );

		/*

		switch ( fileType )
		{
			case Vsnd.AudioFileType.WAV:
				reader.BaseStream.CopyTo(  );
		}*/
	}
}
