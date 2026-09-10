// SPDX-License-Identifier: MIT
// Plan 47 / Task 13 — snake_case migration wave 1 tests.
//
// Contract per docs/data/SNAKE_CASE_MIGRATION.md:
//   canonical snake_case loads, legacy camelCase loads, dual spelling with
//   identical values loads, conflicting dual spellings fail loudly, IDs are
//   untouched, and both spellings drive identical system state (identical
//   SaveChecksum over captured state).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Crafting;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Data
{
    public sealed class SnakeCaseWave1Tests : CatalogTestBase
    {
        private static readonly SystemTextJsonSerializer Json = new();
        private static readonly FileSystemIO Files = new();

        // ── Normalizer unit contract ────────────────────────────────

        private static readonly Dictionary<string, string> Aliases = new()
        {
            ["displayName"] = "display_name",
            ["inputItems"] = "input_items",
        };

        [Fact]
        public void Normalizer_LegacyKeys_AreRenamedSpellingOnly()
        {
            string json = "{\"id\":\"x\",\"displayName\":\"Fuel\",\"inputItems\":{\"item_a\":2}}";
            string outJson = CatalogKeyNormalizer.Normalize(json, Aliases, "test.json");
            Assert.Contains("\"display_name\":\"Fuel\"", outJson);
            Assert.Contains("\"input_items\":", outJson);
            Assert.DoesNotContain("displayName", outJson);
            Assert.DoesNotContain("inputItems", outJson);
            // Values — including ID strings — are untouched.
            Assert.Contains("\"item_a\":2", outJson);
        }

        [Fact]
        public void Normalizer_CanonicalKeys_PassThroughUnchanged()
        {
            string json = "{\"id\":\"x\",\"display_name\":\"Fuel\"}";
            Assert.Equal(json, CatalogKeyNormalizer.Normalize(json, Aliases, "test.json"));
        }

        [Fact]
        public void Normalizer_DualKeys_SameValue_LegacyDropped()
        {
            string json = "{\"displayName\":\"Fuel\",\"display_name\":\"Fuel\"}";
            string outJson = CatalogKeyNormalizer.Normalize(json, Aliases, "test.json");
            Assert.Contains("\"display_name\":\"Fuel\"", outJson);
            Assert.DoesNotContain("displayName", outJson);
        }

        [Fact]
        public void Normalizer_DualKeys_ConflictingValues_FailLoudly()
        {
            string json = "{\"displayName\":\"Fuel\",\"display_name\":\"Solvent\"}";
            var ex = Assert.Throws<CatalogKeyConflictException>(() =>
                CatalogKeyNormalizer.Normalize(json, Aliases, "test.json"));
            Assert.Contains("displayName", ex.Message, StringComparison.Ordinal);
            Assert.Contains("display_name", ex.Message, StringComparison.Ordinal);
        }

        [Fact]
        public void Normalizer_NestedArraysAndObjects_AreNormalized()
        {
            string json = "{\"items\":[{\"displayName\":\"A\"},{\"inner\":{\"displayName\":\"B\"}}]}";
            string outJson = CatalogKeyNormalizer.Normalize(json, Aliases, "test.json");
            Assert.DoesNotContain("displayName", outJson);
            Assert.Contains("\"display_name\":\"A\"", outJson);
            Assert.Contains("\"display_name\":\"B\"", outJson);
        }

        [Fact]
        public void Normalizer_MalformedJson_IsPassedThroughForTypedLoaderDiagnostics()
        {
            string bad = "{\"displayName\": ";
            Assert.Equal(bad, CatalogKeyNormalizer.Normalize(bad, Aliases, "test.json"));
        }

        // ── On-disk canonical load (integration) ────────────────────

        [Fact]
        public void OnDisk_Wave1Catalogs_LoadCanonicalSnakeCase()
        {
            string dataDir = DataDirectory;

            var chemical = ChemicalSynthesisCatalogLoader.Load(dataDir, Files, Json);
            Assert.NotNull(chemical);
            Assert.True(chemical!.Processes.Count >= 16,
                $"chemical pair should load >= 16 processes, got {chemical.Processes.Count}");
            Assert.All(chemical.Processes.Values, p =>
            {
                Assert.False(string.IsNullOrWhiteSpace(p.displayName), $"process '{p.id}' lost display_name");
                Assert.True(p.inputItems.Count > 0, $"process '{p.id}' lost input_items");
            });

            var crops = HydroponicCropCatalogLoader.Load(dataDir, Files, Json);
            Assert.NotNull(crops);
            Assert.Equal(10, crops!.Crops.Count);
            Assert.All(crops.Crops.Values, c =>
            {
                Assert.False(string.IsNullOrWhiteSpace(c.baseYieldItemId), $"crop '{c.id}' lost base_yield_item_id");
                Assert.True(c.growthTicks > 0, $"crop '{c.id}' lost growth_ticks");
            });

            var cores = NuclearCoreCatalogLoader.Load(dataDir, Files, Json);
            Assert.NotNull(cores);
            Assert.Equal(6, cores!.Profiles.Count);
            Assert.All(cores.Profiles.Values, p =>
            {
                Assert.False(string.IsNullOrWhiteSpace(p.powerClass), $"core '{p.id}' lost power_class");
            });

            var modules = ArmoredCrawlerModuleCatalogLoader.Load(dataDir, Files, Json);
            Assert.NotNull(modules);
            Assert.Equal(10, modules!.Modules.Count);
            Assert.All(modules.Modules.Values, m =>
            {
                Assert.False(string.IsNullOrWhiteSpace(m.slotType), $"module '{m.id}' lost slot_type");
            });
        }

        [Fact]
        public void OnDisk_Wave1Ids_AreUntouched()
        {
            // Canonical ID pins: spelling migration must never rename IDs.
            string dataDir = DataDirectory;
            var crops = HydroponicCropCatalogLoader.Load(dataDir, Files, Json)!;
            Assert.Contains("crop_rad_scrubbing_kelp", crops.Crops.Keys);

            var modules = ArmoredCrawlerModuleCatalogLoader.Load(dataDir, Files, Json)!;
            Assert.Contains(modules.Modules.Keys, k => k.StartsWith("module_crawler_", StringComparison.Ordinal));

            var chemical = ChemicalSynthesisCatalogLoader.Load(dataDir, Files, Json)!;
            Assert.Contains(chemical.Processes.Keys, k => k.StartsWith("synth_", StringComparison.Ordinal));
        }

        // ── Dual-read: legacy fixtures drive identical DTOs ─────────

        [Fact]
        public void DualRead_LegacyCamelCase_LoadsIdenticallyToCanonical()
        {
            // Chemical: legacy wire keys.
            const string legacyJson = """
                {
                  "schema_version": 1,
                  "processes": [
                    {
                      "id": "chem_test_reaction",
                      "displayName": "Test Reaction",
                      "requiredApparatusTier": 2,
                      "inputItems": { "item_solvent": 3 },
                      "outputItems": { "item_product": 1 },
                      "processingTicks": 4,
                      "heatBand": "High",
                      "volatilityRating": 0.3,
                      "scrubberDemand": 1.5,
                      "equipmentWear": 2.5,
                      "skillRequirement": 12.0
                    }
                  ]
                }
                """;
            const string canonicalJson = """
                {
                  "schema_version": 1,
                  "processes": [
                    {
                      "id": "chem_test_reaction",
                      "display_name": "Test Reaction",
                      "required_apparatus_tier": 2,
                      "input_items": { "item_solvent": 3 },
                      "output_items": { "item_product": 1 },
                      "processing_ticks": 4,
                      "heat_band": "High",
                      "volatility_rating": 0.3,
                      "scrubber_demand": 1.5,
                      "equipment_wear": 2.5,
                      "skill_requirement": 12.0
                    }
                  ]
                }
                """;

            var fromLegacy = System.Text.Json.JsonSerializer.Deserialize<ChemicalSynthesisCatalogDto>(
                CatalogKeyNormalizer.Normalize(legacyJson, ChemicalSynthesisCatalogLoader.KeyAliases, "legacy.json"),
                Ashfall.Core.SystemTextJsonSerializer.Options);
            var fromCanonical = System.Text.Json.JsonSerializer.Deserialize<ChemicalSynthesisCatalogDto>(
                canonicalJson, Ashfall.Core.SystemTextJsonSerializer.Options);

            Assert.NotNull(fromLegacy);
            Assert.NotNull(fromCanonical);
            var a = fromLegacy!.processes[0];
            var b = fromCanonical!.processes[0];
            Assert.Equal(b.id, a.id);
            Assert.Equal(b.displayName, a.displayName);
            Assert.Equal(b.requiredApparatusTier, a.requiredApparatusTier);
            Assert.Equal(b.processingTicks, a.processingTicks);
            Assert.Equal(b.heatBand, a.heatBand);
            Assert.Equal(b.volatilityRating, a.volatilityRating);
            Assert.Equal("item_solvent", a.inputItems.Single().Key);
            Assert.Equal(3, a.inputItems.Single().Value);
            // Both spellings drive byte-identical system state.
            Assert.Equal(
                SaveChecksum.Compute(fromCanonical),
                SaveChecksum.Compute(fromLegacy));
        }

        [Fact]
        public void DualRead_ConflictingDualSpellings_FailLoudly()
        {
            const string conflictJson = """
                {
                  "schema_version": 1,
                  "processes": [
                    {
                      "id": "chem_test_reaction",
                      "displayName": "Wrong Name",
                      "display_name": "Right Name",
                      "inputItems": { "item_solvent": 3 },
                      "outputItems": { "item_product": 1 }
                    }
                  ]
                }
                """;
            Assert.Throws<CatalogKeyConflictException>(() =>
                CatalogKeyNormalizer.Normalize(conflictJson, ChemicalSynthesisCatalogLoader.KeyAliases, "conflict.json"));
        }

        [Fact]
        public void DualRead_DutyRosterSeasons_LegacyAndCanonicalMatch()
        {
            const string legacy = """
                {
                  "schema_version": 1,
                  "items": [
                    { "id": "season_test", "windowMinDays": 5, "windowMaxDays": 9,
                      "encounterWeight": 1.25, "steamTripChanceBoost": 0.05 }
                  ]
                }
                """;
            var loaded = CatalogLocator.LoadWrappedList<DutyRosterSeasonEntry>(
                CatalogKeyNormalizer.Normalize(legacy, DutyRosterCatalogLoader.SeasonKeyAliases, "seasons.json"),
                Ashfall.Core.SystemTextJsonSerializer.Options);
            var entry = Assert.Single(loaded);
            Assert.Equal("season_test", entry.id);
            Assert.Equal(5, entry.windowMinDays);
            Assert.Equal(9, entry.windowMaxDays);
            Assert.Equal(1.25f, entry.encounterWeight);
            Assert.Equal(0.05f, entry.steamTripChanceBoost);
        }
    }
}
