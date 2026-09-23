// SPDX-License-Identifier: MIT
// Plan 152 host integration tests: verifies the Core vehicle customization
// authority's validated-catalog seam, census contract, schema-gated restore,
// the state round-trip, and the save-section / day-event wiring the host
// depends on.

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Save;
using Ashfall.Core.Vehicles;
using Xunit;

namespace Ashfall.Core.Tests.Vehicles
{
    public sealed class Plan152VehicleCustomizationHostIntegrationTests
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

        private static string DataDir() =>
            Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static VehicleCustomizationCatalog BoundCatalog()
        {
            var loaded = VehicleModuleCatalogLoader.Load(DataDir(), new FileSystemIO());
            Assert.False(loaded.HasErrors, string.Join("; ", loaded.Errors));

            var catalog = new VehicleCustomizationCatalog();
            catalog.BindValidatedModules(loaded.Modules);
            return catalog;
        }

        // ── 1 — the validated bind seam is the only path the host uses ──────

        [Fact]
        public void BindValidatedModules_ReplacesLenientState_AndKeepsAuthoredRows()
        {
            var catalog = VehicleCustomizationCatalog.LoadFromJson(@"{ ""modules"": [ { ""module_id"": ""module_lenient"", ""module_type"": ""utility"", ""name"": ""L"" } ] }");
            Assert.True(catalog.TryGetModule("module_lenient", out _));

            var loaded = VehicleModuleCatalogLoader.Load(DataDir(), new FileSystemIO());
            catalog.BindValidatedModules(loaded.Modules);

            Assert.True(catalog.AllModules.Count >= 20);
            Assert.False(catalog.TryGetModule("module_lenient", out _));
        }

        [Fact]
        public void AuthoredCatalog_IsStrict_AndCoversEveryCategory()
        {
            var loaded = VehicleModuleCatalogLoader.Load(DataDir(), new FileSystemIO());

            Assert.False(loaded.HasErrors, string.Join("; ", loaded.Errors));
            Assert.True(loaded.Modules.Count >= 20);

            var types = loaded.Modules.Select(m => m.ModuleType.ToLowerInvariant()).Distinct().ToList();
            Assert.Contains("armor", types);
            Assert.Contains("cargo", types);
            Assert.Contains("living", types);
            Assert.Contains("weapon", types);
            Assert.Contains("utility", types);
        }

        // ── 2 — census contract ─────────────────────────────────────────────

        [Fact]
        public void Census_ReportsModulesVehiclesInstallsCampsAndMobileCapability()
        {
            var system = new VehicleCustomizationSystem(BoundCatalog());

            system.InstallModule("v_hauler", "reinforced_hull");
            system.InstallModule("v_hauler", "bunk_beds_module");
            system.DeployBaseCamp("v_hauler", "loc_relay_station");

            var census = system.GetCensus();

            Assert.True(census.LoadedModulesCount >= 20);
            Assert.Equal(1, census.VehiclesWithModulesCount);
            Assert.Equal(2, census.TotalInstalledModules);
            Assert.Equal(1, census.DeployedBaseCampsCount);
            Assert.Equal(1, census.MobileBaseCapableVehicles);
        }

        // ── 3 — schema gating ───────────────────────────────────────────────

        [Fact]
        public void RestoreState_RejectsAnUnknownSchemaVersion()
        {
            var system = new VehicleCustomizationSystem(BoundCatalog());

            var ex = Assert.Throws<InvalidOperationException>(
                () => system.RestoreState(@"{""schema_version"":99,""vehicle_modules"":{},""base_camps"":{}}"));

            Assert.Contains("schema_version", ex.Message);
        }

        [Fact]
        public void RestoreState_AcceptsALegacyPayloadWithoutASchemaVersion()
        {
            var system = new VehicleCustomizationSystem(BoundCatalog());

            system.RestoreState(@"{""vehicle_modules"":{""v_old"":[""reinforced_hull""]},""base_camps"":{}}");

            Assert.Single(system.GetInstalledModules("v_old"));
            Assert.Equal("reinforced_hull", system.GetInstalledModules("v_old")[0]);
        }

        // ── 4 — state round-trip ────────────────────────────────────────────

        [Fact]
        public void CaptureRestore_IsAFullRoundTrip()
        {
            var system = new VehicleCustomizationSystem(BoundCatalog());
            system.InstallModule("v_hauler", "reinforced_hull");
            system.InstallModule("v_hauler", "bunk_beds_module");
            system.DeployBaseCamp("v_hauler", "loc_relay_station");

            var restored = new VehicleCustomizationSystem(BoundCatalog());
            restored.RestoreState(system.CaptureState());

            Assert.Equal(2, restored.GetInstalledModules("v_hauler").Count);
            Assert.Equal("loc_relay_station", restored.GetBaseCampLocation("v_hauler"));
            Assert.True(restored.IsMobileBaseCapable("v_hauler"));
            Assert.Equal(system.GetCensus().TotalInstalledModules, restored.GetCensus().TotalInstalledModules);
        }

        [Fact]
        public void CaptureRestore_IsDeterministic()
        {
            var system = new VehicleCustomizationSystem(BoundCatalog());
            system.InstallModule("v_hauler", "reinforced_hull");
            system.DeployBaseCamp("v_hauler", "loc_relay_station");

            Assert.Equal(system.CaptureState(), system.CaptureState());
        }

        [Fact]
        public void DeployBaseCamp_RequiresALivingModule()
        {
            var system = new VehicleCustomizationSystem(BoundCatalog());

            // Stock vehicle: no living module, so it cannot become a base.
            Assert.False(system.DeployBaseCamp("v_stock", "loc_x"));
            Assert.False(system.IsBaseCampDeployed("v_stock"));

            system.InstallModule("v_stock", "bunk_beds_module");
            Assert.True(system.DeployBaseCamp("v_stock", "loc_x"));
        }

        // ── 5 — host wiring the save orchestrator depends on ────────────────

        [Fact]
        public void SaveSectionRegistry_DeclaresVehicleCustomization()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("vehicle_customization", out _));
            Assert.Equal("vehicle_customization_save.json", SaveSectionRegistry.FileNameFor("vehicle_customization"));
        }

        [Fact]
        public void DayEventVocabulary_ClassifiesVehicleCustomizationTickAsAnInternalHeartbeat()
        {
            Assert.True(DayEventVocabulary.IsInternalHeartbeat("vehicle_customization_ticked"));
            Assert.Equal(SemanticKind.Heartbeat, DayEventVocabulary.GetSemanticKind("vehicle_customization_ticked"));
        }
    }
}
