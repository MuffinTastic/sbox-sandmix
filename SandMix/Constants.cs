using Sandbox;

namespace SandMix;

public static class SandMix
{
	public const string ProjectName = "s&mix";
	public const string ProjectTagline = "Dynamic node-based sound mixer for s&box";
	public const string ProjectVersion = "0.0.2";
	public const string ProjectRepoURL = "https://github.com/MuffinTastic/sbox-sandmix-tool";
	public static readonly string[] Authors = { "MuffinTastic" };

	// --- //

	// Don't change these two
	public const int Channels = 2;
	public const int SampleRate = 44100; 

	public const int SampleSize = 1024;
	
	public const int UpdateMaxPasses = 256;
	public const int UpdateSampleMin = 2048;

	// --- //

	[ConVar.Client( "smix_debug" )]
	public static bool Debug { get; set; } = false;
}
