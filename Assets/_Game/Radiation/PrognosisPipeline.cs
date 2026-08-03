using System;
using UnityEngine;
using AtomicWar._Game.Survivors;
using Random = System.Random;

namespace AtomicWar._Game.Radiation
{
    /// <summary>Medical-exam read-model: only populated when a doctor/medicine check is made.</summary>
    public readonly struct PrognosisEstimate
    {
        public readonly PrognosisStage Stage;
        public readonly float EstimatedDaysToNextStage;

        public PrognosisEstimate(PrognosisStage stage, float estimatedDaysToNextStage)
        {
            Stage = stage;
            EstimatedDaysToNextStage = estimatedDaysToNextStage;
        }
    }

    /// <summary>
    /// The delayed "fallout kills you later" layer on top of RadiationSystem's existing
    /// instant acute/chronic checks (which are left untouched -- this is additive).
    ///
    /// A big single exposure pushes AcuteDoseWindow (a decaying rolling sum, the
    /// clinically relevant quantity) over ProdromalTriggerDose: Healthy -> Prodromal
    /// (small immediate nausea dip) -> Latent (survivor feels fine, needs look normal --
    /// this is the horror) -> Manifest (health crashes, Acute Radiation Syndrome, needs
    /// bleed) -> RecoveryOrDeath (a treatment-modulated curve).
    ///
    /// Every exposure, big or small, also trickles into LatentDamage -- permanent
    /// accumulated tissue damage that never decays. LatentDamage crossing its own
    /// threshold grants Chronic Illness through the SAME GrantStatus call site
    /// RadiationSystem's existing lifetime-rad check already uses, so there is one
    /// unified pipeline (and one idempotent status grant) reachable from either the
    /// acute or the chronic path, not two disconnected systems.
    ///
    /// Iodine is a timing decision, not a stat stick: AdministerIodine starts
    /// IodineProtectionTimer counting down from IodineWindowHours. Mitigation is only
    /// applied to the acute lump added to LatentDamage at the moment a Prodromal
    /// trigger fires -- if the timer has already expired (or iodine hasn't been taken
    /// yet) when that dose lands, the mitigation is permanently missed. Taking iodine
    /// after the triggering dose has already landed cannot retroactively help it.
    /// </summary>
    public static class PrognosisPipeline
    {
        /// <summary>Per-day retention of the rolling acute-dose window (leaky bucket).</summary>
        public const float AcuteDoseWindowDecayPerDay = 0.75f;
        /// <summary>
        /// AcuteDoseWindow at/above this triggers the Healthy -> Prodromal transition. Kept
        /// safely above RadiationSystem.AcuteThreshold (80) so a single short exposure that
        /// only grazes the old instant acute-sickness check does not also double-dip into
        /// Prodromal's health/morale hit.
        /// </summary>
        public const float ProdromalTriggerDose = 100f;

        /// <summary>Immediate health dip on Prodromal onset (nausea).</summary>
        public const float ProdromalHealthDip = 5f;
        /// <summary>Immediate morale dip on Prodromal onset (nausea).</summary>
        public const float ProdromalMoraleDip = 10f;
        /// <summary>Days spent in Prodromal before entering Latent.</summary>
        public const float ProdromalDurationDays = 1f;

        /// <summary>Latent duration (days) at minimum triggering severity.</summary>
        public const float MaxLatencyDays = 12f;
        /// <summary>Latent duration (days) at maximum severity -- worse doses manifest sooner.</summary>
        public const float MinLatencyDays = 6f;
        /// <summary>LatentDamage at/above this reads as maximum severity (1.0) for latency/crash curves.</summary>
        public const float LatentDamageSeverityReference = 60f;

        /// <summary>Health lost on Manifest onset at minimum severity.</summary>
        public const float ManifestHealthCrashMin = 30f;
        /// <summary>Health lost on Manifest onset at maximum severity (exceeds any remaining health -- lethal).</summary>
        public const float ManifestHealthCrashMax = 150f;
        /// <summary>Days in Manifest before RecoveryOrDeath resolves, absent an earlier lethal crash.</summary>
        public const float ManifestResolutionDays = 4f;
        /// <summary>Base health lost per day while in Manifest, scaled by severity.</summary>
        public const float ManifestBleedPerDay = 8f;
        /// <summary>Fraction of death chance removed by bed rest (SurvivorState.Resting) during Manifest.</summary>
        public const float BedRestMitigation = 0.6f;
        /// <summary>Delta guaranteed to zero out Health regardless of current value (single-writer death).</summary>
        private const float LethalHealthLoss = 100f;

        /// <summary>Fraction of every dose (big or small) that trickles into permanent LatentDamage.</summary>
        public const float ChronicDamageFactor = 0.05f;
        /// <summary>Fraction of the dose above ProdromalTriggerDose added to LatentDamage as an acute lump.</summary>
        public const float AcuteLatentDamageFactor = 1f;
        /// <summary>LatentDamage at/above this ALSO grants Chronic Illness (unified with the lifetime-rad path).</summary>
        public const float LatentDamageChronicThreshold = 100f;

        /// <summary>Hours of iodine protection window granted per administration.</summary>
        public const float IodineWindowHours = 12f;
        /// <summary>Multiplier applied to the acute LatentDamage lump if iodine is active at trigger time.</summary>
        public const float IodineMitigationFactor = 0.35f;

        /// <summary>
        /// Feed a just-applied dose (rads, i.e. radsPerHour * hours) into the rolling window and
        /// permanent latent damage, and evaluate whether it crosses a stage/status boundary.
        /// Call once per Expose() with a positive dose.
        /// </summary>
        public static void OnExposure(Survivor survivor, float dose, NeedsSystem needsSystem, Action<Survivor, SurvivorStatus> grantStatus)
        {
            if (survivor == null || dose <= 0f || needsSystem == null || grantStatus == null)
            {
                return;
            }

            survivor.AcuteDoseWindow += dose;
            survivor.LatentDamage += dose * ChronicDamageFactor;

            bool canTrigger = survivor.PrognosisStage == PrognosisStage.Healthy
                || (survivor.PrognosisStage == PrognosisStage.RecoveryOrDeath && survivor.IsAlive);
            if (canTrigger && survivor.AcuteDoseWindow >= ProdromalTriggerDose)
            {
                TriggerProdromal(survivor, needsSystem);
            }

            if (!survivor.HasChronicIllness && survivor.LatentDamage >= LatentDamageChronicThreshold)
            {
                grantStatus(survivor, SurvivorStatus.ChronicIllness);
            }
        }

        /// <summary>Advance the rolling window decay, iodine timer, and stage timers by elapsed game hours.</summary>
        public static void Tick(Survivor survivor, float gameHours, NeedsSystem needsSystem, Random rng, Action<Survivor, SurvivorStatus> grantStatus)
        {
            if (survivor == null || !survivor.IsAlive || gameHours <= 0f || needsSystem == null)
            {
                return;
            }

            float days = gameHours / 24f;

            survivor.AcuteDoseWindow *= Mathf.Pow(AcuteDoseWindowDecayPerDay, days);
            if (survivor.AcuteDoseWindow < 0.01f)
            {
                survivor.AcuteDoseWindow = 0f;
            }

            if (survivor.IodineProtectionTimer > 0f)
            {
                survivor.IodineProtectionTimer = Mathf.Max(0f, survivor.IodineProtectionTimer - gameHours);
            }

            switch (survivor.PrognosisStage)
            {
                case PrognosisStage.Prodromal:
                    survivor.OnsetTimer -= days;
                    if (survivor.OnsetTimer <= 0f)
                    {
                        EnterLatent(survivor);
                    }
                    break;

                case PrognosisStage.Latent:
                    survivor.OnsetTimer -= days;
                    if (survivor.OnsetTimer <= 0f)
                    {
                        EnterManifest(survivor, needsSystem, grantStatus);
                    }
                    break;

                case PrognosisStage.Manifest:
                    TickManifest(survivor, days, needsSystem, rng);
                    break;
            }
        }

        /// <summary>
        /// Medical-exam hook: the only way an onset estimate is revealed. Callers (a doctor
        /// action, a medical exam UI) call this on demand -- it is never pushed automatically,
        /// unlike the dosimeter's exposure-rate readout.
        /// </summary>
        public static PrognosisEstimate Examine(Survivor survivor)
        {
            if (survivor == null)
            {
                return new PrognosisEstimate(PrognosisStage.Healthy, 0f);
            }
            return new PrognosisEstimate(survivor.PrognosisStage, Mathf.Max(0f, survivor.OnsetTimer));
        }

        private static void TriggerProdromal(Survivor survivor, NeedsSystem needsSystem)
        {
            float overflow = survivor.AcuteDoseWindow - ProdromalTriggerDose;
            float lump = Mathf.Max(0f, overflow) * AcuteLatentDamageFactor;
            if (survivor.IodineProtectionTimer > 0f)
            {
                lump *= IodineMitigationFactor;
            }
            survivor.LatentDamage += lump;

            survivor.PrognosisStage = PrognosisStage.Prodromal;
            survivor.OnsetTimer = ProdromalDurationDays;

            // The acute dose reading itself has been dealt with (treated / metabolized) --
            // only LatentDamage carries the injury forward. Without this, RadiationSystem's
            // pre-existing RadiationDose >= AcuteThreshold check (which never decays on its
            // own) would keep re-firing HealthLossPerHourAtAcute on every subsequent tick,
            // draining the survivor to zero well before Latent ever gets a chance to run --
            // defeating the entire "feels fine during Latent" premise of this pipeline.
            survivor.RadiationDose = 0f;

            needsSystem.Modify(survivor, NeedKind.Health, -ProdromalHealthDip);
            needsSystem.Modify(survivor, NeedKind.Morale, -ProdromalMoraleDip);
        }

        private static void EnterLatent(Survivor survivor)
        {
            float severity = ComputeSeverity(survivor);
            survivor.PrognosisStage = PrognosisStage.Latent;
            survivor.OnsetTimer = Mathf.Lerp(MaxLatencyDays, MinLatencyDays, severity);
        }

        private static void EnterManifest(Survivor survivor, NeedsSystem needsSystem, Action<Survivor, SurvivorStatus> grantStatus)
        {
            float severity = ComputeSeverity(survivor);
            survivor.PrognosisStage = PrognosisStage.Manifest;
            survivor.OnsetTimer = ManifestResolutionDays;
            grantStatus(survivor, SurvivorStatus.AcuteRadiationSyndrome);

            float crash = Mathf.Lerp(ManifestHealthCrashMin, ManifestHealthCrashMax, severity);
            needsSystem.Modify(survivor, NeedKind.Health, -crash);
        }

        private static void TickManifest(Survivor survivor, float days, NeedsSystem needsSystem, Random rng)
        {
            if (!survivor.IsAlive)
            {
                return;
            }

            float severity = ComputeSeverity(survivor);
            bool resting = survivor.State == SurvivorState.Resting;
            float bleedPerDay = ManifestBleedPerDay * severity * (resting ? (1f - BedRestMitigation) : 1f);
            needsSystem.Modify(survivor, NeedKind.Health, -bleedPerDay * days);
            if (!survivor.IsAlive)
            {
                return;
            }

            survivor.OnsetTimer -= days;
            if (survivor.OnsetTimer <= 0f)
            {
                ResolveOutcome(survivor, severity, resting, needsSystem, rng);
            }
        }

        private static void ResolveOutcome(Survivor survivor, float severity, bool resting, NeedsSystem needsSystem, Random rng)
        {
            float deathChance = Mathf.Clamp01(severity - (resting ? BedRestMitigation : 0f));
            var random = rng ?? new Random();
            bool dies = random.NextDouble() < deathChance;

            survivor.PrognosisStage = PrognosisStage.RecoveryOrDeath;
            if (dies)
            {
                needsSystem.Modify(survivor, NeedKind.Health, -LethalHealthLoss);
            }
        }

        private static float ComputeSeverity(Survivor survivor)
        {
            return Mathf.Clamp01(survivor.LatentDamage / LatentDamageSeverityReference);
        }
    }
}
