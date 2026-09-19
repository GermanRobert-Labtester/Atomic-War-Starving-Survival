// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class ShelterAtmosphereSystemTests
    {
        [Fact]
        public void UpdateEnvironmentalInputs_CalculatesMoodScoreAndCategory()
        {
            var system = new ShelterAtmosphereSystem();
            AtmosphereMoodCategory? shifted = null;
            system.OnMoodShifted += (cat, score) => shifted = cat;

            system.UpdateEnvironmentalInputs(
                lighting: 80f,
                acousticComfort: 80f,
                airPurity: 85f,
                thermalComfort: 80f,
                cleanliness: 85f,
                socialWarmth: 80f,
                decorationLevel: 75f,
                currentDay: 2
            );

            Assert.True(system.OverallMoodScore >= 75f);
            Assert.True(system.CurrentMoodCategory == AtmosphereMoodCategory.Comfortable || system.CurrentMoodCategory == AtmosphereMoodCategory.Welcoming);
            Assert.NotNull(shifted);
        }

        [Fact]
        public void EvaluateAtmosphere_MapsToSterileProfile_WhenCleanAndUndecorated()
        {
            var system = new ShelterAtmosphereSystem();
            system.UpdateEnvironmentalInputs(
                lighting: 80f,
                acousticComfort: 70f,
                airPurity: 80f,
                thermalComfort: 70f,
                cleanliness: 95f,
                socialWarmth: 40f,
                decorationLevel: 10f,
                currentDay: 3
            );

            Assert.Equal(AtmosphereProfileType.Sterile, system.ActiveProfile);
        }

        [Fact]
        public void EvaluateAtmosphere_MapsToWarmProfile_WhenDecoratedAndSocial()
        {
            var system = new ShelterAtmosphereSystem();
            system.UpdateEnvironmentalInputs(
                lighting: 70f,
                acousticComfort: 60f,
                airPurity: 70f,
                thermalComfort: 70f,
                cleanliness: 70f,
                socialWarmth: 85f,
                decorationLevel: 80f,
                currentDay: 4
            );

            Assert.Equal(AtmosphereProfileType.Warm, system.ActiveProfile);
        }

        [Fact]
        public void GetActiveModifiers_ReturnsExpectedBuffsAndDebuffs()
        {
            var system = new ShelterAtmosphereSystem();
            system.UpdateEnvironmentalInputs(80f, 80f, 80f, 80f, 70f, 80f, 80f, 1);

            var mods = system.GetActiveModifiers();
            Assert.NotNull(mods);
            Assert.True(mods.MoraleModifier > 0f);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new ShelterAtmosphereSystem();
            system1.UpdateEnvironmentalInputs(90f, 90f, 90f, 90f, 90f, 80f, 80f, 12);

            var state = system1.CaptureState();
            Assert.Equal(system1.OverallMoodScore, state.OverallMoodScore);
            Assert.Equal(12, state.LastUpdatedDay);

            var system2 = new ShelterAtmosphereSystem();
            system2.RestoreState(state);

            Assert.Equal(system1.OverallMoodScore, system2.OverallMoodScore);
            Assert.Equal(system1.CurrentMoodCategory, system2.CurrentMoodCategory);
            Assert.Equal(system1.ActiveProfile, system2.ActiveProfile);
        }
    }
}
