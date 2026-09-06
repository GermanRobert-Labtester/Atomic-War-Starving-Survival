// SPDX-License-Identifier: MIT
using Ashfall.Core.Audio;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ReactiveAmbienceEvaluatorTests
    {
        [Fact]
        public void PowerState_SelectsBunkerHumOrLowPower()
        {
            var evaluator = new ReactiveAmbienceEvaluator();

            // Powered and healthy -> Bunker hum
            var res1 = evaluator.Evaluate(new AmbienceEvaluationContext { HasPower = true, IsBrownout = false });
            Assert.True(res1.BunkerHumActive);
            Assert.False(res1.BunkerLowPowerActive);

            // Brownout -> Low power active, hum inactive
            var res2 = evaluator.Evaluate(new AmbienceEvaluationContext { HasPower = true, IsBrownout = true });
            Assert.False(res2.BunkerHumActive);
            Assert.True(res2.BunkerLowPowerActive);

            // Power loss -> Low power active, hum inactive
            var res3 = evaluator.Evaluate(new AmbienceEvaluationContext { HasPower = false, IsBrownout = false });
            Assert.False(res3.BunkerHumActive);
            Assert.True(res3.BunkerLowPowerActive);
        }

        [Fact]
        public void RadiationSpike_EnforcesHysteresis()
        {
            var evaluator = new ReactiveAmbienceEvaluator();

            // 1. Initial sub-threshold level: 30 (below high threshold of 35) -> no spike
            var res1 = evaluator.Evaluate(new AmbienceEvaluationContext { ExternalRadiation = 30f });
            Assert.False(res1.RadiationSpikeActive);
            Assert.Equal(0f, res1.GeigerIntensity);

            // 2. Spike hits 36 (>= 35) -> activates
            var res2 = evaluator.Evaluate(new AmbienceEvaluationContext { ExternalRadiation = 36f });
            Assert.True(res2.RadiationSpikeActive);
            Assert.True(res2.GeigerIntensity > 0f);

            // 3. Radiation drops to 25 (between 20 and 35) -> hysteresis keeps it active!
            var res3 = evaluator.Evaluate(new AmbienceEvaluationContext { ExternalRadiation = 25f });
            Assert.True(res3.RadiationSpikeActive, "Hysteresis should keep radiation spike active between 20 and 35");

            // 4. Radiation drops to 18 (< 20) -> deactivates!
            var res4 = evaluator.Evaluate(new AmbienceEvaluationContext { ExternalRadiation = 18f });
            Assert.False(res4.RadiationSpikeActive);
            Assert.Equal(0f, res4.GeigerIntensity);
        }

        [Fact]
        public void SicknessMisery_EnforcesHysteresis()
        {
            var evaluator = new ReactiveAmbienceEvaluator();

            // 1. 2 infected survivors (below high threshold of 3) -> no misery layer
            var res1 = evaluator.Evaluate(new AmbienceEvaluationContext { InfectedSurvivorCount = 2 });
            Assert.False(res1.SicknessMiseryActive);

            // 2. 3 infected survivors (>= 3) -> activates
            var res2 = evaluator.Evaluate(new AmbienceEvaluationContext { InfectedSurvivorCount = 3 });
            Assert.True(res2.SicknessMiseryActive);
            Assert.True(res2.SicknessMiseryIntensity > 0f);

            // 3. Drops to 2 infected survivors -> hysteresis keeps misery layer active
            var res3 = evaluator.Evaluate(new AmbienceEvaluationContext { InfectedSurvivorCount = 2 });
            Assert.True(res3.SicknessMiseryActive, "Hysteresis should keep sickness misery active at 2 infected");

            // 4. Drops to 1 infected survivor (<= 1) -> deactivates!
            var res4 = evaluator.Evaluate(new AmbienceEvaluationContext { InfectedSurvivorCount = 1 });
            Assert.False(res4.SicknessMiseryActive);
            Assert.Equal(0f, res4.SicknessMiseryIntensity);
        }

        [Fact]
        public void SurfaceListening_SelectsContextualCues()
        {
            var evaluator = new ReactiveAmbienceEvaluator();

            // Surface listening inactive -> empty cue
            var res1 = evaluator.Evaluate(new AmbienceEvaluationContext { IsSurfaceListening = false, WeatherKind = "Ashfall" });
            Assert.Equal(string.Empty, res1.SurfaceLoopCue);

            // Ashfall weather on surface
            var res2 = evaluator.Evaluate(new AmbienceEvaluationContext { IsSurfaceListening = true, WeatherKind = "Ashfall" });
            Assert.Equal("amb_surface_ashfall", res2.SurfaceLoopCue);

            // Radiation storm overrides surface weather
            var res3 = evaluator.Evaluate(new AmbienceEvaluationContext { IsSurfaceListening = true, ExternalRadiation = 50f });
            Assert.Equal("amb_rad_storm_exterior", res3.SurfaceLoopCue);
        }
    }
}
