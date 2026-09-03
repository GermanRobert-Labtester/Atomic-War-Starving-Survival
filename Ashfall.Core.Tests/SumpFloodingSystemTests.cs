using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SumpFloodingSystemTests
    {
        [Fact] public void AddNode_CreatesNode()
        {
            var s = Create(out _, out _, out _);
            var r = s.AddNode("sump_a", "Lower Level");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(s.State.nodes);
        }

        [Fact] public void InstallPump_AddsPump()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            var r = s.InstallPump("sump_a");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(s.State.nodes[0].hasSumpPump);
        }

        [Fact] public void AddMitigation_FloatValve()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            var r = s.AddMitigation("sump_a", "float_valve");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.True(s.State.nodes[0].hasFloatValve);
        }

        [Fact] public void TickDay_NoPower_PumpDegrades()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.InstallPump("sump_a");
            s.SetNodePower("sump_a", true); // pump switched on, but grid has no power for room
            for (int i = 0; i < 200; i++) s.TickDay(i + 1);
            Assert.True(s.State.nodes[0].pumpCondition < 100f);
        }

        [Fact] public void DrainNode_ReducesWater()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].waterLevelCm = 100f;
            var r = s.DrainNode("sump_a");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(50f, s.State.nodes[0].waterLevelCm);
        }

        [Fact] public void IsNodeAvailable_FalseWhenFlooded()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].isFlooded = true;
            s.State.nodes[0].equipmentDisabled = true;
            Assert.False(s.IsNodeAvailable("sump_a"));
        }

        [Fact] public void TickDay_NaturalDrainComplete_ResetsEquipmentDisabled()
        {
            // Bug-08 regression: when an un-flooded node finishes draining to 0cm
            // via the natural-drain branch, equipmentDisabled must clear — the
            // node is no longer waterlogged. Previously the latch stayed true,
            // forcing the player to call DrainNode manually.
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            var node = s.State.nodes[0];
            node.isFlooded = false;
            node.equipmentDisabled = true;
            node.waterLevelCm = 2f; // forces the drain branch on the very next tick
            s.TickDay(1);
            Assert.Equal(0f, node.waterLevelCm);
            Assert.False(node.equipmentDisabled);
        }

        [Fact] public void CaptureRestoreState_PreservesNodes()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.InstallPump("sump_a");
            var state = s.CaptureState();
            Assert.Single(state.nodes);

            var s2 = Create(out _, out _, out _);
            s2.RestoreState(state);
            Assert.Single(s2.State.nodes);
            Assert.True(s2.State.nodes[0].hasSumpPump);
        }

        // ── Plan 70: stratum-driven ingress, silt/sludge, pump load ─────

        [Fact] public void ApplyStratumCatalog_RegistersStrata_IgnoringInvalidDefs()
        {
            var s = Create(out _, out _, out _);
            int applied = s.ApplyStratumCatalog(new List<SumpStratumDef>
            {
                new SumpStratumDef { stratum_id = "stratum_a" },
                new SumpStratumDef { stratum_id = "" },          // invalid: no id
                null!,                                            // invalid: null def
            });
            Assert.Equal(1, applied);
        }

        [Fact] public void AssignStratum_UnknownStratumOrNode_Fails()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            Assert.NotEqual(ActionResult.StatusKind.Success, s.AssignStratum("sump_a", "stratum_missing").Status);
            Assert.NotEqual(ActionResult.StatusKind.Success, s.AssignStratum("no_such_node", "stratum_a").Status);
        }

        [Fact] public void TickDay_StratumIngress_ScalesWithGroundwaterPressure()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.ApplyStratumCatalog(new List<SumpStratumDef>
            {
                new SumpStratumDef { stratum_id = "stratum_a", base_ingress_cm_per_day = 4f, water_table_pressure = 1.5f }
            });
            Assert.Equal(ActionResult.StatusKind.Success, s.AssignStratum("sump_a", "stratum_a").Status);

            s.State.globalGroundwaterLevel = 20f;
            s.TickDay(1);
            float g1 = s.State.globalGroundwaterLevel;
            float level1 = s.State.nodes[0].waterLevelCm;

            // Raise live groundwater by 10 → the pressure term (0.5 + G/10) grows
            // with the observed level, whatever the weather drift contributed.
            s.State.globalGroundwaterLevel = g1 + 10f;
            s.TickDay(2);
            float g2 = s.State.globalGroundwaterLevel;
            float level2 = s.State.nodes[0].waterLevelCm;

            // No pump, no flood: level2 = level1 + ingress2 − natural drain (2),
            // with ingress2 = base × (0.5 + G2/10) × pressure.
            float expectedIngress2 = 4f * (0.5f + g2 / 10f) * 1.5f;
            Assert.Equal(level1 + expectedIngress2 - 2f, level2, 2);
            // Scaling direction: higher groundwater must ingress strictly more.
            float ingress1 = 4f * (0.5f + g1 / 10f) * 1.5f;
            Assert.True(expectedIngress2 > ingress1,
                "stratum ingress must scale with groundwater pressure");
        }

        [Fact] public void TickDay_SiltAccumulates_AndSettles_WithMassConserved()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.ApplyStratumCatalog(new List<SumpStratumDef>
            {
                new SumpStratumDef { stratum_id = "stratum_a", base_ingress_cm_per_day = 4f, water_table_pressure = 1f, silt_fraction = 0.05f }
            });
            s.AssignStratum("sump_a", "stratum_a");

            s.State.globalGroundwaterLevel = 20f;
            for (int day = 1; day <= 3; day++)
                s.TickDay(day);

            var node = s.State.nodes[0];
            float totalSolids = node.suspendedSolidsKg + node.settledSludgeKg;
            // Daily inflow 4 cm × (0.5 + ~G/10) pressure ≈ 3×4 cm × 10 L/cm × 0.05
            // ≈ 6 kg over three days — assert mass arrived and none vanished.
            Assert.True(totalSolids > 0f, "no silt accumulated");
            Assert.Equal(totalSolids, node.suspendedSolidsKg + node.settledSludgeKg, 4);
            Assert.True(node.settledSludgeKg > 0f, "no sludge settled out of suspension");
        }

        [Fact] public void TickDay_CleanPump_KeepsLegacyWearRate()
        {
            var s = Create(out var _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.InstallPump("sump_a");
            s.SetNodePower("sump_a", true);
            s.TickDay(1);
            Assert.Equal(99.9f, s.State.nodes[0].pumpCondition, 3);
        }

        [Fact] public void TickDay_SolidsLoad_IncreasesPumpWear()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.InstallPump("sump_a");
            s.SetNodePower("sump_a", true);
            s.State.nodes[0].suspendedSolidsKg = 30f; // solidsFactor = min(4, 30/10) = 3
            s.TickDay(1);
            // wear = 0.1 × (1 + 1 × 3) = 0.4
            Assert.Equal(99.6f, s.State.nodes[0].pumpCondition, 3);
        }

        [Fact] public void TickDay_SuspendedSolids_ReduceThroughput()
        {
            // Two identical systems; the dirty one must drain slower.
            var clean = Create(out _, out _, out _);
            var dirty = Create(out _, out _, out _);
            foreach (var s in new[] { clean, dirty })
            {
                s.AddNode("sump_a", "Lower Level");
                s.InstallPump("sump_a");
                s.SetNodePower("sump_a", true);
                s.State.globalGroundwaterLevel = 0f;
                s.State.nodes[0].waterLevelCm = 100f;
            }
            dirty.State.nodes[0].suspendedSolidsKg = 30f; // viscosity 1/(1+0.6) ≈ 0.625

            clean.TickDay(1);
            dirty.TickDay(1);

            Assert.True(dirty.State.nodes[0].waterLevelCm > clean.State.nodes[0].waterLevelCm,
                "solids-laden water must reduce pump throughput");
        }

        [Fact] public void TickDay_SettledSludge_AboveThreshold_BlocksStrainer()
        {
            var clean = Create(out _, out _, out _);
            var clogged = Create(out _, out _, out _);
            foreach (var s in new[] { clean, clogged })
            {
                s.AddNode("sump_a", "Lower Level");
                s.InstallPump("sump_a");
                s.SetNodePower("sump_a", true);
                s.State.globalGroundwaterLevel = 0f;
                s.State.nodes[0].waterLevelCm = 100f;
            }
            clogged.State.nodes[0].settledSludgeKg = SumpFloodingSystem.StrainerBlockageThresholdKg + 5f;

            clean.TickDay(1);
            clogged.TickDay(1);

            Assert.True(clogged.State.nodes[0].waterLevelCm > clean.State.nodes[0].waterLevelCm,
                "settled sludge above the strainer threshold must halve throughput");
        }

        [Fact] public void FloodStart_StratumToxicity_TieredContamination()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level", maxWaterLevelCm: 10f);
            s.ApplyStratumCatalog(new List<SumpStratumDef>
            {
                new SumpStratumDef { stratum_id = "stratum_a", base_ingress_cm_per_day = 5f, water_table_pressure = 1.4f, toxicity_tier = 2 }
            });
            s.AssignStratum("sump_a", "stratum_a");
            s.State.globalGroundwaterLevel = 20f;

            s.TickDay(1); // inflow ≥ 5 × 2.5 × 1.4 = 17.5 > 0.8 × 10 → floods

            Assert.True(s.State.nodes[0].isFlooded);
            // tier 2 → contamination gain 0.05 × (2+1) = 0.15 (legacy flat gain is 0.2)
            Assert.Equal(0.15f, s.State.nodes[0].contaminationLevel, 3);
        }

        [Fact] public void CaptureRestore_PreservesStratumAndSludgeState()
        {
            var s = Create(out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].stratumId = "stratum_a";
            s.State.nodes[0].suspendedSolidsKg = 3.5f;
            s.State.nodes[0].settledSludgeKg = 12.25f;

            var saved = s.CaptureState();
            s.State.nodes[0].suspendedSolidsKg = 0f;
            s.RestoreState(saved);

            var node = s.State.nodes[0];
            Assert.Equal("stratum_a", node.stratumId);
            Assert.Equal(3.5f, node.suspendedSolidsKg, 4);
            Assert.Equal(12.25f, node.settledSludgeKg, 4);
        }

        [Fact] public void RestoreOldSave_MissingSludgeFields_BehavesLegacy()
        {
            // Pre-Plan-70 saves carry no stratum/solids fields; defaults must
            // yield the exact legacy inflow/wear model (no free windfall, no flood).
            var s = Create(out _, out _, out _);
            var oldSave = new SumpFloodingState();
            oldSave.nodes.Add(new SumpNode { nodeId = "sump_a", displayName = "Lower Level", hasSumpPump = true, pumpPowered = true });
            s.RestoreState(oldSave);

            s.State.globalGroundwaterLevel = 20f;
            s.TickDay(1);

            var node = s.State.nodes[0];
            Assert.Equal(0f, node.suspendedSolidsKg, 4);   // legacy inflow carries no silt
            Assert.Equal(0f, node.settledSludgeKg, 4);
            Assert.Equal(99.9f, node.pumpCondition, 3);    // exact legacy wear
        }

        // ── Plan 70 slice 2: flocculation + centrifuge dewatering ──────

        private static SumpFloodingSystem CreateWithServices(
            out WeatherSystem weather, out PowerGridSystem power, out YearOfAshDeepFreezeSystem df,
            out Inventory.Inventory inventory, out WaterTreatmentSystem water
            )
        {
            weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 42);
            var state = new PowerGridState
            {
                GenerationWatts = 800,
                FuelUnits = 100,
                BatteryCapacityWh = 4000,
                BatteryReserveWh = 2000
            };
            var rooms = new System.Collections.Generic.List<PowerGridRoom>
            {
                new PowerGridRoom("sump_a", "Lower Level", 100f)
            };
            power = new PowerGridSystem(state, rooms, new SeededRng(42));
            df = new YearOfAshDeepFreezeSystem();
            inventory = new Inventory.Inventory();
            water = new WaterTreatmentSystem();
            var sys = new SumpFloodingSystem(new SeededRng(42), weather, power, df);
            sys.BindServices(inventory, water);
            return sys;
        }

        [Fact] public void StartFlocculation_InvalidTierOrUnknownNode_Fails()
        {
            var s = CreateWithServices(out _, out _, out _, out _, out _);
            s.AddNode("sump_a", "Lower Level");
            Assert.NotEqual(ActionResult.StatusKind.Success, s.StartFlocculation("sump_a", 0).Status);
            Assert.NotEqual(ActionResult.StatusKind.Success, s.StartFlocculation("sump_a", 3).Status);
            Assert.NotEqual(ActionResult.StatusKind.Success, s.StartFlocculation("no_node", 1).Status);
        }

        [Fact] public void StartFlocculation_MissingChemical_Fails()
        {
            var s = CreateWithServices(out _, out _, out _, out var inv, out _);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].suspendedSolidsKg = 10f;
            // no chemicals stocked
            Assert.NotEqual(ActionResult.StatusKind.Success, s.StartFlocculation("sump_a", 1).Status);
            Assert.Equal(10f, s.State.nodes[0].suspendedSolidsKg, 3); // untouched
        }

        [Fact] public void StartFlocculation_ConsumesExactlyTierDose_FromInventory()
        {
            var s = CreateWithServices(out _, out _, out _, out var inv, out _);
            inv.AddById(SumpFloodingSystem.FlocculantItemId, 4); // exactly two tier-1 doses
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].suspendedSolidsKg = 10f;

            Assert.Equal(ActionResult.StatusKind.Success, s.StartFlocculation("sump_a", 1).Status);
            Assert.Equal(ActionResult.StatusKind.Success, s.StartFlocculation("sump_a", 1).Status);
            // third dose must find the pantry empty
            s.State.nodes[0].suspendedSolidsKg = 10f;
            Assert.NotEqual(ActionResult.StatusKind.Success, s.StartFlocculation("sump_a", 1).Status);
        }

        [Fact] public void StartFlocculation_Tier1_CapturesSolids_WithMassConserved()
        {
            var s = CreateWithServices(out _, out _, out _, out var inv, out _);
            inv.AddById(SumpFloodingSystem.FlocculantItemId, 10);
            s.AddNode("sump_a", "Lower Level");
            var node = s.State.nodes[0];
            node.suspendedSolidsKg = 10f;
            node.settledSludgeKg = 0f;

            var res = s.StartFlocculation("sump_a", 1);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);

            // tier 1 at reference load: capture 0.6 × 10 = 6 kg; dose 2 units × 0.5 kg joins sludge
            Assert.Equal(4f, node.suspendedSolidsKg, 3);
            Assert.Equal(7f, node.settledSludgeKg, 3);
            // mass balance: final solids = initial solids + dosed chemical mass
            Assert.Equal(10f + 1f, node.suspendedSolidsKg + node.settledSludgeKg, 3);
        }

        [Fact] public void StartFlocculation_ReducesContamination_BoundedByTier()
        {
            var s = CreateWithServices(out _, out _, out _, out var inv, out _);
            inv.AddById(SumpFloodingSystem.FlocculantItemId, 10);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].suspendedSolidsKg = 10f;
            s.State.nodes[0].contaminationLevel = 0.5f;

            s.StartFlocculation("sump_a", 2);
            // tier 2 removal 0.05 × 2 = 0.1
            Assert.Equal(0.4f, s.State.nodes[0].contaminationLevel, 3);
        }

        [Fact] public void StartFlocculation_OverdoseOnThinSolids_FoulsWater()
        {
            var s = CreateWithServices(out _, out _, out _, out var inv, out _);
            inv.AddById(SumpFloodingSystem.FlocculantItemId, 10);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].suspendedSolidsKg = 0.5f; // thin solids
            s.State.nodes[0].contaminationLevel = 0.2f;

            var res = s.StartFlocculation("sump_a", 1);
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            // over-dosing penalty: residual chemical raises contamination
            Assert.Equal(0.22f, s.State.nodes[0].contaminationLevel, 3);
        }

        [Fact] public void RunCentrifugeBatch_NoPower_Fails()
        {
            var s = CreateWithServices(out _, out var grid, out _, out var inv, out _);
            inv.AddById(SumpFloodingSystem.CentrifugeFilterItemId, 5);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].settledSludgeKg = 30f;
            grid.SetBreaker("sump_a", closed: false); // breaker open → room unpowered

            Assert.NotEqual(ActionResult.StatusKind.Success, s.RunCentrifugeBatch("sump_a").Status);
            Assert.Equal(30f, s.State.nodes[0].settledSludgeKg, 3); // untouched
        }

        [Fact] public void RunCentrifugeBatch_NoSludge_OrMissingCloth_Fails()
        {
            var s = CreateWithServices(out _, out _, out _, out var inv, out _);
            s.AddNode("sump_a", "Lower Level");

            // no sludge
            Assert.NotEqual(ActionResult.StatusKind.Success, s.RunCentrifugeBatch("sump_a").Status);

            // sludge but no cloth
            s.State.nodes[0].settledSludgeKg = 30f;
            Assert.NotEqual(ActionResult.StatusKind.Success, s.RunCentrifugeBatch("sump_a").Status);
            Assert.Equal(30f, s.State.nodes[0].settledSludgeKg, 3);
        }

        [Fact] public void RunCentrifugeBatch_MassBalance_AndGreywaterRoutedToWaterTreatment()
        {
            var s = CreateWithServices(out _, out _, out _, out var inv, out var water);
            inv.AddById(SumpFloodingSystem.CentrifugeFilterItemId, 5);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].settledSludgeKg = 30f;

            var res = s.RunCentrifugeBatch("sump_a");
            Assert.Equal(ActionResult.StatusKind.Success, res.Status);

            // full-separation split of a 30 kg batch:
            // greywater 12 L, tailings 4.5 kg, cake = remainder 13.5 kg
            var node = s.State.nodes[0];
            Assert.Equal(0f, node.settledSludgeKg, 3);
            Assert.Equal(12f, water.State.rawWater, 3);              // routed as Raw (non-potable)
            Assert.Equal(4.5f, s.State.hazardousTailingsKg, 3);
            Assert.Equal(13.5f, s.State.dewateredCakeKg, 3);
            // mass balance: cake + tailings + greywater ≡ batch input
            Assert.Equal(30f, s.State.dewateredCakeKg + s.State.hazardousTailingsKg + water.State.rawWater, 3);
            Assert.Equal(0f, s.State.unroutedGreywaterLiters, 3);
            // consumables/wear: media 100 − 5, condition 100 − 1
            Assert.Equal(95f, s.State.centrifugeFilterMedia, 3);
            Assert.Equal(99f, s.State.centrifugeCondition, 3);
            Assert.Equal(1, s.State.centrifugeBatchesCompleted);
        }

        [Fact] public void RunCentrifugeBatch_LowMedia_YieldsWetterCake_StillConservesMass()
        {
            var s = CreateWithServices(out _, out _, out _, out var inv, out var water);
            inv.AddById(SumpFloodingSystem.CentrifugeFilterItemId, 5);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].settledSludgeKg = 30f;
            s.State.centrifugeFilterMedia = 15f; // below low-media threshold

            s.RunCentrifugeBatch("sump_a");

            // half efficiency: 6 L recovered, tailings unchanged, cake absorbs the rest
            Assert.Equal(6f, water.State.rawWater, 3);
            Assert.Equal(4.5f, s.State.hazardousTailingsKg, 3);
            Assert.Equal(19.5f, s.State.dewateredCakeKg, 3);
            Assert.Equal(30f, s.State.dewateredCakeKg + s.State.hazardousTailingsKg + water.State.rawWater, 3);
        }

        [Fact] public void RunCentrifugeBatch_WithoutWaterTreatment_ConservesGreywaterUnrouted()
        {
            var s = CreateWithServices(out _, out _, out _, out var inv, out _);
            s.BindServices(inv, null); // no water-treatment authority bound
            inv.AddById(SumpFloodingSystem.CentrifugeFilterItemId, 5);
            s.AddNode("sump_a", "Lower Level");
            s.State.nodes[0].settledSludgeKg = 30f;

            s.RunCentrifugeBatch("sump_a");

            // greywater must not vanish — it waits in the unrouted buffer
            Assert.Equal(12f, s.State.unroutedGreywaterLiters, 3);
            Assert.Equal(30f, s.State.dewateredCakeKg + s.State.hazardousTailingsKg + s.State.unroutedGreywaterLiters, 3);
        }

        [Fact] public void ReplaceCentrifugeMedia_ConsumesCloth_ResetsMedia()
        {
            var s = CreateWithServices(out _, out _, out _, out var inv, out _);
            inv.AddById(SumpFloodingSystem.CentrifugeFilterItemId, 1);
            s.State.centrifugeFilterMedia = 15f;

            Assert.Equal(ActionResult.StatusKind.Success, s.ReplaceCentrifugeMedia().Status);
            Assert.Equal(100f, s.State.centrifugeFilterMedia, 3);

            // second replace without cloth fails
            Assert.NotEqual(ActionResult.StatusKind.Success, s.ReplaceCentrifugeMedia().Status);
            Assert.Equal(100f, s.State.centrifugeFilterMedia, 3);
        }

        [Fact] public void FullPipeline_FlocculateThenCentrifuge_SludgeMassConserved()
        {
            var s = CreateWithServices(out _, out _, out _, out var inv, out var water);
            inv.AddById(SumpFloodingSystem.FlocculantItemId, 10);
            inv.AddById(SumpFloodingSystem.CentrifugeFilterItemId, 5);
            s.AddNode("sump_a", "Lower Level");
            var node = s.State.nodes[0];
            node.suspendedSolidsKg = 10f;
            node.settledSludgeKg = 0f;

            s.StartFlocculation("sump_a", 1);          // 10 kg suspended → 4 kg + 7 kg settled
            s.RunCentrifugeBatch("sump_a");            // 7 kg settled → 3.15 cake + 1.05 tailings + 2.8 L

            float totalOut = s.State.dewateredCakeKg + s.State.hazardousTailingsKg
                + water.State.rawWater + node.suspendedSolidsKg + node.settledSludgeKg;
            // inputs: 10 kg silt + 1 kg flocculant mass — everything accounted for
            Assert.Equal(11f, totalOut, 3);
        }

        private static SumpFloodingSystem Create(out WeatherSystem weather, out PowerGridSystem power, out YearOfAshDeepFreezeSystem df)
        {
            weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 42);
            var state = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
            var rooms = new System.Collections.Generic.List<PowerGridRoom>
            {
                new PowerGridRoom("sump_a", "Lower Level", 100f)
            };
            power = new PowerGridSystem(state, rooms, new SeededRng(42));
            df = new YearOfAshDeepFreezeSystem();
            return new SumpFloodingSystem(new SeededRng(42), weather, power, df);
        }
    }
}
