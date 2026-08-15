using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Somatic Flashback System — high atmospheric noise or air siren sounds
    /// trigger visual/auditory distortions for traumatized survivors, reducing
    /// work efficiency unless grounded by another companion.
    ///
    /// Plain C#, leaf assembly. Host injects audio event listener and
    /// companion-grounding lookup.
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
        public const float CompanionGroundingRange = 1f; // must be in same room

        // ── Events ─────────────────────────────────────────────────────
        public event Action<Survivor, float> OnFlashbackTriggered;
        // sv, durationHours
        public event Action<Survivor, float, float> OnFlashbackGrounded;
        // sv, originalPenalty, reducedPenalty
        public event Action<Survivor> OnFlashbackEnded;

        // ── State ──────────────────────────────────────────────────────
        private readonly Dictionary<string, float> _activeFlashbackRemaining =
            new Dictionary<string, float>();

        // ── Host hooks ─────────────────────────────────────────────────
        public Func<Survivor, Survivor, bool> IsCompanionInSameRoom;
        // survivor, companion → bool
        public Action<Survivor, float> SetWorkEfficiencyPenalty;
        public Func<IReadOnlyList<Survivor>> GetSurvivors;
        public System.Random Rng;

        /// <summary>
        /// Called when an audio event fires (siren, explosion, loud noise).
        /// Checks each susceptible survivor for flashback triggers.
        /// </summary>
        public void OnAudioEvent(string audioEventId, float noiseSeverity)
        {
            var survivors = GetSurvivors?.Invoke();
            if (survivors == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (sv.FlashbackSusceptibility <= 0f) continue;

                float chance = sv.FlashbackSusceptibility * noiseSeverity *
                    BaseFlashbackChancePerNoise;
                if ((Rng?.NextDouble() ?? 0.5) >= chance) continue;

                // Check if grounded by a companion
                Survivor groundedBy = FindGroundingCompanion(sv, survivors);
                float duration = MinFlashbackDurationHours +
                    (float)((Rng?.NextDouble() ?? 0.5) *
                    (MaxFlashbackDurationHours - MinFlashbackDurationHours));

                float penalty;
                if (groundedBy != null)
                {
                    sv.IsGroundedByCompanion = true;
                    penalty = GroundedWorkEfficiencyPenalty;
                    OnFlashbackGrounded?.Invoke(sv, FlashbackWorkEfficiencyPenalty,
                        GroundedWorkEfficiencyPenalty);
                }
                else
                {
                    penalty = FlashbackWorkEfficiencyPenalty;
                }

                sv.FlashbackWorkEfficiencyPenalty = penalty;
                _activeFlashbackRemaining[sv.Id] = duration;
                SetWorkEfficiencyPenalty?.Invoke(sv, penalty);
                OnFlashbackTriggered?.Invoke(sv, duration);
            }
        }

        /// <summary>
        /// Tick — count down active flashback timers.
        /// </summary>
        public void Tick(Survivor sv, float gameHours)
        {
            if (sv == null || !sv.IsAlive) return;

            // Decay susceptibility
            if (sv.FlashbackSusceptibility > 0f)
            {
                sv.FlashbackSusceptibility = Math.Max(0f,
                    sv.FlashbackSusceptibility - FlashbackDecayPerDay * (gameHours / 24f));
            }

            // Count down active flashback
            if (_activeFlashbackRemaining.TryGetValue(sv.Id, out float remaining))
            {
                remaining -= gameHours;
                if (remaining <= 0f)
                {
                    _activeFlashbackRemaining.Remove(sv.Id);
                    sv.FlashbackWorkEfficiencyPenalty = 0f;
                    sv.IsGroundedByCompanion = false;
                    SetWorkEfficiencyPenalty?.Invoke(sv, 0f);
                    OnFlashbackEnded?.Invoke(sv);
                }
                else
                {
                    _activeFlashbackRemaining[sv.Id] = remaining;
                }
            }
        }

        /// <summary>
        /// Increase flashback susceptibility after a traumatic event.
        /// </summary>
        public void IncreaseSusceptibility(Survivor sv, float amount)
        {
            if (sv == null) return;
            sv.FlashbackSusceptibility = Math.Min(1f,
                sv.FlashbackSusceptibility + amount);
        }

        private Survivor FindGroundingCompanion(Survivor sv,
            IReadOnlyList<Survivor> survivors)
        {
            for (int i = 0; i < survivors.Count; i++)
            {
                var other = survivors[i];
                if (other == null || other == sv || !other.IsAlive) continue;
                if (IsCompanionInSameRoom?.Invoke(sv, other) == true)
                    return other;
            }
            return null;
        }

        /// <summary>
        /// Returns true if the survivor currently has an active flashback.
        /// </summary>
        public bool HasActiveFlashback(Survivor sv)
        {
            return sv != null && _activeFlashbackRemaining.ContainsKey(sv.Id);
        }
    }
}
