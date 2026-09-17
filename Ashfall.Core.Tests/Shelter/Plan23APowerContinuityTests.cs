// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// C2[6] 23A — one power authority continuity repairs.
    ///
    /// Pins the new Core contracts added by the plan: the canonical
    /// demand/supply/deficit/runtime/fuel read model on <see cref="PowerGridSystem"/>,
    /// the mechanical air-handling power gate on <see cref="VentilationSystem"/>, and
    /// the authored <c>requires_power</c> gate on <see cref="LibraryStudySystem"/>.
    /// All gates are optional and default to legacy behaviour, so these tests also
    /// pin the null/default path.
    /// </summary>
    public class Plan23APowerContinuityTests
    {
        private static PowerGridSystem MakeGrid(float generation, float batteryReserve,
            float batteryCapacity = 4000f, float fuel = 100f)
        {
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("room_air_filtration", "Air Filtration", 180f,
                    PowerGridRoomPriority.Critical),
                new PowerGridRoom("room_workshop", "Workshop", 300f,
                    PowerGridRoomPriority.Low),
                new PowerGridRoom("room_lighting_main", "Lighting", 80f,
                    PowerGridRoomPriority.Low)
            };
            var state = new PowerGridState
            {
                GenerationWatts = generation,
                FuelUnits = fuel,
                BatteryCapacityWh = batteryCapacity,
                BatteryReserveWh = batteryReserve
            };
            return new PowerGridSystem(state, rooms, new SeededRng(23));
        }

        // ---- canonical read model -------------------------------------------

        [Fact]
        public void ReadModel_DeficitMatchesAllocatorSupply()
        {
            // gen 800, reserve 0 → available 800; draw 560 → no deficit.
            var surplus = MakeGrid(800f, 0f);
            Assert.Equal(560f, surplus.TotalDrawWatts, 3);
            Assert.Equal(800f, surplus.AvailableSupplyWatts, 3);
            Assert.Equal(0f, surplus.DeficitWatts, 3);
            Assert.True(float.IsPositiveInfinity(surplus.EstimatedRuntimeHours));

            // gen 400, reserve 2400 → available 400 + 100 = 500; draw 560 → deficit 60.
            var deficit = MakeGrid(400f, 2400f);
            Assert.Equal(500f, deficit.AvailableSupplyWatts, 3);
            Assert.Equal(60f, deficit.DeficitWatts, 3);
            // runtime = reserve / (draw - gen) = 2400 / 160 = 15 h
            Assert.Equal(15f, deficit.EstimatedRuntimeHours, 3);
        }

        [Fact]
        public void ReadModel_FuelRunwayMatchesTickBurn()
        {
            // base 800 W → burn = 800 * 24 * 0.001 = 19.2 / day; 96 fuel → 5 days.
            var grid = MakeGrid(800f, 0f, fuel: 96f);
            Assert.Equal(5f, grid.EstimatedFuelRunwayDays, 3);
        }

        // ---- mechanical ventilation power gate ------------------------------

        private static VentilationSystem MakeVent()
            => new VentilationSystem(new StartingLevelSystem());

        [Fact]
        public void Ventilation_UnpoweredMechanical_AccumulatesMoreSmoke()
        {
            var powered = MakeVent();
            var unpowered = MakeVent();
            powered.RegisterSource(new VentilationSource
            {
                sourceId = "generator", roomId = "room_air_filtration",
                smokeOutputPerDay = 10f, coOutputPerDay = 40f,
                requiresExhaust = true, isActive = true
            });
            unpowered.RegisterSource(new VentilationSource
            {
                sourceId = "generator", roomId = "room_air_filtration",
                smokeOutputPerDay = 10f, coOutputPerDay = 40f,
                requiresExhaust = true, isActive = true
            });

            powered.TickDay(1, 0f, false, mechanicalPowerAvailable: true);
            unpowered.TickDay(1, 0f, false, mechanicalPowerAvailable: false);

            Assert.True(unpowered.SmokeSoot > powered.SmokeSoot,
                $"unpowered={unpowered.SmokeSoot} should exceed powered={powered.SmokeSoot}");
            Assert.True(unpowered.CarbonMonoxide > powered.CarbonMonoxide);
        }

        [Fact]
        public void Ventilation_DefaultMechanicalPower_IsLegacyParity()
        {
            var implicitPower = MakeVent();
            var explicitPower = MakeVent();
            foreach (var v in new[] { implicitPower, explicitPower })
            {
                v.RegisterSource(new VentilationSource
                {
                    sourceId = "generator", roomId = "room_air_filtration",
                    smokeOutputPerDay = 10f, coOutputPerDay = 5f,
                    requiresExhaust = true, isActive = true
                });
            }

            implicitPower.TickDay(1, 0f, false);
            explicitPower.TickDay(1, 0f, false, mechanicalPowerAvailable: true);

            Assert.Equal(explicitPower.SmokeSoot, implicitPower.SmokeSoot, 3);
            Assert.Equal(explicitPower.CarbonMonoxide, implicitPower.CarbonMonoxide, 3);
        }

        // ---- library authored power gate ------------------------------------

        private static LibraryStudySystem MakeLibrary()
            => new LibraryStudySystem(new SkillProgressionSystem(), new ResearchSystem(),
                new Journal.JournalSystem(), new DutyRosterSystem());

        private static void AddManual(LibraryStudySystem lib, string id, bool requiresPower,
            int hours = 5)
        {
            lib.LoadCatalog(new List<ManualDefinition>
            {
                new ManualDefinition
                {
                    manual_id = id,
                    display_name = id,
                    studyHoursRequired = hours,
                    requiresPower = requiresPower
                }
            });
        }

        [Fact]
        public void Library_PoweredManual_BlockedWhenUnpowered()
        {
            var lib = MakeLibrary();
            AddManual(lib, "man_powered", requiresPower: true);
            bool power = false;
            lib.PowerAvailable = () => power;

            var result = lib.StartStudy("man_powered", "survivor_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);

            power = true;
            Assert.Equal(ActionResult.StatusKind.Success,
                lib.StartStudy("man_powered", "survivor_1").Status);
        }

        [Fact]
        public void Library_PoweredManual_JobPausesThenResumes()
        {
            var lib = MakeLibrary();
            AddManual(lib, "man_powered", requiresPower: true);
            bool power = true;
            lib.PowerAvailable = () => power;

            Assert.Equal(ActionResult.StatusKind.Success,
                lib.StartStudy("man_powered", "survivor_1").Status);

            power = false;
            lib.TickDay(1);
            Assert.False(lib.State.activeJobs[0].isComplete);
            float stalledProgress = lib.State.activeJobs[0].progressHours;

            lib.TickDay(2);
            Assert.Equal(stalledProgress, lib.State.activeJobs[0].progressHours, 3);

            power = true;
            lib.TickDay(3);
            Assert.True(lib.State.activeJobs[0].isComplete);
        }

        [Fact]
        public void Library_UnpoweredManual_IgnoresPower()
        {
            var lib = MakeLibrary();
            AddManual(lib, "man_free", requiresPower: false);
            lib.PowerAvailable = () => false;

            Assert.Equal(ActionResult.StatusKind.Success,
                lib.StartStudy("man_free", "survivor_1").Status);
            lib.TickDay(1);
            Assert.True(lib.State.activeJobs[0].isComplete);
        }

        [Fact]
        public void Library_NullProvider_IsLegacyPowered()
        {
            var lib = MakeLibrary();
            AddManual(lib, "man_powered", requiresPower: true);
            // No PowerAvailable bound → legacy always-powered behaviour.
            Assert.Equal(ActionResult.StatusKind.Success,
                lib.StartStudy("man_powered", "survivor_1").Status);
            lib.TickDay(1);
            Assert.True(lib.State.activeJobs[0].isComplete);
        }
    }
}