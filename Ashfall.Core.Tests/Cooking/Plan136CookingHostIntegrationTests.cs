#nullable enable
// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 136 — Wildlife Trapping → Food Pipeline & Cooking System host-integration gate.
//
// Pins the production wiring contract:
//   * authored recipes_cooking.json loads cleanly with strict validation (>= 15 recipes),
//   * state capture/restore round-trips with schema version 1,
//   * save section registry registers "cooking" -> "cooking_save.json",
//   * event vocabulary classifies "cooking_ticked" as Heartbeat,
//   * CLI registry defines CookingSelfTest and --cooking-selftest descriptor,
//   * host files (CookingHostSession, HostCli.Cooking, Main.Cooking) exist and wire into lifecycle,
//   * trapping catch (raw_meat) can be consumed by CookingSystem to yield cooked food.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Cooking;
using Ashfall.Core.Inventory;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Cooking
{
    public sealed class Plan136CookingHostIntegrationTests
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
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string DataDir() => Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        [Fact]
        public void AuthoredCatalogs_LoadAndPassStrictValidation()
        {
            var result = CookingRecipeCatalogLoader.Load(DataDir(), new FileSystemIO());

            Assert.True(result.Success, string.Join("; ", result.Errors));
            Assert.True(result.Recipes.Count >= 15, $"Expected at least 15 recipes, found {result.Recipes.Count}");

            var ids = result.Recipes.Select(r => r.id).ToList();
            Assert.Equal(ids.Count, ids.Distinct(StringComparer.OrdinalIgnoreCase).Count());

            var roast = result.Recipes.FirstOrDefault(r => r.id == "recipe_roasted_meat");
            Assert.NotNull(roast);
            Assert.Equal("cooked_meat", roast!.outputItemId);

            var broth = result.Recipes.FirstOrDefault(r => r.id == "recipe_boiled_meat_broth");
            Assert.NotNull(broth);
            Assert.True(broth!.radiationRemoval >= 0.8f);
        }

        [Fact]
        public void TrappingToCookingPipeline_ConsumesCatchAndDeliversMeal()
        {
            var system = new CookingSystem(null, new SeededRng(42));
            var loadResult = CookingRecipeCatalogLoader.Load(DataDir(), new FileSystemIO());
            system.BindValidatedRecipes(loadResult.Recipes);

            var inventory = new Ashfall.Core.Inventory.Inventory();
            var source = new InventoryCookingSource(inventory);

            // Simulate raw meat from trapping harvest
            inventory.AddById("raw_meat", 5);
            Assert.Equal(5, inventory.CountById("raw_meat"));

            // Start cooking roasted meat
            var startRes = system.StartCooking("recipe_roasted_meat", "cook_survivor", "improvised_stove", source, currentMinute: 0f);
            Assert.True(startRes.IsSuccess);
            Assert.Equal(4, inventory.CountById("raw_meat")); // 1 consumed
            Assert.Single(system.State.activeOperations);

            // Progress cooking to completion
            float requiredMinutes = system.State.activeOperations[0].totalMinutesRequired;
            int completed = system.ProgressCooking(requiredMinutes + 1f, source);

            Assert.Equal(1, completed);
            Assert.Empty(system.State.activeOperations);
            Assert.Single(system.State.completedOperations);
            Assert.True(inventory.CountById("cooked_meat") >= 1);
            Assert.Equal(1, system.GetCensus().TotalMealsPrepared);
            Assert.True(system.GetCensus().CookingSkillLevel > 0f);
        }

        [Fact]
        public void RadiationDecontamination_RemovesRadiationAccurately()
        {
            var system = new CookingSystem(null, new SeededRng(100));
            var loadResult = CookingRecipeCatalogLoader.Load(DataDir(), new FileSystemIO());
            system.BindValidatedRecipes(loadResult.Recipes);

            var inventory = new Ashfall.Core.Inventory.Inventory();
            var source = new InventoryCookingSource(inventory);

            inventory.AddById("raw_meat", 2);
            inventory.AddById("clean_water", 2);

            var startRes = system.StartCooking("recipe_boiled_meat_broth", "cook_master", "basic_boiler", source, currentMinute: 0f);
            Assert.True(startRes.IsSuccess);

            float totalMins = system.State.activeOperations[0].totalMinutesRequired;
            system.ProgressCooking(totalMins + 1f, source);

            var completedOp = system.State.completedOperations.First();
            Assert.True(completedOp.radiationRemainingFraction <= 0.25f);
            Assert.True(system.GetCensus().TotalFoodDecontaminated > 0f);
        }

        [Fact]
        public void StateCaptureAndRestore_RoundTripsDeterministically()
        {
            var system = new CookingSystem();
            system.RegisterRecipe(new CookingRecipe
            {
                id = "recipe_test",
                displayName = "Test Stew",
                outputItemId = "test_stew",
                outputQuantity = 2,
                cookTimeMinutes = 25f
            });

            system.StartCooking("recipe_test", "cook_1", "improvised_stove", null, 0f);
            var captured = system.CaptureState();
            Assert.Equal(1, captured.schema_version);
            Assert.Single(captured.activeOperations);

            string jsonA = JsonSerializer.Serialize(captured);

            var restored = new CookingSystem();
            restored.RegisterRecipe(new CookingRecipe
            {
                id = "recipe_test",
                displayName = "Test Stew",
                outputItemId = "test_stew",
                outputQuantity = 2,
                cookTimeMinutes = 25f
            });
            restored.RestoreState(captured);

            var capturedAgain = restored.CaptureState();
            string jsonB = JsonSerializer.Serialize(capturedAgain);

            Assert.Equal(jsonA, jsonB);
            Assert.Single(restored.State.activeOperations);
            Assert.Equal("recipe_test", restored.State.activeOperations[0].recipeId);
        }

        [Fact]
        public void SaveSectionRegistry_RegistersCookingSection()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("cooking", out var meta));
            Assert.NotNull(meta);
            Assert.Equal("cooking_save.json", SaveSectionRegistry.FileNameFor("cooking"));
            Assert.Equal("SaveCooking", meta!.SaveMethod);
            Assert.Equal("SetupCooking", meta.SetupMethod);
            Assert.Equal("cooking", meta.Owner);

            Assert.True(SaveSectionRegistry.SectionFileNames.TryGetValue("cooking", out string? file));
            Assert.Equal("cooking_save.json", file);
        }

        [Fact]
        public void DayEventVocabulary_ClassifiesCookingHeartbeat()
        {
            var kind = DayEventVocabulary.GetSemanticKind("cooking_ticked");
            Assert.Equal(SemanticKind.Heartbeat, kind);
            Assert.True(DayEventVocabulary.IsInternalHeartbeat("cooking_ticked"));

            string matrix = ReadRepoFile("docs", "campaign", "EVENT_SEMANTIC_PARITY_MATRIX.md");
            Assert.Contains("cooking_ticked", matrix);
        }

        [Fact]
        public void HostCliRegistry_RegistersCookingSelfTest()
        {
            var desc = HostCliRegistry.AllDescriptors.FirstOrDefault(d => d.Action == HostCliAction.CookingSelfTest);
            Assert.NotNull(desc);
            Assert.Equal("--cooking-selftest", desc!.PrimaryFlag);
            Assert.Contains("--cooking-test", desc.Aliases);
        }

        [Fact]
        public void HostWiring_SourceFilesExistAndDeclareSeams()
        {
            string hostSession = ReadRepoFile("src", "Host", "CookingHostSession.cs");
            Assert.Contains("class CookingHostSession", hostSession);
            Assert.Contains("class CookingSaveStore", hostSession);
            Assert.Contains("SaveStoreHub.Checksummed", hostSession);

            string hostCli = ReadRepoFile("src", "Host", "HostCli.Cooking.cs");
            Assert.Contains("RunCookingSelfTest", hostCli);

            string mainCooking = ReadRepoFile("src", "Main.Cooking.cs");
            Assert.Contains("EnsureCooking", mainCooking);
            Assert.Contains("SetupCooking", mainCooking);
            Assert.Contains("SaveCooking", mainCooking);
            Assert.Contains("TickCooking", mainCooking);

            string campaignOwners = ReadRepoFile("src", "Main.CampaignOwners.cs");
            Assert.Contains("CookingDayOwner", campaignOwners);
            Assert.Contains("\"cooking\"", campaignOwners);

            string saveOrchestrator = ReadRepoFile("src", "Main.SaveOrchestrator.cs");
            Assert.Contains("SetupCooking();", saveOrchestrator);
            Assert.Contains("SaveCooking();", saveOrchestrator);

            string mainApp = ReadRepoFile("src", "Main.Application.cs");
            Assert.Contains("HostCliAction.CookingSelfTest", mainApp);
            Assert.Contains("FlushCookingIfDirty", mainApp);
        }
    }
}
