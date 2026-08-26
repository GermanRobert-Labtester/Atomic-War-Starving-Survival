using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class WeatherAtmosphereMapTests
    {
        [Fact]
        public void Clear_HasFullVisibilityAndNoParticles()
        {
            var a = WeatherAtmosphereMap.For(WeatherKind.Clear, headless: true);
            Assert.Equal(1f, a.Visibility);
            Assert.Equal(0, a.AshParticleCount);
            Assert.Equal(0, a.RainParticleCount);
            Assert.Equal(0f, a.FogDensity);
        }

        [Fact]
        public void Headless_AlwaysZeroParticles()
        {
            var kinds = new[] { WeatherKind.Ashfall, WeatherKind.FalloutStorm,
                WeatherKind.Blizzard, WeatherKind.BlackSnow, WeatherKind.Rain };
            foreach (var k in kinds)
            {
                var a = WeatherAtmosphereMap.For(k, headless: true);
                Assert.Equal(0, a.AshParticleCount);
                Assert.Equal(0, a.RainParticleCount);
            }
        }

        [Fact]
        public void FalloutStorm_HasHighestFogAndLowestVisibility()
        {
            var a = WeatherAtmosphereMap.For(WeatherKind.FalloutStorm, headless: false);
            Assert.True(a.FogDensity >= 0.7f);
            Assert.True(a.Visibility <= 0.4f);
            Assert.True(a.AshParticleCount > 0);
        }

        [Fact]
        public void Rain_HasRainParticlesButNoAsh()
        {
            var a = WeatherAtmosphereMap.For(WeatherKind.Rain, headless: false);
            Assert.True(a.RainParticleCount > 0);
            Assert.Equal(0, a.AshParticleCount);
        }

        [Fact]
        public void AshFall_HasAshButNotExtremeFog()
        {
            var a = WeatherAtmosphereMap.For(WeatherKind.Ashfall, headless: false);
            Assert.True(a.AshParticleCount > 0);
            Assert.True(a.FogDensity < 0.7f);
        }

        [Fact]
        public void UnknownKind_FallsBackToClear()
        {
            var a = WeatherAtmosphereMap.For((WeatherKind)999, headless: true);
            Assert.Equal(1f, a.Visibility);
            Assert.Equal(0, a.AshParticleCount);
        }

        [Fact]
        public void Deterministic_SameInputs_IdenticalOutput()
        {
            var a = WeatherAtmosphereMap.For(WeatherKind.Blizzard, headless: false);
            var b = WeatherAtmosphereMap.For(WeatherKind.Blizzard, headless: false);
            Assert.Equal(a.AshParticleCount, b.AshParticleCount);
            Assert.Equal(a.RainParticleCount, b.RainParticleCount);
            Assert.Equal(a.Visibility, b.Visibility);
            Assert.Equal(a.AudioCueId, b.AudioCueId);
        }

        [Fact]
        public void Tint_HasFourComponentsRGBA()
        {
            var a = WeatherAtmosphereMap.For(WeatherKind.Overcast, headless: true);
            Assert.Equal(4, a.Tint.Length);
        }
    }
}
