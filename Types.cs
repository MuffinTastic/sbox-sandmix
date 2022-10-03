using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandMix;

public struct AudioSamples
{
	public short[] Samples = new short[SandMix.SampleSize];

	public AudioSamples()
	{

	}

	public void CopyFrom( ReadOnlySpan<short> samples )
	{
		samples.CopyTo( Samples.AsSpan() );
	}
}
