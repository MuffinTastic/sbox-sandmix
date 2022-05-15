using System.ComponentModel.DataAnnotations;
using Sandbox;

namespace SandMix.Nodes.Inputs;

[Library, Display( Name = "Constant Float", Description = "Constant float input", GroupName = "Inputs" )]
public class FloatConstantNode : BaseNode
{
	[Output]
	public float Value { get; set; } = 0;
}
