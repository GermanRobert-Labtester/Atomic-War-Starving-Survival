// SPDX-License-Identifier: MIT
// Plan 171 — strict dynamic quest template loader, census, and schema-gate tests.

using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Quests;
using Xunit;

namespace Ashfall.Core.Tests.Quests
{
    public sealed class DynamicQuestTemplateCatalogLoaderTests
    {
        private static string RepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found");
        }

        private static string AuthoredJson() =>
            File.ReadAllText(Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data", "dynamic_quest_templates.json"));

        [Fact]
        public void Authored_Catalog_Loads_And_Binds()
        {
            var templates = DynamicQuestTemplateCatalogLoader.LoadFromJson(AuthoredJson());
            Assert.True(templates.Count >= 7);

            var generator = new DynamicQuestGenerator();
            generator.BindAuthoredTemplates(templates);
            Assert.Equal(templates.Count, generator.GetCensus().TemplateCount);
        }

        [Theory]
        [InlineData("{\"schema_version\":1,\"templates\":[{\"template_id\":\"t1\",\"type\":\"FetchResource\",\"title\":\"A\",\"description\":\"d\",\"base_difficulty\":1,\"required_quantity\":1},{\"template_id\":\"t1\",\"type\":\"FetchResource\",\"title\":\"B\",\"description\":\"d\",\"base_difficulty\":1,\"required_quantity\":1}]}")]
        [InlineData("{\"schema_version\":1,\"templates\":[{\"template_id\":\"t1\",\"type\":\"NotAType\",\"title\":\"A\",\"description\":\"d\",\"base_difficulty\":1,\"required_quantity\":1}]}")]
        [InlineData("{\"schema_version\":1,\"templates\":[{\"template_id\":\"t1\",\"type\":\"FetchResource\",\"title\":\"\",\"description\":\"d\",\"base_difficulty\":1,\"required_quantity\":1}]}")]
        [InlineData("{\"schema_version\":1,\"templates\":[{\"template_id\":\"t1\",\"type\":\"FetchResource\",\"title\":\"A\",\"description\":\"d\",\"base_difficulty\":6,\"required_quantity\":1}]}")]
        [InlineData("{\"schema_version\":1,\"templates\":[{\"template_id\":\"t1\",\"type\":\"FetchResource\",\"title\":\"A\",\"description\":\"d\",\"base_difficulty\":1,\"required_quantity\":1,\"reward_item_id\":\"x\",\"reward_item_count\":0}]}")]
        [InlineData("{\"schema_version\":2,\"templates\":[{\"template_id\":\"t1\",\"type\":\"FetchResource\",\"title\":\"A\",\"description\":\"d\",\"base_difficulty\":1,\"required_quantity\":1}]}")]
        public void Malformed_Catalog_Is_Rejected(string json)
        {
            Assert.Throws<InvalidOperationException>(() => DynamicQuestTemplateCatalogLoader.LoadFromJson(json));
        }

        [Fact]
        public void Generation_Lifecycle_And_Census_Behave()
        {
            var templates = DynamicQuestTemplateCatalogLoader.LoadFromJson(AuthoredJson());
            var generator = new DynamicQuestGenerator();
            generator.BindAuthoredTemplates(templates);

            var generated = generator.GenerateQuests(currentDay: 1);
            Assert.InRange(generated.Count, 1, 3);
            Assert.Empty(generator.GenerateQuests(currentDay: 2)); // cooldown

            string id = generated[0].QuestId;
            Assert.True(generator.AcceptQuest(id, "survivor_x"));
            Assert.True(generator.ProgressQuest(id, generated[0].RequiredQuantity));
            Assert.True(generator.CompleteQuest(id, 3));

            var census = generator.GetCensus();
            Assert.Equal(templates.Count, census.TemplateCount);
            Assert.Equal(1, census.CompletedQuests);
        }

        [Fact]
        public void Rewards_Are_Carried_And_RestoreState_Rejects_Newer_Schema()
        {
            var templates = DynamicQuestTemplateCatalogLoader.LoadFromJson(AuthoredJson());
            var generator = new DynamicQuestGenerator();
            generator.BindAuthoredTemplates(templates);
            generator.GenerateQuests(currentDay: 1);

            var state = generator.CaptureState();
            Assert.All(state.Quests, q => Assert.NotNull(q.RewardItemId));

            var restored = new DynamicQuestGenerator();
            restored.BindAuthoredTemplates(templates);
            restored.RestoreState(state);
            Assert.Equal(state.Quests.Count, restored.CaptureState().Quests.Count);

            var newer = generator.CaptureState();
            newer.SchemaVersion = 99;
            Assert.Throws<InvalidOperationException>(() => restored.RestoreState(newer));

            var legacy = generator.CaptureState();
            legacy.SchemaVersion = 0;
            restored.RestoreState(legacy); // legacy payload accepted and normalized
        }
    }
}
