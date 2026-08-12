using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Respiratory Degenerative Illness System (Silicosis / Black Lung).
    /// Prolonged exposure to fallout ash and unmaintained HEPA filters causes
    /// progressive coughing and irreversible lung damage. Requires specialized
    /// herbal teas or medical inhalers to manage.
    ///
    /// Owns: Survivor.RespiratoryDegradation, Survivor.RequiresInhaler,
    /// Survivor.InhalerReliefHours.
    ///
    /// Ties into AirFiltration system for HEPA-filter degradation rate.
    /// </summary>
    public class RespiratoryDegenerationSystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float AshExposureDegradationRate = 0.5f;
        // per hour with filter < 50%
        public const float UnmaintainedFilterMultiplier = 2.0f;
        public const float FilterHealthThreshold = 50f;
        public const float DegradationPerDayWithoutFilter = 2f;
        public const float InhalerReliefDurationHours = 8f;
        public const float InhalerDegradationReduction = 10f;
        public const float HerbalTeaDegradationReduction = 3f;
        public const float IrreversibleThreshold = 80f;
        public const float SevereCoughThreshold = 50f;
        public const float SevereCoughStaminaPenalty = 0.30f;
        public const float SevereCoughMoraleDrainPerDay = 3f;
        public const float TerminalLungThreshold = 95f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<Survivor, float> OnRespiratoryDegradationIncreased;
        public event Action<Survivor> OnRequiresInhaler;
        public event Action<Survivor> OnSevereCoughStarted;
        public event Action<Survivor> OnTerminalLungDamage;

        // ── Host hooks ─────────────────────────────────────────────────
        public Func<float> GetFilterHealth;
        // 0..100 from AirFiltration
        public Func<bool> IsInFalloutStorm;
        public Func<bool> IsInAshZone;
        public Action<Survivor, float> ApplyStaminaPenalty;
        public Action<Survivor, float> ApplyMoraleDelta;
        public Func<float> GetDay;
        public System.Random Rng;

        /// <summary>
        /// Tick — accumulate respiratory degradation based on air quality.
        /// </summary>
        public void Tick(Survivor sv, float gameHours)
        {
            if (sv == null || !sv.IsAlive) return;
            if (sv.HasPermanentLungDamage && sv.RespiratoryDegradation >= IrreversibleThreshold)
                return; // already past the point of no return, no further accumulation needed

            float filterHealth = GetFilterHealth?.Invoke() ?? 100f;
            float rate = 0f;

            // Degradation from ash exposure
            if (IsInFalloutStorm?.Invoke() == true)
            {
                rate = AshExposureDegradationRate;
                if (filterHealth < FilterHealthThreshold)
                    rate *= UnmaintainedFilterMultiplier;
            }
            else if (IsInAshZone?.Invoke() == true)
            {
                rate = AshExposureDegradationRate * 0.5f;
            }
            else if (filterHealth < FilterHealthThreshold)
            {
                // Indoor but filter is failing
                rate = DegradationPerDayWithoutFilter / 24f;
            }

            if (rate <= 0f) return;

            float degradation = rate * gameHours;
            float oldValue = sv.RespiratoryDegradation;
            sv.RespiratoryDegradation = Math.Min(100f,
                sv.RespiratoryDegradation + degradation);

            if (sv.RespiratoryDegradation > oldValue)
                OnRespiratoryDegradationIncreased?.Invoke(sv,
                    sv.RespiratoryDegradation - oldValue);

            // Threshold checks
            if (sv.RespiratoryDegradation >= SevereCoughThreshold &&
                oldValue < SevereCoughThreshold)
            {
                OnSevereCoughStarted?.Invoke(sv);
            }

            if (sv.RespiratoryDegradation >= IrreversibleThreshold &&
                oldValue < IrreversibleThreshold)
            {
                sv.HasPermanentLungDamage = true;
                OnRequiresInhaler?.Invoke(sv);
            }

            if (sv.RespiratoryDegradation >= TerminalLungThreshold &&
                oldValue < TerminalLungThreshold)
            {
                OnTerminalLungDamage?.Invoke(sv);
            }

            // Apply ongoing effects
            if (sv.RespiratoryDegradation >= SevereCoughThreshold)
            {
                ApplyStaminaPenalty?.Invoke(sv, SevereCoughStaminaPenalty);
                float moraleDrain = SevereCoughMoraleDrainPerDay * (gameHours / 24f);
                ApplyMoraleDelta?.Invoke(sv, -moraleDrain);
            }

            // Count down inhaler relief
            if (sv.InhalerReliefHours > 0f)
            {
                sv.InhalerReliefHours -= gameHours;
                if (sv.InhalerReliefHours <= 0f)
                    sv.InhalerReliefHours = 0f;
            }
        }

        /// <summary>
        /// Apply a medical inhaler to reduce respiratory degradation and provide relief.
        /// </summary>
        public bool ApplyInhaler(Survivor sv)
        {
            if (sv == null || !sv.IsAlive) return false;
            if (sv.RespiratoryDegradation <= 0f) return false;

            sv.InhalerReliefHours = InhalerReliefDurationHours;
            sv.RespiratoryDegradation = Math.Max(0f,
                sv.RespiratoryDegradation - InhalerDegradationReduction);
            sv.RequiresInhaler = sv.RespiratoryDegradation >= IrreversibleThreshold;
            return true;
        }

        /// <summary>
        /// Apply herbal tea for mild respiratory relief.
        /// </summary>
        public bool ApplyHerbalTea(Survivor sv)
        {
            if (sv == null || !sv.IsAlive) return false;
            if (sv.RespiratoryDegradation <= 0f) return false;

            sv.RespiratoryDegradation = Math.Max(0f,
                sv.RespiratoryDegradation - HerbalTeaDegradationReduction);
            sv.RequiresInhaler = sv.RespiratoryDegradation >= IrreversibleThreshold;
            return true;
        }

        /// <summary>
        /// Get effective stamina multiplier accounting for respiratory damage.
        /// </summary>
        public float GetStaminaMultiplier(Survivor sv)
        {
            if (sv == null) return 1f;
            if (sv.RespiratoryDegradation < SevereCoughThreshold) return 1f;
            if (sv.InhalerReliefHours > 0f) return 1f; // inhaler suppresses symptoms
            return 1f - SevereCoughStaminaPenalty;
        }
    }
}
