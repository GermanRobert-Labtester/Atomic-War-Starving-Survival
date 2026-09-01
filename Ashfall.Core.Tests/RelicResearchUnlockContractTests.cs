using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class RelicResearchUnlockContractTests
    {
        [Fact]
        public void AllRelicRecipes_NonEmptyResearchUnlocks_ResolveStaticallyInResearchCatalog()
        {
            var research = new ResearchSystem();
            research.RegisterDefaults();

            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);

            string relicPath = Path.Combine(dataDir, "relic_recipes.json");
            Assert.True(File.Exists(relicPath), $"relic_recipes.json not found at {relicPath}");

            string json = File.ReadAllText(relicPath);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var recipesArray = root.TryGetProperty("recipes", out var r) ? r : root;

            int validatedUnlocks = 0;
            foreach (var recipe in recipesArray.EnumerateArray())
            {
                if (recipe.TryGetProperty("research_unlock_id", out var unlockProp))
                {
                    string unlockId = unlockProp.GetString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(unlockId))
                    {
                        var def = research.GetKnowledge(unlockId);
                        Assert.NotNull(def);
                        Assert.Equal(unlockId, def.id);
                        Assert.False(string.IsNullOrWhiteSpace(def.displayName));
                        Assert.False(string.IsNullOrWhiteSpace(def.category));
                        validatedUnlocks++;
                    }
                }
            }

            Assert.Equal(16, validatedUnlocks);
        }

        [Fact]
        public void RelicResearchUnlock_SaveLoad_RoundTripsState()
        {
            var research1 = new ResearchSystem();
            research1.RegisterDefaults();

            string targetId = "knowledge_micro_dosimeter_blueprint";
            research1.UnlockManual(targetId);
            research1.CompleteResearch(targetId);

            var saved = research1.CaptureState();
            Assert.Contains(targetId, saved.unlockedIds);
            Assert.Contains(targetId, saved.completedIds);

            var research2 = new ResearchSystem();
            research2.RegisterDefaults();
            research2.RestoreState(saved);

            var restoredDef = research2.GetKnowledge(targetId);
            Assert.NotNull(restoredDef);
            Assert.True(restoredDef.isUnlocked);
            Assert.True(restoredDef.isCompleted);
        }
    }
}
