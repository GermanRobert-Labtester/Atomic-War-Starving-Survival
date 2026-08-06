using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PheromoneMaskingState
    {
        public string survivorId;
        public float hoursRemaining = 0f;
        public bool isActive = false;
        public string sourceCreature = "";
    }

    [Serializable]
    public class PheromoneMaskingSave
    {
        public List<PheromoneMaskingState> entries = new List<PheromoneMaskingState>();
    }

    /// <summary>
    /// Prompt #558: System: Pheromone Masking (Mutant Camo).
    /// Item_MutantGland harvested from Amalgamations or Bears is crushed and
    /// spread on a HazmatSuit. For 24 hours all mutated/feral animals treat the
    /// survivor as friendly, but human NPCs shoot on sight out of disgust/fear.
    /// </summary>
    public class PheromoneMaskingSystem
    {
        private const float MaskingDurationHours = 24f;

        /// <summary>Creature types considered mutated/feral (friendly under masking).</summary>
        private static readonly HashSet<string> MutatedCreatureTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "feral_dog",
            "amalgamation",
            "mutant_bear",
            "mutant_rat",
            "feral_cat",
            "irradiated_crow"
        };

        private static readonly HashSet<string> HumanNpcTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "bandit",
            "military_patrol",
            "mercenary",
            "scavenger",
            "faction_scout",
            "civilian"
        };

        private readonly Dictionary<string, PheromoneMaskingState> _states = new Dictionary<string, PheromoneMaskingState>();

        public event Action<string, string> OnPheromoneApplied;       // (survivorId, sourceCreature)
        public event Action<string> OnPheromoneExpired;               // (survivorId)
        public event Action<string, string> OnHostileHumanReaction;   // (survivorId, npcType)

        public IReadOnlyDictionary<string, PheromoneMaskingState> States => _states;

        /// <summary>
        /// Apply a mutant gland to the survivor's HazmatSuit, activating 24h masking.
        /// </summary>
        public void ApplyMutantGland(string survivorId, string sourceCreature)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(sourceCreature)) return;

            if (!_states.TryGetValue(survivorId, out var state))
            {
                state = new PheromoneMaskingState { survivorId = survivorId };
                _states[survivorId] = state;
            }

            state.hoursRemaining = MaskingDurationHours;
            state.isActive = true;
            state.sourceCreature = sourceCreature;

            OnPheromoneApplied?.Invoke(survivorId, sourceCreature);
        }

        /// <summary>
        /// Advance time. Decrements remaining hours and deactivates masking at 0.
        /// </summary>
        public void TickHour(string survivorId, float hours)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_states.TryGetValue(survivorId, out var state)) return;
            if (!state.isActive) return;

            state.hoursRemaining = Mathf.Max(0f, state.hoursRemaining - hours);

            if (state.hoursRemaining <= 0f)
            {
                state.isActive = false;
                state.hoursRemaining = 0f;
                OnPheromoneExpired?.Invoke(survivorId);
            }
        }

        /// <summary>
        /// Returns the disposition of a creature toward the survivor.
        /// "friendly" if masking is active and creature is mutated/feral.
        /// "hostile_on_sight" if masking is active and creature is a human NPC.
        /// "neutral" otherwise.
        /// </summary>
        public string GetAnimalDisposition(string survivorId, string creatureType)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(creatureType))
                return "neutral";

            if (!_states.TryGetValue(survivorId, out var state) || !state.isActive)
                return "neutral";

            if (MutatedCreatureTypes.Contains(creatureType))
                return "friendly";

            if (HumanNpcTypes.Contains(creatureType))
            {
                OnHostileHumanReaction?.Invoke(survivorId, creatureType);
                return "hostile_on_sight";
            }

            return "neutral";
        }

        /// <summary>True when pheromone masking is currently active for the survivor.</summary>
        public bool IsMaskingActive(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            return _states.TryGetValue(survivorId, out var state) && state.isActive;
        }

        public PheromoneMaskingSave CaptureState()
        {
            var save = new PheromoneMaskingSave();
            foreach (var kvp in _states)
            {
                save.entries.Add(new PheromoneMaskingState
                {
                    survivorId = kvp.Value.survivorId,
                    hoursRemaining = kvp.Value.hoursRemaining,
                    isActive = kvp.Value.isActive,
                    sourceCreature = kvp.Value.sourceCreature
                });
            }
            return save;
        }

        public void RestoreState(PheromoneMaskingSave save)
        {
            _states.Clear();
            if (save?.entries == null) return;
            foreach (var entry in save.entries)
            {
                _states[entry.survivorId] = new PheromoneMaskingState
                {
                    survivorId = entry.survivorId,
                    hoursRemaining = entry.hoursRemaining,
                    isActive = entry.isActive,
                    sourceCreature = entry.sourceCreature
                };
            }
        }
    }
}
