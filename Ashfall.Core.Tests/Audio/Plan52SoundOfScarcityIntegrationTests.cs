// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Audio;
using Xunit;

namespace Ashfall.Core.Tests.Audio
{
    public sealed class Plan52SoundOfScarcityIntegrationTests
    {
        [Fact]
        public void ScarcityAudioStateMachine_TransitionsBetweenBedsCorrectly()
        {
            var sm = new ScarcityAudioStateMachine();
            Assert.Equal(ScarcityAmbienceBed.BunkerAmbience, sm.CurrentBed);

            // Move to surface
            sm.UpdateContext(onSurface: true, "clear");
            Assert.Equal(ScarcityAmbienceBed.SurfaceAmbience, sm.CurrentBed);

            // Storm on surface
            sm.UpdateContext(onSurface: true, "ash_storm");
            Assert.Equal(ScarcityAmbienceBed.SurfaceStormAmbience, sm.CurrentBed);

            // Return to bunker
            sm.UpdateContext(onSurface: false, "clear", occupantCount: 5);
            Assert.Equal(ScarcityAmbienceBed.BunkerAmbience, sm.CurrentBed);

            // Crowded bunker
            sm.UpdateContext(onSurface: false, "clear", occupantCount: 25);
            Assert.Equal(ScarcityAmbienceBed.CrowdedShelterAmbience, sm.CurrentBed);
        }

        [Fact]
        public void ScarcityAudioStateMachine_SilenceWeatherStatesDropAllAmbience()
        {
            var sm = new ScarcityAudioStateMachine();

            sm.UpdateContext(onSurface: true, "silence");
            Assert.Equal(ScarcityAmbienceBed.AbsoluteSilence, sm.CurrentBed);

            sm.UpdateContext(onSurface: false, "silent_spring");
            Assert.Equal(ScarcityAmbienceBed.AbsoluteSilence, sm.CurrentBed);

            sm.UpdateContext(onSurface: true, "false_spring");
            Assert.Equal(ScarcityAmbienceBed.AbsoluteSilence, sm.CurrentBed);
        }

        [Fact]
        public void ScarcityAudioStateMachine_MapsAll22CanonicalWeatherKinds()
        {
            Assert.Equal(22, ScarcityAudioStateMachine.TotalMappedWeatherKinds);

            var sm = new ScarcityAudioStateMachine();
            string[] weatherKinds = new[]
            {
                "clear", "overcast", "fog", "ash_fall", "ash_storm",
                "acid_rain", "acid_snow", "bio_fog", "black_snow", "blood_rain",
                "emp_storm", "glass_storm", "rad_hail", "algae_bloom", "ash_lightning",
                "particulate_fog", "thermal_inversion", "ice_storm", "nuclear_winter",
                "silence", "silent_spring", "false_spring"
            };

            foreach (var w in weatherKinds)
            {
                var profile = sm.GetWeatherAudioProfile(w);
                Assert.NotNull(profile);
                Assert.Equal(w, profile.WeatherKind);
            }
        }

        [Fact]
        public void ScarcityAudioStateMachine_DuckingPolicyPreventsAlertPileup()
        {
            var sm = new ScarcityAudioStateMachine();

            Assert.Equal(0.0f, sm.CurrentDuckAttenuationDb);
            Assert.Equal(0, sm.ActiveAlertCount);

            sm.TriggerAlert("alert_radiation_spike");
            Assert.Equal(-7.0f, sm.CurrentDuckAttenuationDb);
            Assert.Equal(1, sm.ActiveAlertCount);

            sm.TriggerAlert("alert_storm_warning");
            Assert.Equal(-7.0f, sm.CurrentDuckAttenuationDb);
            Assert.Equal(2, sm.ActiveAlertCount);

            // Exceeding max concurrent alerts caps count
            sm.TriggerAlert("alert_airlock_breach");
            Assert.Equal(2, sm.ActiveAlertCount);

            sm.ReleaseAlert();
            Assert.Equal(1, sm.ActiveAlertCount);
            Assert.Equal(-7.0f, sm.CurrentDuckAttenuationDb);

            sm.ReleaseAlert();
            Assert.Equal(0, sm.ActiveAlertCount);
            Assert.Equal(0.0f, sm.CurrentDuckAttenuationDb);
        }

        [Fact]
        public void ScarcityAudioStateMachine_RadiationExposureLifecycleHaltsLoopOnEnd()
        {
            var sm = new ScarcityAudioStateMachine();

            Assert.False(sm.RadiationState.GeigerLoopActive);
            Assert.Equal(GeigerRateBand.Off, sm.RadiationState.RateBand);

            // Low radiation
            sm.SetRadiationExposure(0.2f);
            Assert.True(sm.RadiationState.GeigerLoopActive);
            Assert.Equal(GeigerRateBand.Low, sm.RadiationState.RateBand);

            // Medium radiation
            sm.SetRadiationExposure(1.5f);
            Assert.Equal(GeigerRateBand.Medium, sm.RadiationState.RateBand);

            // Lethal radiation
            sm.SetRadiationExposure(15.0f);
            Assert.Equal(GeigerRateBand.Lethal, sm.RadiationState.RateBand);

            // Explicit exposure end clears loop
            sm.EndRadiationExposure();
            Assert.False(sm.RadiationState.GeigerLoopActive);
            Assert.Equal(GeigerRateBand.Off, sm.RadiationState.RateBand);
            Assert.Equal(0.0f, sm.RadiationState.DoseRateRadsPerHour);
        }

        [Fact]
        public void ScarcityAudioStateMachine_FiresSeamsOnTransitionAndDucking()
        {
            var sm = new ScarcityAudioStateMachine();

            ScarcityAmbienceBed oldBedReported = ScarcityAmbienceBed.AbsoluteSilence;
            ScarcityAmbienceBed newBedReported = ScarcityAmbienceBed.AbsoluteSilence;
            float reportedDuck = 999.0f;
            bool geigerActiveReported = false;
            GeigerRateBand geigerBandReported = GeigerRateBand.Off;

            sm.OnAmbienceBedTransitionSeam = (oldB, newB) =>
            {
                oldBedReported = oldB;
                newBedReported = newB;
            };

            sm.OnDuckingChangedSeam = duck =>
            {
                reportedDuck = duck;
            };

            sm.OnGeigerLoopStateChangedSeam = (active, band) =>
            {
                geigerActiveReported = active;
                geigerBandReported = band;
            };

            sm.UpdateContext(onSurface: true, "clear");
            Assert.Equal(ScarcityAmbienceBed.BunkerAmbience, oldBedReported);
            Assert.Equal(ScarcityAmbienceBed.SurfaceAmbience, newBedReported);

            sm.TriggerAlert("alert_test");
            Assert.Equal(-7.0f, reportedDuck);

            sm.SetRadiationExposure(0.4f);
            Assert.True(geigerActiveReported);
            Assert.Equal(GeigerRateBand.Low, geigerBandReported);

            sm.EndRadiationExposure();
            Assert.False(geigerActiveReported);
            Assert.Equal(GeigerRateBand.Off, geigerBandReported);
        }
    }
}
