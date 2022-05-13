using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SandMix.Nodes;

public abstract class BaseNode
{
	[Browsable( false )]
	public string Identifier { get; set; }

	[Browsable( false )]
	public Vector2 Position { get; set; }

	[Browsable( false ), JsonIgnore]
	public Graph Graph { get; set; }

	public string Name { get; set; }
	public string Comment { get; set; }

	public BaseNode()
	{
		Identifier = Guid.NewGuid().ToString();
	}

	public bool IsNamed( string name )
	{
		return string.Equals( name, Identifier, StringComparison.OrdinalIgnoreCase );
	}

	public class InputAttribute : Attribute
	{

	}

	public class OutputAttribute : Attribute
	{

	}

	public class ConstantAttribute : Attribute
	{

	}
}
