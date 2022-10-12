using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Mix.Maths;

[Icon( "add" ), Display( Name = "Add Float", Description = "Add two floats together", GroupName = "Maths" )]
public class FloatAddNode : BaseMixNode
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
		Output = X + Y;

		Log.Info( $"node {Name}, x {X} y {Y} result {Output}" );

		SetDoneProcessing();
	}
}
