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

        [Fact] public void BeginTreatment_CaregiverOnDuty_Blocks()
        {
            // Bug-09 regression: a survivor currently holding a duty role must
            // not be assignable as a crisis caregiver — doing so pulls a critical
            // shift worker away.
            var m = Create(out var roster, out _, out _, out _, out _);
            roster.Unlock(0); // unlock expansion so WriteName works
            // Register the candidate caregiver as a real roster resident, then
            // put them on a duty shift.
            string roleKey = DutyRosterSystem.AssignmentRoles[0];
            roster.WriteName("caregiver_busy", displayName: "Caregiver Busy",
                occupationObserved: "medic", script: DutyRosterSystem.ScriptPencil,
                day: 1, sleptHere: true);
            Assert.True(roster.Assign(roleKey, "caregiver_busy"));
            Assert.Equal(roleKey, roster.GetRoleOf("caregiver_busy"));
            m.TriggerCrisis("survivor_1", 70f, CrisisProfile.AcuteStress);
            var caseId = m.State.activeCases[0].caseId;
            var r = m.BeginTreatment(caseId, "caregiver_busy", "counseling");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            // Crisis must remain Active, not InTreatment.
            Assert.Equal(CrisisStatus.Active, m.State.activeCases[0].status);
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

        [Fact] public void ChronicUnhandledSurvivor_PastThreshold_TransitionsToChronic()
        {
            // Bug-05 regression: a crisis left untreated past the chronic threshold
            // must transition to CrisisStatus.Chronic and be archived into
            // resolvedCases (preserving state history instead of silently dropping it).
            var m = Create(out _, out _, out _, out _, out _);
            m.TriggerCrisis("survivor_1", 70f, CrisisProfile.AcuteStress);
            // 20 simulated days without entering treatment (LeaveActive quarantine holds).
            for (int d = 0; d < 20; d++) m.TickDay(d + 1);
            var resolved = m.State.resolvedCases;
            Assert.Single(resolved);
            Assert.Equal(CrisisStatus.Chronic, resolved[0].status);
            Assert.Equal("survivor_1", resolved[0].survivorId);
            // No longer in activeCases.
            Assert.Empty(m.State.activeCases);
            // Ward occupancy is freed.
            Assert.Equal(0, m.State.currentOccupancy);
            // Survivor is no longer 'in crisis' (matches existing IsInCrisis semantics).
            Assert.False(m.IsInCrisis("survivor_1"));
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
