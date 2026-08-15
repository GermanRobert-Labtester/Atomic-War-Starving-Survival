using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    // ── Save/load DTOs ───────────────────────────────────────────────
    [Serializable]
    public sealed class FlashbackSurvivorState
    {
        public string survivorId = string.Empty;
        public float susceptibility;
        public float activeRemainingHours;
        public float workEfficiencyPenalty;
        public bool isGroundedByCompanion;
    }

    [Serializable]
    public sealed class SomaticFlashbackSaveState
    {
        public List<FlashbackSurvivorState> survivors = new List<FlashbackSurvivorState>();
    }

    /// <summary>
    /// Somatic Flashback System — high atmospheric noise or air siren sounds
    /// trigger visual/auditory distortions for traumatized survivors, reducing
    /// work efficiency unless grounded by another companion.
    ///
    /// Engine-agnostic port: uses string survivor IDs, raises C# events on
    /// state changes, save/load safe via CaptureState/RestoreState (deep copy).
    /// Host injects audio event listener and companion-grounding lookup.
    /// </summary>
    public class SomaticFlashbackSystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float BaseFlashbackChancePerNoise = 0.15f;
        public const float FlashbackWorkEfficiencyPenalty = 0.60f;
        public const float GroundedWorkEfficiencyPenalty = 0.10f;
        public const float MinFlashbackDurationHours = 2f;
        public const float MaxFlashbackDurationHours = 6f;
        public const float FlashbackSusceptibilityPerTrauma = 0.10f;
        public const float FlashbackDecayPerDay = 0.03f;

        // ── Events ─────────────────────────────────────────────────────
        /// <summary>Fired when a flashback is triggered. Args: survivorId, durationHours.</summary>
        public event Action<string, float> OnFlashbackTriggered;

        /// <summary>Fired when a companion grounds the flashback. Args: survivorId, originalPenalty, reducedPenalty.</summary>
        public event Action<string, float, float> OnFlashbackGrounded;

        /// <summary>Fired when a flashback ends naturally. Args: survivorId.</summary>
        public event Action<string> OnFlashbackEnded;

        /// <summary>Fired on any state mutation (for UI/save).</summary>
        public event Action OnStateChanged;

        // ── Internal per-survivor state ────────────────────────────────
        private readonly Dictionary<string, FlashbackSurvivorState> _bySurvivor =
            new Dictionary<string, FlashbackSurvivorState>(StringComparer.Ordinal);

        // ── Host hooks ─────────────────────────────────────────────────
        /// <summary>Host provides: (survivorId, companionId) → true if in same room.</summary>
        public Func<string, string, bool> IsCompanionInSameRoom;

        /// <summary>Host provides: returns list of alive survivor IDs.</summary>
        public Func<IReadOnlyList<string>> GetAliveSurvivorIds;

        /// <summary>RNG source; host should inject a seeded Random for determinism.</summary>
        public Random Rng;

        // ── Public API ─────────────────────────────────────────────────

        private FlashbackSurvivorState GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state))
            {
                state = new FlashbackSurvivorState { survivorId = survivorId };
                _bySurvivor[survivorId] = state;
            }
            return state;
        }

        /// <summary>
        /// Returns the current flashback susceptibility for a survivor (0–1).
        /// </summary>
        public float GetSusceptibility(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var s) ? s.susceptibility : 0f;
        }

        /// <summary>
        /// Returns the remaining flashback hours for a survivor (0 if none active).
        /// </summary>
        public float GetActiveFlashbackRemaining(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var s) ? s.activeRemainingHours : 0f;
        }

        /// <summary>
        /// Returns the current work-efficiency penalty for a survivor (0–1).
        /// </summary>
        public float GetWorkEfficiencyPenalty(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var s) ? s.workEfficiencyPenalty : 0f;
        }

        /// <summary>
        /// Returns true if the survivor is currently grounded by a companion.
        /// </summary>
        public bool IsGroundedByCompanion(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var s) && s.isGroundedByCompanion;
        }

        /// <summary>
        /// Returns true if the survivor currently has an active flashback.
        /// </summary>
        public bool HasActiveFlashback(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var s) && s.activeRemainingHours > 0f;
        }

        /// <summary>
        /// Increase flashback susceptibility after a traumatic event.
        /// </summary>
        public void IncreaseSusceptibility(string survivorId, float amount)
        {
            if (string.IsNullOrEmpty(survivorId) || amount <= 0f) return;
            var state = GetOrCreate(survivorId);
            state.susceptibility = MathfCompat.Min(1f, state.susceptibility + amount);
            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Called when an audio event fires (siren, explosion, loud noise).
        /// Checks each susceptible survivor for flashback triggers.
        /// </summary>
        public void OnAudioEvent(string audioEventId, float noiseSeverity)
        {
            var survivorIds = GetAliveSurvivorIds?.Invoke();
            if (survivorIds == null || survivorIds.Count == 0) return;

            for (int i = 0; i < survivorIds.Count; i++)
            {
                var svId = survivorIds[i];
                if (string.IsNullOrEmpty(svId)) continue;

                var state = GetOrCreate(svId);
                if (state.susceptibility <= 0f) continue;

                float chance = state.susceptibility * noiseSeverity * BaseFlashbackChancePerNoise;
                if ((Rng?.NextDouble() ?? 0.5) >= chance) continue;

                // Check if grounded by a companion
                string groundedBy = FindGroundingCompanion(svId, survivorIds);
                float duration = MinFlashbackDurationHours +
                    (float)((Rng?.NextDouble() ?? 0.5) *
                    (MaxFlashbackDurationHours - MinFlashbackDurationHours));

                float penalty;
                if (groundedBy != null)
                {
                    state.isGroundedByCompanion = true;
                    penalty = GroundedWorkEfficiencyPenalty;
                    OnFlashbackGrounded?.Invoke(svId, FlashbackWorkEfficiencyPenalty,
                        GroundedWorkEfficiencyPenalty);
                }
                else
                {
                    state.isGroundedByCompanion = false;
                    penalty = FlashbackWorkEfficiencyPenalty;
                }

                state.workEfficiencyPenalty = penalty;
                state.activeRemainingHours = duration;
                OnFlashbackTriggered?.Invoke(svId, duration);
                OnStateChanged?.Invoke();
            }
        }

        /// <summary>
        /// Tick — decay susceptibility and count down active flashback timers.
        /// </summary>
        public void Tick(string survivorId, float gameHours)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_bySurvivor.TryGetValue(survivorId, out var state)) return;

            bool changed = false;

            // Decay susceptibility
            if (state.susceptibility > 0f)
            {
                float old = state.susceptibility;
                state.susceptibility = Math.Max(0f,
                    state.susceptibility - FlashbackDecayPerDay * (gameHours / 24f));
                if (state.susceptibility != old) changed = true;
            }

            // Count down active flashback
            if (state.activeRemainingHours > 0f)
            {
                state.activeRemainingHours -= gameHours;
                if (state.activeRemainingHours <= 0f)
                {
                    state.activeRemainingHours = 0f;
                    state.workEfficiencyPenalty = 0f;
                    state.isGroundedByCompanion = false;
                    OnFlashbackEnded?.Invoke(survivorId);
                }
                changed = true;
            }

            if (changed) OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Tick all known survivors at once.
        /// </summary>
        public void TickAll(float gameHours)
        {
            // Copy keys to allow mutation during iteration
            var keys = new List<string>(_bySurvivor.Keys);
            for (int i = 0; i < keys.Count; i++)
                Tick(keys[i], gameHours);
        }

        // ── Save / Load ────────────────────────────────────────────────

        public SomaticFlashbackSaveState CaptureState()
        {
            var save = new SomaticFlashbackSaveState();
            foreach (var kv in _bySurvivor)
            {
                var s = kv.Value;
                save.survivors.Add(new FlashbackSurvivorState
                {
                    survivorId = s.survivorId,
                    susceptibility = s.susceptibility,
                    activeRemainingHours = s.activeRemainingHours,
                    workEfficiencyPenalty = s.workEfficiencyPenalty,
                    isGroundedByCompanion = s.isGroundedByCompanion
                });
            }
            return save;
        }

        public void RestoreState(SomaticFlashbackSaveState save)
        {
            _bySurvivor.Clear();
            if (save?.survivors == null) return;
            foreach (var s in save.survivors)
            {
                if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
                _bySurvivor[s.survivorId] = new FlashbackSurvivorState
                {
                    survivorId = s.survivorId,
                    susceptibility = s.susceptibility,
                    activeRemainingHours = s.activeRemainingHours,
                    workEfficiencyPenalty = s.workEfficiencyPenalty,
                    isGroundedByCompanion = s.isGroundedByCompanion
                };
            }
            OnStateChanged?.Invoke();
        }

        // ── Private helpers ────────────────────────────────────────────

        private string FindGroundingCompanion(string survivorId, IReadOnlyList<string> survivorIds)
        {
            if (IsCompanionInSameRoom == null) return null;
            for (int i = 0; i < survivorIds.Count; i++)
            {
                var other = survivorIds[i];
                if (other == survivorId || string.IsNullOrEmpty(other)) continue;
                if (IsCompanionInSameRoom(survivorId, other))
                    return other;
            }
            return null;
        }
    }
}
