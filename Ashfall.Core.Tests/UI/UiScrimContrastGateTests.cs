// SPDX-License-Identifier: MIT
// UI-SCRIM-CONTRAST-2026-09-29 — closes ACCESSIBILITY_REPORT_2026-09-29 §3
// (stack-dependent translucent contrast; plan .ai/plans/ui-scrim-contrast-2026-09-29.md).
//
// Method: backdrop art cannot exceed sRGB (1,1,1), so compositing each
// audited scrim over a pure-white backdrop and computing the WCAG contrast
// of the surface's text token is a worst-case bound — every real backdrop
// is ≥ this contrast. Per-surface alphas are also asserted as source
// tripwires: changing one re-opens the audit and must re-run this math.
// Math uses plain sRGB tuples (Godot 2D blends in sRGB space; Core tokens
// are engine-free tuples).
using System;
using System.IO;
using System.Linq;
using Xunit;
using Theme = Ashfall.Core.UI.Theme;

namespace Ashfall.Core.Tests
{
    public sealed class UiScrimContrastGateTests
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

        private static void AssertSourceContains(params string[] partsAndNeedle)
        {
            string path = Path.Combine(new[] { RepoRoot() }
                .Concat(partsAndNeedle.Take(partsAndNeedle.Length - 1)).ToArray());
            Assert.True(File.Exists(path), $"missing source file: {path}");
            Assert.Contains(partsAndNeedle[^1], File.ReadAllText(path));
        }

        private static (float r, float g, float b) Token((float r, float g, float b, float a) t)
            => (t.r, t.g, t.b);

        private static float Luminance((float r, float g, float b) c)
        {
            static float Chan(float v)
                => v <= 0.03928f ? v / 12.92f : MathF.Pow((v + 0.055f) / 1.055f, 2.4f);
            return 0.2126f * Chan(c.r) + 0.7152f * Chan(c.g) + 0.0722f * Chan(c.b);
        }

        private static (float r, float g, float b) CompositeOverWhite((float r, float g, float b) scrim, float alpha)
            => Composite(scrim, alpha, (1f, 1f, 1f));

        private static (float r, float g, float b) Composite(
            (float r, float g, float b) scrim, float alpha, (float r, float g, float b) backdrop)
            => (scrim.r * alpha + backdrop.r * (1 - alpha),
                scrim.g * alpha + backdrop.g * (1 - alpha),
                scrim.b * alpha + backdrop.b * (1 - alpha));

        private static float Contrast((float r, float g, float b) fg, (float r, float g, float b) bg)
        {
            float l1 = Luminance(fg), l2 = Luminance(bg);
            float hi = MathF.Max(l1, l2), lo = MathF.Min(l1, l2);
            return (hi + 0.05f) / (lo + 0.05f);
        }

        private const float AaNormalText = 4.5f;

        [Theory]
        [InlineData(0.90f)]  // §3-closed dims: MapDetail scene scrim, Expedition
        [InlineData(0.92f)]  // art dim, GameOver carousel overlay, GameHud meters,
                             // Theme.InkPanelStrong panel scrim — Muted ≥ 4.96
        public void InkScrims_PassAaOverWorstCaseWhiteBackdrop(float alpha)
        {
            var worst = CompositeOverWhite(Token(Theme.Ink), alpha);
            Assert.True(Contrast(Token(Theme.Pale), worst) >= AaNormalText,
                $"Pale over Ink @ {alpha}: {Contrast(Token(Theme.Pale), worst):F2}");
            Assert.True(Contrast(Token(Theme.Muted), worst) >= AaNormalText,
                $"Muted over Ink @ {alpha}: {Contrast(Token(Theme.Muted), worst):F2}");
        }

        [Fact]
        public void MainMenuContent_SitsOnBackingPanel_NotRawCarousel()
        {
            // Audit §3 correction 2026-09-29: the report claimed menu
            // status/version labels render over the 0.55 carousel overlay,
            // but the entire menu column lives inside MakePanel(520, 0) —
            // a false positive. Worst-case over the 0.55 overlay would have
            // been 2.02:1 even for Warm, so this structure is load-bearing:
            // if the backing panel is removed, the labels land on the raw
            // carousel and §3 re-opens.
            AssertSourceContains("src", "UI", "MainMenuPanel.cs",
                "UiAssetManifest.MainMenuBackgrounds, 0.55f)");
            AssertSourceContains("src", "UI", "MainMenuPanel.cs",
                "AshfallUiHelpers.MakePanel(520, 0)");
        }

        [Fact]
        public void ExpeditionEncounterBanner_Passes()
        {
            // Banner scrim: Entropy-derived tint at 0.94 alpha (pkg 9), Warm
            // text over worst-case white.
            AssertSourceContains("src", "UI", "ExpeditionPanel.cs",
                "0.12f, Ashfall.Core.UI.Theme.Entropy.g * 0.12f, Ashfall.Core.UI.Theme.Entropy.b * 0.12f, 0.94f)");
            var worst = CompositeOverWhite(
                (Theme.Entropy.r * 0.12f, Theme.Entropy.g * 0.12f, Theme.Entropy.b * 0.12f), 0.94f);
            Assert.True(Contrast(Token(Theme.Warm), worst) >= AaNormalText);
            Assert.True(Contrast(Token(Theme.Pale), worst) >= AaNormalText);
        }

        [Fact]
        public void ClosedLowAlphaSurfaces_StayAtClosedAlphas()
        {
            // §3 closures 2026-09-29 — each was raised to 0.90 because its
            // old alpha let worst-case bright art under Muted/Dim text:
            //   MapDetail 0.74 → Muted 2.74:1; Expedition 0.82 → Dim 3.78:1;
            //   GameOver 0.80 → Dim 3.44:1. At Ink-dim ≥ 0.90 every token
            // holds ≥ 4.96:1 over any art. Changing an alpha re-opens §3.
            AssertSourceContains("src", "UI", "MapDetailPanel.cs", "0.03f, 0.04f, 0.05f, 0.90f");
            AssertSourceContains("src", "UI", "ExpeditionPanel.cs", "BackdropArt.ExpeditionDeparture, 0.90f");
            AssertSourceContains("src", "UI", "GameOverPanel.cs", "UiAssetManifest.GameOverBackgrounds, 0.90f)");
            AssertSourceContains("src", "UI", "GameHudOverlay.cs", "Theme.Ink.b, 0.9f");
        }

        [Fact]
        public void CompoundTranslucentChrome_DarkensNotLightens()
        {
            // Metric card Ink 0.72 → status rail Ink 0.55 → sidebar row Ink
            // 0.40, stacked over a host panel scrim (0.92) over worst-case
            // white. Each ink layer only pulls the composite back toward
            // Ink, so the chain's worst case is its first layer; assert the
            // full chain and the audit's exact per-layer alphas.
            AssertSourceContains("src", "UI", "AshfallMetricCard.cs", "DesignTheme.Ink.b, 0.72f");
            AssertSourceContains("src", "UI", "AshfallStatusRail.cs", "DesignTheme.Ink.b, 0.55f");
            AssertSourceContains("src", "UI", "AshfallSidebar.cs", "DesignTheme.Ink.b, 0.40f");

            var host = CompositeOverWhite(Token(Theme.Ink), 0.92f);
            var card = Composite(Token(Theme.Ink), 0.72f, host);
            var rail = Composite(Token(Theme.Ink), 0.55f, card);
            var row = Composite(Token(Theme.Ink), 0.40f, rail);

            Assert.True(Contrast(Token(Theme.Pale), row) >= AaNormalText);
            Assert.True(Contrast(Token(Theme.Dim), row) >= AaNormalText);
            Assert.True(Luminance(row) <= Luminance(host),
                "compound ink chrome must not be lighter than its host scrim");
        }
    }
}
