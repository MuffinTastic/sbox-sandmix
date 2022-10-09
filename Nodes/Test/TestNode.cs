using SandMix.Nodes.Mix;
using System.ComponentModel.DataAnnotations;
using Sandbox;

namespace SandMix.Nodes.Test;

[Display( Name = "Test", GroupName = "Test" )]
public class TestNode : BaseMixNode
{
	[Output, ResourceType("folder")]
	public string Testo { get; set; }

	[Output]
	public float AAWD { get; set; }
}
