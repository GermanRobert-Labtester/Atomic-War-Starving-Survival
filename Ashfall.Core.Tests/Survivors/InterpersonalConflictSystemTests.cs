// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class InterpersonalConflictSystemTests
    {
        [Fact]
        public void InitiateConflict_CreatesActiveConflictWithInitialEscalation()
        {
            var system = new InterpersonalConflictSystem();
            InterpersonalConflict? triggered = null;
            system.OnConflictInitiated += c => triggered = c;

            var conflict = system.InitiateConflict(
                initiatorId: "dweller_a",
                targetId: "dweller_b",
                type: ConflictType.ResourceDispute,
                triggerDescription: "Dispute over extra water ration",
                severity: ConflictSeverity.Moderate,
                currentDay: 3
            );

            Assert.NotNull(conflict);
            Assert.Equal("dweller_a", conflict.InitiatorId);
            Assert.Equal("dweller_b", conflict.TargetId);
            Assert.Equal(35f, conflict.EscalationScore);
            Assert.False(conflict.IsResolved);
            Assert.Equal(1, system.ActiveConflictCount);
            Assert.Equal(triggered, conflict);
        }

        [Fact]
        public void EscalateConflict_IncreasesScoreAndEmitsEvent()
        {
            var system = new InterpersonalConflictSystem();
            var conflict = system.InitiateConflict("dweller_c", "dweller_d", ConflictType.Argument, "Loud noise during sleep", ConflictSeverity.Mild);

            bool riskEmitted = false;
            system.OnPhysicalFightRisk += c => riskEmitted = true;

            float newScore = system.EscalateConflict(conflict.ConflictId, 70f, currentDay: 4);

            Assert.True(newScore >= 80f);
            Assert.Equal(ConflictSeverity.Crisis, conflict.Severity);
            Assert.True(riskEmitted);
        }

        [Fact]
        public void MediateConflict_ResolvesConflictAndClearsGrievances()
        {
            var system = new InterpersonalConflictSystem();
            var conflict = system.InitiateConflict("worker_1", "worker_2", ConflictType.ShiftConflict, "Workload dispute");
            system.AddGrievance("worker_1", "worker_2", "Left post early", intensity: 40f);

            bool mediated = system.MediateConflict(conflict.ConflictId, mediatorId: "leader_1", currentDay: 5);

            Assert.True(mediated);
            Assert.True(conflict.IsResolved);
            Assert.Equal(ConflictResolutionMethod.Mediation, conflict.ResolutionMethod);
            Assert.Equal(0, system.ActiveConflictCount);
            Assert.Empty(system.GetGrievancesForSurvivor("worker_1"));
        }

        [Fact]
        public void TickDay_DecaysUnbackedConflicts_AndHealsGrievances()
        {
            var system = new InterpersonalConflictSystem();
            var conflict = system.InitiateConflict("survivor_1", "survivor_2", ConflictType.Argument, "Minor disagreement", ConflictSeverity.Mild);
            conflict.EscalationScore = 5f;

            // Tick 2 days without grievances: conflict should cool down and resolve
            system.TickDay(1);
            system.TickDay(2);

            Assert.True(conflict.IsResolved);
            Assert.Equal(ConflictResolutionMethod.NaturalDecay, conflict.ResolutionMethod);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new InterpersonalConflictSystem();
            var c = system1.InitiateConflict("surv_x", "surv_y", ConflictType.PersonalSlight, "Insult in canteen");
            system1.AddGrievance("surv_x", "surv_y", "Public embarrassment", 50f);

            var state = system1.CaptureState();
            Assert.Single(state.Conflicts);
            Assert.Single(state.Grievances);

            var system2 = new InterpersonalConflictSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.ActiveConflictCount);
            Assert.Equal(1, system2.TotalGrievancesCount);
            var restored = system2.GetActiveConflicts().First();
            Assert.Equal("surv_x", restored.InitiatorId);
            Assert.Equal("surv_y", restored.TargetId);
        }

        [Fact]
        public void ProjectCanonicalRelations_ProducesTypedReadModelWithoutMutation()
        {
            var state = new SurvivorRelationsState();
            state.relationships.Add(new RelationshipEntry
            {
                dwellerA = "surv_b",
                dwellerB = "surv_a",
                affinity = -40f,
                resentment = 65f
            });
            state.activeConflicts.Add(new ConflictEntry
            {
                conflictId = "conflict_8_surv_a_surv_b",
                dwellerA = "surv_a",
                dwellerB = "surv_b",
                cause = "ration dispute",
                dayStarted = 8,
                isResolved = false
            });

            var conflicts = InterpersonalConflictSystem.ProjectCanonicalRelations(state, currentDay: 12);
            var grievances = InterpersonalConflictSystem.ProjectCanonicalGrievances(state, currentDay: 12);

            Assert.Single(conflicts);
            Assert.Equal("relations:conflict_8_surv_a_surv_b", conflicts[0].ConflictId);
            Assert.Equal(ConflictSeverity.Severe, conflicts[0].Severity);
            Assert.Equal(65f, conflicts[0].EscalationScore);
            Assert.Equal(8, conflicts[0].StartedDay);
            Assert.Single(grievances);
            Assert.Equal("relations:surv_a|surv_b", grievances[0].GrievanceId);
            Assert.Equal("surv_a", grievances[0].HolderId);
            Assert.Equal(65f, grievances[0].Intensity);
            Assert.Single(state.activeConflicts);
            Assert.Single(state.relationships);
            Assert.False(state.activeConflicts[0].isResolved);
        }

        [Fact]
        public void ResolutionMoraleOutcome_IsTypedAndDeterministic()
        {
            Assert.Equal(2f, InterpersonalConflictSystem.MoraleDeltaFor(MediationStyle.Apology));
            Assert.Equal(3f, InterpersonalConflictSystem.MoraleDeltaFor(MediationStyle.ResourceSettlement));
            Assert.Equal(-1f, InterpersonalConflictSystem.MoraleDeltaFor(MediationStyle.Discipline));
            Assert.Equal(-3f, InterpersonalConflictSystem.MoraleDeltaFor(MediationStyle.Refusal));
        }
    }
}
