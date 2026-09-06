using System;
using Ashfall.Core;
using Ashfall.Core.StartingLevel;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// SHELTER_FAILURE_EFFECTS_QUARANTINE_WIRING Phase 1: fx_filtration_off —
    /// air filtration responds to the room_air_filtration breaker state.
    /// Powered play is legacy-identical (default argument).
    /// </summary>
    public sealed class StartingLevelAirPowerTests
    {
        private static StartingLevelSystem MakeSystem()
        {
            var system = new StartingLevelSystem();
            // Fresh state: filter health 100, quality 100.
            return system;
        }

        [Fact]
        public void Powered_Degradation_MatchesLegacy()
        {
            var system = MakeSystem();
            float healthBefore = system.State.airFilterHealthPercent;
            system.TickDay(isFilterDutyAssigned: false, outdoorWeather: WeatherKind.Clear, powerAvailability01: 1f);
            Assert.Equal(healthBefore - 5f, system.State.airFilterHealthPercent);
            Assert.Equal(Math.Min(100f, system.State.airFilterHealthPercent * 0.9f + 10f),
                system.State.airQualityPercent);
        }

        [Fact]
        public void Unpowered_DegradationDoubles_DutyMitigationDisabled()
        {
            var powered = MakeSystem();
            var unpowered = MakeSystem();

            // Same duty mitigation, different power: unpowered adds +4 and zeroes duty.
            powered.TickDay(true, WeatherKind.Clear, 1f);
            unpowered.TickDay(true, WeatherKind.Clear, 0f);

            // Powered + duty: 5 * 0.5 = 2.5 degradation → 97.5 remaining.
            // Unpowered (duty zeroed): 5 + 4 = 9 degradation → 91 remaining.
            Assert.Equal(97.5f, powered.State.airFilterHealthPercent, 2);
            Assert.Equal(91f, unpowered.State.airFilterHealthPercent, 2);
        }

        [Fact]
        public void Unpowered_QualityLosesPoweredOffset()
        {
            var system = MakeSystem();
            // High filter health: powered quality sits at filter*0.9+10;
            // unpowered at filter*0.9 (offset gone).
            system.TickDay(false, WeatherKind.Clear, 0f);
            float expected = Math.Clamp(system.State.airFilterHealthPercent * 0.9f, 0f, 100f);
            Assert.Equal(expected, system.State.airQualityPercent, 2);
        }

        [Fact]
        public void UnpoweredStacks_WithHazardWeather()
        {
            var system = MakeSystem();
            // Clear + unpowered: 5 + 4 = 9. Hazard + unpowered: 5 + 4 + 4 = 13.
            system.TickDay(false, WeatherKind.FalloutStorm, 0f);
            Assert.Equal(100f - 13f, system.State.airFilterHealthPercent, 2);
        }

        [Fact]
        public void DefaultOverload_LegacyCallers_Powered()
        {
            var system = MakeSystem();
            system.TickDay(); // parameterless legacy
            Assert.Equal(95f, system.State.airFilterHealthPercent, 2);
        }

        [Fact]
        public void PartialPower_TreatedAsPowered()
        {
            var system = MakeSystem();
            // Any nonzero availability counts as powered (breaker-derived 0/1
            // today; the fraction form exists for future granularity).
            system.TickDay(false, WeatherKind.Clear, 0.5f);
            Assert.Equal(95f, system.State.airFilterHealthPercent, 2);
        }
    }
}
