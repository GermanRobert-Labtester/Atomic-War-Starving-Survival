using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BloodToxicityState
    {
        public string survivorId;
        public int chemUsageCount = 0;
        public float bloodToxicityLevel = 0f;
        public int chemThreshold = 5;
        public bool isBloodToxic = false;
    }

    [Serializable]
    public class BloodToxicitySave
    {
        public List<BloodToxicityState> entries = new List<BloodToxicityState>();
    }

    /// <summary>
    /// Prompt #551: System: Blood Toxicity (Chem Abuse).
    /// If a survivor abuses too many Chems (Morphine, Anti-Rad, Amphetamines),
    /// their blood becomes toxic. If bitten by FeralDogs or Cannibals, the
    /// attacker takes Poison damage — a grim, self-destructive defense mechanism.
    /// </summary>
    public class BloodToxicitySystem
    {
        private static readonly HashSet<string> TrackedChemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "morphine",
            "anti_rad",
            "amphetamines"
        };

        private static readonly HashSet<string> SusceptibleAttackerTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "feral_dog",
            "cannibal"
        };

        private const float ToxicityPerChemUse = 20f;
        private const float BiteRetaliationDamage = 20f;

        private readonly Dictionary<string, BloodToxicityState> _states = new Dictionary<string, BloodToxicityState>();

        public event Action<string, string> OnChemAbuseRecorded;      // (survivorId, chemId)
        public event Action<string> OnBloodToxicityReached;           // (survivorId)
        public event Action<string, string, float> OnAttackerPoisoned; // (survivorId, attackerType, damage)

        public IReadOnlyDictionary<string, BloodToxicityState> States => _states;

        /// <summary>
        /// Record a chem use event for the given survivor. Only tracked chems
        /// (morphine, anti_rad, amphetamines) count toward blood toxicity.
        /// </summary>
        public void RecordChemUse(string survivorId, string chemId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(chemId)) return;
            if (!TrackedChemIds.Contains(chemId)) return;

            if (!_states.TryGetValue(survivorId, out var state))
            {
                state = new BloodToxicityState { survivorId = survivorId };
                _states[survivorId] = state;
            }

            state.chemUsageCount++;
            state.bloodToxicityLevel = Mathf.Min(100f, state.chemUsageCount * ToxicityPerChemUse);

            OnChemAbuseRecorded?.Invoke(survivorId, chemId);

            if (!state.isBloodToxic && state.chemUsageCount >= state.chemThreshold)
            {
                state.isBloodToxic = true;
                OnBloodToxicityReached?.Invoke(survivorId);
            }
        }

        /// <summary>
        /// Returns poison retaliation damage dealt to an attacker that bites
        /// the survivor. Returns 0 if blood is not toxic or attacker is not susceptible.
        /// </summary>
        public float GetBiteRetaliationDamage(string survivorId, string attackerType)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(attackerType)) return 0f;
            if (!_states.TryGetValue(survivorId, out var state)) return 0f;
            if (!state.isBloodToxic) return 0f;
            if (!SusceptibleAttackerTypes.Contains(attackerType)) return 0f;

            OnAttackerPoisoned?.Invoke(survivorId, attackerType, BiteRetaliationDamage);
            return BiteRetaliationDamage;
        }

        /// <summary>True when the survivor's blood toxicity has crossed the threshold.</summary>
        public bool IsBloodToxic(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            return _states.TryGetValue(survivorId, out var state) && state.isBloodToxic;
        }

        public BloodToxicitySave CaptureState()
        {
            var save = new BloodToxicitySave();
            foreach (var kvp in _states)
            {
                save.entries.Add(new BloodToxicityState
                {
                    survivorId = kvp.Value.survivorId,
                    chemUsageCount = kvp.Value.chemUsageCount,
                    bloodToxicityLevel = kvp.Value.bloodToxicityLevel,
                    chemThreshold = kvp.Value.chemThreshold,
                    isBloodToxic = kvp.Value.isBloodToxic
                });
            }
            return save;
        }

        public void RestoreState(BloodToxicitySave save)
        {
            _states.Clear();
            if (save?.entries == null) return;
            foreach (var entry in save.entries)
            {
                _states[entry.survivorId] = new BloodToxicityState
                {
                    survivorId = entry.survivorId,
                    chemUsageCount = entry.chemUsageCount,
                    bloodToxicityLevel = entry.bloodToxicityLevel,
                    chemThreshold = entry.chemThreshold,
                    isBloodToxic = entry.isBloodToxic
                };
            }
        }
    }
}
