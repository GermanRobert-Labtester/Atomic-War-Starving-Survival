using System;
using System.IO;
using System.Text.RegularExpressions;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Binding-purity + token-parity + field-probe contract tests for the
    /// Unity UI Toolkit track (TradeScreenView / EconomyHudView).
    ///
    /// These tests run in the pure .NET test host (no UnityEngine) and verify:
    ///  1. View source files reference zero Ashfall.Core namespaces (purity).
    ///  2. USS :root variables match Ashfall.Core.UI.Theme constants (parity).
    ///  3. UXML element names match the view's public name constants (contract).
    ///  4. The shadow Theme.cs has been removed (single-source invariant).
    /// </summary>
    public class UiToolkitBindingPurityTests
    {
        private static string ProjectRoot()
        {
            // AppContext.BaseDirectory → bin/Debug/net9.0; walk up to repo root.
            string dir = AppContext.BaseDirectory;
            for (int i = 0; i < 6; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Path.GetDirectoryName(dir);
                if (dir == null) break;
            }
            return Directory.GetCurrentDirectory();
        }

        // ── 1. Binding Purity ──────────────────────────────────────────

        [Theory]
        [InlineData("Assets/_Game/UI/TradeScreenView.cs")]
        [InlineData("Assets/_Game/UI/EconomyHudView.cs")]
        public void ViewSource_DoesNotImport_CoreNamespaces(string relativePath)
        {
            string path = Path.Combine(ProjectRoot(), relativePath);
            Assert.True(File.Exists(path), $"View source not found: {path}");

            string src = File.ReadAllText(path);
            Assert.DoesNotContain("using Ashfall.Core", src);
            Assert.DoesNotContain("using Ashfall.Core.Economy", src);
            Assert.DoesNotContain("using Ashfall.Core.UI", src);
            Assert.DoesNotContain("using Ashfall.Core.Radio", src);
        }

        [Theory]
        [InlineData("Assets/_Game/UI/TradeScreenView.cs")]
        [InlineData("Assets/_Game/UI/EconomyHudView.cs")]
        public void ViewSource_DoesNotReference_MarketSystemOrDynamicEconomy(string relativePath)
        {
            string path = Path.Combine(ProjectRoot(), relativePath);
            string src = File.ReadAllText(path);
            Assert.DoesNotContain("MarketSystem", src);
            Assert.DoesNotContain("DynamicEconomySystem", src);
            Assert.DoesNotContain("FactionStanceEngine", src);
        }

        [Theory]
        [InlineData("Assets/_Game/UI/TradeScreenView.cs")]
        [InlineData("Assets/_Game/UI/EconomyHudView.cs")]
        public void ViewSource_OnlyRaises_IntentEvents(string relativePath)
        {
            string path = Path.Combine(ProjectRoot(), relativePath);
            string src = File.ReadAllText(path);

            // Views should only raise Action events (intent), never pass simulation state out.
            var eventMatches = Regex.Matches(src, @"public\s+event\s+(\w+)");
            foreach (Match m in eventMatches)
            {
                Assert.Equal("Action", m.Groups[1].Value);
            }
        }

        // ── 2. Shadow Theme Elimination ────────────────────────────────

        [Fact]
        public void ShadowTheme_FileDoesNotExist()
        {
            string shadowPath = Path.Combine(ProjectRoot(), "Assets/_Game/UI/Theme.cs");
            Assert.False(File.Exists(shadowPath),
                "Shadow Theme.cs still exists at Assets/_Game/UI/Theme.cs — " +
                "canonical source is Ashfall.Core.UI.Theme");
        }

        [Fact]
        public void CanonicalTheme_IsOnlyThemeClass()
        {
            // Verify no other Theme class exists in the Unity host layer.
            string[] csFiles = Directory.GetFiles(
                Path.Combine(ProjectRoot(), "Assets"), "*.cs", SearchOption.AllDirectories);

            int themeClassCount = 0;
            foreach (var file in csFiles)
            {
                string src = File.ReadAllText(file);
                if (Regex.IsMatch(src, @"public\s+static\s+class\s+Theme\b"))
                    themeClassCount++;
            }
            Assert.Equal(1, themeClassCount);
        }

        // ── 3. USS ↔ Theme.cs Token Parity ────────────────────────────

        [Fact]
        public void UssRoot_HexColors_MatchThemeConstants()
        {
            string ussPath = Path.Combine(ProjectRoot(), "Assets/_Game/UI/DiegeticHud.uss");
            Assert.True(File.Exists(ussPath), $"USS not found: {ussPath}");

            string uss = File.ReadAllText(ussPath);

            VerifyUssHex(uss, "--ink", Theme.InkHex);
            VerifyUssHex(uss, "--warm", Theme.WarmHex);
            VerifyUssHex(uss, "--hot", Theme.HotHex);
            VerifyUssHex(uss, "--pale", Theme.PaleHex);
            VerifyUssHex(uss, "--muted", Theme.MutedHex);
            VerifyUssHex(uss, "--dim", Theme.DimHex);
            VerifyUssHex(uss, "--exclusive", Theme.ExclusiveHex);
            VerifyUssHex(uss, "--critical", Theme.CriticalHex);
        }

        [Fact]
        public void UssRoot_RgbaColors_MatchThemeTuples_WithinEpsilon()
        {
            string ussPath = Path.Combine(ProjectRoot(), "Assets/_Game/UI/DiegeticHud.uss");
            string uss = File.ReadAllText(ussPath);

            VerifyUssRgba(uss, "--ink-panel", Theme.InkPanel);
            VerifyUssRgba(uss, "--line", Theme.Line);
            VerifyUssRgba(uss, "--line-soft", Theme.LineSoft);
        }

        [Fact]
        public void UssRoot_SpacingTokens_MatchThemeConstants()
        {
            string ussPath = Path.Combine(ProjectRoot(), "Assets/_Game/UI/DiegeticHud.uss");
            string uss = File.ReadAllText(ussPath);

            VerifyUssPx(uss, "--hud-edge", Theme.HudEdge);
            VerifyUssPx(uss, "--hud-panel-padding", Theme.HudPanelPadding);
        }

        [Fact]
        public void TradeScreenUss_UsesOnlyVarTokens_NoHardcodedColors()
        {
            string ussPath = Path.Combine(ProjectRoot(), "Assets/_Game/UI/TradeScreen.uss");
            Assert.True(File.Exists(ussPath));
            string uss = File.ReadAllText(ussPath!);

            // No bare hex colors in trade screen USS — all must use var(--token).
            var bareHex = Regex.Matches(uss, @"(?<!\w)#[0-9a-fA-F]{6}\b");
            Assert.Empty(bareHex);
        }

        [Fact]
        public void EconomyHudUss_UsesOnlyVarTokens_NoHardcodedColors()
        {
            string ussPath = Path.Combine(ProjectRoot(), "Assets/_Game/UI/EconomyHud.uss");
            Assert.True(File.Exists(ussPath));
            string uss = File.ReadAllText(ussPath!);

            var bareHex = Regex.Matches(uss, @"(?<!\w)#[0-9a-fA-F]{6}\b");
            Assert.Empty(bareHex);
        }

        // ── 4. UXML ↔ View Name Constants (Field Probe Contract) ──────

        [Fact]
        public void Uxml_ContainsAll_TradeScreenViewElementNames()
        {
            string uxmlPath = Path.Combine(ProjectRoot(), "Assets/_Game/UI/DiegeticHud.uxml");
            Assert.True(File.Exists(uxmlPath));
            string uxml = File.ReadAllText(uxmlPath);

            // All 14 Godot-panel probe fields must have matching UXML elements.
            string[] requiredNames = {
                "trade-faction-name",     // 1. Faction name
                "trade-leader-name",      // 2. Leader name
                "trade-stance-badge",     // 3. Stance badge
                "trade-trust",            // 4. Trust meter
                "trade-aggression",       // 5. Aggression meter
                "trade-repels",           // 6. Repel counter
                "trade-player-lines",     // 7. Player offers
                "trade-faction-lines",    // 8. Faction asks
                "trade-player-total-value", // 9. Player total
                "trade-faction-total-value", // 10. Faction total
                "trade-fair-indicator",   // 11. Fairness
                "trade-parley-btn",       // 12. Parley button
                "trade-parley-msg",       // 13. Parley message
                "trade-radio-ticker",     // 14. Radio ticker
            };

            foreach (var name in requiredNames)
            {
                Assert.Contains($"name=\"{name}\"", uxml);
            }
        }

        [Fact]
        public void Uxml_ContainsAll_EconomyHudViewElementNames()
        {
            string uxmlPath = Path.Combine(ProjectRoot(), "Assets/_Game/UI/DiegeticHud.uxml");
            Assert.True(File.Exists(uxmlPath));
            string uxml = File.ReadAllText(uxmlPath);

            string[] requiredNames = {
                "economy-strip",
                "economy-strip-day",
                "economy-strip-supply",
                "economy-strip-price",
                "economy-panel",
                "economy-panel-summary",
                "economy-goods-list",
                "economy-empty",
            };

            foreach (var name in requiredNames)
            {
                Assert.Contains($"name=\"{name}\"", uxml);
            }
        }

        // ── 5. Trade/ Economy Sizing Tokens Match Theme ───────────────

        [Fact]
        public void TradeScreenUss_PanelSize_MatchesThemeConstants()
        {
            string ussPath = Path.Combine(ProjectRoot(), "Assets/_Game/UI/TradeScreen.uss");
            string uss = File.ReadAllText(ussPath);

            Assert.Contains($"min-width: {Theme.TradePanelMinWidth}px", uss);
            Assert.Contains($"max-width: {Theme.TradePanelMaxWidth}px", uss);
            Assert.Contains($"max-height: {Theme.TradePanelMaxHeight}px", uss);
        }

        [Fact]
        public void EconomyHudUss_PanelSize_MatchesThemeConstants()
        {
            string ussPath = Path.Combine(ProjectRoot(), "Assets/_Game/UI/EconomyHud.uss");
            string uss = File.ReadAllText(ussPath);

            Assert.Contains($"width: {Theme.EconomyStripWidth}px", uss);
            Assert.Contains($"min-width: {Theme.EconomyPanelMinWidth}px", uss);
            Assert.Contains($"max-width: {Theme.EconomyPanelMaxWidth}px", uss);
            Assert.Contains($"max-height: {Theme.EconomyPanelMaxHeight}px", uss);
        }

        // ── Helpers ────────────────────────────────────────────────────

        private static void VerifyUssHex(string uss, string varName, string expectedHex)
        {
            var match = Regex.Match(uss, $@"{Regex.Escape(varName)}:\s*(#[0-9a-fA-F]{{6}})\b");
            Assert.True(match.Success, $"USS variable {varName} not found as hex");
            Assert.Equal(expectedHex.ToUpperInvariant(), match.Groups[1].Value.ToUpperInvariant());
        }

        private static void VerifyUssRgba(string uss, string varName,
            (float r, float g, float b, float a) expected)
        {
            var match = Regex.Match(uss,
                $@"{Regex.Escape(varName)}:\s*rgba\(\s*([\d.]+)\s*,\s*([\d.]+)\s*,\s*([\d.]+)\s*,\s*([\d.]+)\s*\)");
            Assert.True(match.Success, $"USS variable {varName} not found as rgba()");

            float r = float.Parse(match.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
            float g = float.Parse(match.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);
            float b = float.Parse(match.Groups[3].Value, System.Globalization.CultureInfo.InvariantCulture);
            float a = float.Parse(match.Groups[4].Value, System.Globalization.CultureInfo.InvariantCulture);

            float epsilon = 2f / 255f; // ~0.008 — USS rounds to integer RGB
            Assert.True(Math.Abs(r / 255f - expected.r) < epsilon || Math.Abs(r - expected.r) < epsilon,
                $"R mismatch on {varName}: uss={r} theme={expected.r}");
            Assert.True(Math.Abs(g / 255f - expected.g) < epsilon || Math.Abs(g - expected.g) < epsilon,
                $"G mismatch on {varName}: uss={g} theme={expected.g}");
            Assert.True(Math.Abs(b / 255f - expected.b) < epsilon || Math.Abs(b - expected.b) < epsilon,
                $"B mismatch on {varName}: uss={b} theme={expected.b}");
            Assert.True(Math.Abs(a - expected.a) < 0.02f,
                $"A mismatch on {varName}: uss={a} theme={expected.a}");
        }

        private static void VerifyUssPx(string uss, string varName, int expectedPx)
        {
            var match = Regex.Match(uss, $@"{Regex.Escape(varName)}:\s*(\d+)px");
            Assert.True(match.Success, $"USS variable {varName} not found as Npx");
            Assert.Equal(expectedPx, int.Parse(match.Groups[1].Value));
        }
    }
}
