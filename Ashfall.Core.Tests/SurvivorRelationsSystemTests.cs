using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SurvivorRelationsSystemTests
    {
        [Fact] public void GetOrCreateRelationship_CreatesNew()
        {
            var sr = Create();
            var rel = sr.GetOrCreateRelationship("anna", "boris");
            Assert.NotNull(rel);
            Assert.Equal(0, rel.affinity);
        }

        [Fact] public void ModifyAffinity_ChangesValue()
        {
            var sr = Create();
            sr.ModifyAffinity("anna", "boris", 20f);
            var rel = sr.GetOrCreateRelationship("anna", "boris");
            Assert.Equal(20f, rel.affinity);
        }

        [Fact] public void ModifyAffinity_Negative_Clamps()
        {
            var sr = Create();
            sr.ModifyAffinity("anna", "boris", -200f);
            var rel = sr.GetOrCreateRelationship("anna", "boris");
            Assert.Equal(-100f, rel.affinity);
        }

        [Fact] public void ApplyGrief_AffectsRelatedDwellers()
        {
            var sr = Create();
            sr.GetOrCreateRelationship("anna", "boris").affinity = 50f;
            sr.GetOrCreateRelationship("anna", "clara").affinity = 30f;
            sr.ApplyGrief("anna", 20f);
            Assert.True(sr.GetOrCreateRelationship("anna", "boris").grief > 0);
            Assert.True(sr.GetOrCreateRelationship("anna", "clara").grief > 0);
        }

        [Fact] public void Mediate_ResolvesConflict()
        {
            var sr = Create();
            sr.GetOrCreateRelationship("anna", "boris").resentment = 60f;
            var conflict = sr.TryTriggerConflict();
            if (conflict != null)
            {
                var r = sr.Mediate(conflict.conflictId, "mediator_1", MediationStyle.Apology);
                Assert.Equal(ActionResult.StatusKind.Success, r.Status);
                Assert.Single(sr.State.mediationHistory);
            }
        }

        [Fact] public void Mediate_UnknownConflict_Fails()
        {
            var sr = Create();
            var r = sr.Mediate("nonexistent", "mediator_1", MediationStyle.Apology);
            Assert.Equal(ActionResult.StatusKind.Failed, r.Status);
        }

        [Fact] public void CaptureRestoreState_PreservesRelationships()
        {
            var sr = Create();
            sr.ModifyAffinity("anna", "boris", 35f);
            var state = sr.CaptureState();
            Assert.Single(state.relationships);

            var sr2 = Create();
            sr2.RestoreState(state);
            Assert.Single(sr2.State.relationships);
            Assert.Equal(35f, sr2.GetOrCreateRelationship("anna", "boris").affinity);
        }

        private static SurvivorRelationsSystem Create() => new SurvivorRelationsSystem(new SeededRng(42));
    }
}
