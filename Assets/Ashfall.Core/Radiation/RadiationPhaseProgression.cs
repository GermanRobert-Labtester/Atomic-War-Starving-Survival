using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Radiation
{
    /// <summary>
    /// Clinical radiation-sickness phase. The full disease model layered on top
    /// of RadiationSystem's acute/chronic checks: a big exposure pushes through
    /// Healthy -> Prodromal -> Latent -> ManifestIllness -> ChronicFibrosis or
    /// RecoveryOrDeath. Ported from Unity's RadiationPhaseProgression + the
    /// PrognosisPipeline it depends on, merged into one self-contained, engine-
    /// agnostic system.
    /// </summary>
    public enum RadiationSicknessPhase
    {
        Healthy,
        Prodromal,
        Latent,
        ManifestIllness,
        ChronicFibrosis,
        RecoveryOrDeath
    }

    /// <summary>
    /// Per-survivor state for the radiation phase-progression system. Plain DTO;
    /// the host maps it onto its own survivor object.
    /// </summary>
    public class PhaseProgressionState
    {
        public string Id = string.Empty;
        public bool IsAlive = true;

        // ── Phase state machine ───────────────────────────────────────
        public RadiationSicknessPhase Phase = RadiationSicknessPhase.Healthy;
        public float PhaseHoursElapsed;

        // ── Prognosis pipeline state ──────────────────────────────────
        /// <summary>Rolling acute-dose window (leaky bucket, decays per day).</summary>
        public float AcuteDoseWindow;
        /// <summary>Permanent accumulated tissue damage; never decays.</summary>
        public float LatentDamage;
        /// <summary>Days (fractional) until the next stage transition.</summary>
        public float OnsetTimer;
        /// <summary>Hours of iodine protection remaining.</summary>
        public float IodineProtectionTimer;

        // ── Outcome state ─────────────────────────────────────────────
        public float LungCapacity = 100f;
        public bool HasPermanentLungDamage;
        public bool HasTerminalPrognosis;
        public float TerminalPrognosisDaysRemaining;

        // ── Needs (host-applied via events, but tracked here for queries) ─
        public float Health = 100f;
        public bool IsResting;
    }

    /// <summary>Serializable snapshot of all survivors' phase-progression state.</summary>
    [Serializable]
    public class PhaseProgressionSaveState
    {
        public string systemId = RadiationPhaseProgression.SystemId;
        public List<PhaseProgressionSurvivorSave> survivors = new List<PhaseProgressionSurvivorSave>();
    }

    [Serializable]
    public class PhaseProgressionSurvivorSave
    {
        public string survivorId = string.Empty;
        public string phase = "Healthy";
        public float phaseHoursElapsed;
        public float acuteDoseWindow;
        public float latentDamage;
        public float onsetTimer;
        public float iodineProtectionTimer;
        public float lungCapacity = 100f;
        public bool hasPermanentLungDamage;
        public bool hasTerminalPrognosis;
        public float terminalPrognosisDaysRemaining;
    }

    /// <summary>
    /// Engine-agnostic port of Unity's RadiationPhaseProgression + PrognosisPipeline.
    /// Owns the full clinical disease model: rolling acute-dose window, latent damage
    /// accumulation, 5-phase state machine (Healthy -> Prodromal -> Latent ->
    /// ManifestIllness -> ChronicFibrosis or RecoveryOrDeath), and outcome resolution.
    ///
    /// Survivor-agnostic: operates on PhaseProgressionState via string ids. The host
    /// subscribes to effect events (health/morale deltas, chronic illness grants) and
    /// applies them in its own domain.
    ///
    /// Save/load via CaptureState/RestoreState. Deterministic with ISeededRng.
    /// </summary>
    public class RadiationPhaseProgression
    {
        public const string SystemId = "radiation_phase_progression";

        // ── Prognosis pipeline constants ──────────────────────────────

        /// <summary>Per-day retention of the rolling acute-dose window (leaky bucket).</summary>
        public const float AcuteDoseWindowDecayPerDay = 0.75f;
        /// <summary>
        /// AcuteDoseWindow at/above this triggers Healthy -> Prodromal. Kept safely
        /// above RadiationSystem.AcuteThreshold (80) so a short exposure that only
        /// grazes the instant acute-sickness check does not also trigger Prodromal.
        /// </summary>
        public const float ProdromalTriggerDose = 100f;
        /// <summary>Fraction of every dose that trickles into permanent LatentDamage.</summary>
        public const float ChronicDamageFactor = 0.05f;
        /// <summary>Fraction of the dose above ProdromalTriggerDose added as an acute lump.</summary>
        public const float AcuteLatentDamageFactor = 1f;
        /// <summary>LatentDamage at/above this also grants Chronic Illness.</summary>
        public const float LatentDamageChronicThreshold = 100f;
        /// <summary>Hours of iodine protection per administration.</summary>
        public const float IodineWindowHours = 12f;
        /// <summary>Multiplier on the acute LatentDamage lump if iodine is active at trigger time.</summary>
        public const float IodineMitigationFactor = 0.35f;
        /// <summary>LatentDamage at/above this reads as maximum severity (1.0) for curves.</summary>
        public const float LatentDamageSeverityReference = 60f;

        // ── Phase duration constants (game-hours) ─────────────────────

        public const float ProdromalDurationHours = 24f;
        public const float LatentMinDurationHours = 144f;   // 6 days
        public const float LatentMaxDurationHours = 288f;   // 12 days
        public const float ManifestMinDurationHours = 72f;  // ~3 days
        public const float ManifestMaxDurationHours = 96f;  // 4 days

        // ── Health impact constants ───────────────────────────────────

        public const float ProdromalHealthDip = 5f;
        public const float ProdromalMoraleDip = 10f;
        public const float ProdromalFatigueRate = 1.5f;
        public const float ManifestHealthCrashMin = 30f;
        public const float ManifestHealthCrashMax = 150f;
        public const float ManifestBleedPerDay = 8f;
        public const float BedRestMitigation = 0.6f;
        public const float ChronicFibrosisLungCapacityMin = 0.40f;
        public const float ChronicFibrosisLungCapacityMax = 0.70f;
        public const float ChronicFibrosisThreshold = 120f;
        /// <summary>Delta guaranteed to zero out health (single-writer death).</summary>
        private const float LethalHealthLoss = 100f;

        // ── Internal ledger ───────────────────────────────────────────

        private readonly Dictionary<string, PhaseProgressionState> _survivors =
            new Dictionary<string, PhaseProgressionState>();

        private readonly ISeededRng _rng;

        // ── Events (host applies effects in its own domain) ───────────

        /// <summary>Fired when a survivor's sickness phase changes. Args: survivorId, oldPhase, newPhase.</summary>
        public event Action<string, RadiationSicknessPhase, RadiationSicknessPhase> OnPhaseChanged;
        /// <summary>Fired when lung capacity is permanently reduced. Args: survivorId, newCapacity.</summary>
        public event Action<string, float> OnLungCapacityReduced;
        /// <summary>Fired when a terminal prognosis is declared. Args: survivorId, daysRemaining.</summary>
        public event Action<string, float> OnTerminalPrognosisDeclared;
        /// <summary>Host should apply this health delta. Args: survivorId, delta.</summary>
        public event Action<string, float> OnHealthDeltaRequested;
        /// <summary>Host should apply this morale delta. Args: survivorId, delta.</summary>
        public event Action<string, float> OnMoraleDeltaRequested;
        /// <summary>Host should grant chronic illness status. Args: survivorId.</summary>
        public event Action<string> OnChronicIllnessRequested;
        /// <summary>Host should mark chronic fibrosis on the survivor. Args: survivorId.</summary>
        public event Action<string> OnChronicFibrosisMarked;
        /// <summary>Host should reset the survivor's RadiationSystem dose to 0 (Prodromal trigger metabolized the acute dose). Args: survivorId.</summary>
        public event Action<string> OnRadiationDoseResetRequested;
        /// <summary>Fired on any state change (for save/UI refresh).</summary>
        public event Action OnStateChanged;

        public IReadOnlyDictionary<string, PhaseProgressionState> Survivors => _survivors;

        public RadiationPhaseProgression(ISeededRng? rng = null)
        {
            _rng = rng;
        }

        // ── Registration ──────────────────────────────────────────────

        public void Register(PhaseProgressionState state)
        {
            if (state != null && !string.IsNullOrEmpty(state.Id))
                _survivors[state.Id] = state;
        }

        public void Unregister(string survivorId)
        {
            _survivors.Remove(survivorId);
        }

        // ── Exposure (call once per radiation Expose with positive dose) ─

        /// <summary>
        /// Feed a just-applied dose into the rolling window and permanent latent
        /// damage, and evaluate whether it crosses a stage boundary. Call once per
        /// RadiationSystem.Expose() with a positive dose.
        /// </summary>
        public void OnExposure(string survivorId, float dose)
        {
            if (string.IsNullOrEmpty(survivorId) || dose <= 0f) return;
            if (!_survivors.TryGetValue(survivorId, out var sv) || !sv.IsAlive) return;

            sv.AcuteDoseWindow += dose;
            sv.LatentDamage += dose * ChronicDamageFactor;

            bool canTrigger = sv.Phase == RadiationSicknessPhase.Healthy
                || (sv.Phase == RadiationSicknessPhase.RecoveryOrDeath && sv.IsAlive);

            if (canTrigger && sv.AcuteDoseWindow >= ProdromalTriggerDose)
                TriggerProdromal(sv);

            if (!sv.HasPermanentLungDamage && sv.LatentDamage >= LatentDamageChronicThreshold)
                OnChronicIllnessRequested?.Invoke(survivorId);

            RaiseChanged();
        }

        /// <summary>Administer iodine — starts the protection timer.</summary>
        public void AdministerIodine(string survivorId)
        {
            if (!_survivors.TryGetValue(survivorId, out var sv) || !sv.IsAlive) return;
            sv.IodineProtectionTimer = MathfCompat.Max(sv.IodineProtectionTimer, IodineWindowHours);
            RaiseChanged();
        }

        // ── Tick (call once per game-hour substep) ────────────────────

        /// <summary>
        /// Advance the rolling window decay, iodine timer, and stage timers by
        /// elapsed game hours. Applies phase-specific health effects.
        /// </summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            foreach (var kvp in _survivors)
            {
                var sv = kvp.Value;
                if (sv == null || !sv.IsAlive) continue;
                TickSurvivor(kvp.Key, sv, gameHours);
            }
        }

        private void TickSurvivor(string survivorId, PhaseProgressionState sv, float gameHours)
        {
            float days = gameHours / 24f;

            // Decay the rolling acute-dose window
            sv.AcuteDoseWindow *= (float)Math.Pow(AcuteDoseWindowDecayPerDay, days);
            if (sv.AcuteDoseWindow < 0.01f)
                sv.AcuteDoseWindow = 0f;

            // Tick iodine protection timer
            if (sv.IodineProtectionTimer > 0f)
                sv.IodineProtectionTimer = MathfCompat.Max(0f, sv.IodineProtectionTimer - gameHours);

            // Phase-specific tick
            sv.PhaseHoursElapsed += gameHours;

            switch (sv.Phase)
            {
                case RadiationSicknessPhase.Prodromal:
                    TickProdromal(survivorId, sv, gameHours, days);
                    break;
                case RadiationSicknessPhase.Latent:
                    TickLatent(survivorId, sv, days);
                    break;
                case RadiationSicknessPhase.ManifestIllness:
                    TickManifest(survivorId, sv, gameHours, days);
                    break;
                case RadiationSicknessPhase.ChronicFibrosis:
                case RadiationSicknessPhase.Healthy:
                case RadiationSicknessPhase.RecoveryOrDeath:
                    // No ongoing effects
                    break;
            }
        }

        // ── Phase ticks ───────────────────────────────────────────────

        private void TickProdromal(string survivorId, PhaseProgressionState sv, float gameHours, float days)
        {
            // Ongoing health drain from nausea/fatigue
            float healthDrain = ProdromalHealthDip * (gameHours / ProdromalDurationHours) * ProdromalFatigueRate;
            OnHealthDeltaRequested?.Invoke(survivorId, -healthDrain);

            sv.OnsetTimer -= days;
            if (sv.OnsetTimer <= 0f)
                EnterLatent(sv);
        }

        private void TickLatent(string survivorId, PhaseProgressionState sv, float days)
        {
            // Silent phase — survivor feels fine, damage is hidden
            sv.OnsetTimer -= days;
            if (sv.OnsetTimer <= 0f)
                EnterManifest(survivorId, sv);
        }

        private void TickManifest(string survivorId, PhaseProgressionState sv, float gameHours, float days)
        {
            float severity = ComputeSeverity(sv);
            float bleedPerDay = ManifestBleedPerDay * severity *
                (sv.IsResting ? (1f - BedRestMitigation) : 1f);
            OnHealthDeltaRequested?.Invoke(survivorId, -bleedPerDay * days);

            sv.OnsetTimer -= days;
            if (sv.OnsetTimer <= 0f)
                ResolveOutcome(survivorId, sv);
        }

        // ── Phase transitions ─────────────────────────────────────────

        private void TriggerProdromal(PhaseProgressionState sv)
        {
            float overflow = sv.AcuteDoseWindow - ProdromalTriggerDose;
            float lump = MathfCompat.Max(0f, overflow) * AcuteLatentDamageFactor;
            if (sv.IodineProtectionTimer > 0f)
                lump *= IodineMitigationFactor;
            sv.LatentDamage += lump;

            // Reset the acute dose — it has been "treated/metabolized"; only
            // LatentDamage carries the injury forward.
            sv.AcuteDoseWindow = 0f;
            OnRadiationDoseResetRequested?.Invoke(sv.Id);

            TransitionTo(sv, RadiationSicknessPhase.Prodromal);
            sv.OnsetTimer = ProdromalDurationHours / 24f; // stored in days for timer

            // Immediate health/morale dip (nausea) — signal host to apply
            sv.Health = MathfCompat.Max(0f, sv.Health - ProdromalHealthDip);
            OnHealthDeltaRequested?.Invoke(sv.Id, -ProdromalHealthDip);
            OnMoraleDeltaRequested?.Invoke(sv.Id, -ProdromalMoraleDip);
        }

        private void EnterLatent(PhaseProgressionState sv)
        {
            float severity = ComputeSeverity(sv);
            float latentDays = MathfCompat.Lerp(LatentMaxDurationHours / 24f, LatentMinDurationHours / 24f, severity);
            TransitionTo(sv, RadiationSicknessPhase.Latent);
            sv.OnsetTimer = latentDays;
        }

        private void EnterManifest(string survivorId, PhaseProgressionState sv)
        {
            float severity = ComputeSeverity(sv);
            float manifestDays = MathfCompat.Lerp(ManifestMinDurationHours / 24f, ManifestMaxDurationHours / 24f, 1f - severity);
            TransitionTo(sv, RadiationSicknessPhase.ManifestIllness);
            sv.OnsetTimer = manifestDays;

            // Health crash on onset
            float crash = MathfCompat.Lerp(ManifestHealthCrashMin, ManifestHealthCrashMax, severity);
            sv.Health = MathfCompat.Max(0f, sv.Health - crash);
            OnHealthDeltaRequested?.Invoke(survivorId, -crash);

            OnChronicIllnessRequested?.Invoke(survivorId);
        }

        private void ResolveOutcome(string survivorId, PhaseProgressionState sv)
        {
            float severity = ComputeSeverity(sv);
            float deathChance = MathfCompat.Clamp01(severity - (sv.IsResting ? BedRestMitigation : 0f));

            bool dies = false;
            if (_rng != null)
                dies = _rng.NextDouble() < deathChance;
            else
                dies = deathChance > 0.5f; // deterministic fallback with no rng

            if (dies)
            {
                // Terminal prognosis
                float daysRemaining = 3f;
                if (_rng != null)
                    daysRemaining = 3f + (float)(_rng.NextDouble() * 4f); // 3-7 days
                sv.HasTerminalPrognosis = true;
                sv.TerminalPrognosisDaysRemaining = daysRemaining;
                sv.Health = 0f;
                OnHealthDeltaRequested?.Invoke(survivorId, -LethalHealthLoss);
                OnTerminalPrognosisDeclared?.Invoke(survivorId, daysRemaining);
                TransitionTo(sv, RadiationSicknessPhase.RecoveryOrDeath);
            }
            else if (sv.LatentDamage >= ChronicFibrosisThreshold)
            {
                // Chronic fibrosis
                double rngVal = _rng != null ? _rng.NextDouble() : 0.5;
                float lungReduction = ChronicFibrosisLungCapacityMin +
                    (float)(rngVal * (ChronicFibrosisLungCapacityMax - ChronicFibrosisLungCapacityMin));
                sv.LungCapacity = MathfCompat.Max(sv.LungCapacity * lungReduction, 20f);
                sv.HasPermanentLungDamage = true;
                OnChronicFibrosisMarked?.Invoke(survivorId);
                OnLungCapacityReduced?.Invoke(survivorId, sv.LungCapacity);
                TransitionTo(sv, RadiationSicknessPhase.ChronicFibrosis);
            }
            else
            {
                TransitionTo(sv, RadiationSicknessPhase.RecoveryOrDeath);
            }

            RaiseChanged();
        }

        private void TransitionTo(PhaseProgressionState sv, RadiationSicknessPhase newPhase)
        {
            if (sv.Phase == newPhase) return;
            var oldPhase = sv.Phase;
            sv.Phase = newPhase;
            sv.PhaseHoursElapsed = 0f;
            OnPhaseChanged?.Invoke(sv.Id, oldPhase, newPhase);
        }

        private static float ComputeSeverity(PhaseProgressionState sv)
        {
            return MathfCompat.Clamp01(sv.LatentDamage / LatentDamageSeverityReference);
        }

        // ── Queries ───────────────────────────────────────────────────

        /// <summary>
        /// Medical exam: return a human-readable estimate of the survivor's
        /// current phase and prognosis. Hidden from player by default; requires
        /// a doctor or medical check to reveal.
        /// </summary>
        public string GetPhasePrognosisText(string survivorId)
        {
            if (!_survivors.TryGetValue(survivorId, out var sv)) return "Unknown";
            return sv.Phase switch
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

        /// <summary>Get the current phase for a survivor.</summary>
        public RadiationSicknessPhase GetPhase(string survivorId)
        {
            return _survivors.TryGetValue(survivorId, out var sv) ? sv.Phase : RadiationSicknessPhase.Healthy;
        }

        /// <summary>Get the onset timer (days until next stage) for a survivor.</summary>
        public float GetOnsetTimer(string survivorId)
        {
            return _survivors.TryGetValue(survivorId, out var sv) ? MathfCompat.Max(0f, sv.OnsetTimer) : 0f;
        }

        /// <summary>Get the latent damage for a survivor.</summary>
        public float GetLatentDamage(string survivorId)
        {
            return _survivors.TryGetValue(survivorId, out var sv) ? sv.LatentDamage : 0f;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public PhaseProgressionSaveState CaptureState()
        {
            var save = new PhaseProgressionSaveState { systemId = SystemId };
            var ids = new List<string>(_survivors.Keys);
            ids.Sort(string.CompareOrdinal);
            for (int i = 0; i < ids.Count; i++)
            {
                var src = _survivors[ids[i]];
                save.survivors.Add(new PhaseProgressionSurvivorSave
                {
                    survivorId = ids[i],
                    phase = src.Phase.ToString(),
                    phaseHoursElapsed = src.PhaseHoursElapsed,
                    acuteDoseWindow = src.AcuteDoseWindow,
                    latentDamage = src.LatentDamage,
                    onsetTimer = src.OnsetTimer,
                    iodineProtectionTimer = src.IodineProtectionTimer,
                    lungCapacity = src.LungCapacity,
                    hasPermanentLungDamage = src.HasPermanentLungDamage,
                    hasTerminalPrognosis = src.HasTerminalPrognosis,
                    terminalPrognosisDaysRemaining = src.TerminalPrognosisDaysRemaining
                });
            }
            return save;
        }

        public void RestoreState(PhaseProgressionSaveState saved)
        {
            // Clear phase state on registered survivors
            foreach (var kvp in _survivors)
            {
                var sv = kvp.Value;
                sv.Phase = RadiationSicknessPhase.Healthy;
                sv.PhaseHoursElapsed = 0f;
                sv.AcuteDoseWindow = 0f;
                sv.LatentDamage = 0f;
                sv.OnsetTimer = 0f;
                sv.IodineProtectionTimer = 0f;
                sv.LungCapacity = 100f;
                sv.HasPermanentLungDamage = false;
                sv.HasTerminalPrognosis = false;
                sv.TerminalPrognosisDaysRemaining = 0f;
            }

            if (saved == null) { RaiseChanged(); return; }

            for (int i = 0; i < saved.survivors.Count; i++)
            {
                var s = saved.survivors[i];
                if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
                if (!_survivors.TryGetValue(s.survivorId, out var sv)) continue;

                if (Enum.TryParse(s.phase, out RadiationSicknessPhase phase))
                    sv.Phase = phase;
                sv.PhaseHoursElapsed = s.phaseHoursElapsed;
                sv.AcuteDoseWindow = s.acuteDoseWindow;
                sv.LatentDamage = s.latentDamage;
                sv.OnsetTimer = s.onsetTimer;
                sv.IodineProtectionTimer = s.iodineProtectionTimer;
                sv.LungCapacity = s.lungCapacity > 0f ? s.lungCapacity : 100f;
                sv.HasPermanentLungDamage = s.hasPermanentLungDamage;
                sv.HasTerminalPrognosis = s.hasTerminalPrognosis;
                sv.TerminalPrognosisDaysRemaining = s.terminalPrognosisDaysRemaining;
            }

            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke();
    }
}
