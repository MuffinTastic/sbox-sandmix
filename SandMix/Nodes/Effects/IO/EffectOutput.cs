using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Effects.IO;

[Display( Name = "#smix.node.effectoutput", Description = "#smix.node.effectoutput.description", GroupName = "#smix.node.category.io" )]
public class EffectOutput : BaseEffectGraphNode
{
	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.output" )]
	public float[][] Output { get; set; }
}
