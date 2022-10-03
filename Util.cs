using Sandbox;
using System.Threading.Tasks;

namespace SandMix;

public static class SandMixUtil
{
	public static Task CompletedTask =>
#if SMIXTOOL
		Task.CompletedTask;
#else
		GameTask.CompletedTask;
#endif

}
