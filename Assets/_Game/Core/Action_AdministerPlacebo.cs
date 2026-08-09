using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AdministerPlaceboState
    {
        public string action_id = "action_administer_placebo";
        public string survivor_id = "";
        public int clean_water_used = 0;
        public int success_count = 0;
        public float discovery_chance = 0f;
        public bool discovered = false;
    }

    /// <summary>
    /// Prompt #838: Placebo.
    /// Give CleanWater disguised as Medicine. 50 % cure rate for
    /// Psychosomatic / Anxiety ailments, 0 % for physical conditions.
    /// If the survivor discovers the fake, triggers a ViolentBreak.
    /// Costs 1 CleanWater per use.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_AdministerPlacebo
    {
        // ── Constants ──────────────────────────────────────────────────
        private const float PSYCHOSOMATIC_SUCCESS_RATE = 0.50f;
        private const float DISCOVERY_MULTIPLIER = 0.3f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string> OnPlaceboAdministered;       // survivorId
        public event Action<string> OnPlaceboSucceeded;          // survivorId
        public event Action<string> OnPlaceboFailed;             // survivorId
        public event Action<string> OnPlaceboDiscovered;         // survivorId
        public event Action<string> OnViolentBreakTriggered;     // survivorId

        // ── State ──────────────────────────────────────────────────────
        private string _survivorId;
        private int _cleanWaterUsed;
        private int _successCount;
        private float _discoveryChance;
        private bool _discovered;

        private readonly System.Random _rng = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("action_administerplacebo");

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Administer a placebo (CleanWater disguised as medicine) to a
        /// survivor. Returns true if the placebo cured the ailment.
        /// </summary>
        public bool Administer(string survivorId, bool isPsychosomatic)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;

            _survivorId = survivorId;
            _cleanWaterUsed++;

            OnPlaceboAdministered?.Invoke(survivorId);

            if (!isPsychosomatic)
            {
                // Physical ailments: 0 % success
                OnPlaceboFailed?.Invoke(survivorId);
                return false;
            }

            float roll = (float)_rng.NextDouble();
            if (roll < PSYCHOSOMATIC_SUCCESS_RATE)
            {
                _successCount++;
                OnPlaceboSucceeded?.Invoke(survivorId);
                return true;
            }

            OnPlaceboFailed?.Invoke(survivorId);
            return false;
        }

        /// <summary>
        /// Check whether the survivor discovers the placebo is fake.
        /// Discovery chance = intelligence * 0.3.
        /// Returns true if discovered, triggering ViolentBreak.
        /// </summary>
        public bool CheckDiscovery(float intelligence)
        {
            if (_discovered) return true;

            float chance = Mathf.Clamp01(intelligence * DISCOVERY_MULTIPLIER);
            _discoveryChance = chance;

            float roll = (float)_rng.NextDouble();
            if (roll < chance)
            {
                _discovered = true;
                OnPlaceboDiscovered?.Invoke(_survivorId);
                OnViolentBreakTriggered?.Invoke(_survivorId);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the overall success rate for psychosomatic ailments.
        /// </summary>
        public float GetSuccessRate()
        {
            return PSYCHOSOMATIC_SUCCESS_RATE;
        }

        /// <summary>Returns true if the last placebo was discovered as fake.</summary>
        public bool WasDiscovered()
        {
            return _discovered;
        }

        /// <summary>Returns the total number of CleanWater consumed.</summary>
        public int GetCleanWaterUsed()
        {
            return _cleanWaterUsed;
        }

        /// <summary>Returns the total number of successful placebo cures.</summary>
        public int GetSuccessCount()
        {
            return _successCount;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public AdministerPlaceboState CaptureState()
        {
            return new AdministerPlaceboState
            {
                action_id = "action_administer_placebo",
                survivor_id = _survivorId ?? "",
                clean_water_used = _cleanWaterUsed,
                success_count = _successCount,
                discovery_chance = _discoveryChance,
                discovered = _discovered
            };
        }

        public void RestoreState(AdministerPlaceboState saved)
        {
            if (saved == null) return;
            _survivorId = saved.survivor_id;
            _cleanWaterUsed = saved.clean_water_used;
            _successCount = saved.success_count;
            _discoveryChance = saved.discovery_chance;
            _discovered = saved.discovered;
        }
    }
}
