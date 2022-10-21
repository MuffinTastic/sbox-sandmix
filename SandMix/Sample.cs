using Sandbox;

namespace SandMix;

public static class Sample
{
	public static float ToFloat( short sample )
	{
		var f = sample / 32768.0f;
		return Clamp( f );
	}

	public static float ToFloat( byte sample )
	{
		var f = (sample - 127.0f) * (1.0f / 128.0f);
		return Clamp( f );
	}

	public static short ToShort( float sample )
	{
		var clamp = sample.Clamp( -1.0f, 1.0f );
		var s = (short)(clamp * 32768.0f);
		return s;
	}

	public static float Clamp( float sample )
	{
		return sample.Clamp( -1.0f, 1.0f );
	}
}
