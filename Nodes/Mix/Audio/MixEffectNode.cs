﻿using Sandbox;
using SandMix.Nodes.Effects;
using SandMix.Nodes.Effects.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes.Mix.Audio;


[Display( Name = "Effect", Description = "Apply an effect to the audio stream", GroupName = "Audio" )]
public class MixEffectNode : BaseMixNode
{
	[Browsable( false ), Input, JsonIgnore]
	public float[][] Input { get; set; }

	[Browsable( false ), Output, JsonIgnore]
	public float[][] Output { get; set; }

	[ResourceType( EffectResource.FileExtension )]
	public string Effect { get; set; }

	// --- //

	private EffectResource Resource;
	private List<BaseEffectImplementation> EffectNodes;

	private float[][][] Buffers;

#if !SMIXTOOL
	public override Task Load()
	{
		Output = SandMixUtil.CreateBuffers();

		NodeThrowIf( string.IsNullOrEmpty( Effect ), "Effect missing" );

		Resource = ResourceLibrary.Get<EffectResource>( Effect );
		NodeThrowIf( Resource is null, $"Couldn't load effect {Effect}" );

		Buffers = new float[2][][];

		for ( int i = 0; i < 2; i++ )
		{
			Buffers[i] = SandMixUtil.CreateBuffers();
		}

		Resource.PostReloadEvent += Reload;

		Reload();

		return SandMixUtil.CompletedTask;
	}

	private void Reload()
	{
		if ( EffectNodes is null )
		{
			EffectNodes = new();
		}
		else
		{
			EffectNodes.Clear();
		}

		var graph = GraphContainer.Deserialize( Resource.JsonData );
		NodeThrowIf( graph.GraphType != GraphType.Effect, $"{Effect} is not an effect graph" );
		
		var inputs = graph.Nodes.OfType<EffectInput>();
		NodeThrowIf( inputs.Count() != 1, $"{Effect} must have one input node" );

		var outputs = graph.Nodes.OfType<EffectOutput>();
		NodeThrowIf( outputs.Count() != 1, $"{Effect} must have one output node" );
		
		BaseNode inputNode = inputs.First();
		BaseNode outputNode = outputs.First();

		BaseNode currentNode = inputNode;

		// add nodes in the order they're connected
		while ( currentNode != outputNode )
		{
			string plug = currentNode == inputNode ? "Input" : "Output";
			var connections = graph.FindFrom( $"{currentNode.Identifier}.{plug}" );
			NodeThrowIf( connections.Count != 1, $"{Effect} must have a valid connection" );
			var connection = connections.First();

			var split2 = connection.Item2.Split( '.', 2 );
			currentNode = graph.Find( split2[0] );
			NodeThrowIf( currentNode is null, $"{Effect} must have a valid connection" );

			if ( currentNode != inputNode && currentNode != outputNode)
			{
				var effectNode = currentNode as BaseEffectImplementation;
				NodeThrowIf( effectNode is null, $"{Effect} contains a non-effect node" );

				_ = effectNode.Load();
				EffectNodes.Add( effectNode );
			}
		}
	}

	public override void Unload()
	{
		EffectNodes?.Clear();
		EffectNodes = null;
	}

	public override void Update()
	{
		if ( Input is null || Output is null || Buffers is null )
		{
			return;
		}

		if ( !EffectNodes.Any() )
		{
			for ( int i = 0; i < SandMix.Channels; i++ )
			{
				Input[i].CopyTo( Output[i].AsSpan() );
			}

			return;
		}

		ref var inputArray = ref Buffers[0];
		ref var outputArray = ref Buffers[1];

		for ( int i = 0; i < SandMix.Channels; i++ )
		{
			Input[i].CopyTo( inputArray[i].AsSpan() );
		}

		for ( int i = 0; i < EffectNodes.Count; i++ )
		{
			if ( i != 0 )
			{
				// swap array references
				var temp = inputArray;
				inputArray = outputArray;
				outputArray = temp;
			}

			EffectNodes[i].ProcessEffect( ref inputArray, ref outputArray );
		}

		for ( int i = 0; i < SandMix.Channels; i++ )
		{
			outputArray[i].CopyTo( Output[i].AsSpan() );
		}
	}
#endif
}
