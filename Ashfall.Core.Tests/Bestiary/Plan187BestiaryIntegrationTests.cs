// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 187: Bestiary & Creature Encounter Tracking UI — Integration Tests
// Verifies integration with WastelandBestiaryCatalog, creature discovery,
// sighting records, tiered unlock progression, kill/butcher tracking,
// completion percentage, and save/restore roundtrips.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Bestiary;

namespace Ashfall.Core.Tests.Bestiary
{
    public sealed class Plan187BestiaryIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsWastelandBestiary()
        {
            var system = new BestiarySystem();
            string path = Path.Combine(DataDirectory, "narrative", "wasteland_wildlife_bestiary.json");
            Assert.True(File.Exists(path), $"wasteland_wildlife_bestiary.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            Assert.Equal(24, system.Catalog.AllCreatures.Count);
            var wolf = system.Catalog.GetById("creature_01_two_headed_steppe_wolf");
            Assert.NotNull(wolf);
            Assert.Equal("The Bicephalous Stalker", wolf.colloquial_name);
            Assert.Equal(3, wolf.threat_level);
        }

        [Fact]
        public void RecordEncounter_DiscoversCreatureAndLogsSighting()
        {
            var system = new BestiarySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "narrative", "wasteland_wildlife_bestiary.json")));

            string? discoveredCreature = null;
            system.OnCreatureDiscovered += id => discoveredCreature = id;

            var record = system.RecordEncounter(
                "creature_01_two_headed_steppe_wolf",
                day: 2,
                locationId: "loc_pine_barrens",
                witnessId: "surv_01");

            Assert.Equal(1, system.DiscoveredCount);
            Assert.Equal("creature_01_two_headed_steppe_wolf", discoveredCreature);
            Assert.Equal(1, record.EncounterCount);
            Assert.Equal(2, record.DiscoveredDay);
            Assert.Equal("loc_pine_barrens", record.FirstLocationId);
            Assert.Contains("discovery", record.UnlockedNoteKeys);

            var sightings = system.GetRecentSightings();
            Assert.Single(sightings);
            Assert.Equal("creature_01_two_headed_steppe_wolf", sightings[0].CreatureId);
            Assert.Equal(2, sightings[0].Day);
            Assert.Equal("loc_pine_barrens", sightings[0].LocationId);
            Assert.Equal("surv_01", sightings[0].WitnessSurvivorId);
        }

        [Fact]
        public void TieredUnlocks_ProgressWithEncounters()
        {
            var system = new BestiarySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "narrative", "wasteland_wildlife_bestiary.json")));

            const string cid = "creature_03_armored_slag_beetle";

            // 1 encounter: only discovery
            system.RecordEncounter(cid, 1);
            Assert.False(system.IsBasicStatsUnlocked(cid));
            Assert.False(system.IsBehaviorUnlocked(cid));
            Assert.False(system.IsCombatTacticsUnlocked(cid));

            // 2 more encounters (total 3): unlocks basic_stats
            system.RecordEncounter(cid, 2);
            system.RecordEncounter(cid, 3);
            Assert.True(system.IsBasicStatsUnlocked(cid));
            Assert.False(system.IsBehaviorUnlocked(cid));

            // 2 more encounters (total 5): unlocks behavior
            system.RecordEncounter(cid, 4);
            system.RecordEncounter(cid, 5);
            Assert.True(system.IsBehaviorUnlocked(cid));
            Assert.False(system.IsCombatTacticsUnlocked(cid));

            // 5 more encounters (total 10): unlocks combat_tactics
            for (int d = 6; d <= 10; d++)
                system.RecordEncounter(cid, d);

            Assert.True(system.IsCombatTacticsUnlocked(cid));
        }

        [Fact]
        public void RecordKillAndButcher_UnlocksCombatAndHarvestNotes()
        {
            var system = new BestiarySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "narrative", "wasteland_wildlife_bestiary.json")));

            const string cid = "creature_02_blind_subterranean_molerat";

            string? unlockedKey = null;
            system.OnNoteUnlocked += (creature, key) =>
            {
                if (creature == cid) unlockedKey = key;
            };

            // Kill immediately unlocks combat tactics
            system.RecordKill(cid, 3, "loc_sewer");
            var record = system.GetDiscovery(cid);
            Assert.NotNull(record);
            Assert.Equal(1, record.KillCount);
            Assert.True(system.IsCombatTacticsUnlocked(cid));

            // Butcher unlocks harvest_yields
            system.RecordButcher(cid, 3);
            Assert.Equal(1, record.ButcherCount);
            Assert.Contains("harvest_yields", record.UnlockedNoteKeys);
            Assert.Equal("harvest_yields", unlockedKey);
        }

        [Fact]
        public void GetCompletionPercentage_TracksCollectionProgress()
        {
            var system = new BestiarySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "narrative", "wasteland_wildlife_bestiary.json")));

            Assert.Equal(0.0f, system.GetCompletionPercentage());

            // Discover 6 distinct creatures out of 24 -> 25.0%
            system.RecordEncounter("creature_01_two_headed_steppe_wolf", 1);
            system.RecordEncounter("creature_02_blind_subterranean_molerat", 1);
            system.RecordEncounter("creature_03_armored_slag_beetle", 1);
            system.RecordEncounter("creature_04_ash_strider_crane", 1);
            system.RecordEncounter("creature_05_cinder_adder", 1);
            system.RecordEncounter("creature_06_quarry_ghoul_badger", 1);

            Assert.Equal(25.0f, system.GetCompletionPercentage());
        }

        [Fact]
        public void SaveRestoreState_PreservesDiscoveriesAndSightings()
        {
            var system = new BestiarySystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "narrative", "wasteland_wildlife_bestiary.json")));

            system.RecordEncounter("creature_01_two_headed_steppe_wolf", 1, "loc_woods", "surv_alex");
            system.RecordKill("creature_01_two_headed_steppe_wolf", 2, "loc_woods");
            system.RecordButcher("creature_01_two_headed_steppe_wolf", 2);

            var state = system.CaptureState();

            var restoredSystem = new BestiarySystem();
            restoredSystem.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "narrative", "wasteland_wildlife_bestiary.json")));
            restoredSystem.RestoreState(state);

            Assert.Equal(1, restoredSystem.DiscoveredCount);
            var d = restoredSystem.GetDiscovery("creature_01_two_headed_steppe_wolf");
            Assert.NotNull(d);
            Assert.Equal(2, d.EncounterCount);
            Assert.Equal(1, d.KillCount);
            Assert.Equal(1, d.ButcherCount);
            Assert.True(restoredSystem.IsCombatTacticsUnlocked("creature_01_two_headed_steppe_wolf"));
            Assert.Contains("harvest_yields", d.UnlockedNoteKeys);

            var sightings = restoredSystem.GetRecentSightings();
            Assert.Equal(2, sightings.Count);
        }
    }
}
