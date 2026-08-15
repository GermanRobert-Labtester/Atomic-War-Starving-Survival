using System;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Integration tests for the RadiationPhaseProgression system.
    /// Verifies: phase sync from PrognosisPipeline, ChronicFibrosis
    /// permanent lung damage, terminal prognosis declaration,
    /// and the full Prodromal→Latent→Manifest→Fibrosis pipeline.
    /// </summary>
    [TestFixture]
    public class RadiationPhaseProgressionTests
    {
        private const float Eps = 1e-3f;

        private static NeedsSystem NewNeedsSystem()
        {
            return new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
        }

        [Test]
        public void PhaseSync_HealthyToProdromal_UpdatesSicknessPhase()
        {
            var phaseProg = new RadiationPhaseProgression { Rng = new Random(42) };
            var survivor = new Survivor { Id = "s1" };

            Assert.AreEqual(RadiationSicknessPhase.Healthy, survivor.SicknessPhase);

            // Simulate PrognosisPipeline setting PrognosisStage to Prodromal
            survivor.PrognosisStage = PrognosisStage.Prodromal;
            phaseProg.Tick(survivor, 1f);

            Assert.AreEqual(RadiationSicknessPhase.Prodromal, survivor.SicknessPhase);
        }

        [Test]
        public void PhaseSync_LatentToManifest_UpdatesSicknessPhase()
        {
            var phaseProg = new RadiationPhaseProgression { Rng = new Random(42) };
            var survivor = new Survivor { Id = "s1" };

            survivor.PrognosisStage = PrognosisStage.Latent;
            survivor.SicknessPhase = RadiationSicknessPhase.Prodromal;
            phaseProg.Tick(survivor, 1f);

            Assert.AreEqual(RadiationSicknessPhase.Latent, survivor.SicknessPhase);

            // Now simulate Manifest
            survivor.PrognosisStage = PrognosisStage.Manifest;
            phaseProg.Tick(survivor, 1f);

            Assert.AreEqual(RadiationSicknessPhase.ManifestIllness, survivor.SicknessPhase);
        }

        [Test]
        public void ManifestResolve_HighLatentDamage_TriggersChronicFibrosis()
        {
            var phaseProg = new RadiationPhaseProgression
            {
                Rng = new Random(42),
                ApplyHealthDelta = (sv, d) => { if (sv?.Needs != null) sv.Needs.Health += d; },
                ApplyMoraleDelta = (sv, d) => { if (sv?.Needs != null) sv.Needs.Morale += d; },
                MarkChronicFibrosis = sv => { sv.HasPermanentLungDamage = true; }
            };
            var survivor = new Survivor
            {
                Id = "s1",
                LatentDamage = RadiationPhaseProgression.ChronicFibrosisThreshold + 10f
            };
            survivor.Needs.Health = 50f;
            survivor.SicknessPhase = RadiationSicknessPhase.ManifestIllness;
            survivor.PrognosisStage = PrognosisStage.RecoveryOrDeath;

            // Simulate tick — SyncPhaseFromPrognosisStage will detect
            // Manifest→RecoveryOrDeath transition and call ResolveManifest
            phaseProg.Tick(survivor, 1f);

            // Should have transitioned to ChronicFibrosis due to high LatentDamage
            Assert.AreEqual(RadiationSicknessPhase.ChronicFibrosis, survivor.SicknessPhase);
            Assert.IsTrue(survivor.HasPermanentLungDamage);
            Assert.Less(survivor.LungCapacity, 100f,
                "Lung capacity should be permanently reduced");
        }

        [Test]
        public void ManifestResolve_LowLatentDamage_TransitionsToRecovery()
        {
            var phaseProg = new RadiationPhaseProgression
            {
                Rng = new Random(42),
                ApplyHealthDelta = (sv, d) => { if (sv?.Needs != null) sv.Needs.Health += d; }
            };
            var survivor = new Survivor
            {
                Id = "s1",
                LatentDamage = 30f // well below ChronicFibrosis threshold
            };
            survivor.Needs.Health = 80f;
            survivor.SicknessPhase = RadiationSicknessPhase.ManifestIllness;
            survivor.PrognosisStage = PrognosisStage.RecoveryOrDeath;

            phaseProg.Tick(survivor, 1f);

            Assert.AreEqual(RadiationSicknessPhase.RecoveryOrDeath, survivor.SicknessPhase);
            Assert.IsFalse(survivor.HasPermanentLungDamage);
        }

        [Test]
        public void ManifestBleed_WhileResting_ReducedByBedRestMitigation()
        {
            float healthDeltaApplied = 0f;
            var phaseProg = new RadiationPhaseProgression
            {
                Rng = new Random(42),
                ApplyHealthDelta = (sv, d) => { healthDeltaApplied += d; }
            };
            var survivor = new Survivor
            {
                Id = "s1",
                State = SurvivorState.Resting
            };
            survivor.SicknessPhase = RadiationSicknessPhase.ManifestIllness;
            survivor.PrognosisStage = PrognosisStage.Manifest;
            survivor.Needs.Health = 100f;

            phaseProg.Tick(survivor, 24f); // 1 full day

            // Expected bleed: 8 * (1 - 0.6) = 3.2
            Assert.Less(Math.Abs(healthDeltaApplied - (-3.2f)), 1f,
                $"Resting bleed should be ~3.2, got {healthDeltaApplied}");
        }

        [Test]
        public void ManifestBleed_NotResting_FullBleedApplied()
        {
            float healthDeltaApplied = 0f;
            var phaseProg = new RadiationPhaseProgression
            {
                Rng = new Random(42),
                ApplyHealthDelta = (sv, d) => { healthDeltaApplied += d; }
            };
            var survivor = new Survivor
            {
                Id = "s1",
                State = SurvivorState.Working
            };
            survivor.SicknessPhase = RadiationSicknessPhase.ManifestIllness;
            survivor.PrognosisStage = PrognosisStage.Manifest;
            survivor.Needs.Health = 100f;

            phaseProg.Tick(survivor, 24f);

            // Expected bleed: 8 (no mitigation)
            Assert.Less(Math.Abs(healthDeltaApplied - (-8f)), 1f,
                $"Working bleed should be ~8, got {healthDeltaApplied}");
        }

        [Test]
        public void PhaseHoursElapsed_AccumulatesDuringActivePhase()
        {
            var phaseProg = new RadiationPhaseProgression { Rng = new Random(42) };
            var survivor = new Survivor { Id = "s1" };
            survivor.SicknessPhase = RadiationSicknessPhase.Prodromal;
            survivor.PrognosisStage = PrognosisStage.Prodromal;

            phaseProg.Tick(survivor, 5f);
            Assert.AreEqual(5f, survivor.PhaseHoursElapsed, Eps);

            phaseProg.Tick(survivor, 3f);
            Assert.AreEqual(8f, survivor.PhaseHoursElapsed, Eps);
        }

        [Test]
        public void GetPhasePrognosisText_ReturnsCorrectTextForEachPhase()
        {
            var phaseProg = new RadiationPhaseProgression { Rng = new Random(42) };
            var survivor = new Survivor { Id = "s1" };

            survivor.SicknessPhase = RadiationSicknessPhase.Healthy;
            StringAssert.Contains("No radiation sickness", phaseProg.GetPhasePrognosisText(survivor));

            survivor.SicknessPhase = RadiationSicknessPhase.Prodromal;
            StringAssert.Contains("nausea", phaseProg.GetPhasePrognosisText(survivor).ToLower());

            survivor.SicknessPhase = RadiationSicknessPhase.Latent;
            StringAssert.Contains("appears recovered", phaseProg.GetPhasePrognosisText(survivor).ToLower());

            survivor.SicknessPhase = RadiationSicknessPhase.ManifestIllness;
            StringAssert.Contains("marrow suppression", phaseProg.GetPhasePrognosisText(survivor).ToLower());

            survivor.SicknessPhase = RadiationSicknessPhase.ChronicFibrosis;
            StringAssert.Contains("permanent lung damage", phaseProg.GetPhasePrognosisText(survivor).ToLower());
        }

        [Test]
        public void SyncPhase_DeadSurvivor_NoPhaseChange()
        {
            var phaseProg = new RadiationPhaseProgression { Rng = new Random(42) };
            var survivor = new Survivor
            {
                Id = "s1",
                State = SurvivorState.Dead
            };
            survivor.SicknessPhase = RadiationSicknessPhase.ManifestIllness;
            survivor.PrognosisStage = PrognosisStage.Manifest;

            phaseProg.Tick(survivor, 1f);

            // Dead survivors should not transition
            Assert.AreEqual(RadiationSicknessPhase.ManifestIllness, survivor.SicknessPhase);
        }
    }
}
