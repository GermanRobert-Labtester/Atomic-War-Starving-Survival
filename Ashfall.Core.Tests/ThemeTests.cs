using System;
using System.IO;
using System.Text.Json;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ThemeTests
    {
        [Fact]
        public void HexColors_AreSevenChars_WithHashPrefix()
        {
            Assert.Equal(7, Theme.InkHex.Length);
            Assert.Equal(7, Theme.WarmHex.Length);
            Assert.Equal(7, Theme.HotHex.Length);
            Assert.Equal(7, Theme.PaleHex.Length);
            Assert.Equal(7, Theme.MutedHex.Length);
            Assert.Equal(7, Theme.DimHex.Length);
            Assert.Equal(7, Theme.ExclusiveHex.Length);
            Assert.Equal(7, Theme.CriticalHex.Length);
            Assert.Equal(7, Theme.EntropyHex.Length);
            Assert.Equal(7, Theme.LetheHex.Length);
            Assert.Equal(7, Theme.OzoneHex.Length);
            Assert.Equal(7, Theme.LetheAmberHex.Length);
            Assert.Equal(7, Theme.LetheRedHex.Length);
            Assert.StartsWith("#", Theme.WarmHex);
        }

        [Fact]
        public void RgbaComponents_InZeroToOneRange()
        {
            CheckRgba(Theme.Ink);
            CheckRgba(Theme.InkPanel);
            CheckRgba(Theme.Line);
            CheckRgba(Theme.LineSoft);
            CheckRgba(Theme.Warm);
            CheckRgba(Theme.Hot);
            CheckRgba(Theme.Pale);
            CheckRgba(Theme.Muted);
            CheckRgba(Theme.Dim);
            CheckRgba(Theme.Exclusive);
            CheckRgba(Theme.Critical);
            CheckRgba(Theme.Entropy);
            CheckRgba(Theme.Lethe);
            CheckRgba(Theme.Ozone);
            CheckRgba(Theme.Ghost);
            CheckRgba(Theme.EntropyGlow);
            CheckRgba(Theme.LetheAmber);
            CheckRgba(Theme.LetheRed);
        }

        [Fact]
        public void HexColors_MatchRgbaTuples_WithinEpsilon()
        {
            VerifyHexMatchesRgba(Theme.InkHex, Theme.Ink);
            VerifyHexMatchesRgba(Theme.WarmHex, Theme.Warm);
            VerifyHexMatchesRgba(Theme.HotHex, Theme.Hot);
            VerifyHexMatchesRgba(Theme.PaleHex, Theme.Pale);
            VerifyHexMatchesRgba(Theme.MutedHex, Theme.Muted);
            VerifyHexMatchesRgba(Theme.DimHex, Theme.Dim);
            VerifyHexMatchesRgba(Theme.ExclusiveHex, Theme.Exclusive);
            VerifyHexMatchesRgba(Theme.CriticalHex, Theme.Critical);
            VerifyHexMatchesRgba(Theme.EntropyHex, Theme.Entropy);
            VerifyHexMatchesRgba(Theme.LetheHex, Theme.Lethe);
            VerifyHexMatchesRgba(Theme.OzoneHex, Theme.Ozone);
            VerifyHexMatchesRgba(Theme.LetheAmberHex, Theme.LetheAmber);
            VerifyHexMatchesRgba(Theme.LetheRedHex, Theme.LetheRed);
        }

        [Fact]
        public void ContrastRatio_PaleOnInk_SatisfiesWcagAaa()
        {
            float l1 = CalculateRelativeLuminance(Theme.Pale);
            float l2 = CalculateRelativeLuminance(Theme.Ink);
            float ratio = (Math.Max(l1, l2) + 0.05f) / (Math.Min(l1, l2) + 0.05f);

            // WCAG AAA requires >= 7.0 for normal body text
            Assert.True(ratio >= 7.0f, $"Contrast ratio {ratio:0.00} is below 7.0 (WCAG AAA)");
        }

        [Fact]
        public void SpacingTokens_ArePositive()
        {
            Assert.True(Theme.HudEdge > 0);
            Assert.True(Theme.HudPanelPadding > 0);
            Assert.True(Theme.SpacingXs > 0);
            Assert.True(Theme.SpacingSm > 0);
            Assert.True(Theme.SpacingMd > 0);
            Assert.True(Theme.SpacingLg > 0);
            Assert.True(Theme.SpacingXl > 0);
        }

        [Fact]
        public void SpacingTokens_AreOrdered()
        {
            Assert.True(Theme.SpacingXs < Theme.SpacingSm);
            Assert.True(Theme.SpacingSm < Theme.SpacingMd);
            Assert.True(Theme.SpacingMd < Theme.SpacingLg);
            Assert.True(Theme.SpacingLg < Theme.SpacingXl);
        }

        [Fact]
        public void FontSizes_ArePositive()
        {
            Assert.True(Theme.FontSizeH1 > 0);
            Assert.True(Theme.FontSizeBody > 0);
            Assert.True(Theme.FontSizeSmall > 0);
            Assert.True(Theme.FontSizeMono > 0);
            Assert.True(Theme.FontSizeLabel > 0);
        }

        [Fact]
        public void FontSizes_AreOrdered()
        {
            Assert.True(Theme.FontSizeLabel < Theme.FontSizeSmall);
            Assert.True(Theme.FontSizeSmall < Theme.FontSizeBody);
            Assert.True(Theme.FontSizeBody < Theme.FontSizeH3);
            Assert.True(Theme.FontSizeH3 < Theme.FontSizeH2);
            Assert.True(Theme.FontSizeH2 < Theme.FontSizeH1);
        }

        [Fact]
        public void PanelSizing_ConstraintsAreConsistent()
        {
            Assert.True(Theme.PanelMaxWidth > 0);
            Assert.True(Theme.PanelMinWidthNarrow > 0);
            Assert.True(Theme.PanelMinWidthStandard > Theme.PanelMinWidthNarrow);
            Assert.True(Theme.PanelMinWidthWide > Theme.PanelMinWidthStandard);
            Assert.True(Theme.PanelMaxWidthWide > Theme.PanelMinWidthWide);
        }

        [Fact]
        public void TradePanel_ConstraintsAreConsistent()
        {
            Assert.True(Theme.TradePanelMinWidth > 0);
            Assert.True(Theme.TradePanelMaxWidth > Theme.TradePanelMinWidth);
            Assert.True(Theme.TradePanelMaxHeight > 0);
            Assert.True(Theme.TradeColumnMinWidth > 0);
        }

        [Fact]
        public void EconomyHud_ConstraintsAreConsistent()
        {
            Assert.True(Theme.EconomyStripWidth > 0);
            Assert.True(Theme.EconomyStripHeight > 0);
            Assert.True(Theme.EconomyPanelMinWidth > 0);
            Assert.True(Theme.EconomyPanelMaxWidth > Theme.EconomyPanelMinWidth);
            Assert.True(Theme.EconomyPanelMaxHeight > 0);
        }

        [Fact]
        public void CornerRadii_AreOrdered()
        {
            Assert.True(Theme.RadiusSm <= Theme.RadiusMd);
            Assert.True(Theme.RadiusMd <= Theme.RadiusLg);
        }

        [Fact]
        public void InkPanel_AlphaIsLessThanOne()
        {
            Assert.True(Theme.InkPanel.a < 1f);
            Assert.True(Theme.InkPanel.a > 0f);
        }

        [Fact]
        public void AssetManifest_AllFilesAndMetasExistOnDisk()
        {
            string manifestPath = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/UI/manifest.json");
            if (!File.Exists(manifestPath))
            {
                // Try fallback relative path
                manifestPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/UI/manifest.json");
            }

            Assert.True(File.Exists(manifestPath), $"Manifest not found at {manifestPath}");

            string json = File.ReadAllText(manifestPath);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Assert.StartsWith("1.", root.GetProperty("version").GetString());
            var assets = root.GetProperty("assets");
            Assert.True(assets.GetArrayLength() >= 26, "Expected at least 26 assets in manifest");

            string projectRoot = Path.GetDirectoryName(Path.GetDirectoryName(manifestPath))!;
            foreach (var asset in assets.EnumerateArray())
            {
                string relPath = asset.GetProperty("path").GetString()!;
                string fullPath = Path.Combine(projectRoot, "..", relPath);
                fullPath = Path.GetFullPath(fullPath);

                Assert.True(File.Exists(fullPath), $"Asset file does not exist: {fullPath}");
                var fileInfo = new FileInfo(fullPath);
                Assert.True(fileInfo.Length > 0, $"Asset file is empty: {fullPath}");

                string metaPath = fullPath + ".meta";
                Assert.True(File.Exists(metaPath), $"Asset meta file does not exist: {metaPath}");
            }
        }

        private static void CheckRgba((float r, float g, float b, float a) c)
        {
            Assert.InRange(c.r, 0f, 1f);
            Assert.InRange(c.g, 0f, 1f);
            Assert.InRange(c.b, 0f, 1f);
            Assert.InRange(c.a, 0f, 1f);
        }

        private static void VerifyHexMatchesRgba(string hex, (float r, float g, float b, float a) rgba)
        {
            int rInt = Convert.ToInt32(hex.Substring(1, 2), 16);
            int gInt = Convert.ToInt32(hex.Substring(3, 2), 16);
            int bInt = Convert.ToInt32(hex.Substring(5, 2), 16);

            float r = rInt / 255f;
            float g = gInt / 255f;
            float b = bInt / 255f;

            Assert.True(Math.Abs(r - rgba.r) < 0.015f, $"R mismatch on {hex}: hex={r:0.00} tuple={rgba.r:0.00}");
            Assert.True(Math.Abs(g - rgba.g) < 0.015f, $"G mismatch on {hex}: hex={g:0.00} tuple={rgba.g:0.00}");
            Assert.True(Math.Abs(b - rgba.b) < 0.015f, $"B mismatch on {hex}: hex={b:0.00} tuple={rgba.b:0.00}");
        }

        private static float CalculateRelativeLuminance((float r, float g, float b, float a) c)
        {
            float R = c.r <= 0.03928f ? c.r / 12.92f : (float)Math.Pow((c.r + 0.055f) / 1.055f, 2.4);
            float G = c.g <= 0.03928f ? c.g / 12.92f : (float)Math.Pow((c.g + 0.055f) / 1.055f, 2.4);
            float B = c.b <= 0.03928f ? c.b / 12.92f : (float)Math.Pow((c.b + 0.055f) / 1.055f, 2.4);
            return 0.2126f * R + 0.7152f * G + 0.0722f * B;
        }
    }
}
