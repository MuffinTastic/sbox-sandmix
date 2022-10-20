using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes.Mix.Maths;

[Display( Name = "Random Float", Description = "Generate a random float number within a range", GroupName = "#smix.node.category.maths" )]
public class FloatRandom : BaseMixNode
{
	public float Min { get; set; } = 0.0f;
	public float Max { get; set; } = 1.0f;

	// --- //

	[Browsable( false ), Output, JsonIgnore]
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
