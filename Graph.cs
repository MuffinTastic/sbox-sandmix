using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

namespace SandMix;
using SandMix.Nodes;

public class Graph
{
	[Browsable( true )]
	public List<BaseNode> Nodes { get; init; } = new();
	public List<(string, string)> Connections { get; init; } = new();

	public Graph()
	{
		Nodes = new List<BaseNode>();
	}

	public void Add( BaseNode node )
	{
		node.Graph = this;
		Nodes.Add( node );
	}

	public void Remove( BaseNode node )
	{
		Nodes.Remove( node );
	}

	public BaseNode Find( string name )
	{
		return Nodes.FirstOrDefault( x => x.IsNamed( name ) );
	}

	public void Connect( string from, string to )
	{
		Connections.Add( (from, to) );
		Log.Info( Connections.Count() );
	}

	public void Disconnect( string from, string to )
	{
		Connections.Remove( (from, to) );
	}

	public List<(string, string)> FindFrom( string from )
	{
		return Connections.Where( ( c ) => c.Item1 == from ).ToList();
	}
	public List<(string, string)> FindTo( string to )
	{
		return Connections.Where( ( c ) => c.Item2 == to ).ToList();
	}

	public static Graph Deserialize()
	{
		throw new NotImplementedException();
	}

	public string Serialize()
	{
		JsonSerializerOptions options = new()
		{
			IncludeFields = true,
			WriteIndented = true
		};
		options.Converters.Add( new BaseNodeConverter() );

		return JsonSerializer.Serialize( this, typeof( Graph ), options );
	}
}
