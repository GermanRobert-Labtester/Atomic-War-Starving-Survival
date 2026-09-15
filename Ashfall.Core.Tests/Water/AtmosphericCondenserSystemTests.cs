// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Water
{
    /// <summary>
    /// B5–B8 expansion (§9.9): atmospheric condenser — build-gated bounded
    /// source with a weather-indexed deterministic yield into the treatment
    /// intake, registered grid load, canonical membrane build/replace, and
    /// save-safe restore. Physically no brine (condensation, not desalination).
    /// </summary>
    public class AtmosphericCondenserSystemTests
    {
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
            return new PowerGridSystem(state, rooms, new SeededRng(67));
        }

        private static WeatherSystem MakeWeather(WeatherKind kind)
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 42);
            weather.ForceWeather(kind);
            return weather;
        }

        private static AtmosphericCondenserSystem MakeCondenser(
            PowerGridSystem? grid = null, WaterTreatmentSystem? water = null, WeatherKind kind = WeatherKind.Rain)
        {
            return new AtmosphericCondenserSystem(
                grid ?? MakeGrid(), water ?? new WaterTreatmentSystem(),
                MakeWeather(kind), NullLog.Instance);
        }

        [Fact]
        public void Build_RequiresCapability_NeverGrantedByResearch()
        {
            var c = MakeCondenser();
            Assert.False(c.TryBuild(false, out var reason));
            Assert.Equal("missing_knowledge", reason);
            Assert.False(c.IsBuilt);
        }

        [Fact]
        public void Build_RegistersGridLoad_Idempotent()
        {
            var grid = MakeGrid();
            float drawBefore = grid.TotalDrawWatts;
            var c = MakeCondenser(grid);

            Assert.True(c.TryBuild(true, out var reason), reason);
            Assert.False(c.TryBuild(true, out var again));
            Assert.Equal("already_built", again);

            Assert.True(grid.IsRoomServed(AtmosphericCondenserSystem.PowerRoomId));
            Assert.Equal(drawBefore + AtmosphericCondenserSystem.ArrayDrawWatts, grid.TotalDrawWatts, 1);
        }

        [Fact]
        public void HumidityIndex_IsDeterministic_ByWeatherKind()
        {
            Assert.Equal(1.0f, AtmosphericCondenserSystem.HumidityIndexFor(WeatherKind.Rain), 3);
            Assert.Equal(0.5f, AtmosphericCondenserSystem.HumidityIndexFor(WeatherKind.Clear), 3);
            Assert.Equal(0.2f, AtmosphericCondenserSystem.HumidityIndexFor(WeatherKind.Blizzard), 3);
        }

        [Fact]
        public void Condensing_YieldsWeatherIndexedRaw_NeverPotable()
        {
            var grid = MakeGrid();
            var water = new WaterTreatmentSystem();
            var c = new AtmosphericCondenserSystem(grid, water, MakeWeather(WeatherKind.Rain), NullLog.Instance);
            c.TryBuild(true, out _);

            c.TickDay(1);

            float expected = AtmosphericCondenserSystem.BaseYieldLitersPerDay * 1.0f;
            Assert.Equal(expected, water.State.rawWater, 1);
            Assert.Equal(0f, water.State.cleanWater); // treatment owns purification
            Assert.Equal(100f - AtmosphericCondenserSystem.MembraneWearPerCondensingDay,
                c.MembraneIntegrity, 3); // wear only on condensing days
        }

        [Fact]
        public void Condensing_Stops_WhenDisabled_OrUnpowered_OrMembraneSpent()
        {
            var grid = MakeGrid(generationWatts: 0f, batteryReserveWh: 0f);
            var water = new WaterTreatmentSystem();
            var c = new AtmosphericCondenserSystem(grid, water, MakeWeather(WeatherKind.Rain), NullLog.Instance);
            c.TryBuild(true, out _);

            c.TickDay(1); // unserved
            Assert.Equal(0L, c.TotalYieldLiters);
            Assert.Equal(100f, c.MembraneIntegrity, 3);

            var c2 = MakeCondenser(water: water);
            c2.TryBuild(true, out _);
            Assert.True(c2.SetEnabled(false, out _));
            c2.TickDay(2);
            Assert.Equal(0L, c2.TotalYieldLiters);

            // Spent membrane: no exchange surface, no yield, no wear.
            c2.State.membraneIntegrity = 0f;
            Assert.True(c2.SetEnabled(true, out _));
            c2.TickDay(3);
            Assert.Equal(0L, c2.TotalYieldLiters);
        }

        [Fact]
        public void Condensing_Skips_AdvisoryBlockedIntake()
        {
            var grid = MakeGrid();
            var water = new WaterTreatmentSystem();
            water.RegisterContaminationAdvisory("adv_c", "severe",
                new List<string> { AtmosphericCondenserSystem.SourceId });
            var c = new AtmosphericCondenserSystem(grid, water, MakeWeather(WeatherKind.Rain), NullLog.Instance);
            c.TryBuild(true, out _);

            c.TickDay(1);

            Assert.Equal(0L, c.TotalYieldLiters);
            Assert.Equal(100f, c.MembraneIntegrity, 3); // vapor never condensed
        }

        [Fact]
        public void ReplaceMembrane_RestoresIntegrity_BlockedWhenHealthyOrUnbuilt()
        {
            var c = MakeCondenser();
            Assert.False(c.ReplaceMembrane(out var unbuilt));
            Assert.Equal("not_built", unbuilt);

            c.TryBuild(true, out _);
            Assert.False(c.ReplaceMembrane(out var full)); // fresh membrane
            Assert.Equal("membrane_integrity_full", full);

            c.State.membraneIntegrity = 30f;
            Assert.True(c.ReplaceMembrane(out var ok), ok);
            Assert.Equal(100f, c.MembraneIntegrity, 1);
        }

        [Fact]
        public void SaveRoundTrip_PreservesLedger_AndReRegistersLoad_NoReplay()
        {
            var grid = MakeGrid();
            var water = new WaterTreatmentSystem();
            var c = new AtmosphericCondenserSystem(grid, water, MakeWeather(WeatherKind.Rain), NullLog.Instance);
            c.TryBuild(true, out _);
            c.TickDay(1);
            c.TickDay(2);
            var saved = c.CaptureState();

            var grid2 = MakeGrid();
            var water2 = new WaterTreatmentSystem();
            var c2 = new AtmosphericCondenserSystem(grid2, water2, MakeWeather(WeatherKind.Rain), NullLog.Instance);
            c2.RestoreState(saved);

            Assert.True(c2.IsBuilt);
            Assert.True(grid2.IsRoomServed(AtmosphericCondenserSystem.PowerRoomId));
            Assert.Equal(2L * 15L, c2.TotalYieldLiters);
            Assert.Equal(0f, water2.State.rawWater); // restore does not replay condensing
        }

        [Fact]
        public void LegacySave_RestoresUnbuiltDefaults_NoFreeArray()
        {
            const string legacy = "{\"systemId\":\"water_condenser\",\"schemaVersion\":1}";
            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<AtmosphericCondenserState>(legacy);
            Assert.NotNull(state);

            var grid = MakeGrid();
            var c = new AtmosphericCondenserSystem(grid, new WaterTreatmentSystem(),
                MakeWeather(WeatherKind.Rain), NullLog.Instance);
            c.RestoreState(state!);
            Assert.False(c.IsBuilt);
            Assert.False(grid.IsRoomServed(AtmosphericCondenserSystem.PowerRoomId));
        }
    }
}
