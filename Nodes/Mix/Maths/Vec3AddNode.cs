using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Mix.Maths;

[Icon( "add" ), Display( Name = "Add Vector3", Description = "Add two Vector3s together", GroupName = "#smix.node.category.maths" )]
public class Vec3AddNode : BaseMixNode
{
	[Browsable( false ), Input, JsonIgnore]
	public Vector3 X { get; set; }

	[Browsable( false ), Input, JsonIgnore]
	public Vector3 Y { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public Vector3 Output { get; set; }

	// --- //

	public override void ProcessMix()
	{
		Output = X + Y;

		Log.Info( $"node {Name}, x {X} y {Y} result {Output}" );

		SetDoneProcessing();
	}
}
