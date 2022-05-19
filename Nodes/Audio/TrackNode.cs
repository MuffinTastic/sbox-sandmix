using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Sandbox;

namespace SandMix.Nodes.Audio;

[Library, Display( Name = "Track", Description = "Audio track - Sources from a .vsnd file", GroupName = "Audio" )]
public class TrackNode : BaseAudio
{
	[ResourceType( "vsnd" )]
	public string Track { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public BaseAudio Output { get; set; }
}
