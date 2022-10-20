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

[Display( Name = "Dynamics", Description = "Apply dynamics to the audio stream (compression/limiting/expansion/noise gate)", GroupName = "#smix.node.category.audio" )]
public class DynamicsNode : BaseMixNode
{
	public DynamicsMode Mode { get; set; } = DynamicsMode.Compressor;

	public float Threshold { get; set; }
	
	public float Ratio { get; set; }

	public float MakeupGain { get; set; } = 0.0f;

	public float Attack { get; set; } = 0.01f;

	public float Release { get; set; } = 0.1f;

	public float MinAmplitudeDb { get; set; } = -120.0f;

	// --- //

	[Browsable( false ), Input, JsonIgnore]
	public float[][] Input { get; set; }

	[Browsable( false ), Output, JsonIgnore]
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
