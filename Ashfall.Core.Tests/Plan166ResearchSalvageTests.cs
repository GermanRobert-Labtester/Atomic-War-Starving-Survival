using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plan166ResearchSalvageTests
    {
        private sealed class FixedRng : ISeededRng
        {
            private readonly Queue<double> _values;
            public FixedRng(params double[] values) => _values = new Queue<double>(values);
            public int Seed => 166;
            public int Next(int minInclusive, int maxExclusive) => minInclusive;
            public float NextFloat() => (float)NextDouble();
            public double NextDouble() => _values.Count > 0 ? _values.Dequeue() : 0.0;
        }

        private static WorkshopReverseEngineeringSystem Create(
            out InventoryContainer inventory,
            out ResearchSystem research,
            ISeededRng rng)
        {
            inventory = new InventoryContainer();
            research = new ResearchSystem();
            var crafting = new CraftingSystem(inventory);
            var workshop = new WorkshopReverseEngineeringSystem(inventory, research, crafting);
            workshop.BindTechSalvageRng(rng);
            workshop.LoadTechSalvageCatalog(new[]
            {
                new PreWarTechDef
                {
                    Id = "tech_test_device",
                    SourceItemId = "item_test_device",
                    Complexity = 1,
                    BaseResearchPoints = 5,
                    BlueprintRequiredPoints = 10,
                    BaseSuccessChance = 0.70f,
                    CatastrophicFailureChance = 0f,
                    PossibleBlueprintIds = new List<string> { "knowledge_test_blueprint" },
                    BaseScrapYields = new List<TechSalvageYieldDef>
                    {
                        new TechSalvageYieldDef { ItemId = "scrap_metal", Amount = 2 }
                    }
                }
            });
            return workshop;
        }

        [Fact]
        public void SuccessfulRecovery_UsesCanonicalResearchWalletAndBlueprintProgress()
        {
            var workshop = Create(out var inventory, out var research, new FixedRng(0.0, 0.0, 0.0));
            inventory.AddById("item_test_device", 1);

            Assert.True(workshop.StartTechDismantle("item_test_device", "survivor_1", day: 12).IsSuccess);
            Assert.Equal(0, inventory.CountById("item_test_device"));
            var result = workshop.TickProgress(100f);

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(5, research.State.researchPointsAvailable);
            var progress = research.GetBlueprintProgress("knowledge_test_blueprint");
            Assert.NotNull(progress);
            Assert.Equal(5, progress!.progressPoints);
            Assert.Equal(2, inventory.CountById("scrap_metal"));
            Assert.Contains("tech_test_device", progress.sourceTechIds);
        }

        [Fact]
        public void CatastrophicFailure_IsSeededAndConsumesSourceExactlyOnce()
        {
            var workshop = Create(out var inventory, out _, new FixedRng(0.99, 0.0, 0.0));
            inventory.AddById("item_test_device", 1);
            workshop.LoadTechSalvageCatalog(new[]
            {
                new PreWarTechDef
                {
                    Id = "tech_test_device",
                    SourceItemId = "item_test_device",
                    Complexity = 1,
                    BaseResearchPoints = 5,
                    BlueprintRequiredPoints = 10,
                    BaseSuccessChance = 0.05f,
                    CatastrophicFailureChance = 1f,
                    PossibleBlueprintIds = new List<string> { "knowledge_test_blueprint" }
                }
            });

            Assert.True(workshop.StartTechDismantle("item_test_device", "survivor_1").IsSuccess);
            var result = workshop.TickProgress(100f);

            Assert.Equal(ActionResult.StatusKind.Failed, result.Status);
            Assert.Equal("catastrophic_failure", result.FailureCode);
            Assert.Equal(0, inventory.CountById("item_test_device"));
            Assert.Equal(0, inventory.CountById("scrap_metal"));
            Assert.True(workshop.IsTechSalvageCompleted("tech_test_device"));
        }

        [Fact]
        public void ResearchFacilityQualityAndSkillRaisePreviewChance()
        {
            var workshop = Create(out var inventory, out _, new FixedRng());
            inventory.AddById("item_test_device", 1);
            workshop.BindSkillEvaluator(_ => 1f);
            var basePreview = workshop.PreviewTechDismantle("item_test_device", "survivor_1");

            workshop.BindSkillEvaluator(_ => 1.5f);
            var improvedPreview = workshop.PreviewTechDismantle(
                "item_test_device",
                "survivor_1",
                new ResearchFacilityContext { EquipmentQuality01 = 1f });

            Assert.True(basePreview.isAvailable);
            Assert.True(improvedPreview.isAvailable);
            Assert.True(improvedPreview.successChance > basePreview.successChance,
                $"base={basePreview.successChance:R}, improved={improvedPreview.successChance:R}, " +
                $"baseFacility={basePreview.failureCode}, improvedFacility={improvedPreview.failureCode}");
            Assert.True(improvedPreview.catastrophicFailureChance <= basePreview.catastrophicFailureChance);
        }

        [Fact]
        public void ResearchFacilityQualityIsPersistedIntoOutcomeResolution()
        {
            var workshop = Create(out var inventory, out _, new FixedRng(0.8, 0.0, 0.0));
            inventory.AddById("item_test_device", 1);

            var facility = new ResearchFacilityContext { EquipmentQuality01 = 1f };
            Assert.True(workshop.StartTechDismantle("item_test_device", "survivor_1", facility: facility).IsSuccess);
            var saved = workshop.CaptureState();
            Assert.Equal(1f, saved.techEquipmentQuality01);

            workshop.RestoreState(saved);
            var result = workshop.TickProgress(100f);

            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(5, result.Deltas["research_points"]);
        }

        [Fact]
        public void TechSalvageCatalogLoadsAndValidatesAuthoritativeData()
        {
            string root = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            string dataDir = System.IO.Path.Combine(root, "Assets", "StreamingAssets", "Data");
            var catalog = TechSalvageCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(4, catalog.Count);
            Assert.True(TechSalvageCatalogLoader.Validate(catalog, out var error), error);
        }

        [Fact]
        public void ResearchPointsAndBlueprintProgressSurviveRoundTripWithoutAliasing()
        {
            var research = new ResearchSystem();
            Assert.True(research.TryAddResearchPoints(7, "test_source"));
            Assert.True(research.TryAddBlueprintProgress("knowledge_test_blueprint", 3, 10, 9, "tech_test_device"));
            var saved = research.CaptureState();

            Assert.True(research.TryAddResearchPoints(2, "later"));
            Assert.Equal(7, saved.researchPointsAvailable);

            var restored = new ResearchSystem();
            restored.RestoreState(saved);
            Assert.Equal(7, restored.State.researchPointsAvailable);
            Assert.Equal(7, restored.State.researchPointsLifetimeEarned);
            Assert.Equal(3, restored.GetBlueprintProgress("knowledge_test_blueprint")!.progressPoints);
        }
    }
}
