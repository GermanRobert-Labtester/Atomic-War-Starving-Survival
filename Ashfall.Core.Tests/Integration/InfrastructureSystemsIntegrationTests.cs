// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public sealed class InfrastructureSystemsIntegrationTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            candidate = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void FullInfrastructureSystemsCycle_Trade_Power_Defense_Surgery_SaveRoundTrip()
        {
            string dataDir = GetDataDir();

            // 1. Load authoritative definitions from JSON catalogs
            var routes = CaravanTradeRouteCatalogLoader.Load(dataDir);
            var procedures = SurgicalProcedureCatalogLoader.Load(dataDir);
            var subgridNodes = PowerSubgridCatalogLoader.Load(dataDir);
            var defenseDefs = PerimeterDefenseCatalogLoader.Load(dataDir);

            Assert.Equal(10, routes.Count);
            Assert.Equal(10, procedures.Count);
            Assert.Equal(12, subgridNodes.Count);
            Assert.Equal(8, defenseDefs.Count);

            // 2. Shared shelter inventory and deterministic RNG seeds
            var shelterInventory = new Inventory.Inventory { MaxWeight = 2000f, Capacity = 100 };
            var rng = new SeededRng(1986);

            // Initialize systems
            var caravanSystem = new CaravanTradeNetworkSystem(routes, shelterInventory, rng);
            var surgicalSystem = new AdvancedSurgicalWardSystem(procedures, shelterInventory, rng);
            var powerSubgridSystem = new PowerDistributionSubgridSystem(subgridNodes, shelterInventory, rng);
            var perimeterDefenseSystem = new PerimeterDefenseSystem(defenseDefs, shelterInventory, rng);

            // Give shelter starting goods to barter
            shelterInventory.TryProduce("scrap_metal", 50);
            shelterInventory.TryProduce("scrap_wood", 30);
            shelterInventory.TryProduce("clean_water", 10);
            shelterInventory.TryProduce("surgical_scalpel", 1); // Surgeon tool

            // ── PHASE 1: Caravan Arrival & Barter ─────────────────────────
            var manifest = caravanSystem.ScheduleCaravan("route_compact_supply_line", currentDay: 1);
            for (int d = 1; d <= 6; d++)
            {
                caravanSystem.TickDay(d);
            }

            Assert.Equal(CaravanStatus.Arrived, manifest.status);
            manifest.stocks["anesthetic_ether"] = 10;
            manifest.stocks["sterile_gauze"] = 20;
            manifest.stocks["machine_oil"] = 5;
            manifest.stocks["electrical_wire"] = 15;
            manifest.stocks["copper_fuse"] = 4;
            manifest.stocks["sandbags"] = 10;
            manifest.stocks["ammo_556"] = 100;

            // Barter: trade 20 scrap_metal and 10 scrap_wood for medical & infrastructure supplies
            var barterRes = caravanSystem.ExecuteBarter(
                manifest.manifest_id,
                playerOffered: new Dictionary<string, int>
                {
                    { "scrap_metal", 20 },
                    { "scrap_wood", 10 }
                },
                playerRequested: new Dictionary<string, int>
                {
                    { "anesthetic_ether", 2 },
                    { "sterile_gauze", 4 },
                    { "machine_oil", 1 },
                    { "electrical_wire", 4 },
                    { "sandbags", 4 },
                    { "ammo_556", 50 }
                });

            // Adjust values if needed: offered: 20*3 + 10*2 = 80
            // requested: 2*25 + 4*6 + 1*18 + 4*8 + 4*5 + 50*8.4 = 50 + 24 + 18 + 32 + 20 + 420 = 564
            // So shelter needs to provide sufficient value!
            if (!barterRes.Success)
            {
                // Provide high value surplus
                shelterInventory.TryProduce("clean_water", 200); // 200 * 4 = 800 value
                barterRes = caravanSystem.ExecuteBarter(
                    manifest.manifest_id,
                    playerOffered: new Dictionary<string, int> { { "clean_water", 150 } },
                    playerRequested: new Dictionary<string, int>
                    {
                        { "anesthetic_ether", 2 },
                        { "sterile_gauze", 4 },
                        { "machine_oil", 1 },
                        { "electrical_wire", 8 },
                        { "sandbags", 4 },
                        { "ammo_556", 50 }
                    });
            }

            Assert.True(barterRes.Success);
            Assert.True(shelterInventory.CountById("anesthetic_ether") >= 2);
            Assert.True(shelterInventory.CountById("sterile_gauze") >= 4);
            Assert.True(shelterInventory.CountById("machine_oil") >= 1);
            Assert.True(shelterInventory.CountById("electrical_wire") >= 8);
            Assert.True(shelterInventory.CountById("sandbags") >= 4);
            Assert.True(shelterInventory.CountById("ammo_556") >= 50);

            // ── PHASE 2: Subgrid Power & Transformer Maintenance ─────────
            // Apply heavy load to workshop feed -> thermal stress
            powerSubgridSystem.ApplyRoomLoad("room_workshop", 4400f);
            var workshopNode = powerSubgridSystem.FindNodeForRoom("room_workshop");
            Assert.NotNull(workshopNode);
            Assert.True(workshopNode.temperature_celsius > PowerDistributionSubgridSystem.AmbientTemperatureCelsius);

            // Service workshop transformer using newly acquired oil and wire
            var maintRes = powerSubgridSystem.PerformTransformerMaintenance("node_workshop_feed");
            Assert.True(maintRes.IsSuccess);
            Assert.Equal(100.0f, workshopNode.transformer_oil_condition);
            Assert.Equal(PowerDistributionSubgridSystem.AmbientTemperatureCelsius, workshopNode.temperature_celsius);

            // Confirm perimeter sentry relay is powered
            Assert.True(powerSubgridSystem.IsRoomPowered("room_airlock"));

            // ── PHASE 3: Perimeter Construction & Defense ─────────────────
            // Construct sandbag berm and 5.56mm sentry turret
            shelterInventory.TryProduce("scrap_metal", 20); // ensure scrap metal available
            shelterInventory.TryProduce("scrap_wood", 10);

            var bermRes = perimeterDefenseSystem.ConstructEmplacement("def_sandbag_berm");
            Assert.True(bermRes.IsSuccess);

            var turretRes = perimeterDefenseSystem.ConstructEmplacement("def_sentry_turret_556");
            Assert.True(turretRes.IsSuccess);

            var turretEmp = perimeterDefenseSystem.Emplacements.FirstOrDefault(e => e.defense_id == "def_sentry_turret_556");
            Assert.NotNull(turretEmp);

            // Load turret with acquired ammo
            var loadRes = perimeterDefenseSystem.LoadAmmo(turretEmp.emplacement_id, 50);
            Assert.True(loadRes.IsSuccess);
            Assert.Equal(50, turretEmp.loaded_ammo_count);

            // Simulate raider assault: turret is powered via subgrid
            var assaultRes = perimeterDefenseSystem.SimulateRaiderAssault(
                raiderStrength: 12,
                isNight: false,
                isEmplacementPowered: id => powerSubgridSystem.IsRoomPowered("room_airlock"));

            Assert.True(assaultRes.Repelled);
            Assert.False(assaultRes.Breached);
            Assert.True(assaultRes.RoundsFiredTotal > 0);
            Assert.True(turretEmp.barrel_wear_percent > 0f);

            // ── PHASE 4: Major Trauma Surgery & Recovery ──────────────────
            // Defender suffered abdominal trauma requiring exploratory laparotomy
            // Requires 2 anesthetic_ether and 4 sterile_gauze (both in inventory from barter)
            var surgVal = surgicalSystem.ValidatePreOp("survivor_guard", "survivor_medic", "surg_exploratory_laparotomy");
            Assert.True(surgVal.IsSuccess);

            var surgStart = surgicalSystem.StartOperation("survivor_guard", "survivor_medic", "surg_exploratory_laparotomy");
            Assert.True(surgStart.IsSuccess);
            Assert.Single(surgicalSystem.ActiveOperations);

            var activeOp = surgicalSystem.ActiveOperations[0];
            for (int h = 0; h < activeOp.total_duration_hours; h++)
            {
                surgicalSystem.TickOperationHour(activeOp);
            }

            Assert.True(activeOp.is_completed);
            Assert.True(activeOp.patient_survived);
            Assert.Single(surgicalSystem.RecoveryPatients);
            Assert.Equal(4, surgicalSystem.RecoveryPatients[0].recovery_days_remaining);

            // Autoclave sterilization
            shelterInventory.TryProduce("clean_water", 1);
            var autoRes = surgicalSystem.RunAutoclaveCycle(hasPower: powerSubgridSystem.IsRoomPowered("room_clinic"));
            Assert.True(autoRes.IsSuccess);
            Assert.Equal(100.0f, surgicalSystem.SterileFieldPercent);

            // ── PHASE 5: Save / Reload / Resume Determinism ───────────────
            var caravanSave = caravanSystem.CaptureState();
            var surgerySave = surgicalSystem.CaptureState();
            var subgridSave = powerSubgridSystem.CaptureState();
            var defenseSave = perimeterDefenseSystem.CaptureState();

            Assert.NotNull(caravanSave);
            Assert.NotNull(surgerySave);
            Assert.NotNull(subgridSave);
            Assert.NotNull(defenseSave);

            // Create fresh systems
            var caravanSystemB = new CaravanTradeNetworkSystem(routes, new Inventory.Inventory(), new SeededRng(2026));
            var surgerySystemB = new AdvancedSurgicalWardSystem(procedures, new Inventory.Inventory(), new SeededRng(2026));
            var subgridSystemB = new PowerDistributionSubgridSystem(subgridNodes, new Inventory.Inventory(), new SeededRng(2026));
            var defenseSystemB = new PerimeterDefenseSystem(defenseDefs, new Inventory.Inventory(), new SeededRng(2026));

            caravanSystemB.RestoreState(caravanSave);
            surgerySystemB.RestoreState(surgerySave);
            subgridSystemB.RestoreState(subgridSave);
            defenseSystemB.RestoreState(defenseSave);

            // Verify state parity
            Assert.Single(surgerySystemB.RecoveryPatients);
            Assert.Equal(4, surgerySystemB.RecoveryPatients[0].recovery_days_remaining);
            Assert.Equal(100.0f, surgerySystemB.SterileFieldPercent);

            var turretB = defenseSystemB.FindEmplacement(turretEmp.emplacement_id);
            Assert.NotNull(turretB);
            Assert.Equal(turretEmp.loaded_ammo_count, turretB.loaded_ammo_count);
            Assert.Equal(turretEmp.barrel_wear_percent, turretB.barrel_wear_percent);

            var workshopNodeB = subgridSystemB.FindNode("node_workshop_feed");
            Assert.NotNull(workshopNodeB);
            Assert.Equal(100.0f, workshopNodeB.transformer_oil_condition);

            // Advance day by 1 on restored system
            surgerySystemB.TickDay(7);
            Assert.Equal(3, surgerySystemB.RecoveryPatients[0].recovery_days_remaining); // 4 -> 3
        }
    }
}
