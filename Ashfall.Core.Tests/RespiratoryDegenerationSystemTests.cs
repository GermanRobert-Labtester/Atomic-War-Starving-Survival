using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class RespiratoryDegenerationSystemTests
    {
        private const string SvA = "sv_alpha";
        private const string SvB = "sv_bravo";

        private static RespiratoryDegenerationSystem CreateSystem(
            float filterHealth = 100f,
            bool falloutStorm = false,
            bool ashZone = false)
        {
            var sys = new RespiratoryDegenerationSystem();
            sys.GetFilterHealth = () => filterHealth;
            sys.IsInFalloutStorm = () => falloutStorm;
            sys.IsInAshZone = () => ashZone;
            return sys;
        }

        // ── 1. Tick accumulates degradation during fallout storm ──────

        [Fact]
        public void Tick_FalloutStorm_AccumulatesDegradation()
        {
            var sys = CreateSystem(falloutStorm: true);
            float totalDelta = 0f;
            sys.OnRespiratoryDegradationIncreased += (id, delta) => totalDelta += delta;

            sys.TickHours(SvA, 4f);

            Assert.True(sys.RespiratoryDegradation(SvA) > 0f);
            Assert.Equal(RespiratoryDegenerationSystem.AshExposureDegradationRate * 4f,
                sys.RespiratoryDegradation(SvA), 4);
            Assert.True(totalDelta > 0f);
        }

        // ── 2. Tick with good filter indoors — no accumulation ────────

        [Fact]
        public void Tick_GoodFilterIndoors_NoAccumulation()
        {
            var sys = CreateSystem(filterHealth: 80f); // above threshold
            sys.TickHours(SvA, 8f);
            Assert.Equal(0f, sys.RespiratoryDegradation(SvA));
        }

        // ── 3. Tick with bad filter indoors — accumulates slowly ──────

        [Fact]
        public void Tick_BadFilterIndoors_AccumulatesSlowly()
        {
            var sys = CreateSystem(filterHealth: 30f); // below threshold
            sys.TickHours(SvA, 24f);

            float expected = RespiratoryDegenerationSystem.DegradationPerDayWithoutFilter; // 2f per day
            Assert.Equal(expected, sys.RespiratoryDegradation(SvA), 4);
        }

        // ── 4. Bad filter during storm doubles rate ───────────────────

        [Fact]
        public void Tick_FalloutStorm_BadFilter_DoublesRate()
        {
            var sys = CreateSystem(falloutStorm: true, filterHealth: 20f);
            sys.TickHours(SvA, 2f);

            float expected = RespiratoryDegenerationSystem.AshExposureDegradationRate *
                             RespiratoryDegenerationSystem.UnmaintainedFilterMultiplier * 2f;
            Assert.Equal(expected, sys.RespiratoryDegradation(SvA), 4);
        }

        // ── 5. Ash zone gives half rate ───────────────────────────────

        [Fact]
        public void Tick_AshZone_HalfRate()
        {
            var sys = CreateSystem(ashZone: true);
            sys.TickHours(SvA, 6f);

            float expected = RespiratoryDegenerationSystem.AshExposureDegradationRate * 0.5f * 6f;
            Assert.Equal(expected, sys.RespiratoryDegradation(SvA), 4);
        }

        // ── 6. Severe cough threshold event fires ─────────────────────

        [Fact]
        public void Tick_CrossesSevereCoughThreshold_FiresEvent()
        {
            var sys = CreateSystem(falloutStorm: true);
            var firedFor = new List<string>();
            sys.OnSevereCoughStarted += id => firedFor.Add(id);

            // Manually push degradation past threshold
            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = RespiratoryDegenerationSystem.SevereCoughThreshold - 1f;

            sys.TickHours(SvA, 10f); // enough to push past 50

            Assert.Contains(SvA, firedFor);
            Assert.True(sys.RespiratoryDegradation(SvA) >= RespiratoryDegenerationSystem.SevereCoughThreshold);
        }

        // ── 7. Irreversible threshold sets permanent damage ───────────

        [Fact]
        public void Tick_CrossesIrreversibleThreshold_SetsPermanentDamage()
        {
            var sys = CreateSystem(falloutStorm: true);
            bool inhalerRequired = false;
            sys.OnRequiresInhaler += id => { if (id == SvA) inhalerRequired = true; };

            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = RespiratoryDegenerationSystem.IrreversibleThreshold - 1f;

            sys.TickHours(SvA, 10f);

            Assert.True(sys.HasPermanentLungDamage(SvA));
            Assert.True(sys.RequiresInhaler(SvA));
            Assert.True(inhalerRequired);
        }

        // ── 8. Terminal lung threshold event fires ────────────────────

        [Fact]
        public void Tick_CrossesTerminalThreshold_FiresEvent()
        {
            var sys = CreateSystem(falloutStorm: true);
            bool terminalFired = false;
            sys.OnTerminalLungDamage += id => { if (id == SvA) terminalFired = true; };

            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = RespiratoryDegenerationSystem.TerminalLungThreshold - 1f;

            sys.TickHours(SvA, 10f);

            Assert.True(terminalFired);
        }

        // ── 9. ApplyInhaler reduces degradation and provides relief ───

        [Fact]
        public void ApplyInhaler_ReducesDegradation_ProvidesRelief()
        {
            var sys = CreateSystem();
            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = 60f;

            bool result = sys.ApplyInhaler(SvA);

            Assert.True(result);
            Assert.Equal(60f - RespiratoryDegenerationSystem.InhalerDegradationReduction,
                sys.RespiratoryDegradation(SvA), 4);
            Assert.Equal(RespiratoryDegenerationSystem.InhalerReliefDurationHours,
                sys.InhalerReliefHours(SvA), 4);
        }

        // ── 10. ApplyInhaler fails when degradation is zero ───────────

        [Fact]
        public void ApplyInhaler_ZeroDegradation_ReturnsFalse()
        {
            var sys = CreateSystem();
            sys.GetOrCreate(SvA); // create record with 0 degradation

            Assert.False(sys.ApplyInhaler(SvA));
        }

        // ── 11. ApplyHerbalTea reduces degradation ────────────────────

        [Fact]
        public void ApplyHerbalTea_ReducesDegradation()
        {
            var sys = CreateSystem();
            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = 20f;

            bool result = sys.ApplyHerbalTea(SvA);

            Assert.True(result);
            Assert.Equal(20f - RespiratoryDegenerationSystem.HerbalTeaDegradationReduction,
                sys.RespiratoryDegradation(SvA), 4);
        }

        // ── 12. ApplyHerbalTea fails for unknown survivor ─────────────

        [Fact]
        public void ApplyHerbalTea_UnknownSurvivor_ReturnsFalse()
        {
            var sys = CreateSystem();
            Assert.False(sys.ApplyHerbalTea("sv_nobody"));
        }

        // ── 13. GetStaminaMultiplier returns correct values ───────────

        [Fact]
        public void GetStaminaMultiplier_BelowThreshold_Returns1()
        {
            var sys = CreateSystem();
            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = 30f;

            Assert.Equal(1f, sys.GetStaminaMultiplier(SvA));
        }

        [Fact]
        public void GetStaminaMultiplier_AboveThreshold_NoRelief_ReturnsPenalty()
        {
            var sys = CreateSystem();
            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = 60f;
            state.inhalerReliefHours = 0f;

            float expected = 1f - RespiratoryDegenerationSystem.SevereCoughStaminaPenalty;
            Assert.Equal(expected, sys.GetStaminaMultiplier(SvA), 4);
        }

        [Fact]
        public void GetStaminaMultiplier_AboveThreshold_WithRelief_Returns1()
        {
            var sys = CreateSystem();
            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = 60f;
            state.inhalerReliefHours = 4f;

            Assert.Equal(1f, sys.GetStaminaMultiplier(SvA));
        }

        // ── 14. Inhaler relief countdown ──────────────────────────────

        [Fact]
        public void Tick_InhalerRelief_CountsDown()
        {
            var sys = CreateSystem();
            var state = sys.GetOrCreate(SvA);
            state.inhalerReliefHours = 5f;
            state.respiratoryDegradation = 10f; // need some degradation so tick doesn't early-out

            // Need a rate > 0 to exercise the inhaler countdown path
            sys.IsInAshZone = () => true;
            sys.TickHours(SvA, 3f);

            Assert.Equal(2f, sys.InhalerReliefHours(SvA), 4);
        }

        [Fact]
        public void Tick_InhalerRelief_ClampsToZero()
        {
            var sys = CreateSystem();
            var state = sys.GetOrCreate(SvA);
            state.inhalerReliefHours = 2f;

            sys.IsInAshZone = () => true;
            sys.TickHours(SvA, 5f);

            Assert.Equal(0f, sys.InhalerReliefHours(SvA));
        }

        // ── 15. Null/empty survivor ID rejected ───────────────────────

        [Fact]
        public void Tick_NullSurvivorId_NoOp()
        {
            var sys = CreateSystem(falloutStorm: true);
            sys.TickHours(null, 4f);
            sys.TickHours("", 4f);
            // No exception, no state created
            Assert.Empty(sys.Survivors);
        }

        [Fact]
        public void ApplyInhaler_NullSurvivorId_ReturnsFalse()
        {
            var sys = CreateSystem();
            Assert.False(sys.ApplyInhaler(null));
            Assert.False(sys.ApplyInhaler(""));
        }

        // ── 16. Severe cough stamina/morale events fire during tick ───

        [Fact]
        public void Tick_AboveSevereCough_FiresStaminaAndMoraleEvents()
        {
            var sys = CreateSystem(falloutStorm: true);
            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = RespiratoryDegenerationSystem.SevereCoughThreshold + 5f;

            float staminaPenalty = 0f;
            float moraleDrain = 0f;
            sys.OnStaminaPenaltyRequested += (id, factor) => staminaPenalty += factor;
            sys.OnMoraleDrainRequested += (id, amount) => moraleDrain += amount;

            sys.TickHours(SvA, 12f);

            Assert.True(staminaPenalty > 0f);
            Assert.True(moraleDrain < 0f); // negative = drain
        }

        // ── 17. Past irreversible, no further accumulation ────────────

        [Fact]
        public void Tick_PastIrreversibleWithPermanentDamage_NoAccumulation()
        {
            var sys = CreateSystem(falloutStorm: true);
            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = RespiratoryDegenerationSystem.IrreversibleThreshold + 5f;
            state.hasPermanentLungDamage = true;

            float before = sys.RespiratoryDegradation(SvA);
            sys.TickHours(SvA, 24f);

            Assert.Equal(before, sys.RespiratoryDegradation(SvA));
        }

        // ── 18. CaptureState / RestoreState round-trip ────────────────

        [Fact]
        public void SaveLoad_RoundTrip_PreservesState()
        {
            var sys = CreateSystem(falloutStorm: true);
            sys.TickHours(SvA, 10f);
            sys.TickHours(SvB, 5f);
            sys.ApplyInhaler(SvA);

            var snapshot = sys.CaptureState();

            var sys2 = CreateSystem();
            sys2.RestoreState(snapshot);

            Assert.Equal(sys.RespiratoryDegradation(SvA), sys2.RespiratoryDegradation(SvA), 4);
            Assert.Equal(sys.RespiratoryDegradation(SvB), sys2.RespiratoryDegradation(SvB), 4);
            Assert.Equal(sys.HasPermanentLungDamage(SvA), sys2.HasPermanentLungDamage(SvA));
            Assert.Equal(sys.InhalerReliefHours(SvA), sys2.InhalerReliefHours(SvA), 4);
        }

        // ── 19. CaptureState snapshot isolation ───────────────────────

        [Fact]
        public void CaptureState_SnapshotIsolation_MutatingSnapshotDoesNotAffectLive()
        {
            var sys = CreateSystem(falloutStorm: true);
            sys.TickHours(SvA, 10f);

            var snapshot = sys.CaptureState();
            snapshot.survivors[0].respiratoryDegradation = 999f;

            // Live state unchanged
            Assert.True(sys.RespiratoryDegradation(SvA) < 999f);
        }

        // ── 20. CaptureState ordinal ordering ─────────────────────────

        [Fact]
        public void CaptureState_OrdinalOrdering()
        {
            var sys = CreateSystem(falloutStorm: true);
            sys.TickHours("sv_zulu", 2f);
            sys.TickHours("sv_alpha", 2f);
            sys.TickHours("sv_mike", 2f);

            var snapshot = sys.CaptureState();
            Assert.Equal(3, snapshot.survivors.Count);
            Assert.Equal("sv_alpha", snapshot.survivors[0].survivorId);
            Assert.Equal("sv_mike", snapshot.survivors[1].survivorId);
            Assert.Equal("sv_zulu", snapshot.survivors[2].survivorId);
        }

        // ── 21. RestoreState(null) safety ─────────────────────────────

        [Fact]
        public void RestoreState_Null_DoesNotThrow()
        {
            var sys = CreateSystem(falloutStorm: true);
            sys.TickHours(SvA, 10f);

            sys.RestoreState(null);

            Assert.Empty(sys.Survivors);
        }

        // ── 22. SaveChecksum stability across round-trip ──────────────

        [Fact]
        public void SaveChecksum_StableAcrossRoundTrip()
        {
            var sys = CreateSystem(falloutStorm: true);
            sys.TickHours(SvA, 10f);
            sys.TickHours(SvB, 5f);

            var snapshot1 = sys.CaptureState();
            string hash1 = SaveChecksum.Compute(snapshot1);

            var sys2 = CreateSystem();
            sys2.RestoreState(snapshot1);
            var snapshot2 = sys2.CaptureState();
            string hash2 = SaveChecksum.Compute(snapshot2);

            Assert.Equal(hash1, hash2);
        }

        // ── 23. Degradation capped at 100 ─────────────────────────────

        [Fact]
        public void Tick_DegradationCappedAt100()
        {
            var sys = CreateSystem(falloutStorm: true, filterHealth: 10f);
            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = 99f;

            sys.TickHours(SvA, 100f);

            Assert.Equal(100f, sys.RespiratoryDegradation(SvA));
        }

        // ── 24. OnStateChanged fires on mutations ─────────────────────

        [Fact]
        public void OnStateChanged_FiresOnTickAndTreatment()
        {
            var sys = CreateSystem(falloutStorm: true);
            int changeCount = 0;
            sys.OnStateChanged += () => changeCount++;

            sys.TickHours(SvA, 4f);
            int afterTick = changeCount;
            Assert.True(afterTick > 0);

            sys.ApplyInhaler(SvA);
            Assert.True(changeCount > afterTick);
        }

        // ── 25. ApplyHerbalTea clears requiresInhaler when below threshold

        [Fact]
        public void ApplyHerbalTea_BelowIrreversible_ClearsRequiresInhaler()
        {
            var sys = CreateSystem();
            var state = sys.GetOrCreate(SvA);
            state.respiratoryDegradation = RespiratoryDegenerationSystem.IrreversibleThreshold + 5f;
            state.requiresInhaler = true;

            sys.ApplyHerbalTea(SvA);

            // 85 - 3 = 82, still above 80
            Assert.True(sys.RequiresInhaler(SvA));

            // Apply again: 82 - 3 = 79, below threshold
            sys.ApplyHerbalTea(SvA);
            Assert.False(sys.RequiresInhaler(SvA));
        }
    }
}
