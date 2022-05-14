using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SandMix.Nodes.Maths;

[Library, Icon( "remove" ), Display( Name = "Sub Float", Description = "Subtract one float from another", GroupName = "Maths" )]
public class FloatSubNode : BaseNode
{
	[Input]
	public float X { get; set; }

	[Input]
	public float Y { get; set; }

	[Browsable( false ), Output]
	public float Result => X - Y;
}
