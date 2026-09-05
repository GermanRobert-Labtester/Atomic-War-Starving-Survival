// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    using SeededRng = Ashfall.Core.SeededRng;

    public sealed class HydroponicBiomeTests : CatalogTestBase
    {
        private (HydroponicCropCatalog catalog, Inventory.Inventory inv) CreateFixture()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var cat = HydroponicCropCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(cat);

            var inv = new Inventory.Inventory();
            return (cat!, inv);
        }

        [Fact]
        public void Catalog_LoadsTenCultivars_AndValidatesAllFields()
        {
            var (catalog, _) = CreateFixture();
            Assert.Equal(10, catalog.Crops.Count);

            foreach (var crop in catalog.Crops.Values)
            {
                bool valid = crop.Validate(out string err);
                Assert.True(valid, $"Crop '{crop.id}' failed validation: {err}");
            }
        }

        [Fact]
        public void Catalog_ReferencedOutputItems_ExistInItemCatalogs()
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

            foreach (var crop in catalog.Crops.Values)
            {
                Assert.True(allItemIds.Contains(crop.baseYieldItemId),
                    $"Crop '{crop.id}' references output item '{crop.baseYieldItemId}' which does not exist in item catalogs.");
            }
        }

        [Fact]
        public void TryMixNutrientBatch_DeductsWaterAndChemicals_Atomically()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42));

            inv.AddById("clean_water", 4);
            inv.AddById("scrap_chemical", 2);

            bool mixed = sys.TryMixNutrientBatch(2);
            Assert.True(mixed);
            Assert.Equal(20.0f, sys.NutrientTankReserve);
            Assert.Equal(0, inv.CountById("clean_water"));
            Assert.Equal(0, inv.CountById("scrap_chemical"));
        }

        [Fact]
        public void TryMixNutrientBatch_InsufficientInputs_NoPartialDeduction()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42));

            inv.AddById("clean_water", 1); // Need 2
            inv.AddById("scrap_chemical", 2);

            bool mixed = sys.TryMixNutrientBatch(1);
            Assert.False(mixed);
            Assert.Equal(0.0f, sys.NutrientTankReserve);
            Assert.Equal(1, inv.CountById("clean_water"));
            Assert.Equal(2, inv.CountById("scrap_chemical"));
        }

        [Fact]
        public void TryPlantCrop_DeductsSeed_InitializesRack()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42));

            sys.AddSeed("crop_winter_rye_x1", 2);
            Assert.Equal(2, sys.SeedVaultInventory["crop_winter_rye_x1"]);

            bool planted = sys.TryPlantCrop("rack_01", "crop_winter_rye_x1", "worker_elena");
            Assert.True(planted);
            Assert.Equal(1, sys.SeedVaultInventory["crop_winter_rye_x1"]);

            var rack = sys.GetRack("rack_01");
            Assert.NotNull(rack);
            Assert.Equal("crop_winter_rye_x1", rack!.cropId);
            Assert.Equal(0, rack.growthPermille);
            Assert.Equal(100.0f, rack.rootHealth);
            Assert.Equal("worker_elena", rack.assignedWorkerId);
        }

        [Fact]
        public void TryPlantCrop_OccupiedRack_Blocked()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42));

            sys.AddSeed("crop_winter_rye_x1", 2);
            sys.TryPlantCrop("rack_01", "crop_winter_rye_x1");

            bool secondPlant = sys.TryPlantCrop("rack_01", "crop_winter_rye_x1");
            Assert.False(secondPlant);
        }

        [Fact]
        public void SetLedSpectrum_ValidatesModes()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42));

            Assert.True(sys.SetLedSpectrum("rack_01", "Flowering_Red"));
            Assert.Equal("Flowering_Red", sys.GetRack("rack_01")!.ledSpectrum);

            Assert.True(sys.SetLedSpectrum("rack_01", "Hardening_Infrared"));
            Assert.Equal("Hardening_Infrared", sys.GetRack("rack_01")!.ledSpectrum);

            Assert.False(sys.SetLedSpectrum("rack_01", "Invalid_Ultraviolet"));
        }

        [Fact]
        public void PowerBrownout_StallsGrowthAndDecaysRootHealth()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42), isGridPowered: () => false);

            inv.AddById("clean_water", 10);
            inv.AddById("scrap_chemical", 5);
            Assert.True(sys.TryMixNutrientBatch(5));
            sys.AddSeed("crop_winter_rye_x1", 1);
            sys.TryPlantCrop("rack_01", "crop_winter_rye_x1");

            sys.TickDay(1);

            var rack = sys.GetRack("rack_01")!;
            Assert.Equal(0, rack.growthPermille); // Stalled
            Assert.Equal(85.0f, rack.rootHealth); // Decayed from 100
        }

        [Fact]
        public void WaterShortage_DecaysRootHealth()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42), waterConsume: _ => false);

            inv.AddById("clean_water", 10);
            inv.AddById("scrap_chemical", 5);
            Assert.True(sys.TryMixNutrientBatch(5));
            sys.AddSeed("crop_winter_rye_x1", 1);
            sys.TryPlantCrop("rack_01", "crop_winter_rye_x1");

            sys.TickDay(1);

            var rack = sys.GetRack("rack_01")!;
            Assert.Equal(0, rack.growthPermille);
            Assert.Equal(80.0f, rack.rootHealth);
        }

        [Fact]
        public void OptimalConditions_AdvancesGrowthDeterministically()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42));

            inv.AddById("clean_water", 20);
            inv.AddById("scrap_chemical", 10);
            sys.TryMixNutrientBatch(5);

            sys.AddSeed("crop_winter_rye_x1", 1);
            sys.TryPlantCrop("rack_01", "crop_winter_rye_x1");

            sys.TickDay(1);

            var rack = sys.GetRack("rack_01")!;
            Assert.True(rack.growthPermille > 0, "Growth should advance under optimal conditions");
            Assert.Equal(100.0f, rack.rootHealth);
        }

        [Fact]
        public void FloweringRedSpectrum_BoostsHarvestYield()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42));

            sys.AddSeed("crop_winter_rye_x1", 1);
            sys.TryPlantCrop("rack_01", "crop_winter_rye_x1");
            var rack = sys.GetRack("rack_01")!;
            rack.growthPermille = 1000; // Mature
            sys.SetLedSpectrum("rack_01", "Flowering_Red");

            bool harvested = sys.TryHarvest("rack_01");
            Assert.True(harvested);
            // Base yield for winter rye is 4, Flowering_Red adds +1 -> 5
            Assert.Equal(5, inv.CountById("crop_ash_grain"));
            Assert.Equal(string.Empty, rack.cropId);
        }

        [Fact]
        public void RadiationExposure_TriggersDeterministicMutation()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42));

            sys.AddSeed("crop_rad_scrubbing_kelp", 1);
            sys.TryPlantCrop("rack_01", "crop_rad_scrubbing_kelp");

            var rack = sys.GetRack("rack_01")!;
            // High radiation triggers mutation affinity roll
            for (int d = 1; d <= 10; d++)
            {
                sys.TickDay(d, ambientRadiation: 80.0f);
                if (rack.activeTraits.Count > 0) break;
            }

            Assert.NotEmpty(rack.activeTraits);
            Assert.Contains(rack.activeTraits[0], HydroponicBiomeSystem.MutationTraitPool);
        }

        [Fact]
        public void TryStabilizeTrait_ConsumesResources_AndPersists()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42));

            inv.AddById("scrap_chemical", 2);
            inv.AddById("clean_water", 1);

            bool stabilized = sys.TryStabilizeTrait("Trait_Cold_Hardy");
            Assert.True(stabilized);
            Assert.Contains("Trait_Cold_Hardy", sys.UnlockedStabilizedTraits);
            Assert.Equal(0, inv.CountById("scrap_chemical"));
            Assert.Equal(0, inv.CountById("clean_water"));

            // Newly planted crops inherit stabilized traits
            sys.AddSeed("crop_winter_rye_x1", 1);
            sys.TryPlantCrop("rack_01", "crop_winter_rye_x1");
            Assert.Contains("Trait_Cold_Hardy", sys.GetRack("rack_01")!.activeTraits);
        }

        [Fact]
        public void TryHarvest_MatureCrop_GrantsItemsAtomicallyAndResetsRack()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42));

            sys.AddSeed("crop_winter_rye_x1", 1);
            sys.TryPlantCrop("rack_01", "crop_winter_rye_x1");

            var rack = sys.GetRack("rack_01")!;
            rack.growthPermille = 900;
            Assert.False(sys.TryHarvest("rack_01"), "Immature crop cannot be harvested");

            rack.growthPermille = 1000;
            Assert.True(sys.TryHarvest("rack_01"));
            Assert.Equal(4, inv.CountById("crop_ash_grain"));
            Assert.Equal(1, sys.SeedVaultInventory["crop_winter_rye_x1"]);
            Assert.Equal(string.Empty, rack.cropId);
        }

        [Fact]
        public void SaveRestore_PreservesFullRackState_ContinuationEquivalence()
        {
            var (catalog, inv) = CreateFixture();
            var sysA = new HydroponicBiomeSystem(inv, catalog, new SeededRng(42));

            sysA.AddSeed("crop_winter_rye_x1", 3);
            sysA.TryPlantCrop("rack_01", "crop_winter_rye_x1");
            sysA.SetLedSpectrum("rack_01", "Hardening_Infrared");
            sysA.GetRack("rack_01")!.growthPermille = 450;
            sysA.GetRack("rack_01")!.rootHealth = 88.0f;

            var save = sysA.CaptureState();

            var sysB = new HydroponicBiomeSystem(inv, catalog, new SeededRng(999));
            sysB.RestoreState(save);

            var rackB = sysB.GetRack("rack_01")!;
            Assert.Equal("crop_winter_rye_x1", rackB.cropId);
            Assert.Equal(450, rackB.growthPermille);
            Assert.Equal(88.0f, rackB.rootHealth);
            Assert.Equal("Hardening_Infrared", rackB.ledSpectrum);
            Assert.Equal(2, sysB.SeedVaultInventory["crop_winter_rye_x1"]);
        }
    }
}
