// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Vehicles;
using Xunit;

namespace Ashfall.Core.Tests.Records
{
    /// <summary>
    /// Plan 152 — strict loader contract for the authored vehicle module table.
    /// The data authority decides which modules exist and what they cost, so a
    /// malformed row must be rejected rather than silently defaulted into a
    /// live vehicle build.
    /// </summary>
    public class VehicleModuleCatalogLoaderTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent;
            return dir?.FullName ?? throw new InvalidOperationException("repo root not found");
        }

        private static string DataDir() =>
            Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private const string ValidTable = @"{
          ""schema_version"": 1,
          ""modules"": [
            { ""module_id"": ""module_reinforced_hull"", ""module_type"": ""armor"", ""name"": ""Reinforced Hull"",
              ""defense_bonus"": 20.0, ""speed_modifier"": -0.06, ""cargo_bonus"": 0.0, ""bunk_capacity"": 0,
              ""installation_days"": 3, ""scrap_cost"": 60, ""components_cost"": 30 },
            { ""module_id"": ""module_bunk_beds"", ""module_type"": ""living"", ""name"": ""Bunk Beds"",
              ""defense_bonus"": 0.0, ""speed_modifier"": -0.05, ""cargo_bonus"": 0.0, ""bunk_capacity"": 4,
              ""installation_days"": 2, ""scrap_cost"": 40, ""components_cost"": 20 }
          ]
        }";

        [Fact]
        public void ValidTable_LoadsEveryRow()
        {
            var result = VehicleModuleCatalogLoader.LoadFromJson(ValidTable);

            Assert.False(result.HasErrors);
            Assert.Equal(2, result.Modules.Count);
            Assert.Equal("module_reinforced_hull", result.Modules[0].ModuleId);
            Assert.Equal("armor", result.Modules[0].ModuleType);
            Assert.Equal(20f, result.Modules[0].DefenseBonus, 3);
            Assert.Equal(4, result.Modules[1].BunkCapacity);
        }

        [Fact]
        public void EmptyTable_IsAHardError()
        {
            var result = VehicleModuleCatalogLoader.LoadFromJson(
                @"{ ""schema_version"": 1, ""modules"": [] }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("must not be empty"));
        }

        [Fact]
        public void DuplicateModuleId_IsRejected()
        {
            var result = VehicleModuleCatalogLoader.LoadFromJson(@"{
              ""schema_version"": 1,
              ""modules"": [
                { ""module_id"": ""module_same"", ""module_type"": ""armor"", ""name"": ""A"" },
                { ""module_id"": ""module_same"", ""module_type"": ""armor"", ""name"": ""B"" }
              ]
            }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("duplicate vehicle module id"));
        }

        [Fact]
        public void UnknownModuleType_IsRejected()
        {
            var result = VehicleModuleCatalogLoader.LoadFromJson(@"{
              ""schema_version"": 1,
              ""modules"": [
                { ""module_id"": ""module_x"", ""module_type"": ""submarine"", ""name"": ""X"" }
              ]
            }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("unknown module_type"));
        }

        [Fact]
        public void EmptyModuleIdAndName_AreRejected()
        {
            var result = VehicleModuleCatalogLoader.LoadFromJson(@"{
              ""schema_version"": 1,
              ""modules"": [
                { ""module_id"": """", ""module_type"": ""armor"", ""name"": """" }
              ]
            }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("empty module_id"));
            Assert.Contains(result.Errors, e => e.Contains("empty name"));
        }

        [Fact]
        public void NegativeDefenseBonus_IsRejected()
        {
            var result = VehicleModuleCatalogLoader.LoadFromJson(@"{
              ""schema_version"": 1,
              ""modules"": [
                { ""module_id"": ""module_x"", ""module_type"": ""armor"", ""name"": ""X"", ""defense_bonus"": -5.0 }
              ]
            }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("negative defense_bonus"));
        }

        [Fact]
        public void SpeedModifierBeyondFloor_IsRejected()
        {
            var result = VehicleModuleCatalogLoader.LoadFromJson(@"{
              ""schema_version"": 1,
              ""modules"": [
                { ""module_id"": ""module_x"", ""module_type"": ""cargo"", ""name"": ""X"", ""speed_modifier"": -0.95 }
              ]
            }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("speed_modifier"));
        }

        [Fact]
        public void CargoPenaltyLargerThanBaseHold_IsRejected()
        {
            var result = VehicleModuleCatalogLoader.LoadFromJson(@"{
              ""schema_version"": 1,
              ""modules"": [
                { ""module_id"": ""module_x"", ""module_type"": ""cargo"", ""name"": ""X"", ""cargo_bonus"": -120.0 }
              ]
            }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("cargo_bonus"));
        }

        [Fact]
        public void SubDayInstallation_IsRejected()
        {
            var result = VehicleModuleCatalogLoader.LoadFromJson(@"{
              ""schema_version"": 1,
              ""modules"": [
                { ""module_id"": ""module_x"", ""module_type"": ""utility"", ""name"": ""X"", ""installation_days"": 0 }
              ]
            }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("installation_days"));
        }

        [Fact]
        public void UnsupportedSchemaVersion_IsRejected()
        {
            var result = VehicleModuleCatalogLoader.LoadFromJson(
                @"{ ""schema_version"": 7, ""modules"": [ { ""module_id"": ""a"", ""module_type"": ""armor"", ""name"": ""A"" } ] }");

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("schema_version"));
        }

        [Fact]
        public void AuthoredTable_OnDisk_LoadsStrictly_WithAllFiveCategories()
        {
            var result = VehicleModuleCatalogLoader.Load(DataDir(), new Ashfall.Core.FileSystemIO());

            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.True(result.Modules.Count >= 20, $"expected >= 20 authored modules, found {result.Modules.Count}");

            var types = result.Modules.Select(m => m.ModuleType.ToLowerInvariant()).Distinct().ToList();
            Assert.Contains("armor", types);
            Assert.Contains("cargo", types);
            Assert.Contains("living", types);
            Assert.Contains("weapon", types);
            Assert.Contains("utility", types);
        }
    }
}
