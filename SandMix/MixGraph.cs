﻿using Sandbox;
using SandMix.Nodes;
using SandMix.Nodes.Mix;
using SandMix.Nodes.Mix.Audio;
using SandMix.Nodes.Mix.Inputs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SandMix;

public class MixGraph
{
	public MixGraphResource Resource { get; private set; }

	private GraphContainer Graph;

	private List<LoadedConnection> LoadedConnections = new();

	private Dictionary<string, FloatInputNode> InputNodes = new();
	private AudioOutputNode OutputNode;

	private struct LoadedConnection
	{


		public BaseMixGraphNode OutputNode;
		public PropertyDescription OutputProperty;

		public BaseMixGraphNode InputNode;
		public PropertyDescription InputProperty;
		public uint InputCRC;
	}

	private bool Ready = false;
	private RealTimeSince LastLoaded = 1;

	private MixGraph( MixGraphResource resource )
	{
		Resource = resource;

		Resource.PostReloadEvent += ReloadGraph;
		ReloadGraph();
	}

#if !SMIXTOOL
	public static MixGraph Load( string file )
	{
		if ( ResourceLibrary.TryGet<MixGraphResource>( file, out var resource ) )
		{
			return new MixGraph( resource );
		}
		else
		{
			throw new FileNotFoundException( file );
		}
	}
#endif

	public void ReloadGraph()
	{
		if ( LastLoaded < 0.1f )
			return;

		LastLoaded = 0;

		if ( Graph is not null )
		{
			foreach ( var node in Graph.Nodes )
			{
				node.Unload();
			}
		}

		Ready = false;
		Graph = null;

		try
		{
			Log.Info( $"Loading mixgraph {Resource.ResourcePath}..." );

			Graph = GraphContainer.Deserialize( Resource.JsonData );

			if ( SandMix.Debug )
			{
				foreach ( var node in Graph.Nodes )
				{
					Log.Info( $"Node: {node.GetType()}, {node.Name}" );
				}
			}
			
			Graph.Validate();

			if ( Graph.GraphType != GraphType.Mix )
			{
				throw new GraphContainer.InvalidGraphException( "Not a mix graph!" );
			}

			LoadConnections();

			// Sets Ready
			_ = LoadNodes();
		}
		catch ( Exception ex )
		{
			Log.Error( ex );
			Log.Error( $"Couldn't load mixgraph {Resource.ResourcePath} - see console" );
		}
	}

	private async Task LoadNodes()
	{
		try
		{
			var tasks = new List<Task>();

			foreach ( var node in Graph.Nodes )
			{
				tasks.Add( node.Load() );
			}

			foreach ( var task in tasks )
			{
				await task;
			}

			Ready = true;
			Log.Info( $"Mixgraph {Resource.ResourcePath} loaded" );
		}
		catch ( Exception ex )
		{
			Log.Error( ex );
			Log.Error( $"Couldn't load mixgraph {Resource.ResourcePath} - see console" );
		}
	}

	[Event.Hotload]
	private void LoadConnections()
	{
		LoadedConnections.Clear();
		InputNodes.Clear();
		OutputNode = null;

		foreach ( var connection in Graph.Connections )
		{
			var split1 = connection.Item1.Split( '.', 2 );
			var split2 = connection.Item2.Split( '.', 2 );

			var outputNode = (BaseMixGraphNode) Graph.Find( split1[0] );
			var inputNode = (BaseMixGraphNode) Graph.Find( split2[0] );

			var output = split1[1];
			var input = split2[1];

			var outputProperty = TypeLibrary.GetDescription( outputNode.GetType() ).GetProperty( output );
			var inputProperty = TypeLibrary.GetDescription( inputNode.GetType() ).GetProperty( input );

			inputNode.AddConnectedInput( inputProperty );

			var loadedConnection = new LoadedConnection()
			{
				OutputNode = outputNode,
				OutputProperty = outputProperty,

				InputNode = inputNode,
				InputProperty = inputProperty,
				InputCRC = BaseMixGraphNode.GetInputId( inputProperty )
			};

			LoadedConnections.Add( loadedConnection );

			if ( outputNode is FloatInputNode floatInputNode )
			{
				if ( !string.IsNullOrEmpty( floatInputNode.Name ) )
				{
					InputNodes.Add( floatInputNode.Name, floatInputNode );
				}
			}
		}

		var outputNodes = Graph.Nodes.OfType<AudioOutputNode>();
		var count = outputNodes.Count();

		if ( count == 0 )
			throw new GraphContainer.InvalidGraphException( "No output nodes" );
		else if ( count > 1 )
			throw new GraphContainer.InvalidGraphException( "More than one output node" );

		OutputNode = outputNodes.First();
	}

#if !SMIXTOOL
	public void Update()
	{
		if ( !Ready ) return;

		try
		{
			// Run updates until queued sample count is above the desired amount
			while ( OutputNode.Queued < SandMix.UpdateSampleMin )
			{
				// Reset nodes for current update
				foreach ( var node in Graph.Nodes )
				{
					var mixNode = (BaseMixGraphNode)node;
					mixNode.Reset();
				}

				int passes = 0;

				// Run update passes until outputnode can queue audio
				while ( !OutputNode.DoneProcessing )
				{
					// We can skip an extra loop and save unnecessary processing
					// from unconnected nodes by just going through connections
					foreach ( var connection in LoadedConnections )
					{
						var inputNode = connection.InputNode;
						var outputNode = connection.OutputNode;

						// Are inputs ready but we haven't processed data yet? Do it!
						if ( outputNode.IsReady && !outputNode.DoneProcessing )
						{
							outputNode.ProcessMix();
						}

						// Is there data ready that we haven't copied yet? Do it!
						if ( outputNode.DoneProcessing && !inputNode.GetReady( connection.InputCRC ) )
						{
							var value = connection.OutputProperty.GetValue( outputNode );
							connection.InputProperty.SetValue( inputNode, value );

							inputNode.SetReady( connection.InputCRC );
						}
					}

					// By skipping unconnected nodes like that though, we also skip the output
					// So let's do this manually
					if ( OutputNode.IsReady )
					{
						// Update is done, play them samples
						OutputNode.ProcessMix();
					}

					passes++;

					if ( passes > SandMix.UpdateMaxPasses )
					{
						throw new Exception( "Caught infinite loop - did someone forget SetDoneProcessing() in a node?" );
					}
				}
			}
		}
		// catch ( TargetException ex )
		// Why is this exception not whitelisted?
		// Let's just... check the error string :)
		catch ( Exception ex ) when ( ex.Message == "Object does not match target type." )
		{
			// When hotloading some objects become invalid and throw this exception, rebuild them
			LoadConnections();
		}
	}

	public void SetInput( string name, float value )
	{
		if ( InputNodes.TryGetValue( name, out FloatInputNode node ) )
		{
			if ( node is null ) return;

			node.Value = value;
		}
		else
		{
			Log.Warning( $"Tried to set invalid mixgraph input '{name}', ignoring" );
			InputNodes.Add( name, null );
		}
	}
#endif
}
