// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: first-hour stage panel accessibility coverage (T05).
//
// The seven first-hour onboarding stages route a new player to seven panels.
// This gate pins the route -> panel contract and proves both host self-tests
// cover them: the --ui-accessibility-selftest representative set (Gate 6) and
// the --ui-layout-selftest focusability corpus. A future stage added without
// its panel audit fails here instead of shipping an unreachable surface.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Onboarding;

namespace Ashfall.Core.Tests.UI
{
    public sealed class FirstHourStagePanelAccessibilityGateTests
    {
        private static readonly IReadOnlyDictionary<string, string> ExpectedRouteToPanel =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["water_treatment"] = "WaterTreatmentPanel",
                ["power_grid"] = "PowerGridPanel",
                ["inventory"] = "InventoryPanel",
                ["duty_roster"] = "DutyRosterPanel",
                ["dose_ledger"] = "DoseLedgerPanel",
                ["research"] = "ResearchPanel",
                ["expeditions"] = "ExpeditionPanel",
            };

        private static string RepoRoot()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory,
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    if (File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj"))
                        && Directory.Exists(Path.Combine(dir.FullName, "src", "Host")))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string ReadRepoFile(params string[] parts) =>
            File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        private static IReadOnlyList<(OnboardingStage Stage, string Route)> FirstHourRoutes() =>
            OnboardingCatalog.FirstHourOrder
                .Select(stage => (stage, OnboardingCatalog.DefFor(OnboardingProfile.FirstHour, stage).ShowMeWhereRoute))
                .ToList();

        [Fact]
        public void FirstHourStageRoutes_MatchThePinnedPanelMap()
        {
            var actual = FirstHourRoutes().Select(entry => entry.Route).ToList();
            var expected = ExpectedRouteToPanel.Keys.ToList();

            Assert.Equal(7, actual.Count);
            Assert.Equal(
                expected.OrderBy(route => route, StringComparer.Ordinal).ToList(),
                actual.OrderBy(route => route, StringComparer.Ordinal).ToList());
        }

        [Fact]
        public void AccessibilitySelfTest_CoversEveryFirstHourStagePanel()
        {
            string a11y = ReadRepoFile("src", "Host", "UiAccessibilitySelfTest.cs");
            var errors = new List<string>();

            foreach (var (stage, route) in FirstHourRoutes())
            {
                if (!ExpectedRouteToPanel.TryGetValue(route, out string? panel))
                {
                    errors.Add($"{stage}: route '{route}' is not in the pinned panel map");
                    continue;
                }
                if (!a11y.Contains($"[\"{route}\"] = \"{panel}\"", StringComparison.Ordinal))
                    errors.Add($"{stage}: a11y self-test route map is missing '{route}' -> '{panel}'");
                if (!a11y.Contains($"new {panel}()", StringComparison.Ordinal))
                    errors.Add($"{stage}: a11y self-test does not construct {panel}");
            }

            Assert.True(errors.Count == 0,
                "First-hour stage panels not covered by --ui-accessibility-selftest:\n"
                + string.Join("\n", errors));
        }

        [Fact]
        public void FocusabilityGate_IncludesEveryFirstHourStagePanel()
        {
            string gate = ReadRepoFile("src", "Host", "HostCli.Command.RunUiLayoutSelfTest.cs");
            var missing = ExpectedRouteToPanel.Values
                .Distinct(StringComparer.Ordinal)
                .Where(panel => !gate.Contains($"\"{panel}\"", StringComparison.Ordinal))
                .ToList();

            Assert.True(missing.Count == 0,
                "First-hour stage panels missing from the --ui-layout-selftest focusability corpus: "
                + string.Join(", ", missing));
        }
    }
}
