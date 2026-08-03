using System;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for the delayed latent-damage / prognosis pipeline layered on top
    /// of RadiationSystem: the Healthy -> Prodromal -> Latent -> Manifest -> RecoveryOrDeath
    /// staged timeline for a big single exposure, the iodine protection timing window, and
    /// save/load fidelity of prognosis state mid-Latent.
    /// </summary>
    [TestFixture]
    public class PrognosisTests
    {
        private const float Eps = 1e-3f;

        private static NeedsSystem NewNeedsSystem()
        {
            return new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
        }

        private static void TickDays(RadiationSystem rad, Survivor survivor, float days)
        {
            const float hoursPerCall = 1f;
            float totalHours = days * 24f;
            for (float t = 0f; t < totalHours - 1e-4f; t += hoursPerCall)
            {
                rad.Tick(1f);
            }
        }

        // ---- 1-shot high dose: staged timeline ----

        [Test]
        public void OneShotHighDose_HealthFineEarly_ThenCrashesAtManifest()
        {
            var needs = NewNeedsSystem();
            var rad = new RadiationSystem(needs, rng: new Random(12345));
            var survivor = new Survivor { Id = "s1" };
            rad.Register(survivor);

            // One severe exposure on day 0 (150 dose in one hour -- well above the
            // Prodromal trigger), then nothing else ever again.
            rad.Expose(survivor, radsPerHour: 150f, hours: 1f);

            Assert.AreEqual(PrognosisStage.Prodromal, survivor.PrognosisStage);
            Assert.Greater(survivor.Needs.Health, 80f, "Small nausea dip only -- not a crash yet.");

            // Days 0..2: still fine (Prodromal -> Latent, "feels fine" horror window).
            TickDays(rad, survivor, 2f);
            Assert.IsTrue(survivor.PrognosisStage == PrognosisStage.Latent || survivor.PrognosisStage == PrognosisStage.Prodromal);
            Assert.Greater(survivor.Needs.Health, 80f, "Health must still read fine during days 0-2.");
            Assert.IsTrue(survivor.IsAlive);

            // Advance to just before Manifest should hit (day ~7.25 with our tuning).
            TickDays(rad, survivor, 5f); // now at day 7
            Assert.AreEqual(PrognosisStage.Latent, survivor.PrognosisStage, "Should still be silently Latent just before onset.");
            Assert.Greater(survivor.Needs.Health, 80f, "No health warning while still in Latent.");

            // Cross the onset boundary.
            TickDays(rad, survivor, 1f); // now at day 8
            Assert.AreEqual(PrognosisStage.Manifest, survivor.PrognosisStage, "Onset timer elapsed -- should have crashed into Manifest.");
            Assert.LessOrEqual(survivor.Needs.Health, 10f, "Manifest onset must crash health hard.");
            Assert.IsTrue(survivor.HasStatus(SurvivorStatus.AcuteRadiationSyndrome));
        }

        // ---- Acceptance scenario: dose day 3, silent 4-9, dead day 11 ----

        [Test]
        public void Acceptance_DoseOnDay3_SilentUntilDay9_DeadOnDay11()
        {
            var needs = NewNeedsSystem();
            var rad = new RadiationSystem(needs, rng: new Random(999));
            var survivor = new Survivor { Id = "s1" };
            rad.Register(survivor);

            TickDays(rad, survivor, 3f); // days 0-2 pass with no exposure at all
            rad.Expose(survivor, radsPerHour: 150f, hours: 1f); // the single dose, on day 3

            for (int day = 4; day <= 9; day++)
            {
                TickDays(rad, survivor, 1f);
                Assert.IsTrue(survivor.IsAlive, $"Should still be alive on day {day}.");
                Assert.Greater(survivor.Needs.Health, 80f, $"No health warning expected on day {day}.");
            }

            TickDays(rad, survivor, 2f); // day 9 -> day 11

            Assert.IsFalse(survivor.IsAlive, "Survivor should have died by day 11.");
        }

        // ---- Iodine timing window ----

        [Test]
        public void IodineWithinWindow_ReducesLatentDamage_ComparedToNoIodine()
        {
            var needsA = NewNeedsSystem();
            var radA = new RadiationSystem(needsA, rng: new Random(1));
            var survivorA = new Survivor { Id = "protected" };
            radA.Register(survivorA);
            radA.AdministerIodine(survivorA);
            radA.Expose(survivorA, radsPerHour: 150f, hours: 1f);

            var needsB = NewNeedsSystem();
            var radB = new RadiationSystem(needsB, rng: new Random(1));
            var survivorB = new Survivor { Id = "unprotected" };
            radB.Register(survivorB);
            radB.Expose(survivorB, radsPerHour: 150f, hours: 1f);

            Assert.Less(survivorA.LatentDamage, survivorB.LatentDamage,
                "Iodine taken before/at exposure should meaningfully reduce locked-in latent damage.");
        }

        [Test]
        public void IodineThreeDaysLate_DoesNotReduceLatentDamage()
        {
            var needsLate = NewNeedsSystem();
            var radLate = new RadiationSystem(needsLate, rng: new Random(1));
            var survivorLate = new Survivor { Id = "late" };
            radLate.Register(survivorLate);
            radLate.Expose(survivorLate, radsPerHour: 150f, hours: 1f);
            TickDays(radLate, survivorLate, 3f); // 3 days pass, THEN iodine is administered
            radLate.AdministerIodine(survivorLate);

            var needsNone = NewNeedsSystem();
            var radNone = new RadiationSystem(needsNone, rng: new Random(1));
            var survivorNone = new Survivor { Id = "none" };
            radNone.Register(survivorNone);
            radNone.Expose(survivorNone, radsPerHour: 150f, hours: 1f);
            TickDays(radNone, survivorNone, 3f);

            Assert.AreEqual(survivorNone.LatentDamage, survivorLate.LatentDamage, Eps,
                "Iodine administered 3 days after the dose already landed should do essentially nothing.");
        }

        // ---- Save/load mid-Latent ----

        [Test]
        public void SaveLoad_MidLatent_PreservesOnsetTimerExactly()
        {
            var needs = NewNeedsSystem();
            var rad = new RadiationSystem(needs, rng: new Random(7));
            var survivor = new Survivor { Id = "s1" };
            rad.Register(survivor);

            rad.Expose(survivor, radsPerHour: 150f, hours: 1f);
            TickDays(rad, survivor, 1.5f); // past the Prodromal->Latent transition

            Assert.AreEqual(PrognosisStage.Latent, survivor.PrognosisStage, "Precondition: must be Latent before saving.");
            float onsetBefore = survivor.OnsetTimer;
            float latentDamageBefore = survivor.LatentDamage;
            float acuteWindowBefore = survivor.AcuteDoseWindow;

            string json = JsonUtility.ToJson(survivor);
            var restored = JsonUtility.FromJson<Survivor>(json);

            Assert.AreEqual(PrognosisStage.Latent, restored.PrognosisStage);
            Assert.AreEqual(onsetBefore, restored.OnsetTimer, Eps);
            Assert.AreEqual(latentDamageBefore, restored.LatentDamage, Eps);
            Assert.AreEqual(acuteWindowBefore, restored.AcuteDoseWindow, Eps);
        }

        // ---- Medical exam hook ----

        [Test]
        public void ExaminePrognosis_ReportsCurrentStageAndOnsetEstimate()
        {
            var needs = NewNeedsSystem();
            var rad = new RadiationSystem(needs, rng: new Random(3));
            var survivor = new Survivor { Id = "s1" };
            rad.Register(survivor);

            rad.Expose(survivor, radsPerHour: 150f, hours: 1f);

            var estimate = rad.ExaminePrognosis(survivor);
            Assert.AreEqual(PrognosisStage.Prodromal, estimate.Stage);
            Assert.AreEqual(survivor.OnsetTimer, estimate.EstimatedDaysToNextStage, Eps);
        }

        // ---- Existing acute/chronic behaviour must remain untouched (additive layer) ----

        [Test]
        public void ExistingAcuteAndChronicThresholds_StillFireAlongsidePrognosis()
        {
            var needs = NewNeedsSystem();
            var rad = new RadiationSystem(needs, rng: new Random(5));
            var survivor = new Survivor { Id = "s1" };
            rad.Register(survivor);

            rad.Expose(survivor, radsPerHour: 85f, hours: 1f);

            Assert.GreaterOrEqual(survivor.RadiationDose, RadiationSystem.AcuteThreshold);
            Assert.IsTrue(survivor.HasAcuteRadiationSickness, "Old instant acute-sickness flag must still fire.");
        }
    }
}
