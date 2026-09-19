// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class Plan220ShelterAtmosphereIntegrationTests
    {
        [Fact]
        public void EnvironmentalPipeline_UpdatesAllFacets_AndTransitionsMoodCategories()
        {
            var system = new ShelterAtmosphereSystem();
            AtmosphereMoodCategory lastShifted = AtmosphereMoodCategory.Neutral;
            int shiftCount = 0;
            system.OnMoodShifted += (cat, score) =>
            {
                lastShifted = cat;
                shiftCount++;
            };

            // Bleak conditions: dark, loud, dirty, freezing, isolated
            system.UpdateEnvironmentalInputs(
                lighting: 10f,
                acousticComfort: 15f,
                airPurity: 20f,
                thermalComfort: 15f,
                cleanliness: 20f,
                socialWarmth: 10f,
                decorationLevel: 5f,
                currentDay: 1);

            Assert.Equal(AtmosphereMoodCategory.Bleak, system.CurrentMoodCategory);
            Assert.True(system.OverallMoodScore < 25f);
            Assert.True(shiftCount > 0);

            // Transition to Welcoming conditions
            system.UpdateEnvironmentalInputs(
                lighting: 85f,
                acousticComfort: 80f,
                airPurity: 85f,
                thermalComfort: 85f,
                cleanliness: 80f,
                socialWarmth: 75f,
                decorationLevel: 70f,
                currentDay: 2);

            Assert.True(system.CurrentMoodCategory == AtmosphereMoodCategory.Comfortable || system.CurrentMoodCategory == AtmosphereMoodCategory.Welcoming);
            Assert.True(system.OverallMoodScore >= 75f);
            Assert.Equal(2, system.LastUpdatedDay);
        }

        [Fact]
        public void NoiseDiscipline_AttenuatesRoomNoise_AndImprovesAcousticComfort()
        {
            var noiseSys = new ShelterNoiseSystem();
            var atmoSys = new ShelterAtmosphereSystem();

            // Heavy machinery in workshop generates loud acoustic noise
            noiseSys.AddNoiseSource(NoiseSourceType.Machinery, "workshop", output: 85f, freq: NoiseFrequency.Medium);
            noiseSys.TickDay(currentDay: 1, currentHour: 12);

            float initialNoise = noiseSys.OverallNoiseLevel;
            Assert.True(initialNoise > 40f);

            float initialAcousticComfort = Math.Clamp(100f - initialNoise, 0f, 100f);
            atmoSys.UpdateEnvironmentalInputs(70f, initialAcousticComfort, 80f, 70f, 70f, 60f, 40f, 1);
            float initialMood = atmoSys.OverallMoodScore;

            // Apply acoustic insulation (wall and door soundproofing)
            noiseSys.SoundproofRoom("workshop", wallAdd: 60f, doorAdd: 50f);
            noiseSys.TickDay(currentDay: 2, currentHour: 12);

            float insulatedNoise = noiseSys.OverallNoiseLevel;
            Assert.True(insulatedNoise < initialNoise);

            float improvedAcousticComfort = Math.Clamp(100f - insulatedNoise, 0f, 100f);
            Assert.True(improvedAcousticComfort > initialAcousticComfort);

            atmoSys.UpdateEnvironmentalInputs(70f, improvedAcousticComfort, 80f, 70f, 70f, 60f, 40f, 2);
            Assert.True(atmoSys.OverallMoodScore > initialMood);
        }

        [Fact]
        public void QuietHours_Enforcement_AndDetectionRisk()
        {
            var noiseSys = new ShelterNoiseSystem();
            noiseSys.SetQuietHours(enabled: true, startHour: 22, endHour: 6);
            noiseSys.AddNoiseSource(NoiseSourceType.Generator, "power_room", output: 65f);

            NoiseEvent? capturedEvent = null;
            noiseSys.OnNoiseSpike += ev => capturedEvent = ev;

            // Tick during quiet hours
            noiseSys.TickDay(currentDay: 5, currentHour: 23);

            Assert.NotNull(capturedEvent);
            Assert.Equal("quiet_hours_violation", capturedEvent.EventType);
            Assert.True(noiseSys.DetectionRisk > 0f);

            float previousRisk = noiseSys.DetectionRisk;
            noiseSys.AttenuateDetectionRisk(2.0f);
            Assert.True(noiseSys.DetectionRisk < previousRisk);
        }

        [Theory]
        [InlineData(90f, 70f, 90f, 70f, 90f, 20f, 10f, AtmosphereProfileType.Sterile)]
        [InlineData(70f, 60f, 70f, 25f, 60f, 50f, 50f, AtmosphereProfileType.Cold)]
        [InlineData(70f, 25f, 70f, 70f, 60f, 80f, 50f, AtmosphereProfileType.Chaotic)]
        [InlineData(70f, 60f, 70f, 70f, 70f, 75f, 75f, AtmosphereProfileType.Warm)]
        [InlineData(75f, 85f, 85f, 75f, 75f, 50f, 40f, AtmosphereProfileType.Serene)]
        [InlineData(80f, 40f, 60f, 60f, 60f, 50f, 40f, AtmosphereProfileType.Industrial)]
        public void ProfileClassification_CorrectlyCategorizesShelterStates(
            float light, float acoustic, float air, float thermal, float clean, float social, float decor,
            AtmosphereProfileType expectedProfile)
        {
            var system = new ShelterAtmosphereSystem();
            system.UpdateEnvironmentalInputs(light, acoustic, air, thermal, clean, social, decor, 1);
            Assert.Equal(expectedProfile, system.ActiveProfile);
        }

        [Fact]
        public void ActiveModifiers_CalculatesExpectedMoraleAndProductivity()
        {
            var system = new ShelterAtmosphereSystem();
            // Warm profile setup:
            system.UpdateEnvironmentalInputs(80f, 70f, 80f, 80f, 75f, 80f, 80f, 1);
            Assert.Equal(AtmosphereProfileType.Warm, system.ActiveProfile);

            var mods = system.GetActiveModifiers();
            Assert.NotNull(mods);
            Assert.True(mods.MoraleModifier > 0f);
            Assert.True(mods.StressReliefModifier > 0f);
        }

        [Fact]
        public void StatePersistence_RoundTrips_AtmosphereAndNoiseStates()
        {
            var atmo1 = new ShelterAtmosphereSystem();
            atmo1.UpdateEnvironmentalInputs(88f, 76f, 82f, 79f, 85f, 70f, 65f, 10);

            var atmoState = atmo1.CaptureState();
            string jsonAtmo = JsonSerializer.Serialize(atmoState);
            var restoredAtmoState = JsonSerializer.Deserialize<AtmosphereState>(jsonAtmo);
            Assert.NotNull(restoredAtmoState);

            var atmo2 = new ShelterAtmosphereSystem();
            atmo2.RestoreState(restoredAtmoState);

            Assert.Equal(atmo1.OverallMoodScore, atmo2.OverallMoodScore);
            Assert.Equal(atmo1.CurrentMoodCategory, atmo2.CurrentMoodCategory);
            Assert.Equal(atmo1.ActiveProfile, atmo2.ActiveProfile);
            Assert.Equal(atmo1.LightingQuality, atmo2.LightingQuality);
            Assert.Equal(atmo1.AcousticComfort, atmo2.AcousticComfort);
            Assert.Equal(10, atmo2.LastUpdatedDay);

            var noise1 = new ShelterNoiseSystem();
            noise1.SetQuietHours(true, 23, 7);
            noise1.AddNoiseSource(NoiseSourceType.Alarm, "siren", 95f);
            noise1.SoundproofRoom("quarters", 40f, 30f);

            var noiseState = noise1.CaptureState();
            string jsonNoise = JsonSerializer.Serialize(noiseState);
            var restoredNoiseState = JsonSerializer.Deserialize<ShelterNoiseState>(jsonNoise);
            Assert.NotNull(restoredNoiseState);

            var noise2 = new ShelterNoiseSystem();
            noise2.RestoreState(restoredNoiseState);

            Assert.True(noise2.QuietHoursActive);
            Assert.Equal(23, noise2.QuietHoursStart);
            Assert.Equal(7, noise2.QuietHoursEnd);
            Assert.Single(noise2.Sources);
            Assert.Equal(2, noise2.RoomProfiles.Count);
        }
    }
}
