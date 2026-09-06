using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ShelterThermalThermodynamicsPlan57Tests
    {
        private static ShelterThermalSystem CreateSystem(out InventoryContainer inventory, out YearOfAshDeepFreezeSystem df)
        {
            var needs = new NeedsSystem();
            var sl = new StartingLevelSystem();
            df = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState { indoorTemperatureCelsius = -10f });
            inventory = new InventoryContainer();
            return new ShelterThermalSystem(new SeededRng(42), needs, sl, df);
        }

        [Fact]
        public void RetrofitInsulation_WithMaterials_UpgradesRoomAndConsumesInventory()
        {
            var thermal = CreateSystem(out var inventory, out var df);
            thermal.AddRoom("bunker_hall", "Bunker Hall", 100f, insulationFactor: 1.0f);

            // Add scrap metal to inventory for insul_scrap_panels (requires 20 scrap)
            inventory.TryProduce("item_scrap_metal", 25);

            var result = thermal.RetrofitInsulation("bunker_hall", "insul_scrap_panels", inventory);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.Equal(1, thermal.State.roomInsulationLevels["bunker_hall"]);
            Assert.Equal("insul_scrap_panels", thermal.State.roomInstalledInsulation["bunker_hall"]);
            Assert.Equal(1.25f, thermal.State.rooms[0].insulationFactor);
            Assert.Equal(5, inventory.CountById("item_scrap_metal")); // 25 - 20 = 5
        }

        [Fact]
        public void RetrofitInsulation_InsufficientMaterials_Blocks()
        {
            var thermal = CreateSystem(out var inventory, out var df);
            thermal.AddRoom("bunker_hall", "Bunker Hall", 100f);

            inventory.TryProduce("item_scrap_metal", 5); // Needs 20

            var result = thermal.RetrofitInsulation("bunker_hall", "insul_scrap_panels", inventory);
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("insufficient_materials", result.FailureCode);
        }

        [Fact]
        public void ThawPipeWithBlowtorch_RequiresTorch_ClearsFreeze()
        {
            var thermal = CreateSystem(out var inventory, out var df);
            thermal.AddRoom("room_a", "Room A", 50f);
            thermal.AddRoom("room_b", "Room B", 50f);
            thermal.AddPipe("pipe_1", "room_a", "room_b");

            var pipe = thermal.State.pipes[0];
            pipe.isFrozen = true;
            pipe.freezeScore = 75f;

            // Without blowtorch
            var blockedResult = thermal.ThawPipeWithBlowtorch("pipe_1", inventory);
            Assert.Equal(ActionResult.StatusKind.Blocked, blockedResult.Status);
            Assert.Equal("no_blowtorch", blockedResult.FailureCode);

            // Give blowtorch and fuel
            inventory.TryProduce("item_blowtorch", 1);
            inventory.TryProduce("item_fuel", 5);

            var successResult = thermal.ThawPipeWithBlowtorch("pipe_1", inventory);
            Assert.Equal(ActionResult.StatusKind.Success, successResult.Status);
            Assert.False(pipe.isFrozen);
            Assert.Equal(0f, pipe.freezeScore);
            Assert.Equal(4, inventory.CountById("item_fuel")); // 1 consumed
        }

        [Fact]
        public void AuxiliaryStoves_HeatRoomAndExpire()
        {
            var thermal = CreateSystem(out var inventory, out var df);
            thermal.AddRoom("mess_hall", "Mess Hall", 80f);
            thermal.State.rooms[0].currentTempC = -5f;

            var stove = new AuxiliaryStove
            {
                stoveId = "stove_potbelly_1",
                displayName = "Potbelly Scrap Stove",
                roomId = "mess_hall",
                heatKw = 3.5f,
                fuelItemId = "item_fuel"
            };
            var addRes = thermal.AddAuxiliaryStove("mess_hall", stove);
            Assert.Equal(ActionResult.StatusKind.Success, addRes.Status);

            inventory.TryProduce("item_fuel", 10);
            var lightRes = thermal.LightAuxiliaryStove("mess_hall", "stove_potbelly_1", burnDays: 2, inventory);
            Assert.Equal(ActionResult.StatusKind.Success, lightRes.Status);
            Assert.Equal(8, inventory.CountById("item_fuel")); // 10 - 2 = 8

            // Tick day 1
            thermal.TickDay(1);
            Assert.True(stove.isBurning);
            Assert.Equal(1, stove.daysRemaining);

            // Tick day 2
            thermal.TickDay(2);
            Assert.False(stove.isBurning);
            Assert.Equal(0, stove.daysRemaining);
        }

        [Fact]
        public void GeneratorWasteHeat_DistributesCogenerationToRadiators()
        {
            var thermal = CreateSystem(out _, out var df);
            thermal.AddRoom("rad_room", "Radiator Room", 80f, hasRadiator: true);
            thermal.State.rooms[0].currentTempC = 0f;
            thermal.State.boilerActive = false; // Boiler OFF

            thermal.SetGeneratorWasteHeat(50f, pumpActive: true);
            thermal.TickDay(1);

            // Cogeneration should provide heat, preventing deep drop to -10C
            Assert.True(thermal.State.rooms[0].currentTempC > -5f);
        }

        [Fact]
        public void TechnicianEfficiency_ReducesBoilerFuelConsumption()
        {
            // System A: low efficiency
            var sysA = CreateSystem(out _, out _);
            sysA.AddRoom("room_1", "Room 1", 80f, hasRadiator: true);
            sysA.SetBoilerActive(true, 60f);
            sysA.State.boilerFuelLevel = 50f;
            sysA.SetTechnicianEfficiency(0.7f);
            sysA.TickDay(1);
            float fuelBurnA = 50f - sysA.State.boilerFuelLevel;

            // System B: high efficiency
            var sysB = CreateSystem(out _, out _);
            sysB.AddRoom("room_1", "Room 1", 80f, hasRadiator: true);
            sysB.SetBoilerActive(true, 60f);
            sysB.State.boilerFuelLevel = 50f;
            sysB.SetTechnicianEfficiency(1.5f);
            sysB.TickDay(1);
            float fuelBurnB = 50f - sysB.State.boilerFuelLevel;

            Assert.True(fuelBurnB < fuelBurnA, $"High efficiency burn ({fuelBurnB}) should be less than low ({fuelBurnA})");
        }

        [Fact]
        public void SeismicDamage_ShearsPipesAndRadiators()
        {
            var thermal = CreateSystem(out _, out _);
            thermal.AddRoom("workshop", "Workshop", 80f, hasRadiator: true);
            thermal.SetRadiatorValve("workshop", 1.0f);
            thermal.AddPipe("conduit_1", "workshop", "bunker_core");

            var shearPipeRes = thermal.ShearPipe("conduit_1", 0.8f);
            Assert.Equal(ActionResult.StatusKind.Success, shearPipeRes.Status);
            Assert.True(thermal.State.pipes[0].hasBurst);
            Assert.Equal(0.8f, thermal.State.pipes[0].burstSeverity);

            var shearRadRes = thermal.ShearRadiatorLoop("workshop");
            Assert.Equal(ActionResult.StatusKind.Success, shearRadRes.Status);
            Assert.Equal(0f, thermal.State.rooms[0].radiatorValveOpen);

            Assert.Contains(thermal.State.incidentLog, i => i.kind == ThermalIncidentKind.SeismicShear);
            Assert.Contains(thermal.State.incidentLog, i => i.kind == ThermalIncidentKind.RadiatorRupture);
        }

        [Fact]
        public void StatePreservation_CapturesAndRestoresPlan57Fields()
        {
            var thermal = CreateSystem(out _, out _);
            thermal.AddRoom("room_a", "Room A", 100f);
            thermal.RetrofitInsulation("room_a", "insul_mineral_wool");
            thermal.SetGeneratorWasteHeat(30f, pumpActive: true);
            thermal.SetTechnicianEfficiency(1.35f);
            thermal.AddAuxiliaryStove("room_a", new AuxiliaryStove { stoveId = "s1", isBurning = true, daysRemaining = 4 });

            var state = thermal.CaptureState();

            var restored = CreateSystem(out _, out _);
            restored.RestoreState(state);

            Assert.Equal(2, restored.State.roomInsulationLevels["room_a"]);
            Assert.Equal("insul_mineral_wool", restored.State.roomInstalledInsulation["room_a"]);
            Assert.Equal(30f, restored.State.generatorWasteHeatKw);
            Assert.True(restored.State.circulationPumpActive);
            Assert.Equal(1.35f, restored.State.technicianEfficiencyMult);
            Assert.Single(restored.State.stoves);
            Assert.True(restored.State.stoves[0].isBurning);
            Assert.Equal(4, restored.State.stoves[0].daysRemaining);
        }
    }
}
