// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 54 — seven-day slice playtest instrument host-integration gate.
//
// Pins the production wiring contract:
//   * the authored scenario loads through a strict Core loader (snake_case data
//     authority), and an incomplete scenario is an error rather than a default,
//   * the freeze hash identifies the exact authored content,
//   * beats pass only on a matching action + outcome, and unknown days fail,
//   * the scorecard requires every beat plus retained survivors,
//   * the evidence store round-trips the scorecard,
//   * the probe is registered.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plan54SevenDaySliceHostIntegrationTests
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

        private static Ashfall.Core.Campaign.SliceScenarioData LoadAuthored()
        {
            var result = Ashfall.Core.Campaign.SliceScenarioCatalogLoader.Load(
                Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data"), new Ashfall.Core.FileSystemIO());
            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.NotNull(result.Scenario);
            return result.Scenario!;
        }

        [Fact]
        public void AuthoredScenario_LoadsWithSevenBeatsThroughTheStrictLoader()
        {
            var data = LoadAuthored();

            Assert.Equal("slice_seven_days_v1", data.ScenarioId);
            Assert.Equal(7, data.TargetDays);
            Assert.Equal(7, data.Beats.Count);
            Assert.All(data.Beats, b =>
            {
                Assert.False(string.IsNullOrWhiteSpace(b.BeatId));
                Assert.False(string.IsNullOrWhiteSpace(b.ActionKey));
                Assert.False(string.IsNullOrWhiteSpace(b.ExpectedOutcome));
                Assert.False(string.IsNullOrWhiteSpace(b.RequiredSystem));
                Assert.True(b.Day is >= 1 and <= 7);
            });
            // One beat per day, in order.
            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6, 7 }, data.Beats.Select(b => b.Day).ToArray());
        }

        [Fact]
        public void Loader_RejectsAnIncompleteScenario()
        {
            string temp = Path.Combine(Path.GetTempPath(), "ashfall_slice_invalid_" + Guid.NewGuid().ToString("n"));
            Directory.CreateDirectory(temp);
            try
            {
                File.WriteAllText(Path.Combine(temp, "slice_seven_days.json"),
                    "{\"schema_version\":1,\"scenario_id\":\"broken\",\"target_days\":0,\"beats\":[]}");
                var result = Ashfall.Core.Campaign.SliceScenarioCatalogLoader.Load(
                    temp, new Ashfall.Core.FileSystemIO());

                Assert.True(result.HasErrors);
                Assert.Null(result.Scenario);
            }
            finally
            {
                Directory.Delete(temp, true);
            }
        }

        [Fact]
        public void FreezeHash_IdentifiesTheExactAuthoredContent()
        {
            var first = new Ashfall.Core.Campaign.SliceScenario(LoadAuthored());
            string hash = first.ComputeScenarioHash();

            Assert.False(string.IsNullOrWhiteSpace(hash));
            Assert.Equal(hash, first.ComputeScenarioHash());
            Assert.True(first.IsScenarioFrozen(hash));
            Assert.False(first.IsScenarioFrozen(new string('0', hash.Length)));

            var second = new Ashfall.Core.Campaign.SliceScenario(LoadAuthored());
            Assert.Equal(hash, second.ComputeScenarioHash());
        }

        [Fact]
        public void Beats_VerifyActionAndOutcomeAndFailLoudly()
        {
            var slice = new Ashfall.Core.Campaign.SliceScenario(LoadAuthored());

            Assert.True(slice.EvaluateDayBeat(1, "ration_distribution", "NeedsEvaluated", out _));
            Assert.False(slice.EvaluateDayBeat(1, "wrong_action", "NeedsEvaluated", out _));
            Assert.False(slice.EvaluateDayBeat(1, "ration_distribution", "WrongOutcome", out _));
            Assert.False(slice.EvaluateDayBeat(99, "ration_distribution", "NeedsEvaluated", out _));

            var scorecard = slice.CompleteSlice(3);
            Assert.Equal(1, scorecard.CompletedBeats);
            Assert.Equal(7, scorecard.TotalBeats);
            Assert.False(scorecard.Passed, "a partial slice must not pass");
        }

        [Fact]
        public void Scorecard_RequiresEveryBeatAndRetainedSurvivors()
        {
            var slice = new Ashfall.Core.Campaign.SliceScenario(LoadAuthored());
            foreach (var beat in slice.Data.Beats)
                Assert.True(slice.EvaluateDayBeat(beat.Day, beat.ActionKey, beat.ExpectedOutcome, out _));

            Assert.True(slice.CompleteSlice(4).Passed);
            Assert.False(slice.CompleteSlice(0).Passed, "no retained survivors is not a pass");
        }

        [Fact]
        public void HostSessionAndStore_RoundTripTheEvidence()
        {
            string host = ReadRepoFile("src", "Host", "SliceScenarioHostSession.cs");
            Assert.Contains("SliceScenarioCatalogLoader.Load(", host);
            Assert.Contains("public SliceScorecard Complete(int retainedSurvivors)", host);
            Assert.Contains("public bool IsFrozen(string expectedHash)", host);

            string probe = ReadRepoFile("src", "Host", "HostCli.SliceScenario.cs");
            Assert.Contains("public static int RunSliceScenarioSelfTest(", probe);
            // Beats are measured from live owners, not self-reported.
            Assert.Contains("MeasureNeedsBeat", probe);
            Assert.Contains("MeasureRadiationBeat", probe);
            Assert.Contains("MeasureMemorialBeat", probe);
            Assert.Contains("MeasureCommitmentBeat", probe);
            // Unmeasured beats stay honest.
            Assert.Contains("NotMeasuredInProbe", probe);
        }
    }
}
