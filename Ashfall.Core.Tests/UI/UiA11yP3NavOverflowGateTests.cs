// SPDX-License-Identifier: MIT
// A11Y-P3-NAV-OVERFLOW-2026-09-29 — static drift gate for the UI
// accessibility P3 package (plan .ai/plans/ui-a11y-p3-nav-overflow-2026-09-29.md,
// findings docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md §5b/§9.3/§9.9).
// Pattern: MainTriadDriftGateTests / UiA11yP1InputGateTests.
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiA11yP3NavOverflowGateTests
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

        private static string Slice(string src, string startMarker, string endMarker)
        {
            int start = src.IndexOf(startMarker, StringComparison.Ordinal);
            Assert.True(start >= 0, $"marker not found: {startMarker}");
            int end = src.IndexOf(endMarker, start + 1, StringComparison.Ordinal);
            if (end < 0) end = src.Length;
            return src.Substring(start, end - start);
        }

        [Fact]
        public void Main_HasTopmostOverlayHelper_AndTabTrap()
        {
            string lifecycle = ReadSrc("src", "Main.PanelLifecycle.cs");

            string helper = Slice(
                lifecycle,
                "private Control? TopmostVisibleOverlayPanel()",
                "public override void _Input(InputEvent @event)");
            Assert.Contains("OverlayPanelCatalog()", helper);
            // Panels already mid-close must not own the scope.
            Assert.Contains("UiMotion.IsClosing(panel)", helper);

            string tabTrap = Slice(
                lifecycle,
                "public override void _Input(InputEvent @event)",
                "private void CloseSettingsPanel()");
            Assert.Contains("AshfallFocusPolicy.TrapFocus(topmost, @event)", tabTrap);
            // Self-handling Tab panels stay excluded.
            Assert.Contains("topmost is CombatPanel or DailyBriefingModal", tabTrap);
        }

        [Fact]
        public void ArrowNavigation_IsScopedToTheOpenOverlay()
        {
            // Nav scope root must be the topmost visible overlay when one is
            // open, not the whole Main tree (arrows could wander behind it).
            string app = ReadSrc("src", "Main.Application.cs");
            Assert.Contains(
                "AshfallFocusNavigator.HandleNavInput(TopmostVisibleOverlayPanel() ?? (Control)this, @event)",
                app);
            Assert.DoesNotContain("AshfallFocusNavigator.HandleNavInput(this, @event)", app);
        }

        [Theory]
        [InlineData("private static Label MakeHeaderLabel(Column col)", "private static Control MakeCellControl(Cell cell, Column col)")]
        [InlineData("var lbl = new Label\n        {\n            Text = cell.Text ?? string.Empty,", "lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;")]
        public void DataGridLabels_EllipsizeAtColumnEdge(string startMarker, string endMarker)
        {
            string block = Slice(
                ReadSrc("src", "UI", "AshfallDataGrid.cs"),
                startMarker,
                endMarker);
            Assert.Contains("ClipText = true", block);
            Assert.Contains("TextServer.OverrunBehavior.TrimEllipsis", block);
        }

        [Fact]
        public void DashboardShellTitle_EllipsizesBeforeCloseButton()
        {
            string block = Slice(
                ReadSrc("src", "UI", "AshfallDashboardShell.cs"),
                "_titleLabel = new Label",
                "_titleLabel.AddThemeFontSizeOverride");
            Assert.Contains("ClipText = true", block);
            Assert.Contains("TextServer.OverrunBehavior.TrimEllipsis", block);
        }
    }
}
