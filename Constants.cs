using Sandbox;

namespace SandMix;

public static class SandMix
{
	// Don't change these
	public const int Channels = 2;
	public const int SampleRate = 44100; 

	public const int SampleSize = 1024;
	
	public const int UpdatePasses = 5;
	public const int UpdateSampleMin = 2048;


	[ConVar.Client( "smix_debug" )]
	public static bool Debug { get; set; } = false;
}
