using Sandbox;
using Sandbox.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SandMix.Nodes.Mix;

public abstract class BaseMixNode : BaseNode
{
	public static uint GetInputId( PropertyDescription input ) => Crc32.FromString( input.Name );

	protected Dictionary<uint, bool> ReadyInputs = new();

	/// <summary>
	/// Are all of the inputs ready for processing?
	/// </summary>
	[Browsable( false ), JsonIgnore]
	public bool IsReady => ReadyInputs.Values.All( x => x );

	/// <summary>
	/// Register this input as connected, so that processing can wait on it
	/// </summary>
	/// <param name="input"></param>
	public void AddReadyInput( PropertyDescription input )
	{
		Assert.NotNull( input.GetCustomAttribute<InputAttribute>() );

		var crc = GetInputId( input );
		if ( ReadyInputs.ContainsKey( crc ) )
		{
			// might happen due to reload
			ReadyInputs[crc] = false;
		}
		else
		{
			ReadyInputs.Add( crc, false );
		}
	}

	public bool GetReady( uint crc )
	{
		if ( ReadyInputs.ContainsKey( crc ) )
		{
			return ReadyInputs[crc];
		}

		return true;
	}

	public void SetReady( uint crc )
	{
		if ( ReadyInputs.ContainsKey( crc ) )
		{
			ReadyInputs[crc] = true;
		}
	}

	/// <summary>
	/// Have we already done work?
	/// </summary>
	[Browsable( false ), JsonIgnore]
	public bool DoneProcessing { get; private set; } = false;

	/// <summary>
	/// Flag ourselves as done so we don't do more work in the current pass
	/// </summary>
	protected void SetDoneProcessing()
	{
		DoneProcessing = true;
	}

	/// <summary>
	/// Reset input and processing status for this node
	/// </summary>
	public void Reset()
	{
		foreach ( var input in ReadyInputs )
		{
			ReadyInputs[input.Key] = false;
		}

		DoneProcessing = false;
	}


	public abstract void ProcessMix();
}
