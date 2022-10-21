using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Effects.IO;

[Display( Name = "Effect Output", Description = "Output to the effect graph", GroupName = "#smix.node.category.io" )]
public class EffectOutput : BaseEffectNode
{
	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.output" )]
	public float[][] Output { get; set; }
}
