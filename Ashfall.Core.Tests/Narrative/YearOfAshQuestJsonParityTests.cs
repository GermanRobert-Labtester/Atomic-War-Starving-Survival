using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.YearOfAsh;

namespace Ashfall.Core.Tests.Narrative
{
    public class YearOfAshQuestJsonParityTests
    {
        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
        }

        [Fact]
        public void ExportAndVerify_All8Questlines_MatchBaselineExact()
        {
            var baseline = BuiltInQuestlineCatalog.CreateAll();
            Assert.Equal(8, baseline.Count);

            var container = new YearOfAshQuestContainer
            {
                schema_version = 1,
                quests = baseline
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };
            string jsonString = JsonSerializer.Serialize(container, options);

            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            string targetPath = fileIO.Combine(dataDir, YearOfAshCatalogLoader.QuestlinesFile);

            // Write the canonical year_of_ash_questlines.json
            fileIO.WriteAllText(targetPath, jsonString);

            // Load back through Core catalog loader
            var serializer = new SystemTextJsonSerializer();
            var loaded = YearOfAshCatalogLoader.LoadQuestlines(dataDir, fileIO, serializer);

            Assert.Equal(baseline.Count, loaded.Count);

            for (int i = 0; i < baseline.Count; i++)
            {
                var baseQ = baseline[i];
                var loadQ = loaded.Find(q => q.questlineId == baseQ.questlineId);
                Assert.NotNull(loadQ);

                Assert.Equal(baseQ.questlineId, loadQ.questlineId);
                Assert.Equal(baseQ.title, loadQ.title);
                Assert.Equal(baseQ.synopsis, loadQ.synopsis);
                Assert.Equal(baseQ.factionTag, loadQ.factionTag);
                Assert.Equal(baseQ.firstStageId, loadQ.firstStageId);
                Assert.Equal(baseQ.minDay, loadQ.minDay);
                Assert.Equal(baseQ.maxDay, loadQ.maxDay);
                Assert.Equal(baseQ.stages.Count, loadQ.stages.Count);

                for (int s = 0; s < baseQ.stages.Count; s++)
                {
                    var baseStage = baseQ.stages[s];
                    var loadStage = loadQ.stages.Find(st => st.stageId == baseStage.stageId);
                    Assert.NotNull(loadStage);

                    Assert.Equal(baseStage.stageId, loadStage.stageId);
                    Assert.Equal(baseStage.title, loadStage.title);
                    Assert.Equal(baseStage.narrativePrompt, loadStage.narrativePrompt);
                    Assert.Equal(baseStage.unlockOnDay, loadStage.unlockOnDay);
                    Assert.Equal(baseStage.isTerminal, loadStage.isTerminal);
                    Assert.Equal(baseStage.terminalOutcome, loadStage.terminalOutcome);
                    Assert.Equal(baseStage.choices.Count, loadStage.choices.Count);

                    for (int c = 0; c < baseStage.choices.Count; c++)
                    {
                        var baseChoice = baseStage.choices[c];
                        var loadChoice = loadStage.choices.Find(ch => ch.choiceId == baseChoice.choiceId);
                        Assert.NotNull(loadChoice);

                        Assert.Equal(baseChoice.choiceId, loadChoice.choiceId);
                        Assert.Equal(baseChoice.text, loadChoice.text);
                        Assert.Equal(baseChoice.nextStageId, loadChoice.nextStageId);
                        Assert.Equal(baseChoice.moraleDelta, loadChoice.moraleDelta);
                        Assert.Equal(baseChoice.guiltDelta, loadChoice.guiltDelta);
                        Assert.Equal(baseChoice.grantItemId ?? string.Empty, loadChoice.grantItemId ?? string.Empty);
                        Assert.Equal(baseChoice.grantItemQuantity, loadChoice.grantItemQuantity);
                        Assert.Equal(baseChoice.targetFactionId ?? string.Empty, loadChoice.targetFactionId ?? string.Empty);
                        Assert.Equal(baseChoice.factionStandingDelta, loadChoice.factionStandingDelta);
                        Assert.Equal(baseChoice.unlockEncounterId ?? string.Empty, loadChoice.unlockEncounterId ?? string.Empty);
                        Assert.Equal(baseChoice.outcomeNarrative, loadChoice.outcomeNarrative);

                        int baseCondCount = baseChoice.conditions?.Count ?? 0;
                        int loadCondCount = loadChoice.conditions?.Count ?? 0;
                        Assert.Equal(baseCondCount, loadCondCount);

                        for (int k = 0; k < baseCondCount; k++)
                        {
                            Assert.Equal(baseChoice.conditions[k].conditionTag, loadChoice.conditions[k].conditionTag);
                            Assert.Equal(baseChoice.conditions[k].isBlocker, loadChoice.conditions[k].isBlocker);
                        }
                    }
                }
            }
        }

        [Fact]
        public void PilotQuestline_GarrisonBloodDebt_LoadsAndPlaysThroughChoices()
        {
            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var quests = YearOfAshCatalogLoader.LoadQuestlines(dataDir, fileIO, serializer);
            var bloodDebt = quests.Find(q => q.questlineId == "quest_garrison_blood_debt");
            Assert.NotNull(bloodDebt);

            var system = new QuestlineSystem();
            system.RegisterQuestline(bloodDebt);

            Assert.True(system.StartQuestline("quest_garrison_blood_debt", 185));
            var record = system.State.active.Find(a => a.questlineId == "quest_garrison_blood_debt");
            Assert.NotNull(record);
            Assert.Equal("stage_blood_debt_demand", record.currentStageId);

            // Choice 1: confront Ola
            var r1 = system.TakeChoice("quest_garrison_blood_debt", "choice_confront_ola", 185);
            Assert.NotNull(r1);
            Assert.Equal("stage_blood_debt_ola_testimony", record.currentStageId);

            // Choice 2: forge rebuttal
            var r2 = system.TakeChoice("quest_garrison_blood_debt", "choice_forge_tribunal_rebuttal", 187);
            Assert.NotNull(r2);
            Assert.Equal("stage_blood_debt_garrison_bluff", record.currentStageId);
            Assert.Equal("item_falsified_clearance", r2.grantItemId);

            // Choice 3: pass bluff
            var r3 = system.TakeChoice("quest_garrison_blood_debt", "choice_pass_the_bluff", 200);
            Assert.NotNull(r3);
            Assert.Equal("stage_blood_debt_resolution_protected", record.currentStageId);
            Assert.Equal(QuestlineStatus.Completed, record.status);
        }
    }
}
