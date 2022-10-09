using Sandbox;

namespace SandMix;

[GameResource("s&mix mixgraph", "smix", "Mixgraph for live audio processing with s&mix", Icon = "account_tree" )]
public partial class MixGraphResource : SandMixResource
{
	public const string FileExtension = "smix";

	[HideInEditor]
	public string JsonData { get; set; }
}
