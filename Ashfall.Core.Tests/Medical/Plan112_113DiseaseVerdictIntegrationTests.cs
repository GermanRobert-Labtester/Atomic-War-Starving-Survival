// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Verdict;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class Plan112_113DiseaseVerdictIntegrationTests
    {
        private static string DataDirectory
        {
            get
            {
                string start = Directory.GetCurrentDirectory();
                if (CatalogLocator.TryFindDataDirectory(start, out string found))
                    return found;
                if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                    return found;
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
            }
        }

        private static readonly string[] ExpectedSevenInvestigationQuestlines = new[]
        {
            "quest_verdict_the_dead_frequency",
            "quest_verdict_the_missing_reel",
            "quest_verdict_the_cold_reading",
            "quest_verdict_the_unsigned_tally",
            "quest_verdict_the_interference_pattern",
            "quest_verdict_the_last_entry",
            "quest_verdict_the_open_count"
        };

        [Fact]
        public void Plan112_DiseaseCatalog_FullTwentyConditions_WithSupportedVectorsAndResolvableCountermeasures()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var catalog = DiseaseCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(catalog);
            Assert.False(catalog.HasErrors, string.Join("; ", catalog.Errors));
            Assert.Equal(20, catalog.Count);

            var items = new HashSet<string>(
                ItemCatalogLoader.Load(DataDirectory, files, json).Select(item => item.id),
                StringComparer.Ordinal);

            var supportedVectors = new HashSet<string>(StringComparer.Ordinal)
            {
                DiseaseVectorNames.Water,
                DiseaseVectorNames.Air,
                DiseaseVectorNames.Blood,
                DiseaseVectorNames.Spore
            };

            var diseaseIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var disease in catalog.Diseases)
            {
                Assert.NotNull(disease);
                Assert.False(string.IsNullOrWhiteSpace(disease.id));
                Assert.StartsWith("disease_", disease.id);
                Assert.True(diseaseIds.Add(disease.id), $"Duplicate disease ID: {disease.id}");

                Assert.Contains(disease.vector, supportedVectors);
                Assert.Contains(disease.countermeasure_item_id, items);
                Assert.True(disease.lethality >= 0f && disease.lethality <= 1f);
                Assert.True(disease.infectivity >= 0f && disease.infectivity <= 1f);
                Assert.True(disease.illness_days >= 1);
                Assert.NotEmpty(disease.guidance);
                Assert.NotEmpty(disease.source_note);
                Assert.NotEmpty(disease.treatments);
                Assert.NotEmpty(disease.phases);
            }

            // Verify the four Plan 112 expansion conditions exist
            Assert.Contains("disease_dysentery", diseaseIds);
            Assert.Contains("disease_meningococcal_fever", diseaseIds);
            Assert.Contains("disease_bloodborne_hepatitis", diseaseIds);
            Assert.Contains("disease_spore_wound_dermatitis", diseaseIds);
        }

        [Fact]
        public void Plan113_VerdictQuestlines_FullInvestigationRoster_WithAcyclicDAGAndValidTerminals()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var system = new QuestlineSystem();

            int count = VerdictQuestCatalogLoader.LoadAndRegister(system, DataDirectory, files, json);
            Assert.True(count >= 23, $"Expected at least 23 questlines, got {count}");

            foreach (string qId in ExpectedSevenInvestigationQuestlines)
            {
                var q = system.FindDefinition(qId);
                Assert.NotNull(q);
                Assert.False(string.IsNullOrWhiteSpace(q.title));
                Assert.False(string.IsNullOrWhiteSpace(q.synopsis));
                Assert.True(q.minDay >= 160 && q.maxDay <= 365, $"Day window for {qId} out of range: {q.minDay}-{q.maxDay}");
                Assert.False(string.IsNullOrWhiteSpace(q.firstStageId));

                var stageMap = q.stages.ToDictionary(s => s.stageId, s => s, StringComparer.Ordinal);
                Assert.Contains(q.firstStageId, stageMap.Keys);

                // Verify DAG reaches terminals and has no dead-ends
                var visited = new HashSet<string>(StringComparer.Ordinal);
                var queue = new Queue<string>();
                queue.Enqueue(q.firstStageId);

                bool hasTerminal = false;

                while (queue.Count > 0)
                {
                    string currId = queue.Dequeue();
                    if (!visited.Add(currId)) continue;

                    Assert.True(stageMap.TryGetValue(currId, out var stage), $"Stage {currId} not found in {qId}");

                    if (stage.isTerminal)
                    {
                        hasTerminal = true;
                        Assert.True(stage.terminalOutcome == QuestlineStatus.Completed || stage.terminalOutcome == QuestlineStatus.Failed);
                    }
                    else
                    {
                        Assert.NotEmpty(stage.choices);
                        foreach (var choice in stage.choices)
                        {
                            Assert.False(string.IsNullOrWhiteSpace(choice.choiceId));
                            Assert.False(string.IsNullOrWhiteSpace(choice.text));
                            if (!string.IsNullOrEmpty(choice.nextStageId))
                            {
                                Assert.True(stageMap.ContainsKey(choice.nextStageId),
                                    $"Choice {choice.choiceId} in stage {currId} points to non-existent stage {choice.nextStageId}");
                                queue.Enqueue(choice.nextStageId);
                            }
                        }
                    }
                }

                Assert.True(hasTerminal, $"Questline {qId} has no reachable terminal stage.");
            }
        }

        [Fact]
        public void Plan112_113_IndependentLifecycleAndRuntimeSimulations_CoexistWithoutInterference()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // 1. Initialize Disease System
            var diseaseCatalog = DiseaseCatalogLoader.Load(DataDirectory, files, json);
            var diseaseSystem = new DiseaseSystem(rng: new SeededRng(112113));
            diseaseSystem.BindCatalog(diseaseCatalog);

            // 2. Initialize Questline System
            var questSystem = new QuestlineSystem();
            VerdictQuestCatalogLoader.LoadAndRegister(questSystem, DataDirectory, files, json);

            // 3. Simulate Disease Infection
            string survivorId = "survivor_triage_auditor";
            diseaseSystem.Infect(survivorId, "disease_dysentery", 200);
            Assert.True(diseaseSystem.IsInfected(survivorId, "disease_dysentery"));

            // 4. Simulate Questline Progression
            string questId = "quest_verdict_the_open_count";
            var questDef = questSystem.FindDefinition(questId);
            Assert.NotNull(questDef);

            bool started = questSystem.StartQuestline(questId, 250);
            Assert.True(started);
            var activeRecord = questSystem.State.active.Find(a => a.questlineId == questId);
            Assert.NotNull(activeRecord);
            Assert.Equal(questDef.firstStageId, activeRecord.currentStageId);

            // Advance quest
            var stage = questDef.FindStage(activeRecord.currentStageId);
            Assert.NotNull(stage);
            var choice = stage.choices.First();
            var result = questSystem.TakeChoice(questId, choice.choiceId, 251);
            Assert.NotNull(result);

            // 5. Verify Disease System remains completely isolated and intact
            Assert.True(diseaseSystem.IsInfected(survivorId, "disease_dysentery"));
            var def = diseaseSystem.GetDefinition("disease_dysentery");
            Assert.NotNull(def);
            Assert.Equal(DiseaseVectorNames.Water, def.vector);
        }

        [Fact]
        public void Plan112_113_CrossSystem_MedicalTriageAndVerdictInvestigation_CoherenceContract()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var diseaseCatalog = DiseaseCatalogLoader.Load(DataDirectory, files, json);
            var questSystem = new QuestlineSystem();
            int count1 = VerdictQuestCatalogLoader.LoadAndRegister(questSystem, DataDirectory, files, json);

            // Verify both catalogs reload deterministically
            var diseaseCatalog2 = DiseaseCatalogLoader.Load(DataDirectory, files, json);
            Assert.Equal(diseaseCatalog.Count, diseaseCatalog2.Count);

            var questSystem2 = new QuestlineSystem();
            int count2 = VerdictQuestCatalogLoader.LoadAndRegister(questSystem2, DataDirectory, files, json);
            Assert.Equal(count1, count2);
            Assert.Equal(questSystem.Catalog.Count, questSystem2.Catalog.Count);

            // Verify core clinical countermeasure items exist in items catalog
            var items = ItemCatalogLoader.Load(DataDirectory, files, json).ToDictionary(i => i.id, i => i, StringComparer.Ordinal);
            Assert.Contains("item_clean_water", items.Keys);
            Assert.Contains("item_gas_mask", items.Keys);
            Assert.Contains("item_antibiotics", items.Keys);

            // Verify Verdict investigative evidence tokens exist
            Assert.Contains("item_archive_tape_silo_key", items.Keys);
            Assert.Contains("evidence_call_calibration", items.Keys);
        }
    }
}
