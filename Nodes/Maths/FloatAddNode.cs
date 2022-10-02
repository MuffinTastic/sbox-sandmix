using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Maths;

[Icon( "add" ), Display( Name = "Add Float", Description = "Add two floats together", GroupName = "Maths" )]
public class FloatAddNode : BaseNode
{
	[Input, JsonIgnore]
	public float X { get; set; }

	[Input, JsonIgnore]
	public float Y { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public float Result => X + Y;
}
