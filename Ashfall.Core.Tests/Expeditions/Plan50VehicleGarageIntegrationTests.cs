// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Xunit;
using InventoryModel = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Wave 8 B2 — Plan 50 garage completion contract: fitted modifications
    /// decorate the expedition profile, trip distance accumulates component
    /// wear, catastrophic wear immobilizes, and recovery advances over days.
    /// </summary>
    public sealed class Plan50VehicleGarageIntegrationTests
    {
        private static VehicleGarageCatalog Catalog() => new VehicleGarageCatalog
        {
            schema_version = 1,
            modifications = new List<VehicleModificationDefinition>
            {
                new VehicleModificationDefinition
                {
                    id = "vmod_test_cargo",
                    slot_type = "cargo",
                    install_cost = new List<VehicleModificationCost>
                    {
                        new VehicleModificationCost { item_id = "scrap_metal", amount = 5 }
                    },
                    effects = new VehicleModificationEffects
                    {
                        cargo_capacity_delta = 50f,
                        fuel_consumption_multiplier = 1.1f,
                        wear_rate_multiplier = 1.0f
                    }
                },
                new VehicleModificationDefinition
                {
                    id = "vmod_test_armor",
                    slot_type = "protection",
                    install_cost = new List<VehicleModificationCost>
                    {
                        new VehicleModificationCost { item_id = "scrap_metal", amount = 5 }
                    },
                    effects = new VehicleModificationEffects
                    {
                        speed_multiplier_delta = -0.05f,
                        radiation_protection_permille = 300,
                        wear_rate_multiplier = 1.0f
                    }
                }
            }
        };

        private static (VehicleGarageSystem garage, InventoryModel inv) Fixture()
        {
            var inv = new InventoryModel();
            inv.AddById("scrap_metal", 100);
            inv.AddById("mechanical_parts", 100);
            return (new VehicleGarageSystem(Catalog(), new SeededRng(50)), inv);
        }

        [Fact]
        public void DecorateProfile_AppliesFittedCargoSpeedAndFuel()
        {
            var (garage, inv) = Fixture();
            Assert.True(garage.InstallModification("v1", "cargo", "vmod_test_cargo", inv, out string r1), r1);
            Assert.True(garage.InstallModification("v1", "protection", "vmod_test_armor", inv, out string r2), r2);

            var profile = new ExpeditionVehicleProfile
            {
                vehicleId = "v1",
                cargoCapacityKg = 100f,
                speedMultiplier = 1f,
                fuelPerTravelTick = 1f
            };
            garage.DecorateProfile(profile);

            Assert.Equal(150f, profile.cargoCapacityKg, 3);
            Assert.Equal(0.95f, profile.speedMultiplier, 3);
            Assert.Equal(1.1f, profile.fuelPerTravelTick, 3);
        }

        [Fact]
        public void DecorateProfile_NullOrUnrecorded_NoOp()
        {
            var (garage, _) = Fixture();
            garage.DecorateProfile(null);

            var profile = new ExpeditionVehicleProfile { vehicleId = "never_seen", cargoCapacityKg = 40f, speedMultiplier = 1f, fuelPerTravelTick = 1f };
            garage.DecorateProfile(profile);
            Assert.Equal(40f, profile.cargoCapacityKg, 3);
            Assert.Equal(1f, profile.speedMultiplier, 3);
        }

        [Fact]
        public void GetInstalledSlots_ReflectsInstallsAndRemoval()
        {
            var (garage, inv) = Fixture();
            garage.InstallModification("v1", "cargo", "vmod_test_cargo", inv, out _);
            Assert.True(garage.GetInstalledSlots("v1").TryGetValue("cargo", out var fitted));
            Assert.Equal("vmod_test_cargo", fitted);

            garage.UninstallModification("v1", "cargo", inv, out _);
            Assert.False(garage.GetInstalledSlots("v1").ContainsKey("cargo"));
        }

        [Fact]
        public void CatastrophicWear_Immobilizes_AndRecoveryAdvancesThenClears()
        {
            var (garage, inv) = Fixture();
            garage.RecordTripWear("v1", 1000f);
            Assert.True(garage.IsImmobilized("v1"));

            Assert.True(garage.RegisterRecoveryMission("v1", "loc_field", 10, out string missionId, out string reason), reason);
            garage.AdvanceRecoveries(119);
            Assert.False(garage.ActiveRecoveries[missionId].isComplete);
            garage.AdvanceRecoveries(1);
            Assert.True(garage.ActiveRecoveries[missionId].isComplete);

            Assert.True(garage.CompleteRecoveryMission(missionId, inv, out string completeReason), completeReason);
            Assert.False(garage.IsImmobilized("v1"));
            Assert.False(garage.ActiveRecoveries.ContainsKey(missionId));
        }

        [Fact]
        public void AdvanceRecoveries_NonPositiveDelta_NoProgress()
        {
            var (garage, _) = Fixture();
            garage.RecordTripWear("v1", 1000f);
            garage.RegisterRecoveryMission("v1", "loc_field", 10, out string missionId, out _);
            Assert.Equal(0, garage.AdvanceRecoveries(0));
            Assert.Equal(0, garage.ActiveRecoveries[missionId].progressTicks);
        }
    }
}