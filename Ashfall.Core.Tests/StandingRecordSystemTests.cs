using System;
using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class LocationMemorySystemTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static LocationMemorySystem Memory()
        {
            var sys = new LocationMemorySystem(new FileSystemIO(), new SystemTextJsonSerializer());
            sys.Load(DataDir());
            sys.Unlock();
            return sys;
        }

        [Fact]
        public void StrataLoadFromJson()
        {
            var mem = Memory();
            Assert.True(mem.StratumCount >= 30);
            Assert.NotNull(mem.GetStratumText("loc_cut_kilometre_19", "pre"));
            Assert.NotNull(mem.GetStratumText("loc_cut_kilometre_19", "now"));
        }

        [Fact]
        public void NowStrataSelectedByMutation()
        {
            var mem = Memory();
            Assert.Null(mem.GetActiveRecast("loc_cut_kilometre_19"));
            mem.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
            string active = mem.GetActiveRecast("loc_cut_kilometre_19");
            Assert.NotNull(active);
            Assert.Contains("CUT-19", active);
            Assert.Contains("Ivy", active);
        }

        [Fact]
        public void ScrapedOverridesPlated()
        {
            var mem = Memory();
            mem.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
            mem.ApplyMutation(LocationMemorySystem.MutationKm19Scraped);
            string active = mem.GetActiveRecast("loc_cut_kilometre_19");
            Assert.Contains("short one post", active);
        }

        [Fact]
        public void PalimpsestSelectedLast()
        {
            var mem = Memory();
            mem.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
            mem.ApplyMutation(LocationMemorySystem.MutationKm19Palimpsest);
            string active = mem.GetActiveRecast("loc_cut_kilometre_19");
            Assert.Contains("two things", active);
        }

        [Fact]
        public void SaveRoundtrip()
        {
            var json = new SystemTextJsonSerializer();
            var mem = Memory();
            mem.ApplyMutation(LocationMemorySystem.MutationLockGaugesFiled);
            var restored = new LocationMemorySystem(new FileSystemIO(), new SystemTextJsonSerializer());
            restored.Load(DataDir());
            restored.Unlock();
            restored.RestoreState(json.Deserialize<LocationMemoryState>(json.Serialize(mem.CaptureState())));
            Assert.True(restored.HasMutation(LocationMemorySystem.MutationLockGaugesFiled));
        }

        [Fact]
        public void RecastEventFiresOncePerSite()
        {
            var mem = Memory();
            int fired = 0;
            mem.OnLocationRecast += (site, text) => fired++;
            mem.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
            Assert.Equal(1, fired);
            mem.ApplyMutation(LocationMemorySystem.MutationTransitMaps);
            Assert.Equal(2, fired);
            mem.ApplyMutation(LocationMemorySystem.MutationKm19Plated);
            Assert.Equal(2, fired); // same-site recast does not re-fire
        }
    }

    public class SiteEncounterSystemTests
    {
        [Fact]
        public void LockedUntilUnlock()
        {
            var sys = new SiteEncounterSystem();
            Assert.False(sys.StartEncounter("enc_site_plate_screwer", "room_km19_post",
                SiteEncounterSystem.KindPlateScrewer, 75));
        }

        [Fact]
        public void StartAndResolve()
        {
            var sys = new SiteEncounterSystem(1808);
            sys.Unlock();
            Assert.True(sys.StartEncounter("enc_site_plate_screwer", "room_km19_post",
                SiteEncounterSystem.KindPlateScrewer, 75, "mutation_km19_plated"));
            Assert.True(sys.ResolveEncounter("enc_site_plate_screwer", 75));
            Assert.False(sys.ResolveEncounter("enc_site_plate_screwer", 76));
            Assert.True(sys.IsResolved("enc_site_plate_screwer"));
        }

        [Fact]
        public void ThreeScrapesWithdrawOverlay()
        {
            var sys = new SiteEncounterSystem();
            sys.Unlock();
            Assert.True(sys.OverlayAccess);
            sys.ScrapePlate(75);
            sys.ScrapePlate(76);
            sys.ScrapePlate(77);
            Assert.False(sys.OverlayAccess);
            Assert.Equal(3, sys.PlatesScraped);
        }

        [Fact]
        public void RestoreOverlayAccessReopens()
        {
            var sys = new SiteEncounterSystem();
            sys.Unlock();
            for (int i = 0; i < 3; i++) sys.ScrapePlate(80 + i);
            Assert.False(sys.OverlayAccess);
            sys.RestoreOverlayAccess();
            Assert.True(sys.OverlayAccess);
        }

        [Fact]
        public void SaveRoundtrip()
        {
            var json = new SystemTextJsonSerializer();
            var sys = new SiteEncounterSystem(1808);
            sys.Unlock();
            sys.StartEncounter("enc_site_gauge_read", "room_lock_gauges",
                SiteEncounterSystem.KindGaugeRead, 90, "mutation_lock_gauges_filed");
            sys.ResolveEncounter("enc_site_gauge_read", 90);
            sys.ScrapePlate(91);
            string blob = json.Serialize(sys.CaptureState());
            var restored = new SiteEncounterSystem();
            restored.RestoreState(json.Deserialize<SiteEncounterState>(blob));
            Assert.True(restored.IsResolved("enc_site_gauge_read"));
            Assert.Equal(1, restored.PlatesScraped);
            Assert.True(restored.OverlayAccess);
        }
    }

    public class StandingRecordCatalogTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void TenMainQuestsRegistered()
        {
            var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            Assert.True(catalog.Quests.Count >= 10);
            // All ten mains (plan §4.1)
            Assert.NotNull(catalog.GetQuest("quest_record_the_plate"));
            Assert.NotNull(catalog.GetQuest("quest_record_grease_pencil"));
            Assert.NotNull(catalog.GetQuest("quest_record_wrong_stacks"));
            Assert.NotNull(catalog.GetQuest("quest_record_the_book"));
            Assert.NotNull(catalog.GetQuest("quest_record_mass_or_lot"));
            Assert.NotNull(catalog.GetQuest("quest_record_hands"));
            Assert.NotNull(catalog.GetQuest("quest_record_friendly_obstacle"));
            Assert.NotNull(catalog.GetQuest("quest_record_the_failure"));
            Assert.NotNull(catalog.GetQuest("quest_record_fallback"));
            Assert.NotNull(catalog.GetQuest("quest_record_which_gazetteer"));
        }

        [Fact]
        public void MainsChainInOrder()
        {
            var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            Assert.Equal("quest_record_the_plate",
                catalog.GetQuest("quest_record_grease_pencil").prereq_quest_id);
            Assert.Equal("quest_record_grease_pencil",
                catalog.GetQuest("quest_record_wrong_stacks").prereq_quest_id);
            Assert.Equal("quest_record_wrong_stacks",
                catalog.GetQuest("quest_record_the_book").prereq_quest_id);
            Assert.Equal("quest_record_the_book",
                catalog.GetQuest("quest_record_mass_or_lot").prereq_quest_id);
            Assert.Equal("quest_record_mass_or_lot",
                catalog.GetQuest("quest_record_hands").prereq_quest_id);
            Assert.Equal("quest_record_the_failure",
                catalog.GetQuest("quest_record_fallback").prereq_quest_id);
            Assert.Equal("quest_record_fallback",
                catalog.GetQuest("quest_record_which_gazetteer").prereq_quest_id);
        }

        [Fact]
        public void MainsTargetSpineLocations()
        {
            var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            Assert.Equal("location_ministry_of_truth_bunker",
                catalog.GetQuest("quest_record_the_book").target_location_id);
            Assert.Equal("loc_lock_gate_four",
                catalog.GetQuest("quest_record_the_failure").target_location_id);
            Assert.Equal("location_the_memory_vault",
                catalog.GetQuest("quest_record_which_gazetteer").target_location_id);
        }

        [Fact]
        public void EveryMainHasWorldChangeMutation()
        {
            var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());
            for (int i = 0; i < catalog.Quests.Count; i++)
            {
                var q = catalog.Quests[i];
                Assert.False(string.IsNullOrEmpty(q.id));
                Assert.False(string.IsNullOrEmpty(q.complete_mutation),
                    q.id + " must name a world-change mutation (plan world-change bar)");
                Assert.True(q.StageCount >= 3, q.id + " should have 3+ objectives (spatial bar)");
            }
        }

        [Fact]
        public void RosterNpcsPresentInCharacters()
        {
            string path = Path.Combine(DataDir(), "characters.json");
            var json = new SystemTextJsonSerializer();
            var chars = json.Deserialize<System.Collections.Generic.List<StandingCharProbe>>(
                File.ReadAllText(path));
            var ids = new System.Collections.Generic.HashSet<string>();
            for (int i = 0; i < chars.Count; i++) ids.Add(chars[i].id);
            Assert.Contains("npc_maren_holt", ids);
            Assert.Contains("npc_ira_vell", ids);
            Assert.Contains("npc_benno_kade", ids);
            Assert.Contains("npc_quil_esser", ids);
            Assert.Contains("npc_osric_tann", ids);
            Assert.Contains("npc_dara_mewn", ids);
        }

        private sealed class StandingCharProbe
        {
            public string id;
        }
    }
}