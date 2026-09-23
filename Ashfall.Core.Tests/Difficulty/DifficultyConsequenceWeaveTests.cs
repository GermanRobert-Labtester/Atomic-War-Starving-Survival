// SPDX-License-Identifier: MIT
using System;
using Xunit;
using Ashfall.Core.Difficulty;

namespace Ashfall.Core.Tests.Difficulty
{
    public sealed class DifficultyConsequenceWeaveTests
    {
        [Fact]
        public void LegacyScalars_ProducesDefaultMultipliers()
        {
            var weave = new DifficultyConsequenceWeave(DifficultyScalarsProvider.Legacy);

            Assert.Equal(1f, weave.WarStageSeverityMultiplier);
            Assert.Equal(10, weave.ComputeCrisisDeadlineDays(10));
            Assert.Equal(1f, weave.ShockWeightMultiplier);
            Assert.True(DifficultyConsequenceWeave.IsMonotonicallyHarsherOrEqual(DifficultyScalarsProvider.Legacy, DifficultyScalarsProvider.Legacy));
        }

        [Fact]
        public void HarsherScalars_ScalesConsequencesAndValidatesMonotonicity()
        {
            var harshScalars = new DifficultyScalars
            {
                hunger_rate_mult = 1.5f,
                thirst_rate_mult = 1.5f,
                radiation_gain_mult = 1.5f,
                disease_onset_mult = 1.5f,
                hostile_encounter_mult = 1.5f,
                market_price_mult = 1.2f,
                equipment_decay_mult = 1.3f,
                crisis_deadline_mult = 0.8f
            };

            var harshPreset = new DifficultyPreset
            {
                id = "difficulty_harsh",
                display_name = "Harsh",
                description = "Harsh survival parameters.",
                scalars = harshScalars
            };

            var harshProvider = DifficultyScalarsProvider.FromPreset(harshPreset);
            var weave = new DifficultyConsequenceWeave(harshProvider);

            // War stage severity: 1.5 * (1 / 0.8) = 1.875
            Assert.Equal(1.875f, weave.WarStageSeverityMultiplier, precision: 3);

            // Crisis deadline: 10 * 0.8 = 8 days
            Assert.Equal(8, weave.ComputeCrisisDeadlineDays(10));

            // Shock weight: (1.2 + 1.5) * 0.5 = 1.35
            Assert.Equal(1.35f, weave.ShockWeightMultiplier, precision: 2);

            // Monotonicity checks
            Assert.True(DifficultyConsequenceWeave.IsMonotonicallyHarsherOrEqual(harshProvider, DifficultyScalarsProvider.Legacy));
            Assert.False(DifficultyConsequenceWeave.IsMonotonicallyHarsherOrEqual(DifficultyScalarsProvider.Legacy, harshProvider));
        }

        [Fact]
        public void DeadlineClamping_NeverReturnsZeroOrNegativeDays()
        {
            var weave = new DifficultyConsequenceWeave(DifficultyScalarsProvider.Legacy);

            Assert.Equal(1, weave.ComputeCrisisDeadlineDays(0));
            Assert.Equal(1, weave.ComputeCrisisDeadlineDays(-5));
        }
    }
}
