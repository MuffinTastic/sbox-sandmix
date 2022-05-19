using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SandMix.Nodes.Audio;

[Library, Display( Name = "Mix Output", Description = "Plays the output of the mix as a single ingame sound stream", GroupName = "Audio" )]
public class MixOutputNode : BaseAudio
{
	[Browsable( false ), Input]
	public BaseAudio Output { get; set; }
}
