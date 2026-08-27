using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class CampaignDayCoordinatorSourceGateTests
    {
        [Fact]
        public void StandardPhases_CoverAllFiveSimulationPhases()
        {
            var coord = new CampaignDayCoordinator();

            // Register standard 5-phase owners
            coord.Register("weather_world", new DummyOwner(), phase: 1);
            coord.Register("holdfast_core", new DummyOwner(), phase: 1);
            coord.Register("crafting_production", new DummyOwner(), phase: 2);
            coord.Register("starting_level_rations", new DummyOwner(), phase: 2);
            coord.Register("survivors_needs", new DummyOwner(), phase: 3);
            coord.Register("duty_roster", new DummyOwner(), phase: 3);
            coord.Register("expeditions_caravans", new DummyOwner(), phase: 4);
            coord.Register("narrative_quests_verdict", new DummyOwner(), phase: 4);
            coord.Register("host_events", new DummyOwner(), phase: 5);
            coord.Register("memorial", new DummyOwner(), phase: 5);

            Assert.Equal(10, coord.Owners.Count);

            var result = coord.Advance(1);
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(10, result.OwnerCount);
        }

        [Fact]
        public void FailClosed_EnsuresAtomicDayProgression()
        {
            var coord = new CampaignDayCoordinator();
            coord.Register("phase1_good", new DummyOwner(), phase: 1);
            coord.Register("phase3_faulty", new CrashingOwner(), phase: 3);
            coord.Register("phase5_good", new DummyOwner(), phase: 5);

            var capture = new CapturingPersistence();
            var result = coord.Advance(1, capture, failClosed: true);

            Assert.NotNull(result);
            Assert.True(result.HasFailures);
            Assert.False(result.Succeeded);
            Assert.Equal(-1, coord.LastAdvancedDay); // Not committed
            Assert.Equal(0, capture.Day); // Persistence aborted
        }

        [Fact]
        public void SourceGate_UiPanelsDoNotInvokeDirectDailyTickMonolith()
        {
            string? root = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(root) && !Directory.Exists(Path.Combine(root, "src", "UI")))
            {
                var parent = Directory.GetParent(root);
                root = parent?.FullName;
            }

            if (string.IsNullOrEmpty(root) || !Directory.Exists(Path.Combine(root, "src", "UI")))
                return; // Not running in repo tree

            string uiDir = Path.Combine(root, "src", "UI");
            var violations = new List<string>();

            // UI Panels should not directly call TickSimDay
            foreach (var file in Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories))
            {
                string text = File.ReadAllText(file);
                if (text.Contains("TickSimDay("))
                {
                    violations.Add($"{Path.GetFileName(file)}: directly calls TickSimDay");
                }
            }

            Assert.Empty(violations);
        }

        private sealed class DummyOwner : IDayAdvanceOwner
        {
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events) { }
        }

        private sealed class CrashingOwner : IDayAdvanceOwner
        {
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                throw new InvalidOperationException("intentional simulated fault");
            }
        }

        private sealed class CapturingPersistence : IDayAdvancePersistence
        {
            public int Day;
            public void PersistBeforeBriefing(int day, IReadOnlyList<DayOwnerReport> ownerReports)
            {
                Day = day;
            }
        }
    }
}
