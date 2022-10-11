using System.Collections.Generic;

namespace NWaves.FeatureExtractors.Options
{
    /// <summary>
    /// Defines properties for configuring <see cref="AmsExtractor"/>. 
    /// General contracts are:
    /// <list type="bullet">
    ///     <item>Sampling rate must be positive number</item>
    ///     <item>Frame duration must be positive number</item>
    ///     <item>Hop duration must be positive number</item>
    /// </list>
    /// <para>
    /// Default values:
    /// <list type="bullet">
    ///     <item>FrameDuration = 0.025</item>
    ///     <item>HopDuration = 0.01</item>
    ///     <item>Window = WindowType.Rectangular</item>
    ///     <item>ModulationFftSize = 64</item>
    ///     <item>ModulationHopSize = 4</item>
    /// </list>
    /// </para>
    /// </summary>
    public class AmsOptions : FeatureExtractorOptions
    {
        public int ModulationFftSize { get; set; } = 64;
        public int ModulationHopSize { get; set; } = 4;
        public int FftSize { get; set; }
        public IEnumerable<float[]> Featuregram { get; set; }
        public float[][] FilterBank { get; set; }
    }
}
