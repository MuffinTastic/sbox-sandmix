using System.Collections.Generic;


namespace NWaves.FeatureExtractors.Options
{
    /// <summary>
    /// Defines properties for configuring multi-feature extractors. 
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
    ///     <item>FeatureList = "all"</item>
    /// </list>
    /// </para>
    /// </summary>
    public class MultiFeatureOptions : FeatureExtractorOptions
    {
        public string FeatureList { get; set; } = "all";
        public int FftSize { get; set; }
        public float[] Frequencies { get; set; }
        public (double, double, double)[] FrequencyBands { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
