using Sandbox;

namespace SandMix;

public static class SandMix
{
	public const int SampleRate = 44100; // Don't change this

	public const int SampleSize = 1024;
	
	public const int UpdatePasses = 5;
	public const int UpdateSampleMin = 512;
	public const int UpdateSampleMax = 4096;


	[ConVar.Client( "smix_debug" )]
	public static bool Debug { get; set; } = false;
}
