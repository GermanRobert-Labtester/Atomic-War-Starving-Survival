// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// SHELTER_HARDENING: catalog-driven surge tuning (ConfigureSurge) and the
    /// per-room draw query feeding the distribution subgrid.
    /// </summary>
    public sealed class PowerGridSurgeConfigurationTests
    {
        private static PowerGridSystem MakeSystem()
        {
            var rooms = new[]
            {
                new PowerGridRoom("room_foundry", "Foundry", 220f, PowerGridRoomPriority.Low),
                new PowerGridRoom("room_clinic", "Clinic", 120f, PowerGridRoomPriority.Critical),
                new PowerGridRoom("room_greenhouse", "Greenhouse", 160f, PowerGridRoomPriority.Standard),
            };
            var state = new PowerGridState
            {
                GenerationWatts = 800f,
                BatteryCapacityWh = 4000f,
                BatteryReserveWh = 4000f,
                FuelUnits = 100f
            };
            return new PowerGridSystem(state, rooms, new SeededRng(11));
        }

        [Fact]
        public void ConfigureSurge_ZeroSeverity_DisablesWeatherSurgePath()
        {
            var system = MakeSystem();
            system.ConfigureSurge(empStormSeverity: 0f, batteryDrainFraction: 0.15f);
            Assert.Empty(system.ApplySurgeDay(5, system.EmpStormSeverity));
            Assert.Equal(0, system.State.LastSurgeDay); // day not consumed
            Assert.Equal(4000f, system.State.BatteryReserveWh);
        }

        [Fact]
        public void ConfigureSurge_CatalogValuesDriveDrain()
        {
            var system = MakeSystem();
            // 4000 * 0.25 * 1.0 = 1000 drained at full severity.
            system.ConfigureSurge(empStormSeverity: 1f, batteryDrainFraction: 0.25f);
            system.ApplySurgeDay(5, system.EmpStormSeverity);
            Assert.Equal(3000f, system.State.BatteryReserveWh, 2);
        }

        [Fact]
        public void ConfigureSurge_ClampsOutOfRangeValues()
        {
            var system = MakeSystem();
            system.ConfigureSurge(empStormSeverity: 5f, batteryDrainFraction: -1f);
            Assert.Equal(1f, system.EmpStormSeverity);
            Assert.Equal(0f, system.SurgeBatteryDrain);
        }

        [Fact]
        public void GetRoomDrawWatts_PoweredReturnsCatalogDraw_TrippedReturnsZero()
        {
            var system = MakeSystem();
            Assert.Equal(220f, system.GetRoomDrawWatts("room_foundry"), 2);
            system.MarkTripped("room_foundry", 1);
            Assert.Equal(0f, system.GetRoomDrawWatts("room_foundry"), 2);
            Assert.Equal(0f, system.GetRoomDrawWatts("room_does_not_exist"), 2);
        }
    }
}
