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

        [Fact]
        public void SourceGate_DirectDailyAdvanceOnlyViaCoordinator()
        {
            // Initiative #111 substep 12: no new direct daily ticks outside the
            // coordinator. Main partials and UI panels must reach the day
            // advance through Main.Holdfast.cs (which owns TickSimDay and the
            // two coordinator Advance call sites). UiTests partials are the
            // sanctioned exception — they drive the real coordinator paths.
            string? root = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(root) && !Directory.Exists(Path.Combine(root, "src")))
            {
                var parent = Directory.GetParent(root);
                root = parent?.FullName;
            }

            if (string.IsNullOrEmpty(root) || !Directory.Exists(Path.Combine(root, "src")))
                return; // Not running in repo tree

            string srcDir = Path.Combine(root, "src");
            var violations = new List<string>();

            foreach (var file in Directory.GetFiles(srcDir, "*.cs", SearchOption.AllDirectories))
            {
                string name = Path.GetFileName(file);
                string rel = Path.GetRelativePath(srcDir, file).Replace('\\', '/');
                bool isMainPartial = name.StartsWith("Main.", StringComparison.Ordinal);
                bool isUi = rel.StartsWith("UI/", StringComparison.Ordinal);
                if (!isMainPartial && !isUi) continue;
                if (name.EndsWith("Tests.cs", StringComparison.Ordinal)) continue;

                bool advanceAllowed = name == "Main.Holdfast.cs";
                bool tickAllowed = advanceAllowed || name.StartsWith("Main.UiTests.", StringComparison.Ordinal);
                if (advanceAllowed && tickAllowed) continue;

                string text = File.ReadAllText(file);
                if (!advanceAllowed && text.Contains("_campaignDay.Advance("))
                    violations.Add($"{rel}: calls _campaignDay.Advance directly (only Main.Holdfast.cs may)");
                if (!tickAllowed && text.Contains("TickSimDay("))
                    violations.Add($"{rel}: calls TickSimDay (only Main.Holdfast.cs and UiTests selftests may)");
            }

            Assert.True(violations.Count == 0,
                "Direct daily-advance violations found — route day advancement through the coordinator:\n  " +
                string.Join("\n  ", violations));
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
