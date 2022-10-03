using Sandbox;
using SandMix.Nodes;
using SandMix.Nodes.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SandMix;

public class MixGraph
{
	public string File { get; private set; }

	private FileWatch GraphWatch;
	private GraphContainer Graph;

	private List<LoadedConnection> LoadedConnections = new();
	private List<MixOutputNode> OutputNodes;

	private struct LoadedConnection
	{
		public BaseNode OutputNode;
		public PropertyDescription OutputProperty;

		public BaseNode InputNode;
		public PropertyDescription InputProperty;
	}

	private bool Ready = false;

	public MixGraph( string file )
	{
		File = file;

		ReloadGraph();
	}

	public void ReloadGraph()
	{
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
			Log.Info( $"Loading mixgraph {File}..." );
			
			var data = FileSystem.Mounted.ReadAllText( File );
			Graph = GraphContainer.Deserialize( data );

			if ( SandMix.Debug )
			{
				foreach ( var node in Graph.Nodes )
				{
					Log.Info( $"Node: {node.GetType()}, {node.Name}" );
				}
			}
			
			Graph.Validate();

			LoadConnections();

			if ( !OutputNodes.Any() )
				throw new GraphContainer.InvalidGraphException( "No output nodes" );

			// Sets Ready
			_ = LoadNodes();
		}
		catch ( Exception ex )
		{
			Log.Error( ex );
			Log.Error( $"Couldn't load mixgraph {File} - see console" );
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
			Log.Info( $"Mixgraph {File} loaded" );
		}
		catch ( Exception ex )
		{
			Log.Error( ex );
			Log.Error( $"Couldn't load mixgraph {File} - see console" );
		}
	}

	[Event.Hotload]
	private void LoadConnections()
	{
		LoadedConnections.Clear();
		OutputNodes?.Clear();

		foreach ( var connection in Graph.Connections )
		{
			var split1 = connection.Item1.Split( '.', 2 );
			var split2 = connection.Item2.Split( '.', 2 );

			BaseNode outputNode = Graph.Find( split1[0] );
			BaseNode inputNode = Graph.Find( split2[0] );

			var output = split1[1];
			var input = split2[1];

			var outputProperty = TypeLibrary.GetDescription( outputNode.GetType() ).GetProperty( output );
			var inputProperty = TypeLibrary.GetDescription( inputNode.GetType() ).GetProperty( input );

			var loadedConnection = new LoadedConnection()
			{
				OutputNode = outputNode,
				OutputProperty = outputProperty,

				InputNode = inputNode,
				InputProperty = inputProperty,
			};

			LoadedConnections.Add( loadedConnection );
		}

		OutputNodes = Graph.Nodes.OfType<MixOutputNode>().ToList();
	}

	public void Update( int passes = 0 )
	{
#if !SMIXTOOL
		if ( !Ready ) return;

		try
		{
			if ( passes < SandMix.UpdatePasses )
			{
				var minimumQueued = OutputNodes.Select( n => n.Queued ).Min();

				if ( minimumQueued > SandMix.UpdateSampleMax )
					return;
			}

			// Update nodes
			foreach ( var node in Graph.Nodes )
			{
				node.Update();
			}

			// Propagate data
			foreach ( var connection in LoadedConnections )
			{
				var value = connection.OutputProperty.GetValue( connection.OutputNode );
				connection.InputProperty.SetValue( connection.InputNode, value );
			}

			if ( passes < SandMix.UpdatePasses )
			{
				var minimumQueued = OutputNodes.Select( n => n.Queued ).Min();

				if ( minimumQueued < SandMix.UpdateSampleMin )
				{
					Update( passes + 1 );
				}
			}
		}
		//catch ( TargetException ex )
		catch ( Exception ex ) when ( ex.Message == "Object does not match target type." )
		{
			// This happens because of hotloading
			// Try loading stuff again
			LoadConnections();
		}
#endif
	}
}
