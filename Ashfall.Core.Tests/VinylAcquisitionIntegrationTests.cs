// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class VinylAcquisitionIntegrationTests
    {
        private static readonly string DataDir = FindDataDir();

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "narrative", "vinyl_record_archive.json");
                if (File.Exists(probe)) return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        [Fact]
        public void All30AuthoredRecords_HaveAcquisitionRoute()
        {
            string archivePath = Path.Combine(DataDir, "narrative", "vinyl_record_archive.json");
            Assert.True(File.Exists(archivePath), $"Vinyl archive not found at: {archivePath}");

            string json = File.ReadAllText(archivePath);
            var catalog = new VinylRecordCatalog();
            catalog.Load(json, new SystemTextJsonSerializer());

            Assert.Equal(30, catalog.AllRecords.Count);

            var routes = VinylRecordAcquisitionMap.GetAllRoutes();
            Assert.Equal(30, routes.Count);

            foreach (var entry in catalog.AllRecords)
            {
                var route = VinylRecordAcquisitionMap.GetRoute(entry.record_id);
                Assert.NotNull(route);
                Assert.Equal(entry.record_id, route.RecordId);
                Assert.False(string.IsNullOrEmpty(route.AssociatedItemId), $"Record {entry.record_id} has empty item ID");
                Assert.False(string.IsNullOrEmpty(route.AcquisitionChannel), $"Record {entry.record_id} has empty channel");
                Assert.False(string.IsNullOrEmpty(route.LocationType), $"Record {entry.record_id} has empty location type");
                Assert.False(string.IsNullOrEmpty(route.RarityTier), $"Record {entry.record_id} has empty rarity tier");
                Assert.True(route.DiscoveryWeight > 0, $"Record {entry.record_id} has non-positive weight");
            }
        }

        [Fact]
        public void IsVinylAcquisitionItem_IdentifiesVinylItemsCorrectly()
        {
            Assert.True(VinylRecordAcquisitionMap.IsVinylAcquisitionItem("item_vinyl_collection"));
            Assert.True(VinylRecordAcquisitionMap.IsVinylAcquisitionItem("item_collectible_vinyl_chamber_record"));
            Assert.True(VinylRecordAcquisitionMap.IsVinylAcquisitionItem("item_collectible_vinyl_civil_broadcast"));
            Assert.True(VinylRecordAcquisitionMap.IsVinylAcquisitionItem("item_collectible_vinyl_folk_compilation"));

            Assert.False(VinylRecordAcquisitionMap.IsVinylAcquisitionItem("item_canned_food"));
            Assert.False(VinylRecordAcquisitionMap.IsVinylAcquisitionItem("scrap_metal"));
            Assert.False(VinylRecordAcquisitionMap.IsVinylAcquisitionItem(""));
        }

        [Fact]
        public void TryAcquireFromItem_AddsUnownedRecordToVinylSystem()
        {
            var system = new VinylMoraleSystem();
            var rng = new SeededRng(12345);

            string? acquired = VinylRecordAcquisitionMap.TryAcquireFromItem("item_vinyl_collection", system, rng);
            Assert.NotNull(acquired);
            Assert.Contains(acquired, system.State.ownedRecordIds);

            // Acquiring again yields a different unowned record
            string? acquired2 = VinylRecordAcquisitionMap.TryAcquireFromItem("item_vinyl_collection", system, rng);
            Assert.NotNull(acquired2);
            Assert.NotEqual(acquired, acquired2);
            Assert.Contains(acquired2, system.State.ownedRecordIds);
            Assert.Equal(2, system.State.ownedRecordIds.Count);
        }

        [Fact]
        public void ResolveRecordForDiscovery_ResolvesByLocation()
        {
            var rng = new SeededRng(9999);
            var owned = new HashSet<string>();

            string? recordConcert = VinylRecordAcquisitionMap.ResolveRecordForDiscovery("concert_hall", owned, rng);
            Assert.NotNull(recordConcert);
            var route = VinylRecordAcquisitionMap.GetRoute(recordConcert);
            Assert.NotNull(route);
            Assert.Equal("concert_hall", route.LocationType);
        }

        [Fact]
        public void PlaybackAndDailyMorale_WorksWithAcquiredRecord()
        {
            var system = new VinylMoraleSystem();
            system.LoadCatalog(new List<VinylRecordDefinition>
            {
                new VinylRecordDefinition
                {
                    record_id = "record_01_valse_triste_sibelius_78rpm",
                    display_name = "Valse Triste",
                    genre = "classical",
                    morale_daily_bonus = 6f
                }
            });

            system.AcquireRecord("record_01_valse_triste_sibelius_78rpm");

            var playResult = system.Play("record_01_valse_triste_sibelius_78rpm", day: 10);
            Assert.True(playResult.IsSuccess);
            Assert.True(system.IsPlaying);

            float appliedMorale = 0;
            system.OnMoraleApplied += m => appliedMorale = m;

            system.ApplyDailyEffect(10);
            Assert.Equal(6f, appliedMorale);
            Assert.Equal(6f, system.State.totalMoraleApplied);
            Assert.Equal(1, system.State.totalPlays);
        }

        [Fact]
        public void RareCulturalRecord_TriggersBroadcastEvent()
        {
            var system = new VinylMoraleSystem();
            system.LoadCatalog(new List<VinylRecordDefinition>
            {
                new VinylRecordDefinition
                {
                    record_id = "record_03_rachmaninoff_piano_concerto_2_adagio",
                    display_name = "Piano Concerto No. 2",
                    genre = "classical",
                    morale_daily_bonus = 8f
                }
            });

            system.AcquireRecord("record_03_rachmaninoff_piano_concerto_2_adagio");

            bool broadcastFired = false;
            VinylRecordDefinition? broadcastDef = null;
            int broadcastDay = -1;

            system.OnCulturalBroadcast += (rec, d) =>
            {
                broadcastFired = true;
                broadcastDef = rec;
                broadcastDay = d;
            };

            system.Play("record_03_rachmaninoff_piano_concerto_2_adagio", day: 42);

            Assert.True(broadcastFired);
            Assert.NotNull(broadcastDef);
            Assert.Equal("record_03_rachmaninoff_piano_concerto_2_adagio", broadcastDef.record_id);
            Assert.Equal(42, broadcastDay);
            Assert.Equal(0.85f, system.State.lastBroadcastSignalStrength);
        }

        [Fact]
        public void State_RoundTripsThroughCaptureAndRestore()
        {
            var system1 = new VinylMoraleSystem();
            system1.AcquireRecord("record_01_valse_triste_sibelius_78rpm");
            system1.AcquireRecord("record_02_autumn_leaves_dark_eyes_gypsy_trio");
            system1.State.totalPlays = 5;
            system1.State.totalMoraleApplied = 30f;

            var captured = system1.CaptureState();

            var system2 = new VinylMoraleSystem();
            system2.RestoreState(captured);

            Assert.Equal(2, system2.State.ownedRecordIds.Count);
            Assert.Contains("record_01_valse_triste_sibelius_78rpm", system2.State.ownedRecordIds);
            Assert.Contains("record_02_autumn_leaves_dark_eyes_gypsy_trio", system2.State.ownedRecordIds);
            Assert.Equal(5, system2.State.totalPlays);
            Assert.Equal(30f, system2.State.totalMoraleApplied);
        }
    }
}
