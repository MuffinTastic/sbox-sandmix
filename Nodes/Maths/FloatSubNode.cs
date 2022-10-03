using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Maths;

[Icon( "remove" ), Display( Name = "Sub Float", Description = "Subtract one float from another", GroupName = "Maths" )]
public class FloatSubNode : BaseNode
{
	[Browsable( false ), Input, JsonIgnore]
	public float X { get; set; }

	[Browsable( false ), Input, JsonIgnore]
	public float Y { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public float Result { get; set; }

	// --- //

	public override void Update()
	{
		Result = X - Y;
	}
}
