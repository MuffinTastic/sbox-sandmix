using SandMix.Nodes.Mix;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Debug;

[Display( Name = "Debug Float Passthrough", Description = "Just passing through", GroupName = "#smix.node.category.debug" )]
class DebugFloatPassthrough : BaseMixGraphNode
{
	[Browsable( false ), Input, JsonIgnore]
	public float Input { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public float Output { get; set; }

	// --- //

	public override void ProcessMix()
	{
		Output = Input;

		if ( SandMix.Debug )
			Log.Info( $"Passthrough {Output}" );

		SetDoneProcessing();
	}
}
