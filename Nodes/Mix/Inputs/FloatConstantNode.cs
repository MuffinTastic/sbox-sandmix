using System.ComponentModel.DataAnnotations;
using Sandbox;

namespace SandMix.Nodes.Mix.Inputs;

[Display( Name = "Float Constant", Description = "Constant float input", GroupName = "#smix.node.category.inputs" )]
public class FloatConstantNode : BaseMixNode
{
	[Output]
	public float Value { get; set; } = 0;

	// --- //

	public override void ProcessMix()
	{
		SetDoneProcessing();
	}
}
