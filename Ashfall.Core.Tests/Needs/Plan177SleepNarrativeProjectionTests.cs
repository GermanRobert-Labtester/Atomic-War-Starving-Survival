// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Needs;
using Xunit;

namespace Ashfall.Core.Tests.Plan177Psych
{
    /// <summary>
    /// DEBT-177-SLEEP-EVENT-CONSUMER — the sleep beat is a read-only projection
    /// over the existing mental-health record. It derives a class, produces a
    /// deterministic line, mutates nothing, and never invents a dream system.
    /// </summary>
    public sealed class Plan177SleepNarrativeProjectionTests
    {
        private static SurvivorMentalHealthSystem CreateSystem() => new SurvivorMentalHealthSystem();

        [Fact]
        public void Calm_survivor_reads_restful_and_is_not_emitted()
        {
            var mental = CreateSystem();
            var record = mental.GetOrCreateRecord("sv_calm");
            record.stressPermille = 100;

            var beat = new SleepNarrativeProjection(mental).Project("sv_calm", 5);

            Assert.Equal(SleepBeatKind.Restful, beat.Kind);
            Assert.False(beat.Emitted, "restful sleep must not fill the journal");
        }

        [Fact]
        public void Insomnia_alone_reads_troubled()
        {
            var mental = CreateSystem();
            var record = mental.GetOrCreateRecord("sv_tired");
            record.stressPermille = 150;
            record.insomniaDaysRemaining = 3;

            Assert.Equal(SleepBeatKind.Troubled, new SleepNarrativeProjection(mental).Project("sv_tired", 5).Kind);
        }

        [Fact]
        public void Trauma_with_insomnia_reads_nightmare_and_emits()
        {
            var mental = CreateSystem();
            var record = mental.GetOrCreateRecord("sv_scarred");
            record.stressPermille = 200;
            record.insomniaDaysRemaining = 2;
            record.activeTraumaIds.Add("trauma_nightmares");

            var beat = new SleepNarrativeProjection(mental).Project("sv_scarred", 6);

            Assert.Equal(SleepBeatKind.Nightmare, beat.Kind);
            Assert.True(beat.Emitted);
            Assert.Equal("sleep_nightmare:sv_scarred", beat.JournalKey);
        }

        [Fact]
        public void Active_crisis_reads_nightmare_even_without_insomnia()
        {
            var record = new SurvivorMentalHealthRecord
            {
                survivorId = "sv_crisis",
                stressPermille = 300,
                currentCrisisId = "crisis_acute_stress",
                crisisDaysRemaining = 2
            };

            Assert.Equal(SleepBeatKind.Nightmare, SleepNarrativeProjection.Classify(record));
        }

        [Fact]
        public void Extreme_stress_alone_reads_nightmare()
        {
            var record = new SurvivorMentalHealthRecord
            {
                survivorId = "sv_stressed",
                stressPermille = SleepNarrativeProjection.NightmareStressPermille
            };

            Assert.Equal(SleepBeatKind.Nightmare, SleepNarrativeProjection.Classify(record));
        }

        [Fact]
        public void Projection_mutates_no_state()
        {
            var mental = CreateSystem();
            var record = mental.GetOrCreateRecord("sv_still");
            record.stressPermille = 500;
            record.insomniaDaysRemaining = 4;
            int stressBefore = record.stressPermille;
            int insomniaBefore = record.insomniaDaysRemaining;

            new SleepNarrativeProjection(mental).Project("sv_still", 9);

            Assert.Equal(stressBefore, record.stressPermille);
            Assert.Equal(insomniaBefore, record.insomniaDaysRemaining);
            Assert.Empty(record.activeTraumaIds);
        }

        [Fact]
        public void Same_seed_and_day_replay_the_same_line()
        {
            string Line(int seed)
            {
                var mental = CreateSystem();
                mental.GetOrCreateRecord("sv_rep").insomniaDaysRemaining = 1;
                var beat = new SleepNarrativeProjection(mental, new SeededRng(seed)).Project("sv_rep", 7);
                return beat.Text;
            }

            Assert.Equal(Line(1986), Line(1986));
        }

        [Fact]
        public void Empty_survivor_id_yields_no_beat()
        {
            var beat = new SleepNarrativeProjection(CreateSystem()).Project(string.Empty, 3);
            Assert.Equal(SleepBeatKind.None, beat.Kind);
            Assert.False(beat.Emitted);
        }
    }
}
