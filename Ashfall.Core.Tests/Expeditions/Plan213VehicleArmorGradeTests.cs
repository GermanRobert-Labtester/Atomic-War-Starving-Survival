// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Xunit;
using InventoryModel = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Expeditions
{
    public sealed class Plan213VehicleArmorGradeTests
    {
        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("StreamingAssets/Data was not found");
        }

        private static VehicleArmorGradeCatalog RealArmorCatalog()
        {
            var result = VehicleArmorGradeCatalogLoader.Load(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(result.HasErrors, string.Join("\n", result.Errors));
            Assert.NotNull(result.Catalog);
            return result.Catalog!;
        }

        private static VehicleGarageSystem Garage()
        {
            var garage = new VehicleGarageSystem();
            garage.LoadArmorCatalog(RealArmorCatalog());
            return garage;
        }

        private static InventoryModel Materials(int amount = 100)
        {
            var inventory = new InventoryModel { Capacity = 64, MaxWeight = 10000f };
            inventory.AddById("scrap_metal", amount);
            inventory.AddById("mechanical_parts", amount);
            inventory.AddById("item_metallurgy_iron_ingot", amount);
            inventory.AddById("item_ebpvd_ceramic_target_ingot", amount);
            return inventory;
        }

        private static ExpeditionVehicleProfile Profile(string id = "vehicle_test") => new ExpeditionVehicleProfile
        {
            vehicleId = id,
            speedMultiplier = 1f,
            cargoCapacityKg = 100f,
            fuelPerTravelTick = 10f,
            breakdownChancePerTick = 0.2f
        };

        [Fact]
        public void T01_RealCatalogLoadsFiveRowsAndContiguousTiers()
        {
            var catalog = RealArmorCatalog();
            Assert.Equal(5, catalog.grades.Count);
            Assert.Equal("grade_0_stock", catalog.default_grade_id);
            Assert.Equal(new[] { 0, 1, 2, 3, 4 }, catalog.grades.ConvertAll(g => g.tier));
        }

        [Fact]
        public void T02_DefaultRowIsNeutral()
        {
            var grade = RealArmorCatalog().grades[0];
            Assert.True(grade.is_default);
            Assert.Equal(0, grade.mitigation_permille);
            Assert.Equal(0, grade.wear_absorption_permille);
            Assert.Equal(0, grade.integrity_pool_permille);
            Assert.Equal(0f, grade.speed_multiplier_delta);
            Assert.Equal(1f, grade.fuel_consumption_multiplier);
            Assert.Empty(grade.install_cost);
            Assert.Empty(grade.reforge_cost);
        }

        [Fact]
        public void T03ToT09_DataGateReportsEachRepresentativeViolation()
        {
            string json = "{\"schema_version\":1,\"default_grade_id\":\"grade_0_stock\",\"grades\":["
                + "{\"id\":\"grade_0_stock\",\"display_name\":\"Stock\",\"description\":\"x\",\"tier\":0,\"is_default\":true,\"mitigation_permille\":0,\"wear_absorption_permille\":0,\"integrity_pool_permille\":0,\"speed_multiplier_delta\":0,\"fuel_consumption_multiplier\":1,\"compatible_terrain_types\":[\"road\"],\"install_cost\":[],\"install_labor_ticks\":0,\"reforge_cost\":[],\"tags\":[\"armor\"]},"
                + "{\"id\":\"grade_1_bad\",\"display_name\":\"Bad\",\"description\":\"x\",\"tier\":1,\"is_default\":false,\"mitigation_permille\":251,\"wear_absorption_permille\":351,\"integrity_pool_permille\":100,\"speed_multiplier_delta\":0,\"fuel_consumption_multiplier\":1,\"compatible_terrain_types\":[\"ocean\"],\"install_cost\":[{\"item_id\":\"not_an_item\",\"amount\":1}],\"install_labor_ticks\":0,\"reforge_cost\":[{\"item_id\":\"item_ebpvd_ceramic_target_ingot\",\"amount\":2}],\"tags\":[]}] }";
            string temp = Path.Combine(Path.GetTempPath(), "ashfall-armor-invalid-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(temp);
            try
            {
                File.WriteAllText(Path.Combine(temp, "vehicle_armor_grades.json"), json);
                File.Copy(Path.Combine(DataDir(), "items.json"), Path.Combine(temp, "items.json"));
                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateVehicleArmorGradeCatalog(temp, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("mitigation_permille"));
                Assert.Contains(report.Errors, e => e.Contains("wear_absorption_permille"));
                Assert.Contains(report.Errors, e => e.Contains("unknown item_id"));
                Assert.Contains(report.Errors, e => e.Contains("unsupported compatible terrain"));
                Assert.Contains(report.Errors, e => e.Contains("reforge_cost may not contain"));
                Assert.Contains(report.Errors, e => e.Contains("tags must contain"));
            }
            finally { Directory.Delete(temp, true); }
        }

        [Fact]
        public void T10_LegacyProfileAndWearRemainIdentical()
        {
            var control = new VehicleGarageSystem();
            var candidate = Garage();
            var controlProfile = Profile();
            var candidateProfile = Profile();
            control.GetOrCreateRecord("vehicle_test");
            candidate.GetOrCreateRecord("vehicle_test");
            control.DecorateProfile(controlProfile);
            candidate.DecorateProfile(candidateProfile);
            control.RecordTripWear("vehicle_test", 60f, 1f);
            candidate.RecordTripWear("vehicle_test", 60f, 1f);
            Assert.Equal(controlProfile.speedMultiplier, candidateProfile.speedMultiplier);
            Assert.Equal(controlProfile.fuelPerTravelTick, candidateProfile.fuelPerTravelTick);
            Assert.Equal(controlProfile.breakdownChancePerTick, candidateProfile.breakdownChancePerTick);
            Assert.Equal(new SystemTextJsonSerializer().Serialize(control.CaptureState()), new SystemTextJsonSerializer().Serialize(candidate.CaptureState()));
        }

        [Fact]
        public void T11_InstallStampsAndInsufficientBillDoesNotMutate()
        {
            var garage = Garage();
            var inventory = Materials(100);
            Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_1_scrap_plate", inventory, out string reason), reason);
            var record = garage.GetRecord("vehicle_test")!;
            Assert.Equal("grade_1_scrap_plate", record.armorGradeId);
            Assert.Equal(record.armorIntegrityMaxPermille, record.armorIntegrityPermille);
            int before = inventory.CountById("scrap_metal");
            var poor = new InventoryModel();
            Assert.False(garage.InstallArmorGrade("vehicle_test", "grade_4_alloyed_heavy_plate", poor, out _));
            Assert.Equal("grade_1_scrap_plate", record.armorGradeId);
            Assert.Equal(before, inventory.CountById("scrap_metal"));
        }

        [Fact]
        public void T12_ImmobilizedVehicleRefusesInstallAndReforge()
        {
            var garage = Garage();
            var inventory = Materials();
            Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_1_scrap_plate", inventory, out _));
            var record = garage.GetRecord("vehicle_test")!;
            record.isImmobilized = true;
            record.armorIntegrityPermille = 0;
            Assert.False(garage.InstallArmorGrade("vehicle_test", "grade_2_sheet_plate", inventory, out string installReason));
            Assert.Contains("immobilized", installReason);
            Assert.False(garage.ReforgeArmorPlate("vehicle_test", inventory, out string reforgeReason));
            Assert.Contains("immobilized", reforgeReason);
        }

        [Fact]
        public void T13_TerrainGateRefusesCoastalForCompositeAndUnsetResolverPasses()
        {
            var garage = Garage();
            var inventory = Materials();
            garage.VehicleTerrainResolver = id => id == "dredger" ? "coastal" : "road";
            Assert.False(garage.InstallArmorGrade("dredger", "grade_3_composite_plate", inventory, out string reason));
            Assert.Contains("coastal", reason);
            garage.VehicleTerrainResolver = null;
            Assert.True(garage.InstallArmorGrade("dredger", "grade_3_composite_plate", inventory, out reason), reason);
        }

        [Fact]
        public void T14_ReplaceRefundsHalfOldScrapAndResetsIntegrity()
        {
            var garage = Garage();
            var inventory = Materials();
            Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_1_scrap_plate", inventory, out _));
            garage.RecordTripWear("vehicle_test", 10f);
            int scrapBefore = inventory.CountById("scrap_metal");
            Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_3_composite_plate", inventory, out string reason), reason);
            Assert.Equal(scrapBefore - 8 + 2, inventory.CountById("scrap_metal"));
            var profile = garage.GetArmorProfile("vehicle_test");
            Assert.Equal("grade_3_composite_plate", profile.GradeId);
            Assert.Equal(profile.IntegrityMaxPermille, profile.IntegrityPermille);
        }

        [Fact]
        public void T15_MitigationIsExactAndNeverImmunity()
        {
            var garage = Garage();
            var inventory = Materials();
            Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_2_sheet_plate", inventory, out _));
            var profile = Profile();
            garage.DecorateProfile(profile);
            Assert.Equal(0.17f, profile.breakdownChancePerTick, 5);
            Assert.True(profile.breakdownChancePerTick > 0f);
            Assert.Equal(0.96f, profile.speedMultiplier, 5);
            Assert.Equal(10.5f, profile.fuelPerTravelTick, 5);
        }

        [Fact]
        public void T16_AbsorptionSplitsChassisAndClampsToIntegrity()
        {
            var garage = Garage();
            var inventory = Materials();
            Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_1_scrap_plate", inventory, out _));
            garage.RecordTripWear("vehicle_test", 60f);
            var record = garage.GetRecord("vehicle_test")!;
            Assert.Equal(96, record.chassisStressPermille);
            Assert.Equal(76, record.armorIntegrityPermille);
            record.armorIntegrityPermille = 3;
            garage.RecordTripWear("vehicle_test", 60f);
            Assert.Equal(213, record.chassisStressPermille);
            Assert.Equal(0, record.armorIntegrityPermille);
        }

        [Fact]
        public void T17_EngineAndTransmissionUseFullBaseWear()
        {
            var armored = Garage();
            var stock = Garage();
            var inventory = Materials();
            Assert.True(armored.InstallArmorGrade("vehicle_test", "grade_4_alloyed_heavy_plate", inventory, out _));
            armored.RecordTripWear("vehicle_test", 60f);
            stock.RecordTripWear("vehicle_test", 60f);
            Assert.Equal(stock.GetRecord("vehicle_test")!.engineFoulingPermille, armored.GetRecord("vehicle_test")!.engineFoulingPermille);
            Assert.Equal(stock.GetRecord("vehicle_test")!.transmissionWearPermille, armored.GetRecord("vehicle_test")!.transmissionWearPermille);
        }

        [Fact]
        public void T18_DepletedPlateLosesMitigationButKeepsMassPenalty()
        {
            var garage = Garage();
            var inventory = Materials();
            Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_2_sheet_plate", inventory, out _));
            var record = garage.GetRecord("vehicle_test")!;
            record.armorIntegrityPermille = 0;
            var profile = Profile();
            garage.DecorateProfile(profile);
            Assert.Equal(0.2f, profile.breakdownChancePerTick, 5);
            Assert.Equal(0.96f, profile.speedMultiplier, 5);
            Assert.Equal("depleted", garage.GetArmorProfile("vehicle_test").ConditionBand);
        }

        [Fact]
        public void T19_ReforgeRestoresStampedPoolAndRejectsFullOrStock()
        {
            var garage = Garage();
            var inventory = Materials();
            Assert.False(garage.ReforgeArmorPlate("none", inventory, out string stockReason));
            Assert.Contains("No fitted", stockReason);
            Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_2_sheet_plate", inventory, out _));
            var record = garage.GetRecord("vehicle_test")!;
            record.armorIntegrityPermille = 1;
            int before = inventory.CountById("scrap_metal");
            Assert.True(garage.ReforgeArmorPlate("vehicle_test", inventory, out string reason), reason);
            Assert.Equal(record.armorIntegrityMaxPermille, record.armorIntegrityPermille);
            Assert.Equal(before - 3, inventory.CountById("scrap_metal"));
            Assert.False(garage.ReforgeArmorPlate("vehicle_test", inventory, out string fullReason));
            Assert.Contains("full integrity", fullReason);
        }

        [Fact]
        public void T20_FoundryQualityStampsPoolButNotMitigation()
        {
            var poorGarage = Garage();
            poorGarage.ArmorMaterialQualitySource = (out FoundryMaterialQuality quality) =>
            {
                quality = new FoundryMaterialQuality("machine_steel", FoundryPurityTier.Poor, 1000, 800, 1000);
                return true;
            };
            var exceptionalGarage = Garage();
            exceptionalGarage.ArmorMaterialQualitySource = (out FoundryMaterialQuality quality) =>
            {
                quality = new FoundryMaterialQuality("machine_steel", FoundryPurityTier.Exceptional, 1000, 1200, 1000);
                return true;
            };
            Assert.True(poorGarage.InstallArmorGrade("poor", "grade_2_sheet_plate", Materials(), out _));
            Assert.True(exceptionalGarage.InstallArmorGrade("exceptional", "grade_2_sheet_plate", Materials(), out _));
            var poor = poorGarage.GetArmorProfile("poor");
            var exceptional = exceptionalGarage.GetArmorProfile("exceptional");
            Assert.InRange(poor.IntegrityMaxPermille, 70, 182);
            Assert.InRange(exceptional.IntegrityMaxPermille, 70, 182);
            Assert.Equal(poor.MitigationPermille, exceptional.MitigationPermille);
            Assert.Equal("Poor", poor.Purity);
            Assert.Equal("Exceptional", exceptional.Purity);
        }

        [Fact]
        public void T21_StampedPoolIsNotRecomputedAfterProviderChanges()
        {
            var garage = Garage();
            bool enabled = true;
            garage.ArmorMaterialQualitySource = (out FoundryMaterialQuality quality) =>
            {
                quality = new FoundryMaterialQuality("machine_steel", FoundryPurityTier.High, 1000, 1200, 1000);
                return enabled;
            };
            Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_2_sheet_plate", Materials(), out _));
            int stamped = garage.GetArmorProfile("vehicle_test").IntegrityMaxPermille;
            enabled = false;
            var restored = new VehicleGarageSystem();
            restored.LoadArmorCatalog(RealArmorCatalog());
            restored.ArmorMaterialQualitySource = garage.ArmorMaterialQualitySource;
            restored.RestoreState(garage.CaptureState());
            Assert.Equal(stamped, restored.GetArmorProfile("vehicle_test").IntegrityMaxPermille);
        }

        [Fact]
        public void T22_SaveRoundtripPreservesArmorStateMidWear()
        {
            var garage = Garage();
            Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_3_composite_plate", Materials(), out _));
            garage.RecordTripWear("vehicle_test", 60f);
            var before = garage.GetRecord("vehicle_test")!;
            var restored = Garage();
            restored.RestoreState(garage.CaptureState());
            var after = restored.GetRecord("vehicle_test")!;
            Assert.Equal(before.armorIntegrityPermille, after.armorIntegrityPermille);
            Assert.Equal(before.armorIntegrityMaxPermille, after.armorIntegrityMaxPermille);
            Assert.Equal(before.armorGradeId, after.armorGradeId);
            Assert.Equal(before.armorMaterialProfileId, after.armorMaterialProfileId);
            Assert.Equal(before.armorPurity, after.armorPurity);
        }

        [Fact]
        public void T23_LegacyRecordDefaultsToStock()
        {
            var garage = Garage();
            garage.RestoreState(new VehicleGarageState
            {
                vehicleRecords = new Dictionary<string, VehicleCustomizationRecord>
                {
                    ["legacy"] = new VehicleCustomizationRecord { vehicleId = "legacy", chassisStressPermille = 100 }
                }
            });
            var profile = garage.GetArmorProfile("legacy");
            Assert.True(profile.IsDefault);
            Assert.Equal("none", profile.ConditionBand);
            Assert.Equal(0, profile.MitigationPermille);
            Assert.Equal(0, profile.IntegrityMaxPermille);
        }

        [Fact]
        public void T24_SameOperationsProduceIdenticalStateFingerprint()
        {
            string Run()
            {
                var garage = Garage();
                Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_2_sheet_plate", Materials(), out _));
                garage.RecordTripWear("vehicle_test", 60f);
                garage.RecordTripWear("vehicle_test", 60f);
                var rec = garage.GetRecord("vehicle_test")!;
                rec.armorIntegrityPermille = 0;
                Assert.True(garage.ReforgeArmorPlate("vehicle_test", Materials(), out _));
                return new SystemTextJsonSerializer().Serialize(garage.CaptureState());
            }
            Assert.Equal(Run(), Run());
        }

        [Fact]
        public void T25_ExistingModsComposeBeforeArmorAndRadiationIsSeparate()
        {
            var garage = Garage();
            var modJson = File.ReadAllText(Path.Combine(DataDir(), "vehicle_modifications.json"));
            garage.LoadCatalog(VehicleGarageCatalogLoader.Load(modJson, new SystemTextJsonSerializer()));
            var inventory = Materials();
            Assert.True(garage.InstallModification("vehicle_test", "protection", "vmod_reinforced_bullbar", inventory, out _));
            Assert.True(garage.InstallArmorGrade("vehicle_test", "grade_2_sheet_plate", inventory, out _));
            var record = garage.GetRecord("vehicle_test")!;
            garage.RecordTripWear("vehicle_test", 60f);
            Assert.Equal(76, record.chassisStressPermille);
            Assert.Equal(50, garage.GetEffectiveRadiationProtectionPermille("vehicle_test"));
        }

        [Fact]
        public void T26_GetArmorProfileIsTotalAndUnknownInstallRefuses()
        {
            var garage = Garage();
            Assert.True(garage.GetArmorProfile("unknown").IsDefault);
            Assert.True(garage.GetArmorProfile("naval_style_profile").IsDefault);
            garage.VehicleTerrainResolver = id => id == "known" ? "road" : null;
            Assert.False(garage.InstallArmorGrade("unknown", "grade_1_scrap_plate", Materials(), out string reason));
            Assert.Contains("not found", reason);
        }

        [Fact]
        public void Soak_ThirtyDaysKeepsIntegrityBoundedAndWearMonotonic()
        {
            var stock = Garage();
            var g1 = Garage();
            var g2 = Garage();
            var g4 = Garage();
            Assert.True(g1.InstallArmorGrade("vehicle", "grade_1_scrap_plate", Materials(), out _));
            Assert.True(g2.InstallArmorGrade("vehicle", "grade_2_sheet_plate", Materials(), out _));
            Assert.True(g4.InstallArmorGrade("vehicle", "grade_4_alloyed_heavy_plate", Materials(), out _));
            for (int day = 0; day < 30; day++)
            {
                stock.RecordTripWear("vehicle", 20f);
                g1.RecordTripWear("vehicle", 20f);
                g2.RecordTripWear("vehicle", 20f);
                g4.RecordTripWear("vehicle", 20f);
                Assert.InRange(g1.GetArmorProfile("vehicle").IntegrityPermille, 0, g1.GetArmorProfile("vehicle").IntegrityMaxPermille);
                Assert.InRange(g2.GetArmorProfile("vehicle").IntegrityPermille, 0, g2.GetArmorProfile("vehicle").IntegrityMaxPermille);
                Assert.InRange(g4.GetArmorProfile("vehicle").IntegrityPermille, 0, g4.GetArmorProfile("vehicle").IntegrityMaxPermille);
            }
            Assert.True(stock.GetRecord("vehicle")!.chassisStressPermille >= g1.GetRecord("vehicle")!.chassisStressPermille);
            Assert.True(g1.GetRecord("vehicle")!.chassisStressPermille >= g2.GetRecord("vehicle")!.chassisStressPermille);
            Assert.True(g2.GetRecord("vehicle")!.chassisStressPermille >= g4.GetRecord("vehicle")!.chassisStressPermille);
        }
    }
}
