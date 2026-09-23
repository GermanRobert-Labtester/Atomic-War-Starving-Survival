// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Records
{
    /// <summary>
    /// Plan 150 — strict loader contract for the authored courtship table. The
    /// data authority decides which courtship activities exist and what they
    /// are worth, so a malformed row must be rejected rather than silently
    /// defaulted into a live courtship event.
    /// </summary>
    public class RomanceCourtshipCatalogLoaderTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent;
            return dir?.FullName ?? throw new InvalidOperationException("repo root not found");
        }

        private static string DataDir() =>
            Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private const string ValidTable = @"{
          ""schema_version"": 1,
          ""courtship_events"": [
            { ""event_id"": ""courtship_shared_meal"", ""name"": ""A Meal Taken Together"",
              ""min_affinity"": 20, ""base_success_rate"": 0.8, ""score_gain"": 6,
              ""stress_reduction"": 4.0, ""description"": ""Two trays at the same end of the table."" },
            { ""event_id"": ""courtship_evening_walk"", ""name"": ""The Perimeter Walk"",
              ""min_affinity"": 22, ""base_success_rate"": 0.75, ""score_gain"": 8,
              ""stress_reduction"": 6.0, ""description"": ""The last circuit before lights-out."" }
          ]
        }";

        [Fact]
        public void ValidTable_LoadsEveryRow()
        {
            var result = RomanceCourtshipCatalogLoader.LoadFromJson(ValidTable);

            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Events.Count);
            Assert.Equal("courtship_shared_meal", result.Events[0].EventId);
            Assert.Equal(20, result.Events[0].MinAffinity);
            Assert.Equal(6, result.Events[0].ScoreGain);
            Assert.Equal(0.8f, result.Events[0].BaseSuccessRate, 3);
        }

        [Fact]
        public void EmptyTable_IsAHardError()
        {
            var result = RomanceCourtshipCatalogLoader.LoadFromJson(
                @"{ ""schema_version"": 1, ""courtship_events"": [] }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("must not be empty"));
        }

        [Fact]
        public void DuplicateEventId_IsRejected()
        {
            var result = RomanceCourtshipCatalogLoader.LoadFromJson(@"{
              ""schema_version"": 1,
              ""courtship_events"": [
                { ""event_id"": ""courtship_same"", ""name"": ""A"", ""score_gain"": 5 },
                { ""event_id"": ""courtship_same"", ""name"": ""B"", ""score_gain"": 5 }
              ]
            }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("duplicate courtship event id"));
        }

        [Theory]
        [InlineData(@"""base_success_rate"": 1.5", "base_success_rate")]
        [InlineData(@"""base_success_rate"": 0.0", "base_success_rate")]
        public void OutOfRangeSuccessRate_IsRejected(string field, string expected)
        {
            string json = $@"{{
              ""schema_version"": 1,
              ""courtship_events"": [
                {{ ""event_id"": ""courtship_x"", ""name"": ""X"", {field}, ""score_gain"": 5 }}
              ]
            }}";

            var result = RomanceCourtshipCatalogLoader.LoadFromJson(json);

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains(expected));
        }

        [Fact]
        public void NegativeScoreGain_IsRejected()
        {
            var result = RomanceCourtshipCatalogLoader.LoadFromJson(@"{
              ""schema_version"": 1,
              ""courtship_events"": [
                { ""event_id"": ""courtship_x"", ""name"": ""X"", ""score_gain"": 0 }
              ]
            }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("score_gain"));
        }

        [Fact]
        public void OutOfRangeMinAffinity_IsRejected()
        {
            var result = RomanceCourtshipCatalogLoader.LoadFromJson(@"{
              ""schema_version"": 1,
              ""courtship_events"": [
                { ""event_id"": ""courtship_x"", ""name"": ""X"", ""min_affinity"": 140, ""score_gain"": 5 }
              ]
            }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("min_affinity"));
        }

        [Fact]
        public void EmptyEventIdAndName_AreRejected()
        {
            var result = RomanceCourtshipCatalogLoader.LoadFromJson(@"{
              ""schema_version"": 1,
              ""courtship_events"": [
                { ""event_id"": """", ""name"": """", ""score_gain"": 5 }
              ]
            }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("empty event_id"));
            Assert.Contains(result.Errors, e => e.Contains("empty name"));
        }

        [Fact]
        public void UnsupportedSchemaVersion_IsRejected()
        {
            var result = RomanceCourtshipCatalogLoader.LoadFromJson(
                @"{ ""schema_version"": 99, ""courtship_events"": [ { ""event_id"": ""a"", ""name"": ""A"", ""score_gain"": 5 } ] }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("schema_version"));
        }

        [Fact]
        public void AuthoredTable_OnDisk_LoadsStrictly()
        {
            // The shipped authority must satisfy the same contract the tests enforce.
            var result = RomanceCourtshipCatalogLoader.Load(DataDir(), new Ashfall.Core.FileSystemIO());

            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.True(result.Events.Count >= 20, $"expected >= 20 authored events, found {result.Events.Count}");
            Assert.Equal(result.Events.Count, result.Events.Select(e => e.EventId).Distinct().Count());
        }
    }
}
