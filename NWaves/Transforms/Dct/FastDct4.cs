﻿using System;

namespace NWaves.Transforms
{
    /// <summary>
    /// Represents Discrete Cosine Transform of Type-IV. 
    /// This FFT-based implementation of DCT-IV is faster for bigger DCT sizes.
    /// </summary>
    public class FastDct4 : IDct
    {
        /// <summary>
        /// Internal FFT transformer.
        /// </summary>
        private readonly Fft _fft;

        // Internal temporary buffers:
        
        private readonly float[] _temp;
        private readonly float[] _tempRe;
        private readonly float[] _tempIm;

        /// <summary>
        /// Gets size of DCT-IV.
        /// </summary>
        public int Size => 2 * _fft.Size;

        /// <summary>
        /// Constructs <see cref="FastDct4"/> of given <paramref name="dctSize"/>.
        /// </summary>
        /// <param name="dctSize">Size of DCT-IV</param>
        public FastDct4(int dctSize)
        {
            var halfSize = dctSize / 2;
            _fft = new Fft(halfSize);
            _temp = new float[halfSize];
            _tempRe = new float[halfSize];
            _tempIm = new float[halfSize];
        }

        /// <summary>
        /// Does DCT-IV.
        /// </summary>
        /// <param name="input">Input data</param>
        /// <param name="output">Output data</param>
        public void Direct(float[] input, float[] output)
        {
            Array.Clear(output, 0, output.Length);

            var N = Size;

            // mutiply by exp(-j * pi * n / N):

            for (var m = 0; m < _temp.Length; m++)
            {
                var re = input[2 * m];
                var im = input[N - 1 - 2 * m];
                var cos = Math.Cos(Math.PI * m / N);
                var sin = Math.Sin(-Math.PI * m / N);

                _temp[m] = 2 * (float)(re * cos - im * sin);
                output[m] = 2 * (float)(re * sin + im * cos);
            };

            _fft.Direct(_temp, output);

            // mutiply by exp(-j * pi * (2n + 0.5) / 2N):

            for (var m = 0; m < _temp.Length; m++)
            {
                var re = _temp[m];
                var im = output[m];
                var cos = Math.Cos(0.5 * Math.PI * (2 * m + 0.5) / N);
                var sin = Math.Sin(-0.5 * Math.PI * (2 * m + 0.5) / N);

                _tempRe[m] = (float)(re * cos - im * sin);
                _tempIm[m] = (float)(re * sin + im * cos);
            };

            for (int m = 0, k = 0; m < N; m += 2, k++)
            {
                output[m] = _tempRe[k];
            }
            for (int m = 1, k = (N - 2) / 2; m < N; m += 2, k--)
            {
                output[m] = -_tempIm[k];
            }
        }

        /// <summary>
        /// Does normalized DCT-IV.
        /// </summary>
        /// <param name="input">Input data</param>
        /// <param name="output">Output data</param>
        public void DirectNorm(float[] input, float[] output)
        {
            Direct(input, output);

            var norm = (float)(0.5 * Math.Sqrt(2.0 / Size));

            for (var i = 0; i < Size; output[i++] *= norm) ;
        }

        /// <summary>
        /// Does Inverse DCT-IV.
        /// </summary>
        /// <param name="input">Input data</param>
        /// <param name="output">Output data</param>
        public void Inverse(float[] input, float[] output)
        {
            Direct(input, output);
        }

        /// <summary>
        /// Does normalized Inverse DCT-IV.
        /// </summary>
        /// <param name="input">Input data</param>
        /// <param name="output">Output data</param>
        public void InverseNorm(float[] input, float[] output)
        {
            DirectNorm(input, output);
        }
    }
}
