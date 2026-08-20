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
            al.State.blastDoorIntegrity = 50f;
            al.RepairDoor(30f);
            Assert.Equal(80f, al.State.blastDoorIntegrity);
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
    }
}
