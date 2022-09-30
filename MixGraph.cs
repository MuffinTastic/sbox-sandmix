using Sandbox;
using SandMix.Nodes;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SandMix;

public class MixGraph
{
	public string File { get; private set; }

	private FileWatch GraphWatch;
	private GraphContainer Graph;

	public MixGraph( string file )
	{
		File = file;

		ReloadGraph();
	}

	private void ReloadGraph()
	{
		var data = FileSystem.Mounted.ReadAllText( File );

		try
		{
			Graph = GraphContainer.Deserialize( data );
		}
		catch ( JsonException ex )
		{
			Log.Error( $"Couldn't deserialize mixgraph {File}: {ex.Message}" );
		}

		Log.Info( $"Mixgraph {File} loaded" );
	}
}
