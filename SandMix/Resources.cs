using System;
using Sandbox;
#if SMIXTOOL
using Tools;
#endif
using SandMix.Nodes;

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

#if SMIXTOOL
	public static (SandMixResource, GraphType) GetFromAsset( Asset asset )
	{
		var graphType = asset.AssetType.FileExtension switch
		{
			MixGraphResource.FileExtension => GraphType.Mix,
			EffectGraphResource.FileExtension => GraphType.Effect,
			_ => throw new Exception( "Unknown graph type" )
		};

		SandMixResource resource = graphType switch
		{
			GraphType.Mix => asset.LoadResource<MixGraphResource>(),
			GraphType.Effect => asset.LoadResource<EffectGraphResource>(),
			_ => throw new Exception( "Unknown graph type" )
		};

		return (resource, graphType);
	}
#endif
}

[GameResource( $"{SandMix.ProjectName} mix graph", FileExtension, $"Mix graph for live audio processing with {SandMix.ProjectName}", Icon = Icon )]
public partial class MixGraphResource : SandMixResource
{
	public const string Icon = "account_tree";
	public const string FileExtension = "smix";
}


[GameResource( $"{SandMix.ProjectName} effect graph", FileExtension, $"Effect preset for {SandMix.ProjectName}", Icon = Icon )]
public partial class EffectGraphResource : SandMixResource
{
	public const string Icon = "leak_add";
	public const string FileExtension = "smixefct";
}
