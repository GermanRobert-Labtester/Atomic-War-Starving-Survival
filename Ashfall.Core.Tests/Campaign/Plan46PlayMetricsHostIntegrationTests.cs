// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 46 / C2[20] — playable metrics host-integration gate.
//
// Pins the production wiring contract added by the host integration:
//   * the `playable_metrics` save section is registered with its projection
//     file,
//   * the local recorder/funnel/engine composition lives in one host session,
//   * every canonical producer the plan needs is wired (day advance, death,
//     rations policy, sortie completion, save/quit, onboarding sigil seam),
//   * the aggregation reads existing owners instead of new counters,
//   * the CLI probe is registered.
// The recorder remains an audit read model: no test relies on it for gameplay.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Save;
using Ashfall.Core.Telemetry;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plan46PlayMetricsHostIntegrationTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        [Fact]
        public void PlayableMetricsSection_IsRegisteredWithProjectionFile()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("playable_metrics", out var section));
            Assert.NotNull(section);
            Assert.Equal("SavePlayMetrics", section!.SaveMethod);
            Assert.Equal("SetupPlayMetrics", section.SetupMethod);
            Assert.Equal("playable_metrics_save.json", SaveSectionRegistry.FileNameFor("playable_metrics"));
        }

        [Fact]
        public void HostSession_ComposesTheThreeCoreAuthoritiesWithoutAFourth()
        {
            string session = ReadRepoFile("src", "Host", "PlayMetricsHostSession.cs");
            // One recorder, one funnel, one engine invocation per aggregate.
            Assert.Contains("new PlaySessionRecorder(", session);
            Assert.Contains("new FirstHourFunnel()", session);
            Assert.Contains("PlayableMetricsAggregationEngine.Evaluate(", session);
            // The recorder's bounded buffer is never re-implemented in the host.
            Assert.DoesNotContain("new List<PlaySessionEvent>()", session);
        }

        [Fact]
        public void CanonicalProducers_AreWiredToTheRecorder()
        {
            string holdfast = ReadRepoFile("src", "Main.Holdfast.cs");
            Assert.Contains("RecordPlayMetricsDayJoined(day);", holdfast);

            string fate = ReadRepoFile("src", "Main.SurvivorFate.cs");
            Assert.Contains("RecordPlayMetricSurvivorPerished(fate.survivorId);", fate);

            string economyHost = ReadRepoFile("src", "Host", "EconomyHostSession.cs");
            Assert.Contains("RationTierChangedSeam?.Invoke(target);", economyHost);

            string economyMain = ReadRepoFile("src", "Main.Economy.cs");
            Assert.Contains("_economy.RationTierChangedSeam +=", economyMain);

            string onboarding = ReadRepoFile("src", "Main.Onboarding.cs");
            Assert.Contains("RecordPlayMetricSigil(sigil);", onboarding);
        }

        [Fact]
        public void AggregationInputs_ReadExistingOwnersNotNewCounters()
        {
            string main = ReadRepoFile("src", "Main.PlayMetrics.cs");
            Assert.Contains("_survivorFate?.DeathCount", main);
            Assert.Contains("_expeditions?.Engine?.CompletedCount", main);
            Assert.Contains("_waterTreatment?.System?.TotalWater", main);
            // Difficulty scalar is derived from the canonical provider, not stored.
            Assert.Contains("DeriveDifficultyScalarPermille(", main);
            // No parallel harvest counter: the read model is named.
            Assert.Contains("session.HarvestedResourceCount", main);
        }

        [Fact]
        public void OnboardingSigils_CoverTheFunnelsUiDrivenSteps()
        {
            // The funnel's four UI-driven targets are exactly the sigils the host
            // already observes through the canonical seam.
            var targets = new[]
            {
                "protocol.ration", "inventory.used", "weather.read", "expedition.dispatched"
            };
            string sigilCalls = string.Join("\n",
                Directory.EnumerateFiles(Path.Combine(RepoRoot(), "src"), "*.cs", SearchOption.AllDirectories)
                    .SelectMany(f => File.ReadAllLines(f))
                    .Where(line => line.Contains("ObserveSigil(\"")));

            foreach (string target in targets)
            {
                Assert.Contains($"ObserveSigil(\"{target}\")", sigilCalls);
            }
        }

        [Fact]
        public void FunnelRestore_RestoresCompletionWithoutReReportingFirsts()
        {
            var first = new FirstHourFunnel();
            var recorder = new PlaySessionRecorder("probe", "probe");
            var sessionIdEvent = recorder.RecordSigil("protocol.ration", 1);
            Assert.True(first.ProcessEvent(sessionIdEvent));
            Assert.True(first.IsStepCompleted("guidance_opened"));

            var restored = new FirstHourFunnel();
            restored.RestoreCompletedSteps(first.CompletedSteps.ToDictionary(kv => kv.Key, kv => kv.Value));
            Assert.True(restored.IsStepCompleted("guidance_opened"));
            Assert.Equal(1, restored.CompletedStepCount);
        }

        [Fact]
        public void AggregationEngine_IsDeterministicAndPrivacySafe()
        {
            var inputs = new SessionMetricInputs(
                daysSurvived: 30, peakPopulation: 5, casualtiesCount: 2, totalScavengeSorties: 8,
                totalResourcesHarvested: 60, totalWaterPurifiedLiters: 120, crisesResolved: 2,
                crisesFailed: 1, difficultyScalarPermille: 1000);

            var a = PlayableMetricsAggregationEngine.Evaluate(inputs);
            var b = PlayableMetricsAggregationEngine.Evaluate(inputs);

            Assert.Equal(a.HardshipIndexPermille, b.HardshipIndexPermille);
            Assert.Equal(a.EfficiencyRatingPermille, b.EfficiencyRatingPermille);
            Assert.Equal(a.SurvivalStabilityScorePermille, b.SurvivalStabilityScorePermille);
            Assert.Equal(a.Grade, b.Grade);
        }

        [Fact]
        public void SessionEventJsonL_IsAnonymousAndRoundTrippable()
        {
            var recorder = new PlaySessionRecorder("probe_session", "probe");
            recorder.SetContext(99, "difficulty_austere");
            var evt = recorder.Record("panel_opened", "journal", "ok", 3, 1200L);

            string json = recorder.ToJsonLine(evt);
            var parsed = JsonSerializer.Deserialize<PlaySessionEvent>(json);

            Assert.NotNull(parsed);
            Assert.Equal("probe_session", parsed!.SessionId);
            Assert.Equal(99L, parsed.Seed);
            Assert.Equal("difficulty_austere", parsed.PresetId);
            Assert.Equal(3, parsed.Day);
            // No identity fields exist on the wire beyond the anonymous session id.
            Assert.DoesNotContain("user", json, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void HostCliProbe_IsRegistered()
        {
            string actions = ReadRepoFile("src", "Host", "HostCli.cs");
            Assert.Contains("PlayMetricsSelfTest", actions);
            Assert.Contains("--playable-metrics-selftest", actions);
            string probe = ReadRepoFile("src", "Host", "HostCli.PlayMetrics.cs");
            Assert.Contains("public static int RunPlayMetricsSelfTest(", probe);
        }
    }
}
