using System;
using System.IO;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Memorial;
using Ashfall.Core.Narrative;
using Ashfall.Core.Spiritual;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Spiritual
{
    public class Plan30SpiritualWorldTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string dataDir = string.Empty;
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) { dataDir = candidate; break; }
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return dataDir;
        }

        [Fact]
        public void SpiritualCatalog_LoadsAndIndexesAllCatalogs()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var catalog = SpiritualCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(catalog);

            // 19 rituals/superstitions/comforts
            Assert.Equal(19, catalog.Rituals.Count);
            Assert.NotNull(catalog.GetRitual("ritual_exterior_door_tap"));
            Assert.NotNull(catalog.GetRitual("ritual_crust_for_the_waste"));
            Assert.NotNull(catalog.GetRitual("ritual_birthday_match_flame"));
            Assert.NotNull(catalog.GetRitual("ritual_participation_in_hot_zones"));
            Assert.NotNull(catalog.GetRitual("superstition_intake_vent_nightmare"));
            Assert.NotNull(catalog.GetRitual("superstition_lucky_lower_bunk"));
            Assert.NotNull(catalog.GetRitual("folklore_comfort_blackout_freeze"));

            // 6 memorial rites
            Assert.Equal(6, catalog.MemorialRites.Count);
            Assert.NotNull(catalog.GetMemorialRite("memorial_rite_roll_call_naming"));
            Assert.NotNull(catalog.GetMemorialRite("memorial_rite_empty_bunk_night"));
            Assert.NotNull(catalog.GetMemorialRite("memorial_rite_division_of_effects"));
            Assert.NotNull(catalog.GetMemorialRite("memorial_rite_work_gang_farewell"));
            Assert.NotNull(catalog.GetMemorialRite("memorial_rite_wall_tally_engraving"));
            Assert.NotNull(catalog.GetMemorialRite("memorial_rite_last_wish_committal"));

            // 3 belief movements
            Assert.Equal(3, catalog.Movements.Count);
            var ash = catalog.GetMovement("belief_ash_witnesses");
            Assert.NotNull(ash);
            Assert.Equal("The Ash Witnesses", ash!.DisplayName);
            Assert.Contains("Testes Cineris", ash.LatinName);

            var rebuilders = catalog.GetMovement("belief_rebuilders");
            Assert.NotNull(rebuilders);
            Assert.Equal("The Rebuilders", rebuilders!.DisplayName);

            var listeners = catalog.GetMovement("belief_listeners");
            Assert.NotNull(listeners);
            Assert.Equal("The Listeners", listeners!.DisplayName);
        }

        [Fact]
        public void ChildrenFolklore_ContainsExpandedPiecesWithOperationalTruth()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            string path = Path.Combine(dataDir, "narrative", "bunker_children_folklore.json");
            Assert.True(fileIO.FileExists(path));

            string json = fileIO.ReadAllText(path);
            var list = CatalogLocator.LoadWrappedList<ChildrenFolkloreEntry>(json, SystemTextJsonSerializer.Options);
            Assert.NotNull(list);
            Assert.True(list!.Count >= 17, $"Expected >= 17 folklore entries, found {list.Count}");

            var ids = new HashSet<string>();
            foreach (var item in list) ids.Add(item.Id);

            Assert.Contains("folklore_children_dosimeter_counting_rhyme", ids);
            Assert.Contains("folklore_children_deep_cold_lullaby", ids);
            Assert.Contains("folklore_children_the_outer_door_story", ids);
            Assert.Contains("folklore_children_the_vent_walker_ticking", ids);
            Assert.Contains("folklore_children_the_filter_ghost_rhyme", ids);
            Assert.Contains("folklore_children_three_mask_rule_song", ids);
            Assert.Contains("folklore_children_red_light_freeze_game", ids);
            Assert.Contains("folklore_children_the_missing_subfloor", ids);
            Assert.Contains("folklore_children_the_quiet_radio_whisper", ids);
            Assert.Contains("folklore_children_ash_footprint_taboo", ids);
            Assert.Contains("folklore_children_the_last_window_glass", ids);
            Assert.Contains("folklore_children_name_under_the_bunk", ids);
        }

        [Fact]
        public void GraffitiPostings_ContainsSpiritualAndFolkloreEchoes()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            string path = Path.Combine(dataDir, "bunker_graffiti_postings.json");
            Assert.True(fileIO.FileExists(path));

            string raw = fileIO.ReadAllText(path);
            Assert.Contains("graffiti_door_tap_tally", raw);
            Assert.Contains("graffiti_under_bunk_scratch", raw);
            Assert.Contains("graffiti_vent_walker_drawing", raw);
            Assert.Contains("graffiti_geiger_click_ladder", raw);
            Assert.Contains("graffiti_crossed_out_frequencies", raw);
            Assert.Contains("graffiti_false_window_sketch", raw);
        }

        [Fact]
        public void SpiritualMeaningCoordinator_RitualCooldownPreventsMoraleFarming()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = SpiritualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var coordinator = new SpiritualMeaningCoordinator(catalog);

            float totalMoraleAwarded = 0f;
            Action<float> moraleSink = delta => totalMoraleAwarded += delta;

            // Day 1: Execute door tap (+1.5 morale, 1d cooldown)
            bool firstTry = coordinator.TryPerformRitual("ritual_exterior_door_tap", 1, moraleSink);
            Assert.True(firstTry);
            Assert.Equal(1.5f, totalMoraleAwarded);

            // Day 1: Second attempt within cooldown fails!
            bool secondTry = coordinator.TryPerformRitual("ritual_exterior_door_tap", 1, moraleSink);
            Assert.False(secondTry);
            Assert.Equal(1.5f, totalMoraleAwarded); // no duplicate morale

            // Day 2: Cooldown expired -> executes successfully
            bool thirdTry = coordinator.TryPerformRitual("ritual_exterior_door_tap", 2, moraleSink);
            Assert.True(thirdTry);
            Assert.Equal(3.0f, totalMoraleAwarded);
        }

        [Fact]
        public void SpiritualMeaningCoordinator_MourningArcAdvancesDeterministically()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = SpiritualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var coordinator = new SpiritualMeaningCoordinator(catalog);

            string deceasedId = "sv_engineer_elena";
            coordinator.RegisterDeath(deceasedId, 10);

            var arc = coordinator.GetMourningArc(deceasedId);
            Assert.NotNull(arc);
            Assert.Equal(1, arc!.CurrentStage); // Stage 1: Acute Shock

            // Day 11 (+1 day) -> Stage 2: Empty Shift
            coordinator.TickMourning(11);
            Assert.Equal(2, arc.CurrentStage);

            // Day 13 (+3 days) -> Stage 3: Return of the Ordinary
            coordinator.TickMourning(13);
            Assert.Equal(3, arc.CurrentStage);

            // Day 17 (+7 days) -> Stage 4: Memorial Observance
            coordinator.TickMourning(17);
            Assert.Equal(4, arc.CurrentStage);

            // Perform memorial rite
            bool riteResult = coordinator.PerformMemorialRite(deceasedId, "memorial_rite_roll_call_naming", 18);
            Assert.True(riteResult);
            Assert.True(arc.RiteCompleted);
            Assert.Equal("memorial_rite_roll_call_naming", arc.PerformedRiteId);

            // Day 40 (+30 days) -> Stage 5: Long-Tail Echo / Anniversary
            coordinator.TickMourning(40);
            Assert.Equal(5, arc.CurrentStage);
        }

        [Fact]
        public void IdeologicalFriction_IncludesPlan30BeliefMovements()
        {
            var friction = new IdeologicalFrictionSystem();
            friction.RegisterBelief("sv_witness", "belief_ash_witnesses");
            friction.RegisterBelief("sv_rebuilder", "belief_rebuilders");
            friction.RegisterBelief("sv_second_witness", "belief_ash_witnesses");

            // Ash Witnesses vs Rebuilders -> Conflict penalty
            float compat = friction.GetRoommateCompatibilityMultiplier("sv_witness", "sv_rebuilder");
            Assert.Equal(1f - IdeologicalFrictionSystem.ConflictSleepQualityPenalty, compat);

            // Ash Witnesses vs Ash Witnesses -> Synergy bonus
            float synergy = friction.GetRoommateCompatibilityMultiplier("sv_witness", "sv_second_witness");
            Assert.Equal(1f + IdeologicalFrictionSystem.SynergySleepQualityBonus, synergy);
        }

        [Fact]
        public void SpiritualCoordinator_SaveLoadRoundTrip_PreservesArcsAndCooldowns()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = SpiritualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var coordA = new SpiritualMeaningCoordinator(catalog);

            coordA.RegisterDeath("sv_scout", 5);
            coordA.PerformMemorialRite("sv_scout", "memorial_rite_wall_tally_engraving", 6);
            coordA.TryPerformRitual("ritual_birthday_match_flame", 8);

            var save = coordA.CaptureState();
            Assert.NotNull(save);
            Assert.Single(save.MourningArcs);
            Assert.Single(save.RitualLastPerformedDay);

            var coordB = new SpiritualMeaningCoordinator(catalog);
            coordB.RestoreState(save);

            var arcB = coordB.GetMourningArc("sv_scout");
            Assert.NotNull(arcB);
            Assert.Equal(5, arcB!.DeathDay);
            Assert.Equal("memorial_rite_wall_tally_engraving", arcB.PerformedRiteId);
            Assert.True(arcB.RiteCompleted);

            Assert.False(coordB.CanPerformRitual("ritual_birthday_match_flame", 10)); // 5d cooldown from day 8 -> day 13
            Assert.True(coordB.CanPerformRitual("ritual_birthday_match_flame", 14));
        }
    }
}
