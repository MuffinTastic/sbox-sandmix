using System.ComponentModel.DataAnnotations;
using Sandbox;

namespace SandMix.Nodes.Mix.Inputs;

[Display( Name = "Constant Float", Description = "Constant float input", GroupName = "Inputs" )]
public class FloatConstantNode : BaseMixNode
{
	[Output]
	public float Value { get; set; } = 0;

	public override void ProcessMix()
	{
		SetDoneProcessing();
	}
}
