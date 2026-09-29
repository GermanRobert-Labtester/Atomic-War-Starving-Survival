// SPDX-License-Identifier: MIT
// A11Y-TARGET-SIZE-SWEEP2-2026-09-29 — static drift gate for the central
// control-defaults normalization seam (plan
// .ai/plans/ui-a11y-target-size-sweep2-2026-09-29.md). The seam replaces a
// ~230-site per-edit sweep: if it is unwired or narrowed, direct buttons
// regress to font-coupled (~27px) heights and fixed-width labels overflow.
// Pattern: MainTriadDriftGateTests / UiA11yFinalColorGateTests.
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiA11yTargetSizeSweep2GateTests
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

        private static string SrcBetween(string src, string startMarker, string endMarker)
        {
            int start = src.IndexOf(startMarker, StringComparison.Ordinal);
            Assert.True(start >= 0, $"marker missing: {startMarker}");
            int end = src.IndexOf(endMarker, start, StringComparison.Ordinal);
            Assert.True(end > start, $"end marker missing after {startMarker}: {endMarker}");
            return src.Substring(start, end - start);
        }

        [Fact]
        public void EnforceControlDefaults_FloorsButtonsAndClipsFixedWidthLabels()
        {
            string body = SrcBetween(
                ReadSrc("src", "UI", "AshfallUiTheme.cs"),
                "public static void EnforceControlDefaults",
                "private static Godot.Theme Build");

            // Button branch: raise any sub-28px interactive target to the floor.
            Assert.Contains("root is Button button", body);
            Assert.Contains("button.CustomMinimumSize.Y < DesignTheme.MinInteractiveHeight", body);
            Assert.Contains("button.CustomMinimumSize.X, DesignTheme.MinInteractiveHeight", body);

            // Label branch: fixed-width, non-autowrap labels get ellipsis clipping.
            Assert.Contains("root is Label label", body);
            Assert.Contains("!label.ClipText", body);
            Assert.Contains("label.AutowrapMode == TextServer.AutowrapMode.Off", body);
            Assert.Contains("label.CustomMinimumSize.X > 0f", body);
            Assert.Contains("label.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis", body);

            // Recurses so lazy subtrees are covered too.
            Assert.Contains("EnforceControlDefaults(child)", body);
        }

        [Fact]
        public void ShowPanelLifecycle_NormalizesPanelBeforeOpen()
        {
            string body = SrcBetween(
                ReadSrc("src", "Main.PanelLifecycle.cs"),
                "private void ShowPanelLifecycle",
                "private void CloseAllOverlayPanels");

            // Before animation/focus, every panel open normalizes its subtree.
            int enforce = body.IndexOf("AshfallUiTheme.EnforceControlDefaults(panel)", StringComparison.Ordinal);
            int animate = body.IndexOf("AnimateOpen(panel)", StringComparison.Ordinal);
            Assert.True(enforce >= 0, "ShowPanelLifecycle must call EnforceControlDefaults");
            Assert.True(animate >= 0, "ShowPanelLifecycle must still animate");
            Assert.True(enforce < animate, "normalize before AnimateOpen so layout uses final min sizes");
        }

        [Fact]
        public void BootPath_RunsDeferredWholeTreeSweep()
        {
            string src = ReadSrc("src", "Main.UiPanels.cs");

            Assert.Contains("CallDeferred(nameof(RunUiA11yDefaultsSweep))", src);
            Assert.Contains("AshfallUiTheme.EnforceControlDefaults(this)", src);
        }
    }
}
