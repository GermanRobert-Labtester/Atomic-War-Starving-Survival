// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    using SeededRng = Ashfall.Core.SeededRng;

    public sealed class ArmoredCrawlerExpeditionTests : CatalogTestBase
    {
        private (ArmoredCrawlerModuleCatalog catalog, Inventory.Inventory inv) CreateFixture()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var cat = ArmoredCrawlerModuleCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(cat);

            var inv = new Inventory.Inventory();
            return (cat!, inv);
        }

        [Fact]
        public void Catalog_LoadsTenModules_AndValidatesAllFields()
        {
            var (catalog, _) = CreateFixture();
            Assert.Equal(10, catalog.Modules.Count);

            foreach (var mod in catalog.Modules.Values)
            {
                bool valid = mod.Validate(out string err);
                Assert.True(valid, $"Module '{mod.id}' failed validation: {err}");
            }
        }

        [Fact]
        public void Catalog_Modules_ExistInItemCatalogs()
        {
            var (catalog, _) = CreateFixture();
            var allItemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

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
                            if (!string.IsNullOrEmpty(id)) allItemIds.Add(id);
                        }
                    }
                }
            }

            foreach (var mod in catalog.Modules.Values)
            {
                Assert.True(allItemIds.Contains(mod.id),
                    $"Crawler module '{mod.id}' is missing from item catalogs.");
            }
        }

        [Fact]
        public void TryInstallModule_ConsumesModuleItem_Atomically()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            inv.AddById("module_crawler_living_quarters", 1);

            bool installed = sys.TryInstallModule("crawler_01", "module_crawler_living_quarters");
            Assert.True(installed);
            Assert.Equal(0, inv.CountById("module_crawler_living_quarters"));

            var crawler = sys.GetCrawler("crawler_01")!;
            Assert.Contains("module_crawler_living_quarters", crawler.installedModuleIds);
        }

        [Fact]
        public void TryInstallModule_ExceedingMaxSlots_Blocked()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            var crawler = sys.GetCrawler("crawler_01")!;
            crawler.maxSlots = 2;

            inv.AddById("module_crawler_living_quarters", 1);
            inv.AddById("module_crawler_auxiliary_tank", 1);
            inv.AddById("module_crawler_machine_lathe", 1);

            Assert.True(sys.TryInstallModule("crawler_01", "module_crawler_living_quarters"));
            Assert.True(sys.TryInstallModule("crawler_01", "module_crawler_auxiliary_tank"));
            Assert.False(sys.TryInstallModule("crawler_01", "module_crawler_machine_lathe"));

            Assert.Equal(1, inv.CountById("module_crawler_machine_lathe")); // Not deducted
        }

        [Fact]
        public void TryInstallModule_ExceedingMaxMass_Blocked()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            var crawler = sys.GetCrawler("crawler_01")!;
            crawler.maxMass = 1300.0f; // Chassis is 1200kg, living quarters is 450kg -> 1650 > 1300

            inv.AddById("module_crawler_living_quarters", 1);

            bool installed = sys.TryInstallModule("crawler_01", "module_crawler_living_quarters");
            Assert.False(installed);
            Assert.Equal(1, inv.CountById("module_crawler_living_quarters"));
        }

        [Fact]
        public void TryUninstallModule_RemovesModuleAndGrantsItem()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            inv.AddById("module_crawler_living_quarters", 1);
            sys.TryInstallModule("crawler_01", "module_crawler_living_quarters");

            bool uninstalled = sys.TryUninstallModule("crawler_01", "module_crawler_living_quarters");
            Assert.True(uninstalled);
            Assert.Equal(1, inv.CountById("module_crawler_living_quarters"));
            Assert.DoesNotContain("module_crawler_living_quarters", sys.GetCrawler("crawler_01")!.installedModuleIds);
        }

        [Fact]
        public void ComputeTotalMass_SumsChassisAndModules()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            // Chassis dry mass is 1200kg
            Assert.Equal(1200.0f, sys.ComputeTotalMass("crawler_01"));

            inv.AddById("module_crawler_living_quarters", 1); // 450kg
            sys.TryInstallModule("crawler_01", "module_crawler_living_quarters");
            Assert.Equal(1650.0f, sys.ComputeTotalMass("crawler_01"));
        }

        [Fact]
        public void GetEffectiveCrewBerths_AccountsForCabinModules()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            // Base berths: 2
            Assert.Equal(2, sys.GetEffectiveCrewBerths("crawler_01"));

            inv.AddById("module_crawler_living_quarters", 1); // +4 berths
            sys.TryInstallModule("crawler_01", "module_crawler_living_quarters");
            Assert.Equal(6, sys.GetEffectiveCrewBerths("crawler_01"));
        }

        [Fact]
        public void HasWorkshopCapability_DetectsLatheModule()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            Assert.False(sys.HasWorkshopCapability("crawler_01"));

            inv.AddById("module_crawler_machine_lathe", 1);
            sys.TryInstallModule("crawler_01", "module_crawler_machine_lathe");
            Assert.True(sys.HasWorkshopCapability("crawler_01"));
        }

        [Fact]
        public void CanTraverseTerrain_ValidatesTrackedTerrainTypes()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            Assert.True(sys.CanTraverseTerrain("crawler_01", "deep_ash"));
            Assert.True(sys.CanTraverseTerrain("crawler_01", "slag"));
            Assert.True(sys.CanTraverseTerrain("crawler_01", "mud"));
            Assert.False(sys.CanTraverseTerrain("crawler_01", "ocean_abyss"));

            // If immobilized, cannot traverse
            sys.GetCrawler("crawler_01")!.isImmobilized = true;
            Assert.False(sys.CanTraverseTerrain("crawler_01", "deep_ash"));
        }

        [Fact]
        public void ProjectToVehicleProfile_ReflectsModulesAndCondition()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            inv.AddById("module_crawler_auxiliary_tank", 1); // +250kg cargo, -0.20 fuelMod
            sys.TryInstallModule("crawler_01", "module_crawler_auxiliary_tank");

            var profile = sys.ProjectToVehicleProfile("crawler_01");
            Assert.Equal("crawler_01", profile.vehicleId);
            Assert.Equal(750.0f, profile.cargoCapacityKg); // 500 + 250
            Assert.True(profile.speedMultiplier > 0f);
        }

        [Fact]
        public void ImmobilizedCrawler_HasZeroSpeedMultiplier()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            sys.GetCrawler("crawler_01")!.isImmobilized = true;
            var profile = sys.ProjectToVehicleProfile("crawler_01");
            Assert.Equal(0.0f, profile.speedMultiplier);
        }

        [Fact]
        public void TryRepairTrack_ConsumesMechanicalParts_RestoresMobility()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            var crawler = sys.GetCrawler("crawler_01")!;
            crawler.trackCondition = 0.0f;
            crawler.isImmobilized = true;

            inv.AddById("mechanical_parts", 2);

            bool repaired = sys.TryRepairTrack("crawler_01");
            Assert.True(repaired);
            Assert.Equal(100.0f, crawler.trackCondition);
            Assert.False(crawler.isImmobilized);
            Assert.Equal(0, inv.CountById("mechanical_parts"));
        }

        [Fact]
        public void TryDeployCamp_AndDismantleCamp_UpdatesState()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            inv.AddById("module_crawler_machine_lathe", 1);
            sys.TryInstallModule("crawler_01", "module_crawler_machine_lathe");

            bool deployed = sys.TryDeployCamp("crawler_01", "loc_rad_crater_summit");
            Assert.True(deployed);
            Assert.Single(sys.RemoteCamps);
            var camp = sys.RemoteCamps[0];
            Assert.Equal("loc_rad_crater_summit", camp.locationId);
            Assert.True(camp.hasWorkshop);

            bool dismantled = sys.TryDismantleCamp("crawler_01");
            Assert.True(dismantled);
            Assert.Empty(sys.RemoteCamps);
        }

        [Fact]
        public void SaveRestore_PreservesModulesTracksAndRemoteCamps()
        {
            var (catalog, inv) = CreateFixture();
            var sysA = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(42));

            inv.AddById("module_crawler_living_quarters", 1);
            sysA.TryInstallModule("crawler_01", "module_crawler_living_quarters");
            sysA.TryDeployCamp("crawler_01", "loc_iron_ridge");
            sysA.GetCrawler("crawler_01")!.trackCondition = 68.0f;

            var save = sysA.CaptureState();

            var sysB = new ArmoredCrawlerExpeditionSystem(inv, catalog, new SeededRng(999));
            sysB.RestoreState(save);

            var crawlerB = sysB.GetCrawler("crawler_01")!;
            Assert.Contains("module_crawler_living_quarters", crawlerB.installedModuleIds);
            Assert.Equal(68.0f, crawlerB.trackCondition);
            Assert.Single(sysB.RemoteCamps);
            Assert.Equal("loc_iron_ridge", sysB.RemoteCamps[0].locationId);
        }
    }
}
