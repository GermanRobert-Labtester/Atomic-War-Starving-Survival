// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using AtomicWar._Game.Shelter;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests
{
    public class NoiseDisciplineBenchmarkTests
    {
        private readonly ITestOutputHelper _output;

        public NoiseDisciplineBenchmarkTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private static NoiseDisciplineSystem.Source? LinearFind(IReadOnlyList<NoiseDisciplineSystem.Source> sources, string id)
        {
            if (sources == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < sources.Count; i++)
            {
                var s = sources[i];
                if (s != null && string.Equals(s.Id, id, StringComparison.Ordinal))
                    return s;
            }
            return null;
        }

        [Fact]
        public void Benchmark_LinearVsDictionarySourceLookup()
        {
            const int sourceCount = 100;
            const int iterations = 50000;

            var sys = new NoiseDisciplineSystem();
            var sourceIds = new List<string>(sourceCount);

            for (int i = 0; i < sourceCount; i++)
            {
                string id = $"noise_src_{i:D4}";
                sourceIds.Add(id);
                sys.RegisterSource(id, 10f + (i % 20), new Vector2Int(i, i * 2));
            }

            // Warmup
            long warmupSum = 0;
            for (int it = 0; it < 200; it++)
            {
                for (int i = 0; i < sourceIds.Count; i++)
                {
                    var s = sys.GetSource(sourceIds[i]);
                    if (s != null) warmupSum++;
                }
            }
            Assert.True(warmupSum > 0);

            // Baseline: Linear search via List.Find
            var swBaseline = Stopwatch.StartNew();
            long baselineMatches = 0;
            for (int it = 0; it < iterations; it++)
            {
                int idx = it % sourceCount;
                string queryId = sourceIds[idx];
                var s = sys.Current.Sources.Find(src => src.Id == queryId);
                if (s != null) baselineMatches++;
            }
            swBaseline.Stop();
            long baselineMs = swBaseline.ElapsedMilliseconds;

            // Optimized: Dictionary lookup via GetSource / _sourcesById
            var swOptimized = Stopwatch.StartNew();
            long optimizedMatches = 0;
            for (int it = 0; it < iterations; it++)
            {
                int idx = it % sourceCount;
                string queryId = sourceIds[idx];
                var s = sys.GetSource(queryId);
                if (s != null) optimizedMatches++;
            }
            swOptimized.Stop();
            long optimizedMs = swOptimized.ElapsedMilliseconds;

            // Correctness check: identical results
            Assert.Equal(baselineMatches, optimizedMatches);

            double speedup = baselineMs > 0 ? (double)baselineMs / Math.Max(1, optimizedMs) : 1.0;
            _output.WriteLine($"[Benchmark Results: Noise Source Lookup] Iterations: {iterations}, Sources: {sourceCount}");
            _output.WriteLine($"Baseline (Linear List.Find): {baselineMs} ms");
            _output.WriteLine($"Optimized (NoiseDisciplineSystem.GetSource): {optimizedMs} ms");
            _output.WriteLine($"Speedup factor: {speedup:F2}x faster");

            Assert.True(optimizedMs <= baselineMs, $"Optimized ({optimizedMs}ms) was not faster than baseline ({baselineMs}ms)");
        }

        [Fact]
        public void Benchmark_RegisterSourceAndUpdate()
        {
            const int sourceCount = 100;
            const int iterations = 30000;

            var sys = new NoiseDisciplineSystem();
            for (int i = 0; i < sourceCount; i++)
            {
                sys.RegisterSource($"src_{i:D4}", 15f);
            }

            var sw = Stopwatch.StartNew();
            for (int it = 0; it < iterations; it++)
            {
                int idx = it % sourceCount;
                string id = $"src_{idx:D4}";
                // Updating existing source exercises dictionary lookup
                sys.RegisterSource(id, 20f + (it % 5), new Vector2Int(idx, idx));
            }
            sw.Stop();

            _output.WriteLine($"[Benchmark Results: RegisterSource repeated update] Iterations: {iterations}, Elapsed: {sw.ElapsedMilliseconds} ms");
            Assert.Equal(sourceCount, sys.Current.Sources.Count);
        }

        [Fact]
        public void NoiseDisciplineSystem_CorrectnessAndEdgeCases()
        {
            var sys = new NoiseDisciplineSystem();

            // Null/empty safety
            sys.RegisterSource(null!, 50f);
            sys.RegisterSource(string.Empty, 50f);
            Assert.Empty(sys.Current.Sources);
            Assert.Null(sys.GetSource(null!));
            Assert.Null(sys.GetSource(string.Empty));

            // Register and update
            sys.RegisterSource("generator", 15f, new Vector2Int(1, 2));
            Assert.Equal(15f, sys.CurrentLevel);
            Assert.Single(sys.Current.Sources);
            var src = sys.GetSource("generator");
            Assert.NotNull(src);
            Assert.Equal(new Vector2Int(1, 2), src!.Position);

            // Update existing source
            sys.RegisterSource("generator", 25f, new Vector2Int(3, 4));
            Assert.Equal(25f, sys.CurrentLevel);
            Assert.Single(sys.Current.Sources);
            Assert.Equal(new Vector2Int(3, 4), sys.GetSource("generator")!.Position);

            // Add second source
            sys.RegisterSource("radio", 10f);
            Assert.Equal(35f, sys.CurrentLevel);
            Assert.Equal(2, sys.Current.Sources.Count);

            // Toggle active
            sys.SetSourceActive("generator", false);
            Assert.Equal(10f, sys.CurrentLevel);
            sys.SetSourceActive("generator", true);
            Assert.Equal(35f, sys.CurrentLevel);

            // Severity thresholds
            Assert.Equal("audible", sys.Severity);
            sys.RegisterSource("alarm", 40f); // 35 + 40 = 75
            Assert.Equal("loud", sys.Severity);
            Assert.Equal(0.35f, sys.GetRaidProbabilityDelta());

            // Save & restore
            var state = sys.CaptureState();
            var restored = new NoiseDisciplineSystem();
            restored.RestoreState(state);

            Assert.Equal(3, restored.Current.Sources.Count);
            Assert.Equal(75f, restored.CurrentLevel);
            Assert.NotNull(restored.GetSource("generator"));
            Assert.NotNull(restored.GetSource("radio"));
            Assert.NotNull(restored.GetSource("alarm"));

            // Verify lookup works after restore
            restored.SetSourceActive("alarm", false);
            Assert.Equal(35f, restored.CurrentLevel);
        }
    }
}
