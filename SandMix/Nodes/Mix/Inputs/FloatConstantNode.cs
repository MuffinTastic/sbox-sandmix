using System.ComponentModel.DataAnnotations;
using Sandbox;

namespace SandMix.Nodes.Mix.Inputs;

[Display( Name = "#smix.node.floatconstant", Description = "#smix.node.floatconstant.description", GroupName = "#smix.node.category.inputs" )]
public class FloatConstantNode : BaseMixGraphNode
{
	[Output, Display( Name = "#smix.node.value" )]
	public float Value { get; set; } = 0;

	// --- //

	public override void ProcessMix()
	{
		SetDoneProcessing();
	}
}
