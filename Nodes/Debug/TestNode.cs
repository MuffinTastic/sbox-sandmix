using SandMix.Nodes.Mix;
using System.ComponentModel.DataAnnotations;
using Sandbox;
using SandMix.Nodes.Effects;

namespace SandMix.Nodes.Debug;

[Display( Name = "Test", GroupName = "Debug" )]
public class DebugTestNode : BaseEffectImplementation
{
	public override void ProcessEffect( ref float[][] input, ref float[][] output )
	{
		for ( int i = 0; i < SandMix.Channels; i++ )
		{
			for ( int j = 0; j < SandMix.SampleSize; j++ )
			{
				output[i][j] = input[i][j] + 1;
			}
		}
	}
}
