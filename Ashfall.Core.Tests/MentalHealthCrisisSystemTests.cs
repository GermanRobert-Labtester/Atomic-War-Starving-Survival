using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class MentalHealthCrisisSystemTests
    {
        [Fact] public void TriggerCrisis_CreatesCase()
        {
            var m = Create(out _, out _, out _, out _, out _);
            var r = m.TriggerCrisis("survivor_1", 70f, CrisisProfile.AcuteStress);
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(m.State.activeCases);
            Assert.Equal("survivor_1", m.State.activeCases[0].survivorId);
        }

        [Fact] public void TriggerCrisis_DuplicateSurvivor_Blocked()
        {
            var m = Create(out _, out _, out _, out _, out _);
            m.TriggerCrisis("survivor_1", 70f, CrisisProfile.AcuteStress);
            var r = m.TriggerCrisis("survivor_1", 50f, CrisisProfile.GuiltInsomnia);
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void BeginTreatment_UpdatesStatus()
        {
            var m = Create(out _, out _, out _, out _, out _);
            m.TriggerCrisis("survivor_1", 70f, CrisisProfile.AcuteStress);
            var caseId = m.State.activeCases[0].caseId;
            var r = m.BeginTreatment(caseId, "caregiver_1", "counseling");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal(CrisisStatus.InTreatment, m.State.activeCases[0].status);
        }

        [Fact] public void TickDay_RecoversMildCrisis()
        {
            var m = Create(out _, out var needs, out _, out _, out _);
            m.TriggerCrisis("survivor_1", 20f, CrisisProfile.AcuteStress);
            var caseId = m.State.activeCases[0].caseId;
            m.BeginTreatment(caseId, "caregiver_1", "counseling");
            // Mild crisis recovers in ~7 days at 15/day
            for (int i = 0; i < 10; i++) m.TickDay(i + 1);
            Assert.Empty(m.State.activeCases);
            Assert.Contains("survivor_1", m.State.resolvedCases.Select(c => c.survivorId));
        }

        [Fact] public void IsInCrisis_ReturnsTrue()
        {
            var m = Create(out _, out _, out _, out _, out _);
            m.TriggerCrisis("survivor_1", 70f, CrisisProfile.AcuteStress);
            Assert.True(m.IsInCrisis("survivor_1"));
        }

        [Fact] public void IsEligibleForWork_Recovered_ReturnsTrue()
        {
            var m = Create(out _, out var needs, out _, out _, out _);
            m.TriggerCrisis("survivor_1", 20f, CrisisProfile.AcuteStress);
            var caseId = m.State.activeCases[0].caseId;
            m.BeginTreatment(caseId, "caregiver_1", "counseling");
            for (int i = 0; i < 10; i++) m.TickDay(i + 1);
            Assert.True(m.IsEligibleForWork("survivor_1"));
        }

        [Fact] public void CaptureRestoreState_PreservesCases()
        {
            var m = Create(out _, out _, out _, out _, out _);
            m.TriggerCrisis("survivor_1", 70f, CrisisProfile.AcuteStress);
            var state = m.CaptureState();
            Assert.Single(state.activeCases);

            var m2 = Create(out _, out _, out _, out _, out _);
            m2.RestoreState(state);
            Assert.Single(m2.State.activeCases);
        }

        private static MentalHealthCrisisSystem Create(out DutyRosterSystem roster, out NeedsSystem needs, out MedicalWardSystem medical, out ChemicalDependencySystem dependency, out SurvivorRelationsSystem relations)
        {
            roster = new DutyRosterSystem();
            needs = new NeedsSystem();
            var wardState = new MedicalWardState();
            var bed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.Psychiatric);
            var proc = new MedicalProcedureDef("proc_1", "Procedure 1", "MedicalSystem");
            medical = new MedicalWardSystem(wardState, new[] { bed }, new[] { proc });
            dependency = new ChemicalDependencySystem();
            relations = new SurvivorRelationsSystem(new SeededRng(42));
            return new MentalHealthCrisisSystem(new SeededRng(42), needs, medical, dependency, roster);
        }
    }
}
