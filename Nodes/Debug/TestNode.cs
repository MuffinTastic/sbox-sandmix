﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Sandbox;
using SandMix.Nodes.Mix;

namespace SandMix.Nodes.Debug;

[Display( Name = "Test", GroupName = "Debug" )]
public class DebugTestNode : BaseMixNode
{
	[ResourceType( "vsnd" )]
	public string Track { get; set; }
	[ResourceType( EffectResource.FileExtension )]
	public string Effect { get; set; }
	public float RangedThingy { get; set; }

	// --- //

	[Browsable( false ), Input, JsonIgnore]
	public float Output { get; set; }

	// --- //

	public override void ProcessMix()
	{


		SetDoneProcessing();
	}
}
