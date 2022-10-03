using Sandbox.Internal;
using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes;

public abstract class BaseNode
{
	[Browsable( false )]
	public string Identifier { get; set; }

	[Browsable( false )]
	public Vector2 Position { get; set; }

	[Browsable( false ), JsonIgnore]
	public GraphContainer Graph { get; set; }

	public string Name { get; set; }
	public string Comment { get; set; }

	// ----- //

	public static string GetNewIdentifier()
	{
		return Guid.NewGuid().ToString();
	}

	public BaseNode()
	{
		Identifier = GetNewIdentifier();
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

	public virtual Task Load()
	{
		return SandMixUtil.CompletedTask;
	}

	public virtual void Unload()
	{

	}

	public virtual void Update()
	{

	}
}
