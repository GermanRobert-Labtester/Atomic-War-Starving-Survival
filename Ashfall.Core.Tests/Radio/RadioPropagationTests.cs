// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public sealed class RadioPropagationTests
    {
        [Fact]
        public void WeatherAttenuation_OrderedByAtmosphericSeverity()
        {
            float clear = RadioPropagationEngine.GetWeatherAttenuation(WeatherKind.Clear);
            float overcast = RadioPropagationEngine.GetWeatherAttenuation(WeatherKind.Overcast);
            float ashfall = RadioPropagationEngine.GetWeatherAttenuation(WeatherKind.Ashfall);
            float blizzard = RadioPropagationEngine.GetWeatherAttenuation(WeatherKind.Blizzard);
            float fallout = RadioPropagationEngine.GetWeatherAttenuation(WeatherKind.FalloutStorm);
            float emp = RadioPropagationEngine.GetWeatherAttenuation(WeatherKind.EMPStorm);

            Assert.Equal(1.0f, clear);
            Assert.True(clear > overcast, "Clear must attenuate less than Overcast");
            Assert.True(overcast > ashfall, "Overcast must attenuate less than Ashfall");
            Assert.True(ashfall > blizzard, "Ashfall must attenuate less than Blizzard");
            Assert.True(blizzard > fallout, "Blizzard must attenuate less than FalloutStorm");
            Assert.True(fallout > emp, "FalloutStorm must attenuate less than EMPStorm");
            Assert.InRange(emp, 0.1f, 0.25f);
        }

        [Fact]
        public void TerrainAttenuation_SubterraneanAndMountainsPenalizePropagation()
        {
            float open = RadioPropagationEngine.GetTerrainAttenuation("open");
            float urban = RadioPropagationEngine.GetTerrainAttenuation("urban");
            float mountain = RadioPropagationEngine.GetTerrainAttenuation("mountainous");
            float sub = RadioPropagationEngine.GetTerrainAttenuation("subterranean");

            Assert.Equal(1.0f, open);
            Assert.True(open > urban, "Open must propagate better than Urban");
            Assert.True(urban > mountain, "Urban must propagate better than Mountainous");
            Assert.True(mountain > sub, "Mountainous must propagate better than Subterranean");
            Assert.Equal(0.40f, sub);
        }

        [Fact]
        public void DistancePathLoss_MonotonicallyDecreasesWithTicks()
        {
            float d1 = RadioPropagationEngine.GetDistanceAttenuation(1);
            float d3 = RadioPropagationEngine.GetDistanceAttenuation(3);
            float d6 = RadioPropagationEngine.GetDistanceAttenuation(6);
            float d12 = RadioPropagationEngine.GetDistanceAttenuation(12);

            Assert.Equal(1.0f, d1);
            Assert.True(d1 > d3);
            Assert.True(d3 > d6);
            Assert.True(d6 > d12);
        }

        [Fact]
        public void TuningOffset_AttenuatesSignalWithinTolerance()
        {
            var def = new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_88_3",
                FrequencyMhzStr = "88.3",
                SourceName = "Trapped Mechanic",
                MessageFragments = new List<DistressMessageFragment>
                {
                    new DistressMessageFragment { Day = 1, Clarity = 0.5f, Text = "Mayday" }
                }
            };

            var ctx = new RadioPropagationContext(1, WeatherKind.Clear, 2, "urban", 0.05f);

            var exact = RadioPropagationEngine.EvaluatePropagation(def, ctx, 0.0f);
            var slightOffset = RadioPropagationEngine.EvaluatePropagation(def, ctx, 0.2f);
            var outside = RadioPropagationEngine.EvaluatePropagation(def, ctx, 0.6f);

            Assert.True(exact.IsLocked);
            Assert.True(slightOffset.CarrierStrength < exact.CarrierStrength);
            Assert.Equal(0f, outside.CarrierStrength);
            Assert.False(outside.IsLocked);
        }

        [Fact]
        public void EMPStorm_RaisesNoiseFloorAndSuppressesLock()
        {
            var def = new DistressSignalDefinition
            {
                FrequencyId = "freq_distress_88_3",
                FrequencyMhzStr = "88.3",
                SourceName = "Trapped Mechanic",
                MessageFragments = new List<DistressMessageFragment>
                {
                    new DistressMessageFragment { Day = 1, Clarity = 0.8f, Text = "Trapped" }
                }
            };

            var clearCtx = new RadioPropagationContext(1, WeatherKind.Clear, 3, "urban", 0.05f);
            var empCtx = new RadioPropagationContext(1, WeatherKind.EMPStorm, 3, "urban", 0.05f);

            var clearRes = RadioPropagationEngine.EvaluatePropagation(def, clearCtx, 0.0f);
            var empRes = RadioPropagationEngine.EvaluatePropagation(def, empCtx, 0.0f);

            Assert.True(clearRes.IsLocked);
            Assert.False(empRes.IsLocked);
            Assert.True(empRes.NoiseFloor > clearRes.NoiseFloor);
            Assert.True(empRes.EffectiveVu < RadioPropagationEngine.LockVuThreshold);
        }

        [Fact]
        public void MonotonicClarity_PreservesHighestObservedClarity()
        {
            var active = new ActiveDistressSignal
            {
                SignalId = "freq_distress_88_3",
                HighestClarity = 0.0f
            };

            RadioPropagationEngine.UpdateMonotonicClarity(active, 0.45f);
            Assert.Equal(0.45f, active.HighestClarity);

            // Lower clarity (e.g. bad weather or distant observation) should NOT decrease it
            RadioPropagationEngine.UpdateMonotonicClarity(active, 0.20f);
            Assert.Equal(0.45f, active.HighestClarity);

            // Higher clarity updates it
            RadioPropagationEngine.UpdateMonotonicClarity(active, 0.90f);
            Assert.Equal(0.90f, active.HighestClarity);
        }
    }
}
