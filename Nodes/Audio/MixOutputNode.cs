using Sandbox;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes.Audio;

[Display( Name = "Mix Output", Description = "Plays the output of the mix as a single ingame sound stream", GroupName = "Audio" )]
public class MixOutputNode : BaseAudio
{
	[Browsable( false ), Input, JsonIgnore]
	public AudioSamples Output { get; set; }

	// ----- //

#if !SMIXTOOL
	Sound SoundOrigin;
	SoundStream SoundStream;

	public int Queued => SoundStream.QueuedSampleCount;

	public override Task Load()
	{
		Output = new AudioSamples();

		SoundOrigin = Sound.FromScreen( "core.soundscape_2d" );
		SoundStream = SoundOrigin.CreateStream( SandMix.SampleRate / 2, 2 ); // Why / 2?

		return SandMixUtil.CompletedTask;
	}

	public override void Unload()
	{
		SoundOrigin.Stop();

		SoundStream.Delete();
		SoundStream = null;
	}

	public override void Update()
	{
		SoundStream.WriteData( Output.Samples.AsSpan() );
		//Log.Info( SoundStream.QueuedSampleCount );
	}
#endif
}
