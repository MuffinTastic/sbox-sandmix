using NWaves.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes.Mix.Audio;

[Display( Name = "#smix.node.dynamics", Description = "#smix.node.dynamics.description", GroupName = "#smix.node.category.audio" )]
public class DynamicsNode : BaseMixGraphNode
{
	[Display( Name = "#smix.node.dynamics.mode" )]
	public DynamicsMode Mode { get; set; } = DynamicsMode.Compressor;

	[Display( Name = "#smix.node.dynamics.threshold" )]
	public float Threshold { get; set; }

	[Display( Name = "#smix.node.dynamics.ratio" )]
	public float Ratio { get; set; }

	[Display( Name = "#smix.node.dynamics.makeupgain" )]
	public float MakeupGain { get; set; } = 0.0f;

	[Display( Name = "#smix.node.dynamics.attack" )]
	public float Attack { get; set; } = 0.01f;

	[Display( Name = "#smix.node.dynamics.release" )]
	public float Release { get; set; } = 0.1f;

	[Display( Name = "#smix.node.dynamics.minamplitudedb" )]
	public float MinAmplitudeDb { get; set; } = -120.0f;

	// --- //

	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.input" )]
	public float[][] Input { get; set; }

	[Browsable( false ), Output, JsonIgnore, Display( Name = "#smix.node.output" )]
	public float[][] Output { get; set; }

	// --- //

	private DynamicsProcessor[] Processors;

	public override Task Load()
	{
		Output = SandMixUtil.CreateBuffers();
		Processors = new DynamicsProcessor[SandMix.Channels];

		for ( int i = 0; i < SandMix.Channels; i++ )
		{
			Processors[i] = new DynamicsProcessor( Mode, SandMix.SampleRate, Threshold, Ratio, MakeupGain, Attack, Release, MinAmplitudeDb );
		}

		return SandMixUtil.CompletedTask;
	}

	public override void ProcessMix()
	{
		for ( int i = 0; i < SandMix.Channels; i++ )
		{
			for ( int j = 0; j < SandMix.SampleSize; j++ )
			{
				Output[i][j] = Processors[i].Process( Input[i][j] );
			}
		}

		SetDoneProcessing();
	}
}
