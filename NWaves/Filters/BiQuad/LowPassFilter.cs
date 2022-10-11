﻿using System;

namespace NWaves.Filters.BiQuad
{
    /// <summary>
    /// Represents BiQuad lowpass filter.
    /// </summary>
    public class LowPassFilter : BiQuadFilter
    {
        /// <summary>
        /// Gets cutoff frequency.
        /// </summary>
        public double Frequency { get; protected set; }

        /// <summary>
        /// Gets Q factor.
        /// </summary>
        public double Q { get; protected set; }

        /// <summary>
        /// Constructs <see cref="LowPassFilter"/>.
        /// </summary>
        /// <param name="frequency">Normalized cutoff frequency in range [0..0.5]</param>
        /// <param name="q">Q factor</param>
        public LowPassFilter(double frequency, double q = 1)
        {
            SetCoefficients(frequency, q);
        }

        /// <summary>
        /// Sets filter coefficients.
        /// </summary>
        /// <param name="frequency">Normalized cutoff frequency in range [0..0.5]</param>
        /// <param name="q">Q factor</param>
        private void SetCoefficients(double frequency, double q)
        {
            // The coefficients are calculated automatically according to 
            // audio-eq-cookbook by R.Bristow-Johnson and WebAudio API.

            Frequency = frequency;
            Q = q;

            var omega = 2 * Math.PI * frequency;
            var alpha = Math.Sin(omega) / (2 * q);
            var cosw = Math.Cos(omega);

            _b[0] = (float)((1 - cosw) / 2);
            _b[1] = (float)(1 - cosw);
            _b[2] = _b[0];

            _a[0] = (float)(1 + alpha);
            _a[1] = (float)(-2 * cosw);
            _a[2] = (float)(1 - alpha);

            Normalize();
        }

        /// <summary>
        /// Changes filter coefficients online (preserving the state of the filter).
        /// </summary>
        /// <param name="frequency">Normalized cutoff frequency in range [0..0.5]</param>
        /// <param name="q">Q factor</param>
        public void Change(double frequency, double q = 1)
        {
            SetCoefficients(frequency, q);
        }
    }
}