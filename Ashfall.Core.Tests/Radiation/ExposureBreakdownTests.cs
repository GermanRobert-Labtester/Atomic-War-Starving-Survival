// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests.Plan20ARadiation
{
    /// <summary>
    /// C2 / Plan 20A (§16) — exposure breakdown read model. The breakdown must
    /// carry exactly the inputs the tick used and compute the effective rate
    /// through the canonical resolver, so UI display cannot drift from
    /// simulation (plan §3.8/§16.1).
    /// </summary>
    public sealed class ExposureBreakdownTests
    {
        private static ExposureEnvironment Env(
            SurvivorExposureLocation kind,
            float zone,
            float weather = 0f,
            float fallout = 0f,
            float anomaly = 0f,
            float shielding = 0f,
            string locationId = "")
        {
            return new ExposureEnvironment
            {
                LocationKind = kind,
                LocationId = locationId,
                BaseRadRate = zone - weather - fallout - anomaly,
                WeatherRadModifier = weather,
                FalloutContamination = fallout,
                AnomalyRadRate = anomaly,
                ShelterShielding = shielding,
                EffectiveZoneRadLevel = zone,
                ExposureReason = "test"
            };
        }

        [Fact]
        public void EffectiveRate_MatchesCanonicalResolver_ShieldingFallback()
        {
            var env = Env(SurvivorExposureLocation.WastelandOutdoors, zone: 190f, weather: 150f);
            var b = ExposureBreakdown.Build(env, gearProtection: 5f, accumulatedDose: 12f, lifetimeDose: 300f);
            Assert.Equal(RadiationSystem.ComputeExposurePerHour(190f, 5f, 0f), b.EffectiveExposurePerHour);
            Assert.Equal(185f, b.EffectiveExposurePerHour, 2);
        }

        [Fact]
        public void EffectiveRate_MatchesCanonicalResolver_InteriorQueryPath()
        {
            var env = Env(SurvivorExposureLocation.ShelterInterior, zone: 40f, shielding: 32f);
            var b = ExposureBreakdown.Build(env, gearProtection: 3f, accumulatedDose: 0f, lifetimeDose: 0f,
                interiorRads: 8f);
            Assert.Equal(5f, b.EffectiveExposurePerHour, 2); // 8 interior − 3 gear
        }

        [Fact]
        public void Components_AreCopiedFromTheSameEnvironmentTheTickUsed()
        {
            var env = Env(SurvivorExposureLocation.Expedition, zone: 207f, weather: 150f,
                fallout: 5f, anomaly: 12f, locationId: "loc_reactor_rim");
            var b = ExposureBreakdown.Build(env, 0f, 40f, 900f);
            Assert.Equal(40f, b.BaseRadRate, 2);
            Assert.Equal(150f, b.WeatherModifier, 2);
            Assert.Equal(5f, b.FalloutContamination, 2);
            Assert.Equal(12f, b.AnomalyRate, 2);
            Assert.Equal(207f, b.ZoneAmbient, 2);
            Assert.Equal(40f, b.AccumulatedDose, 2);
            Assert.Equal(900f, b.LifetimeDose, 2);
        }

        [Theory]
        [InlineData(SurvivorExposureLocation.ShelterInterior, "", "Shelter interior")]
        [InlineData(SurvivorExposureLocation.ShelterPerimeter, "", "Shelter perimeter")]
        [InlineData(SurvivorExposureLocation.WastelandOutdoors, "", "Surface")]
        [InlineData(SurvivorExposureLocation.Expedition, "loc_x", "Expedition — loc_x")]
        [InlineData(SurvivorExposureLocation.Expedition, "", "Expedition")]
        public void PositionLabel_IsMeaningfulForEveryLocationKind(
            SurvivorExposureLocation kind, string locationId, string expected)
        {
            Assert.Equal(expected, ExposureBreakdown.BuildPositionLabel(kind, locationId));
        }

        [Fact]
        public void PositionLabel_UsesDisplayNameWhenProvided()
        {
            Assert.Equal("Expedition — Reactor Rim",
                ExposureBreakdown.BuildPositionLabel(
                    SurvivorExposureLocation.Expedition, "loc_reactor_rim", "Reactor Rim"));
        }

        [Fact]
        public void DisplayLine_ShowsCause_AndHidesZeroComponents()
        {
            var env = Env(SurvivorExposureLocation.WastelandOutdoors, zone: 190f, weather: 150f);
            var b = ExposureBreakdown.Build(env, 5f, 12.5f, 230f);
            string line = b.ToDisplayLine();
            Assert.Contains("Surface", line);
            Assert.Contains("Zone 190", line);
            Assert.Contains("weather +150", line);
            Assert.Contains("gear −5", line);
            Assert.Contains("185.0 mSv/h", line);
            Assert.Contains("dose 12.5/100", line);
            Assert.Contains("lifetime 230.0", line);
            Assert.DoesNotContain("fallout", line);
            Assert.DoesNotContain("anomaly", line);
            Assert.DoesNotContain("shielding", line);
        }

        [Fact]
        public void DisplayLine_ShelterInterior_ShowsShielding()
        {
            var env = Env(SurvivorExposureLocation.ShelterInterior, zone: 2f, shielding: 0.8f);
            var b = ExposureBreakdown.Build(env, 0f, 0f, 0f);
            string line = b.ToDisplayLine();
            Assert.Contains("Shelter interior", line);
            Assert.Contains("shielding −0.8", line);
            Assert.Contains("1.2 mSv/h", line);
        }

        [Fact]
        public void Build_NullEnvironment_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ExposureBreakdown.Build(null!, 0f, 0f, 0f));
        }
    }

    /// <summary>
    /// Source-level gates for the §16 ownership contract: the panel must read
    /// the host breakdown (single source of truth) and must not carry its own
    /// dose arithmetic; the host read model must use the canonical resolver.
    /// </summary>
    public sealed class ExposureBreakdownSourceGateTests
    {
        private static string RepoRoot
        {
            get
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8; i++)
                {
                    if (File.Exists(Path.Combine(dir, "Ashfall.csproj"))) return dir;
                    dir = Path.GetDirectoryName(dir)!;
                }
                throw new InvalidOperationException("repository root not found");
            }
        }

        private static string Read(string relativePath)
            => File.ReadAllText(Path.Combine(RepoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar)));

        [Fact]
        public void RadiationDetailPanel_ConsumesHostBreakdown()
        {
            string src = Read("src/UI/RadiationDetailPanel.cs");
            Assert.Contains("GetExposureBreakdown", src, StringComparison.Ordinal);
            Assert.Contains("ToDisplayLine", src, StringComparison.Ordinal);
        }

        [Fact]
        public void RadiationDetailPanel_DoesNotRecomputeDoseMath()
        {
            string src = Read("src/UI/RadiationDetailPanel.cs");
            Assert.DoesNotContain("ComputeExposurePerHour", src, StringComparison.Ordinal);
            Assert.DoesNotContain("ComputeEffectiveRate", src, StringComparison.Ordinal);
            Assert.DoesNotContain("ComputeGearProtection", src, StringComparison.Ordinal);
        }

        [Fact]
        public void HostBreakdown_UsesCanonicalEffectiveRateResolver()
        {
            string src = Read("src/Host/SurvivorsHostSession.cs");
            Assert.Contains("ExposureBreakdown.Build", src, StringComparison.Ordinal);
        }
    }
}