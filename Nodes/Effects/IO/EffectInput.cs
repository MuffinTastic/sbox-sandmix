using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Effects.IO;

[Display( Name = "Effect Input", Description = "Input to the effect graph", GroupName = "I/O" )]
public class EffectInput : BaseEffectNode
{
	[Browsable( false ), Output, JsonIgnore]
	public AudioSamples Input { get; set; }
}
