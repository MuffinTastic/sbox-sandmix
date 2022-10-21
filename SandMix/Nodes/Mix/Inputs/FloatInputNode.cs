using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandMix.Nodes.Mix.Inputs;

[Display( Name = "#smix.node.floatinput", Description = "#smix.node.floatinput.description", GroupName = "#smix.node.category.inputs" )]
public class FloatInputNode : BaseMixNode
{
	[Output, Display( Name = "#smix.node.value" )]
	public float Value { get; set; } = 0;

	// --- //

	public override void ProcessMix()
	{
		SetDoneProcessing();
	}
}
