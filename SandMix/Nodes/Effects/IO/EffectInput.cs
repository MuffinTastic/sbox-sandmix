using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Effects.IO;

[Display( Name = "#smix.node.effectinput", Description = "#smix.node.effectinput.description", GroupName = "#smix.node.category.io" )]
public class EffectInput : BaseEffectGraphNode
{
	[Browsable( false ), Output, JsonIgnore, Display( Name = "#smix.node.input" )]
	public float[][] Input { get; set; }
}
