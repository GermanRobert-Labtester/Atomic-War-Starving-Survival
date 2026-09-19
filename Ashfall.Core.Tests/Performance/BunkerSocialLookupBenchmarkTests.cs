// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests
{
    public class BunkerSocialLookupBenchmarkTests
    {
        private readonly ITestOutputHelper _output;

        public BunkerSocialLookupBenchmarkTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public class MockSurvivor
        {
            public string Id { get; set; } = string.Empty;
            public bool IsAlive { get; set; } = true;
            public float Morale { get; set; } = 75f;
        }

        public struct SocialPair
        {
            public string A;
            public string B;

            public SocialPair(string a, string b)
            {
                A = a;
                B = b;
            }
        }

        private static MockSurvivor? LinearFind(IReadOnlyList<MockSurvivor> survivors, string id)
        {
            if (survivors == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s != null && string.Equals(s.Id, id, StringComparison.Ordinal))
                    return s;
            }
            return null;
        }

        private static Dictionary<string, MockSurvivor> BuildLookup(IReadOnlyList<MockSurvivor> survivors)
        {
            if (survivors == null) return new Dictionary<string, MockSurvivor>(0, StringComparer.Ordinal);
            var dict = new Dictionary<string, MockSurvivor>(survivors.Count, StringComparer.Ordinal);
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s != null && !string.IsNullOrEmpty(s.Id) && !dict.ContainsKey(s.Id))
                {
                    dict[s.Id] = s;
                }
            }
            return dict;
        }

        [Fact]
        public void Benchmark_LinearVsDictionaryLookup()
        {
            const int survivorCount = 100;
            const int pairCount = 50;
            const int iterations = 10000;

            var survivors = new List<MockSurvivor>(survivorCount);
            for (int i = 0; i < survivorCount; i++)
            {
                survivors.Add(new MockSurvivor { Id = $"surv_{i:D4}", IsAlive = true, Morale = 80f });
            }

            var pairs = new List<SocialPair>(pairCount);
            for (int i = 0; i < pairCount; i++)
            {
                pairs.Add(new SocialPair($"surv_{i:D4}", $"surv_{(survivorCount - 1 - i):D4}"));
            }

            // Warmup
            long warmupSum = 0;
            for (int it = 0; it < 100; it++)
            {
                foreach (var pair in pairs)
                {
                    var a = LinearFind(survivors, pair.A);
                    var b = LinearFind(survivors, pair.B);
                    if (a != null && b != null) warmupSum++;
                }
            }
            Assert.True(warmupSum > 0);

            // Baseline: Linear search inside loop
            var swBaseline = Stopwatch.StartNew();
            long baselineMatches = 0;
            for (int it = 0; it < iterations; it++)
            {
                foreach (var pair in pairs)
                {
                    var a = LinearFind(survivors, pair.A);
                    var b = LinearFind(survivors, pair.B);
                    if (a != null && b != null)
                    {
                        baselineMatches++;
                    }
                }
            }
            swBaseline.Stop();
            long baselineMs = swBaseline.ElapsedMilliseconds;

            // Optimized: Dictionary lookup table built once per tick/iteration
            var swOptimized = Stopwatch.StartNew();
            long optimizedMatches = 0;
            for (int it = 0; it < iterations; it++)
            {
                var lookup = BuildLookup(survivors);
                foreach (var pair in pairs)
                {
                    lookup.TryGetValue(pair.A, out var a);
                    lookup.TryGetValue(pair.B, out var b);
                    if (a != null && b != null)
                    {
                        optimizedMatches++;
                    }
                }
            }
            swOptimized.Stop();
            long optimizedMs = swOptimized.ElapsedMilliseconds;

            // Correctness check: identical behavior and match counts
            Assert.Equal(baselineMatches, optimizedMatches);

            double speedup = baselineMs > 0 ? (double)baselineMs / Math.Max(1, optimizedMs) : 1.0;
            _output.WriteLine($"[Benchmark Results] Iterations: {iterations}, Survivors: {survivorCount}, Pairs: {pairCount}");
            _output.WriteLine($"Baseline (Linear Find): {baselineMs} ms");
            _output.WriteLine($"Optimized (Dictionary Lookup): {optimizedMs} ms");
            _output.WriteLine($"Speedup factor: {speedup:F2}x faster");

            // Optimized must be measurably faster
            Assert.True(optimizedMs <= baselineMs, $"Optimized ({optimizedMs}ms) was not faster than baseline ({baselineMs}ms)");
        }
    }
}
