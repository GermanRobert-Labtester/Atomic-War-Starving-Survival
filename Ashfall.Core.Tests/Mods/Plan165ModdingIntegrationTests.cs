// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Mods;
using Xunit;

namespace Ashfall.Core.Tests.Plan165Modding
{
    public sealed class Plan165ModdingIntegrationTests
    {
        private static string GetSchemaJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/mod_manifest_schema.json");
            if (File.Exists(path))
                return File.ReadAllText(path);

            string altPath = "Assets/StreamingAssets/Data/mod_manifest_schema.json";
            if (File.Exists(altPath))
                return File.ReadAllText(altPath);

            return string.Empty;
        }

        [Fact]
        public void Specification_LoadsSuccessfully_WhitelistPopulated()
        {
            var system = new ModSupportSystem();
            string json = GetSchemaJson();
            if (!string.IsNullOrEmpty(json))
                system.LoadSpecification(json);

            Assert.Equal(1, system.Specification.SchemaVersion);
            Assert.Contains("items.json", system.Specification.AllowedCatalogs);
            Assert.Contains("nuclear_winter_phases.json", system.Specification.AllowedCatalogs);
            Assert.NotEmpty(system.Specification.ConflictPolicies);
        }

        [Fact]
        public void ManifestValidation_ValidatesIdAndSchemaVersion()
        {
            var system = new ModSupportSystem();

            var validManifest = new ModManifest
            {
                ModId = "custom_scavenge_pack",
                DisplayName = "Custom Scavenge Pack",
                Version = "1.0.0",
                SchemaVersion = 1,
                Catalogs = { "items.json" }
            };
            var validRes = system.ValidateManifest(validManifest);
            Assert.True(validRes.IsValid, $"Valid manifest rejected with: {string.Join(", ", validRes.Errors)}");

            var invalidIdManifest = new ModManifest
            {
                ModId = "INVALID ID WITH SPACES!",
                DisplayName = "Bad Mod",
                Version = "1.0.0",
                SchemaVersion = 1
            };
            var invalidRes = system.ValidateManifest(invalidIdManifest);
            Assert.False(invalidRes.IsValid);
            Assert.Contains(invalidRes.Errors, e => e.Contains("must contain only lowercase alphanumeric"));
        }

        [Fact]
        public void ModRegistration_EvaluatesGameAndContractVersion()
        {
            var system = new ModSupportSystem();

            var compatibleMod = new ModManifest
            {
                ModId = "weather_expanded",
                DisplayName = "Weather Expanded",
                Version = "1.2.0",
                GameRange = ">=1.0.0 <2.0.0",
                ModContractRange = ">=1",
                Catalogs = { "weather_effects.json" }
            };

            var regComp = system.RegisterMod(compatibleMod, currentGameVersion: "1.5.0", currentContractVersion: 1);
            Assert.Equal(ModStatus.Enabled, regComp.Status);

            var incompatibleMod = new ModManifest
            {
                ModId = "future_tech",
                DisplayName = "Future Tech",
                Version = "2.0.0",
                GameRange = ">=2.0.0",
                Catalogs = { "items.json" }
            };

            var regIncomp = system.RegisterMod(incompatibleMod, currentGameVersion: "1.5.0", currentContractVersion: 1);
            Assert.Equal(ModStatus.Incompatible, regIncomp.Status);
        }

        [Fact]
        public void DependencyManagement_DetectsMissingAndCyclicDependencies()
        {
            var system = new ModSupportSystem();

            // Mod B depends on Mod A (which doesn't exist yet)
            var modB = new ModManifest
            {
                ModId = "mod_addon",
                DisplayName = "Mod Addon",
                Dependencies = { "mod_core" },
                Catalogs = { "items.json" }
            };

            var regB = system.RegisterMod(modB);
            Assert.Equal(ModStatus.MissingDependency, regB.Status);

            // Now register Mod A
            var modA = new ModManifest
            {
                ModId = "mod_core",
                DisplayName = "Mod Core",
                Catalogs = { "items.json" }
            };
            var regA = system.RegisterMod(modA);
            Assert.Equal(ModStatus.Enabled, regA.Status);
            // Recheck modB status now that dependency is satisfied
            Assert.Equal(ModStatus.Enabled, system.RegisteredMods["mod_addon"].Status);

            // Now test cyclic dependency: modC -> modD -> modC
            var modC = new ModManifest
            {
                ModId = "mod_c",
                DisplayName = "Mod C",
                Dependencies = { "mod_d" },
                Catalogs = { "items.json" }
            };
            var modD = new ModManifest
            {
                ModId = "mod_d",
                DisplayName = "Mod D",
                Dependencies = { "mod_c" },
                Catalogs = { "items.json" }
            };

            system.RegisterMod(modC);
            system.RegisterMod(modD);

            Assert.Equal(ModStatus.CyclicDependency, system.RegisteredMods["mod_c"].Status);
        }

        [Fact]
        public void DeterministicLoadOrder_And_ConflictDetection()
        {
            var system = new ModSupportSystem();

            var modA = new ModManifest
            {
                ModId = "mod_alpha",
                DisplayName = "Mod Alpha",
                LoadOrder = 20,
                Catalogs = { "items.json" },
                AllowOverrides = false
            };
            var modB = new ModManifest
            {
                ModId = "mod_beta",
                DisplayName = "Mod Beta",
                LoadOrder = 10,
                Catalogs = { "items.json" },
                AllowOverrides = false
            };

            system.RegisterMod(modA);
            system.RegisterMod(modB);

            var loadOrder = system.ResolveLoadOrder();
            Assert.Equal(2, loadOrder.Count);
            // Lower load order comes first
            Assert.Equal("mod_beta", loadOrder[0]);
            Assert.Equal("mod_alpha", loadOrder[1]);

            // Conflict detection: both modify items.json without AllowOverrides
            var conflicts = system.DetectConflicts();
            Assert.NotEmpty(conflicts);
            Assert.Equal("items.json", conflicts[0].CatalogName);
        }

        [Fact]
        public void SaveState_RoundTrip_PreservesActiveAndDisabledStates()
        {
            var system = new ModSupportSystem();

            var mod1 = new ModManifest { ModId = "mod_opt1", DisplayName = "Opt 1", Catalogs = { "items.json" } };
            var mod2 = new ModManifest { ModId = "mod_opt2", DisplayName = "Opt 2", Catalogs = { "items.json" } };

            system.RegisterMod(mod1);
            system.RegisterMod(mod2);

            system.SetModEnabled("mod_opt2", false);
            Assert.Equal(ModStatus.Disabled, system.RegisteredMods["mod_opt2"].Status);

            var save = system.CaptureState();
            Assert.Equal(1, save.SchemaVersion);
            Assert.Contains("mod_opt1", save.ActiveModIds);
            Assert.Contains("mod_opt2", save.DisabledModIds);

            var newSystem = new ModSupportSystem();
            newSystem.RegisterMod(mod1);
            newSystem.RegisterMod(mod2);
            newSystem.RestoreState(save);

            Assert.Equal(ModStatus.Enabled, newSystem.RegisteredMods["mod_opt1"].Status);
            Assert.Equal(ModStatus.Disabled, newSystem.RegisteredMods["mod_opt2"].Status);
        }
    }
}
