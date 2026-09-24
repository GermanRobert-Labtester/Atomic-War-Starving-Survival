// SPDX-License-Identifier: MIT
using Ashfall.Core.Needs;
using Xunit;

namespace Ashfall.Core.Tests.Needs
{
    public sealed class SleepNarrativePhantomPainTests
    {
        [Fact]
        public void Classify_WithPhantomPain_ReturnsPhantomPainKind_WhenNoNightmare()
        {
            var record = new SurvivorMentalHealthRecord();
            var kind = SleepNarrativeProjection.Classify(record, hasPhantomPain: true);

            Assert.Equal(SleepBeatKind.PhantomPain, kind);
        }

        [Fact]
        public void Classify_WithPhantomPainAndCrisis_NightmareDominates()
        {
            var record = new SurvivorMentalHealthRecord
            {
                currentCrisisId = "crisis_breakdown",
                crisisDaysRemaining = 3
            };
            var kind = SleepNarrativeProjection.Classify(record, hasPhantomPain: true);

            Assert.Equal(SleepBeatKind.Nightmare, kind);
        }

        [Fact]
        public void Project_WithPhantomPain_EmitsPhantomPainBeat()
        {
            var mentalHealth = new SurvivorMentalHealthSystem();
            var projection = new SleepNarrativeProjection(mentalHealth);

            var beat = projection.Project("survivor_amputee_1", day: 10, hasPhantomPain: true);

            Assert.Equal(SleepBeatKind.PhantomPain, beat.Kind);
            Assert.True(beat.Emitted);
            Assert.Contains("sleep_phantom_pain", beat.JournalKey);
            Assert.NotEmpty(beat.Text);
        }

        [Fact]
        public void Project_NegativeExtremeDay_StillReturnsRestfulLine()
        {
            var projection = new SleepNarrativeProjection(new SurvivorMentalHealthSystem());

            var beat = projection.Project("survivor_boundary", day: int.MinValue + 1);

            Assert.Equal(SleepBeatKind.Restful, beat.Kind);
            Assert.NotEmpty(beat.Text);
        }

        [Fact]
        public void Project_WithoutPhantomPain_HealthySurvivorIsRestful()
        {
            var mentalHealth = new SurvivorMentalHealthSystem();
            var projection = new SleepNarrativeProjection(mentalHealth);

            var beat = projection.Project("survivor_healthy_1", day: 10, hasPhantomPain: false);

            Assert.Equal(SleepBeatKind.Restful, beat.Kind);
            Assert.False(beat.Emitted);
            Assert.Contains("sleep_restful", beat.JournalKey);
        }
    }
}
