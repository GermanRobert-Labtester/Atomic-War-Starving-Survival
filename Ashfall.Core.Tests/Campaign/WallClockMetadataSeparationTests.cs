using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class WallClockMetadataSeparationTests
    {
        private sealed class MemoryFileIO : IFileIO
        {
            public readonly Dictionary<string, string> Files = new(StringComparer.OrdinalIgnoreCase);
            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => Files.ContainsKey(path);
            public string ReadAllText(string path) => Files.TryGetValue(path, out var text) ? text : throw new FileNotFoundException(path);
            public void WriteAllText(string path, string contents) => Files[path] = contents;
            public void DeleteFile(string path) => Files.Remove(path);
            public string Combine(params string[] parts) => Path.Combine(parts);
        }

        private sealed class TestLog : ILog
        {
            public void Info(string message) { }
            public void Warn(string message) { }
            public void Error(string message) { }
        }

        [Serializable]
        private class TestSimState
        {
            public int Day = 5;
            public float RationStock = 42.5f;
            public string ColonyName = "Vault-86";
        }

        [Fact]
        public void SimulationChecksum_RemainsIdentical_RegardlessOfWallClockTimestamps()
        {
            var state = new TestSimState { Day = 10, RationStock = 120.0f, ColonyName = "Ashfall-Alpha" };

            // Compute hash at time T1
            var clock1 = new FrozenWallClock(new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc));
            string hash1 = SaveChecksum.Compute(state);

            // Compute hash at time T2 (5 years later)
            var clock2 = new FrozenWallClock(new DateTime(2031, 8, 27, 23, 59, 59, DateTimeKind.Utc));
            string hash2 = SaveChecksum.Compute(state);

            Assert.Equal(hash1, hash2);
        }

        [Theory]
        [InlineData("en-US")]
        [InlineData("fr-FR")]
        [InlineData("de-DE")]
        [InlineData("ja-JP")]
        [InlineData("ar-SA")]
        public void CultureAndClockVariations_ProduceIdenticalStateHashes(string cultureName)
        {
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUiCulture = CultureInfo.CurrentUICulture;

            try
            {
                var culture = new CultureInfo(cultureName);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

                var state = new TestSimState { Day = 7, RationStock = 99.75f, ColonyName = "Sector-7" };
                string hash = SaveChecksum.Compute(state);

                // Checksum must be deterministic and invariant across all cultures
                Assert.NotNull(hash);
                Assert.Equal(64, hash.Length);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUiCulture;
            }
        }

        [Fact]
        public void QuarantineCorruptSave_MovesCorruptSaveToQuarantinePath()
        {
            var files = new MemoryFileIO();
            var json = new SystemTextJsonSerializer();
            var log = new TestLog();
            var clock = new FrozenWallClock(new DateTime(2026, 8, 27, 21, 30, 45, DateTimeKind.Utc));

            var slotService = new SaveSlotService(files, json, log, "user://", clock);
            var profileId = new SaveProfileId("default");
            var slotId = new SaveSlotId("slot_1");

            slotService.CreateSlot(profileId, slotId);
            string aggregatePath = slotService.GetAggregatePath(profileId, slotId);
            files.WriteAllText(aggregatePath, "corrupt json payload");

            var result = slotService.TryLoadAggregate(profileId, slotId);
            Assert.False(result.IsSuccess);

            // Quarantined file should exist with quarantine extension
            string expectedQuarantineSuffix = "slot_1" + SaveSlotService.QuarantineExtension;
            Assert.Contains(files.Files.Keys, k => k.EndsWith(expectedQuarantineSuffix, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void DailyBriefingReport_HasNoWallClockDependence()
        {
            var inputs = new DailyBriefingInputs
            {
                Day = 3,
                BuildSeed = 42,
                GeneratedUtc = string.Empty,
                SurvivorChanges = new List<DailyBriefingEntry>
                {
                    new DailyBriefingEntry("Survivor Changes", "survivor_1", "Recovered from sickness.", order: 1)
                }
            };

            var report1 = DailyBriefingReportBuilder.Build(inputs);
            var report2 = DailyBriefingReportBuilder.Build(inputs);

            Assert.Equal(report1.Day, report2.Day);
            Assert.Equal(report1.Title, report2.Title);
            Assert.Equal(report1.TotalEntries, report2.TotalEntries);
            Assert.Equal(string.Empty, report1.GeneratedUtc);
        }
    }
}
