using NWaves.Operations;
using NWaves.Signals;
using Sandbox;
using System.Linq;
using System.Threading.Tasks;

namespace SandMix;

public static class SandMixUtil
{
	public static Task CompletedTask =>
#if SMIXTOOL
		Task.CompletedTask;
#else
		GameTask.CompletedTask;
#endif

	public static string Base64Encode( string plainText )
	{
		var plainTextBytes = System.Text.Encoding.UTF8.GetBytes( plainText );
		return System.Convert.ToBase64String( plainTextBytes );
	}
	public static string Base64Decode( string base64EncodedData )
	{
		var base64EncodedBytes = System.Convert.FromBase64String( base64EncodedData );
		return System.Text.Encoding.UTF8.GetString( base64EncodedBytes );
	}

	public static float[][] CreateBuffers()
	{
		float[][] buffers = new float[SandMix.Channels][];

		for ( int i = 0; i < buffers.Length; i++ )
		{
			buffers[i] = new float[SandMix.SampleSize];
		}

		return buffers;
	}
	public static float[][] GetSoundChannels( SoundData soundData )
	{
		Assert.False( soundData.Channels > 2, "Only mono and stereo supported" );

		var sourceLength = soundData.Samples.Length;
		var bufLength = sourceLength / soundData.Channels;

		float[][] buffers = new float[SandMix.Channels][];
		for ( int i = 0; i < buffers.Length; i++ )
		{
			buffers[i] = new float[bufLength];
		}

		if ( soundData.Channels > 1 )
		{
			int b = 0;
			for ( int i = 0; i < sourceLength; i += SandMix.Channels )
			{
				for ( int j = 0; j < SandMix.Channels; j++ )
				{
					buffers[j][b] = soundData.Samples[i + j] * 0.5f;
				}

				b++;
			}
		}
		else
		{
			for ( int i = 0; i < sourceLength; i++ )
			{
				buffers[0][i] = soundData.Samples[i] * 0.5f;
				buffers[1][i] = soundData.Samples[i] * 0.5f;
			}
		}

		var signals = buffers.Select( b => new DiscreteSignal( soundData.SampleRate, b ) ).ToArray();

		var resampler = new Resampler();

		signals = signals.Select( s => resampler.Resample( s, SandMix.SampleRate ) ).ToArray();

		for ( int i = 0; i < buffers.Length; i++ )
		{
			buffers[i] = signals[i].Samples;
		}

		return buffers;
	}

	public static short[] GetStreamSamples( float[][] buffers )
	{
		var channels = buffers.Length;
		Assert.False( channels > 2, "Only mono and stereo are supported" );

		var length = buffers[0].Length;
		var buffer = new short[buffers[0].Length * channels];

		int b = 0;
		for ( int i = 0; i < length; i++ )
		{
			for ( int j = 0; j < channels; j++ )
			{
				buffer[b + j] = Sample.ToShort( buffers[j][i] * 2.0f );
			}

			b += channels;
		}

		return buffer;
	}
}
