using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Survivors
{
    // ── Save-state DTOs ────────────────────────────────────────────────
    [Serializable]
    public sealed class CombatTraumaSurvivorState
    {
        public string survivorId = string.Empty;
        public int combatEncountersSurvived;
        public float hoursSinceLastCombat;
        public float hypervigilanceLevel;
        public bool hadFalseAlarmTonight;
        public bool isGroundedByCompanion;
    }

    [Serializable]
    public sealed class CombatTraumaSaveState
    {
        public List<CombatTraumaSurvivorState> survivors = new List<CombatTraumaSurvivorState>();
    }

    /// <summary>
    /// Combat Trauma &amp; Hypervigilance System — surviving violent skirmishes
    /// increases defense and reaction speed during raids, but causes accidental
    /// false-alarm alerts inside the bunker during high-stress night hours.
    ///
    /// Engine-agnostic port: uses string survivor IDs, raises C# events on state
    /// change, and is save/load safe via CaptureState/RestoreState (deep copy).
    /// Host injects morale and RNG callbacks.
    /// </summary>
    public class CombatTraumaSystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float HypervigilancePerCombat = 0.05f;
        public const float HypervigilanceDecayPerDay = 0.02f;
        public const float DefenseBonusPerHypervigilance = 0.15f;
        public const float FalseAlarmChancePerNight = 0.30f;
        public const float FalseAlarmMoraleHit = -5f;
        public const float CompanionGroundingReduction = 0.50f;
        public const float MaxHypervigilance = 1f;
        public const float CombatDecayThresholdHours = 72f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, float> OnHypervigilanceIncreased;
        public event Action<string> OnFalseAlarmTriggered;
        public event Action<float> OnShelterFalseAlarm;
        public event Action OnStateChanged;

        // ── Host hooks ─────────────────────────────────────────────────
        public Action<string, float> ApplyMoraleDelta;
        public ISeededRng Rng;

        // ── Internal state ─────────────────────────────────────────────
        private readonly Dictionary<string, CombatTraumaSurvivorState> _bySurvivor =
            new Dictionary<string, CombatTraumaSurvivorState>(StringComparer.Ordinal);

        private CombatTraumaSurvivorState GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state))
            {
                state = new CombatTraumaSurvivorState { survivorId = survivorId };
                _bySurvivor[survivorId] = state;
            }
            return state;
        }

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Ensure a survivor is tracked (e.g. on spawn). Optional — methods
        /// auto-create state on first access.
        /// </summary>
        public void RegisterSurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            GetOrCreate(survivorId);
        }

        /// <summary>
        /// Set whether a survivor is ground by a companion (reduces false-alarm chance).
        /// </summary>
        public void SetGroundedByCompanion(string survivorId, bool grounded)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            var state = GetOrCreate(survivorId);
            if (state.isGroundedByCompanion != grounded)
            {
                state.isGroundedByCompanion = grounded;
                OnStateChanged?.Invoke();
            }
        }

        /// <summary>
        /// Call after a survivor survives a combat encounter (raid, skirmish, etc.).
        /// </summary>
        public void OnCombatSurvived(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            var state = GetOrCreate(survivorId);
            state.combatEncountersSurvived++;
            state.hoursSinceLastCombat = 0f;
            float oldLevel = state.hypervigilanceLevel;
            state.hypervigilanceLevel = MathfCompat.Min(MaxHypervigilance,
                state.hypervigilanceLevel + HypervigilancePerCombat);
            if (state.hypervigilanceLevel > oldLevel)
                OnHypervigilanceIncreased?.Invoke(survivorId, state.hypervigilanceLevel);
            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Get the defense multiplier for this survivor during raids.
        /// </summary>
        public float GetDefenseMultiplier(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state)) return 1f;
            return 1f + (state.hypervigilanceLevel * DefenseBonusPerHypervigilance);
        }

        /// <summary>
        /// Get the current hypervigilance level for a survivor.
        /// </summary>
        public float GetHypervigilanceLevel(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var state)
                ? state.hypervigilanceLevel : 0f;
        }

        /// <summary>
        /// Get the number of combat encounters survived by a survivor.
        /// </summary>
        public int GetCombatEncountersSurvived(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var state)
                ? state.combatEncountersSurvived : 0;
        }

        /// <summary>
        /// Tick — decay hypervigilance and roll for false alarms at night.
        /// Called once per game-hour substep. False alarm check only fires
        /// once per night per survivor.
        /// </summary>
        public void Tick(string survivorId, float gameHours, bool isNightTime)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state)) return;

            state.hoursSinceLastCombat += gameHours;

            // Decay hypervigilance if no combat recently
            if (state.hoursSinceLastCombat > CombatDecayThresholdHours &&
                state.hypervigilanceLevel > 0f)
            {
                float dailyDecay = HypervigilanceDecayPerDay * (gameHours / 24f);
                state.hypervigilanceLevel = MathfCompat.Max(0f,
                    state.hypervigilanceLevel - dailyDecay);
            }

            // False alarm check — only at night, once per night
            if (isNightTime && !state.hadFalseAlarmTonight && state.hypervigilanceLevel > 0.1f)
            {
                float chance = state.hypervigilanceLevel * FalseAlarmChancePerNight *
                    (gameHours / 12f); // scale by night-length fraction

                // Companion grounding reduces chance
                if (state.isGroundedByCompanion)
                    chance *= (1f - CompanionGroundingReduction);

                if ((Rng?.NextDouble() ?? 0.5) < chance)
                {
                    state.hadFalseAlarmTonight = true;
                    ApplyMoraleDelta?.Invoke(survivorId, FalseAlarmMoraleHit);
                    OnFalseAlarmTriggered?.Invoke(survivorId);
                    OnShelterFalseAlarm?.Invoke(FalseAlarmMoraleHit);
                    OnStateChanged?.Invoke();
                }
            }
        }

        /// <summary>
        /// Reset the per-night false alarm flag. Called at dawn by host.
        /// </summary>
        public void ResetNightFlags()
        {
            foreach (var kv in _bySurvivor)
                kv.Value.hadFalseAlarmTonight = false;
        }

        /// <summary>
        /// Check whether a survivor is currently tracked.
        /// </summary>
        public bool IsTracked(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId) && _bySurvivor.ContainsKey(survivorId);
        }

        // ── Save / Load ────────────────────────────────────────────────

        public CombatTraumaSaveState CaptureState()
        {
            var save = new CombatTraumaSaveState();
            foreach (var kv in _bySurvivor)
            {
                var s = kv.Value;
                save.survivors.Add(new CombatTraumaSurvivorState
                {
                    survivorId = s.survivorId,
                    combatEncountersSurvived = s.combatEncountersSurvived,
                    hoursSinceLastCombat = s.hoursSinceLastCombat,
                    hypervigilanceLevel = s.hypervigilanceLevel,
                    hadFalseAlarmTonight = s.hadFalseAlarmTonight,
                    isGroundedByCompanion = s.isGroundedByCompanion
                });
            }
            return save;
        }

        public void RestoreState(CombatTraumaSaveState save)
        {
            _bySurvivor.Clear();
            if (save?.survivors == null) return;
            foreach (var s in save.survivors)
            {
                if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
                _bySurvivor[s.survivorId] = new CombatTraumaSurvivorState
                {
                    survivorId = s.survivorId,
                    combatEncountersSurvived = s.combatEncountersSurvived,
                    hoursSinceLastCombat = s.hoursSinceLastCombat,
                    hypervigilanceLevel = s.hypervigilanceLevel,
                    hadFalseAlarmTonight = s.hadFalseAlarmTonight,
                    isGroundedByCompanion = s.isGroundedByCompanion
                };
            }
            OnStateChanged?.Invoke();
        }
    }
}
