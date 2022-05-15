using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Maths;

[Library, Icon( "remove" ), Display( Name = "Sub Float", Description = "Subtract one float from another", GroupName = "Maths" )]
public class FloatSubNode : BaseNode
{
	[Input, JsonIgnore]
	public float X { get; set; }

	[Input, JsonIgnore]
	public float Y { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public float Result => X - Y;
}
