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
