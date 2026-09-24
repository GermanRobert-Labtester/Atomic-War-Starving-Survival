// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class AirlockSecuritySystemTests
    {
        [Fact] public void CycleDoor_UpdatesState()
        {
            var al = Create();
            al.CycleDoor(AirlockDoorState.Open);
            Assert.Equal(AirlockDoorState.Open, al.State.doorState);
        }

        [Fact] public void VisitorArrives_CreatesIncident()
        {
            var al = Create();
            al.VisitorArrives("traveller_1", "merchant");
            Assert.True(al.HasPendingIncident);
        }

        [Fact] public void ResolveIncident_Admit_UpdatesLog()
        {
            var al = Create();
            al.VisitorArrives("traveller_1", "merchant");
            var r = al.ResolveIncident(VisitorDecision.Admit);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.False(al.HasPendingIncident);
            Assert.Single(al.State.incidentLog);
            Assert.Equal(1, al.State.totalAdmissions);
        }

        [Fact] public void ResolveIncident_TurnAway_UpdatesCount()
        {
            var al = Create();
            al.VisitorArrives("traveller_1", "merchant");
            al.ResolveIncident(VisitorDecision.TurnAway);
            Assert.Equal(1, al.State.totalTurnaways);
        }

        [Fact] public void ResolveIncident_Defend_DamagesDoor()
        {
            var al = Create();
            al.VisitorArrives("raider_1", "raider");
            float before = al.State.blastDoorIntegrity;
            al.ResolveIncident(VisitorDecision.Defend);
            Assert.True(al.State.blastDoorIntegrity < before);
        }

        [Fact] public void ResolveIncident_WithoutVisitor_Blocks()
        {
            var al = Create();
            var r = al.ResolveIncident(VisitorDecision.Admit);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void RepairDoor_RestoresIntegrity()
        {
            var al = Create();
            SetIntegrity(al, 50f);
            al.RepairDoor(30f);
            Assert.Equal(80f, al.State.blastDoorIntegrity);
        }

        [Fact]
        public void RepairDoor_InvalidAmounts_BlockWithoutMutation()
        {
            var al = Create();
            SetIntegrity(al, 50f);

            foreach (var amount in new[] { -1f, float.NaN, float.PositiveInfinity })
            {
                var result = al.RepairDoor(amount);
                Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
                Assert.Equal("invalid_amount", result.FailureCode);
            }

            Assert.Equal(50f, al.State.blastDoorIntegrity);
        }

        [Fact]
        public void TickDay_DuplicateBackwardAndNegativeDays_AreSafe()
        {
            var al = Create();
            var state = al.CaptureState();
            state.alertness = 0f;
            al.RestoreState(state);
            al.AssignSentry("guard");
            int changeNotifications = 0;
            al.OnSecurityChanged += () => changeNotifications++;

            al.TickDay(10);
            float afterFirstTick = al.State.alertness;
            al.TickDay(10);
            al.TickDay(9);

            Assert.Equal(15f, afterFirstTick);
            Assert.Equal(afterFirstTick, al.State.alertness);
            Assert.Equal(10, al.CaptureState().lastSecurityDay);
            Assert.Equal(1, changeNotifications);
            Assert.Throws<ArgumentOutOfRangeException>(() => al.TickDay(-1));
        }

        [Fact]
        public void Restore_MalformedState_NormalizesAndFilters()
        {
            var al = Create();
            var saved = new AirlockSecurityState
            {
                systemId = "   ",
                blastDoorIntegrity = float.PositiveInfinity,
                doorState = (AirlockDoorState)999,
                sentryId = " sentry_7 ",
                alertness = float.NaN,
                visitorId = string.Empty,
                hasActiveIncident = true,
                totalAdmissions = -5,
                totalTurnaways = -8
            };
            saved.incidentLog.Add(null!);
            saved.incidentLog.Add(new AirlockIncidentLog
            {
                day = -1,
                visitorId = " invalid ",
                decision = VisitorDecision.None,
                outcome = " invalid "
            });
            saved.incidentLog.Add(new AirlockIncidentLog
            {
                day = 4,
                visitorId = " visitor_4 ",
                decision = VisitorDecision.TurnAway,
                outcome = " turned away "
            });

            al.RestoreState(saved);
            var state = al.CaptureState();

            Assert.Equal(AirlockSecuritySystem.SystemId, state.systemId);
            Assert.Equal(0f, state.blastDoorIntegrity);
            Assert.Equal(0f, state.alertness);
            Assert.Equal(AirlockDoorState.Secure, state.doorState);
            Assert.Equal("sentry_7", state.sentryId);
            Assert.False(state.hasActiveIncident);
            Assert.Equal(0, state.totalAdmissions);
            Assert.Equal(0, state.totalTurnaways);
            Assert.Single(state.incidentLog);
            Assert.Equal("visitor_4", state.incidentLog[0].visitorId);
        }

        [Fact]
        public void StateAndIncidentEvents_AreDetachedSnapshots()
        {
            var al = Create();
            AirlockIncidentLog? resolved = null;
            al.OnIncidentResolved += entry => resolved = entry;
            al.TickDay(9);
            al.VisitorArrives(" visitor_9 ", " trader ");
            al.ResolveIncident(VisitorDecision.Admit);

            al.State.totalAdmissions = 99;
            al.State.incidentLog.Clear();
            resolved!.visitorId = "tampered";

            var state = al.CaptureState();
            Assert.Equal(1, state.totalAdmissions);
            Assert.Single(state.incidentLog);
            Assert.Equal("visitor_9", state.incidentLog[0].visitorId);
            Assert.Equal(9, state.incidentLog[0].day);
        }

        [Fact]
        public void InvalidTransitions_DoNotCloseOrOverwriteIncidents()
        {
            var al = Create();
            var blankVisitor = al.VisitorArrives("   ", "merchant");
            Assert.Equal(ActionResult.StatusKind.Blocked, blankVisitor.Status);
            Assert.Equal("invalid_visitor", blankVisitor.FailureCode);
            Assert.False(al.HasPendingIncident);

            al.VisitorArrives("visitor_1", "merchant");

            var duplicateArrival = al.VisitorArrives("visitor_2", "raider");
            Assert.Equal(ActionResult.StatusKind.Blocked, duplicateArrival.Status);
            Assert.Equal("incident_active", duplicateArrival.FailureCode);
            Assert.Equal("visitor_1", al.State.visitorId);

            foreach (var decision in new[] { VisitorDecision.None, (VisitorDecision)999 })
            {
                var result = al.ResolveIncident(decision);
                Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
                Assert.Equal("invalid_decision", result.FailureCode);
                Assert.True(al.HasPendingIncident);
                Assert.Empty(al.State.incidentLog);
            }

            var invalidDoor = al.CycleDoor((AirlockDoorState)999);
            Assert.Equal(ActionResult.StatusKind.Blocked, invalidDoor.Status);
            Assert.Equal("invalid_door_state", invalidDoor.FailureCode);
        }

        [Fact]
        public void Restore_PreservesCurrentDayForSubsequentIncidentLogs()
        {
            var source = Create();
            source.TickDay(12);
            source.VisitorArrives("visitor_12", "merchant");

            var restored = Create();
            restored.RestoreState(source.CaptureState());
            restored.ResolveIncident(VisitorDecision.TurnAway);

            Assert.Equal(12, restored.State.incidentLog[0].day);
        }

        [Fact]
        public void CountersSaturate_AndIncidentHistoryIsBounded()
        {
            var al = Create();
            var state = al.CaptureState();
            state.totalAdmissions = int.MaxValue;
            state.totalTurnaways = int.MaxValue;
            al.RestoreState(state);

            for (int i = 0; i < 261; i++)
            {
                al.VisitorArrives($"visitor_{i}", "merchant");
                al.ResolveIncident(i % 2 == 0 ? VisitorDecision.Admit : VisitorDecision.TurnAway);
            }

            Assert.Equal(int.MaxValue, al.State.totalAdmissions);
            Assert.Equal(int.MaxValue, al.State.totalTurnaways);
            Assert.Equal(256, al.State.incidentLog.Count);
        }

        [Fact]
        public void LegacySaveWithoutDayCursor_RestoresSentinel()
        {
            const string legacy = "{\"systemId\":\"airlock_security\",\"blastDoorIntegrity\":100,\"doorState\":0,\"alertness\":100}";
            var saved = new SystemTextJsonSerializer().Deserialize<AirlockSecurityState>(legacy);

            Assert.NotNull(saved);
            Assert.Equal(-1, saved!.lastSecurityDay);

            var system = Create();
            system.RestoreState(saved);
            Assert.Equal(-1, system.CaptureState().lastSecurityDay);
        }

        [Fact] public void CaptureRestoreState_PreservesIncidents()
        {
            var al = Create();
            al.VisitorArrives("traveller_1", "merchant");
            al.ResolveIncident(VisitorDecision.Admit);
            var state = al.CaptureState();
            Assert.Single(state.incidentLog);

            var al2 = Create();
            al2.RestoreState(state);
            Assert.Single(al2.State.incidentLog);
        }

        private static AirlockSecuritySystem Create() => new AirlockSecuritySystem(new SeededRng(42));

        private static void SetIntegrity(AirlockSecuritySystem system, float value)
        {
            var state = system.CaptureState();
            state.blastDoorIntegrity = value;
            system.RestoreState(state);
        }
    }
}
