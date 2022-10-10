using SandMix.Nodes.Mix;
using System.ComponentModel.DataAnnotations;
using Sandbox;
using SandMix.Nodes.Effects;

namespace SandMix.Nodes.Test;

[Display( Name = "Test", GroupName = "Test" )]
public class TestNode : BaseEffectImplementation
{
	public override void ProcessEffect( ref float[] input, ref float[] output )
	{
		for ( int i = 0; i < SandMix.SampleSize; i++ )
		{
			output[i] = input[i] + 1;
		}
	}
}
