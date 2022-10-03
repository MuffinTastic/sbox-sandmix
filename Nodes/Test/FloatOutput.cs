using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Test;

[Display( Name = "Float Output", Description = "Output", GroupName = "Test" )]
class FloatOutput : BaseNode
{
	[Browsable( false ), Input, JsonIgnore]
	public float Output { get; set; }

	// --- //

	public override void Update()
	{
		if ( SandMix.Debug )
			Log.Info( $"Output {Output}" );
	}
}
