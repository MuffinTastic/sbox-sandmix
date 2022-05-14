using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SandMix.Nodes.Maths;

[Library, Icon( "add" ), Display( Name = "Add Float", Description = "Add two floats together", GroupName = "Maths" )]
public class FloatAddNode : BaseNode
{
	[Input]
	public float X { get; set; }

	[Input]
	public float Y { get; set; }

	[Browsable( false ), Output]
	public float Result => X + Y;
}
