using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Mix.Maths;

[Icon( "add" ), Display( Name = "Add Vector3", Description = "Add two Vector3s together", GroupName = "Maths" )]
public class Vec3AddNode : BaseMixNode
{
	[Input, JsonIgnore]
	public Vector3 X { get; set; }

	[Input, JsonIgnore]
	public Vector3 Y { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public Vector3 Result => X + Y;
}
