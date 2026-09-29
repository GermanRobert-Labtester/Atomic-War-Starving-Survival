// SPDX-License-Identifier: MIT
// A11Y-P2.5-SIDEBAR-HOVER-OVERFLOW-2026-09-29 — static drift gate for the UI
// accessibility package (plan .ai/plans/ui-a11y-sidebar-hover-overflow-2026-09-29.md,
// findings docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md §9.5/§7/§6).
// Pattern: MainTriadDriftGateTests / UiA11yP1InputGateTests.
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiA11ySidebarHoverOverflowGateTests
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
        public void SidebarRows_AreFocusableButtons_NotMouseOnlyPanels()
        {
            string src = ReadSrc("src", "UI", "AshfallSidebar.cs");
            // Row container must be a Button (focus navigator + Tab trap only
            // reach Button-family controls) with visible focus + hover.
            Assert.Contains("var row = new Button { Text = string.Empty };", src);
            Assert.Contains("row.AddThemeStyleboxOverride(\"focus\", AshfallFocusPolicy.MakeFocusVisibleStyleBox());", src);
            Assert.Contains("row.AddThemeStyleboxOverride(\"hover\", hoverSb);", src);
            Assert.Contains("row.Pressed += () => Select(item.Id);", src);
            Assert.DoesNotContain("var row = new PanelContainer();", src);
            // Highlight logic must follow the row type change.
            Assert.Contains("child is Button row", src);
            Assert.DoesNotContain("child is PanelContainer row", src);
        }

        [Fact]
        public void DataGridSelectableRows_HaveHoverFeedback()
        {
            string block = Slice(
                ReadSrc("src", "UI", "AshfallDataGrid.cs"),
                "AshfallFocusPolicy.ApplyFocusVisibleStyle(panel);",
                "return panel;");
            Assert.Contains("panel.MouseEntered +=", block);
            Assert.Contains("panel.MouseExited +=", block);
            Assert.Contains("ApplyRowStyle(panel, row);", block);
        }

        [Theory]
        [InlineData("Economy/TradeScreenGodotPanel.cs", "Vector2(100, 0) };")]
        [InlineData("Economy/TradeScreenGodotPanel.cs", "Vector2(120, 0) };")]
        [InlineData("Economy/TradeScreenGodotPanel.cs", "Vector2(140, 0) };")]
        [InlineData("UI/AshfallMetricCard.cs", "VerticalAlignment = VerticalAlignment.Center\n        };")]
        [InlineData("UI/SurvivorsPanel.cs", "nameLbl.CustomMinimumSize = new Vector2(140, 0);\n                row.AddChild(nameLbl);")]
        [InlineData("UI/GameDashboardPanel.cs", "name.CustomMinimumSize = new Vector2(74, 0);\n            row.AddChild(name);")]
        public void FixedWidthLabels_ClipWithEllipsis(string path, string oldShape)
        {
            // The pre-fix label shapes (no clip between min-size and add-child)
            // must not exist at the audited sites anymore.
            string src = ReadSrc("src", path.Replace('/', Path.DirectorySeparatorChar));
            Assert.DoesNotContain(oldShape, src);
            Assert.Contains("TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis", src);
        }
    }
}
