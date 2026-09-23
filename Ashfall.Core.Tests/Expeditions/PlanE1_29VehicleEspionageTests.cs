// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.Inventory;
using Xunit;
using InventoryModel = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Expeditions
{
    public sealed class PlanE1_29VehicleEspionageTests
    {
        private static VehicleGarageCatalog CreateVehicleCatalog() => new VehicleGarageCatalog
        {
            schema_version = 1,
            modifications = new List<VehicleModificationDefinition>
            {
                new VehicleModificationDefinition
                {
                    id = "mod_armored_plating",
                    slot_type = "protection",
                    install_cost = new List<VehicleModificationCost>
                    {
                        new VehicleModificationCost { item_id = "scrap_metal", amount = 10 }
                    },
                    effects = new VehicleModificationEffects
                    {
                        speed_multiplier_delta = -0.1f,
                        radiation_protection_permille = 400,
                        wear_rate_multiplier = 0.8f
                    }
                }
            }
        };

        private static FactionIntelligenceCatalog CreateEspionageCatalog() => new FactionIntelligenceCatalog
        {
            schema_version = 1,
            operations = new List<FactionOperationDefinition>
            {
                new FactionOperationDefinition
                {
                    id = "fop_infiltrate_convoy",
                    display_name = "Convoy Infiltration",
                    target_faction = "iron_raiders",
                    operation_class = "recon",
                    target_subsystem = "patrol"
                }
            }
        };

        [Fact]
        public void VehicleGarage_InstallModification_ConsumesPartsAndDecoratesProfile()
        {
            var inv = new InventoryModel();
            inv.AddById("scrap_metal", 50);
            var catalog = CreateVehicleCatalog();
            var garage = new VehicleGarageSystem(catalog, new SeededRng(50));

            bool installed = garage.InstallModification("hauler_1", "protection", "mod_armored_plating", inv, out string reason);
            Assert.True(installed, reason);
            Assert.Equal(40, inv.CountById("scrap_metal")); // 10 consumed

            var profile = new ExpeditionVehicleProfile
            {
                vehicleId = "hauler_1",
                speedMultiplier = 1.0f
            };
            garage.DecorateProfile(profile);
            Assert.Equal(0.9f, profile.speedMultiplier, 3);
        }

        [Fact]
        public void ShelterEspionage_EnrollSleeperAndInvestigateSuspect()
        {
            var system = new ShelterEspionageSystem(CreateEspionageCatalog(), new SeededRng(101));

            bool enrolled = system.EnrollSleeperAgent("survivor_mole", "iron_raiders", 750);
            Assert.True(enrolled);
            Assert.True(system.IsSleeperAgent("survivor_mole"));

            system.AddSuspicion("survivor_mole", 600);
            var rec = system.GetSleeperRecord("survivor_mole");
            Assert.NotNull(rec);
            Assert.Equal(600, rec.suspicionPermille);

            bool investigated = system.InvestigateSuspect("survivor_mole", 80, out bool unmasked, out string report);
            Assert.True(investigated);
            Assert.True(unmasked);
            Assert.Contains("verified as active operative", report);
        }

        [Fact]
        public void VehicleAndEspionage_CaptureAndRestore_PreservesState()
        {
            var inv = new InventoryModel();
            inv.AddById("scrap_metal", 100);
            var garage = new VehicleGarageSystem(CreateVehicleCatalog(), new SeededRng(50));
            garage.InstallModification("hauler_2", "protection", "mod_armored_plating", inv, out _);

            var espionage = new ShelterEspionageSystem(CreateEspionageCatalog(), new SeededRng(101));
            espionage.EnrollSleeperAgent("operative_x", "sun_seekers", 500);

            var gState = garage.CaptureState();
            var eState = espionage.CaptureState();

            Assert.NotEmpty(gState.vehicleRecords);
            Assert.Single(eState.sleeperAgents);

            var restoredGarage = new VehicleGarageSystem(CreateVehicleCatalog(), new SeededRng(50));
            var restoredEspionage = new ShelterEspionageSystem(CreateEspionageCatalog(), new SeededRng(101));

            restoredGarage.RestoreState(gState);
            restoredEspionage.RestoreState(eState);

            Assert.True(restoredGarage.GetInstalledSlots("hauler_2").ContainsKey("protection"));
            Assert.True(restoredEspionage.IsSleeperAgent("operative_x"));
        }
    }
}
