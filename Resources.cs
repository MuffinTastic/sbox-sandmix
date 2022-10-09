using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandMix;

public abstract class SandMixResource : GameResource
{
	[HideInEditor]
	public string JsonData { get; set; }

	public event Action PostLoadEvent;
	public event Action PostReloadEvent;

	protected override void PostLoad()
	{
		PostLoadEvent?.Invoke();
	}

	protected override void PostReload()
	{
		PostReloadEvent?.Invoke();
	}
}

[GameResource( "s&mix mixgraph", "smix", "Mixgraph for live audio processing with s&mix", Icon = Icon )]
public partial class MixGraphResource : SandMixResource
{
	public const string Icon = "account_tree";
	public const string FileExtension = "smix";
}


[GameResource( "s&mix effect", "smixefct", "Effect preset for s&mix", Icon = Icon )]
public partial class EffectResource : SandMixResource
{
	public const string Icon = "leak_add";
	public const string FileExtension = "smixefct";
}
