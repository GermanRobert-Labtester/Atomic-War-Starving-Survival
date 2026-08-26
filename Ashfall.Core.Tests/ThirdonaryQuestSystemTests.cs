using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Thirdonary;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Tests for ThirdonaryQuestSystem: catalog binding, lifecycle, tick triggers,
    /// cooldowns, trigger flags, and save round-trip fidelity.
    /// </summary>
    public class ThirdonaryQuestSystemTests
    {
        // ─── Helpers ───────────────────────────────────────────────────────────────

        private static ThirdonaryQuestDef MakeQuest(
            string id = "quest_third_test_stash",
            int minDay = 0,
            int maxDay = 0,
            int cooldownDays = 3,
            List<string>? triggerFlags = null)
        {
            return new ThirdonaryQuestDef
            {
                id = id,
                display_name = "Test Quest",
                category = "environmental",
                trigger = "A test trigger.",
                discovery = "You discover something.",
                min_day = minDay,
                max_day = maxDay,
                cooldown_days = cooldownDays,
                difficulty = "easy",
                moral_weight = "none",
                trigger_flags = triggerFlags ?? new List<string>(),
                choices = new List<ThirdonaryChoice>
                {
                    new ThirdonaryChoice
                    {
                        id = "choice_a",
                        label = "Do the thing",
                        outcome_text = "You did the thing.",
                        epitaph = "Did the thing.",
                        moral_delta = 5,
                        empathy_delta = 1,
                        effects = new List<ThirdonaryEffect>
                        {
                            new ThirdonaryEffect { type = "morale", target = "", value = 10 }
                        }
                    },
                    new ThirdonaryChoice
                    {
                        id = "choice_b",
                        label = "Don't do the thing",
                        outcome_text = "You walked away.",
                        epitaph = "Didn't do the thing.",
                        moral_delta = -3,
                        empathy_delta = 0,
                        effects = new List<ThirdonaryEffect>()
                    }
                }
            };
        }

        private static ThirdonaryWorldState MakeWorldState(
            int day = 5,
            HashSet<string>? flags = null)
        {
            return new ThirdonaryWorldState
            {
                CurrentDay = day,
                ActiveFlags = flags ?? new HashSet<string>(StringComparer.Ordinal)
            };
        }

        private static ThirdonaryQuestSystem MakeSystem(params ThirdonaryQuestDef[] quests)
        {
            var system = new ThirdonaryQuestSystem();
            system.BindCatalog(quests.ToList());
            return system;
        }

        // ─── Catalog Binding ──────────────────────────────────────────────────────

        [Fact]
        public void BindCatalog_LoadsAllQuests()
        {
            var system = MakeSystem(MakeQuest("quest_third_a"), MakeQuest("quest_third_b"));
            var world = MakeWorldState(flags: new HashSet<string>(StringComparer.Ordinal));
            var available = system.GetAvailableQuests(world);
            Assert.Equal(2, available.Count);
        }

        [Fact]
        public void BindCatalog_EmptyCatalog_NoQuestsAvailable()
        {
            var system = new ThirdonaryQuestSystem();
            system.BindCatalog(Array.Empty<ThirdonaryQuestDef>());
            var world = MakeWorldState();
            Assert.Empty(system.GetAvailableQuests(world));
        }

        // ─── Lifecycle ────────────────────────────────────────────────────────────

        [Fact]
        public void StartQuest_SetsStarted()
        {
            var system = MakeSystem(MakeQuest());
            system.StartQuest("quest_third_test_stash", 1);
            Assert.True(system.IsStarted("quest_third_test_stash"));
            Assert.False(system.IsCompleted("quest_third_test_stash"));
        }

        [Fact]
        public void StartQuest_AlreadyStarted_Noop()
        {
            var system = MakeSystem(MakeQuest());
            system.StartQuest("quest_third_test_stash", 1);
            system.StartQuest("quest_third_test_stash", 2);
            var progress = system.GetProgress("quest_third_test_stash");
            Assert.Equal(1, progress!.day_started);
        }

        [Fact]
        public void StartQuest_UnknownId_Noop()
        {
            var system = MakeSystem(MakeQuest());
            system.StartQuest("quest_nonexistent", 1);
            Assert.False(system.IsStarted("quest_nonexistent"));
        }

        [Fact]
        public void CompleteQuest_SetsCompleted()
        {
            var system = MakeSystem(MakeQuest());
            system.StartQuest("quest_third_test_stash", 1);
            system.CompleteQuest("quest_third_test_stash", 2);
            Assert.True(system.IsCompleted("quest_third_test_stash"));
            Assert.Equal(1, system.QuestsCompleted);
        }

        [Fact]
        public void CompleteQuest_NotStarted_Noop()
        {
            var system = MakeSystem(MakeQuest());
            system.CompleteQuest("quest_third_test_stash", 1);
            Assert.False(system.IsCompleted("quest_third_test_stash"));
        }

        [Fact]
        public void FailQuest_SetsFailed()
        {
            var system = MakeSystem(MakeQuest());
            system.StartQuest("quest_third_test_stash", 1);
            system.FailQuest("quest_third_test_stash", 2);
            Assert.True(system.IsFailed("quest_third_test_stash"));
            Assert.Equal(1, system.QuestsFailed);
        }

        [Fact]
        public void MakeChoice_SetsChosenChoiceId()
        {
            var system = MakeSystem(MakeQuest());
            system.StartQuest("quest_third_test_stash", 1);
            system.MakeChoice("quest_third_test_stash", "choice_a", 1);
            var progress = system.GetProgress("quest_third_test_stash");
            Assert.Equal("choice_a", progress!.chosen_choice_id);
        }

        // ─── TickDay / Trigger Evaluation ─────────────────────────────────────────

        [Fact]
        public void TickDay_StartsEligibleQuests()
        {
            var quest = MakeQuest(triggerFlags: new List<string> { "flag_debris" });
            var system = MakeSystem(quest);
            var world = MakeWorldState(flags: new HashSet<string>(StringComparer.Ordinal) { "flag_debris" });

            var started = system.TickDay(world);

            Assert.Single(started);
            Assert.Equal("quest_third_test_stash", started[0]);
            Assert.True(system.IsStarted("quest_third_test_stash"));
        }

        [Fact]
        public void TickDay_MissingFlags_DoesNotStart()
        {
            var quest = MakeQuest(triggerFlags: new List<string> { "flag_debris", "flag_nearby" });
            var system = MakeSystem(quest);
            var world = MakeWorldState(flags: new HashSet<string>(StringComparer.Ordinal) { "flag_debris" });

            var started = system.TickDay(world);

            Assert.Empty(started);
            Assert.False(system.IsStarted("quest_third_test_stash"));
        }

        [Fact]
        public void TickDay_EmptyFlags_AlwaysEligible()
        {
            var quest = MakeQuest(triggerFlags: new List<string>());
            var system = MakeSystem(quest);
            var world = MakeWorldState();

            var started = system.TickDay(world);

            Assert.Single(started);
        }

        [Fact]
        public void TickDay_RespectsMinDay()
        {
            var quest = MakeQuest(minDay: 10);
            var system = MakeSystem(quest);
            var world = MakeWorldState(day: 5);

            var started = system.TickDay(world);

            Assert.Empty(started);
        }

        [Fact]
        public void TickDay_RespectsMaxDay()
        {
            var quest = MakeQuest(maxDay: 10);
            var system = MakeSystem(quest);
            var world = MakeWorldState(day: 15);

            var started = system.TickDay(world);

            Assert.Empty(started);
        }

        [Fact]
        public void TickDay_RespectsCooldown()
        {
            var quest = MakeQuest(cooldownDays: 5);
            var system = MakeSystem(quest);

            // Start and complete on day 1
            system.StartQuest("quest_third_test_stash", 1);
            system.CompleteQuest("quest_third_test_stash", 1);

            // Try again on day 3 (within cooldown)
            var world = MakeWorldState(day: 3);
            var started = system.TickDay(world);
            Assert.Empty(started);

            // Try again on day 7 (past cooldown)
            var world2 = MakeWorldState(day: 7);
            var started2 = system.TickDay(world2);
            Assert.Single(started2);
        }

        [Fact]
        public void TickDay_ZeroCooldown_OneShot()
        {
            var quest = MakeQuest(cooldownDays: 0);
            var system = MakeSystem(quest);

            system.StartQuest("quest_third_test_stash", 1);
            system.CompleteQuest("quest_third_test_stash", 1);

            var world = MakeWorldState(day: 100);
            var started = system.TickDay(world);
            Assert.Empty(started);
        }

        [Fact]
        public void TickDay_AlreadyStarted_DoesNotRestart()
        {
            var quest = MakeQuest();
            var system = MakeSystem(quest);

            system.StartQuest("quest_third_test_stash", 1);
            var world = MakeWorldState(day: 2);
            var started = system.TickDay(world);
            Assert.Empty(started);
        }

        [Fact]
        public void TickDay_NullWorldState_ReturnsEmpty()
        {
            var system = MakeSystem(MakeQuest());
            var started = system.TickDay(null!);
            Assert.Empty(started);
        }

        // ─── Events ───────────────────────────────────────────────────────────────

        [Fact]
        public void OnQuestStarted_Fires()
        {
            var system = MakeSystem(MakeQuest());
            ThirdonaryQuestDef? fired = null;
            system.OnQuestStarted += d => fired = d;

            system.StartQuest("quest_third_test_stash", 1);

            Assert.NotNull(fired);
            Assert.Equal("quest_third_test_stash", fired!.id);
        }

        [Fact]
        public void OnQuestCompleted_Fires()
        {
            var system = MakeSystem(MakeQuest());
            ThirdonaryQuestDef? fired = null;
            system.OnQuestCompleted += d => fired = d;

            system.StartQuest("quest_third_test_stash", 1);
            system.CompleteQuest("quest_third_test_stash", 2);

            Assert.NotNull(fired);
        }

        [Fact]
        public void OnQuestFailed_Fires()
        {
            var system = MakeSystem(MakeQuest());
            ThirdonaryQuestDef? fired = null;
            system.OnQuestFailed += d => fired = d;

            system.StartQuest("quest_third_test_stash", 1);
            system.FailQuest("quest_third_test_stash", 2);

            Assert.NotNull(fired);
        }

        // ─── Save Round-Trip ──────────────────────────────────────────────────────

        [Fact]
        public void CaptureRestore_RoundTrip()
        {
            var system = MakeSystem(MakeQuest(), MakeQuest("quest_third_b"));
            system.StartQuest("quest_third_test_stash", 1);
            system.MakeChoice("quest_third_test_stash", "choice_a", 1);
            system.CompleteQuest("quest_third_test_stash", 2);

            var captured = system.CaptureState();

            var system2 = MakeSystem(MakeQuest(), MakeQuest("quest_third_b"));
            system2.RestoreState(captured);

            Assert.True(system2.IsCompleted("quest_third_test_stash"));
            Assert.False(system2.IsStarted("quest_third_b"));
            var progress = system2.GetProgress("quest_third_test_stash");
            Assert.Equal("choice_a", progress!.chosen_choice_id);
            Assert.Equal(2, progress.day_resolved);
        }

        [Fact]
        public void CaptureRestore_DeepCopy_NoAliasing()
        {
            var system = MakeSystem(MakeQuest());
            system.StartQuest("quest_third_test_stash", 1);

            var captured = system.CaptureState();

            // Modify original
            system.CompleteQuest("quest_third_test_stash", 2);

            // Captured should be unchanged
            var capturedProgress = captured.quests.First(q => q.quest_id == "quest_third_test_stash");
            Assert.False(capturedProgress.completed);
        }

        [Fact]
        public void RestoreState_InvalidSystemId_Throws()
        {
            var system = MakeSystem(MakeQuest());
            var badState = new ThirdonaryState { system_id = "wrong_system", schema_version = 1 };
            Assert.Throws<ArgumentException>(() => system.RestoreState(badState));
        }

        [Fact]
        public void RestoreState_FutureSchema_Throws()
        {
            var system = MakeSystem(MakeQuest());
            var futureState = new ThirdonaryState { system_id = ThirdonaryQuestSystem.SystemId, schema_version = 99 };
            Assert.Throws<NotSupportedException>(() => system.RestoreState(futureState));
        }

        [Fact]
        public void RestoreState_Null_Throws()
        {
            var system = MakeSystem(MakeQuest());
            Assert.Throws<ArgumentNullException>(() => system.RestoreState(null!));
        }

        // ─── Active / Available Queries ───────────────────────────────────────────

        [Fact]
        public void GetActiveQuests_ReturnsStartedNotCompleted()
        {
            var system = MakeSystem(MakeQuest(), MakeQuest("quest_third_b"));
            system.StartQuest("quest_third_test_stash", 1);
            system.StartQuest("quest_third_b", 1);
            system.CompleteQuest("quest_third_b", 2);

            var active = system.GetActiveQuests();
            Assert.Single(active);
            Assert.Equal("quest_third_test_stash", active[0].id);
        }

        [Fact]
        public void GetAvailableQuests_ExcludesStartedAndCompleted()
        {
            var quest = MakeQuest(triggerFlags: new List<string>(), cooldownDays: 0);
            var system = MakeSystem(quest);
            system.StartQuest("quest_third_test_stash", 1);
            system.CompleteQuest("quest_third_test_stash", 2);

            var world = MakeWorldState(day: 10);
            var available = system.GetAvailableQuests(world);
            Assert.Empty(available);
        }

        // ─── Catalog Loader ───────────────────────────────────────────────────────

        [Fact]
        public void CatalogLoader_LoadsFromJson()
        {
            string dataDir = System.IO.Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", "..",
                "Assets", "StreamingAssets", "Data");
            if (!System.IO.Directory.Exists(dataDir))
            {
                // CI or different working directory — skip
                return;
            }

            var catalog = ThirdonaryCatalogLoader.Load(dataDir);
            Assert.Equal(75, catalog.Count);
            Assert.All(catalog, q => Assert.StartsWith("quest_third_", q.id, StringComparison.Ordinal));
        }

        // ─── Master Catalog Registration ──────────────────────────────────────────

        [Fact]
        public void QuestIdsRegistered_InMasterCatalog()
        {
            string dataDir = System.IO.Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", "..",
                "Assets", "StreamingAssets", "Data");
            if (!System.IO.Directory.Exists(dataDir))
            {
                return;
            }

            var loader = new QuestlineMasterCatalogLoader(
                new FileSystemIO(), new SystemTextJsonSerializer());
            var master = loader.Load(dataDir);

            var thirdonaryIds = new[]
            {
                "quest_third_hidden_stash",
                "quest_third_landmark",
                "quest_third_graffiti_message",
                "quest_third_safe_route",
                "quest_third_resource_spot",
                "quest_third_water_source",
                "quest_third_collapsed_structure",
                "quest_third_abandoned_vehicle",
                "quest_third_dead_drop",
                "quest_third_radio_signal",
                "quest_third_overgrown_garden",
                "quest_third_scorched_earth",
                "quest_third_hidden_bunker",
                "quest_third_supply_cache",
                "quest_third_memorial",
                "quest_third_craft_repair_tool",
                "quest_third_craft_improvise_lockpick",
                "quest_third_craft_upgrade_flashlight",
                "quest_third_craft_shelter_patch",
                "quest_third_craft_salvage_wiring",
                "quest_third_craft_water_filter",
                "quest_third_craft_sharpen_blade",
                "quest_third_craft_clothing_mend",
                "quest_third_craft_trap_repair",
                "quest_third_craft_stove_fix",
                "quest_third_craft_battery_swap",
                "quest_third_craft_boot_sole",
                "quest_third_craft_window_board",
                "quest_third_craft_door_hinge",
                "quest_third_craft_can_opener",
                "quest_third_med_bandage_wound",
                "quest_third_med_boil_water",
                "quest_third_med_breathe_exercise",
                "quest_third_med_clean_infection",
                "quest_third_med_find_antibiotics",
                "quest_third_med_find_painkillers",
                "quest_third_med_food_safety",
                "quest_third_med_iodine_pill",
                "quest_third_med_rest_exhaustion",
                "quest_third_med_sleep_hygiene",
                "quest_third_med_splint_break",
                "quest_third_med_sunburn_treat",
                "quest_third_med_treat_blister",
                "quest_third_med_treat_burn",
                "quest_third_med_water_ration",
                "quest_third_combat_ambush_avoid",
                "quest_third_combat_darkness_move",
                "quest_third_combat_defend_camp",
                "quest_third_combat_dog_encounter",
                "quest_third_combat_escape_pursuit",
                "quest_third_combat_group_encounter",
                "quest_third_combat_mutant_scare",
                "quest_third_combat_negotiate_passage",
                "quest_third_combat_noise_discipline",
                "quest_third_combat_raider_patrol",
                "quest_third_combat_scavenge_threat",
                "quest_third_combat_shelter_fortify",
                "quest_third_combat_sniper_awareness",
                "quest_third_combat_trap_detect",
                "quest_third_combat_weapon_find",
                "quest_third_lore_children_drawing",
                "quest_third_lore_clock_tower",
                "quest_third_lore_coded_message",
                "quest_third_lore_folk_song",
                "quest_third_lore_graffiti_story",
                "quest_third_lore_inscription",
                "quest_third_lore_letter_fragment",
                "quest_third_lore_map_fragment",
                "quest_third_lore_medical_record",
                "quest_third_lore_memorial_wall",
                "quest_third_lore_newspaper",
                "quest_third_lore_photograph",
                "quest_third_lore_radio_archive",
                "quest_third_lore_recording",
                "quest_third_lore_school_exercise"
            };

            foreach (var id in thirdonaryIds)
            {
                Assert.True(master.IsRegistered(id), $"Quest ID '{id}' not registered in questline_master.json");
            }
        }

        // ─── Moral Quest Extensions ───────────────────────────────────────────────

        [Fact]
        public void MoralQuestExtensions_Loaded()
        {
            string dataDir = System.IO.Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", "..",
                "Assets", "StreamingAssets", "Data");
            if (!System.IO.Directory.Exists(dataDir))
            {
                return;
            }

            string path = System.IO.Path.Combine(dataDir, "moral_choice_quests.json");
            string json = System.IO.File.ReadAllText(path);

            Assert.Contains("quest_moral_env_scavenger_child", json);
            Assert.Contains("quest_moral_env_buried_letters", json);
            Assert.Contains("quest_moral_env_shelter_refugee", json);
            Assert.Contains("quest_moral_env_dead_explorer", json);
            Assert.Contains("quest_moral_env_wounded_scavenger", json);
        }
    }
}
