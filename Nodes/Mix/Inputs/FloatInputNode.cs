using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandMix.Nodes.Mix.Inputs;

[Display( Name = "Float Input", Description = "Variable float input, supplied by game code", GroupName = "Inputs" )]
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
