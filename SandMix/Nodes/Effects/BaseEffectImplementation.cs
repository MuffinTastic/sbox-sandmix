using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes.Effects;

public abstract class BaseEffectImplementation : BaseEffectGraphNode
{
	[Browsable( false ), Input, JsonIgnore, Display( Name = "#smix.node.input" )]
	public float[][] Input { get; set; }

	[Browsable( false ), Output, JsonIgnore, Display( Name = "#smix.node.output" )]
	public float[][] Output { get; set; }

	public abstract void ProcessEffect( ref float[][] input, ref float[][] output );
}
