using System;
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
            string roleKey = DutyRosterIds.AssignmentRoles[0];
            roster.WriteName("caregiver_busy", displayName: "Caregiver Busy",
                occupationObserved: "medic", script: DutyRosterIds.ScriptPencil,
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

        // Batch 4 Bug-14 follow-up regression: a *single* shared
        // DutyRosterSystem passed to both MentalHealthCrisisSystem and
        // ApprenticeshipSystem must surface duty-role assignments to BOTH
        // consumers. Previously, Main.ExpandedShelterSystems.cs created
        // a new DutyRosterSystem per system; cross-system busy checks
        // never observed another system's assignments. Here we hold one
        // roster instance alive, then assert both systems reject it.
        [Fact] public void SharedRoster_BothSystems_BlockSurvivorOnDuty()
        {
            var sharedRoster = new DutyRosterSystem();
            var needs = new NeedsSystem();
            var wardState = new MedicalWardState();
            var bed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.Psychiatric);
            var proc = new MedicalProcedureDef("proc_1", "P1", "M");
            var medical = new MedicalWardSystem(wardState, new[] { bed }, new[] { proc });
            var dependency = new ChemicalDependencySystem();
            var relations = new SurvivorRelationsSystem(new SeededRng(42));

            var mh = new MentalHealthCrisisSystem(new SeededRng(42),
                needs, medical, dependency, sharedRoster, null!);
            var app = new ApprenticeshipSystem(new SeededRng(42),
                new SkillProgressionSystem(), sharedRoster, relations, null!);

            // Single shared roster, single survivor on a duty.
            sharedRoster.Unlock(0);
            string roleKey = DutyRosterIds.AssignmentRoles[0];
            sharedRoster.WriteName("shared_survivor", "Shared", "medic",
                DutyRosterIds.ScriptPencil, 1, true);
            Assert.True(sharedRoster.Assign(roleKey, "shared_survivor"));

            // Mental Health: BeginTreatment must reject this survivor as
            // caregiver (Bug-09 path).
            mh.TriggerCrisis("patient_1", 70f, CrisisProfile.AcuteStress);
            string caseId = mh.State.activeCases[0].caseId;
            var mhResult = mh.BeginTreatment(caseId, "shared_survivor", "counseling");
            Assert.Equal(ActionResult.StatusKind.Blocked, mhResult.Status);
            Assert.Equal("caregiver_busy", mhResult.FailureCode);
            Assert.Equal(CrisisStatus.Active, mh.State.activeCases[0].status);

            // Apprenticeship: StartPair must reject this survivor as mentor
            // OR apprentice (Bug-14 Core-Side path), proving the same roster
            // instance is observed by both systems.
            var appResult = app.StartPair("mentor_x", "shared_survivor",
                targetSkillId: "medicine", targetXp: 100f);
            Assert.Equal(ActionResult.StatusKind.Blocked, appResult.Status);
            Assert.Equal("apprentice_busy", appResult.FailureCode);
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
