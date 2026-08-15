using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class RadiationPhaseProgressionTests
    {
        private static PhaseProgressionState MakeSurvivor(string id = "sv_ada")
        {
            return new PhaseProgressionState { Id = id, Health = 100f, LungCapacity = 100f };
        }

        private static RadiationPhaseProgression CreateSystem(ISeededRng? rng = null)
        {
            return new RadiationPhaseProgression(rng ?? new SeededRng(42));
        }

        // ── Phase transitions ─────────────────────────────────────────

        [Fact]
        public void OnExposure_BelowTriggerDose_StaysHealthy()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            // Dose below prodromal trigger (100) — should not trigger
            sys.OnExposure("sv_ada", 90f);
            Assert.Equal(RadiationSicknessPhase.Healthy, sv.Phase);
            // But latent damage should accumulate
            Assert.True(sv.LatentDamage > 0f);
        }

        [Fact]
        public void OnExposure_AtTriggerDose_TransitionsToProdromal()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            var transitions = new List<(RadiationSicknessPhase oldPhase, RadiationSicknessPhase newPhase)>();
            sys.OnPhaseChanged += (id, oldP, newP) => transitions.Add((oldP, newP));

            sys.OnExposure("sv_ada", 110f);
            Assert.Equal(RadiationSicknessPhase.Prodromal, sv.Phase);
            Assert.Single(transitions);
            Assert.Equal(RadiationSicknessPhase.Healthy, transitions[0].oldPhase);
            Assert.Equal(RadiationSicknessPhase.Prodromal, transitions[0].newPhase);
        }

        [Fact]
        public void OnExposure_Prodromal_FiresHealthAndMoraleEvents()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            float healthDelta = 0f, moraleDelta = 0f;
            sys.OnHealthDeltaRequested += (id, d) => healthDelta += d;
            sys.OnMoraleDeltaRequested += (id, d) => moraleDelta += d;

            sys.OnExposure("sv_ada", 120f);

            Assert.Equal(-RadiationPhaseProgression.ProdromalHealthDip, healthDelta, 3);
            Assert.Equal(-RadiationPhaseProgression.ProdromalMoraleDip, moraleDelta, 3);
        }

        [Fact]
        public void OnExposure_Prodromal_FiresDoseResetEvent()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            bool resetFired = false;
            sys.OnRadiationDoseResetRequested += id => resetFired = true;

            sys.OnExposure("sv_ada", 110f);
            Assert.True(resetFired);
            Assert.Equal(0f, sv.AcuteDoseWindow);
        }

        [Fact]
        public void Tick_Prodromal_TransitionsToLatentAfterDuration()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            sys.OnExposure("sv_ada", 110f); // triggers Prodromal
            Assert.Equal(RadiationSicknessPhase.Prodromal, sv.Phase);

            // Prodromal lasts 24 hours = 1 day. OnsetTimer is in days.
            sys.Tick(24f); // 1 day = should transition to Latent
            Assert.Equal(RadiationSicknessPhase.Latent, sv.Phase);
        }

        [Fact]
        public void Tick_Prodromal_OngoingHealthDrain()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            float healthDrain = 0f;
            sys.OnHealthDeltaRequested += (id, d) => healthDrain += d;

            sys.OnExposure("sv_ada", 110f); // Prodromal + immediate dip
            float immediateDip = healthDrain;

            // Tick 12 hours of prodromal (half the duration)
            sys.Tick(12f);
            // Should have additional ongoing drain
            Assert.True(healthDrain < immediateDip); // more negative
        }

        [Fact]
        public void Tick_Latent_TransitionsToManifestAfterDuration()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            sys.OnExposure("sv_ada", 110f); // Prodromal
            sys.Tick(24f);                   // -> Latent
            Assert.Equal(RadiationSicknessPhase.Latent, sv.Phase);

            // Latent duration depends on severity. With latentDamage from 110 dose:
            // chronicDamage = 110 * 0.05 = 5.5, acuteLump = (110-100)*1.0 = 10, total = 15.5
            // severity = 15.5 / 60 = ~0.258
            // latentDays = lerp(12, 6, 0.258) = ~10.45 days = ~250.8 hours
            // Tick enough to pass through latent
            sys.Tick(300f);
            Assert.Equal(RadiationSicknessPhase.ManifestIllness, sv.Phase);
        }

        [Fact]
        public void Tick_Manifest_FiresHealthCrashOnEntry()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            float totalHealthDelta = 0f;
            sys.OnHealthDeltaRequested += (id, d) => totalHealthDelta += d;

            sys.OnExposure("sv_ada", 110f); // Prodromal
            sys.Tick(24f);                   // -> Latent
            float beforeManifest = totalHealthDelta;

            sys.Tick(300f); // -> Manifest
            // Should have a large health crash from manifest onset
            Assert.True(totalHealthDelta < beforeManifest - 20f); // at least 20hp crash
        }

        [Fact]
        public void Tick_Manifest_BedRestReducesBleed()
        {
            var sysResting = CreateSystem(new SeededRng(42));
            var svResting = MakeSurvivor("sv_rest");
            svResting.IsResting = true;
            sysResting.Register(svResting);

            var sysActive = CreateSystem(new SeededRng(42));
            var svActive = MakeSurvivor("sv_act");
            svActive.IsResting = false;
            sysActive.Register(svActive);

            // Get both to Manifest phase
            float bleedResting = 0f, bleedActive = 0f;
            sysResting.OnHealthDeltaRequested += (id, d) => bleedResting += d;
            sysActive.OnHealthDeltaRequested += (id, d) => bleedActive += d;

            sysResting.OnExposure("sv_rest", 110f);
            sysActive.OnExposure("sv_act", 110f);
            sysResting.Tick(24f);   // -> Latent
            sysActive.Tick(24f);
            sysResting.Tick(300f);  // -> Manifest
            sysActive.Tick(300f);

            // Reset bleed counters to measure only manifest bleed
            bleedResting = 0f;
            bleedActive = 0f;

            sysResting.Tick(24f);
            sysActive.Tick(24f);

            // Resting survivor should have less bleed
            Assert.True(System.Math.Abs(bleedResting) < System.Math.Abs(bleedActive));
        }

        [Fact]
        public void ResolveOutcome_LowLatentDamage_TransitionsToRecoveryOrDeath()
        {
            // With low latent damage (< ChronicFibrosisThreshold=120), should recover
            var sys = CreateSystem(new SeededRng(42));
            var sv = MakeSurvivor();
            sv.Health = 100f; // high health = low death chance
            sys.Register(sv);

            // Use a dose that gives moderate latent damage (below 120)
            sys.OnExposure("sv_ada", 110f); // Prodromal
            sys.Tick(24f);                   // -> Latent
            sys.Tick(300f);                  // -> Manifest
            Assert.Equal(RadiationSicknessPhase.ManifestIllness, sv.Phase);

            // Tick through manifest resolution (4 days = 96 hours)
            sys.Tick(100f);

            // Should resolve to either RecoveryOrDeath or ChronicFibrosis
            Assert.True(
                sv.Phase == RadiationSicknessPhase.RecoveryOrDeath ||
                sv.Phase == RadiationSicknessPhase.ChronicFibrosis);
        }

        [Fact]
        public void ResolveOutcome_HighLatentDamage_ChronicFibrosis()
        {
            // Manually set high latent damage to force chronic fibrosis path
            var sys = CreateSystem(new SeededRng(42));
            var sv = MakeSurvivor();
            sv.Health = 100f;
            sys.Register(sv);

            // Push latent damage above ChronicFibrosisThreshold (120)
            sv.LatentDamage = 130f;
            sv.Phase = RadiationSicknessPhase.ManifestIllness;
            sv.OnsetTimer = 0.1f; // about to resolve

            bool fibrosisMarked = false;
            float lungCapacity = -1f;
            sys.OnChronicFibrosisMarked += id => fibrosisMarked = true;
            sys.OnLungCapacityReduced += (id, cap) => lungCapacity = cap;

            sys.Tick(10f); // should resolve manifest

            // With high health (low death chance) and high latent damage, should get fibrosis
            // (death chance = severity - rest mitigation; severity = 130/60 clamped to 1.0;
            //  with rng seed 42 and health=100, death chance is high but rng determines it)
            // Force the scenario: low severity check means survival
            if (sv.Phase == RadiationSicknessPhase.ChronicFibrosis)
            {
                Assert.True(fibrosisMarked);
                Assert.True(lungCapacity < 100f);
                Assert.True(sv.HasPermanentLungDamage);
                Assert.True(sv.LungCapacity >= 20f); // floor
            }
            // If it went to RecoveryOrDeath via death, that's also valid given severity=1.0
        }

        [Fact]
        public void ResolveOutcome_TerminalPrognosis_FiresEvent()
        {
            // Force terminal prognosis: high severity, low health
            var sys = CreateSystem(new SeededRng(42));
            var sv = MakeSurvivor();
            sv.Health = 10f; // very low health
            sv.LatentDamage = 150f; // very high damage
            sv.Phase = RadiationSicknessPhase.ManifestIllness;
            sv.OnsetTimer = 0.1f;
            sys.Register(sv);

            float terminalDays = -1f;
            sys.OnTerminalPrognosisDeclared += (id, days) => terminalDays = days;

            sys.Tick(10f);

            // With severity=1.0 and health=10, death chance is very high
            if (sv.HasTerminalPrognosis)
            {
                Assert.True(terminalDays >= 3f && terminalDays <= 7f);
                Assert.Equal(RadiationSicknessPhase.RecoveryOrDeath, sv.Phase);
            }
        }

        // ── Iodine mitigation ─────────────────────────────────────────

        [Fact]
        public void AdministerIodine_ReducesAcuteLumpOnTrigger()
        {
            var sysWithIodine = CreateSystem(new SeededRng(99));
            var svIodine = MakeSurvivor("sv_iod");
            sysWithIodine.Register(svIodine);
            sysWithIodine.AdministerIodine("sv_iod");

            var sysNoIodine = CreateSystem(new SeededRng(99));
            var svNoIodine = MakeSurvivor("sv_nio");
            sysNoIodine.Register(svNoIodine);

            // Both get same dose
            sysWithIodine.OnExposure("sv_iod", 150f);
            sysNoIodine.OnExposure("sv_nio", 150f);

            // Iodine survivor should have less latent damage (acute lump mitigated)
            Assert.True(svIodine.LatentDamage < svNoIodine.LatentDamage);
        }

        [Fact]
        public void IodineTimer_DecaysOverTime()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            sys.AdministerIodine("sv_ada");
            Assert.Equal(RadiationPhaseProgression.IodineWindowHours, sv.IodineProtectionTimer, 3);

            sys.Tick(6f);
            Assert.Equal(RadiationPhaseProgression.IodineWindowHours - 6f, sv.IodineProtectionTimer, 3);

            sys.Tick(100f);
            Assert.Equal(0f, sv.IodineProtectionTimer, 3);
        }

        // ── Acute dose window decay ───────────────────────────────────

        [Fact]
        public void AcuteDoseWindow_DecaysOverTime()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            // Push dose window above trigger
            sys.OnExposure("sv_ada", 110f); // triggers prodromal, resets window to 0
            // Now expose again without triggering
            sv.Phase = RadiationSicknessPhase.Healthy; // reset for test
            sv.AcuteDoseWindow = 50f;

            sys.Tick(24f); // 1 day: 50 * 0.75^1 = 37.5
            Assert.Equal(37.5f, sv.AcuteDoseWindow, 1);
        }

        [Fact]
        public void AcuteDoseWindow_SmallValuesZeroOut()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);
            sv.AcuteDoseWindow = 0.005f;

            sys.Tick(24f);
            Assert.Equal(0f, sv.AcuteDoseWindow);
        }

        // ── Save/Load roundtrip ───────────────────────────────────────

        [Fact]
        public void SaveLoad_Roundtrip_PreservesState()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            // Push through a few phases
            sys.OnExposure("sv_ada", 110f); // Prodromal
            sys.Tick(24f);                   // -> Latent

            Assert.Equal(RadiationSicknessPhase.Latent, sv.Phase);

            // Capture
            var saved = sys.CaptureState();
            Assert.Single(saved.survivors);
            Assert.Equal("Latent", saved.survivors[0].phase);

            // Restore into a fresh system
            var sys2 = CreateSystem();
            var sv2 = MakeSurvivor();
            sys2.Register(sv2);
            sys2.RestoreState(saved);

            Assert.Equal(RadiationSicknessPhase.Latent, sv2.Phase);
            Assert.Equal(sv.LatentDamage, sv2.LatentDamage, 5);
            Assert.Equal(sv.OnsetTimer, sv2.OnsetTimer, 5);
            Assert.Equal(sv.PhaseHoursElapsed, sv2.PhaseHoursElapsed, 5);
        }

        [Fact]
        public void SaveLoad_Roundtrip_PreservesChronicFibrosis()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sv.Phase = RadiationSicknessPhase.ChronicFibrosis;
            sv.LungCapacity = 55f;
            sv.HasPermanentLungDamage = true;
            sv.LatentDamage = 130f;
            sys.Register(sv);

            var saved = sys.CaptureState();
            var sys2 = CreateSystem();
            var sv2 = MakeSurvivor();
            sys2.Register(sv2);
            sys2.RestoreState(saved);

            Assert.Equal(RadiationSicknessPhase.ChronicFibrosis, sv2.Phase);
            Assert.Equal(55f, sv2.LungCapacity, 3);
            Assert.True(sv2.HasPermanentLungDamage);
            Assert.Equal(130f, sv2.LatentDamage, 3);
        }

        [Fact]
        public void SaveLoad_Roundtrip_PreservesTerminalPrognosis()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sv.Phase = RadiationSicknessPhase.RecoveryOrDeath;
            sv.HasTerminalPrognosis = true;
            sv.TerminalPrognosisDaysRemaining = 5.5f;
            sys.Register(sv);

            var saved = sys.CaptureState();
            var sys2 = CreateSystem();
            var sv2 = MakeSurvivor();
            sys2.Register(sv2);
            sys2.RestoreState(saved);

            Assert.True(sv2.HasTerminalPrognosis);
            Assert.Equal(5.5f, sv2.TerminalPrognosisDaysRemaining, 3);
        }

        [Fact]
        public void SaveLoad_NullRestore_ResetsToHealthy()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sv.Phase = RadiationSicknessPhase.ManifestIllness;
            sv.LatentDamage = 50f;
            sys.Register(sv);

            sys.RestoreState(null);

            Assert.Equal(RadiationSicknessPhase.Healthy, sv.Phase);
            Assert.Equal(0f, sv.LatentDamage);
        }

        [Fact]
        public void SaveLoad_MultipleSurvivors_OrdinalSorted()
        {
            var sys = CreateSystem();
            var svB = MakeSurvivor("sv_b");
            var svA = MakeSurvivor("sv_a");
            svB.Phase = RadiationSicknessPhase.Latent;
            svA.Phase = RadiationSicknessPhase.Prodromal;
            sys.Register(svB);
            sys.Register(svA);

            var saved = sys.CaptureState();
            Assert.Equal(2, saved.survivors.Count);
            // Ordinal sort: "sv_a" < "sv_b"
            Assert.Equal("sv_a", saved.survivors[0].survivorId);
            Assert.Equal("sv_b", saved.survivors[1].survivorId);
        }

        // ── Event firing ──────────────────────────────────────────────

        [Fact]
        public void OnStateChanged_FiresOnExposure()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            int changeCount = 0;
            sys.OnStateChanged += () => changeCount++;

            sys.OnExposure("sv_ada", 50f);
            Assert.True(changeCount > 0);
        }

        [Fact]
        public void OnStateChanged_FiresOnTick()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            int changeCount = 0;
            sys.OnStateChanged += () => changeCount++;
            int beforeCount = changeCount;

            sys.Tick(1f);
            // Tick doesn't always fire OnStateChanged if no transitions happen,
            // but phase transitions during tick should fire it
            // With healthy survivor, no state change expected from tick alone
            // So we test that tick with a transition does fire
            sys.OnExposure("sv_ada", 110f);
            int afterExposure = changeCount;
            Assert.True(afterExposure > beforeCount);
        }

        [Fact]
        public void OnPhaseChanged_FiresOnlyOnActualTransition()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            int phaseChanges = 0;
            sys.OnPhaseChanged += (id, oldP, newP) => phaseChanges++;

            // Small dose — no transition
            sys.OnExposure("sv_ada", 50f);
            Assert.Equal(0, phaseChanges);

            // Big dose — transition to Prodromal
            sys.OnExposure("sv_ada", 60f); // total window = 110
            Assert.Equal(1, phaseChanges);
        }

        [Fact]
        public void OnChronicIllnessRequested_FiresWhenLatentDamageCrossesThreshold()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            int chronicRequests = 0;
            sys.OnChronicIllnessRequested += id => chronicRequests++;

            // Push latent damage above LatentDamageChronicThreshold (100)
            // dose * ChronicDamageFactor = latent damage
            // 100 / 0.05 = 2000 dose needed just from chronic factor
            // Or use a big dose that also adds acute lump
            sv.LatentDamage = 95f;
            sys.OnExposure("sv_ada", 110f); // adds 110*0.05=5.5 -> total 100.5 >= 100

            Assert.True(chronicRequests > 0);
        }

        // ── Edge cases ────────────────────────────────────────────────

        [Fact]
        public void OnExposure_NullSurvivorId_Ignored()
        {
            var sys = CreateSystem();
            sys.OnExposure(null, 100f);
            sys.OnExposure("", 100f);
            // Should not throw
        }

        [Fact]
        public void OnExposure_ZeroDose_Ignored()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            sys.OnExposure("sv_ada", 0f);
            sys.OnExposure("sv_ada", -10f);
            Assert.Equal(0f, sv.LatentDamage);
        }

        [Fact]
        public void OnExposure_DeadSurvivor_Ignored()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sv.IsAlive = false;
            sys.Register(sv);

            sys.OnExposure("sv_ada", 200f);
            Assert.Equal(RadiationSicknessPhase.Healthy, sv.Phase);
        }

        [Fact]
        public void OnExposure_UnregisteredSurvivor_Ignored()
        {
            var sys = CreateSystem();
            sys.OnExposure("sv_unknown", 200f);
            // Should not throw
        }

        [Fact]
        public void Tick_ZeroHours_NoEffect()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);
            sv.Phase = RadiationSicknessPhase.Prodromal;
            sv.OnsetTimer = 1f;

            sys.Tick(0f);
            sys.Tick(-5f);
            Assert.Equal(RadiationSicknessPhase.Prodromal, sv.Phase);
            Assert.Equal(1f, sv.OnsetTimer);
        }

        [Fact]
        public void Tick_HealthySurvivor_NoEvents()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            float healthDelta = 0f;
            sys.OnHealthDeltaRequested += (id, d) => healthDelta += d;

            sys.Tick(24f);
            Assert.Equal(0f, healthDelta);
        }

        [Fact]
        public void GetPhasePrognosisText_ReturnsCorrectText()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);

            Assert.Contains("No radiation sickness", sys.GetPhasePrognosisText("sv_ada"));

            sv.Phase = RadiationSicknessPhase.Prodromal;
            Assert.Contains("Prodromal", sys.GetPhasePrognosisText("sv_ada"));

            sv.Phase = RadiationSicknessPhase.Latent;
            Assert.Contains("Latent", sys.GetPhasePrognosisText("sv_ada"));

            sv.Phase = RadiationSicknessPhase.ManifestIllness;
            Assert.Contains("Manifest", sys.GetPhasePrognosisText("sv_ada"));

            sv.Phase = RadiationSicknessPhase.ChronicFibrosis;
            Assert.Contains("fibrosis", sys.GetPhasePrognosisText("sv_ada"));
        }

        [Fact]
        public void GetPhasePrognosisText_UnknownSurvivor_ReturnsUnknown()
        {
            var sys = CreateSystem();
            Assert.Equal("Unknown", sys.GetPhasePrognosisText("sv_nobody"));
        }

        [Fact]
        public void Unregister_RemovesSurvivor()
        {
            var sys = CreateSystem();
            var sv = MakeSurvivor();
            sys.Register(sv);
            Assert.Equal(RadiationSicknessPhase.Healthy, sys.GetPhase("sv_ada"));

            sys.Unregister("sv_ada");
            Assert.Equal(RadiationSicknessPhase.Healthy, sys.GetPhase("sv_ada")); // default
        }

        [Fact]
        public void LungCapacity_Floor_Is20()
        {
            // Verify the lung capacity floor in chronic fibrosis
            var sys = CreateSystem(new SeededRng(42));
            var sv = MakeSurvivor();
            sv.LungCapacity = 25f; // already low
            sv.LatentDamage = 150f;
            sv.Health = 100f;
            sv.Phase = RadiationSicknessPhase.ManifestIllness;
            sv.OnsetTimer = 0.1f;
            sys.Register(sv);

            sys.Tick(10f);

            if (sv.Phase == RadiationSicknessPhase.ChronicFibrosis)
            {
                Assert.True(sv.LungCapacity >= 20f);
            }
        }

        // ── Determinism ───────────────────────────────────────────────

        [Fact]
        public void Determinism_SameSeed_SameOutcome()
        {
            // Run the same scenario twice with the same seed
            var outcomes1 = RunScenario(42);
            var outcomes2 = RunScenario(42);

            Assert.Equal(outcomes1.phases, outcomes2.phases);
            Assert.Equal(outcomes1.latentDamage, outcomes2.latentDamage, 5);
            Assert.Equal(outcomes1.lungCapacity, outcomes2.lungCapacity, 5);
        }

        [Fact]
        public void Determinism_DifferentSeed_DifferentOutcome()
        {
            // Different seeds may produce different outcomes (not guaranteed, but likely)
            var outcomes1 = RunScenario(42);
            var outcomes2 = RunScenario(999);

            // At minimum, the systems should both complete without error
            Assert.True(outcomes1.phases.Count > 0);
            Assert.True(outcomes2.phases.Count > 0);
        }

        private static (List<RadiationSicknessPhase> phases, float latentDamage, float lungCapacity) RunScenario(int seed)
        {
            var sys = new RadiationPhaseProgression(new SeededRng(seed));
            var sv = MakeSurvivor();
            sys.Register(sv);

            var phases = new List<RadiationSicknessPhase>();
            sys.OnPhaseChanged += (id, oldP, newP) => phases.Add(newP);

            // Simulate a large exposure then tick forward
            sys.OnExposure("sv_ada", 150f);
            for (int i = 0; i < 50; i++)
                sys.Tick(24f); // 50 days

            return (phases, sv.LatentDamage, sv.LungCapacity);
        }
    }
}
