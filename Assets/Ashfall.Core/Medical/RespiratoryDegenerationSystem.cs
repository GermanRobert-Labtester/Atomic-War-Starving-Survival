using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>Per-survivor respiratory degradation snapshot.</summary>
    [Serializable]
    public class RespiratorySurvivorState
    {
        public string survivorId = string.Empty;
        public float respiratoryDegradation = 0f;   // 0..100
        public bool hasPermanentLungDamage = false;
        public bool requiresInhaler = false;
        public float inhalerReliefHours = 0f;
    }

    /// <summary>Top-level save snapshot for the respiratory degeneration system.</summary>
    [Serializable]
    public class RespiratoryDegenerationState
    {
        public string systemId = RespiratoryDegenerationSystem.SystemId;
        public List<RespiratorySurvivorState> survivors = new List<RespiratorySurvivorState>();
    }

    /// <summary>
    /// Engine-agnostic port of the Unity RespiratoryDegenerationSystem
    /// (Assets/_Game/Medical/RespiratoryDegenerationSystem.cs): progressive
    /// lung damage from fallout ash exposure and failing HEPA filters.
    /// Survivor-agnostic (operates on string ids); the host subscribes to
    /// effect events to apply stamina/morale penalties in its own domain.
    /// All constants match the Unity source 1:1.
    ///
    /// Owns per-survivor: respiratoryDegradation, hasPermanentLungDamage,
    /// requiresInhaler, inhalerReliefHours.
    /// </summary>
    public class RespiratoryDegenerationSystem
    {
        public const string SystemId = "respiratory_degeneration_system";

        // ── Constants (match Unity source 1:1) ────────────────────────
        public const float AshExposureDegradationRate = 0.5f;
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

        // ── Per-survivor state ────────────────────────────────────────
        private readonly Dictionary<string, RespiratorySurvivorState> _survivors =
            new Dictionary<string, RespiratorySurvivorState>();

        // ── Host hooks (environment queries) ──────────────────────────
        public Func<float> GetFilterHealth;       // 0..100 from AirFiltration
        public Func<bool> IsInFalloutStorm;
        public Func<bool> IsInAshZone;

        // ── Events (hosts apply the effects in their own domain) ──────
        public event Action<string, float> OnRespiratoryDegradationIncreased; // survivorId, delta
        public event Action<string> OnRequiresInhaler;                        // survivorId
        public event Action<string> OnSevereCoughStarted;                     // survivorId
        public event Action<string> OnTerminalLungDamage;                     // survivorId
        public event Action<string, float> OnStaminaPenaltyRequested;         // survivorId, factor
        public event Action<string, float> OnMoraleDrainRequested;            // survivorId, amount
        public event Action OnStateChanged;

        public IReadOnlyDictionary<string, RespiratorySurvivorState> Survivors => _survivors;

        // ── Queries ───────────────────────────────────────────────────

        /// <summary>Get or create the per-survivor record.</summary>
        public RespiratorySurvivorState GetOrCreate(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            if (!_survivors.TryGetValue(survivorId, out var state))
            {
                state = new RespiratorySurvivorState { survivorId = survivorId };
                _survivors[survivorId] = state;
            }
            return state;
        }

        public float RespiratoryDegradation(string survivorId)
        {
            return _survivors.TryGetValue(survivorId, out var s) ? s.respiratoryDegradation : 0f;
        }

        public bool HasPermanentLungDamage(string survivorId)
        {
            return _survivors.TryGetValue(survivorId, out var s) && s.hasPermanentLungDamage;
        }

        public bool RequiresInhaler(string survivorId)
        {
            return _survivors.TryGetValue(survivorId, out var s) && s.requiresInhaler;
        }

        public float InhalerReliefHours(string survivorId)
        {
            return _survivors.TryGetValue(survivorId, out var s) ? s.inhalerReliefHours : 0f;
        }

        /// <summary>
        /// Get effective stamina multiplier accounting for respiratory damage.
        /// Returns 1.0 when no penalty applies, (1 - SevereCoughStaminaPenalty) when severe.
        /// </summary>
        public float GetStaminaMultiplier(string survivorId)
        {
            if (!_survivors.TryGetValue(survivorId, out var s)) return 1f;
            if (s.respiratoryDegradation < SevereCoughThreshold) return 1f;
            if (s.inhalerReliefHours > 0f) return 1f; // inhaler suppresses symptoms
            return 1f - SevereCoughStaminaPenalty;
        }

        // ── Tick ──────────────────────────────────────────────────────

        /// <summary>
        /// Tick — accumulate respiratory degradation based on air quality.
        /// </summary>
        public void TickHours(string survivorId, float gameHours)
        {
            if (string.IsNullOrEmpty(survivorId) || gameHours <= 0f) return;

            var s = GetOrCreate(survivorId);
            if (s.hasPermanentLungDamage && s.respiratoryDegradation >= IrreversibleThreshold)
                return; // past the point of no return, no further accumulation needed

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
            float oldValue = s.respiratoryDegradation;
            s.respiratoryDegradation = MathfCompat.Min(100f,
                s.respiratoryDegradation + degradation);

            if (s.respiratoryDegradation > oldValue)
                OnRespiratoryDegradationIncreased?.Invoke(survivorId,
                    s.respiratoryDegradation - oldValue);

            // Threshold checks
            if (s.respiratoryDegradation >= SevereCoughThreshold &&
                oldValue < SevereCoughThreshold)
            {
                OnSevereCoughStarted?.Invoke(survivorId);
            }

            if (s.respiratoryDegradation >= IrreversibleThreshold &&
                oldValue < IrreversibleThreshold)
            {
                s.hasPermanentLungDamage = true;
                s.requiresInhaler = true;
                OnRequiresInhaler?.Invoke(survivorId);
            }

            if (s.respiratoryDegradation >= TerminalLungThreshold &&
                oldValue < TerminalLungThreshold)
            {
                OnTerminalLungDamage?.Invoke(survivorId);
            }

            // Apply ongoing effects
            if (s.respiratoryDegradation >= SevereCoughThreshold)
            {
                OnStaminaPenaltyRequested?.Invoke(survivorId, SevereCoughStaminaPenalty);
                float moraleDrain = SevereCoughMoraleDrainPerDay * (gameHours / 24f);
                OnMoraleDrainRequested?.Invoke(survivorId, -moraleDrain);
            }

            // Count down inhaler relief
            if (s.inhalerReliefHours > 0f)
            {
                s.inhalerReliefHours -= gameHours;
                if (s.inhalerReliefHours <= 0f)
                    s.inhalerReliefHours = 0f;
            }

            RaiseChanged();
        }

        // ── Treatments ────────────────────────────────────────────────

        /// <summary>
        /// Apply a medical inhaler to reduce respiratory degradation and provide relief.
        /// </summary>
        public bool ApplyInhaler(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (!_survivors.TryGetValue(survivorId, out var s)) return false;
            if (s.respiratoryDegradation <= 0f) return false;

            s.inhalerReliefHours = InhalerReliefDurationHours;
            s.respiratoryDegradation = MathfCompat.Max(0f,
                s.respiratoryDegradation - InhalerDegradationReduction);
            s.requiresInhaler = s.respiratoryDegradation >= IrreversibleThreshold;
            RaiseChanged();
            return true;
        }

        /// <summary>
        /// Apply herbal tea for mild respiratory relief.
        /// </summary>
        public bool ApplyHerbalTea(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (!_survivors.TryGetValue(survivorId, out var s)) return false;
            if (s.respiratoryDegradation <= 0f) return false;

            s.respiratoryDegradation = MathfCompat.Max(0f,
                s.respiratoryDegradation - HerbalTeaDegradationReduction);
            s.requiresInhaler = s.respiratoryDegradation >= IrreversibleThreshold;
            RaiseChanged();
            return true;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public RespiratoryDegenerationState CaptureState()
        {
            var copy = new RespiratoryDegenerationState { systemId = SystemId };
            var ids = new List<string>(_survivors.Keys);
            ids.Sort(string.CompareOrdinal);
            for (int i = 0; i < ids.Count; i++)
            {
                var src = _survivors[ids[i]];
                copy.survivors.Add(new RespiratorySurvivorState
                {
                    survivorId = src.survivorId,
                    respiratoryDegradation = MathfCompat.Clamp(src.respiratoryDegradation, 0f, 100f),
                    hasPermanentLungDamage = src.hasPermanentLungDamage,
                    requiresInhaler = src.requiresInhaler,
                    inhalerReliefHours = MathfCompat.Max(0f, src.inhalerReliefHours)
                });
            }
            return copy;
        }

        public void RestoreState(RespiratoryDegenerationState saved)
        {
            _survivors.Clear();
            if (saved == null) return;
            for (int i = 0; i < saved.survivors.Count; i++)
            {
                var s = saved.survivors[i];
                if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
                _survivors[s.survivorId] = new RespiratorySurvivorState
                {
                    survivorId = s.survivorId,
                    respiratoryDegradation = MathfCompat.Clamp(s.respiratoryDegradation, 0f, 100f),
                    hasPermanentLungDamage = s.hasPermanentLungDamage,
                    requiresInhaler = s.requiresInhaler,
                    inhalerReliefHours = MathfCompat.Max(0f, s.inhalerReliefHours)
                };
            }
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke();
    }
}
