// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public sealed class Plans146_149IntegrationTests
    {
        /// <summary>All-or-nothing inventory host: validates every line, then consumes.</summary>
        private static Func<IReadOnlyList<InventoryDemand>, bool> MakeConsumer(Dictionary<string, int> inventory)
        {
            return demands =>
            {
                foreach (var d in demands)
                {
                    if (!inventory.TryGetValue(d.ItemId, out int current) || current < d.Quantity)
                        return false;
                }
                foreach (var d in demands)
                {
                    inventory[d.ItemId] -= d.Quantity;
                }
                return true;
            };
        }

        [Fact]
        public void ScenarioA_EbPvdCoatingFullLifecycle_ConsumesInventoryProducesThermalBarrierToleratesBrownout()
        {
            var engine = new EbPvdCoatingEngine();
            var inv = new Dictionary<string, int>
            {
                { "item_ebpvd_ceramic_target_ingot", 2 },
                { "item_mcraly_bond_coat_powder", 2 }
            };
            var consume = MakeConsumer(inv);

            // Start job for 7YSZ thermal barrier coating
            bool started = engine.StartJob("job_alpha", "ebpvd_tbc_yttria_stabilized_zirconia", "superalloy_blade", "op_engineer", 2, true, consume, out string err);
            Assert.True(started, err);
            Assert.Equal(1, inv["item_ebpvd_ceramic_target_ingot"]);
            Assert.Equal(1, inv["item_mcraly_bond_coat_powder"]);

            var rng = new SeededRng(14601);

            // Phase 1: Brownout occurs (available power 2kW < required 10kW)
            var lowPower = PowerSupplyContext.Throttled(2.0f, 10.0f);
            engine.Tick(1.0f, lowPower, null, rng);
            Assert.Equal(ProcessState.Paused, engine.State);
            Assert.Equal(0f, engine.StateDto.ActiveJob!.ProgressHours);

            // Phase 2: Full power restored (15kW available > 10kW required)
            engine.ResumeJob();
            var fullPower = PowerSupplyContext.Full(15.0f);
            // Required time is 4.0 hours for this recipe
            engine.Tick(4.5f, fullPower, null, rng);

            Assert.Equal(ProcessState.Completed, engine.State);
            Assert.NotNull(engine.StateDto.ActiveJob);
            Assert.Equal(ProcessState.Completed, engine.StateDto.ActiveJob!.Status);
            Assert.Single(engine.StateDto.CompletedRecords);

            var record = engine.StateDto.CompletedRecords[0];
            Assert.Equal("superalloy_blade", record.SubstrateTag);
            Assert.Equal(125.0f, record.ThicknessUm);
            Assert.True(record.MaxTempBonusC >= 150.0f);
            Assert.True(record.SpallationRisk01 < 0.20f);
        }

        [Fact]
        public void ScenarioB_MicrofluidicDiagnostics_FabricationAndProbabilisticAssayWithoutDiseaseLeakage()
        {
            var engine = new MicrofluidicDiagnosticEngine();
            var inv = new Dictionary<string, int>
            {
                { "item_pdms_silicone_kit", 2 },
                { "item_assay_reagent_pack", 2 }
            };
            var consume = MakeConsumer(inv);

            // 1. Soft-lithography cartridge fabrication
            bool fabStarted = engine.StartCartridgeManufacturing("fab_job_01", "microfluidic_assay_cholera", "op_chemist", consume, out string fabErr);
            Assert.True(fabStarted, fabErr);

            var power = PowerSupplyContext.Full(5.0f);
            var rng = new SeededRng(14801);

            // Tick for 65 minutes (PDMS cure takes 60.0 mins)
            engine.Tick(65f, power, null, rng, null);
            Assert.Equal(ProcessState.Completed, engine.StateDto.ActiveManufacturingJob!.Status);
            Assert.Equal(1, engine.StateDto.CartridgesManufactured);

            // In inventory, add the fabricated cartridge
            inv["item_microfluidic_cartridge_general"] = 1;

            // 2. Consume manufactured cartridge to run diagnostic assay on patient
            bool runStarted = engine.StartDiagnosticRun("run_pat_01", "survivor_bob", "microfluidic_assay_cholera", "op_medic", 2, 100f, (id, qty) =>
            {
                if (inv.TryGetValue(id, out int count) && count >= qty)
                {
                    inv[id] -= qty;
                    return true;
                }
                return false;
            }, out string runErr);
            Assert.True(runStarted, runErr);
            Assert.Equal(0, inv["item_microfluidic_cartridge_general"]);

            // Tick assay duration (30 minutes)
            // Query callback evaluates patient's actual hidden infection without modifying disease progression
            bool hasInfectionQueried = false;
            engine.Tick(35f, power, null, rng, (patId, disId) =>
            {
                if (patId == "survivor_bob" && disId == "disease_cholera")
                {
                    hasInfectionQueried = true;
                    return true; // infected
                }
                return false;
            });

            Assert.True(hasInfectionQueried);
            Assert.Empty(engine.StateDto.ActiveRuns);
            Assert.Single(engine.StateDto.CompletedResults);

            var result = engine.StateDto.CompletedResults[0];
            Assert.Equal("survivor_bob", result.PatientId);
            Assert.Equal("microfluidic_assay_cholera", result.AssayId);
            Assert.Equal(DiagnosticResultKind.Positive, result.ResultKind);
            Assert.InRange(result.Confidence01, 0.5f, 0.99f);
        }

        [Fact]
        public void ScenarioC_MineClearingFlail_BreachesContaminatedSectorAndReducesExpeditionHazard()
        {
            var routeSystem = new RouteInfrastructureSystem();
            routeSystem.RegisterMinefield("expedition_corridor_north", "seg_mine_gap", density01: 0.85f, day: 5);

            var segBefore = routeSystem.FindSegment("expedition_corridor_north", "seg_mine_gap");
            Assert.NotNull(segBefore);
            Assert.Equal("unbreached", segBefore.ClearanceState);
            Assert.Equal(0.85f, segBefore.MinefieldState.ResidualRisk01);

            var flail = new MineClearingFlailEngine();
            bool breachOk = flail.StartBreach("expedition_corridor_north", "seg_mine_gap", "op_driver", 2, 1200f, routeSystem, out string err);
            Assert.True(breachOk, err);
            Assert.Equal(ProcessState.Running, flail.State);

            var rng = new SeededRng(14701);

            // Tick flail mechanical advance (1200m at 8 km/h takes ~0.15h = 9 minutes)
            flail.TickBreach(0.20f, 5, routeSystem, null, rng);

            Assert.Equal(ProcessState.Completed, flail.State);
            Assert.NotNull(flail.StateDto.ActiveBreach);
            Assert.Equal(ProcessState.Completed, flail.StateDto.ActiveBreach!.Status);

            var segAfter = routeSystem.FindSegment("expedition_corridor_north", "seg_mine_gap");
            Assert.NotNull(segAfter);
            Assert.Equal(1.0f, segAfter.MinefieldState.ClearedFraction01);
            Assert.Equal("cleared", segAfter.ClearanceState);
            Assert.True(segAfter.MinefieldState.ResidualRisk01 <= 0.06f);

            // Flail hardware wear check
            Assert.True(flail.StateDto.TotalClearedDistanceKm >= 1.2f);
            Assert.True(flail.StateDto.BlastShieldIntegrity01 < 1.0f);
        }

        [Fact]
        public void ScenarioD_RailGrinding_ReprofilesTrackDecreasesDerailmentRiskAndIncreasesSpeedLimit()
        {
            var routeSystem = new RouteInfrastructureSystem();
            // High roughness corridor with strict 25 km/h restriction
            routeSystem.RegisterRailSegment("rail_trunk_iron_vein", "sector_deep_quarry", initialRoughness: 0.90f, safeSpeedKph: 25.0f, day: 8);

            var segBefore = routeSystem.FindSegment("rail_trunk_iron_vein", "sector_deep_quarry");
            Assert.NotNull(segBefore);
            Assert.Equal(0.90f, segBefore.RailCondition.RoughnessIndex);
            Assert.Equal(25.0f, segBefore.RailCondition.SafeSpeedLimitKph);

            var grinder = new RailGrindingEngine();
            bool jobOk = grinder.StartGrindingJob("rail_trunk_iron_vein", "sector_deep_quarry", "op_machinist", 8.0f, routeSystem, out string err);
            Assert.True(jobOk, err);
            Assert.Equal(ProcessState.Running, grinder.State);

            var rng = new SeededRng(14901);
            // At 4.0 km/h, 8 km pass takes 2.0 hours
            grinder.TickGrinding(2.5f, 8, routeSystem, null, rng);

            Assert.Equal(ProcessState.Completed, grinder.State);
            Assert.NotNull(grinder.StateDto.ActiveJob);
            Assert.Equal(ProcessState.Completed, grinder.StateDto.ActiveJob!.Status);

            var segAfter = routeSystem.FindSegment("rail_trunk_iron_vein", "sector_deep_quarry");
            Assert.NotNull(segAfter);
            Assert.True(segAfter.RailCondition.RoughnessIndex < 0.70f);
            Assert.True(segAfter.RailCondition.SafeSpeedLimitKph > 35.0f);

            // Grinding stone diameter worn down
            Assert.True(grinder.StateDto.StoneDiameterMm < 250.0f);
        }

        [Fact]
        public void ScenarioE_DeterministicSimulationAndCrossSectionStateRoundtrip()
        {
            // Seed determinism verification
            var rng1 = new SeededRng(777);
            var rng2 = new SeededRng(777);

            var flail1 = new MineClearingFlailEngine();
            var flail2 = new MineClearingFlailEngine();
            var route1 = new RouteInfrastructureSystem();
            var route2 = new RouteInfrastructureSystem();

            route1.RegisterMinefield("rt_test", "sg_01", 0.75f, day: 1);
            route2.RegisterMinefield("rt_test", "sg_01", 0.75f, day: 1);

            flail1.StartBreach("rt_test", "sg_01", "op_1", 1, 1000f, route1, out _);
            flail2.StartBreach("rt_test", "sg_01", "op_1", 1, 1000f, route2, out _);

            flail1.TickBreach(0.1f, 1, route1, null, rng1);
            flail2.TickBreach(0.1f, 1, route2, null, rng2);

            Assert.Equal(flail1.StateDto.ChainLinksRemaining, flail2.StateDto.ChainLinksRemaining);
            Assert.Equal(flail1.StateDto.BlastShieldIntegrity01, flail2.StateDto.BlastShieldIntegrity01);
            Assert.Equal(route1.FindSegment("rt_test", "sg_01")!.MinefieldState.ClearedFraction01,
                         route2.FindSegment("rt_test", "sg_01")!.MinefieldState.ClearedFraction01);

            // State Capture / Restore Roundtrip
            var flailSavedState = flail1.CaptureState();
            var restoredFlail = new MineClearingFlailEngine();
            restoredFlail.RestoreState(flailSavedState);

            Assert.Equal(flail1.StateDto.ChainLinksRemaining, restoredFlail.StateDto.ChainLinksRemaining);
            Assert.Equal(flail1.StateDto.TotalClearedDistanceKm, restoredFlail.StateDto.TotalClearedDistanceKm);
            Assert.Equal(flail1.StateDto.ActiveBreach?.ProgressMeters, restoredFlail.StateDto.ActiveBreach?.ProgressMeters);

            var routeSavedState = route1.CaptureState();
            var restoredRoute = new RouteInfrastructureSystem();
            restoredRoute.RestoreState(routeSavedState);

            var restoredSeg = restoredRoute.FindSegment("rt_test", "sg_01");
            Assert.NotNull(restoredSeg);
            Assert.Equal(route1.FindSegment("rt_test", "sg_01")!.MinefieldState.ClearedFraction01,
                         restoredSeg.MinefieldState.ClearedFraction01);
        }

        /// <summary>
        /// Plans 146–149 MED: coated part mint does not auto-buff PowerGrid;
        /// explicit install publishes under ebpvd_installed with family slots.
        /// </summary>
        [Fact]
        public void ScenarioG_CoatedPartInstall_RequiredForPowerGridContribution()
        {
            var grid = new PowerGridSystem(
                new PowerGridState
                {
                    GenerationWatts = 800f,
                    FuelUnits = 100f,
                    BatteryCapacityWh = 4000f,
                    BatteryReserveWh = 2000f
                },
                new List<PowerGridRoom>
                {
                    new PowerGridRoom("room_test", "Test", 100f, PowerGridRoomPriority.Standard)
                },
                new SeededRng(14649));

            float before = grid.GenerationWatts;
            // Simulate host: consume inventory then install (mint alone changes nothing).
            Assert.True(grid.TryInstallCoatedPart("item_coated_turbine_blade", out string err), err);
            Assert.Equal(before + 40f, grid.GenerationWatts, 2);
            Assert.Equal(40f, grid.GenerationContributions[PowerGridSystem.EbPvdInstalledSourceId], 2);

            var restored = new PowerGridSystem(
                new PowerGridState
                {
                    GenerationWatts = 800f,
                    FuelUnits = 100f,
                    BatteryCapacityWh = 4000f,
                    BatteryReserveWh = 2000f
                },
                new List<PowerGridRoom>
                {
                    new PowerGridRoom("room_test", "Test", 100f, PowerGridRoomPriority.Standard)
                },
                new SeededRng(14649));
            restored.RestoreState(grid.CaptureState());
            Assert.Equal(40f, restored.GenerationContributions[PowerGridSystem.EbPvdInstalledSourceId], 2);
        }

        /// <summary>
        /// Plans 146–149 MED: route travel modifier stretch formula used by
        /// estimate preview and live Start must agree (ceil, no catalog mutate).
        /// </summary>
        [Fact]
        public void ScenarioH_RouteTravelStretch_EstimateParityFormula()
        {
            var routes = new RouteInfrastructureSystem();
            routes.RegisterRailSegment("expedition_corridor_north", "seg_mine_gap", 0.9f, 25f, 1);
            float mult = routes.GetTravelModifier("expedition_corridor_north");
            Assert.True(mult > 1f);

            var catalog = new ExpeditionDefinition
            {
                id = "expedition_corridor_north",
                displayName = "North Corridor",
                distanceTicks = 8
            };
            var forStart = ExpeditionTravelStretch.ProjectDefinition(catalog, mult);
            var forEstimate = ExpeditionTravelStretch.ProjectDefinition(catalog, mult);
            Assert.Equal(forEstimate.distanceTicks, forStart.distanceTicks);
            Assert.Equal(8, catalog.distanceTicks);
            Assert.True(forStart.distanceTicks > 8);
        }

        /// <summary>
        /// Pins the daily-cadence tick contract the host wires (Main.TickPlans146To149):
        /// one 8-hour machine shift per campaign day, with projected power, advances
        /// every system. This is the regression that caught the empty tick stub.
        /// </summary>
        [Fact]
        public void ScenarioF_DailyShiftCadence_AdvancesAllFourSystems()
        {
            const float shiftHours = 8f;

            // EB-PVD: 4h recipe resolves within one shift
            var ebpvd = new EbPvdCoatingEngine();
            ebpvd.StartJob("shift_job", "ebpvd_tbc_yttria_stabilized_zirconia", "superalloy_blade", "op_1", 1, true, demands => true, out _);
            ebpvd.Tick(shiftHours, PowerSupplyContext.Full(15f), null, new SeededRng(11));
            Assert.Equal(ProcessState.Completed, ebpvd.State);

            // Microfluidic: manufacturing (60 min) + assay (30 min) resolve in one shift
            var micro = new MicrofluidicDiagnosticEngine();
            micro.StartCartridgeManufacturing("shift_fab", "microfluidic_assay_cholera", "op_1", demands => true, out _);
            micro.Tick(shiftHours * 60f, PowerSupplyContext.Full(5f), null, new SeededRng(12), null);
            Assert.Equal(ProcessState.Completed, micro.StateDto.ActiveManufacturingJob!.Status);
            micro.StartDiagnosticRun("shift_run", "survivor_01", "microfluidic_assay_cholera", "op_1", 1, 0f, (_, _) => true, out _);
            micro.Tick(shiftHours * 60f, PowerSupplyContext.Full(5f), null, new SeededRng(13), (_, _) => true);
            Assert.Single(micro.StateDto.CompletedResults);

            // Flail: 1000m at ~8 km/h clears inside one shift
            var routes = new RouteInfrastructureSystem();
            routes.RegisterMinefield("shift_corridor", "seg_1", 0.8f, day: 1);
            var flail = new MineClearingFlailEngine();
            flail.StartBreach("shift_corridor", "seg_1", "op_1", 1, 1000f, routes, out _);
            flail.TickBreach(shiftHours, 1, routes, null, new SeededRng(14));
            Assert.Equal(ProcessState.Completed, flail.State);

            // Rail grinder: 50km at ~4 km/h needs two shifts — one shift advances, never regresses
            routes.RegisterRailSegment("shift_rail", "sector_1", 0.85f, 25f, 1);
            var grinder = new RailGrindingEngine();
            grinder.StartGrindingJob("shift_rail", "sector_1", "op_1", 50f, routes, out _);
            grinder.TickGrinding(shiftHours, 1, routes, null, new SeededRng(15));
            float progressOneShift = grinder.StateDto.ActiveJob!.ProgressKm;
            Assert.True(progressOneShift > 0f);
            Assert.True(progressOneShift < 50f, "one shift should not finish a 50 km corridor");
            grinder.TickGrinding(shiftHours, 2, routes, null, new SeededRng(15));
            Assert.True(grinder.StateDto.ActiveJob!.ProgressKm > progressOneShift);
            Assert.Equal(ProcessState.Completed, grinder.State);
        }
    }
}
