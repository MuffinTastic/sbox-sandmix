using Sandbox;
using NWaves.Effects;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SandMix.Nodes.Effects.Effects;

[Display( Name = "#smix.node.echoeffect", Description = "#smix.node.echoeffect.description", GroupName = "#smix.node.category.effects" )]
public class EchoEffectNode : BaseEffectImplementation
{
	[Sandbox.Range( min: 0.0f, max: 5.0f ), Display( Name = "#smix.node.echoeffect.delay" )]
	public float Delay { get; set; } = 1.0f;

	[Sandbox.Range( min: 0.0f, max: 1.0f ), Display( Name = "#smix.node.echoeffect.feedback" )]
	public float Feedback { get; set; } = 0.5f;

	// --- //

	private EchoEffect[] Processors;

	public override Task Load()
	{
		Processors = new EchoEffect[SandMix.Channels];

		for ( int i = 0; i < SandMix.Channels; i++ )
		{
			Processors[i] = new EchoEffect( SandMix.SampleRate, Delay, Feedback );
		}

		return SandMixUtil.CompletedTask;
	}

	public override void ProcessEffect( ref float[][] input, ref float[][] output )
	{
		for ( int i = 0; i < SandMix.Channels; i++ )
		{
			for ( int j = 0; j < SandMix.SampleSize; j++ )
			{
				output[i][j] = Processors[i].Process( input[i][j] );
			}
		}
	}
}
