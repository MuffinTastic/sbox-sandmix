using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sandbox;

namespace SandMix.Nodes.Mix.Audio;

[Display( Name = "Track", Description = "Audio track - Sources from a .vsnd file", GroupName = "Audio" )]
public class TrackNode : BaseMixNode
{
	[ResourceType( "vsnd" )]
	public string Track { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public float[][] Output { get; set; }

	// ----- //

	private SoundData SoundData = null;
	private float[][] SoundSamples = null;

	private int CurrentPosition = 0;

	public override async Task Load()
	{
		Output = SandMixUtil.CreateBuffers();

		NodeThrowIf( string.IsNullOrEmpty( Track ), "Sound file missing" );

		SoundData = SoundLoader.LoadSamples( Track );
		SoundSamples = SandMixUtil.GetSoundChannels( SoundData );

		if ( SandMix.Debug )
			Log.Info( $"Loaded track {Track}, rate {SoundData.SampleRate} channels {SoundData.Channels}" );
	}

	public override void Update()
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
	}
}
