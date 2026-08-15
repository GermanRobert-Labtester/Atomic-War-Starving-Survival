using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class LeadershipSystemTests
    {
        private static readonly List<string> AliveIds = new List<string> { "sv_alpha", "sv_beta", "sv_gamma" };

        private LeadershipSystem CreateSystem()
        {
            var sys = new LeadershipSystem();
            sys.GetAliveSurvivorIds = () => AliveIds;
            return sys;
        }

        // -- 1. Designate leader succeeds for alive survivor -----------------
        [Fact]
        public void DesignateLeader_AliveSurvivor_ReturnsTrueAndSetsLeader()
        {
            var sys = CreateSystem();
            bool result = sys.DesignateLeader("sv_alpha");
            Assert.True(result);
            Assert.Equal("sv_alpha", sys.CurrentLeaderId);
            Assert.True(sys.IsDesignatedLeader("sv_alpha"));
        }

        // -- 2. Designate leader fails for dead/unknown survivor -------------
        [Fact]
        public void DesignateLeader_UnknownSurvivor_ReturnsFalse()
        {
            var sys = CreateSystem();
            Assert.False(sys.DesignateLeader("sv_dead"));
            Assert.Null(sys.CurrentLeaderId);
        }

        // -- 3. Designating new leader clears previous -----------------------
        [Fact]
        public void DesignateLeader_ReplacesPrevious_ClearsOldLeader()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");
            sys.DesignateLeader("sv_beta");

            Assert.Equal("sv_beta", sys.CurrentLeaderId);
            Assert.False(sys.IsDesignatedLeader("sv_alpha"));
            Assert.True(sys.IsDesignatedLeader("sv_beta"));
        }

        // -- 4. Step down clears leader and starts cooldown ------------------
        [Fact]
        public void StepDown_CurrentLeader_ClearsLeaderAndStartsCooldown()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");
            bool stepped = sys.StepDown("sv_alpha");

            Assert.True(stepped);
            Assert.Null(sys.CurrentLeaderId);
            Assert.False(sys.IsDesignatedLeader("sv_alpha"));
            Assert.Equal(LeadershipSystem.StepDownCooldownDays, sys.StepDownCooldown);
        }

        // -- 5. Cannot designate during cooldown -----------------------------
        [Fact]
        public void DesignateLeader_DuringCooldown_ReturnsFalse()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");
            sys.StepDown("sv_alpha");

            // Cooldown is 14 days; tick only 1 day
            sys.Tick(24f);
            Assert.False(sys.DesignateLeader("sv_beta"));
        }

        // -- 6. Cooldown expires after enough time ---------------------------
        [Fact]
        public void Tick_AfterCooldownExpires_AllowsDesignation()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");
            sys.StepDown("sv_alpha");

            // Tick 15 days worth of hours
            sys.Tick(15f * 24f);
            Assert.True(sys.DesignateLeader("sv_beta"));
        }

        // -- 7. Survivor death increases leader stress -----------------------
        [Fact]
        public void OnSurvivorDied_LeaderAlive_IncreasesStress()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");

            sys.OnSurvivorDied("sv_beta");
            Assert.Equal(LeadershipSystem.LeaderStressPerDeath,
                sys.GetLeaderStress("sv_alpha"), 4);
            Assert.Equal(1, sys.GetDeathsWitnessed("sv_alpha"));
        }

        // -- 8. Stress caps at max and fires break risk ----------------------
        [Fact]
        public void OnSurvivorDied_StressReachesMax_FiresBreakRisk()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");

            string breakId = null;
            sys.OnLeaderBreakRisk += id => breakId = id;

            // 4 deaths × 25 = 100 = max
            sys.OnSurvivorDied("sv_beta");
            sys.OnSurvivorDied("sv_gamma");
            sys.OnSurvivorDied("sv_beta");
            sys.OnSurvivorDied("sv_gamma");

            Assert.Equal(LeadershipSystem.LeaderStressMax,
                sys.GetLeaderStress("sv_alpha"), 4);
            Assert.Equal("sv_alpha", breakId);
        }

        // -- 9. Injury increases stress --------------------------------------
        [Fact]
        public void OnSurvivorInjured_IncreasesStress()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");

            sys.OnSurvivorInjured("sv_beta");
            Assert.Equal(LeadershipSystem.LeaderStressPerInjury,
                sys.GetLeaderStress("sv_alpha"), 4);
        }

        // -- 10. Crisis event applies shelter morale aura --------------------
        [Fact]
        public void OnCrisisEvent_LeaderAlive_AppliesMoraleAura()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");

            float applied = 0f;
            sys.ApplyShelterMoraleDelta = delta => applied = delta;

            sys.OnCrisisEvent();
            Assert.Equal(LeadershipSystem.LeaderCrisisMoraleAura, applied, 4);
        }

        // -- 11. Crisis event does nothing without leader --------------------
        [Fact]
        public void OnCrisisEvent_NoLeader_DoesNothing()
        {
            var sys = CreateSystem();
            float applied = -1f;
            sys.ApplyShelterMoraleDelta = delta => applied = delta;

            sys.OnCrisisEvent();
            Assert.Equal(-1f, applied, 4); // never called
        }

        // -- 12. Stress decays over time -------------------------------------
        [Fact]
        public void Tick_DecaysStressOverTime()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");

            sys.OnSurvivorDied("sv_beta"); // +25 stress
            float before = sys.GetLeaderStress("sv_alpha");

            sys.Tick(24f); // 1 day → decay = 2
            float after = sys.GetLeaderStress("sv_alpha");

            Assert.Equal(before - LeadershipSystem.LeaderStressDecayPerDay, after, 4);
        }

        // -- 13. Stress does not go below zero -------------------------------
        [Fact]
        public void Tick_StressDoesNotGoBelowZero()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");
            // No stress accumulated; tick many days
            sys.Tick(2400f);
            Assert.Equal(0f, sys.GetLeaderStress("sv_alpha"), 4);
        }

        // -- 14. Events fire on designate/step-down --------------------------
        [Fact]
        public void DesignateAndStepDown_FireEvents()
        {
            var sys = CreateSystem();
            string designated = null, stepped = null;
            sys.OnLeaderDesignated += id => designated = id;
            sys.OnLeaderSteppedDown += id => stepped = id;

            sys.DesignateLeader("sv_alpha");
            Assert.Equal("sv_alpha", designated);

            sys.StepDown("sv_alpha");
            Assert.Equal("sv_alpha", stepped);
        }

        // -- 15. OnStateChanged fires on mutations ---------------------------
        [Fact]
        public void OnStateChanged_FiresOnDesignateStepDownDeathTick()
        {
            var sys = CreateSystem();
            int count = 0;
            sys.OnStateChanged += () => count++;

            sys.DesignateLeader("sv_alpha");  // +1
            sys.OnSurvivorDied("sv_beta");    // +1
            sys.Tick(24f);                    // +1 (stress changed)
            sys.StepDown("sv_alpha");         // +1

            Assert.True(count >= 4, $"Expected >=4 state changes, got {count}");
        }

        // -- 16. CaptureState / RestoreState roundtrip -----------------------
        [Fact]
        public void CaptureRestore_Roundtrip_PreservesState()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");
            sys.OnSurvivorDied("sv_beta");
            sys.OnSurvivorDied("sv_gamma");
            sys.Tick(12f); // half-day

            var saved = sys.CaptureState();

            var sys2 = CreateSystem();
            sys2.RestoreState(saved);

            Assert.Equal("sv_alpha", sys2.CurrentLeaderId);
            Assert.True(sys2.IsDesignatedLeader("sv_alpha"));
            Assert.Equal(sys.GetLeaderStress("sv_alpha"),
                sys2.GetLeaderStress("sv_alpha"), 4);
            Assert.Equal(sys.GetDeathsWitnessed("sv_alpha"),
                sys2.GetDeathsWitnessed("sv_alpha"));
            Assert.Equal(sys.StepDownCooldown, sys2.StepDownCooldown, 4);
        }

        // -- 17. RestoreState(null) clears everything ------------------------
        [Fact]
        public void RestoreState_Null_ClearsAll()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");
            sys.OnSurvivorDied("sv_beta");

            sys.RestoreState(null);

            Assert.Null(sys.CurrentLeaderId);
            Assert.Equal(0f, sys.GetLeaderStress("sv_alpha"), 4);
            Assert.False(sys.IsDesignatedLeader("sv_alpha"));
        }

        // -- 18. CaptureState produces deep copy (mutation isolation) --------
        [Fact]
        public void CaptureState_IsDeepCopy_MutationsDoNotLeak()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");
            sys.OnSurvivorDied("sv_beta");

            var saved = sys.CaptureState();
            float stressAtSave = sys.GetLeaderStress("sv_alpha");

            // Mutate the live system
            sys.OnSurvivorDied("sv_gamma");
            Assert.NotEqual(stressAtSave, sys.GetLeaderStress("sv_alpha"));

            // Restore from saved — should get original stress
            sys.RestoreState(saved);
            Assert.Equal(stressAtSave, sys.GetLeaderStress("sv_alpha"), 4);
        }

        // -- 19. Null/empty survivor ID safety -------------------------------
        [Fact]
        public void DesignateLeader_NullOrEmpty_ReturnsFalse()
        {
            var sys = CreateSystem();
            Assert.False(sys.DesignateLeader(null));
            Assert.False(sys.DesignateLeader(""));
        }

        // -- 20. StepDown wrong ID returns false -----------------------------
        [Fact]
        public void StepDown_WrongId_ReturnsFalse()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("sv_alpha");
            Assert.False(sys.StepDown("sv_beta"));
            Assert.Equal("sv_alpha", sys.CurrentLeaderId);
        }
    }
}
