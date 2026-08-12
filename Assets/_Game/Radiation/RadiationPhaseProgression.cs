using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Radiation Sickness Phase Progression — the full clinical disease model
    /// layered on top of PrognosisPipeline. Replaces the binary HasAcuteRadiationSickness
    /// with a 5-phase state machine: Healthy → Prodromal → Latent → ManifestIllness →
    /// ChronicFibrosis (or RecoveryOrDeath).
    ///
    /// Owns: Survivor.SicknessPhase, Survivor.PhaseHoursElapsed, Survivor.LungCapacity,
    /// Survivor.HasPermanentLungDamage.
    ///
    /// Save-safe: all state lives on Survivor fields; no internal dictionary needed.
    /// </summary>
    public class RadiationPhaseProgression
    {
        // ── Phase duration constants (game-hours) ──────────────────────
        public const float ProdromalDurationHours = 24f;
        public const float LatentMinDurationHours = 144f;   // 6 days at min severity
        public const float LatentMaxDurationHours = 288f;   // 12 days at max severity
        public const float ManifestMinDurationHours = 72f;  // ~3 days
        public const float ManifestMaxDurationHours = 96f;  // 4 days

        // ── Health impact constants ────────────────────────────────────
        public const float ProdromalHealthDip = 5f;
        public const float ProdromalMoraleDip = 10f;
        public const float ProdromalFatigueRate = 1.5f;
        public const float ManifestHealthCrashMin = 30f;
        public const float ManifestHealthCrashMax = 150f;
        public const float ManifestBleedPerDay = 8f;
        public const float BedRestMitigation = 0.6f;
        public const float ChronicFibrosisLungCapacityMin = 0.40f;
        public const float ChronicFibrosisLungCapacityMax = 0.70f;

        // ── LatentDamage thresholds ────────────────────────────────────
        public const float ChronicFibrosisThreshold = 120f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<Survivor, RadiationSicknessPhase, RadiationSicknessPhase> OnPhaseChanged;
        public event Action<Survivor, float> OnLungCapacityReduced;
        public event Action<Survivor> OnTerminalPrognosisDeclared;

        // ── Host hooks ─────────────────────────────────────────────────
        public Action<Survivor, float> ApplyHealthDelta;
        public Action<Survivor, float> ApplyMoraleDelta;
        public Action<Survivor, string> GrantChronicIllness;
        public Action<Survivor> MarkChronicFibrosis;
        public Func<float> GetDay;
        public System.Random Rng;

        /// <summary>
        /// Feed a just-applied dose into the phase state machine.
        /// Call once per Expose() with a positive dose, AFTER PrognosisPipeline.EvaluateDose.
        /// </summary>
        public void EvaluatePhaseTransition(Survivor sv, float dose, float latentDamage, PrognosisStage prognosisStage)
        {
            if (sv == null || !sv.IsAlive) return;

            // Check if PrognosisPipeline moved us into a new stage
            if (prognosisStage == PrognosisStage.Prodromal &&
                sv.SicknessPhase != RadiationSicknessPhase.Prodromal &&
                sv.SicknessPhase != RadiationSicknessPhase.Latent &&
                sv.SicknessPhase != RadiationSicknessPhase.ManifestIllness &&
                sv.SicknessPhase != RadiationSicknessPhase.ChronicFibrosis)
            {
                TransitionTo(sv, RadiationSicknessPhase.Prodromal);
                if (ApplyHealthDelta != null) ApplyHealthDelta(sv, -ProdromalHealthDip);
                if (ApplyMoraleDelta != null) ApplyMoraleDelta(sv, -ProdromalMoraleDip);
            }
            else if (prognosisStage == PrognosisStage.Manifest &&
                     sv.SicknessPhase != RadiationSicknessPhase.ManifestIllness &&
                     sv.SicknessPhase != RadiationSicknessPhase.ChronicFibrosis)
            {
                float severity = Math.Min(1f, latentDamage / PrognosisPipeline.LatentDamageSeverityReference);
                float crash = Math.Max(ManifestHealthCrashMin,
                    ManifestHealthCrashMin + severity * (ManifestHealthCrashMax - ManifestHealthCrashMin));
                TransitionTo(sv, RadiationSicknessPhase.ManifestIllness);
                if (ApplyHealthDelta != null) ApplyHealthDelta(sv, -crash);

                // Determine latent duration based on severity
                float latentDuration = LatentMaxDurationHours -
                    severity * (LatentMaxDurationHours - LatentMinDurationHours);
                sv.OnsetTimer = latentDuration;
            }
        }

        /// <summary>
        /// Tick the phase state machine. Call once per game-hour substep,
        /// AFTER PrognosisPipeline.Tick has already run (which updates PrognosisStage).
        /// This method syncs SicknessPhase from PrognosisStage and then applies
        /// phase-specific health/morale/stamina effects.
        /// </summary>
        public void Tick(Survivor sv, float gameHours)
        {
            if (sv == null || !sv.IsAlive) return;

            // Sync from PrognosisPipeline's stage (the authoritative source)
            SyncPhaseFromPrognosisStage(sv);

            if (sv.SicknessPhase == RadiationSicknessPhase.Healthy ||
                sv.SicknessPhase == RadiationSicknessPhase.RecoveryOrDeath)
                return;

            sv.PhaseHoursElapsed += gameHours;

            switch (sv.SicknessPhase)
            {
                case RadiationSicknessPhase.Prodromal:
                    // Nausea effects — ongoing health/morale drain
                    if (ApplyHealthDelta != null)
                        ApplyHealthDelta(sv, -ProdromalHealthDip * (gameHours / ProdromalDurationHours) * ProdromalFatigueRate);
                    break;

                case RadiationSicknessPhase.Latent:
                    // Silent phase — survivor feels fine, damage is hidden
                    break;

                case RadiationSicknessPhase.ManifestIllness:
                {
                    // Daily health bleed
                    float bleed = ManifestBleedPerDay * (gameHours / 24f);
                    if (sv.State == SurvivorState.Resting)
                        bleed *= (1f - BedRestMitigation);
                    if (ApplyHealthDelta != null) ApplyHealthDelta(sv, -bleed);
                    break;
                }

                case RadiationSicknessPhase.ChronicFibrosis:
                    // Permanent state — lung capacity stays reduced.
                    // No ongoing damage, just the permanent penalty.
                    break;
            }
        }

        /// <summary>
        /// Sync SicknessPhase to match PrognosisStage (the authoritative source
        /// set by PrognosisPipeline). Also detects Manifest resolution to
        /// trigger ChronicFibrosis or Terminal prognosis.
        /// </summary>
        private void SyncPhaseFromPrognosisStage(Survivor sv)
        {
            var prognosis = sv.PrognosisStage;
            var currentPhase = sv.SicknessPhase;

            RadiationSicknessPhase targetPhase;
            switch (prognosis)
            {
                case PrognosisStage.Healthy:
                    targetPhase = RadiationSicknessPhase.Healthy;
                    break;
                case PrognosisStage.Prodromal:
                    targetPhase = RadiationSicknessPhase.Prodromal;
                    break;
                case PrognosisStage.Latent:
                    targetPhase = RadiationSicknessPhase.Latent;
                    break;
                case PrognosisStage.Manifest:
                    targetPhase = RadiationSicknessPhase.ManifestIllness;
                    break;
                case PrognosisStage.RecoveryOrDeath:
                    // Only resolve once — when transitioning FROM Manifest
                    if (currentPhase == RadiationSicknessPhase.ManifestIllness)
                    {
                        ResolveManifest(sv);
                    }
                    // targetPhase stays as whatever ResolveManifest sets
                    return;
                default:
                    return;
            }

            if (targetPhase != currentPhase)
                TransitionTo(sv, targetPhase);
        }

        private void ResolveManifest(Survivor sv)
        {
            // Death check: if health critically low, chance of death
            float healthRatio = sv.Needs?.Health > 0f ? sv.Needs.Health / 100f : 0f;
            float deathChance = 1f - healthRatio;
            if (Rng != null && Rng.NextDouble() < deathChance)
            {
                // Terminal prognosis — final wish triggers
                float daysRemaining = 3f + (float)(Rng.NextDouble() * 4f); // 3-7 days
                sv.HasTerminalPrognosis = true;
                sv.TerminalPrognosisDaysRemaining = daysRemaining;
                OnTerminalPrognosisDeclared?.Invoke(sv);
                TransitionTo(sv, RadiationSicknessPhase.RecoveryOrDeath);
                return;
            }

            // Chronic fibrosis check
            if (sv.LatentDamage >= ChronicFibrosisThreshold)
            {
                float lungReduction = ChronicFibrosisLungCapacityMin +
                    (float)((Rng?.NextDouble() ?? 0.5) *
                    (ChronicFibrosisLungCapacityMax - ChronicFibrosisLungCapacityMin));
                sv.LungCapacity = Math.Max(sv.LungCapacity * lungReduction, 20f);
                sv.HasPermanentLungDamage = true;
                if (MarkChronicFibrosis != null) MarkChronicFibrosis(sv);
                OnLungCapacityReduced?.Invoke(sv, sv.LungCapacity);
                TransitionTo(sv, RadiationSicknessPhase.ChronicFibrosis);
            }
            else
            {
                TransitionTo(sv, RadiationSicknessPhase.RecoveryOrDeath);
            }
        }

        private void TransitionTo(Survivor sv, RadiationSicknessPhase newPhase)
        {
            if (sv.SicknessPhase == newPhase) return;
            var oldPhase = sv.SicknessPhase;
            sv.SicknessPhase = newPhase;
            sv.PhaseHoursElapsed = 0f;
            OnPhaseChanged?.Invoke(sv, oldPhase, newPhase);
        }

        /// <summary>
        /// Medical exam: return a human-readable estimate of the survivor's
        /// current phase and prognosis. Hidden from player by default; requires
        /// a doctor or medical check to reveal.
        /// </summary>
        public string GetPhasePrognosisText(Survivor sv)
        {
            if (sv == null) return "Unknown";
            return sv.SicknessPhase switch
            {
                RadiationSicknessPhase.Healthy => "No radiation sickness detected.",
                RadiationSicknessPhase.Prodromal => "Prodromal phase: nausea and fatigue. Monitor closely.",
                RadiationSicknessPhase.Latent => "Latent phase: patient appears recovered but damage is progressing internally.",
                RadiationSicknessPhase.ManifestIllness => "Manifest illness: severe symptoms. Marrow suppression, epilation, hemorrhage risk.",
                RadiationSicknessPhase.ChronicFibrosis => "Chronic fibrosis: permanent lung damage. Capacity reduced.",
                RadiationSicknessPhase.RecoveryOrDeath => "Resolution phase: patient is recovering or terminal.",
                _ => "Unknown phase."
            };
        }
    }
}
