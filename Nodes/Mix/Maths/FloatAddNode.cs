using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Mix.Maths;

[Icon( "add" ), Display( Name = "#smix.node.floatadd", Description = "#smix.node.floatadd.description", GroupName = "#smix.node.category.maths" )]
public class FloatAddNode : BaseMixNode
{
	[Browsable( false ), Input, JsonIgnore]
	public float X { get; set; } = 0;

	[Browsable( false ), Input, JsonIgnore]
	public float Y { get; set; } = 0;

	[Browsable( false ), Output, JsonIgnore, Display( Name = "#smix.node.output" )]
	public float Output { get; set; }

	// --- //

	public override void ProcessMix()
	{
		Output = X + Y;

		Log.Info( $"node {Name}, x {X} y {Y} result {Output}" );

		SetDoneProcessing();
	}
}
