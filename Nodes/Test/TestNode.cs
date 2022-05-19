using System.ComponentModel.DataAnnotations;
using Sandbox;

namespace SandMix.Nodes.Test;

[Library, Display( Name = "Test", GroupName = "Test" )]
public class TestNode : BaseNode
{
	[Output, ResourceType("folder")]
	public string Testo { get; set; }

	[Output]
	public float AAWD { get; set; }
}
