using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class CampaignCalendarSourceGateTests
    {
        [Fact]
        public void SourceGate_MainPartialsDoNotDirectlyAssignSimDay()
        {
            string? root = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(root) && !Directory.Exists(Path.Combine(root, "src")))
            {
                var parent = Directory.GetParent(root);
                root = parent?.FullName;
            }

            if (string.IsNullOrEmpty(root) || !Directory.Exists(Path.Combine(root, "src")))
                return; // Not running in repo tree

            var violations = new List<string>();

            // Pattern checking for direct mutations of _simDay (excluding declaration and lambda/comparison)
            var directAssignRegex = new Regex(@"\b_simDay\s*(\+\+|--|\+=|-=|\*=|/=|=(?!=|>))\s*", RegexOptions.Compiled);

            foreach (var file in Directory.GetFiles(Path.Combine(root, "src"), "Main*.cs", SearchOption.TopDirectoryOnly))
            {
                string[] lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    // Skip read-only property definition: private int _simDay => ...
                    if (line.Contains("int _simDay =>")) continue;
                    // Skip comments
                    if (line.StartsWith("//") || line.StartsWith("/*") || line.StartsWith("*")) continue;

                    if (directAssignRegex.IsMatch(line))
                    {
                        violations.Add($"{Path.GetFileName(file)}:L{i + 1}: {line}");
                    }
                }
            }

            Assert.Empty(violations);
        }

        [Fact]
        public void SourceGate_UiPanelsDoNotDirectlyAdvanceDays()
        {
            string? root = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(root) && !Directory.Exists(Path.Combine(root, "src", "UI")))
            {
                var parent = Directory.GetParent(root);
                root = parent?.FullName;
            }

            if (string.IsNullOrEmpty(root) || !Directory.Exists(Path.Combine(root, "src", "UI")))
                return;

            var violations = new List<string>();

            foreach (var file in Directory.GetFiles(Path.Combine(root, "src", "UI"), "*.cs", SearchOption.AllDirectories))
            {
                string[] lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (line.StartsWith("//") || line.StartsWith("/*") || line.StartsWith("*")) continue;

                    if (line.Contains("AdvanceDays("))
                    {
                        violations.Add($"{Path.GetFileName(file)}:L{i + 1}: {line}");
                    }
                }
            }

            Assert.Empty(violations);
        }

        /// <summary>
        /// Task #112 substep 10: the calendar has exactly one writer (the
        /// coordinator, via Advance/RestoreState). Host partials and UI panels
        /// must not write the calendar, advance any clock by hand, or
        /// self-increment a sim-day value. Allowlist: Main.Holdfast.cs owns
        /// the coordinator call sites and the clock&lt;-calendar sync points;
        /// Main.UiTests.* drivers legitimately seed/advance state for
        /// headless scenarios.
        /// </summary>
        [Fact]
        public void SourceGate_CalendarWritesAndHandAdvanceAreCoordinatorOnly()
        {
            string? root = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(root) && !Directory.Exists(Path.Combine(root, "src")))
            {
                var parent = Directory.GetParent(root);
                root = parent?.FullName;
            }

            if (string.IsNullOrEmpty(root) || !Directory.Exists(Path.Combine(root, "src")))
                return; // Not running in repo tree

            var violations = new List<string>();
            var files = new List<string>(Directory.GetFiles(Path.Combine(root, "src"), "Main*.cs", SearchOption.TopDirectoryOnly));
            files.AddRange(Directory.GetFiles(Path.Combine(root, "src", "UI"), "*.cs", SearchOption.AllDirectories));

            foreach (var file in files)
            {
                string name = Path.GetFileName(file);
                string rel = Path.GetRelativePath(Path.Combine(root, "src"), file).Replace('\\', '/');
                bool isUi = rel.StartsWith("UI/", StringComparison.Ordinal);
                bool isUiTests = name.StartsWith("Main.UiTests.", StringComparison.Ordinal);
                bool calendarWriteAllowed = name == "Main.Holdfast.cs" || isUiTests;

                string[] lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (line.StartsWith("//") || line.StartsWith("/*") || line.StartsWith("*")) continue;

                    if (!calendarWriteAllowed && line.Contains("Calendar.SetDay("))
                        violations.Add($"{rel}:L{i + 1}: Calendar.SetDay is coordinator-only — {line}");

                    if (line.Contains(".AdvanceDays("))
                        violations.Add($"{rel}:L{i + 1}: hand-advancing a clock is forbidden — route through the coordinator — {line}");

                    // Self-mutating a sim day (UI panels hold host-pushed display
                    // fields; neither Main partials nor panels may increment them).
                    if (Regex.IsMatch(line, @"\bsimDay\s*(\+\+|--|\+=|-=)", RegexOptions.IgnoreCase))
                        violations.Add($"{rel}:L{i + 1}: sim-day self-mutation forbidden — {line}");
                }
            }

            Assert.True(violations.Count == 0,
                "Campaign-calendar authority violations found:\n  " + string.Join("\n  ", violations));
        }
    }
}
