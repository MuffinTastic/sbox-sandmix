using Sandbox;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes.Mix.Audio;

[Display( Name = "Mix Output", Description = "Plays the output of the mix as a single ingame sound stream", GroupName = "Audio" )]
public class MixOutputNode : BaseMixNode
{
	[Browsable( false ), Input, JsonIgnore]
	public float[][] Output { get; set; }

	// ----- //

#if !SMIXTOOL
	Sound SoundOrigin;
	SoundStream SoundStream;

	public int Queued => SoundStream.QueuedSampleCount;

	public override Task Load()
	{
		NodeThrowIf( !Graph.FindTo( $"{Identifier}.Output" ).Any(), "Nothing connected" );
		
		SoundOrigin = Sound.FromScreen( "core.soundscape_2d" );
		SoundStream = SoundOrigin.CreateStream( SandMix.SampleRate, 2 ); // Why / 2?

		return SandMixUtil.CompletedTask;
	}

	public override void Unload()
	{
		SoundOrigin.Stop();

		SoundStream?.Delete();
		SoundStream = null;
	}

	public override void ProcessMix()
	{
		if ( Output is null ) return;

		var data = SandMixUtil.GetStreamSamples( Output );
		SoundStream.WriteData( data.AsSpan() );

		SetDoneProcessing();
	}
#else
	public override void ProcessMix()
	{
		throw new NotImplementedException();
	}
#endif
}
