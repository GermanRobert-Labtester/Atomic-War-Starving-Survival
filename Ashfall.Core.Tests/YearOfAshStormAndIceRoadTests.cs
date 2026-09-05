using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class YearOfAshStormAndIceRoadTests
    {
        // ── Storm catalog helpers ────────────────────────────────────────────

        private static List<StormWindowEntry> BuildTestCatalog()
        {
            return new List<StormWindowEntry>
            {
                new StormWindowEntry { id = "s1", phase = "deep_freeze",    day_start = 185, day_end = 188, type = "black_blizzard",     intensity = 0.65f, caloric_penalty = 0.12f, radon_spike = 0.00f, faction_morale_penalty = 0.05f, description = "Test blizzard A." },
                new StormWindowEntry { id = "s2", phase = "deep_freeze",    day_start = 200, day_end = 204, type = "thermal_inversion",  intensity = 0.70f, caloric_penalty = 0.15f, radon_spike = 0.06f, faction_morale_penalty = 0.04f, description = "Test inversion B." },
                new StormWindowEntry { id = "s3", phase = "deep_freeze",    day_start = 200, day_end = 202, type = "ash_fallout",        intensity = 0.40f, caloric_penalty = 0.08f, radon_spike = 0.02f, faction_morale_penalty = 0.02f, description = "Test ash fallout C." },
                new StormWindowEntry { id = "s4", phase = "faction_siege",  day_start = 248, day_end = 253, type = "artillery_dust",     intensity = 0.60f, caloric_penalty = 0.06f, radon_spike = 0.05f, faction_morale_penalty = 0.10f, description = "Test artillery D." },
                new StormWindowEntry { id = "s5", phase = "great_thaw",     day_start = 308, day_end = 313, type = "thaw_flood",         intensity = 0.70f, caloric_penalty = 0.05f, radon_spike = 0.18f, faction_morale_penalty = 0.06f, description = "Test thaw flood E." },
            };
        }

        // ── StormCatalog_Load tests ──────────────────────────────────────────

        [Fact]
        public void StormCatalog_Load_ReturnsEntries()
        {
            // Load from the real data file via FileSystemIO
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string dataDir = fileIO.Combine(
                System.IO.Directory.GetCurrentDirectory(),
                "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");

            // Resolve to absolute if possible
            if (!fileIO.DirectoryExists(dataDir))
                dataDir = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data";

            var entries = YearOfAshStormCatalogLoader.Load(dataDir, fileIO, json);
            Assert.True(entries.Count >= 12, $"Expected at least 12 storm entries, got {entries.Count}");
        }

        [Fact]
        public void StormCatalog_GetActiveWindows_Day200_ReturnsDeepFreezeStorms()
        {
            var catalog = BuildTestCatalog();
            var active = StormWindowQuery.GetActiveWindowsForDay(catalog, 200);

            // Day 200 is within s2 (200-204, thermal_inversion) AND s3 (200-202, ash_fallout)
            Assert.Equal(2, active.Count);
            Assert.Contains(active, e => e.id == "s2");
            Assert.Contains(active, e => e.id == "s3");
        }

        [Fact]
        public void StormCatalog_GetActiveWindows_NonOverlapDay_ReturnsSingleEntry()
        {
            var catalog = BuildTestCatalog();
            var active = StormWindowQuery.GetActiveWindowsForDay(catalog, 186);
            Assert.Single(active);
            Assert.Equal("s1", active[0].id);
        }

        [Fact]
        public void StormCatalog_GetCaloricPenalty_AddsCorrectly()
        {
            var catalog = BuildTestCatalog();
            // Day 200: s2 (0.15) + s3 (0.08) = 0.23
            float penalty = StormWindowQuery.GetCaloricPenaltyForDay(catalog, 200);
            Assert.Equal(0.23f, penalty, 3);
        }

        [Fact]
        public void StormCatalog_GetRadonSpike_AddsCorrectly()
        {
            var catalog = BuildTestCatalog();
            // Day 200: s2 (0.06) + s3 (0.02) = 0.08
            float radon = StormWindowQuery.GetRadonSpikeForDay(catalog, 200);
            Assert.Equal(0.08f, radon, 3);
        }

        [Fact]
        public void StormCatalog_NoActiveWindows_ReturnsZeroPenalty()
        {
            var catalog = BuildTestCatalog();
            float penalty = StormWindowQuery.GetCaloricPenaltyForDay(catalog, 400); // past all storms
            Assert.Equal(0f, penalty);
        }

        [Fact]
        public void StormCatalog_HasIceRoadBlockingStorm_ThermalInversion_ReturnsTrue()
        {
            var catalog = BuildTestCatalog();
            Assert.True(StormWindowQuery.HasIceRoadBlockingStorm(catalog, 200)); // s2 is thermal_inversion
        }

        [Fact]
        public void StormCatalog_HasIceRoadBlockingStorm_BlizzardOnly_ReturnsFalse()
        {
            var catalog = BuildTestCatalog();
            Assert.False(StormWindowQuery.HasIceRoadBlockingStorm(catalog, 186)); // s1 is black_blizzard — not blocking
        }

        // ── IceRoad system tests ─────────────────────────────────────────────

        [Fact]
        public void IceRoad_OpenWhenCold_NoThawStorms()
        {
            var sys = new YearOfAshIceRoadSystem();
            sys.TickDay(190, -25f, null); // temp <= -20, no blocking storms
            Assert.True(sys.IsIceRoadOpen);
        }

        [Fact]
        public void IceRoad_ClosedWhenWarm()
        {
            var sys = new YearOfAshIceRoadSystem();
            sys.TickDay(310, -5f, null); // temp > -20 (great thaw)
            Assert.False(sys.IsIceRoadOpen);
        }

        [Fact]
        public void IceRoad_ClosedDuringThawFlood()
        {
            var sys = new YearOfAshIceRoadSystem();
            var activeStorms = new List<StormWindowEntry>
            {
                new StormWindowEntry { id = "flood", type = "thaw_flood", day_start = 308, day_end = 313, caloric_penalty = 0.05f }
            };
            sys.TickDay(310, -22f, activeStorms); // cold enough but thaw_flood blocks
            Assert.False(sys.IsIceRoadOpen);
        }

        [Fact]
        public void IceRoad_TradeMultiplier_HighWhenOpen()
        {
            var sys = new YearOfAshIceRoadSystem();
            sys.TickDay(192, -28f, null);
            Assert.True(sys.IsIceRoadOpen);
            Assert.Equal(1.4f, sys.GetTradeMultiplier(), 3);
        }

        [Fact]
        public void IceRoad_TradeMultiplier_LowWhenClosed()
        {
            var sys = new YearOfAshIceRoadSystem();
            sys.TickDay(320, -5f, null); // warm
            Assert.False(sys.IsIceRoadOpen);
            Assert.Equal(0.6f, sys.GetTradeMultiplier(), 3);
        }

        [Fact]
        public void IceRoad_CaptureRestore_RoundTrip()
        {
            var sys = new YearOfAshIceRoadSystem();
            sys.TickDay(190, -25f, null);
            sys.TickDay(191, -26f, null);

            var captured = sys.CaptureState();
            var sys2 = new YearOfAshIceRoadSystem();
            sys2.RestoreState(captured);

            Assert.Equal(sys.IsIceRoadOpen, sys2.IsIceRoadOpen);
            Assert.Equal(sys.State.totalTradeWindowDays, sys2.State.totalTradeWindowDays);
            Assert.Equal(sys.State.cumulativeExposureScore, sys2.State.cumulativeExposureScore, 4);
        }

        // ── Save migration tests ─────────────────────────────────────────────

        [Fact]
        public void YearOfAshSave_V4ToV5Migration_IceRoadDefaultsPresent()
        {
            // Build a synthetic v4 save (no iceRoad field)
            var v4 = new YearOfAshSaveV4
            {
                saveVersion = 4,
                simDay = 200
            };
            var json = new SystemTextJsonSerializer();
            v4.Checksum = SaveChecksum.Compute(v4);
            string encoded = json.Serialize(v4);

            var migrated = YearOfAshSaveCodec.Decode(encoded, json);

            Assert.Equal(5, migrated.saveVersion);
            Assert.NotNull(migrated.iceRoad);
            Assert.False(migrated.iceRoad.iceRoadOpen);
            Assert.Equal(0, migrated.iceRoad.totalTradeWindowDays);
        }

        [Fact]
        public void YearOfAshSave_V5_RoundTrip_PreservesIceRoad()
        {
            var save = new YearOfAshSave
            {
                saveVersion = 5,
                simDay = 210,
                iceRoad = new IceRoadState
                {
                    iceRoadOpen = true,
                    lastOpenDay = 210,
                    totalTradeWindowDays = 5,
                    cumulativeExposureScore = 1.5f
                }
            };
            save.Checksum = SaveChecksum.Compute(save);

            var json = new SystemTextJsonSerializer();
            string encoded = YearOfAshSaveCodec.Encode(save, json);
            var decoded = YearOfAshSaveCodec.Decode(encoded, json);

            Assert.Equal(5, decoded.saveVersion);
            Assert.True(decoded.iceRoad.iceRoadOpen);
            Assert.Equal(210, decoded.iceRoad.lastOpenDay);
            Assert.Equal(5, decoded.iceRoad.totalTradeWindowDays);
            Assert.Equal(1.5f, decoded.iceRoad.cumulativeExposureScore, 4);
        }
    }
}
