// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ashfall.Core.Crafting;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{

    public sealed class FlagshipIntegrationIxSmokeTests : CatalogTestBase
    {
        private sealed class SystemHarness
        {
            public Inventory.Inventory Inventory { get; }
            public HydroponicBiomeSystem Hydroponics { get; }
            public ChemicalSynthesisSystem ChemicalSynthesis { get; }
            public NuclearCoreLifecycleSystem NuclearCore { get; }
            public ArmoredCrawlerExpeditionSystem CrawlerExpedition { get; }

            public SystemHarness(int seed, string dataDir)
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();

                var cropCat = HydroponicCropCatalogLoader.Load(dataDir, files, json)!;
                var chemCat = ChemicalSynthesisCatalogLoader.Load(dataDir, files, json)!;
                var nucCat = NuclearCoreCatalogLoader.Load(dataDir, files, json)!;
                var crawlerCat = ArmoredCrawlerModuleCatalogLoader.Load(dataDir, files, json)!;

                Inventory = new Inventory.Inventory();

                // Wire cross-system callbacks
                NuclearCore = new NuclearCoreLifecycleSystem(
                    Inventory,
                    nucCat,
                    new SeededRng(seed + 100));

                Hydroponics = new HydroponicBiomeSystem(
                    Inventory,
                    cropCat,
                    new SeededRng(seed + 200),
                    isGridPowered: () => NuclearCore.GetTotalGenerationWatts() >= 450.0f,
                    waterConsume: amount =>
                    {
                        if (Inventory.CountById("clean_water") >= (int)Math.Ceiling(amount))
                        {
                            Inventory.RemoveById("clean_water", (int)Math.Ceiling(amount));
                            return true;
                        }
                        return false;
                    });

                ChemicalSynthesis = new ChemicalSynthesisSystem(
                    Inventory,
                    chemCat,
                    new SeededRng(seed + 300),
                    startingTier: 2);

                CrawlerExpedition = new ArmoredCrawlerExpeditionSystem(
                    Inventory,
                    crawlerCat,
                    new SeededRng(seed + 400));
            }
        }

        [Fact]
        public void CrossSystem_LateGameInfrastructure_Lifecycle_ExecutesCleanly()
        {
            var harness = new SystemHarness(42, DataDirectory);
            var inv = harness.Inventory;

            // 1. Initial supply stocking
            inv.AddById("clean_water", 50);
            inv.AddById("scrap_chemical", 30);
            inv.AddById("chemical_solvent", 10);
            inv.AddById("scrap_metal", 20);
            inv.AddById("lead_sheet", 10);
            inv.AddById("scram_boron_canister", 2);
            inv.AddById("mechanical_parts", 10);
            inv.AddById("module_crawler_living_quarters", 1);
            inv.AddById("module_crawler_machine_lathe", 1);

            // 2. Install and activate pebble-bed nuclear core
            harness.NuclearCore.TryInstallCore("core_main_reactor", "core_naval_pebble_bed_2kw", "reactor_chamber");
            harness.NuclearCore.SetOutputSetting("core_main_reactor", "Normal");
            Assert.Equal(2000.0f, harness.NuclearCore.GetTotalGenerationWatts());

            // 3. Hydroponics biome setup
            harness.Hydroponics.TryMixNutrientBatch(3);
            Assert.Equal(30.0f, harness.Hydroponics.NutrientTankReserve);

            harness.Hydroponics.AddSeed("crop_winter_rye_x1", 2);
            bool planted = harness.Hydroponics.TryPlantCrop("rack_01", "crop_winter_rye_x1", "agronomist_maria");
            Assert.True(planted);
            harness.Hydroponics.SetLedSpectrum("rack_01", "Growth_Blue");

            // 4. Chemical synthesis operation
            bool chemStarted = harness.ChemicalSynthesis.TryStartProcess("synth_high_energy_binder", "retort_01", "chemist_yuri");
            Assert.True(chemStarted);

            // 5. Armored crawler fleet outfitting
            bool mod1 = harness.CrawlerExpedition.TryInstallModule("crawler_01", "module_crawler_living_quarters");
            bool mod2 = harness.CrawlerExpedition.TryInstallModule("crawler_01", "module_crawler_machine_lathe");
            Assert.True(mod1 && mod2);
            Assert.True(harness.CrawlerExpedition.HasWorkshopCapability("crawler_01"));
            Assert.Equal(6, harness.CrawlerExpedition.GetEffectiveCrewBerths("crawler_01")); // 2 base + 4 quarters

            // 6. Advance days across the settlement
            for (int day = 1; day <= 6; day++)
            {
                harness.NuclearCore.TickDay(day, _ => true);
                harness.Hydroponics.TickDay(day, ambientRadiation: 5.0f);
                harness.ChemicalSynthesis.TickDay(day);
                harness.CrawlerExpedition.TickDay(day);
            }

            // 7. Verify chemical synthesis harvest
            bool harvestedChem = harness.ChemicalSynthesis.TryHarvestOutput("retort_01");
            Assert.True(harvestedChem);
            Assert.Equal(1, inv.CountById("synth_high_energy_binder"));

            // 8. Verify crop maturation and harvest
            var rack = harness.Hydroponics.GetRack("rack_01")!;
            Assert.Equal(1000, rack.growthPermille);
            bool harvestedCrop = harness.Hydroponics.TryHarvest("rack_01");
            Assert.True(harvestedCrop);
            Assert.True(inv.CountById("crop_ash_grain") >= 4);

            // 9. Deploy crawler forward staging camp
            bool campDeployed = harness.CrawlerExpedition.TryDeployCamp("crawler_01", "loc_rad_crater_outpost");
            Assert.True(campDeployed);
            Assert.Single(harness.CrawlerExpedition.RemoteCamps);
            Assert.True(harness.CrawlerExpedition.RemoteCamps[0].hasWorkshop);

            // 10. Emergency SCRAM simulation on reactor
            bool scrammed = harness.NuclearCore.TryEmergencyScram("core_main_reactor");
            Assert.True(scrammed);
            Assert.Equal(0.0f, harness.NuclearCore.GetTotalGenerationWatts());
            Assert.Equal(1, inv.CountById("scram_boron_canister")); // 1 remaining
        }

        [Fact]
        public void DeterministicReplay_ThreeConsecutiveRuns_ProduceIdenticalHashes()
        {
            string RunSimulation(int seed)
            {
                var harness = new SystemHarness(seed, DataDirectory);
                var inv = harness.Inventory;

                inv.AddById("clean_water", 100);
                inv.AddById("scrap_chemical", 50);
                inv.AddById("chemical_solvent", 20);
                inv.AddById("lead_sheet", 10);
                inv.AddById("scram_boron_canister", 5);
                inv.AddById("mechanical_parts", 20);
                inv.AddById("module_crawler_reinforced_treads", 1);
                inv.AddById("module_crawler_auxiliary_tank", 1);

                harness.NuclearCore.TryInstallCore("core_01", "core_beryllium_moderated_1kw");
                harness.NuclearCore.SetOutputSetting("core_01", "Normal");

                harness.Hydroponics.TryMixNutrientBatch(4);
                harness.Hydroponics.AddSeed("crop_oilseed_brassica", 2);
                harness.Hydroponics.TryPlantCrop("rack_01", "crop_oilseed_brassica");
                harness.Hydroponics.SetLedSpectrum("rack_01", "Flowering_Red");

                harness.ChemicalSynthesis.TryStartProcess("synth_precision_primer_compound", "retort_01");

                harness.CrawlerExpedition.TryInstallModule("crawler_01", "module_crawler_reinforced_treads");
                harness.CrawlerExpedition.TryInstallModule("crawler_01", "module_crawler_auxiliary_tank");

                for (int d = 1; d <= 5; d++)
                {
                    harness.NuclearCore.TickDay(d, _ => true);
                    harness.Hydroponics.TickDay(d, ambientRadiation: 45.0f);
                    harness.ChemicalSynthesis.TickDay(d);
                    harness.CrawlerExpedition.TickDay(d);
                }

                var hydroSave = harness.Hydroponics.CaptureState();
                var chemSave = harness.ChemicalSynthesis.CaptureState();
                var nucSave = harness.NuclearCore.CaptureState();
                var crawlerSave = harness.CrawlerExpedition.CaptureState();

                var composite = new
                {
                    hydro = hydroSave,
                    chem = chemSave,
                    nuc = nucSave,
                    crawler = crawlerSave
                };

                string json = JsonSerializer.Serialize(composite);
                using var sha = SHA256.Create();
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
                return Convert.ToHexString(hash);
            }

            string hash1 = RunSimulation(42);
            string hash2 = RunSimulation(42);
            string hash3 = RunSimulation(42);

            Assert.Equal(hash1, hash2);
            Assert.Equal(hash2, hash3);
        }
    }
}
