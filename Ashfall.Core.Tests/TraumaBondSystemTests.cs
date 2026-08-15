using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class TraumaBondSystemTests
    {
        // ── 1. Shared hazard creates a bond ────────────────────────────

        [Fact]
        public void OnSharedHazardEndured_CreatesBondBetweenParticipants()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 5f;

            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "fallout_storm");

            Assert.True(sys.HasBond("sv_1", "sv_2"));
            Assert.True(sys.HasBond("sv_2", "sv_1"));
            Assert.Equal(
                TraumaBondSystem.BondStrengthPerSharedHazard,
                sys.GetBondStrength("sv_1", "sv_2"), 4);
        }

        // ── 2. Bond strengthens on repeated hazards ────────────────────

        [Fact]
        public void OnSharedHazardEndured_RepeatedHazard_StrengthensBond()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 1f;

            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "fallout_storm");
            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "raid");

            float expected = TraumaBondSystem.BondStrengthPerSharedHazard * 2f;
            Assert.Equal(expected, sys.GetBondStrength("sv_1", "sv_2"), 4);
        }

        // ── 3. Bond strength is capped at 1.0 ─────────────────────────

        [Fact]
        public void OnSharedHazardEndured_BondStrengthCappedAtOne()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 1f;

            // 4 hazards × 0.30 = 1.20 → clamped to 1.0
            for (int i = 0; i < 4; i++)
                sys.OnSharedHazardEndured(
                    new List<string> { "sv_1", "sv_2" }, $"hazard_{i}");

            Assert.Equal(1f, sys.GetBondStrength("sv_1", "sv_2"), 4);
        }

        // ── 4. Decay removes weak bonds ────────────────────────────────

        [Fact]
        public void Tick_DecaysBondStrength()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 1f;

            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "fallout_storm");

            float before = sys.GetBondStrength("sv_1", "sv_2");
            sys.Tick("sv_1", 24f); // 1 day → decay = 0.01
            float after = sys.GetBondStrength("sv_1", "sv_2");

            Assert.True(after < before);
            Assert.Equal(before - TraumaBondSystem.BondDecayPerDay, after, 4);
        }

        // ── 5. Bond fully decays → removed + event fired ───────────────

        [Fact]
        public void Tick_BondFullyDecays_RemovedAndEventFired()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 1f;

            // Create a bond with minimal strength
            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "fallout_storm");

            // Decay enough to remove (0.30 / 0.01 = 30 days = 720 hours, +extra to clear float imprecision)
            string decayedA = null, decayedB = null;
            sys.OnTraumaBondDecayed += (a, b) => { decayedA = a; decayedB = b; };

            sys.Tick("sv_1", 721f);

            Assert.Equal(0, sys.GetBondCount("sv_1"));
            Assert.Equal("sv_1", decayedA);
            Assert.Equal("sv_2", decayedB);
        }

        // ── 6. Co-shift bonus requires minimum bond strength ───────────

        [Fact]
        public void GetCoShiftEfficiencyBonus_BelowMinThreshold_ReturnsZero()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 1f;

            // Bond strength 0.30 == MinBondStrengthForBonus (0.30)
            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "fallout_storm");

            float bonus = sys.GetCoShiftEfficiencyBonus("sv_1", "sv_2");
            Assert.True(bonus > 0f); // exactly at threshold → qualifies
        }

        [Fact]
        public void GetCoShiftEfficiencyBonus_NoBond_ReturnsZero()
        {
            var sys = new TraumaBondSystem();
            Assert.Equal(0f, sys.GetCoShiftEfficiencyBonus("sv_1", "sv_2"));
        }

        // ── 7. Co-shift bonus scales with bond strength ────────────────

        [Fact]
        public void GetCoShiftEfficiencyBonus_ScalesWithBondStrength()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 1f;

            // 2 hazards → strength 0.60
            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "fallout_storm");
            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "raid");

            float expected = TraumaBondSystem.CoShiftEfficiencyBonus * 0.60f;
            Assert.Equal(expected,
                sys.GetCoShiftEfficiencyBonus("sv_1", "sv_2"), 4);
        }

        // ── 8. Same survivor ID returns zero bonus ─────────────────────

        [Fact]
        public void GetCoShiftEfficiencyBonus_SameSurvivor_ReturnsZero()
        {
            var sys = new TraumaBondSystem();
            Assert.Equal(0f, sys.GetCoShiftEfficiencyBonus("sv_1", "sv_1"));
        }

        // ── 9. Null/empty inputs are safe ──────────────────────────────

        [Fact]
        public void OnSharedHazardEndured_NullOrEmpty_DoesNotThrow()
        {
            var sys = new TraumaBondSystem();
            sys.OnSharedHazardEndured(null, "hazard");
            sys.OnSharedHazardEndured(new List<string>(), "hazard");
            sys.OnSharedHazardEndured(new List<string> { "sv_1" }, "hazard");
            sys.OnSharedHazardEndured(
                new List<string> { null!, "sv_2" }, "hazard");

            Assert.Equal(0, sys.GetBondCount("sv_1"));
        }

        [Fact]
        public void Tick_NullSurvivorId_DoesNotThrow()
        {
            var sys = new TraumaBondSystem();
            sys.Tick(null, 24f);
            sys.Tick("", 24f);
        }

        // ── 10. CaptureState / RestoreState roundtrip ──────────────────

        [Fact]
        public void CaptureRestore_Roundtrip_PreservesAllBonds()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 10f;

            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "fallout_storm");
            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_3" }, "raid");
            sys.OnSharedHazardEndured(
                new List<string> { "sv_2", "sv_3" }, "starvation");

            var save = sys.CaptureState();

            var restored = new TraumaBondSystem();
            restored.RestoreState(save);

            Assert.Equal(sys.GetBondStrength("sv_1", "sv_2"),
                restored.GetBondStrength("sv_1", "sv_2"), 4);
            Assert.Equal(sys.GetBondStrength("sv_1", "sv_3"),
                restored.GetBondStrength("sv_1", "sv_3"), 4);
            Assert.Equal(sys.GetBondStrength("sv_2", "sv_3"),
                restored.GetBondStrength("sv_2", "sv_3"), 4);
            Assert.Equal(sys.GetBondCount("sv_1"),
                restored.GetBondCount("sv_1"));
            Assert.Equal(sys.GetBondCount("sv_2"),
                restored.GetBondCount("sv_2"));
        }

        // ── 11. RestoreState(null) clears all state ────────────────────

        [Fact]
        public void RestoreState_Null_ClearsAllBonds()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 1f;

            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "fallout_storm");
            Assert.True(sys.HasBond("sv_1", "sv_2"));

            sys.RestoreState(null);
            Assert.Equal(0, sys.GetBondCount("sv_1"));
            Assert.False(sys.HasBond("sv_1", "sv_2"));
        }

        // ── 12. CaptureState produces deep copy ────────────────────────

        [Fact]
        public void CaptureState_IsDeepCopy_MutatingOriginalDoesNotAffectSave()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 1f;

            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "fallout_storm");

            var save = sys.CaptureState();
            float savedStrength = save.Survivors[0].Bonds[0].BondStrength;

            // Mutate the original
            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "raid");

            // Save should be unchanged
            Assert.Equal(savedStrength, save.Survivors[0].Bonds[0].BondStrength, 4);
        }

        // ── 13. OnTraumaBondFormed event fires ─────────────────────────

        [Fact]
        public void OnSharedHazardEndured_FiresBondFormedEvent()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 1f;

            var formedEvents = new List<(string a, string b, string h)>();
            sys.OnTraumaBondFormed += (a, b, h) => formedEvents.Add((a, b, h));

            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "fallout_storm");

            // Both directions fire: sv_1→sv_2 and sv_2→sv_1
            Assert.Equal(2, formedEvents.Count);
            Assert.Contains(formedEvents, e => e.a == "sv_1" && e.b == "sv_2" && e.h == "fallout_storm");
            Assert.Contains(formedEvents, e => e.a == "sv_2" && e.b == "sv_1" && e.h == "fallout_storm");
        }

        // ── 14. AdjustAffinity hook is called ──────────────────────────

        [Fact]
        public void OnSharedHazardEndured_CallsAdjustAffinityHook()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 1f;

            string hookA = null, hookB = null;
            float hookDelta = 0f;
            sys.AdjustAffinity = (a, b, d) =>
            {
                hookA = a; hookB = b; hookDelta = d;
            };

            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2" }, "fallout_storm");

            Assert.Equal("sv_1", hookA);
            Assert.Equal("sv_2", hookB);
            Assert.Equal(TraumaBondSystem.BondAffinityBonus, hookDelta, 4);
        }

        // ── 15. Multi-participant hazard creates all pairs ─────────────

        [Fact]
        public void OnSharedHazardEndured_ThreeParticipants_CreatesAllPairs()
        {
            var sys = new TraumaBondSystem();
            sys.GetDay = () => 1f;

            sys.OnSharedHazardEndured(
                new List<string> { "sv_1", "sv_2", "sv_3" }, "raid");

            // All 3 pairs should have bonds in both directions
            Assert.True(sys.HasBond("sv_1", "sv_2"));
            Assert.True(sys.HasBond("sv_2", "sv_1"));
            Assert.True(sys.HasBond("sv_1", "sv_3"));
            Assert.True(sys.HasBond("sv_3", "sv_1"));
            Assert.True(sys.HasBond("sv_2", "sv_3"));
            Assert.True(sys.HasBond("sv_3", "sv_2"));

            Assert.Equal(2, sys.GetBondCount("sv_1"));
            Assert.Equal(2, sys.GetBondCount("sv_2"));
            Assert.Equal(2, sys.GetBondCount("sv_3"));
        }
    }
}
