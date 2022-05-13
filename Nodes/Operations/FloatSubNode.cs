using Sandbox;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SandMix.Nodes.Operations;

[Library, Display( Name = "Sub Float", Description = "Subtract one float from another", GroupName = "Operations" )]
public class FloatSubNode : BaseNode
{
	[Input]
	public float X { get; set; }

	[Input]
	public float Y { get; set; }

	[Browsable( false ), Output]
	public float Result => X - Y;
}
