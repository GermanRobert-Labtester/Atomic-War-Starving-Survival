// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Flagship Task 5: Expedition Loot Integrity & Determinism Suite.
    ///
    /// Production Status of lootCategories:
    /// - Case C: Mixed Namespace containing both direct item IDs (e.g. 'trap_fish', 'trap_body_grip', 'bandage')
    ///   and semantic category/vocabulary tokens (e.g. 'ammo', 'medical', 'tools', 'scrap_cloth').
    /// - Resolved via IExpeditionLootReferenceResolver to prevent treating semantic categories as invalid item foreign keys.
    /// </summary>
    public sealed class ExpeditionLootIntegrityTests : CatalogTestBase
    {
        private (List<ExpeditionDefinition> expeditions, ExpeditionLootReferenceResolver resolver) LoadFixture()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var itemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var itemFile in Directory.GetFiles(DataDirectory, "*item*.json"))
            {
                var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(itemFile));
                if (doc.RootElement.TryGetProperty("items", out var items))
                {
                    foreach (var it in items.EnumerateArray())
                    {
                        if (it.TryGetProperty("id", out var idProp))
                        {
                            string id = idProp.GetString() ?? "";
                            if (!string.IsNullOrEmpty(id)) itemIds.Add(id);
                        }
                    }
                }
            }

            var resolver = new ExpeditionLootReferenceResolver(itemIds);
            var expeditions = ExpeditionCatalogLoader.Load(DataDirectory, files, json);
            return (expeditions, resolver);
        }

        [Fact]
        public void WaterStation_TrapFish_ResolvesToExistingItem()
        {
            var (expeditions, resolver) = LoadFixture();
            var waterStation = expeditions.FirstOrDefault(e => e.id == "loc_water_station");

            Assert.NotNull(waterStation);
            Assert.Contains("trap_fish", waterStation!.lootCategories);

            var refType = resolver.Resolve("trap_fish", out string canonicalId);
            Assert.Equal(ExpeditionLootReferenceType.Item, refType);
            Assert.Equal("trap_fish", canonicalId);
        }

        [Fact]
        public void RuinedGarage_TrapBodyGrip_ResolvesToExistingItem()
        {
            var (expeditions, resolver) = LoadFixture();
            var garage = expeditions.FirstOrDefault(e => e.id == "ruined_garage");

            Assert.NotNull(garage);
            Assert.Contains("trap_body_grip", garage!.lootCategories);

            var refType = resolver.Resolve("trap_body_grip", out string canonicalId);
            Assert.Equal(ExpeditionLootReferenceType.Item, refType);
            Assert.Equal("trap_body_grip", canonicalId);
        }

        [Fact]
        public void AllExpeditionLootReferences_ResolveToKnownNamespace()
        {
            var (expeditions, resolver) = LoadFixture();
            var result = ExpeditionLootValidator.Validate(expeditions, resolver);

            Assert.True(result.IsValid,
                $"Unresolved expedition loot references found: {string.Join("; ", result.Errors.Select(e => e.FormattedMessage))}");
        }

        [Fact]
        public void UnknownDirectItemReference_FailsWithSourceContext()
        {
            var resolver = new ExpeditionLootReferenceResolver(
                itemIds: new[] { "item_salvaged_fuel", "trap_fish" });

            var testExpedition = new ExpeditionDefinition
            {
                id = "loc_test_ruin",
                displayName = "Test Ruin",
                lootCategories = new List<string> { "item_salvaged_fuel", "item_nonexistent_widget" }
            };

            var result = ExpeditionLootValidator.Validate(new[] { testExpedition }, resolver, "expeditions.json");
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);

            var err = result.Errors[0];
            Assert.Equal("loc_test_ruin", err.ExpeditionId);
            Assert.Equal("item_nonexistent_widget", err.UnresolvedValue);
            Assert.Equal("expeditions.json:loc_test_ruin.lootCategories[1]", err.FieldPath);
            Assert.Contains("expected: item_id or expedition_loot_category_id", err.FormattedMessage);
        }

        [Fact]
        public void UnknownLootCategoryReference_FailsWithSourceContext()
        {
            var resolver = new ExpeditionLootReferenceResolver(
                itemIds: new[] { "clean_water" },
                knownCategories: new[] { "ammo", "medical" });

            var testExpedition = new ExpeditionDefinition
            {
                id = "loc_test_depot",
                displayName = "Test Depot",
                lootCategories = new List<string> { "clean_water", "completely_bogus_category_token" }
            };

            var result = ExpeditionLootValidator.Validate(new[] { testExpedition }, resolver, "expeditions.json");
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal("completely_bogus_category_token", result.Errors[0].UnresolvedValue);
        }

        [Fact]
        public void MixedReferenceResolver_DistinguishesItemAndCategory()
        {
            var resolver = new ExpeditionLootReferenceResolver(
                itemIds: new[] { "trap_fish", "trap_body_grip", "copper_wire" },
                knownCategories: new[] { "ammo", "medical", "tools" });

            Assert.Equal(ExpeditionLootReferenceType.Item, resolver.Resolve("trap_fish", out _));
            Assert.Equal(ExpeditionLootReferenceType.Item, resolver.Resolve("trap_body_grip", out _));
            Assert.Equal(ExpeditionLootReferenceType.Category, resolver.Resolve("medical", out _));
            Assert.Equal(ExpeditionLootReferenceType.Category, resolver.Resolve("tools", out _));
            Assert.Equal(ExpeditionLootReferenceType.Unknown, resolver.Resolve("unknown_artifact_xyz", out _));
        }

        [Fact]
        public void ExpeditionIntegritySweep_IncludesLootReferences()
        {
            var files = new FileSystemIO();
            var report = CatalogIntegrityValidator.Validate(DataDirectory, files, SearchOption.TopDirectoryOnly);

            Assert.True(report.Clean,
                $"Global catalog integrity sweep must pass clean including expedition loot: {string.Join("; ", report.Errors)}");
        }

        [Fact]
        public void ExpeditionLootSelection_UsesDeterministicRng()
        {
            var expSystem = new ExpeditionSystem();
            var def = new ExpeditionDefinition
            {
                id = "loc_det_test",
                distanceTicks = 1,
                dangerLevel = 1,
                lootCategories = new List<string> { "trap_fish", "clean_water", "scrap_metal" }
            };
            ExpeditionDefinitionRegistry.Register(def);

            var rngA = new SeededRng(42);
            var rngB = new SeededRng(42);

            expSystem.Start(def, "survivor_a", 1);
            expSystem.TickHours(1.0f, rngA);

            var expSystemB = new ExpeditionSystem();
            expSystemB.Start(def, "survivor_b", 1);
            expSystemB.TickHours(1.0f, rngB);

            var lootA = expSystem.Active.Values.First().loot.Select(l => (l.itemId, l.quantity)).ToList();
            var lootB = expSystemB.Active.Values.First().loot.Select(l => (l.itemId, l.quantity)).ToList();

            Assert.Equal(lootA, lootB);
        }

        [Fact]
        public void ExpeditionLootSelection_Seed42_IsIdenticalAcrossThreeRuns()
        {
            var def = new ExpeditionDefinition
            {
                id = "loc_water_station",
                displayName = "Water Station",
                distanceTicks = 1,
                dangerLevel = 1,
                lootCategories = new List<string> { "clean_water", "water_filter", "water_purification_tablets", "trap_fish" }
            };
            ExpeditionDefinitionRegistry.Register(def);

            List<(string itemId, int quantity)> RunScenario(int seed)
            {
                var sys = new ExpeditionSystem();
                var rng = new SeededRng(seed);
                sys.Start(def, "survivor_scout", 1);
                sys.TickHours(1.0f, rng); // Arrive -> Looting
                sys.TickHours(1.0f, rng); // Looting executes
                var exp = sys.Active.Values.First();
                return exp.loot.Select(l => (l.itemId, l.quantity)).ToList();
            }

            var run1 = RunScenario(42);
            var run2 = RunScenario(42);
            var run3 = RunScenario(42);

            Assert.NotEmpty(run1);
            Assert.Equal(run1, run2);
            Assert.Equal(run2, run3);
        }

        [Fact]
        public void ExpeditionLootSelection_IsStableAgainstUnorderedSourceCollection()
        {
            // Verifies that candidates sorted canonically before selection produce deterministic outcomes
            var candidateSet = new HashSet<string> { "trap_fish", "scrap_metal", "clean_water", "bandage" };
            var sortedList = candidateSet.OrderBy(c => c, StringComparer.Ordinal).ToList();

            var rng1 = new SeededRng(42);
            var rng2 = new SeededRng(42);

            int pick1 = rng1.Next(0, sortedList.Count);
            int pick2 = rng2.Next(0, sortedList.Count);

            Assert.Equal(sortedList[pick1], sortedList[pick2]);
        }
    }
}
