// SPDX-License-Identifier: MIT
using System.Collections.Generic;
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
            SetRecord(mental, "sv_calm", stressPermille: 100);

            var beat = new SleepNarrativeProjection(mental).Project("sv_calm", 5);

            Assert.Equal(SleepBeatKind.Restful, beat.Kind);
            Assert.False(beat.Emitted, "restful sleep must not fill the journal");
        }

        [Fact]
        public void Insomnia_alone_reads_troubled()
        {
            var mental = CreateSystem();
            SetRecord(mental, "sv_tired", stressPermille: 150, insomniaDaysRemaining: 3);

            Assert.Equal(SleepBeatKind.Troubled, new SleepNarrativeProjection(mental).Project("sv_tired", 5).Kind);
        }

        [Fact]
        public void Trauma_with_insomnia_reads_nightmare_and_emits()
        {
            var mental = CreateSystem();
            SetRecord(mental, "sv_scarred", stressPermille: 200, insomniaDaysRemaining: 2,
                activeTraumaIds: new List<string> { "trauma_nightmares" });

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
            SetRecord(mental, "sv_still", stressPermille: 500, insomniaDaysRemaining: 4);
            var before = mental.GetOrCreateRecord("sv_still");
            int stressBefore = before.stressPermille;
            int insomniaBefore = before.insomniaDaysRemaining;

            new SleepNarrativeProjection(mental).Project("sv_still", 9);

            var after = mental.GetOrCreateRecord("sv_still");
            Assert.Equal(stressBefore, after.stressPermille);
            Assert.Equal(insomniaBefore, after.insomniaDaysRemaining);
            Assert.Empty(after.activeTraumaIds);
        }

        [Fact]
        public void Same_seed_and_day_replay_the_same_line()
        {
            string Line(int seed)
            {
                var mental = CreateSystem();
                SetRecord(mental, "sv_rep", insomniaDaysRemaining: 1);
                var beat = new SleepNarrativeProjection(mental, new SeededRng(seed)).Project("sv_rep", 7);
                return beat.Text;
            }

            Assert.Equal(Line(1986), Line(1986));
        }

        private static void SetRecord(
            SurvivorMentalHealthSystem system,
            string survivorId,
            int stressPermille = 200,
            int insomniaDaysRemaining = 0,
            List<string>? activeTraumaIds = null)
        {
            var state = system.CaptureState();
            state.survivorRecords[survivorId] = new SurvivorMentalHealthRecord
            {
                survivorId = survivorId,
                stressPermille = stressPermille,
                insomniaDaysRemaining = insomniaDaysRemaining,
                activeTraumaIds = activeTraumaIds ?? new List<string>()
            };
            system.RestoreState(state);
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
