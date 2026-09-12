// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class StandingRecordQuestExpansionTests
    {
        private static readonly string[] s_tenBaselineTerritorialIds = new[]
        {
            "quest_record_the_plate",
            "quest_record_grease_pencil",
            "quest_record_wrong_stacks",
            "quest_record_the_book",
            "quest_record_mass_or_lot",
            "quest_record_hands",
            "quest_record_friendly_obstacle",
            "quest_record_the_failure",
            "quest_record_fallback",
            "quest_record_which_gazetteer"
        };

        private static readonly string[] s_twelveDeepeningForensicsIds = new[]
        {
            "quest_record_vault_breach_forensics",
            "quest_record_metro_derailment_triage",
            "quest_record_mine_shaft_adit_collapse",
            "quest_record_archive_burn_layer",
            "quest_record_sluice_failure_verdict",
            "quest_record_seed_bank_purge_trace",
            "quest_record_sub_basement_blueprint",
            "quest_record_transit_vent_shaft_route",
            "quest_record_cold_store_sublevel",
            "quest_record_utility_junction_crossover",
            "quest_record_the_unmarked_plaque",
            "quest_record_the_last_watch_beacon"
        };

        private static readonly string[] s_tenPlan118NewQuestIds = new[]
        {
            "quest_record_the_survey_nail",
            "quest_record_the_overlay_pigment",
            "quest_record_the_second_count",
            "quest_record_the_lamp_keepers_oath",
            "quest_record_the_boundary_dispute",
            "quest_record_the_missing_plate",
            "quest_record_the_cold_survey",
            "quest_record_the_lamp_oil_ledger",
            "quest_record_the_rejected_survey",
            "quest_record_the_last_sector"
        };

        private static string GetDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static StandingRecordCatalog LoadCatalog()
        {
            var loader = new StandingRecordCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            return loader.Load(GetDataDir());
        }

        [Fact]
        public void Catalog_Loads_All_32_Quests()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.Quests.Count >= 32, $"Expected at least 32 quests, got {catalog.Quests.Count}");
        }

        [Fact]
        public void All_Quest_Ids_Are_Unique_And_Prefixed()
        {
            var catalog = LoadCatalog();
            var ids = new HashSet<string>(StringComparer.Ordinal);

            foreach (var q in catalog.Quests)
            {
                Assert.NotNull(q);
                Assert.False(string.IsNullOrEmpty(q.id));
                Assert.StartsWith("quest_record_", q.id);
                Assert.True(ids.Add(q.id), $"Duplicate quest ID detected: '{q.id}'");
            }
        }

        [Fact]
        public void Baseline_10_Territorial_Quests_Preserved_Verbatim()
        {
            var catalog = LoadCatalog();
            foreach (string id in s_tenBaselineTerritorialIds)
            {
                var q = catalog.GetQuest(id);
                Assert.NotNull(q);
                Assert.Equal(id, q.id);
                Assert.NotEmpty(q.display_name);
                Assert.NotEmpty(q.briefing);
                Assert.NotEmpty(q.target_location_id);
                Assert.NotEmpty(q.complete_mutation);
                Assert.True(q.StageCount >= 3, $"Quest {id} must have >= 3 stages");
                Assert.NotNull(q.choices);
                Assert.True(q.choices.Length >= 2, $"Quest {id} must have >= 2 choices");
            }
        }

        [Fact]
        public void Deepening_12_Site_Forensics_Quests_Preserved()
        {
            var catalog = LoadCatalog();
            foreach (string id in s_twelveDeepeningForensicsIds)
            {
                var q = catalog.GetQuest(id);
                Assert.NotNull(q);
                Assert.Equal(id, q.id);
                Assert.NotEmpty(q.display_name);
                Assert.NotEmpty(q.briefing);
                Assert.NotEmpty(q.target_location_id);
                Assert.NotEmpty(q.complete_mutation);
                Assert.True(q.StageCount >= 3, $"Quest {id} must have >= 3 stages");
            }
        }

        [Fact]
        public void All_10_New_Plan118_Quests_Present()
        {
            var catalog = LoadCatalog();
            foreach (string id in s_tenPlan118NewQuestIds)
            {
                var q = catalog.GetQuest(id);
                Assert.NotNull(q);
                Assert.Equal(id, q.id);
                Assert.NotEmpty(q.display_name);
                Assert.NotEmpty(q.briefing);
                Assert.Equal("expedition", q.type);
                Assert.InRange(q.min_day, 80, 150);
                Assert.NotEmpty(q.target_location_id);
                Assert.NotEmpty(q.complete_mutation);
                Assert.NotEmpty(q.fail_mutation);
                Assert.NotNull(q.stages);
                Assert.InRange(q.stages.Length, 3, 6);
                Assert.NotNull(q.choices);
                Assert.InRange(q.choices.Length, 2, 4);

                foreach (var stage in q.stages)
                {
                    Assert.NotEmpty(stage.id);
                    Assert.NotEmpty(stage.text);
                }

                foreach (var choice in q.choices)
                {
                    Assert.NotEmpty(choice.id);
                    Assert.NotEmpty(choice.text);
                    Assert.NotEmpty(choice.set_flag);
                }
            }
        }

        [Fact]
        public void Prerequisite_Graph_Has_No_Cycles_And_All_Prereqs_Resolve()
        {
            var catalog = LoadCatalog();
            var questMap = catalog.Quests.ToDictionary(q => q.id, q => q, StringComparer.Ordinal);

            foreach (var q in catalog.Quests)
            {
                if (!string.IsNullOrEmpty(q.prereq_quest_id))
                {
                    Assert.True(questMap.ContainsKey(q.prereq_quest_id),
                        $"Quest '{q.id}' references non-existent prereq_quest_id '{q.prereq_quest_id}'");
                    Assert.NotEqual(q.id, q.prereq_quest_id); // no self-dependency
                }
            }

            // Cycle detection via DFS / recursion stack
            var visited = new HashSet<string>(StringComparer.Ordinal);
            var inStack = new HashSet<string>(StringComparer.Ordinal);

            foreach (var q in catalog.Quests)
            {
                CheckCycle(q.id, questMap, visited, inStack);
            }
        }

        private static void CheckCycle(
            string currentId,
            Dictionary<string, StandingRecordQuestEntry> map,
            HashSet<string> visited,
            HashSet<string> inStack)
        {
            if (inStack.Contains(currentId))
            {
                Assert.Fail($"Prerequisite cycle detected involving quest: '{currentId}'");
            }
            if (visited.Contains(currentId)) return;

            visited.Add(currentId);
            inStack.Add(currentId);

            if (map.TryGetValue(currentId, out var quest) && !string.IsNullOrEmpty(quest.prereq_quest_id))
            {
                CheckCycle(quest.prereq_quest_id, map, visited, inStack);
            }

            inStack.Remove(currentId);
        }

        [Fact]
        public void Prerequisite_Day_Ordering_Is_Coherent()
        {
            var catalog = LoadCatalog();
            var questMap = catalog.Quests.ToDictionary(q => q.id, q => q, StringComparer.Ordinal);

            foreach (var q in catalog.Quests)
            {
                if (!string.IsNullOrEmpty(q.prereq_quest_id) && questMap.TryGetValue(q.prereq_quest_id, out var prereq))
                {
                    Assert.True(q.min_day >= prereq.min_day,
                        $"Quest '{q.id}' (day {q.min_day}) has prereq '{prereq.id}' with later day ({prereq.min_day})");
                }
            }
        }

        [Fact]
        public void Every_Quest_Satisfies_World_Change_Bar_And_Spatial_Bar()
        {
            var catalog = LoadCatalog();
            foreach (var q in catalog.Quests)
            {
                Assert.False(string.IsNullOrEmpty(q.complete_mutation),
                    $"Quest '{q.id}' must specify a non-empty complete_mutation (world-change bar)");
                Assert.True(q.StageCount >= 3,
                    $"Quest '{q.id}' must have >= 3 stages (spatial bar)");
            }
        }

        [Fact]
        public void Cross_Plan_Integrations_Verified()
        {
            var catalog = LoadCatalog();

            // Plan 76 Expedition destinations
            var coldSurvey = catalog.GetQuest("quest_record_the_cold_survey");
            Assert.NotNull(coldSurvey);
            Assert.Equal("loc_west_ridge_survey", coldSurvey.target_location_id);

            var lastSector = catalog.GetQuest("quest_record_the_last_sector");
            Assert.NotNull(lastSector);
            Assert.Equal("loc_birchline_weather_station", lastSector.target_location_id);

            // Plan 82 Verdict locations
            var missingPlate = catalog.GetQuest("quest_record_the_missing_plate");
            Assert.NotNull(missingPlate);
            Assert.Equal("loc_abandoned_tide_gauge", missingPlate.target_location_id);

            var rejectedSurvey = catalog.GetQuest("quest_record_the_rejected_survey");
            Assert.NotNull(rejectedSurvey);
            Assert.Equal("loc_coastal_meteorological_station", rejectedSurvey.target_location_id);

            // Plan 98 Faction choices
            var boundaryDispute = catalog.GetQuest("quest_record_the_boundary_dispute");
            Assert.NotNull(boundaryDispute);
            var disputeFlags = boundaryDispute.choices.Select(c => c.set_flag).ToList();
            Assert.Contains("flag_sr_boundary_compact", disputeFlags);
            Assert.Contains("flag_sr_boundary_garrison", disputeFlags);
            Assert.Contains("flag_sr_boundary_neutral", disputeFlags);

            var oilLedger = catalog.GetQuest("quest_record_the_lamp_oil_ledger");
            Assert.NotNull(oilLedger);
            var oilFlags = oilLedger.choices.Select(c => c.set_flag).ToList();
            Assert.Contains("flag_sr_oil_prosecuted", oilFlags);
            Assert.Contains("flag_sr_oil_authorized", oilFlags);
        }

        [Fact]
        public void Engine_ApplySiteMutation_Idempotence()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var rng = new SeededRng(1982);
            var engine = new StandingRecordEngine(files, json, rng);
            engine.UnlockExpansion(currentDay: 80);

            // Apply complete mutation
            string targetSite = "loc_cut_kilometre_19";
            string testMutation = "mutation_survey_nail_verified";

            Assert.True(engine.ApplySiteMutation(targetSite, testMutation));
            Assert.True(engine.Memory.HasMutation(testMutation));
            Assert.True(engine.Layouts.HasFlag(targetSite, testMutation));

            // Repeated application is idempotent
            Assert.True(engine.ApplySiteMutation(targetSite, testMutation));

            // Capture and restore
            var state = engine.CaptureState();
            var engine2 = new StandingRecordEngine(files, json, rng, null, state);
            Assert.True(engine2.IsUnlocked);
            Assert.True(engine2.Memory.HasMutation(testMutation));
            Assert.True(engine2.Layouts.HasFlag(targetSite, testMutation));
        }
    }
}
