using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Mix.Maths;

[Icon( "remove" ), Display( Name = "Sub Float", Description = "Subtract one float from another", GroupName = "Maths" )]
public class FloatSubNode : BaseMixNode
{
	[Browsable( false ), Input, JsonIgnore]
	public float X { get; set; } = 0;

	[Browsable( false ), Input, JsonIgnore]
	public float Y { get; set; } = 0;

	[Browsable( false ), Output, JsonIgnore]
	public float Output { get; set; }

	// --- //

	public override void ProcessMix()
	{
		Output = X - Y;

		SetDoneProcessing();
	}
}
