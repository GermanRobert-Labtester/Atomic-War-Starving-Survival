// SPDX-License-Identifier: MIT
// xUnit tests for Plan 186: ShelterComponentCatalogLoader
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Records
{
    public sealed class ShelterComponentCatalogLoaderTests
    {
        private static string GetStreamingAssetsDataPath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (Directory.Exists(path)) return path;

            path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data"));
            return path;
        }

        [Fact]
        public void Load_CanonicalCatalog_SucceedsWithExpectedComponents()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = ShelterComponentCatalogLoader.Load(dataDir);

            Assert.True(result.Success, $"Catalog load failed: {string.Join("; ", result.Errors)}");
            Assert.NotNull(result.Catalog);
            Assert.True(result.Catalog!.components.Count >= 10,
                $"Expected at least 10 components; found {result.Catalog.components.Count}");
        }

        [Fact]
        public void Load_CanonicalCatalog_AllFourComponentTypesPresent()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = ShelterComponentCatalogLoader.Load(dataDir);
            Assert.True(result.Success);

            var types = result.Catalog!.components.Select(c => c.component_type.ToLowerInvariant()).Distinct().ToHashSet();
            Assert.Contains("air", types);
            Assert.Contains("water", types);
            Assert.Contains("power", types);
            Assert.Contains("structural", types);
        }

        [Fact]
        public void Load_CanonicalCatalog_AllComponentsHaveValidThresholds()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = ShelterComponentCatalogLoader.Load(dataDir);
            Assert.True(result.Success);

            foreach (var c in result.Catalog!.components)
            {
                Assert.True(c.max_condition > 0, $"Component '{c.component_id}' has non-positive max_condition.");
                Assert.True(c.base_degradation_rate > 0, $"Component '{c.component_id}' has non-positive degradation_rate.");
                Assert.True(c.warning_threshold > c.failure_threshold,
                    $"Component '{c.component_id}': warning_threshold must exceed failure_threshold.");
                Assert.True(c.warning_threshold <= c.max_condition,
                    $"Component '{c.component_id}': warning_threshold must not exceed max_condition.");
            }
        }

        [Fact]
        public void Load_CanonicalCatalog_NoduplicateIds()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = ShelterComponentCatalogLoader.Load(dataDir);
            Assert.True(result.Success);

            var ids = result.Catalog!.components.Select(c => c.component_id).ToList();
            var uniqueIds = ids.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            Assert.Equal(uniqueIds.Count, ids.Count);
        }

        [Fact]
        public void LoadFromJson_InvalidJson_ReturnsErrorResult()
        {
            var result = ShelterComponentCatalogLoader.Load(Path.GetTempPath() + Guid.NewGuid().ToString());
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void LoadFromJson_ValidJson_BindsToSystemCorrectly()
        {
            string json = @"{
                ""schema_version"": 1,
                ""components"": [
                    {
                        ""component_id"": ""test_air"",
                        ""display_name"": ""Test Air Filter"",
                        ""component_type"": ""Air"",
                        ""max_condition"": 100,
                        ""base_degradation_rate"": 2.0,
                        ""warning_threshold"": 40,
                        ""failure_threshold"": 15,
                        ""repair_time_hours"": 3,
                        ""description"": ""Test""
                    }
                ]
            }";

            var catalog = ShelterComponentCatalogLoader.LoadFromJson(json);
            Assert.Equal(1, catalog.schema_version);
            Assert.Single(catalog.components);
            Assert.Equal("test_air", catalog.components[0].component_id);

            var system = new ShelterMaintenanceSystem();
            system.BindValidatedCatalog(catalog);
            Assert.Equal(1, system.TrackedComponentCount);

            var comp = system.GetComponent("test_air");
            Assert.NotNull(comp);
            Assert.True(comp!.IsOperational);
        }

        [Fact]
        public void ShelterMaintenanceSystem_DailyDegradation_ReducesCondition()
        {
            string json = @"{
                ""schema_version"": 1,
                ""components"": [
                    {
                        ""component_id"": ""test_structural"",
                        ""display_name"": ""Test Wall"",
                        ""component_type"": ""Structural"",
                        ""max_condition"": 100,
                        ""base_degradation_rate"": 1.5,
                        ""warning_threshold"": 40,
                        ""failure_threshold"": 10,
                        ""repair_time_hours"": 2,
                        ""description"": ""Test""
                    }
                ]
            }";

            var catalog = ShelterComponentCatalogLoader.LoadFromJson(json);
            var system = new ShelterMaintenanceSystem();
            system.BindValidatedCatalog(catalog);

            float initial = system.GetComponent("test_structural")!.Condition;
            system.TickDay(currentDay: 2, weatherStressMult: 1.0f, radiationStressMult: 1.0f);
            float after = system.GetComponent("test_structural")!.Condition;

            Assert.True(after < initial, $"Expected degradation: initial={initial}, after={after}");
        }

        [Fact]
        public void ShelterMaintenanceSystem_Repair_RestoresConditionAndOperationalStatus()
        {
            string json = @"{
                ""schema_version"": 1,
                ""components"": [
                    {
                        ""component_id"": ""test_power"",
                        ""display_name"": ""Test Power Unit"",
                        ""component_type"": ""Power"",
                        ""max_condition"": 100,
                        ""base_degradation_rate"": 5.0,
                        ""warning_threshold"": 40,
                        ""failure_threshold"": 10,
                        ""repair_time_hours"": 4,
                        ""description"": ""Test""
                    }
                ]
            }";

            var catalog = ShelterComponentCatalogLoader.LoadFromJson(json);
            var system = new ShelterMaintenanceSystem();
            system.BindValidatedCatalog(catalog);

            // Degrade below failure threshold
            for (int d = 1; d <= 20; d++)
                system.TickDay(d, weatherStressMult: 1.0f, radiationStressMult: 1.0f);

            var comp = system.GetComponent("test_power")!;
            bool wasFailedOrWarning = !comp.IsOperational || comp.HasWarning || comp.Condition < 40f;
            Assert.True(wasFailedOrWarning);

            // Overhaul restores to max
            bool overhauled = system.PerformMaintenance("test_power", "Overhaul", skillLevel: 100f, day: 21);
            Assert.True(overhauled);
            comp = system.GetComponent("test_power")!;
            Assert.True(comp.IsOperational);
            Assert.Equal(100.0f, comp.Condition, precision: 2);
        }

        [Fact]
        public void ShelterMaintenanceSystem_SaveRestoreRoundtrip_PreservesState()
        {
            string json = @"{
                ""schema_version"": 1,
                ""components"": [
                    {
                        ""component_id"": ""test_water"",
                        ""display_name"": ""Test Water Filter"",
                        ""component_type"": ""Water"",
                        ""max_condition"": 100,
                        ""base_degradation_rate"": 2.0,
                        ""warning_threshold"": 40,
                        ""failure_threshold"": 10,
                        ""repair_time_hours"": 2,
                        ""description"": ""Test""
                    }
                ]
            }";

            var catalog = ShelterComponentCatalogLoader.LoadFromJson(json);
            var system1 = new ShelterMaintenanceSystem();
            system1.BindValidatedCatalog(catalog);
            system1.TickDay(1, 1.0f, 1.0f);
            system1.PerformMaintenance("test_water", "Clean", skillLevel: 50f, day: 2);

            var captured = system1.CaptureState();

            var system2 = new ShelterMaintenanceSystem();
            system2.RestoreState(captured);
            system2.BindValidatedCatalog(catalog);

            Assert.Equal(system1.TrackedComponentCount, system2.TrackedComponentCount);
            Assert.Equal(system1.GetComponent("test_water")!.Condition,
                system2.GetComponent("test_water")!.Condition, precision: 3);
        }
    }
}
