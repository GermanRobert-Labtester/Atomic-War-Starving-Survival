using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ApprenticeshipIntegrationTests
    {
        private static ApprenticeshipSystem Create(out SkillProgressionSystem skills, out DutyRosterSystem roster, out SurvivorRelationsSystem relations)
        {
            skills = new SkillProgressionSystem();
            roster = new DutyRosterSystem();
            relations = new SurvivorRelationsSystem(new SeededRng(42));
            return new ApprenticeshipSystem(new SeededRng(42), skills, roster, relations);
        }

        [Fact]
        public void StartPair_AndTick_AdvancesProgress()
        {
            var sys = Create(out var skills, out _, out _);
            skills.RecordAction(new SimpleSkillActor("mentor_smith"), "skill_foundry_casting", 50f, 1);
            var start = sys.StartPair("mentor_smith", "apprentice_dweller", "skill_foundry_casting", 50f);
            Assert.True(start.IsSuccess);
            Assert.Single(sys.State.activePairs);

            sys.TickDay(1);
            Assert.True(sys.State.activePairs[0].progressXp > 0f);
        }

        [Fact]
        public void CancelPair_RemovesPair()
        {
            var sys = Create(out var skills, out _, out _);
            skills.RecordAction(new SimpleSkillActor("mentor_smith"), "skill_foundry_casting", 50f, 1);
            sys.StartPair("mentor_smith", "apprentice_dweller", "skill_foundry_casting", 50f);
            var pairId = sys.State.activePairs[0].pairId;
            var cancel = sys.CancelPair(pairId);

            Assert.True(cancel.IsSuccess);
            Assert.Empty(sys.State.activePairs);
        }

        [Fact]
        public void SaveAndRestore_PreservesApprenticeshipState()
        {
            var sys1 = Create(out var skills, out _, out _);
            skills.RecordAction(new SimpleSkillActor("mentor_1"), "skill_chemistry", 50f, 1);
            sys1.StartPair("mentor_1", "apprentice_1", "skill_chemistry", 100f);
            sys1.TickDay(1);

            var state = sys1.CaptureState();
            var sys2 = Create(out _, out _, out _);
            sys2.RestoreState(state);

            Assert.Single(sys2.State.activePairs);
            Assert.Equal("mentor_1", sys2.State.activePairs[0].mentorId);
            Assert.Equal(sys1.State.activePairs[0].progressXp, sys2.State.activePairs[0].progressXp);
        }
    }
}
