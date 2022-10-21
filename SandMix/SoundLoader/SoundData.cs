namespace SandMix;

public class SoundData
{
	public const int NoLoop = -1;

	public string File;
	public int Size;
	public int BitsPerSample;
	public int SampleSize;
	public float Duration;
	public int SampleRate;
	public int SampleCount;
	public int Channels;
	public int LoopStart = NoLoop;
	public int LoopEnd;

	public float[] Samples;

}
