// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Xunit;
using InventoryModel = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Expeditions
{
    public sealed class VehicleArmorGradesTests
    {
        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("StreamingAssets/Data was not found");
        }

        private static VehicleArmorGradeCatalog LoadRealCatalog()
        {
            var result = VehicleArmorGradeCatalogLoader.Load(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.NotNull(result.Catalog);
            return result.Catalog!;
        }

        private static VehicleGarageSystem CreateGarage()
        {
            var garage = new VehicleGarageSystem();
            garage.LoadArmorCatalog(LoadRealCatalog());
            return garage;
        }

        private static InventoryModel CreateInventoryWithMaterials(int count = 50)
        {
            var inv = new InventoryModel { Capacity = 64, MaxWeight = 10000f };
            inv.AddById("scrap_metal", count);
            inv.AddById("mechanical_parts", count);
            inv.AddById("item_metallurgy_iron_ingot", count);
            inv.AddById("item_ebpvd_ceramic_target_ingot", count);
            return inv;
        }

        [Fact]
        public void T01_CatalogLoadsAllFourUpgradeTiersAndStockDefault()
        {
            var catalog = LoadRealCatalog();
            Assert.Equal(5, catalog.grades.Count);
            Assert.Equal("grade_0_stock", catalog.default_grade_id);

            var stock = catalog.grades.Find(g => g.id == "grade_0_stock");
            Assert.NotNull(stock);
            Assert.True(stock!.is_default);
            Assert.Equal(0, stock.tier);
            Assert.Equal(0, stock.mitigation_permille);
            Assert.Equal(0, stock.wear_absorption_permille);
            Assert.Empty(stock.install_cost);

            for (int tier = 1; tier <= 4; tier++)
            {
                var grade = catalog.grades.Find(g => g.tier == tier);
                Assert.NotNull(grade);
                Assert.False(grade!.is_default);
                Assert.NotEmpty(grade.install_cost);
                Assert.NotEmpty(grade.reforge_cost);
            }
        }

        [Fact]
        public void T02_DamageMitigationAndWearAbsorptionMonotonicProgression()
        {
            var catalog = LoadRealCatalog();
            var g1 = catalog.grades.Find(g => g.id == "grade_1_scrap_plate")!;
            var g2 = catalog.grades.Find(g => g.id == "grade_2_sheet_plate")!;
            var g3 = catalog.grades.Find(g => g.id == "grade_3_composite_plate")!;
            var g4 = catalog.grades.Find(g => g.id == "grade_4_alloyed_heavy_plate")!;

            // Strict monotonic scaling across tiers 1 through 4
            Assert.True(g1.mitigation_permille < g2.mitigation_permille);
            Assert.True(g2.mitigation_permille < g3.mitigation_permille);
            Assert.True(g3.mitigation_permille < g4.mitigation_permille);

            Assert.True(g1.wear_absorption_permille < g2.wear_absorption_permille);
            Assert.True(g2.wear_absorption_permille < g3.wear_absorption_permille);
            Assert.True(g3.wear_absorption_permille < g4.wear_absorption_permille);

            Assert.True(g1.integrity_pool_permille < g2.integrity_pool_permille);
            Assert.True(g2.integrity_pool_permille < g3.integrity_pool_permille);
            Assert.True(g3.integrity_pool_permille < g4.integrity_pool_permille);

            Assert.Equal(100, g1.mitigation_permille);
            Assert.Equal(150, g2.mitigation_permille);
            Assert.Equal(200, g3.mitigation_permille);
            Assert.Equal(250, g4.mitigation_permille);
        }

        [Fact]
        public void T03_SpeedPenaltyAndFuelConsumptionProgression()
        {
            var catalog = LoadRealCatalog();
            var g1 = catalog.grades.Find(g => g.id == "grade_1_scrap_plate")!;
            var g2 = catalog.grades.Find(g => g.id == "grade_2_sheet_plate")!;
            var g3 = catalog.grades.Find(g => g.id == "grade_3_composite_plate")!;
            var g4 = catalog.grades.Find(g => g.id == "grade_4_alloyed_heavy_plate")!;

            // Speed penalty deepens (becomes more negative) with heavier armor
            Assert.True(g1.speed_multiplier_delta > g2.speed_multiplier_delta);
            Assert.True(g2.speed_multiplier_delta > g3.speed_multiplier_delta);
            Assert.True(g3.speed_multiplier_delta > g4.speed_multiplier_delta);

            // Fuel multiplier increases with heavier armor
            Assert.True(g1.fuel_consumption_multiplier < g2.fuel_consumption_multiplier);
            Assert.True(g2.fuel_consumption_multiplier < g3.fuel_consumption_multiplier);
            Assert.True(g3.fuel_consumption_multiplier < g4.fuel_consumption_multiplier);
        }

        [Fact]
        public void T04_ChassisStressAbsorptionAndIntegrityDepletion()
        {
            var garage = CreateGarage();
            var inv = CreateInventoryWithMaterials();
            string vehicle = "test_truck";

            Assert.True(garage.InstallArmorGrade(vehicle, "grade_2_sheet_plate", inv, out string err), err);
            var initialProfile = garage.GetArmorProfile(vehicle);
            Assert.Equal("nominal", initialProfile.ConditionBand);
            Assert.Equal(140, initialProfile.IntegrityPermille);
            Assert.Equal(150, initialProfile.MitigationPermille);

            // Trip wear reduces armor integrity while absorbing chassis stress
            garage.RecordTripWear(vehicle, 60f);
            var record = garage.GetRecord(vehicle)!;
            Assert.True(record.armorIntegrityPermille < 140);
            Assert.True(record.chassisStressPermille > 0);

            // Depleting the armor integrity removes active mitigation
            record.armorIntegrityPermille = 0;
            var depletedProfile = garage.GetArmorProfile(vehicle);
            Assert.Equal("depleted", depletedProfile.ConditionBand);
            Assert.Equal(0, depletedProfile.MitigationPermille);
            Assert.Equal(0, depletedProfile.WearAbsorptionPermille);
            // But mass/speed penalty stays intact
            Assert.Equal(initialProfile.SpeedMultiplierDelta, depletedProfile.SpeedMultiplierDelta);
        }

        [Fact]
        public void T05_ReforgePlateRestoresIntegrityPoolUsingReforgeBill()
        {
            var garage = CreateGarage();
            var inv = CreateInventoryWithMaterials(100);
            string vehicle = "reforge_rover";

            Assert.True(garage.InstallArmorGrade(vehicle, "grade_2_sheet_plate", inv, out string installErr), installErr);
            var record = garage.GetRecord(vehicle)!;
            record.armorIntegrityPermille = 10; // heavily worn

            int scrapBefore = inv.CountById("scrap_metal");
            int partsBefore = inv.CountById("mechanical_parts");

            Assert.True(garage.ReforgeArmorPlate(vehicle, inv, out string reforgeErr), reforgeErr);
            Assert.Equal(record.armorIntegrityMaxPermille, record.armorIntegrityPermille);
            Assert.Equal("nominal", garage.GetArmorProfile(vehicle).ConditionBand);

            // Verify reforge cost deducted: 3 scrap, 1 part
            Assert.Equal(scrapBefore - 3, inv.CountById("scrap_metal"));
            Assert.Equal(partsBefore - 1, inv.CountById("mechanical_parts"));

            // Reforging when at full integrity is rejected
            Assert.False(garage.ReforgeArmorPlate(vehicle, inv, out string fullErr));
            Assert.Contains("full integrity", fullErr);
        }
    }
}
