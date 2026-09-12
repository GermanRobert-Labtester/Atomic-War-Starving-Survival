// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ShelterSeismicDynamicsPlan56Tests
    {
        private static (SeismicDynamicsSystem seismic, ShelterThermalSystem thermal, ExcavationHazardSystem hazard, InventoryContainer inventory) CreateFixture(int seed = 42)
        {
            var rng = new SeededRng(seed);
            var inventory = new InventoryContainer();
            var needs = new NeedsSystem();
            var sl = new StartingLevelSystem();
            var df = new YearOfAshDeepFreezeSystem();
            var thermal = new ShelterThermalSystem(rng, needs, sl, df);
            var hazard = new ExcavationHazardSystem(inventory, rng);
            var seismic = new SeismicDynamicsSystem(rng, thermal, hazard, inventory);
            return (seismic, thermal, hazard, inventory);
        }

        [Fact]
        public void TensionAccumulation_AndEarlyWarning_FiresWhenThresholdApproached()
        {
            var (seismic, _, _, _) = CreateFixture();
            string warnedFault = string.Empty;
            float warnedRatio = 0f;

            seismic.OnEarlyWarning += (fId, ratio) =>
            {
                warnedFault = fId;
                warnedRatio = ratio;
            };

            // Set fault close to threshold (threshold = 100)
            seismic.State.faults["fault_sub_strata_rift"].currentTension = 78f;

            // Day 1 tick adds base_accumulation_rate (2.5) * depth_multiplier (1.15) = 2.875 -> 80.875
            seismic.TickDay(1);

            Assert.Equal("fault_sub_strata_rift", warnedFault);
            Assert.True(warnedRatio >= 0.80f);
            Assert.Contains("fault_sub_strata_rift", seismic.State.activeEarlyWarnings);
        }

        [Fact]
        public void EmergencyShoring_DampensTensionAccumulation()
        {
            var (seismic, _, _, inv) = CreateFixture();
            inv.TryProduce("item_scrap_metal", 20);

            var shoreRes = seismic.ApplyEmergencyShoring("fault_sub_strata_rift", durationDays: 4, inv);
            Assert.Equal(ActionResult.StatusKind.Success, shoreRes.Status);
            Assert.Equal(10, inv.CountById("item_scrap_metal")); // 20 - 10 = 10

            var fState = seismic.State.faults["fault_sub_strata_rift"];
            Assert.Equal(4, fState.temporaryShoringDays);
            Assert.Equal(0.5f, fState.temporaryShoringStrength);

            // Tick day: rate should be cut in half
            float startTension = fState.currentTension;
            seismic.TickDay(1);
            float addedTension = fState.currentTension - startTension;

            // Baseline is 2.5 * 1.15 = 2.875; with 50% damping it should be ~1.4375
            Assert.True(addedTension < 2.0f, $"Added tension ({addedTension}) should be dampened below 2.0");
        }

        [Fact]
        public void SectorReinforcement_IncreasesReinforcementLevel()
        {
            var (seismic, _, _, inv) = CreateFixture();
            inv.TryProduce("item_scrap_metal", 50);

            var res1 = seismic.ReinforceSector("sector_excavation_alpha", inv);
            Assert.Equal(ActionResult.StatusKind.Success, res1.Status);
            Assert.Equal(1, seismic.State.sectorReinforcementLevel["sector_excavation_alpha"]);
            Assert.Equal(30, inv.CountById("item_scrap_metal")); // 50 - 20 = 30

            var res2 = seismic.ReinforceSector("sector_excavation_alpha", inv);
            Assert.Equal(ActionResult.StatusKind.Success, res2.Status);
            Assert.Equal(2, seismic.State.sectorReinforcementLevel["sector_excavation_alpha"]);
            Assert.Equal(10, inv.CountById("item_scrap_metal")); // 30 - 20 = 10
        }

        [Fact]
        public void KineticShockInjection_CanForceImmediateSlip()
        {
            var (seismic, _, _, _) = CreateFixture();
            bool quakeFired = false;
            seismic.OnQuakeOccurred += _ => quakeFired = true;

            // Inject 150 MJ shock wave into sector_excavation_alpha
            seismic.InjectKineticShock(150f, "sector_excavation_alpha");

            Assert.True(quakeFired, "Kinetic shock should push tension over threshold and trigger slip");
            Assert.NotEmpty(seismic.State.recentQuakes);
        }

        [Fact]
        public void FaultSlip_RoutesDamageToPipesAndHazardMethane()
        {
            var (seismic, thermal, hazard, _) = CreateFixture();
            thermal.AddRoom("bunker_core", "Bunker Core", 100f, hasRadiator: true);
            thermal.AddRoom("room_sub", "Sub-Station", 100f, hasRadiator: true);
            thermal.AddPipe("pipe_geothermal_1", "bunker_core", "room_sub");

            bool quakeReceived = false;
            seismic.OnQuakeOccurred += q => quakeReceived = true;

            // Force tension right to threshold
            var fState = seismic.State.faults["fault_orbital_crevasse"];
            fState.currentTension = 185f; // Threshold is 180

            seismic.TickDay(1);

            Assert.True(quakeReceived);
            Assert.Equal(1, fState.totalSlips);
            Assert.True(fState.currentTension < 40f, "Tension should reset by ~85%");

            // Check hazard sector outgassing
            var secState = hazard.GetOrCreateSector("sector_excavation_alpha");
            Assert.True(secState.MethanePpm > 300, "Methane should increase in affected sector");
        }

        [Fact]
        public void StatePreservation_CapturesAndRestoresSeismicState()
        {
            var (seismic, _, _, _) = CreateFixture();
            seismic.State.faults["fault_sub_strata_rift"].currentTension = 55.5f;
            seismic.State.faults["fault_sub_strata_rift"].temporaryShoringDays = 3;
            seismic.State.sectorReinforcementLevel["sector_excavation_alpha"] = 2;
            seismic.SetSeismographStatus(false);

            var state = seismic.CaptureState();

            var (restored, _, _, _) = CreateFixture();
            restored.RestoreState(state);

            Assert.Equal(55.5f, restored.State.faults["fault_sub_strata_rift"].currentTension);
            Assert.Equal(3, restored.State.faults["fault_sub_strata_rift"].temporaryShoringDays);
            Assert.Equal(2, restored.State.sectorReinforcementLevel["sector_excavation_alpha"]);
            Assert.False(restored.State.seismographOperational);
        }
    }
}
