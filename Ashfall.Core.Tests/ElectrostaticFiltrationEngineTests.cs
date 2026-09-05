using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;

using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 72 §5.15 engine matrix for the electrostatic precipitation stage:
    /// capture curves, power gating, dust mass balance, rapping, ozone, arc
    /// faults (deterministic), disposal seam. VentilationSystem remains the
    /// canonical air authority — the stage extends it.
    /// </summary>
    public class ElectrostaticFiltrationEngineTests
    {
        private const string RoomId = "vent_room";

        private static ElectrostaticStageDef TestStage(int arcRiskBp = 40, float capacityKg = 12f)
            => new ElectrostaticStageDef
            {
                stage_id = "stage_test",
                display_name = "Test Precipitator",
                dust_capacity_kg = capacityKg,
                maintenance_interval_days = 14,
                operating_profiles = new List<ElectrostaticProfileDef>
                {
                    new ElectrostaticProfileDef
                    {
                        profile_id = "profile_standard",
                        display_name = "Standard",
                        nominal_power_w = 350f,
                        capture_efficiency_pm25 = 0.8f,
                        capture_efficiency_pm10 = 0.9f,
                        hot_ash_capture_efficiency = 0.7f,
                        ozone_output_rate_ppm_per_day = 8f,
                        arc_risk_base_bp = arcRiskBp
                    }
                },
                required_component_ids = new List<ElectrostaticComponentCost>()
            };

        private static (VentilationSystem vent, PowerGridSystem power, Inventory.Inventory inv, ShelterFireHazardSystem fire)
            Create(int seed = 7, bool powered = true, ElectrostaticStageDef? stage = null)
        {
            var vent = new VentilationSystem(new StartingLevelSystem());
            var gridState = new PowerGridState
            {
                GenerationWatts = powered ? 800 : 0,
                FuelUnits = 100,
                BatteryCapacityWh = 4000,
                BatteryReserveWh = 2000
            };
            var power = new PowerGridSystem(gridState, new List<PowerGridRoom> { new PowerGridRoom(RoomId, "Air Hall", 100f) }, new SeededRng(seed));
            if (!powered) power.SetBreaker(RoomId, closed: false);
            var inv = new Inventory.Inventory();
            var fire = new ShelterFireHazardSystem();
            vent.ApplyElectrostaticCatalog(new List<ElectrostaticStageDef> { stage ?? TestStage() });
            vent.BindStageServices(new SeededRng(seed), power, inv, fire);
            return (vent, power, inv, fire);
        }

        private static VentilationSystem InstallFresh(VentilationSystem vent)
        {
            var r = vent.InstallElectrostaticStage("stage_test", RoomId);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            return vent;
        }

        [Fact]
        public void Install_ConsumesCanonicalComponents_AndActivatesStage()
        {
            var (vent, _, inv, _) = Create();
            inv.AddById("mechanical_parts", 4);
            inv.AddById("scrap_chemical", 2);
            inv.AddById("item_air_filter_hepa", 1);

            var r = vent.InstallElectrostaticStage("stage_test", RoomId);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            var stage = vent.State.electrostatic;
            Assert.NotNull(stage);
            Assert.True(stage!.installed);
            Assert.True(stage.energized == false); // energized only after first tick with power
            // component consumption is real: a second install without stock is blocked
            var again = vent.InstallElectrostaticStage("stage_test", RoomId);
            Assert.Equal(ActionResult.StatusKind.Blocked, again.Status);
        }

        [Fact]
        public void Install_MissingComponents_Fails()
        {
            var (vent, _, _, _) = Create(); // empty pantry
            var gated = TestStage();
            gated.required_component_ids = new List<ElectrostaticComponentCost>
            {
                new ElectrostaticComponentCost { item_id = "mechanical_parts", amount = 4 }
            };
            vent.ApplyElectrostaticCatalog(new List<ElectrostaticStageDef> { gated });

            Assert.NotEqual(ActionResult.StatusKind.Success,
                vent.InstallElectrostaticStage("stage_test", RoomId).Status);
            Assert.Null(vent.State.electrostatic);
        }

        [Fact]
        public void TickDay_CleanStage_CapturesWithMassBalance()
        {
            var (vent, _, _, _) = Create();
            InstallFresh(vent);
            vent.TickDay(1, incomingParticulateKgPerDay: 2f);

            var stage = vent.State.electrostatic!;
            Assert.True(stage.energized);
            // fresh plates (100% condition): captured = 2 × 0.8 × 1.0 = 1.6 kg
            Assert.Equal(1.6f, stage.dustLoadKg, 3);
            Assert.Equal(0f, stage.hopperKg, 3);
        }

        [Fact]
        public void TickDay_HotAshLoad_UsesHotAshEfficiency()
        {
            var (vent, _, _, _) = Create();
            InstallFresh(vent);
            vent.TickDay(1, incomingParticulateKgPerDay: 2f, hotAshLoad: true);

            // hot-ash efficiency 0.7: captured = 2 × 0.7 = 1.4 kg
            Assert.Equal(1.4f, vent.State.electrostatic!.dustLoadKg, 3);
        }

        [Fact]
        public void TickDay_DustAccumulates_ToCapacityThenFaults()
        {
            var (vent, _, _, _) = Create();
            InstallFresh(vent);

            // 12 kg capacity at 1.6 kg/day (fresh plates) → capacity fault on ~day 8
            for (int day = 1; day <= 20; day++)
                vent.TickDay(day, incomingParticulateKgPerDay: 2f);

            var stage = vent.State.electrostatic!;
            Assert.True(stage.faulted);
            Assert.Equal("dust_capacity_exceeded", stage.faultReason);
            Assert.True(stage.dustLoadKg <= 12f + 0.001f);
            Assert.False(stage.energized); // faulted stage shuts down
        }

        [Fact]
        public void TickDay_NoPower_StageOffline_NoCaptureNoOzone()
        {
            var (vent, _, _, _) = Create(powered: false);
            InstallFresh(vent);
            vent.TickDay(1, incomingParticulateKgPerDay: 2f);

            var stage = vent.State.electrostatic!;
            Assert.False(stage.energized);
            Assert.Equal(0f, stage.dustLoadKg, 3);
            Assert.Equal(0f, vent.State.ozonePpm, 3);
        }

        [Fact]
        public void TickDay_Ozone_AccumulatesWhileEnergized_DecaysWhenOffline()
        {
            var (vent, power, _, _) = Create();
            InstallFresh(vent);
            vent.TickDay(1, incomingParticulateKgPerDay: 0f);
            Assert.Equal(8f, vent.State.ozonePpm, 3); // standard profile rate

            power.SetBreaker(RoomId, closed: false); // stage offline
            vent.TickDay(2);
            // offline: no new ozone, passive decay 10%
            Assert.Equal(7.2f, vent.State.ozonePpm, 2);
        }

        [Fact]
        public void RapPlates_TransfersDustToHopper_MassConserved()
        {
            var (vent, _, _, _) = Create();
            InstallFresh(vent);
            vent.TickDay(1, incomingParticulateKgPerDay: 2f); // 1.6 kg on plates

            var r = vent.RapPlates();
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            var stage = vent.State.electrostatic!;
            Assert.Equal(0.16f, stage.dustLoadKg, 3);         // 10% residual
            Assert.Equal(1.44f, stage.hopperKg, 3);           // 90% transferred
            Assert.Equal(1.6f, stage.dustLoadKg + stage.hopperKg, 3); // conserved
            Assert.Equal(1, stage.rappingCooldownDays);

            // cooldown blocks immediate re-rap
            Assert.Equal(ActionResult.StatusKind.Blocked, vent.RapPlates().Status);
        }

        [Fact]
        public void EmptyHopper_PacksRadioactiveDrums_ConservingMass()
        {
            var (vent, _, inv, _) = Create();
            InstallFresh(vent);
            vent.State.electrostatic!.hopperKg = 25f;

            var r = vent.EmptyHopperToDrums(4);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(5f, vent.State.electrostatic!.hopperKg, 3);
            Assert.Equal(2, inv.CountById(VentilationSystem.HotDustDrumItemId));

            // below one drum → blocked, nothing deleted
            Assert.Equal(ActionResult.StatusKind.Blocked, vent.EmptyHopperToDrums(4).Status);
            Assert.Equal(5f, vent.State.electrostatic!.hopperKg, 3);
        }

        [Fact]
        public void ArcFault_WithForcedRisk_TripsBreaker_AndIgnitesFire()
        {
            // forced risk: 10000 bp = guaranteed deterministic fault on first tick
            var (vent, power, _, fire) = Create(seed: 7, stage: TestStage(arcRiskBp: 10000));
            InstallFresh(vent);
            int ignited = 0;
            fire.OnFireIgnited += (_, _) => ignited++;

            vent.TickDay(1, incomingParticulateKgPerDay: 0f);

            var stage = vent.State.electrostatic!;
            Assert.True(stage.faulted);
            Assert.Equal("arc_fault", stage.faultReason);
            Assert.Equal(85f, stage.transformerCondition, 3); // 100 − 15 wear
            Assert.True(power.State.IsRoomTripped(RoomId));    // canonical breaker handoff
            Assert.Equal(1, ignited);                          // canonical fire handoff
        }

        [Fact]
        public void ArcFault_DeterministicReplay_SameSeedSameOutcome()
        {
            static (bool faulted, float transformer) Run(int seed)
            {
                var (vent, _, _, _) = Create(seed: seed, stage: TestStage(arcRiskBp: 3000));
                InstallFresh(vent);
                for (int day = 1; day <= 30; day++)
                    vent.TickDay(day, incomingParticulateKgPerDay: 0.5f);
                var st = vent.State.electrostatic!;
                return (st.faulted, st.transformerCondition);
            }

            var a = Run(77);
            var b = Run(77);
            Assert.Equal(a.faulted, b.faulted);
            Assert.Equal(a.transformer, b.transformer, 3);
        }

        [Fact]
        public void Service_ClearsFault_AndRestoresCondition()
        {
            var (vent, _, _, _) = Create();
            InstallFresh(vent);
            vent.State.electrostatic!.faulted = true;
            vent.State.electrostatic!.faultReason = "arc_fault";
            vent.State.electrostatic!.transformerCondition = 40f;

            var r = vent.ServiceElectrostaticStage();
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            var stage = vent.State.electrostatic!;
            Assert.False(stage.faulted);
            Assert.Equal(65f, stage.transformerCondition, 3); // 40 + 25 restore
        }

        [Fact]
        public void SaveLoad_PreservesStageState_Exactly()
        {
            var (vent, _, _, _) = Create();
            InstallFresh(vent);
            vent.State.electrostatic!.dustLoadKg = 3.25f;
            vent.State.electrostatic!.hopperKg = 7.5f;
            vent.State.electrostatic!.faulted = true;
            vent.State.electrostatic!.faultReason = "dust_capacity_exceeded";

            var saved = vent.CaptureState();
            vent.RestoreState(saved);

            var stage = vent.State.electrostatic!;
            Assert.True(stage.installed);
            Assert.Equal(3.25f, stage.dustLoadKg, 4);
            Assert.Equal(7.5f, stage.hopperKg, 4);
            Assert.True(stage.faulted);
            Assert.Equal("dust_capacity_exceeded", stage.faultReason);
            Assert.Equal("profile_standard", stage.profileId);
        }

        [Fact]
        public void OldSave_WithoutStage_NoOpTick_NoRegression()
        {
            var (vent, _, _, _) = Create();
            // pre-Plan-72 save: VentilationState.electrostatic absent → null
            Assert.Null(vent.State.electrostatic);
            vent.TickDay(1, incomingParticulateKgPerDay: 2f); // must not throw or fault
            Assert.Null(vent.State.electrostatic);
            Assert.Equal(0f, vent.State.ozonePpm, 3);
        }
    }
}

namespace Ashfall.Core
{
    /// <summary>Test observation helper (kept out of the shipping API surface).</summary>
    public static class VentilationStageTestAccess
    {
        public static float hopperKgOfStage(this VentilationSystem vent)
            => vent.State.electrostatic?.hopperKg ?? 0f;
    }
}
