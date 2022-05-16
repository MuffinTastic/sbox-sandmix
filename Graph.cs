using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SandMix;
using SandMix.Nodes;

public class Graph
{
	[Browsable( true )]
	public List<BaseNode> Nodes { get; set; } = new();
	public List<(string, string)> Connections { get; set; } = new();


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
		if ( !Connections.Contains( (from, to) ) )
			Connections.Add( (from, to) );
	}

	public void Disconnect( string from, string to )
	{
		if ( Connections.Contains( (from, to) ) )
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

	public static Graph Deserialize( string json )
	{
		JsonSerializerOptions options = new()
		{
			IncludeFields = true,
			WriteIndented = true
		};
		options.Converters.Add( new BaseNodeConverter() );

		return JsonSerializer.Deserialize( json, typeof( Graph ), options ) as Graph;
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

	public void RegenerateIdentifiers()
	{
		foreach ( var node in Nodes )
		{
			var oldIdent = node.Identifier;
			node.Identifier = BaseNode.GetNewIdentifier();

			for ( var i = 0; i < Connections.Count; i++ )
			{
				var split1 = Connections[i].Item1.Split( '.', 2 );
				var split2 = Connections[i].Item2.Split( '.', 2 );

				string newOut = Connections[i].Item1;
				string newIn = Connections[i].Item2;

				if ( split1[0] == oldIdent )
					newOut = $"{node.Identifier}.{split1[1]}";
				if ( split2[0] == oldIdent )
					newIn = $"{node.Identifier}.{split2[1]}";

				Connections[i] = (newOut, newIn);
			}
		}
	}
}
