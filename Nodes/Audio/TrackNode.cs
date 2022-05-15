using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Sandbox;

#if TOOLS
using Tools;
#endif

namespace SandMix.Nodes.Audio;

[Library, Display( Name = "Track", Description = "Audio track - Sources from a .wav file", GroupName = "Audio" )]
public class TrackNode : BaseNode
{
	[ResourceType( "vsnd" )]
	public string Track { get; set; }

	[Browsable( false )]
	public string TrackFile
#if TOOLS
		=> AssetSystem.FindByPath( Track )?.RelativePath;
#else
		{ get; set; }
#endif

	[Browsable( false ), Output, JsonIgnore]
	public Types.Audio Output { get; set; }
}
