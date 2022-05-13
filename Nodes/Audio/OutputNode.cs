using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SandMix.Nodes.Audio;

[Library, Display( Name = "Output", Description = "Audio output - corresponds to a single ingame sound stream", GroupName = "Audio" )]
public class OutputNode : BaseNode
{
	[Browsable( false ), Input]
	public Types.Audio Output { get; set; }
}
