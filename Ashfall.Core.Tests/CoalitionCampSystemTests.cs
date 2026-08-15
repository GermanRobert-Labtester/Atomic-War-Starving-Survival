using Ashfall.Core;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CoalitionCampSystemTests
    {
        [Fact]
        public void Camp_FormsOnceAtTheMuster()
        {
            var camp = new CoalitionCampSystem();
            Assert.False(camp.Form(259));
            Assert.True(camp.Form(260));
            Assert.Equal(260, camp.State.formedDay);
            Assert.Equal(CoalitionCampSystem.BaseMembers, camp.MembersRallied);
            Assert.True(camp.VaskWithCamp);
            Assert.False(camp.Form(261));
        }

        [Fact]
        public void Rally_IncrementsMembers()
        {
            var camp = new CoalitionCampSystem();
            Assert.False(camp.RallyDeserter());
            camp.Form(260);
            camp.RallyDeserter();
            camp.RallyDeserter();
            Assert.Equal(CoalitionCampSystem.BaseMembers + 2, camp.MembersRallied);
        }

        [Fact]
        public void StrategyB_RaisesLockoutRisk()
        {
            var camp = new CoalitionCampSystem();
            camp.Form(260);
            Assert.True(camp.SetStrategy(QuestApproach.B));
            Assert.Equal("B", camp.ChosenStrategy);
            Assert.Equal(15, camp.GarrisonLockoutRisk);
        }

        [Fact]
        public void StrategyD_ZeroesEverything()
        {
            var camp = new CoalitionCampSystem();
            camp.Form(260);
            camp.RallyDeserter();
            Assert.True(camp.SetStrategy(QuestApproach.D));
            Assert.Equal(0, camp.GarrisonLockoutRisk);
            Assert.Equal(0, camp.MembersRallied);
            Assert.False(camp.VaskWithCamp);
        }

        [Fact]
        public void Strategy_LockedOnceChosen()
        {
            var camp = new CoalitionCampSystem();
            camp.Form(260);
            Assert.True(camp.SetStrategy(QuestApproach.C));
            Assert.Equal(CoalitionCampSystem.BaseMembers - 3, camp.MembersRallied);
            Assert.False(camp.SetStrategy(QuestApproach.A));
            Assert.Equal("C", camp.ChosenStrategy);
        }

        [Fact]
        public void Strategy_RequiresFormedCamp()
        {
            var camp = new CoalitionCampSystem();
            Assert.False(camp.SetStrategy(QuestApproach.A));
            Assert.Equal(string.Empty, camp.ChosenStrategy);
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var camp = new CoalitionCampSystem();
            camp.Form(300);
            camp.SetStrategy(QuestApproach.A);
            var snapshot = camp.CaptureState();
            snapshot.membersRallied = 999;
            snapshot.chosenStrategy = "injected";
            Assert.Equal(CoalitionCampSystem.BaseMembers, camp.MembersRallied);
            Assert.Equal("A", camp.ChosenStrategy);
        }

        [Fact]
        public void SaveLoad_RoundTripsAllState()
        {
            var camp = new CoalitionCampSystem();
            camp.Form(261);
            camp.RallyDeserter();
            camp.SetStrategy(QuestApproach.B);

            var restored = new CoalitionCampSystem();
            restored.RestoreState(camp.CaptureState());

            Assert.True(restored.Formed);
            Assert.Equal(261, restored.State.formedDay);
            Assert.Equal(CoalitionCampSystem.BaseMembers + 1, restored.MembersRallied);
            Assert.Equal("B", restored.ChosenStrategy);
            Assert.Equal(15, restored.GarrisonLockoutRisk);
        }

        [Fact]
        public void SaveLoad_ChecksumStable()
        {
            var camp = new CoalitionCampSystem();
            camp.Form(262);
            camp.RallyDeserter();
            camp.SetStrategy(QuestApproach.C);
            string before = SaveChecksum.Compute(camp.CaptureState());

            var restored = new CoalitionCampSystem();
            restored.RestoreState(camp.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());

            Assert.Equal(before, after);
        }

        [Fact]
        public void RestoreState_ClampsOutOfRange()
        {
            var corrupted = new CoalitionCampState
            {
                formed = true,
                membersRallied = -5,
                garrisonLockoutRisk = 250,
                chosenStrategy = "Q"
            };
            var camp = new CoalitionCampSystem();
            camp.RestoreState(corrupted);
            Assert.Equal(0, camp.MembersRallied);
            Assert.Equal(100, camp.GarrisonLockoutRisk);
            Assert.Equal("Q", camp.ChosenStrategy);
        }
    }
}
