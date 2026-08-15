using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Guilt-Driven Insomnia System — ruthless decisions (ration cutting,
    /// leaving companions behind) create GuiltRecords that multiply sleep
    /// quality penalties. Sedatives or interpersonal dialogue can compensate.
    ///
    /// Plain C#, leaf assembly. Host injects sleep quality hook and dialogue
    /// resolution callbacks.
    /// </summary>
    public class GuiltInsomniaSystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float SleepQualityPenaltyPerSeverity = 0.50f;
        public const float SedativeCompensationHours = 12f;
        public const float SedativeSeverityReduction = 0.40f;
        public const float DialogueSeverityReduction = 0.25f;
        public const float NaturalDecayPerDay = 0.05f;
        public const float HighSeverityThreshold = 0.7f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<Survivor, GuiltRecord> OnGuiltRecorded;
        public event Action<Survivor> OnGuiltResolved;
        public event Action<Survivor> OnGuiltInsomniaCritical;

        // ── Host hooks ─────────────────────────────────────────────────
        public Func<float> GetDay;
        public System.Random Rng;
        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        /// <summary>
        /// Record a new guilt source for a survivor.
        /// </summary>
        public void RecordGuilt(Survivor sv, string sourceId, float severity)
        {
            if (sv == null || !sv.IsAlive) return;
            if (severity <= 0f) return;

            int day = Math.Max(1, (int)(GetDay?.Invoke() ?? 1));
            sv.GuiltSources.Add(new GuiltRecord(sourceId, day, severity));
            UpdateInsomniaSeverity(sv);
            OnGuiltRecorded?.Invoke(sv, sv.GuiltSources[sv.GuiltSources.Count - 1]);
        }

        /// <summary>
        /// Apply a sedative to temporarily compensate guilt insomnia.
        /// </summary>
        public bool ApplySedative(Survivor sv)
        {
            if (sv == null || sv.GuiltInsomniaSeverity <= 0f) return false;
            sv.SedativeCompensationHours = SedativeCompensationHours;
            float oldSeverity = sv.GuiltInsomniaSeverity;
            sv.GuiltInsomniaSeverity = Math.Max(0f,
                sv.GuiltInsomniaSeverity - SedativeSeverityReduction);
            return sv.GuiltInsomniaSeverity < oldSeverity;
        }

        /// <summary>
        /// Resolve guilt through interpersonal dialogue.
        /// </summary>
        public bool ResolveGuiltThroughDialogue(Survivor sv)
        {
            if (sv == null || sv.GuiltSources.Count == 0) return false;
            // Remove the most recent guilt source
            sv.GuiltSources.RemoveAt(sv.GuiltSources.Count - 1);
            UpdateInsomniaSeverity(sv);
            if (sv.GuiltSources.Count == 0)
                OnGuiltResolved?.Invoke(sv);
            return true;
        }

        /// <summary>
        /// Get the effective sleep quality multiplier for this survivor.
        /// 1.0 = no penalty, lower = worse sleep.
        /// </summary>
        public float GetSleepQualityMultiplier(Survivor sv)
        {
            if (sv == null) return 1f;
            float penalty = sv.GuiltInsomniaSeverity * SleepQualityPenaltyPerSeverity;
            if (sv.SedativeCompensationHours > 0f)
                penalty *= 0.5f;
            return Math.Max(0.1f, 1f - penalty);
        }

        /// <summary>
        /// Tick — decay sedative compensation and natural guilt decay.
        /// </summary>
        public void Tick(Survivor sv, float gameHours, int currentDay)
        {
            if (sv == null || !sv.IsAlive) return;

            // Decay sedative
            if (sv.SedativeCompensationHours > 0f)
            {
                sv.SedativeCompensationHours -= gameHours;
                if (sv.SedativeCompensationHours <= 0f)
                {
                    sv.SedativeCompensationHours = 0f;
                    UpdateInsomniaSeverity(sv); // re-calc without sedative
                }
            }

            // Natural decay (daily)
            if (sv.GuiltSources.Count > 0)
            {
                // Remove guilt sources older than 30 days
                for (int i = sv.GuiltSources.Count - 1; i >= 0; i--)
                {
                    if (currentDay - sv.GuiltSources[i].DayRecorded > 30)
                        sv.GuiltSources.RemoveAt(i);
                }
                if (sv.GuiltSources.Count == 0)
                {
                    sv.GuiltInsomniaSeverity = 0f;
                    OnGuiltResolved?.Invoke(sv);
                }
                else
                {
                    UpdateInsomniaSeverity(sv);
                }
            }
        }

        private void UpdateInsomniaSeverity(Survivor sv)
        {
            float total = 0f;
            for (int i = 0; i < sv.GuiltSources.Count; i++)
                total += sv.GuiltSources[i].Severity;
            sv.GuiltInsomniaSeverity = Math.Min(1f, total);
            if (sv.GuiltInsomniaSeverity >= HighSeverityThreshold)
                OnGuiltInsomniaCritical?.Invoke(sv);
        }
    }
}
