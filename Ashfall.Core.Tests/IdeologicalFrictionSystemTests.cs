using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class IdeologicalFrictionSystemTests
    {
        [Fact]
        public void SameBelief_Synergy()
        {
            var sys = new IdeologicalFrictionSystem();
            sys.RegisterBelief("sv_1", "pacifist");
            sys.RegisterBelief("sv_2", "pacifist");
            float mult = sys.GetRoommateCompatibilityMultiplier("sv_1", "sv_2");
            Assert.True(mult > 1f);
        }

        [Fact]
        public void ConflictingBeliefs_Penalty()
        {
            var sys = new IdeologicalFrictionSystem();
            sys.RegisterBelief("sv_1", "military_discipline");
            sys.RegisterBelief("sv_2", "pacifist");
            float mult = sys.GetRoommateCompatibilityMultiplier("sv_1", "sv_2");
            Assert.True(mult < 1f);
        }

        [Fact]
        public void NeutralBeliefs_NoEffect()
        {
            var sys = new IdeologicalFrictionSystem();
            sys.RegisterBelief("sv_1", "pacifist");
            sys.RegisterBelief("sv_2", "collectivist_solidarity");
            float mult = sys.GetRoommateCompatibilityMultiplier("sv_1", "sv_2");
            Assert.Equal(1f, mult);
        }

        [Fact]
        public void NoBelief_Registered_ReturnsNeutral()
        {
            var sys = new IdeologicalFrictionSystem();
            Assert.Equal(1f, sys.GetRoommateCompatibilityMultiplier("sv_1", "sv_2"));
        }

        [Fact]
        public void SameSurvivor_ReturnsNeutral()
        {
            var sys = new IdeologicalFrictionSystem();
            sys.RegisterBelief("sv_1", "pacifist");
            Assert.Equal(1f, sys.GetRoommateCompatibilityMultiplier("sv_1", "sv_1"));
        }

        [Fact]
        public void TickRoommates_Conflict_DrainsAffinity()
        {
            var sys = new IdeologicalFrictionSystem();
            sys.RegisterBelief("sv_1", "military_discipline");
            sys.RegisterBelief("sv_2", "pacifist");
            sys.TickRoommates("sv_1", "sv_2", 24f);
            Assert.True(sys.GetAffinity("sv_1", "sv_2") < 0f);
        }

        [Fact]
        public void TickRoommates_Synergy_GainsAffinity()
        {
            var sys = new IdeologicalFrictionSystem();
            sys.RegisterBelief("sv_1", "pacifist");
            sys.RegisterBelief("sv_2", "pacifist");
            sys.TickRoommates("sv_1", "sv_2", 24f);
            Assert.True(sys.GetAffinity("sv_1", "sv_2") > 0f);
        }

        [Fact]
        public void FrictionEvent_Fires()
        {
            var sys = new IdeologicalFrictionSystem();
            sys.RegisterBelief("sv_1", "religious_faith");
            sys.RegisterBelief("sv_2", "atheist_rationalist");
            string firedA = null;
            sys.OnFrictionDetected += (a, _, __) => firedA = a;
            sys.GetRoommateCompatibilityMultiplier("sv_1", "sv_2");
            Assert.Equal("sv_1", firedA);
        }

        [Fact]
        public void SynergyEvent_Fires()
        {
            var sys = new IdeologicalFrictionSystem();
            sys.RegisterBelief("sv_1", "pacifist");
            sys.RegisterBelief("sv_2", "pacifist");
            string firedA = null;
            sys.OnRoommateSynergy += (a, _) => firedA = a;
            sys.GetRoommateCompatibilityMultiplier("sv_1", "sv_2");
            Assert.Equal("sv_1", firedA);
        }

        [Fact]
        public void CaptureRestore_Roundtrip()
        {
            var sys = new IdeologicalFrictionSystem();
            sys.RegisterBelief("sv_1", "military_discipline");
            sys.RegisterBelief("sv_2", "pacifist");
            sys.TickRoommates("sv_1", "sv_2", 24f);

            var save = sys.CaptureState();
            Assert.Single(save.affinities);

            var restored = new IdeologicalFrictionSystem();
            restored.RestoreState(save);
            Assert.True(restored.GetAffinity("sv_1", "sv_2") < 0f);
        }

        [Fact]
        public void RestoreNull_DoesNotCrash()
        {
            var sys = new IdeologicalFrictionSystem();
            sys.RestoreState(null);
        }
    }
}
