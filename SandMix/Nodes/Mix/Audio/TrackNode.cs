using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sandbox;

namespace SandMix.Nodes.Mix.Audio;

[Display( Name = "#smix.node.tracknode", Description = "#smix.node.tracknode.description", GroupName = "#smix.node.category.audio" )]
public class TrackNode : BaseMixNode
{
	[ResourceType( "vsnd" ), Display( Name = "#smix.node.tracknode.track" )]
	public string Track { get; set; }

	// --- //

	[Browsable( false ), Output, JsonIgnore, Display( Name = "#smix.node.output" )]
	public float[][] Output { get; set; }

	// --- //

	private SoundData SoundData = null;
	private float[][] SoundSamples = null;

	private int CurrentPosition = 0;

	public override Task Load()
	{
		NodeThrowIf( string.IsNullOrEmpty( Track ), "Sound file missing" );

		Output = SandMixUtil.CreateBuffers();

		SoundData = SoundLoader.LoadSamples( Track );
		SoundSamples = SandMixUtil.GetSoundChannels( SoundData );

		if ( SandMix.Debug )
			Log.Info( $"Loaded track {Track}, rate {SoundData.SampleRate} channels {SoundData.Channels}" );

		return SandMixUtil.CompletedTask;
	}

	public override void ProcessMix()
	{
		if ( SoundSamples is null ) return;

		var copyLength = Math.Min( SoundSamples[0].Length - CurrentPosition, SandMix.SampleSize );

		for ( int i = 0; i < SandMix.Channels; i++ )
		{
			SoundSamples[i].AsSpan().Slice( CurrentPosition, copyLength ).CopyTo( Output[i] );
		}

		CurrentPosition += SandMix.SampleSize;
		if ( CurrentPosition > SoundSamples[0].Length )
			CurrentPosition = 0;

		SetDoneProcessing();
	}
}
