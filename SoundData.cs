namespace SandMix;

public class SoundData
{
	public const int NoLoop = -1;

	public string File;
	public uint Size;
	public uint BitsPerSample;
	public uint SampleSize;
	public float Duration;
	public uint SampleRate;
	public uint SampleCount;
	public uint Channels;
	public int LoopStart = NoLoop;
	public int LoopEnd;

	public float[] Samples;

}
