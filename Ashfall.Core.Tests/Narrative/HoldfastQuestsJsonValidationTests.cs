using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// Canonical-ID, reachability, and load-equivalence tests for holdfast_quests.json.
    /// Validates that all quest definitions in the JSON file meet the schema requirements
    /// and can be loaded correctly by the HoldfastCatalogLoader.
    /// </summary>
    public class HoldfastQuestsJsonValidationTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static HoldfastCatalog LoadCatalog()
        {
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            return loader.Load(DataDir());
        }

        [Fact]
        public void HoldfastQuestsJsonHasSchemaVersion()
        {
            string jsonPath = Path.Combine(DataDir(), HoldfastCatalogLoader.QuestsFile);
            Assert.True(File.Exists(jsonPath), "holdfast_quests.json must exist in StreamingAssets/Data");

            string json = File.ReadAllText(jsonPath);
            Assert.Contains("\"schema_version\"", json);
        }

        [Fact]
        public void HoldfastQuestsJsonIsValid()
        {
            string jsonPath = Path.Combine(DataDir(), HoldfastCatalogLoader.QuestsFile);
            Assert.True(File.Exists(jsonPath), "holdfast_quests.json must exist in StreamingAssets/Data");

            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var catalog = loader.Load(DataDir());

            // Should load without errors
            Assert.NotNull(catalog);
            Assert.NotNull(catalog.Quests);
        }

        [Fact]
        public void AllQuestIdsAreCanonicalSnakeCase()
        {
            var catalog = LoadCatalog();

            foreach (var quest in catalog.Quests)
            {
                Assert.NotNull(quest.id);
                Assert.NotEmpty(quest.id);
                Assert.StartsWith("quest_holdfast_", quest.id);

                // Validate snake_case pattern: quest_holdfast_{name}
                string suffix = quest.id.Substring("quest_holdfast_".Length);
                Assert.Matches("^[a-z0-9_]+$", suffix);

                // Validate no uppercase letters
                Assert.Equal(quest.id.ToLowerInvariant(), quest.id);
            }
        }

        [Fact]
        public void AllQuestIdsAreUnique()
        {
            var catalog = LoadCatalog();
            var ids = new HashSet<string>();

            foreach (var quest in catalog.Quests)
            {
                Assert.DoesNotContain(quest.id, ids);
                ids.Add(quest.id);
            }
        }

        [Fact]
        public void AllRequiredFieldsPresentInJsonQuests()
        {
            var catalog = LoadCatalog();

            foreach (var quest in catalog.Quests)
            {
                Assert.NotNull(quest.id);
                Assert.NotNull(quest.display_name);
                Assert.NotNull(quest.type);
                Assert.NotNull(quest.briefing);
                Assert.NotNull(quest.stages);
                Assert.NotNull(quest.choices);

                // Validate stages array
                Assert.True(quest.StageCount > 0, $"Quest {quest.id} must have at least one stage");

                foreach (var stage in quest.stages)
                {
                    Assert.NotNull(stage.id);
                    Assert.NotNull(stage.text);
                    Assert.NotEmpty(stage.text);
                }

                // Validate choices array
                Assert.NotNull(quest.choices);
            }
        }

        [Fact]
        public void AllMainQuestIdsExistInJson()
        {
            var catalog = LoadCatalog();

            foreach (string questId in HoldfastQuestSystem.MainQuestIds)
            {
                var quest = catalog.GetQuest(questId);
                Assert.True(quest != null, $"Main quest ID '{questId}' must exist in holdfast_quests.json");
            }
        }

        [Fact]
        public void JsonQuestIdsMatchCSharpConstants()
        {
            var catalog = LoadCatalog();

            var csharpIds = new HashSet<string>(HoldfastQuestSystem.MainQuestIds);
            var jsonIds = new HashSet<string>();

            foreach (var quest in catalog.Quests)
            {
                jsonIds.Add(quest.id);
            }

            // All C# constants should exist in JSON
            foreach (string csharpId in csharpIds)
            {
                Assert.Contains(csharpId, jsonIds);
            }
        }

        [Fact]
        public void AllQuestTypesAreValid()
        {
            var catalog = LoadCatalog();
            var validTypes = new HashSet<string> { "expedition", "dialogue", "exploration", "decision", "crisis" };

            foreach (var quest in catalog.Quests)
            {
                Assert.NotNull(quest.type);
                Assert.Contains(quest.type.ToLowerInvariant(), validTypes);
            }
        }

        [Fact]
        public void AllStagesHaveValidIds()
        {
            var catalog = LoadCatalog();

            foreach (var quest in catalog.Quests)
            {
                for (int i = 0; i < quest.StageCount; i++)
                {
                    string stageId = quest.stages[i].id;
                    Assert.NotNull(stageId);
                    Assert.StartsWith("stage_", stageId);
                    Assert.Matches("^stage_[0-9]+$", stageId);
                }
            }
        }

        [Fact]
        public void AllStagesHaveNonEmptyText()
        {
            var catalog = LoadCatalog();

            foreach (var quest in catalog.Quests)
            {
                foreach (var stage in quest.stages)
                {
                    Assert.NotNull(stage.text);
                    Assert.NotEmpty(stage.text.Trim());
                }
            }
        }

        [Fact]
        public void AllChoicesHaveValidIds()
        {
            var catalog = LoadCatalog();

            foreach (var quest in catalog.Quests)
            {
                foreach (var choice in quest.choices)
                {
                    Assert.NotNull(choice.id);
                    Assert.NotEmpty(choice.id);
                }
            }
        }

        [Fact]
        public void AllChoicesHaveNonEmptyText()
        {
            var catalog = LoadCatalog();

            foreach (var quest in catalog.Quests)
            {
                foreach (var choice in quest.choices)
                {
                    Assert.NotNull(choice.text);
                    Assert.NotEmpty(choice.text.Trim());
                }
            }
        }

        [Fact]
        public void PrereqQuestIdsReferenceValidQuests()
        {
            var catalog = LoadCatalog();
            var allQuestIds = new HashSet<string>();

            foreach (var quest in catalog.Quests)
            {
                allQuestIds.Add(quest.id);
            }

            foreach (var quest in catalog.Quests)
            {
                if (!string.IsNullOrEmpty(quest.prereq_quest_id))
                {
                    Assert.Contains(quest.prereq_quest_id, allQuestIds);
                }
            }
        }

        [Fact]
        public void MinDayValuesAreReasonable()
        {
            var catalog = LoadCatalog();

            foreach (var quest in catalog.Quests)
            {
                Assert.True(quest.min_day >= 1, $"Quest {quest.id} has invalid min_day: {quest.min_day}");
                Assert.True(quest.min_day <= 365, $"Quest {quest.id} has unrealistic min_day: {quest.min_day}");
            }
        }

        [Fact]
        public void LoadEquivalenceBetweenJsonAndSystem()
        {
            var catalog = LoadCatalog();
            var system = new HoldfastQuestSystem();
            system.BindCatalog(catalog.Quests);

            // Verify that the catalog can be bound and quests can be retrieved
            foreach (var quest in catalog.Quests)
            {
                var def = system.GetDef(quest.id);
                Assert.NotNull(def);
                Assert.Equal(quest.id, def.id);
                Assert.Equal(quest.display_name, def.display_name);
                Assert.Equal(quest.briefing, def.briefing);
                Assert.Equal(quest.StageCount, def.StageCount);
            }
        }

        [Fact]
        public void JsonFileIsInGitTracking()
        {
            string jsonPath = Path.Combine(DataDir(), HoldfastCatalogLoader.QuestsFile);
            Assert.True(File.Exists(jsonPath), "holdfast_quests.json must exist in StreamingAssets/Data");

            // Verify the file is tracked by git (not ignored)
            string projectRoot = Path.GetFullPath(Path.Combine(DataDir(), "..", ".."));
            string gitIgnorePath = Path.Combine(projectRoot, ".gitignore");

            if (File.Exists(gitIgnorePath))
            {
                string gitIgnore = File.ReadAllText(gitIgnorePath);
                string relativePath = Path.Combine("Assets", "StreamingAssets", "Data", "holdfast_quests.json")
                    .Replace("\\", "/");

                // Check if path is explicitly ignored
                string[] lines = gitIgnore.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                bool isIgnored = false;
                foreach (string line in lines)
                {
                    string trimmed = line.Trim();
                    if (trimmed.StartsWith(relativePath) ||
                        trimmed.EndsWith("*.json") && !trimmed.StartsWith("!"))
                    {
                        isIgnored = true;
                        break;
                    }
                }

                // If the file pattern is ignored, verify it's whitelisted
                if (isIgnored)
                {
                    bool isWhitelisted = false;
                    foreach (string line in lines)
                    {
                        string trimmed = line.Trim();
                        if (trimmed.StartsWith("!") && trimmed.Contains("holdfast_quests.json"))
                        {
                            isWhitelisted = true;
                            break;
                        }
                    }
                    Assert.True(isWhitelisted, "holdfast_quests.json should not be ignored by git");
                }
            }
        }
    }
}
