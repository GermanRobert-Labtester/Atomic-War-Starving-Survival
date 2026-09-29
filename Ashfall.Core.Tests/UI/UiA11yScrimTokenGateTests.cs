// SPDX-License-Identifier: MIT
// A11Y-SCRIM-TOKEN-2026-09-29 — static drift gate for the panel-scrim
// consolidation (plan .ai/plans/ui-a11y-scrim-token-2026-09-29.md, findings
// docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md §2d).
// Pattern: MainTriadDriftGateTests / UiA11yP1InputGateTests.
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiA11yScrimTokenGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "src")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        [Fact]
        public void Theme_DefinesTheStrongScrimTier()
        {
            string src = File.ReadAllText(Path.Combine(RepoRoot(), "Assets", "Ashfall.Core", "UI", "Theme.cs"));
            Assert.Matches(@"public static readonly \(float r, float g, float b, float a\) InkPanelStrong = \(0\.035f, 0\.043f, 0\.047f, 0\.92f\);", src);
        }

        [Fact]
        public void HostHelper_ExposesPanelScrim()
        {
            string src = File.ReadAllText(Path.Combine(RepoRoot(), "src", "UI", "AshfallUiHelpers.cs"));
            Assert.Contains("public static Color PanelScrim() => ToColor(Theme.InkPanelStrong);", src);
        }

        [Fact]
        public void NoHandRolledNearGreyScrims_RemainInSrcUi()
        {
            // Near-grey full-alpha-range scrims must come from
            // AshfallUiHelpers.PanelScrim() (Theme.InkPanelStrong), not hand
            // rolled literals.
            var regex = new Regex(@"new Color\(0\.0[2-7]f, 0\.0[2-7]f, 0\.0[2-7]f, 0\.\d+f\)", RegexOptions.Compiled);

            // Deliberate exceptions (documented in the plan):
            // - MapDetailPanel 0.74 scene-backdrop: §3 stack-dependent contrast
            //   follow-up, design intent is a visible scene behind the panel.
            var allowlist = new (string File, string Fragment)[]
            {
                ("MapDetailPanel.cs", "new Color(0.03f, 0.04f, 0.05f, 0.74f)"),
            };

            string uiDir = Path.Combine(RepoRoot(), "src", "UI");
            var offenders = Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories)
                .SelectMany(f => File.ReadAllLines(f)
                    .Select((text, i) => (File: f, Line: i + 1, Text: text))
                    .Where(x => regex.IsMatch(x.Text)
                                && !allowlist.Any(a => x.File.EndsWith(a.File, StringComparison.Ordinal)
                                                       && x.Text.Contains(a.Fragment, StringComparison.Ordinal))))
                .ToList();

            Assert.True(offenders.Count == 0,
                "Hand-rolled near-grey scrim literals must use AshfallUiHelpers.PanelScrim() " +
                "(Theme.InkPanelStrong). Offenders:\n" +
                string.Join("\n", offenders.Select(o => $"{o.File}:{o.Line}: {o.Text.Trim()}")));
        }
    }
}
