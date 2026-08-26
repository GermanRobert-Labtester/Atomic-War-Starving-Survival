using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests
{
    public class PowerGridDeterminismTests
    {
        private static PowerGridSystem CreateSystem(int seed, float fuel)
        {
            var state = new PowerGridState { FuelUnits = fuel, BatteryReserveWh = 500, BatteryCapacityWh = 1000 };
            var rooms = new List<PowerGridRoom> { new PowerGridRoom { RoomId = "room_a", DisplayName = "Test", DrawWatts = 100 } };
            return new PowerGridSystem(state, rooms, new SeededRng(seed));
        }

        [Fact]
        public void SameSeed_Determinism_FuelAndBatteryIdentical()
        {
            var a = CreateSystem(42, 100);
            var b = CreateSystem(42, 100);
            var rngA = new SeededRng(42);
            var rngB = new SeededRng(42);
            for (int day = 1; day <= 30; day++)
            {
                a.TickDay(day, rngA);
                b.TickDay(day, rngB);
                Assert.Equal(a.FuelUnits, b.FuelUnits, precision: 3);
                Assert.Equal(a.BatteryReserveWh, b.BatteryReserveWh, precision: 3);
            }
        }

        [Theory]
        [InlineData(42, 30)]
        [InlineData(999, 30)]
        public void NumericalSafety_NoNaNOrNegative(int seed, int days)
        {
            var sys = CreateSystem(seed, 100);
            var rng = new SeededRng(seed);
            for (int day = 1; day <= days; day++)
            {
                sys.TickDay(day, rng);
                Assert.False(float.IsNaN(sys.FuelUnits), $"NaN fuel day {day}");
                Assert.False(float.IsInfinity(sys.FuelUnits), $"Infinity fuel day {day}");
                Assert.True(sys.FuelUnits >= 0, $"Negative fuel {sys.FuelUnits}");
                Assert.False(float.IsNaN(sys.BatteryReserveWh), $"NaN battery day {day}");
                Assert.True(sys.BatteryReserveWh >= 0, $"Negative battery {sys.BatteryReserveWh}");
                Assert.True(sys.BatteryReserveWh <= sys.BatteryCapacityWh + 1e-3f, $"Battery over capacity {sys.BatteryReserveWh} > {sys.BatteryCapacityWh}");
            }
        }

        [Fact]
        public void DifferentSeed_Divergence_Allowed_ButBounded()
        {
            var a = CreateSystem(42, 100);
            var b = CreateSystem(999, 100);
            var rngA = new SeededRng(42);
            var rngB = new SeededRng(999);
            for (int day = 1; day <= 30; day++)
            {
                a.TickDay(day, rngA);
                b.TickDay(day, rngB);
            }
            // Both valid, may diverge due to 5% spike randomness
            Assert.True(a.FuelUnits >= 0 && a.FuelUnits <= 100);
            Assert.True(b.FuelUnits >= 0 && b.FuelUnits <= 100);
            Assert.False(float.IsNaN(a.BatteryReserveWh));
        }
    }
}
