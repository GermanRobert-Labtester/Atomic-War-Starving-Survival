// SPDX-License-Identifier: MIT
using Ashfall.Core.Settings;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.Settings
{
    /// <summary>
    /// Plan 184 Path β — Core CVD mapper contracts. Theme token constants
    /// stay unchanged; mapping is presentation-only.
    /// </summary>
    public class ColorblindColorMapperTests
    {
        [Theory]
        [InlineData(null, ColorblindColorMapper.None)]
        [InlineData("", ColorblindColorMapper.None)]
        [InlineData("  ", ColorblindColorMapper.None)]
        [InlineData("NONE", ColorblindColorMapper.None)]
        [InlineData("Protanopia", ColorblindColorMapper.Protanopia)]
        [InlineData("deuteranopia", ColorblindColorMapper.Deuteranopia)]
        [InlineData("TRITANOPIA", ColorblindColorMapper.Tritanopia)]
        [InlineData("monochrome", ColorblindColorMapper.None)]
        public void NormalizeMode_ReturnsAllowedCanonicalOrNone(string? input, string expected)
        {
            Assert.Equal(expected, ColorblindColorMapper.NormalizeMode(input));
        }

        [Fact]
        public void Map_None_ReturnsIdentityIncludingAlpha()
        {
            var src = Theme.Critical;
            var mapped = ColorblindColorMapper.Map(src, ColorblindColorMapper.None);
            Assert.Equal(src.r, mapped.r);
            Assert.Equal(src.g, mapped.g);
            Assert.Equal(src.b, mapped.b);
            Assert.Equal(src.a, mapped.a);
        }

        [Theory]
        [InlineData(ColorblindColorMapper.Protanopia)]
        [InlineData(ColorblindColorMapper.Deuteranopia)]
        [InlineData(ColorblindColorMapper.Tritanopia)]
        public void Map_ActiveModes_ChangeCriticalAndSuccessDifferently(string mode)
        {
            var criticalMapped = ColorblindColorMapper.Map(Theme.Critical, mode);
            var successMapped = ColorblindColorMapper.Map(Theme.Success, mode);

            Assert.False(
                NearlyEqual(Theme.Critical, criticalMapped),
                $"Critical must change under {mode}");
            Assert.False(
                NearlyEqual(Theme.Success, successMapped),
                $"Success must change under {mode}");
            Assert.Equal(Theme.Critical.a, criticalMapped.a);
            Assert.Equal(Theme.Success.a, successMapped.a);

            // Modes must not collapse Critical and Success to the same RGB.
            Assert.False(
                NearlyEqual(criticalMapped, successMapped),
                $"Critical and Success must remain distinguishable under {mode}");
        }

        [Fact]
        public void Map_Modes_ProduceDistinctCriticalTransforms()
        {
            var protan = ColorblindColorMapper.Map(Theme.Critical, ColorblindColorMapper.Protanopia);
            var deutan = ColorblindColorMapper.Map(Theme.Critical, ColorblindColorMapper.Deuteranopia);
            var tritan = ColorblindColorMapper.Map(Theme.Critical, ColorblindColorMapper.Tritanopia);

            Assert.False(NearlyEqual(protan, deutan));
            Assert.False(NearlyEqual(protan, tritan));
            Assert.False(NearlyEqual(deutan, tritan));
        }

        [Fact]
        public void ThemeConstants_RemainUnchangedByMapperContract()
        {
            // Pin Theme floors so Path β cannot silently rewrite authority tokens.
            Assert.Equal(0.902f, Theme.Critical.r, 3);
            Assert.Equal(0.200f, Theme.Critical.g, 3);
            Assert.Equal(0.200f, Theme.Critical.b, 3);
            Assert.Equal(0.361f, Theme.Success.r, 3);
            Assert.Equal(0.839f, Theme.Success.g, 3);
            Assert.Equal(0.439f, Theme.Success.b, 3);
        }

        private static bool NearlyEqual(
            (float r, float g, float b, float a) a,
            (float r, float g, float b, float a) b)
        {
            const float eps = 0.0005f;
            return System.Math.Abs(a.r - b.r) < eps
                && System.Math.Abs(a.g - b.g) < eps
                && System.Math.Abs(a.b - b.b) < eps
                && System.Math.Abs(a.a - b.a) < eps;
        }
    }
}
