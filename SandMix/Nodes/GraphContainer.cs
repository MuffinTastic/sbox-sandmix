﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SandMix.Nodes;

public enum GraphType : int
{
	Mix,
	Effect
}

public class GraphContainer
{
	public GraphType GraphType { get; set; }
	public Vector2 EditorCenter { get; set; } = Vector2.Zero;
	public float EditorScale { get; set; } = 1.0f;

	[Browsable( true )]
	public List<BaseNode> Nodes { get; set; } = new();
	public List<(string, string)> Connections { get; set; } = new();

	[JsonConstructor]
	public GraphContainer()
	{

	}

	public GraphContainer( GraphType type )
	{
		GraphType = type;
	}

	public bool Any()
	{
		return Nodes.Any() || Connections.Any();
	}

	public void Add( BaseNode node )
	{
		if ( Nodes.Contains( node ) )
			return;

		node.Graph = this;
		Nodes.Add( node );
	}

	public void Remove( BaseNode node )
	{
		if ( !Nodes.Contains( node ) )
			return;

		Nodes.Remove( node );
	}

	public BaseNode Find( string id )
	{
		return Nodes.FirstOrDefault( x => x.HasIdentifier( id ) );
	}

	public void Connect( string from, string to )
	{
		if ( Connections.Contains( (from, to) ) )
			return;

		Connections.Add( (from, to) );
	}

	public void Disconnect( string from, string to )
	{
		if ( !Connections.Contains( (from, to) ) )
			return;

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

	public List<(string, string)> FindFrom( BaseNode node, string from )
	{
		return FindFrom( $"{node.Identifier}.{from}" );
	}

	public List<(string, string)> FindTo( BaseNode node, string to )
	{
		return FindTo( $"{node.Identifier}.{to}" );
	}

	public static GraphContainer Deserialize( string json )
	{
		JsonSerializerOptions options = new()
		{
			IncludeFields = true,
			// WriteIndented = true
		};
		options.Converters.Add( new BaseNodeConverter() );

		//json = SandMixUtil.Base64Decode( json );

		var graph = JsonSerializer.Deserialize( json, typeof( GraphContainer ), options ) as GraphContainer;

		foreach ( var node in graph.Nodes )
		{
			node.Graph = graph;
		}

		return graph;
	}

	public string Serialize()
	{
		JsonSerializerOptions options = new()
		{
			IncludeFields = true,
			// WriteIndented = true
		};
		options.Converters.Add( new BaseNodeConverter() );

		var json = JsonSerializer.Serialize( this, typeof( GraphContainer ), options );

		return json; // SandMixUtil.Base64Encode( json );
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

	public void Validate()
	{
		var uniqueNodes = new HashSet<string>();
		var uniqueInputs = new HashSet<string>();

		foreach ( var connection in Connections )
		{
			var split1 = connection.Item1.Split( '.', 2 );
			var split2 = connection.Item2.Split( '.', 2 );

			uniqueNodes.Add( split1[0] );
			uniqueNodes.Add( split2[0] );

			uniqueInputs.Add( connection.Item2 );
		}

		// Check for missing nodes

		foreach ( var nodeId in uniqueNodes )
		{
			if ( Find( nodeId ) is null )
				throw new InvalidGraphException( $"Connection to missing node {nodeId}" );
		}

		// Check for two connections to the same input

		foreach ( var inputId in uniqueInputs )
		{
			var to = FindTo( inputId );
			if ( to.Count > 1 )
				throw new InvalidGraphException( $"Multiple connections to single input {inputId}" );
		}

		// Validated
	}

	public class InvalidGraphException : Exception
	{
		public InvalidGraphException() : base() { }
		public InvalidGraphException( string message ) : base( message ) { }
		public InvalidGraphException( string message, Exception inner ) : base( message, inner ) { }
	}
}

