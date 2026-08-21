using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ApprenticeshipSystemTests
    {
        [Fact] public void StartPair_UnqualifiedMentor_Blocks()
        {
            var a = Create(out _, out _, out _);
            var r = a.StartPair("mentor_1", "apprentice_1", "skill_medicine");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
        }

        [Fact] public void StartPair_QualifiedMentor_StartsPair()
        {
            var a = Create(out var skills, out _, out _);
            skills.RecordAction(new SimpleSkillActor("mentor_1"), "skill_medicine", 50f, 1);
            var r = a.StartPair("mentor_1", "apprentice_1", "skill_medicine");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Single(a.State.activePairs);
        }

        [Fact] public void TickDay_CompletesApprenticeship()
        {
            var a = Create(out var skills, out _, out _);
            skills.RecordAction(new SimpleSkillActor("mentor_1"), "skill_medicine", 50f, 1);
            a.StartPair("mentor_1", "apprentice_1", "skill_medicine", targetXp: 30f);
            a.TickDay(1);
            a.TickDay(2);
            a.TickDay(3);
            Assert.True(a.State.activePairs[0].isComplete);
            Assert.Contains("skill_medicine", a.State.completedSkillIds);
        }

        [Fact] public void CancelPair_RemovesPair()
        {
            var a = Create(out var skills, out _, out _);
            skills.RecordAction(new SimpleSkillActor("mentor_1"), "skill_medicine", 50f, 1);
            a.StartPair("mentor_1", "apprentice_1", "skill_medicine");
            a.CancelPair(a.State.activePairs[0].pairId);
            Assert.Empty(a.State.activePairs);
        }

        [Fact] public void TickDay_AdvancesProgress()
        {
            var a = Create(out var skills, out _, out _);
            skills.RecordAction(new SimpleSkillActor("mentor_1"), "skill_medicine", 50f, 1);
            a.StartPair("mentor_1", "apprentice_1", "skill_medicine");
            a.TickDay(1);
            Assert.True(a.State.activePairs[0].progressXp > 0f);
        }

        [Fact] public void CaptureRestoreState_PreservesPairs()
        {
            var a = Create(out var skills, out _, out _);
            skills.RecordAction(new SimpleSkillActor("mentor_1"), "skill_medicine", 50f, 1);
            a.StartPair("mentor_1", "apprentice_1", "skill_medicine");
            var state = a.CaptureState();
            Assert.Single(state.activePairs);

            var a2 = Create(out _, out _, out _);
            a2.RestoreState(state);
            Assert.Single(a2.State.activePairs);
        }

        [Fact] public void StartPair_MentorOnDuty_Blocks()
        {
            // BUG-14 regression: StartPair previously called
            // `_roster.GetAssignment(mentorId)` — GetAssignment takes a ROLE,
            // not a survivorId. Setting a mentor on a duty shift never
            // triggered the mentor_busy block. Fix calls `_roster.GetRoleOf`
            // (survivorId-based) instead. This test pins the corrected API.
            var skills = new SkillProgressionSystem();
            skills.RecordAction(new SimpleSkillActor("mentor_busy"), "skill_medicine", 60f, 1);
            var roster = new DutyRosterSystem();
            roster.Unlock(0);
            roster.WriteName("mentor_busy", displayName: "Mentor Busy",
                occupationObserved: "medic", script: DutyRosterSystem.ScriptPencil,
                day: 1, sleptHere: true);
            Assert.True(roster.Assign(DutyRosterSystem.AssignmentRoles[0], "mentor_busy"));
            var a = new ApprenticeshipSystem(new SeededRng(42), skills, roster,
                new SurvivorRelationsSystem(new SeededRng(42)));
            var r = a.StartPair("mentor_busy", "apprentice_1", "skill_medicine");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal("mentor_busy", r.FailureCode);
        }

        [Fact] public void StartPair_ApprenticeOnDuty_Blocks()
        {
            // BUG-14 regression: same shape for apprentice.
            var skills = new SkillProgressionSystem();
            skills.RecordAction(new SimpleSkillActor("mentor_quals"), "skill_medicine", 60f, 1);
            var roster = new DutyRosterSystem();
            roster.Unlock(0);
            roster.WriteName("apprentice_busy", displayName: "Apprentice Busy",
                occupationObserved: "labourer", script: DutyRosterSystem.ScriptPencil,
                day: 1, sleptHere: true);
            Assert.True(roster.Assign(DutyRosterSystem.AssignmentRoles[0], "apprentice_busy"));
            var a = new ApprenticeshipSystem(new SeededRng(42), skills, roster,
                new SurvivorRelationsSystem(new SeededRng(42)));
            var r = a.StartPair("mentor_quals", "apprentice_busy", "skill_medicine");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal("apprentice_busy", r.FailureCode);
        }

        private static ApprenticeshipSystem Create(out SkillProgressionSystem skills, out DutyRosterSystem roster, out SurvivorRelationsSystem relations)
        {
            skills = new SkillProgressionSystem();
            roster = new DutyRosterSystem();
            relations = new SurvivorRelationsSystem(new SeededRng(42));
            return new ApprenticeshipSystem(new SeededRng(42), skills, roster, relations);
        }
    }
}
