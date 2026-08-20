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

        private static ApprenticeshipSystem Create(out SkillProgressionSystem skills, out DutyRosterSystem roster, out SurvivorRelationsSystem relations)
        {
            skills = new SkillProgressionSystem();
            roster = new DutyRosterSystem();
            relations = new SurvivorRelationsSystem(new SeededRng(42));
            return new ApprenticeshipSystem(new SeededRng(42), skills, roster, relations);
        }
    }
}
