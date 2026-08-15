using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class SurgicalBotchState
    {
        public string hazard_id = "hazard_surgical_botch";
        public float botch_chance = 0f;
        public string last_botch_surgery = "";
        public string complication_affliction = "";
        public float second_surgery_difficulty = 0f;
    }

    /// <summary>
    /// Prompt #831: Surgical Botches.
    /// If the surgeon is Fatigued or Anxious, surgery can botch — the patient
    /// survives but gains a new complication Affliction and requires a second,
    /// harder surgery.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    public class Hazard_SurgicalBotch
    {
        // ── Constants ──────────────────────────────────────────────────
        private const float BOTCH_MULTIPLIER = 0.5f;
        private const float SECOND_SURGERY_DIFFICULTY_BONUS = 0.5f;

        private static readonly string[] COMPLICATIONS =
        {
            "infection",
            "nerve_damage",
            "internal_bleeding"
        };

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, string> OnBotchOccurred;        // surgeryType, complication
        public event Action OnPatientSurvived;
        public event Action<string> OnComplicationApplied;          // afflictionId
        public event Action<float> OnSecondSurgeryRequired;         // difficulty

        // ── State ──────────────────────────────────────────────────────
        private float _botchChance;
        private string _lastBotchSurgery = "";
        private string _complicationAffliction = "";
        private float _secondSurgeryDifficulty;

        private readonly System.Random _rng = AtomicWar._Game.Utilities.SeededRandom.Create(
            AtomicWar._Game.Utilities.SeededRandom.WorldSeed, "hazard_surgicalbotch");

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Calculate the botch chance from the surgeon's current fatigue
        /// and anxiety (both 0-1). Formula: ((fatigue + anxiety) / 2) * 0.5.
        /// Maximum is 50 %.
        /// </summary>
        public float CalculateBotchChance(float surgeonFatigue, float surgeonAnxiety)
        {
            float avg = (Mathf.Clamp01(surgeonFatigue) + Mathf.Clamp01(surgeonAnxiety)) * 0.5f;
            return avg * BOTCH_MULTIPLIER;
        }

        /// <summary>
        /// Attempt a surgery. Returns true if the surgery succeeded (no
        /// botch), false if it botched.
        /// </summary>
        public bool AttemptSurgery(string surgeryType, float fatigue, float anxiety)
        {
            if (string.IsNullOrEmpty(surgeryType)) return true;

            float chance = CalculateBotchChance(fatigue, anxiety);
            _botchChance = chance;

            float roll = (float)_rng.NextDouble();
            if (roll < chance)
            {
                // Botch
                string complication = COMPLICATIONS[_rng.Next(COMPLICATIONS.Length)];
                _lastBotchSurgery = surgeryType;
                _complicationAffliction = complication;
                _secondSurgeryDifficulty = 1f + SECOND_SURGERY_DIFFICULTY_BONUS; // +50 %

                OnBotchOccurred?.Invoke(surgeryType, complication);
                OnPatientSurvived?.Invoke();
                OnComplicationApplied?.Invoke(complication);
                OnSecondSurgeryRequired?.Invoke(_secondSurgeryDifficulty);
                return false;
            }

            // Clean surgery
            _lastBotchSurgery = "";
            _complicationAffliction = "";
            _secondSurgeryDifficulty = 0f;
            return true;
        }

        /// <summary>Returns the complication affliction id from the last botch.</summary>
        public string GetComplication()
        {
            return _complicationAffliction;
        }

        /// <summary>Returns the difficulty multiplier for the corrective surgery.</summary>
        public float GetSecondSurgeryDifficulty()
        {
            return _secondSurgeryDifficulty;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public SurgicalBotchState CaptureState()
        {
            return new SurgicalBotchState
            {
                hazard_id = "hazard_surgical_botch",
                botch_chance = _botchChance,
                last_botch_surgery = _lastBotchSurgery,
                complication_affliction = _complicationAffliction,
                second_surgery_difficulty = _secondSurgeryDifficulty
            };
        }

        public void RestoreState(SurgicalBotchState saved)
        {
            if (saved == null) return;
            _botchChance = saved.botch_chance;
            _lastBotchSurgery = saved.last_botch_surgery;
            _complicationAffliction = saved.complication_affliction;
            _secondSurgeryDifficulty = saved.second_surgery_difficulty;
        }
    }
}
