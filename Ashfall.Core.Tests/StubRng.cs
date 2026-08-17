using System;
using System.Collections.Generic;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// A fully scripted ISeededRng so combat/ballistic tests can pin every single
    /// roll and assert exact branch behaviour deterministically. Values are
    /// consumed in FIFO order; Next uses the next double, Next() wraps.
    /// </summary>
    public sealed class StubRng : ISeededRng
    {
        private readonly Queue<double> _q = new Queue<double>();
        public int Seed { get; }
        public StubRng(int seed, params double[] values)
        {
            Seed = seed;
            foreach (var v in values) _q.Enqueue(v);
        }
        public double NextDouble() => _q.Count > 0 ? _q.Dequeue() : 0.5;
        public float NextFloat() => (float)NextDouble();
        public int Next(int minInclusive, int maxExclusive)
        {
            // Return minInclusive deterministically unless an override is queued.
            if (_q.Count > 0) return minInclusive;
            return minInclusive;
        }
    }
}
