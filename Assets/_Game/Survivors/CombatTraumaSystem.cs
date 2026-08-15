using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Combat Trauma & Hypervigilance System — surviving violent skirmishes
    /// increases defense and reaction speed during raids, but causes accidental
    /// false-alarm alerts inside the bunker during high-stress night hours.
    ///
    /// Plain C#, leaf assembly. Host injects defense modifier and alarm callbacks.
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
        public event Action<Survivor, float> OnHypervigilanceIncreased;
        public event Action<Survivor> OnFalseAlarmTriggered;
        public event Action<float> OnShelterFalseAlarm;

        // ── Host hooks ─────────────────────────────────────────────────
        public Action<Survivor, float> ApplyMoraleDelta;
        public Func<float> GetDay;
        public Func<IReadOnlyList<Survivor>> GetSurvivors;
        public System.Random Rng;
        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        /// <summary>
        /// Call after a survivor survives a combat encounter (raid, skirmish, etc.).
        /// </summary>
        public void OnCombatSurvived(Survivor sv)
        {
            if (sv == null || !sv.IsAlive) return;
            sv.CombatEncountersSurvived++;
            sv.HoursSinceLastCombat = 0f;
            float oldLevel = sv.HypervigilanceLevel;
            sv.HypervigilanceLevel = Math.Min(MaxHypervigilance,
                sv.HypervigilanceLevel + HypervigilancePerCombat);
            if (sv.HypervigilanceLevel > oldLevel)
                OnHypervigilanceIncreased?.Invoke(sv, sv.HypervigilanceLevel);
        }

        /// <summary>
        /// Get the defense multiplier for this survivor during raids.
        /// </summary>
        public float GetDefenseMultiplier(Survivor sv)
        {
            if (sv == null) return 1f;
            return 1f + (sv.HypervigilanceLevel * DefenseBonusPerHypervigilance);
        }

        /// <summary>
        /// Tick — decay hypervigilance and roll for false alarms at night.
        /// Called once per game-hour substep. False alarm check only fires
        /// once per night per survivor.
        /// </summary>
        public void Tick(Survivor sv, float gameHours, bool isNightTime)
        {
            if (sv == null || !sv.IsAlive) return;

            sv.HoursSinceLastCombat += gameHours;

            // Decay hypervigilance if no combat recently
            if (sv.HoursSinceLastCombat > CombatDecayThresholdHours &&
                sv.HypervigilanceLevel > 0f)
            {
                float dailyDecay = HypervigilanceDecayPerDay * (gameHours / 24f);
                sv.HypervigilanceLevel = Math.Max(0f,
                    sv.HypervigilanceLevel - dailyDecay);
            }

            // False alarm check — only at night, once per night
            if (isNightTime && !sv.HadFalseAlarmTonight && sv.HypervigilanceLevel > 0.1f)
            {
                float chance = sv.HypervigilanceLevel * FalseAlarmChancePerNight *
                    (gameHours / 12f); // scale by night-length fraction

                // Companion grounding reduces chance
                if (sv.IsGroundedByCompanion)
                    chance *= (1f - CompanionGroundingReduction);

                if ((Rng?.NextDouble() ?? 0.5) < chance)
                {
                    sv.HadFalseAlarmTonight = true;
                    ApplyMoraleDelta?.Invoke(sv, FalseAlarmMoraleHit);
                    OnFalseAlarmTriggered?.Invoke(sv);
                    OnShelterFalseAlarm?.Invoke(FalseAlarmMoraleHit);
                }
            }
        }

        /// <summary>
        /// Reset the per-night false alarm flag. Called at dawn by host.
        /// </summary>
        public void ResetNightFlags(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null)
                    survivors[i].HadFalseAlarmTonight = false;
            }
        }
    }
}
