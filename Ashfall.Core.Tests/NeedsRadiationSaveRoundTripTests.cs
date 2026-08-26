using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Radiation;

namespace Ashfall.Core.Tests
{
    public class NeedsRadiationSaveRoundTripTests
    {
        [Serializable]
        public class SurvivorSliceTestState
        {
            public string id = string.Empty;
            public float hunger;
            public float thirst;
            public float fatigue;
            public float warmth = 100f;
            public float morale = 50f;
            public float health = 100f;
            public float hygiene = 100f;
            public float radiationDose;
            public float lifetimeRadiationExposure;
            public bool hasRadResistance;
            public float radResistanceHoursRemaining;
            public bool hasAcuteSickness;
            public bool hasChronicIllness;
            public bool isAlive = true;
        }

        [Serializable]
        public class SurvivorsTestSave
        {
            public List<SurvivorSliceTestState> survivors = new List<SurvivorSliceTestState>();
            public string Checksum = string.Empty;
        }

        [Fact]
        public void SurvivorNeeds_RoundTrip_PreservesExactValues()
        {
            var original = new SurvivorsTestSave
            {
                survivors = new List<SurvivorSliceTestState>
                {
                    new SurvivorSliceTestState
                    {
                        id = "survivor_dr_sarah_chen",
                        health = 82.5f,
                        hunger = 37.2f,
                        thirst = 45.8f,
                        fatigue = 18.0f,
                        warmth = 92.4f,
                        morale = 64.0f,
                        hygiene = 78.5f,
                        radiationDose = 14.5f,
                        lifetimeRadiationExposure = 42.0f,
                        hasRadResistance = true,
                        radResistanceHoursRemaining = 6.5f,
                        hasAcuteSickness = false,
                        hasChronicIllness = false,
                        isAlive = true
                    },
                    new SurvivorSliceTestState
                    {
                        id = "survivor_gunner_mikhail",
                        health = 54.0f,
                        hunger = 85.0f,
                        thirst = 72.0f,
                        fatigue = 60.0f,
                        warmth = 40.0f,
                        morale = 25.0f,
                        hygiene = 30.0f,
                        radiationDose = 55.0f,
                        lifetimeRadiationExposure = 120.0f,
                        hasRadResistance = false,
                        radResistanceHoursRemaining = 0f,
                        hasAcuteSickness = true,
                        hasChronicIllness = true,
                        isAlive = true
                    }
                }
            };
            original.Checksum = SaveChecksum.Compute(original);

            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(original);
            Assert.False(string.IsNullOrWhiteSpace(json));

            var restored = serializer.Deserialize<SurvivorsTestSave>(json);
            Assert.NotNull(restored);
            Assert.Equal(2, restored.survivors.Count);

            var chen = restored.survivors[0];
            Assert.Equal("survivor_dr_sarah_chen", chen.id);
            Assert.Equal(82.5f, chen.health);
            Assert.Equal(37.2f, chen.hunger);
            Assert.Equal(45.8f, chen.thirst);
            Assert.Equal(14.5f, chen.radiationDose);
            Assert.True(chen.hasRadResistance);

            var mikhail = restored.survivors[1];
            Assert.Equal("survivor_gunner_mikhail", mikhail.id);
            Assert.Equal(54.0f, mikhail.health);
            Assert.Equal(55.0f, mikhail.radiationDose);
            Assert.True(mikhail.hasAcuteSickness);
            Assert.True(mikhail.hasChronicIllness);

            string recomputed = SaveChecksum.Compute(restored);
            Assert.Equal(original.Checksum, recomputed);
        }

        [Fact]
        public void SurvivorNeeds_MutationChangesChecksum()
        {
            var save = new SurvivorsTestSave
            {
                survivors = new List<SurvivorSliceTestState>
                {
                    new SurvivorSliceTestState
                    {
                        id = "survivor_dr_sarah_chen",
                        health = 100f,
                        hunger = 0f
                    }
                }
            };
            string hash1 = SaveChecksum.Compute(save);

            save.survivors[0].hunger = 50f;
            string hash2 = SaveChecksum.Compute(save);

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void NeedsSystem_Tick_CalculatesAccurateDriftAndConsequences()
        {
            var needs = new NeedsSystem();
            var state = new SurvivorNeedsState
            {
                Id = "survivor_test",
                Health = 100f,
                Hunger = 0f,
                Thirst = 0f,
                Fatigue = 0f,
                Warmth = 100f,
                Morale = 50f
            };
            needs.Register(state);

            // Tick 10 hours
            needs.Tick(10f);

            Assert.True(state.Hunger > 0f, "Hunger should increase after tick");
            Assert.True(state.Thirst > 0f, "Thirst should increase after tick");
            Assert.True(state.Fatigue > 0f, "Fatigue should increase after tick");

            // Starvation pressure test
            state.Hunger = 95f; // Critical
            float prevHealth = state.Health;
            needs.Tick(5f);

            Assert.True(state.Health < prevHealth, "Health should drop when hunger is critical");
        }
    }
}
