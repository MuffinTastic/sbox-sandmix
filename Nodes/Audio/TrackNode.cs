using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sandbox;

namespace SandMix.Nodes.Audio;

[Display( Name = "Track", Description = "Audio track - Sources from a .vsnd file", GroupName = "Audio" )]
public class TrackNode : BaseAudio
{
	[ResourceType( "vsnd" )]
	public string Track { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public AudioSamples Output { get; set; }

	// ----- //

	private SoundFile SoundFile = null;
	private short[] TrackSamples = null;

	private int CurrentPosition = 0;

	public override async Task Load()
	{
		Output = new AudioSamples();

		NodeThrowIf( string.IsNullOrEmpty( Track ), "Sound file missing" );

		SoundFile = SoundFile.Load( Track );
		
		if ( !await SoundFile.LoadAsync() )
		{
			Log.Warning( $"MixGraph couldn't load track {Track}" );
			return;
		}

		TrackSamples = await SoundFile.GetSamplesAsync();

		Log.Info( $"Loaded track {Track}, rate {SoundFile.Rate} channels {SoundFile.Channels}" );
	}

	public override void Update()
	{
		if ( TrackSamples is null ) return;

		var samples = TrackSamples.AsSpan();
		var copyLength = Math.Min( samples.Length - CurrentPosition, SandMix.SampleSize );
		Output.CopyFrom( samples.Slice( CurrentPosition, copyLength ) );

		CurrentPosition += SandMix.SampleSize;
		if ( CurrentPosition > TrackSamples.Length )
			CurrentPosition = 0;
	}
}
