using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes.Effects;

public abstract class BaseEffectImplementation : BaseEffectNode
{
	[Browsable( false ), Input, JsonIgnore]
	public AudioSamples Input { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public AudioSamples Output { get; set; }

	public abstract void ProcessEffect( ref float[] input, ref float[] output );
}
