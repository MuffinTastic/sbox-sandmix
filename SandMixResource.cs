using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandMix;

public abstract class SandMixResource : GameResource
{
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
