using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SurvivorRelationsIntegrationTests
    {
        [Fact]
        public void ModifyAffinityAndTrust_UpdatesState()
        {
            var sys = new SurvivorRelationsSystem(new SeededRng(42));
            sys.ModifyAffinity("dweller_1", "dweller_2", 15f);
            sys.ModifyTrust("dweller_1", "dweller_2", 10f);

            Assert.Single(sys.State.relationships);
            var r = sys.State.relationships[0];
            Assert.Equal(15f, r.affinity);
            Assert.Equal(10f, r.trust);
        }

        [Fact]
        public void MediateConflict_ResolvesDispute()
        {
            var sys = new SurvivorRelationsSystem(new SeededRng(42));
            sys.ModifyAffinity("dweller_1", "dweller_2", -50f);
            var conflict = sys.TryTriggerConflict();

            if (conflict != null)
            {
                var med = sys.Mediate(conflict.conflictId, "dweller_mediator", MediationStyle.Apology);
                Assert.True(med.IsSuccess);
                Assert.True(conflict.isResolved);
                Assert.Single(sys.State.mediationHistory);
            }
        }

        [Fact]
        public void SaveAndRestore_PreservesAllRelationsAndGrief()
        {
            var sys1 = new SurvivorRelationsSystem(new SeededRng(42));
            sys1.ModifyAffinity("dweller_1", "dweller_2", 20f);
            sys1.ApplyGrief("dweller_1", 30f);

            var state = sys1.CaptureState();
            var sys2 = new SurvivorRelationsSystem(new SeededRng(42));
            sys2.RestoreState(state);

            Assert.Single(sys2.State.relationships);
            Assert.Equal(30f, sys2.State.relationships[0].grief);
        }
    }
}
