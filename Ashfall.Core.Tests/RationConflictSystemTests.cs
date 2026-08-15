using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class RationConflictSystemTests
    {
        [Fact]
        public void EqualAllocation_NoResentment()
        {
            var sys = new RationConflictSystem(new SeededRng(42));
            sys.RegisterSurvivor("sv_1");
            sys.RegisterSurvivor("sv_2");
            sys.SetAllocation("sv_1", 0.5f);
            sys.SetAllocation("sv_2", 0.5f);
            sys.Tick("sv_1", 24f);
            var state = sys.GetState("sv_1");
            Assert.Equal(0f, state.resentmentLevel);
        }

        [Fact]
        public void UnequalAllocation_BuildsResentment()
        {
            var sys = new RationConflictSystem(new SeededRng(42));
            sys.RegisterSurvivor("sv_1");
            sys.RegisterSurvivor("sv_2");
            sys.SetAllocation("sv_1", 0.2f);
            sys.SetAllocation("sv_2", 0.8f);
            sys.Tick("sv_1", 24f);
            var state = sys.GetState("sv_1");
            Assert.True(state.resentmentLevel > 0f);
            Assert.Equal("sv_2", state.resentmentTargetId);
        }

        [Fact]
        public void Fairness_HighWhenEqual()
        {
            var sys = new RationConflictSystem(new SeededRng(42));
            sys.RegisterSurvivor("sv_1");
            sys.SetAllocation("sv_1", 0.5f);
            sys.Tick("sv_1", 24f);
            var state = sys.GetState("sv_1");
            Assert.True(state.perceivedFairness > 0.9f);
        }

        [Fact]
        public void ResentmentEvent_Fires()
        {
            var sys = new RationConflictSystem(new SeededRng(42));
            sys.RegisterSurvivor("sv_1");
            sys.RegisterSurvivor("sv_2");
            sys.SetAllocation("sv_1", 0.1f);
            sys.SetAllocation("sv_2", 0.9f);
            string firedFor = null;
            sys.OnResentmentBuilt += (id, _, __) => firedFor = id;
            sys.Tick("sv_1", 24f);
            Assert.Equal("sv_1", firedFor);
        }

        [Fact]
        public void Resentment_DecaysWhenFair()
        {
            var sys = new RationConflictSystem(new SeededRng(42));
            sys.RegisterSurvivor("sv_1");
            sys.RegisterSurvivor("sv_2");
            sys.SetAllocation("sv_1", 0.2f);
            sys.SetAllocation("sv_2", 0.8f);
            sys.Tick("sv_1", 24f);
            Assert.True(sys.GetState("sv_1").resentmentLevel > 0f);

            sys.SetAllocation("sv_1", 0.5f);
            sys.SetAllocation("sv_2", 0.5f);
            sys.Tick("sv_1", 24f);
            Assert.True(sys.GetState("sv_1").resentmentLevel < 0.1f);
        }

        [Fact]
        public void CaptureRestore_Roundtrip()
        {
            var sys = new RationConflictSystem(new SeededRng(42));
            sys.RegisterSurvivor("sv_1");
            sys.RegisterSurvivor("sv_2");
            sys.SetAllocation("sv_1", 0.2f);
            sys.SetAllocation("sv_2", 0.8f);
            sys.Tick("sv_1", 24f);

            var save = sys.CaptureState();
            Assert.Equal(2, save.survivors.Count);

            var restored = new RationConflictSystem(new SeededRng(99));
            restored.RestoreState(save);
            Assert.True(restored.GetState("sv_1").resentmentLevel > 0f);
            Assert.Equal("sv_2", restored.GetState("sv_1").resentmentTargetId);
        }

        [Fact]
        public void RestoreNull_DoesNotCrash()
        {
            var sys = new RationConflictSystem();
            sys.RestoreState(null);
        }
    }
}
