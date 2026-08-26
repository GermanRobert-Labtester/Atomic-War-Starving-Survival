using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class AirlockSecurityIntegrationTests
    {
        [Fact]
        public void AssignSentry_AndCycleDoor_UpdatesState()
        {
            var sys = new AirlockSecuritySystem(new SeededRng(42));
            sys.AssignSentry("dweller_guard");
            Assert.Equal("dweller_guard", sys.State.sentryId);

            var cycle = sys.CycleDoor(AirlockDoorState.Cycling);
            Assert.True(cycle.IsSuccess);
            Assert.Equal(AirlockDoorState.Cycling, sys.State.doorState);
        }

        [Fact]
        public void VisitorTriage_ResolvesIncident()
        {
            var sys = new AirlockSecuritySystem(new SeededRng(42));
            var arr = sys.VisitorArrives("vis_wanderer_01", "refugee");
            Assert.True(arr.IsSuccess);
            Assert.True(sys.HasPendingIncident);

            var res = sys.ResolveIncident(VisitorDecision.Admit);
            Assert.True(res.IsSuccess);
            Assert.False(sys.HasPendingIncident);
            Assert.Equal(1, sys.State.totalAdmissions);
            Assert.Single(sys.State.incidentLog);
        }

        [Fact]
        public void SaveAndRestore_PreservesSecurityState()
        {
            var sys1 = new AirlockSecuritySystem(new SeededRng(42));
            sys1.AssignSentry("dweller_guard");
            sys1.VisitorArrives("vis_merchant", "trader");
            sys1.ResolveIncident(VisitorDecision.Quarantine);

            var state = sys1.CaptureState();
            var sys2 = new AirlockSecuritySystem(new SeededRng(42));
            sys2.RestoreState(state);

            Assert.Equal("dweller_guard", sys2.State.sentryId);
            Assert.Single(sys2.State.incidentLog);
            Assert.Equal(VisitorDecision.Quarantine, sys2.State.incidentLog[0].decision);
        }
    }
}
