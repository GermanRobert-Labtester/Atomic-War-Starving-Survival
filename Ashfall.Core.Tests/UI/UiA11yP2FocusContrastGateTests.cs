// SPDX-License-Identifier: MIT
// A11Y-P2-FOCUS-CONTRAST-2026-09-29 — static drift gate for the UI
// accessibility P2 package (plan .ai/plans/ui-a11y-p2-focus-contrast-2026-09-29.md,
// findings docs/ui/ACCESSIBILITY_REPORT_2026-09-29.md §9.4/§9.8).
// Pattern: MainTriadDriftGateTests / UiA11yP1InputGateTests.
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiA11yP2FocusContrastGateTests
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
        public void CloseAllOverlayPanels_UsesSingleDeferredRestore_WithDashboardFallback()
        {
            string body = Slice(
                ReadSrc("src", "Main.PanelLifecycle.cs"),
                "private void CloseAllOverlayPanels",
                "private void CloseSettingsPanel");

            // Single topmost-opener restore attempt with deferred visibility
            // check; per-panel synchronous restore removed (stacked panels).
            Assert.Contains("GetRecordedOpener(", body);
            Assert.Contains("RestoreFocusDeferred(", body);
            Assert.Contains("FocusFirstDeferred(", body);
            Assert.Contains("IsInsideAny(", body);
            Assert.DoesNotContain("RestoreFocusFromRoot(panel)", body);
        }

        [Fact]
        public void FocusPolicy_ExposesDeferredRestoreContract()
        {
            string src = ReadSrc("src", "UI", "AshfallFocusPolicy.cs");
            Assert.Contains("public static Control? GetRecordedOpener(Control root)", src);
            Assert.Contains("public static void RestoreFocusDeferred(Control? opener)", src);
            Assert.Contains("public static void FocusFirstDeferred(Control root)", src);
            Assert.Contains("public static bool IsInsideAny(Control? node, IReadOnlyList<Control> roots)", src);
            // Deferred restore must check visibility after the deferred tick.
            Assert.Contains("IsVisibleInTree()", src);
        }

        [Theory]
        [InlineData("GeigerCalibrationPanel")]
        [InlineData("SafeCrackModal")]
        [InlineData("BrineExtractionPanel")]
        [InlineData("TriangulationPanel")]
        public void AlarmStatusAccents_UseCanonicalTokens(string fileName)
        {
            // Hand-rolled Color primaries (Red/Green/Yellow/White) replaced by
            // Critical/Success/Warning/Pale tokens (a11y audit §2b/§2c).
            string src = ReadSrc("src", "UI", $"{fileName}.cs");
            Assert.DoesNotContain("Colors.Red", src);
            Assert.DoesNotContain("Colors.Green", src);
            Assert.DoesNotContain("Colors.Yellow", src);
            Assert.DoesNotContain("Colors.White", src);
            Assert.Contains("using DesignTheme = Ashfall.Core.UI.Theme;", src);
        }
    }
}
