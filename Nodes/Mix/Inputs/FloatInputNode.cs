using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandMix.Nodes.Mix.Inputs;

[Display( Name = "Float Input", Description = "Variable float input, supplied by game code", GroupName = "#smix.node.category.inputs" )]
public class FloatInputNode : BaseMixNode
{
	[Output]
	public float Value { get; set; } = 0;

	// --- //

	public override void ProcessMix()
	{
		SetDoneProcessing();
	}
}
