using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;
using SandMix.Nodes.Mix;

namespace SandMix.Nodes.Debug;

[Display( Name = "Debug Float Output", Description = "Output", GroupName = "#smix.node.category.debug" )]
class DebugFloatOutput : BaseMixGraphNode
{
	[Browsable( false ), Input, JsonIgnore]
	public float Output { get; set; }

	// --- //

	public override void ProcessMix()
	{
		if ( SandMix.Debug )
			Log.Info( $"Output {Output}" );

		SetDoneProcessing();
	}
}
