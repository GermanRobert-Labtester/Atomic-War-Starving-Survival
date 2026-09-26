// SPDX-License-Identifier: MIT
using System;
using Xunit;
using Ashfall.Core.UI;

namespace Ashfall.Core.Tests.UI
{
    public class ThemeSemanticTokensTests
    {
        [Fact]
        public void SemanticColorTokens_AreValidNormalizedRgba()
        {
            AssertColorTuple(Theme.Ink);
            AssertColorTuple(Theme.InkPanel);
            AssertColorTuple(Theme.Line);
            AssertColorTuple(Theme.LineSoft);
            AssertColorTuple(Theme.Warm);
            AssertColorTuple(Theme.Hot);
            AssertColorTuple(Theme.Pale);
            AssertColorTuple(Theme.Muted);
            AssertColorTuple(Theme.Dim);
            AssertColorTuple(Theme.Exclusive);
            AssertColorTuple(Theme.Critical);
            AssertColorTuple(Theme.Entropy);
            AssertColorTuple(Theme.Lethe);
            AssertColorTuple(Theme.Ozone);
            AssertColorTuple(Theme.Ghost);
            AssertColorTuple(Theme.EntropyGlow);
            AssertColorTuple(Theme.LetheAmber);
            AssertColorTuple(Theme.LetheRed);

            // New semantic tokens
            AssertColorTuple(Theme.Surface);
            AssertColorTuple(Theme.SurfaceCard);
            AssertColorTuple(Theme.BackdropOverlay);
            AssertColorTuple(Theme.SelectedBg);
            AssertColorTuple(Theme.HoverBg);
            AssertColorTuple(Theme.Success);
            AssertColorTuple(Theme.Warning);
            AssertColorTuple(Theme.Radiation);
            AssertColorTuple(Theme.RadiationAcute);
            AssertColorTuple(Theme.Info);
        }

        [Fact]
        public void SemanticHexConstants_StartWithHashAndHaveValidLength()
        {
            AssertHex(Theme.InkHex);
            AssertHex(Theme.WarmHex);
            AssertHex(Theme.HotHex);
            AssertHex(Theme.PaleHex);
            AssertHex(Theme.MutedHex);
            AssertHex(Theme.DimHex);
            AssertHex(Theme.ExclusiveHex);
            AssertHex(Theme.CriticalHex);
            AssertHex(Theme.EntropyHex);
            AssertHex(Theme.LetheHex);
            AssertHex(Theme.OzoneHex);
            AssertHex(Theme.LetheAmberHex);
            AssertHex(Theme.LetheRedHex);

            // Semantic hex tokens
            AssertHex(Theme.SurfaceHex);
            AssertHex(Theme.SurfaceCardHex);
            AssertHex(Theme.SelectedBgHex);
            AssertHex(Theme.HoverBgHex);
            AssertHex(Theme.SuccessHex);
            AssertHex(Theme.WarningHex);
            AssertHex(Theme.RadiationHex);
            AssertHex(Theme.RadiationAcuteHex);
            AssertHex(Theme.InfoHex);
        }

        [Fact]
        public void BackdropOverlay_IsSubduedDarkHighOpacity()
        {
            var overlay = Theme.BackdropOverlay;
            Assert.True(overlay.a >= 0.9f, "Backdrop overlay should have at least 90% opacity for modal occlusion");
            Assert.True(overlay.r < 0.1f && overlay.g < 0.1f && overlay.b < 0.1f, "Backdrop overlay should be dark");
        }

        [Fact]
        public void SemanticStatusColors_AreDistinct()
        {
            Assert.NotEqual(Theme.Success, Theme.Critical);
            Assert.NotEqual(Theme.Warning, Theme.Critical);
            Assert.NotEqual(Theme.Radiation, Theme.RadiationAcute);
            Assert.NotEqual(Theme.Info, Theme.Warning);
        }

        /// <summary>
        /// Contrast ratchet (UI accessibility audit, 2026-09-26):
        /// <see cref="Theme.Critical"/> is consumed as a text `font_color`
        /// across dozens of panels, so it must clear the WCAG AA body-text
        /// floor of 4.5:1 on every opaque surface it is rendered over. The
        /// previous #E63333 failed on four of five surfaces (down to 3.63:1 on
        /// SelectedBg); #FF5252 is the sealed replacement. This gate prevents a
        /// silent regression of that fix and pins hex/tuple agreement for the
        /// token.
        /// </summary>
        [Fact]
        public void CriticalTextColor_MeetsWcagAaOnEveryConsumedSurface()
        {
            var surfaces = new (string Name, (float r, float g, float b, float a) Color)[]
            {
                ("Ink", Theme.Ink),
                ("Surface", Theme.Surface),
                ("SurfaceCard", Theme.SurfaceCard),
                ("HoverBg", Theme.HoverBg),
                ("SelectedBg", Theme.SelectedBg),
            };

            foreach (var surface in surfaces)
            {
                double ratio = ContrastRatio(Theme.Critical, surface.Color);
                Assert.True(ratio >= 4.5,
                    $"Theme.Critical must meet WCAG AA 4.5:1 on {surface.Name}; measured {ratio:0.00}:1.");
            }

            Assert.Equal(Theme.CriticalHex, RgbToHex(Theme.Critical));
        }

        private static double ContrastRatio(
            (float r, float g, float b, float a) fg,
            (float r, float g, float b, float a) bg)
        {
            double lf = RelativeLuminance(fg);
            double lb = RelativeLuminance(bg);
            double lighter = Math.Max(lf, lb);
            double darker = Math.Min(lf, lb);
            return (lighter + 0.05) / (darker + 0.05);
        }

        private static double RelativeLuminance((float r, float g, float b, float a) c)
            => 0.2126 * Linearize(c.r) + 0.7152 * Linearize(c.g) + 0.0722 * Linearize(c.b);

        private static double Linearize(double channel)
            => channel <= 0.04045 ? channel / 12.92 : Math.Pow((channel + 0.055) / 1.055, 2.4);

        private static string RgbToHex((float r, float g, float b, float a) c)
        {
            int r = (int)Math.Round(c.r * 255f);
            int g = (int)Math.Round(c.g * 255f);
            int b = (int)Math.Round(c.b * 255f);
            return $"#{r:X2}{g:X2}{b:X2}";
        }

        private static void AssertColorTuple((float r, float g, float b, float a) c)
        {
            Assert.InRange(c.r, 0f, 1f);
            Assert.InRange(c.g, 0f, 1f);
            Assert.InRange(c.b, 0f, 1f);
            Assert.InRange(c.a, 0f, 1f);
        }

        private static void AssertHex(string hex)
        {
            Assert.NotNull(hex);
            Assert.StartsWith("#", hex);
            Assert.True(hex.Length == 7 || hex.Length == 9, $"Hex string '{hex}' should be 7 (#RRGGBB) or 9 (#RRGGBBAA) characters");
        }
    }
}
