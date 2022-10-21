using SandMix.Nodes.Mix;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes.Debug;

[Display( Name = "Debug Audio Passthrough", GroupName = "#smix.node.category.debug" )]
public class DebugAudioPassthrough : BaseMixGraphNode
{
	[Browsable( false ), Input, JsonIgnore]
	public float[][] AudioInput { get; set; }

	[Input, JsonIgnore]
	public float EmptyInput { get; set; }

	[Browsable( false ), Input, JsonIgnore]
	public Vector3 Other { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public float[][] AudioOutput { get; set; }

	public override void ProcessMix()
	{
		AudioOutput = AudioInput;

		Log.Info( $"node {Name}, empty {EmptyInput} other {Other}" );

		SetDoneProcessing();
	}
}

