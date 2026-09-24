// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Records
{
    public sealed class LifeStagesCatalogLoaderTests
    {
        private static string GetStreamingAssetsDataPath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (Directory.Exists(path)) return path;

            // Fallback for different working directory layouts
            path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data"));
            return path;
        }

        [Fact]
        public void Load_CanonicalCatalog_SucceedsWithExpectedStagesAndMilestones()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = LifeStagesCatalogLoader.Load(dataDir);

            Assert.True(result.Success, $"Catalog load failed: {string.Join("; ", result.Errors)}");
            Assert.NotNull(result.Catalog);
            Assert.Equal(1, result.Catalog.schema_version);
            Assert.Equal(30, result.Catalog.days_per_year);
            Assert.Equal(65, result.Catalog.min_retirement_age_years);
            Assert.Equal(5, result.Catalog.life_stages.Count);
            Assert.Equal(6, result.Catalog.milestones.Count);
        }

        [Fact]
        public void LoadFromJson_EmptyOrWhitespace_ReturnsError()
        {
            var resEmpty = LifeStagesCatalogLoader.LoadFromJson("");
            Assert.False(resEmpty.Success);
            Assert.Contains(resEmpty.Errors, e => e.Contains("empty", StringComparison.OrdinalIgnoreCase));

            var resWs = LifeStagesCatalogLoader.LoadFromJson("   ");
            Assert.False(resWs.Success);
            Assert.Contains(resWs.Errors, e => e.Contains("empty", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void LoadFromJson_InvalidJson_ReturnsError()
        {
            var result = LifeStagesCatalogLoader.LoadFromJson("{ invalid json }");
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("JSON deserialization", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void LoadFromJson_InvalidSchemaVersion_ReturnsError()
        {
            string json = @"{
                ""schema_version"": 0,
                ""days_per_year"": 30,
                ""min_retirement_age_years"": 65,
                ""life_stages"": [
                    { ""stage_id"": ""s1"", ""stage_enum"": ""Prime"", ""display_name"": ""Prime"", ""min_age"": 20, ""max_age"": 40, ""physical_labor_multiplier"": 1.0, ""fatigue_accumulation_multiplier"": 1.0 }
                ]
            }";
            var result = LifeStagesCatalogLoader.LoadFromJson(json);
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("schema_version", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void LoadFromJson_InvalidDaysPerYear_ReturnsError()
        {
            string json = @"{
                ""schema_version"": 1,
                ""days_per_year"": 0,
                ""min_retirement_age_years"": 65,
                ""life_stages"": [
                    { ""stage_id"": ""s1"", ""stage_enum"": ""Prime"", ""display_name"": ""Prime"", ""min_age"": 20, ""max_age"": 40, ""physical_labor_multiplier"": 1.0, ""fatigue_accumulation_multiplier"": 1.0 }
                ]
            }";
            var result = LifeStagesCatalogLoader.LoadFromJson(json);
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("days_per_year", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void LoadFromJson_EmptyLifeStages_ReturnsError()
        {
            string json = @"{
                ""schema_version"": 1,
                ""days_per_year"": 30,
                ""min_retirement_age_years"": 65,
                ""life_stages"": []
            }";
            var result = LifeStagesCatalogLoader.LoadFromJson(json);
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("no life stages", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void LoadFromJson_UnrecognizedStageEnum_ReturnsError()
        {
            string json = @"{
                ""schema_version"": 1,
                ""days_per_year"": 30,
                ""min_retirement_age_years"": 65,
                ""life_stages"": [
                    { ""stage_id"": ""s1"", ""stage_enum"": ""Superhuman"", ""display_name"": ""Hero"", ""min_age"": 20, ""max_age"": 40, ""physical_labor_multiplier"": 1.0, ""fatigue_accumulation_multiplier"": 1.0 }
                ]
            }";
            var result = LifeStagesCatalogLoader.LoadFromJson(json);
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("unrecognized stage_enum", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void LoadFromJson_DuplicateStageId_ReturnsError()
        {
            string json = @"{
                ""schema_version"": 1,
                ""days_per_year"": 30,
                ""min_retirement_age_years"": 65,
                ""life_stages"": [
                    { ""stage_id"": ""dup_stage"", ""stage_enum"": ""Child"", ""display_name"": ""Child 1"", ""min_age"": 0, ""max_age"": 17, ""physical_labor_multiplier"": 0.5, ""fatigue_accumulation_multiplier"": 1.1 },
                    { ""stage_id"": ""dup_stage"", ""stage_enum"": ""Prime"", ""display_name"": ""Child 2"", ""min_age"": 18, ""max_age"": 30, ""physical_labor_multiplier"": 1.0, ""fatigue_accumulation_multiplier"": 1.0 }
                ]
            }";
            var result = LifeStagesCatalogLoader.LoadFromJson(json);
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("Duplicate stage_id", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void LoadFromJson_MaxAgeLessThanMinAge_ReturnsError()
        {
            string json = @"{
                ""schema_version"": 1,
                ""days_per_year"": 30,
                ""min_retirement_age_years"": 65,
                ""life_stages"": [
                    { ""stage_id"": ""s1"", ""stage_enum"": ""Prime"", ""display_name"": ""Prime"", ""min_age"": 50, ""max_age"": 20, ""physical_labor_multiplier"": 1.0, ""fatigue_accumulation_multiplier"": 1.0 }
                ]
            }";
            var result = LifeStagesCatalogLoader.LoadFromJson(json);
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("max_age", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void LoadFromJson_NonPositiveLaborMultiplier_ReturnsError()
        {
            string json = @"{
                ""schema_version"": 1,
                ""days_per_year"": 30,
                ""min_retirement_age_years"": 65,
                ""life_stages"": [
                    { ""stage_id"": ""s1"", ""stage_enum"": ""Prime"", ""display_name"": ""Prime"", ""min_age"": 20, ""max_age"": 50, ""physical_labor_multiplier"": 0.0, ""fatigue_accumulation_multiplier"": 1.0 }
                ]
            }";
            var result = LifeStagesCatalogLoader.LoadFromJson(json);
            Assert.False(result.Success);
            Assert.Contains(result.Errors, e => e.Contains("physical_labor_multiplier", StringComparison.OrdinalIgnoreCase));
        }
    }
}
