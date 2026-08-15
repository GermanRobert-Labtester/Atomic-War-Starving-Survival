using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FinalWishSystemTests
    {
        private static FinalWishSystem CreateSystem(int? seed = 42)
        {
            var sys = new FinalWishSystem();
            sys.Rng = seed.HasValue ? new SeededRng(seed.Value) : null;
            return sys;
        }

        [Fact]
        public void DeclareTerminalPrognosis_ActivatesWish()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            Assert.True(sys.HasTerminalPrognosis("sv_1"));
            Assert.True(sys.HasActiveWish("sv_1"));
            Assert.True(sys.GetDaysRemaining("sv_1") > 0f);
        }

        [Fact]
        public void DeclareTerminalPrognosis_RejectsDeadSurvivor()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: false);
            Assert.False(sys.HasTerminalPrognosis("sv_1"));
            Assert.False(sys.HasActiveWish("sv_1"));
        }

        [Fact]
        public void DeclareTerminalPrognosis_RejectsDuplicate()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            float firstDays = sys.GetDaysRemaining("sv_1");
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            Assert.Equal(firstDays, sys.GetDaysRemaining("sv_1"));
        }

        [Fact]
        public void DeclareTerminalPrognosis_FiresEvent()
        {
            var sys = CreateSystem();
            string firedId = null;
            string firedWish = null;
            float firedDays = 0f;
            sys.OnTerminalPrognosisDeclared += (id, wish, days) =>
            {
                firedId = id;
                firedWish = wish;
                firedDays = days;
            };
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            Assert.Equal("sv_1", firedId);
            Assert.NotEmpty(firedWish);
            Assert.True(firedDays > 0f);
        }

        [Fact]
        public void AdvanceWishStep_CompletesDeliverLetter_InTwoSteps()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            // Default wish is deliver_letter (2 steps)
            Assert.False(sys.AdvanceWishStep("sv_1", "step_1"));
            Assert.True(sys.AdvanceWishStep("sv_1", "step_2"));
            Assert.True(sys.HasCompletedWish("sv_1"));
            Assert.False(sys.HasActiveWish("sv_1"));
        }

        [Fact]
        public void AdvanceWishStep_BuildMemorial_RequiresThreeSteps()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "soldier_archetype", isAlive: true);
            // soldier → build_memorial (3 steps)
            Assert.False(sys.AdvanceWishStep("sv_1", "step_1"));
            Assert.False(sys.AdvanceWishStep("sv_1", "step_2"));
            Assert.True(sys.AdvanceWishStep("sv_1", "step_3"));
            Assert.True(sys.HasCompletedWish("sv_1"));
        }

        [Fact]
        public void AdvanceWishStep_SeeTheSky_CompletesInOneStep()
        {
            var sys = CreateSystem();
            sys.RegisterWish("skywatcher", FinalWishSystem.WishSeeTheSky);
            sys.DeclareTerminalPrognosis("sv_1", "skywatcher", isAlive: true);
            Assert.True(sys.AdvanceWishStep("sv_1", "step_1"));
            Assert.True(sys.HasCompletedWish("sv_1"));
        }

        [Fact]
        public void CompleteWish_AppliesMoraleBuff_AndFiresEvent()
        {
            var sys = CreateSystem();
            float appliedBuff = 0f;
            string completedId = null;
            sys.ApplyPermanentShelterMoraleBuff = (buff) => appliedBuff = buff;
            sys.OnFinalWishCompleted += (id) => completedId = id;
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            sys.AdvanceWishStep("sv_1", "step_1");
            sys.AdvanceWishStep("sv_1", "step_2");
            Assert.Equal(FinalWishSystem.WishCompletedMoraleBuff, appliedBuff);
            Assert.Equal("sv_1", completedId);
        }

        [Fact]
        public void OnPrognosisExpired_AppliesPenalty_AndFiresEvent()
        {
            var sys = CreateSystem();
            float appliedBuff = 0f;
            string failedId = null;
            sys.ApplyPermanentShelterMoraleBuff = (buff) => appliedBuff = buff;
            sys.OnFinalWishFailed += (id) => failedId = id;
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            sys.OnPrognosisExpired("sv_1");
            Assert.Equal(FinalWishSystem.WishFailedMoralePenalty, appliedBuff);
            Assert.Equal("sv_1", failedId);
            Assert.False(sys.HasActiveWish("sv_1"));
        }

        [Fact]
        public void OnPrognosisExpired_DoesNothingIfWishAlreadyCompleted()
        {
            var sys = CreateSystem();
            float appliedBuff = 0f;
            sys.ApplyPermanentShelterMoraleBuff = (buff) => appliedBuff = buff;
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            sys.AdvanceWishStep("sv_1", "step_1");
            sys.AdvanceWishStep("sv_1", "step_2");
            sys.OnPrognosisExpired("sv_1");
            // Should not apply penalty since wish was completed
            Assert.Equal(FinalWishSystem.WishCompletedMoraleBuff, appliedBuff);
        }

        [Fact]
        public void Tick_DecrementsDaysRemaining()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            float initialDays = sys.GetDaysRemaining("sv_1");
            sys.Tick("sv_1", 24f, isAlive: true); // 1 day
            Assert.Equal(initialDays - 1f, sys.GetDaysRemaining("sv_1"), 4);
        }

        [Fact]
        public void Tick_ExpiresPrognosis_WhenDaysReachZero()
        {
            var sys = CreateSystem();
            float appliedBuff = 0f;
            sys.ApplyPermanentShelterMoraleBuff = (buff) => appliedBuff = buff;
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            // Tick enough to expire (max 7 days)
            sys.Tick("sv_1", 24f * 10f, isAlive: true);
            Assert.Equal(FinalWishSystem.WishFailedMoralePenalty, appliedBuff);
            Assert.False(sys.HasActiveWish("sv_1"));
        }

        [Fact]
        public void Tick_DoesNothingForDeadSurvivor()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            float initialDays = sys.GetDaysRemaining("sv_1");
            sys.Tick("sv_1", 24f, isAlive: false);
            Assert.Equal(initialDays, sys.GetDaysRemaining("sv_1"));
        }

        [Fact]
        public void RegisterWish_OverridesArchetypeMapping()
        {
            var sys = CreateSystem();
            sys.RegisterWish("custom_arch", FinalWishSystem.WishRetrieveHeirloom);
            sys.DeclareTerminalPrognosis("sv_1", "custom_arch", isAlive: true);
            Assert.Equal(FinalWishSystem.WishRetrieveHeirloom, sys.GetWishType("sv_1"));
        }

        [Fact]
        public void ArchetypePrefix_Surgeon_MapsToTeachLesson()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "field_surgeon", isAlive: true);
            Assert.Equal(FinalWishSystem.WishTeachLesson, sys.GetWishType("sv_1"));
        }

        [Fact]
        public void ArchetypePrefix_Parent_MapsToReconcile()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "single_parent", isAlive: true);
            Assert.Equal(FinalWishSystem.WishReconcile, sys.GetWishType("sv_1"));
        }

        [Fact]
        public void SaveLoad_RoundTrips()
        {
            var sys = CreateSystem();
            sys.RegisterWish("custom_arch", FinalWishSystem.WishBuildMemorial);
            sys.DeclareTerminalPrognosis("sv_1", "custom_arch", isAlive: true);
            sys.AdvanceWishStep("sv_1", "step_1");
            sys.DeclareTerminalPrognosis("sv_2", "unknown_archetype", isAlive: true);

            var saved = sys.CaptureState();

            var sys2 = CreateSystem();
            sys2.RestoreState(saved);

            Assert.True(sys2.HasActiveWish("sv_1"));
            Assert.Equal(1, sys2.GetStepsCompleted("sv_1"));
            Assert.Equal(FinalWishSystem.WishBuildMemorial, sys2.GetWishType("sv_1"));
            Assert.True(sys2.HasActiveWish("sv_2"));
            Assert.Equal(FinalWishSystem.WishDeliverLetter, sys2.GetWishType("sv_2"));
        }

        [Fact]
        public void SaveLoad_DeepCopy_NoSharedReferences()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            var saved = sys.CaptureState();

            // Mutate original
            sys.AdvanceWishStep("sv_1", "step_1");
            sys.AdvanceWishStep("sv_1", "step_2");

            // Restore into new system — should have original state, not mutated
            var sys2 = CreateSystem();
            sys2.RestoreState(saved);
            Assert.Equal(0, sys2.GetStepsCompleted("sv_1"));
            Assert.True(sys2.HasActiveWish("sv_1"));
            Assert.False(sys2.HasCompletedWish("sv_1"));
        }

        [Fact]
        public void RestoreState_Null_ClearsAll()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            sys.RestoreState(null);
            Assert.False(sys.HasTerminalPrognosis("sv_1"));
            Assert.False(sys.HasActiveWish("sv_1"));
        }

        [Fact]
        public void AdvanceWishStep_RejectsUnknownSurvivor()
        {
            var sys = CreateSystem();
            Assert.False(sys.AdvanceWishStep("nonexistent", "step_1"));
        }

        [Fact]
        public void OnStateChanged_FiresOnDeclare()
        {
            var sys = CreateSystem();
            int fireCount = 0;
            sys.OnStateChanged += () => fireCount++;
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            Assert.True(fireCount > 0);
        }
    }
}
