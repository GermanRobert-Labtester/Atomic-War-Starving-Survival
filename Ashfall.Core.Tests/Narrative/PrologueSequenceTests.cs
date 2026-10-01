// SPDX-License-Identifier: MIT
// ============================================================================
// Prologue sequence tests — authored opening beats for a new campaign.
// Verifies the catalog loads ordered, malformed catalogs are rejected rather
// than inventing copy, day lookup is bounded, and the host surfaces the beat
// through the existing Opening Protocol day-goal seam.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class PrologueSequenceTests
    {
        private static string RepoRoot()
        {
            string[] candidates = { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    if (File.Exists(Path.Combine(dir.FullName, "src", "Main.Prologue.cs")))
                        return dir.FullName;
                    dir = dir.Parent!;
                }
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string DataDir() => Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        [Fact]
        public void AuthoredPrologue_LoadsOrderedBeats()
        {
            string json = File.ReadAllText(Path.Combine(DataDir(), "prologue_sequence.json"));
            var prologue = PrologueSequence.LoadFromJson(json);
            Assert.NotNull(prologue);
            Assert.Equal("prologue_first_light", prologue!.PrologueId);
            Assert.True(prologue.BeatCount >= 3, "the prologue must open at least the first three days");

            int previous = 0;
            foreach (var beat in prologue.Beats)
            {
                Assert.True(beat.day_index > previous, "beats must be strictly ordered by day");
                previous = beat.day_index;
                Assert.False(string.IsNullOrWhiteSpace(beat.title));
                Assert.False(string.IsNullOrWhiteSpace(beat.body));
            }
        }

        [Fact]
        public void MalformedPrologues_AreRejected()
        {
            Assert.Null(PrologueSequence.LoadFromJson(null));
            Assert.Null(PrologueSequence.LoadFromJson(""));
            Assert.Null(PrologueSequence.LoadFromJson("{ not json"));
            Assert.Null(PrologueSequence.LoadFromJson("{\"schema_version\":1,\"beats\":[]}"));
            Assert.Null(PrologueSequence.LoadFromJson(
                "{\"beats\":[{\"day_index\":0,\"title\":\"x\",\"body\":\"y\"}]}"));
            Assert.Null(PrologueSequence.LoadFromJson(
                "{\"beats\":[{\"day_index\":1,\"title\":\"\",\"body\":\"y\"}]}"));
            Assert.Null(PrologueSequence.LoadFromJson(
                "{\"beats\":[{\"day_index\":1,\"title\":\"a\",\"body\":\"y\"},{\"day_index\":1,\"title\":\"b\",\"body\":\"z\"}]}"));
        }

        [Fact]
        public void BeatLookup_IsDayIndexedAndBounded()
        {
            var prologue = PrologueSequence.LoadFromJson(
                "{\"beats\":[" +
                "{\"day_index\":3,\"title\":\"c\",\"body\":\"c\"}," +
                "{\"day_index\":1,\"title\":\"a\",\"body\":\"a\"}," +
                "{\"day_index\":2,\"title\":\"b\",\"body\":\"b\"}]}")!;

            Assert.True(prologue.TryGetBeatForDay(1, out var day1));
            Assert.Equal("a", day1!.title);
            Assert.True(prologue.TryGetBeatForDay(3, out var day3));
            Assert.Equal("c", day3!.title);
            Assert.False(prologue.TryGetBeatForDay(0, out _));
            Assert.False(prologue.TryGetBeatForDay(4, out _));
        }

        [Fact]
        public void HostSource_SurfacesPrologueThroughTheDayGoalSeam()
        {
            string main = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.Prologue.cs"));
            Assert.Contains("TryGetPrologueGoal", main);
            Assert.Contains("prologue_sequence.json", main);

            string slice = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.SliceScenario.cs"));
            Assert.Contains("TryGetPrologueGoal(day", slice);
        }
    }
}
