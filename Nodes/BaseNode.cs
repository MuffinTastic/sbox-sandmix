using Sandbox.Internal;
using System;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
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

	public bool HasIdentifier( string id )
	{
		return string.Equals( id, Identifier, StringComparison.OrdinalIgnoreCase );
	}

	public class InputAttribute : Attribute
	{

	}

	public class OutputAttribute : Attribute
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

	protected string GetNodeError( string message )
	{
		var sb = new StringBuilder();

		var info = Sandbox.DisplayInfo.For( this );
		sb.Append( "Error in " );
		sb.Append( info.Name );
		sb.Append( " node" );

		if ( !string.IsNullOrEmpty( Name ))
		{
			sb.Append( " (" );
			sb.Append( Name );
			sb.Append( ")" );
		}

		sb.Append( ": " );

		sb.Append( message );

		return sb.ToString();
	}

	protected void NodeThrowIf( bool condition, string message )
	{
		if ( condition )
			throw new NodeException( GetNodeError( message ) );
	}

	protected void NodeThrow( string message )
	{
		throw new NodeException( GetNodeError( message ) );
	}

	public class NodeException : Exception
	{
		public NodeException() : base() { }
		public NodeException( string message ) : base( message ) { }
		public NodeException( string message, Exception inner ) : base( message, inner ) { }
	}
}
