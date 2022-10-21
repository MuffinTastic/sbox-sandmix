using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes.Mix.Maths;

[Display( Name = "#smix.node.floatrandom", Description = "#smix.node.floatrandom.description", GroupName = "#smix.node.category.maths" )]
public class FloatRandom : BaseMixNode
{
	[Display( Name = "#smix.node.floatrandom.min" )]
	public float Min { get; set; } = 0.0f;

	[Display( Name = "#smix.node.floatrandom.max" )]
	public float Max { get; set; } = 1.0f;

	// --- //

	[Browsable( false ), Output, JsonIgnore, Display( Name = "#smix.node.output" )]
	public float Output { get; set; }

	// --- //

	private float Range;
	private Random Random;

	public override Task Load()
	{
		Range = Max - Min;
		Random = new Random();

		return SandMixUtil.CompletedTask;
	}

	public override void ProcessMix()
	{
		var sample = Random.NextDouble();
		var scaled = (sample * Range) + Min;
		Output = (float) scaled;

		Log.Info( $"node {Name ?? GetType().Name}, output {Output}" );

		SetDoneProcessing();
	}
}
