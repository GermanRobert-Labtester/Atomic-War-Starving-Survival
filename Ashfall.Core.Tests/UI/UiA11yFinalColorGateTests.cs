// SPDX-License-Identifier: MIT
// A11Y-FINAL-COLOR-2026-09-29 — static drift gate for the last raw
// color-literal sweep on UI chrome and the central label text-overrun seam
// (plan .ai/plans/ui-a11y-final-color-literals-2026-09-29.md).
// Pattern: MainTriadDriftGateTests / UiA11yAccentTokenGateTests.
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UiA11yFinalColorGateTests
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

        private static string ReadRepo(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        // Each swept site must never regain its raw literal, and must route
        // through the named Core token instead.
        [Theory]
        [InlineData("src/UI/EmergencyResponseHud.cs", "new Color(0.12f, 0.02f, 0.02f, 0.95f)", "DesignTheme.Critical")]
        [InlineData("src/UI/BlackProjectsArchivePanel.cs", "new Color(0.12f, 0.04f, 0.04f, 0.85f)", "DesignTheme.Critical")]
        [InlineData("src/UI/ExpeditionPanel.cs", "new Color(0.10f, 0.07f, 0.04f, 0.94f)", "Ashfall.Core.UI.Theme.Entropy")]
        [InlineData("src/UI/UiBackgroundCarousel.cs", "new Color(0.035f, 0.043f, 0.047f, 1f)", "Ashfall.Core.UI.Theme.Ink")]
        [InlineData("src/UI/BackdropArt.cs", "new Color(0.04f, 0.05f, 0.06f", "Ashfall.Core.UI.Theme.Ink")]
        [InlineData("src/World/RoomHotspotView.cs", "0.08f, 0.1f, 0.12f", "Ashfall.Core.UI.Theme.SurfaceCard")]
        public void SweptSites_UseThemeTokens(string relativePath, string bannedLiteral, string requiredToken)
        {
            string src = ReadRepo(relativePath.Split('/'));

            Assert.DoesNotContain(bannedLiteral, src);
            Assert.Contains(requiredToken, src);
        }

        [Fact]
        public void RoomHotspotView_HoverColors_AreTokenDerived()
        {
            string src = ReadRepo("src", "World", "RoomHotspotView.cs");

            Assert.DoesNotContain("0.18f, 0.25f, 0.32f", src);
            Assert.DoesNotContain("0.95f, 0.85f, 0.4f", src);
            Assert.Contains("Ashfall.Core.UI.Theme.Lethe", src);
            Assert.Contains("Ashfall.Core.UI.Theme.Hot", src);
        }

        [Fact]
        public void LabelFactories_RouteThroughFinishLabel()
        {
            string src = ReadRepo("src", "UI", "AshfallUiHelpers.cs");

            Assert.Contains("private static Label FinishLabel", src);
            Assert.Contains("ClipText = true", src);
            Assert.Contains("TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis", src);

            // At least the 18 label factories exit through the seam.
            int exits = CountOccurrences(src, "return FinishLabel(lbl);");
            Assert.True(exits >= 18, $"expected >= 18 FinishLabel exits, found {exits}");
        }

        // Regression guard: FinishLabel must terminate, not recurse (the
        // factory-wide return rewrite once swallowed its own body).
        [Fact]
        public void FinishLabel_DoesNotRecurse()
        {
            string src = ReadRepo("src", "UI", "AshfallUiHelpers.cs");

            int start = src.IndexOf("private static Label FinishLabel", StringComparison.Ordinal);
            Assert.True(start >= 0, "FinishLabel missing");
            int end = src.IndexOf("public static", start, StringComparison.Ordinal);
            string body = src.Substring(start, end - start);

            Assert.Contains("return lbl;", body);
            Assert.DoesNotContain("FinishLabel(lbl)", body);
        }

        [Fact]
        public void MakeButton_ClipsOverrun()
        {
            string src = ReadRepo("src", "UI", "AshfallUiHelpers.cs");

            int start = src.IndexOf("public static Button MakeButton", StringComparison.Ordinal);
            Assert.True(start >= 0, "MakeButton missing");
            int end = src.IndexOf("public static", start + 10, StringComparison.Ordinal);
            string body = src.Substring(start, end - start);

            Assert.Contains("ClipText = true", body);
            Assert.Contains("TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis", body);
        }

        // Part A's "near-exact token match" claims hold only while these token
        // values stay put; pin them so a palette change re-opens the audit.
        [Theory]
        [InlineData("Ink = (0.035f, 0.043f, 0.047f, 1f)")]
        [InlineData("SurfaceCard = (0.078f, 0.098f, 0.118f, 1f)")]
        [InlineData("Lethe = (0.431f, 0.639f, 0.659f, 1f)")]
        [InlineData("Hot = (0.957f, 0.784f, 0.459f, 1f)")]
        [InlineData("Muted = (0.576f, 0.561f, 0.518f, 1f)")]
        [InlineData("Critical = (1.000f, 0.322f, 0.322f, 1f)")]
        [InlineData("Entropy = (0.788f, 0.482f, 0.227f, 1f)")]
        public void ThemeTokens_Pinned(string tokenLine)
            => Assert.Contains(tokenLine, ReadRepo("Assets", "Ashfall.Core", "UI", "Theme.cs"));

        private static int CountOccurrences(string haystack, string needle)
        {
            int count = 0, idx = 0;
            while ((idx = haystack.IndexOf(needle, idx, StringComparison.Ordinal)) >= 0)
            {
                count++;
                idx += needle.Length;
            }
            return count;
        }
    }
}
