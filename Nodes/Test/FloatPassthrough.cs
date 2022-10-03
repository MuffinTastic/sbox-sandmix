using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandMix.Nodes.Test;

[Display( Name = "Float Passthrough", Description = "Just passing through", GroupName = "Test" )]
class FloatPassthrough : BaseNode
{
	[Browsable( false ), Input, JsonIgnore]
	public float Input { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public float Output { get; set; }

	// --- //

	public override void Update()
	{
		Output = Input;

		if ( SandMix.Debug )
			Log.Info( $"Passthrough {Output}" );
	}
}
