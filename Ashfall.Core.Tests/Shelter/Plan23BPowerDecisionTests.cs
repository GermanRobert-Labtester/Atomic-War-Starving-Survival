// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// C2[6] 23B — player load-shedding and consequence legibility contracts:
    /// the canonical rising-water clock, breaker-trip recovery semantics, and the
    /// attributed automatic-vs-player shed briefing lines.
    /// </summary>
    public class Plan23BPowerDecisionTests
    {
        // ---- rising-water clock ---------------------------------------------

        private static SumpFloodingSystem MakeSump(out PowerGridSystem power)
        {
            var weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 42);
            var state = new PowerGridState
            {
                GenerationWatts = 800,
                FuelUnits = 100,
                BatteryCapacityWh = 4000,
                BatteryReserveWh = 2000
            };
            power = new PowerGridSystem(state, new List<PowerGridRoom>
            {
                new PowerGridRoom("sump_a", "Lower Level", 100f)
            }, new SeededRng(42));
            return new SumpFloodingSystem(new SeededRng(42), weather, power, new YearOfAshDeepFreezeSystem());
        }

        [Fact]
        public void SumpRisk_RisingWithoutPump_ReportsFiniteEta()
        {
            var sump = MakeSump(out _);
            sump.AddNode("sump_a", "Lower Level");
            sump.State.globalGroundwaterLevel = 100f; // inflow 50 cm/day, no pump

            sump.TickDay(1);

            var risk = sump.GetRisk("sump_a");
            Assert.True(risk.NodeExists);
            Assert.False(risk.IsFlooded);
            Assert.True(risk.NetChangeCmPerDay > 0f, $"level {risk.LevelCm} should rise");
            Assert.True(risk.HoursToThreshold > 0f && !float.IsPositiveInfinity(risk.HoursToThreshold),
                $"expected finite ETA, got {risk.HoursToThreshold}");
        }

        [Fact]
        public void SumpRisk_ServedPumpDrainingCompetesWithInflow()
        {
            var sump = MakeSump(out _);
            sump.AddNode("sump_a", "Lower Level");
            sump.InstallPump("sump_a");
            sump.SetNodePower("sump_a", true);
            sump.State.globalGroundwaterLevel = 20f; // inflow 10 cm/day < pump 20 cm/day

            sump.TickDay(1);

            var risk = sump.GetRisk("sump_a");
            Assert.True(risk.NetChangeCmPerDay <= 0f, $"net {risk.NetChangeCmPerDay} should not rise");
            Assert.True(float.IsPositiveInfinity(risk.HoursToThreshold));
        }

        [Fact]
        public void SumpRisk_UnknownNode_IsSafe()
        {
            var sump = MakeSump(out _);
            var risk = sump.GetRisk("nope");
            Assert.False(risk.NodeExists);
            Assert.True(float.IsPositiveInfinity(risk.HoursToThreshold));
        }

        // ---- breaker trip recovery ------------------------------------------

        [Fact]
        public void BreakerTrip_BlocksThenResetRestoresRoom()
        {
            var sump = MakeSump(out var power); // reuse room sump_a (100 W, served by 800 W)
            Assert.True(power.IsRoomPowered("sump_a"));

            power.MarkTripped("sump_a", 3);
            Assert.True(power.IsRoomTripped("sump_a"));
            Assert.False(power.IsRoomPowered("sump_a"));

            // Legacy toggle does NOT clear a trip (the defect the reset action fixes).
            power.ToggleBreaker("sump_a");
            Assert.True(power.IsRoomTripped("sump_a"));

            power.ClearTripped("sump_a");
            Assert.False(power.IsRoomTripped("sump_a"));
            power.SetBreaker("sump_a", true); // re-close the breaker the test opened above
            Assert.True(power.IsRoomPowered("sump_a"));
        }

        // ---- attribution ----------------------------------------------------

        private static string Flatten(DailyBriefingReport report)
        {
            var lines = new List<string>();
            if (report?.Sections == null) return string.Empty;
            foreach (var section in report.Sections)
            {
                if (section?.Entries == null) continue;
                foreach (var entry in section.Entries)
                    if (entry != null) lines.Add(entry.Text);
            }
            return string.Join("\n", lines);
        }

        [Fact]
        public void Briefing_AutomaticAndPlayerShed_AreDistinguishable()
        {
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(5, 5, new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("power_shed_automatic", "power_grid", "room_foundry", null, 220f),
                new DayStateChangeEvent("power_shed_player", "power_grid", "room_workshop"),
                new DayStateChangeEvent("power_critical_deficit", "power_grid", "life_support", null, 90f)
            });

            string text = Flatten(report);
            Assert.Contains("automatically", text);
            Assert.Contains("You shut down", text);
            Assert.Contains("LIFE SUPPORT", text);
        }

        [Fact]
        public void Briefing_BrownoutEdges_ArePresent()
        {
            var report = DailyBriefingReportBuilder.BuildFromDayEvents(6, 6, new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("power_brownout_began", "power_grid"),
                new DayStateChangeEvent("power_brownout_restored", "power_grid")
            });
            string text = Flatten(report);
            Assert.Contains("Brownout", text);
            Assert.Contains("Power restored", text);
        }
    }
}