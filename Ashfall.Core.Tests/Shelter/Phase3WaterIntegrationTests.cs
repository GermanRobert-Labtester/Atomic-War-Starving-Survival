// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// B5–B8 Phase 3 (Plan 66) water-integration foundation:
    /// - power-load registration contract: consumer systems expose stable load
    ///   rooms; the grid owns allocation/shedding;
    /// - sump pump power coupling: installed pumps are registered critical
    ///   loads; pump drainage and the centrifuge read the allocation-aware
    ///   served state (was the global-outage IsRoomPowered read, and before
    ///   that — nothing at all: live sump nodes were never grid rooms, so
    ///   pumps could never drain);
    /// - decon canonical water cost: the wash bill consumes the canonical
    ///   potable item and the chlor-alkali bleach (the old "water_clean" /
    ///   "soap" ids exist in no catalog — every decon action was permanently
    ///   blocked in live play).
    /// </summary>
    public class Phase3WaterIntegrationTests
    {
        // ─── A. Power-load registration contract ───────────────────────────

        private static PowerGridSystem MakeGrid(float generationWatts = 800f, float batteryReserveWh = 2000f)
        {
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_air_filtration", "Air Filtration", 180f,
                    PowerGridRoomPriority.Critical, "fx_filtration_off")
            };
            var state = new PowerGridState
            {
                GenerationWatts = generationWatts,
                FuelUnits = 100f,
                BatteryCapacityWh = 4000f,
                BatteryReserveWh = batteryReserveWh
            };
            return new PowerGridSystem(state, rooms, new SeededRng(77));
        }

        [Fact]
        public void RegisterLoadRoom_AddsDraw_AndFiresTypedEvent()
        {
            var grid = MakeGrid();
            float drawBefore = grid.TotalDrawWatts;

            PowerGridEvent? evt = null;
            grid.OnPowerChanged += e => evt = e;

            Assert.True(grid.RegisterLoadRoom(new PowerGridRoom("sump_a", "Lower Level", 90f,
                PowerGridRoomPriority.Critical, "fx_sump_pump_off")));

            Assert.Equal(drawBefore + 90f, grid.TotalDrawWatts, 1);
            Assert.NotNull(evt);
            Assert.Equal(PowerGridEventKind.LoadRoomRegistered, evt!.Kind);
            Assert.Equal("sump_a", evt.RoomId);
        }

        [Fact]
        public void RegisterLoadRoom_IsIdempotent_CatalogRoomTakesPrecedence()
        {
            var grid = MakeGrid();
            float drawBefore = grid.TotalDrawWatts;

            // Same id as the catalog room: ignored, catalog draw preserved.
            Assert.False(grid.RegisterLoadRoom(new PowerGridRoom("room_air_filtration", "X", 999f,
                PowerGridRoomPriority.Low, "x")));
            Assert.Equal(drawBefore, grid.TotalDrawWatts, 1);

            // Second registration of a dynamic room: no duplicate load.
            Assert.True(grid.RegisterLoadRoom(new PowerGridRoom("sump_a", "Lower Level", 90f,
                PowerGridRoomPriority.Critical, "fx_sump_pump_off")));
            Assert.False(grid.RegisterLoadRoom(new PowerGridRoom("sump_a", "Lower Level", 90f,
                PowerGridRoomPriority.Critical, "fx_sump_pump_off")));
            Assert.Equal(drawBefore + 90f, grid.TotalDrawWatts, 1);
        }

        [Fact]
        public void RegisterLoadRoom_ParticipatesInPriorityAllocation()
        {
            // 300W covers the critical catalog room (180W) but not a further
            // critical sump load (90W): 180 + 90 = 270 ≤ 300 → both served.
            var grid = MakeGrid(generationWatts: 300f, batteryReserveWh: 0f);
            Assert.True(grid.RegisterLoadRoom(new PowerGridRoom("sump_a", "Lower Level", 90f,
                PowerGridRoomPriority.Critical, "fx_sump_pump_off")));
            var sum = grid.TickDay(1, new SeededRng(5));
            Assert.Contains("sump_a", sum.ServedRoomIds);
            Assert.Contains("room_air_filtration", sum.ServedRoomIds);
            Assert.True(grid.IsRoomServed("sump_a"));
        }

        // ─── B. Sump pump power coupling ───────────────────────────────────

        private static SumpFloodingSystem MakeSump(out WeatherSystem weather, out PowerGridSystem power,
            float generationWatts = 800f, float batteryReserveWh = 2000f)
        {
            weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 42);
            power = MakeGrid(generationWatts, batteryReserveWh);
            return new SumpFloodingSystem(new SeededRng(42), weather, power,
                new YearOfAshDeepFreezeSystem());
        }

        [Fact]
        public void InstallPump_RegistersGridLoad_ServedWhenGenerationCovers()
        {
            var s = MakeSump(out _, out var power);
            s.AddNode("sump_a", "Lower Level");
            Assert.False(power.IsRoomServed("sump_a")); // not a load yet

            Assert.Equal(ActionResult.StatusKind.Success, s.InstallPump("sump_a").Status);

            Assert.True(power.IsRoomServed("sump_a"));
            Assert.Equal(SumpFloodingSystem.SumpPumpDrawWatts, power.TotalDrawWatts - 180f, 1);
        }

        [Fact]
        public void PumpDrains_WhenServed_AndPlayerEnabled()
        {
            var s = MakeSump(out _, out _);
            s.AddNode("sump_a", "Lower Level");
            s.InstallPump("sump_a");
            s.SetNodePower("sump_a", true);
            s.State.nodes[0].waterLevelCm = 50f;
            s.State.globalGroundwaterLevel = 0f; // no inflow: Clear weather, empty basin

            s.TickDay(1);

            Assert.True(s.State.nodes[0].waterLevelCm < 50f, "a served, enabled pump must drain");
        }

        [Fact]
        public void PumpDoesNotDrain_WhenGenerationCannotServeIt()
        {
            // 180W generation serves only the critical filtration room; the
            // 90W sump load (registered second by RoomId ordinal) sheds.
            var s = MakeSump(out _, out _, generationWatts: 180f, batteryReserveWh: 0f);
            s.AddNode("sump_a", "Lower Level");
            s.InstallPump("sump_a");
            s.SetNodePower("sump_a", true);
            s.State.nodes[0].waterLevelCm = 50f;
            s.State.globalGroundwaterLevel = 0f;

            s.TickDay(1);

            // −2/day natural drainage (non-flooded); a served pump would have
            // drained 20. The pump contributed nothing while shed.
            Assert.Equal(48f, s.State.nodes[0].waterLevelCm, 1);
        }

        [Fact]
        public void Restore_ReRegistersPumpLoads()
        {
            var s1 = MakeSump(out _, out var power);
            s1.AddNode("sump_a", "Lower Level");
            s1.InstallPump("sump_a");
            var saved = s1.CaptureState();

            // Simulate a fresh session: new grid (room list empty of sumps),
            // restored sump state must re-expose the pump load.
            var s2 = MakeSump(out _, out var power2, generationWatts: 0f, batteryReserveWh: 0f);
            s2.RestoreState(saved);

            Assert.Equal(270f, power2.TotalDrawWatts, 1); // catalog room 180W + re-registered pump 90W
            Assert.False(power2.IsRoomServed("sump_a")); // 90W demand vs 0W gen → shed, but the LOAD is registered
        }

        [Fact]
        public void NodeWithoutPump_IsNeverServed()
        {
            var s = MakeSump(out _, out var power);
            s.AddNode("sump_a", "Lower Level"); // no InstallPump
            Assert.False(power.IsRoomServed("sump_a"));
        }

        // ─── C. Decon canonical water cost ─────────────────────────────────

        private static DecontaminationSystem MakeDecon(out Inventory.Inventory inv)
        {
            inv = new Inventory.Inventory();
            var catalog = new DeconProtocolCatalog
            {
                protocols = new List<DeconProtocolDef>
                {
                    new DeconProtocolDef
                    {
                        protocol_id = "decon_test_single",
                        display_name = "Single Gate Test Protocol",
                        stages = new List<DeconStageDef>
                        {
                            new DeconStageDef
                            {
                                stage_id = "stage_gate_only", stage_order = 0,
                                duration_ticks = 1, water_liters = 0,
                                external_contamination_multiplier = 0f,
                                effluent_contamination_contribution = 0f,
                                requires_operator = true, operator_skill_factor = 0f
                            }
                        },
                        total_chelator_units = 0,
                        interlock_threshold_mSv_per_h = 0.5f
                    }
                }
            };
            var rad = new RadiationSystem(seed: 42);
            var airlock = new AirlockSecuritySystem(new SeededRng(42));
            var sl = new StartingLevelSystem();
            return new DecontaminationSystem(new SeededRng(999), rad, inv, airlock, sl, catalog);
        }

        [Fact]
        public void Decon_Wash_ConsumesCanonicalWaterAndBleach_ExactlyOnce()
        {
            var d = MakeDecon(out var inv);
            inv.AddById("clean_water", 10);
            inv.AddById("item_liquid_bleach_carboy", 5);

            var r = d.StartProtocolCycle("decon_test_single", "s1", "gear_a", 0.8f);

            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(9, inv.CountById("clean_water"));
            Assert.Equal(4, inv.CountById("item_liquid_bleach_carboy"));
        }

        [Fact]
        public void Decon_Wash_WithoutCanonicalWater_Blocked_MutatesNothing()
        {
            var d = MakeDecon(out var inv);
            inv.AddById("item_liquid_bleach_carboy", 5); // water missing

            var r = d.StartProtocolCycle("decon_test_single", "s1", "gear_a", 0.8f);

            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal("no_water", r.FailureCode);
            Assert.Equal(5, inv.CountById("item_liquid_bleach_carboy")); // atomic: nothing consumed
            Assert.Null(d.State.activeCase);
        }
    }
}
