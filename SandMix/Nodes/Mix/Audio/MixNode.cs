using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes.Mix.Audio;

[Display( Name = "#smix.node.mixnode", Description = "#smix.node.mixnode.description", GroupName = "#smix.node.category.audio" )]
public class MixNode : BaseMixGraphNode
{
	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.mixnode.line1" )]
	public float[][] Line1 { get; set; }

	[Sandbox.Range( min: 0.0f, max: 1.0f ), Input, Display( Name = "#smix.node.mixnode.line1volume", Description = "#smix.node.defaultvalue" )]
	public float Line1Volume { get; set; } = 1.0f;


	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.mixnode.line2" )]
	public float[][] Line2 { get; set; }

	[Sandbox.Range( min: 0.0f, max: 1.0f ), Input, Display( Name = "#smix.node.mixnode.line2volume", Description = "#smix.node.defaultvalue" )]
	public float Line2Volume { get; set; } = 1.0f;


	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.mixnode.line3" )]
	public float[][] Line3 { get; set; }

	[Sandbox.Range( min: 0.0f, max: 1.0f ), Input, Display( Name = "#smix.node.mixnode.line3volume", Description = "#smix.node.defaultvalue" )]
	public float Line3Volume { get; set; } = 1.0f;


	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.mixnode.line4" )]
	public float[][] Line4 { get; set; }

	[Sandbox.Range( min: 0.0f, max: 1.0f ), Input, Display( Name = "#smix.node.mixnode.line4volume", Description = "#smix.node.defaultvalue" )]
	public float Line4Volume { get; set; } = 1.0f;


	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.mixnode.line5" )]
	public float[][] Line5 { get; set; }

	[Sandbox.Range( min: 0.0f, max: 1.0f ), Input, Display( Name = "#smix.node.mixnode.line5volume", Description = "#smix.node.defaultvalue" )]
	public float Line5Volume { get; set; } = 1.0f;


	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.mixnode.line6" )]
	public float[][] Line6 { get; set; }

	[Sandbox.Range( min: 0.0f, max: 1.0f ), Input, Display( Name = "#smix.node.mixnode.line6volume", Description = "#smix.node.defaultvalue" )]
	public float Line6Volume { get; set; } = 1.0f;


	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.mixnode.line7" )]
	public float[][] Line7 { get; set; }

	[Sandbox.Range( min: 0.0f, max: 1.0f ), Input, Display( Name = "#smix.node.mixnode.line7volume", Description = "#smix.node.defaultvalue" )]
	public float Line7Volume { get; set; } = 1.0f;


	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.mixnode.line8" )]
	public float[][] Line8 { get; set; }

	[Sandbox.Range( min: 0.0f, max: 1.0f ), Input, Display( Name = "#smix.node.mixnode.line8volume", Description = "#smix.node.defaultvalue" )]
	public float Line8Volume { get; set; } = 1.0f;


	[Browsable( false ), Output, JsonIgnore, Display( Name = "#smix.node.output" )]
	public float[][] Output { get; set; }

	// --- //

	public override Task Load()
	{
		Output = SandMixUtil.CreateBuffers();

		return SandMixUtil.CompletedTask;
	}

	public override void ProcessMix()
	{
		for ( int i = 0; i < SandMix.Channels; i++ )
		{
			for ( int j = 0; j < SandMix.SampleSize; j++ )
			{
				Output[i][j] = 0.0f;

				if ( Line1 is not null )
					Output[i][j] += Line1[i][j] * Line1Volume;

				if ( Line2 is not null )
					Output[i][j] += Line2[i][j] * Line2Volume;
				
				if ( Line3 is not null )
					Output[i][j] += Line3[i][j] * Line3Volume;
				
				if ( Line4 is not null )
					Output[i][j] += Line4[i][j] * Line4Volume;
				
				if ( Line5 is not null )
					Output[i][j] += Line5[i][j] * Line5Volume;
				
				if ( Line6 is not null )
					Output[i][j] += Line6[i][j] * Line6Volume;
				
				if ( Line7 is not null )
					Output[i][j] += Line7[i][j] * Line7Volume;
				
				if ( Line8 is not null )
					Output[i][j] += Line8[i][j] * Line8Volume;

				Output[i][j] = Sample.Clamp( Output[i][j] );
			}
		}

		SetDoneProcessing();
	}
}
