// SPDX-License-Identifier: MIT
// A11Y-THEME-COVERAGE-2026-09-29 — static drift gate for the theme-coverage
// extension (plan .ai/plans/ui-theme-coverage-2026-09-29.md). If these
// sections disappear from AshfallUiTheme.Build(), the affected control types
// regress to Godot's light default art on ink panels.
// Pattern: MainTriadDriftGateTests / UiA11yTargetSizeSweep2GateTests.
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiThemeCoverageGateTests
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

        private static string ReadSrc(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        [Theory]
        [InlineData("ProgressBar", "\"background\", \"ProgressBar\"", "\"fill\", \"ProgressBar\"")]
        [InlineData("OptionButton", "\"arrow\", \"OptionButton\"", "\"arrow\", \"OptionButton\"")]
        [InlineData("SpinBox", "\"updown\", \"SpinBox\"", "\"updown\", \"SpinBox\"")]
        [InlineData("TabContainer", "\"tab_selected\", \"TabContainer\"", "\"panel\", \"TabContainer\"")]
        [InlineData("TabBar", "\"tab_selected\", \"TabBar\"", "\"tab_unselected\", \"TabBar\"")]
        [InlineData("RichTextLabel", "\"default_color\", \"RichTextLabel\"", "\"normal_font_size\", \"RichTextLabel\"")]
        public void ThemeCoversControlType(string typeName, string requiredItem, string requiredItem2)
        {
            string src = ReadSrc("src", "UI", "AshfallUiTheme.cs");

            Assert.Contains(requiredItem, src);
            Assert.Contains(requiredItem2, src);

            // Coverage must live inside Build() (installed at boot), not be
            // dead code appended elsewhere in the file.
            int build = src.IndexOf("private static Godot.Theme Build()", StringComparison.Ordinal);
            int item = src.IndexOf(requiredItem, StringComparison.Ordinal);
            Assert.True(item > build, $"{typeName} coverage must be inside Theme Build()");
        }

        // CheckBox/CheckButton icons are registered through one loop over the
        // two type names; assert the loop and the icon item names.
        [Fact]
        public void ThemeCoversCheckFamilyIcons()
        {
            string src = ReadSrc("src", "UI", "AshfallUiTheme.cs");

            Assert.Contains("new[] { \"CheckBox\", \"CheckButton\" }", src);
            Assert.Contains("SetIcon(\"checked\", type", src);
            Assert.Contains("SetIcon(\"unchecked\", type", src);
        }

        [Fact]
        public void SnapshotOrchestrator_RunsDefaultsWalk()
        {
            string src = ReadSrc("src", "UI", "SnapshotOrchestrator.cs");

            Assert.Contains("AshfallUiTheme.InstallOn(root)", src);
            Assert.Contains("AshfallUiTheme.EnforceControlDefaults(root)", src);
        }
    }
}
