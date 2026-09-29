// SPDX-License-Identifier: MIT
// A11Y-LIFECYCLE-BYPASS-2026-09-29 — static drift gate for pkg 12 (plan
// .ai/plans/ui-lifecycle-bypass-2026-09-29.md). Bare `<panel>.Visible = true`
// opens skip the defaults walk, open animation, AND EnsureInitialFocus — the
// class is eliminated and this gate prevents it regrowing. Also pins the
// LineEdit target floor in the defaults walk.
// Pattern: MainTriadDriftGateTests / UiA11yTargetSizeSweep2GateTests.
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiLifecycleBypassGateTests
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

        // Boot-built, always-visible roots with their own focus flows; showing
        // them is not a panel "open" and must not animate/refocus.
        private static readonly string[] ExcludedFiles =
        {
            "src/Main.PanelLifecycle.cs" // owns ShowPanelLifecycle itself
        };

        private static readonly string[] ExcludedReceivers =
        {
            "_mainMenu", "_dashboard", "_gameOver", "_gameUiContainer",
            "_hudOverlay", "_feedbackPanel", "_confirmationModal", "_crisisHud"
        };

        [Fact]
        public void NoMainPartial_BypassesShowPanelLifecycle()
        {
            string srcDir = Path.Combine(RepoRoot(), "src");
            var violations = new System.Collections.Generic.List<string>();

            foreach (string file in Directory.EnumerateFiles(srcDir, "Main.*.cs", SearchOption.TopDirectoryOnly))
            {
                string rel = Path.GetRelativePath(RepoRoot(), file).Replace('\\', '/');
                if (ExcludedFiles.Contains(rel))
                    continue;

                string src = File.ReadAllText(file);
                foreach (Match m in Regex.Matches(src, @"(\w+)\.Visible = true;"))
                {
                    string receiver = m.Groups[1].Value;
                    int line = src[..m.Index].Count(c => c == '\n') + 1;
                    if (!ExcludedReceivers.Contains(receiver))
                        violations.Add($"{rel}:{line} {receiver}.Visible = true;");
                }
            }

            Assert.True(violations.Count == 0,
                "Panel opens must go through ShowPanelLifecycle (defaults walk + " +
                "AnimateOpen + EnsureInitialFocus). Bypasses found:\n" +
                string.Join("\n", violations));
        }

        [Fact]
        public void EnforceControlDefaults_FloorsLineEdits()
        {
            string src = File.ReadAllText(Path.Combine(RepoRoot(), "src", "UI", "AshfallUiTheme.cs"));

            Assert.Contains("root is LineEdit edit", src);
            Assert.Contains("edit.CustomMinimumSize.Y < DesignTheme.MinInteractiveHeight", src);
            Assert.Contains("edit.CustomMinimumSize.X, DesignTheme.MinInteractiveHeight", src);
        }
    }
}
