using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class KleptomaniacState
    {
        public string traitId = "trait_kleptomaniac";
        public bool isStealing;
        public int escalationLevel; // 0=useless, 1=small, 2=critical
        public List<string> stolenItems = new List<string>();
    }

    /// <summary>
    /// Kleptomaniac trait system. Triggered by prolonged depression, a survivor
    /// begins compulsively stealing items from the shelter — escalating from
    /// worthless junk to critical survival supplies.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    
    [Serializable]
    public class TraitKleptomaniacSave
    {
        public List<string> keys = new List<string>();
        public List<KleptomaniacState> values = new List<KleptomaniacState>();
    }
public class Trait_Kleptomaniac
    {
        // ── Events ──────────────────────────────────────────────────────
        public event Action<string, string> OnItemStolen;        // survivorId, itemId
        public event Action<string, int> OnEscalationIncreased;  // survivorId, level

        // ── State ───────────────────────────────────────────────────────
        private Dictionary<string, KleptomaniacState> _states = new Dictionary<string, KleptomaniacState>();

        // ── Item pools per escalation level ─────────────────────────────
        private static readonly string[] Level0Items = { "screw", "paper", "rag", "broken_pencil", "rubber_band" };
        private static readonly string[] Level1Items = { "wrench", "pliers", "hammer", "flashlight", "rope" };
        private static readonly string[] Level2Items = { "ammunition", "iodine_pills", "rad_away", "gas_mask_filter", "clean_water" };

        // ── Public API ──────────────────────────────────────────────────

        /// <summary>
        /// Called when a survivor's depression is high enough for long enough.
        /// Initialises the trait state if it doesn't exist yet.
        /// </summary>
        public void DevelopTrait(string survivorId, float depressionLevel)
        {
            if (string.IsNullOrEmpty(survivorId)) return;

            if (!_states.ContainsKey(survivorId))
            {
                _states[survivorId] = new KleptomaniacState
                {
                    traitId = "trait_kleptomaniac",
                    isStealing = false,
                    escalationLevel = 0
                };
            }

            // Depression threshold to activate stealing (0–1 scale assumed)
            if (depressionLevel >= 0.7f)
            {
                _states[survivorId].isStealing = true;
            }
        }

        /// <summary>
        /// Tick once per in-game hour. If the survivor is actively stealing,
        /// roll the dice: they may pocket an item and the escalation may rise.
        /// </summary>
        public void TickHour(string survivorId, System.Random rng)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_states.TryGetValue(survivorId, out var state)) return;
            if (!state.isStealing) return;

            // ~15 % chance per hour to steal something
            if (rng == null || rng.NextDouble() > 0.15) return;

            string[] pool;
            switch (state.escalationLevel)
            {
                case 0:  pool = Level0Items; break;
                case 1:  pool = Level1Items; break;
                default: pool = Level2Items; break;
            }

            string stolenId = pool[rng.Next(pool.Length)];
            state.stolenItems.Add(stolenId);
            OnItemStolen?.Invoke(survivorId, stolenId);

            // Escalation: after accumulating enough stolen items, level up
            int threshold;
            switch (state.escalationLevel)
            {
                case 0:  threshold = 5;  break; // 5 useless items → escalate
                case 1:  threshold = 3;  break; // 3 small items  → escalate
                default: threshold = int.MaxValue; break; // already max
            }

            if (state.stolenItems.Count >= threshold && state.escalationLevel < 2)
            {
                state.escalationLevel++;
                OnEscalationIncreased?.Invoke(survivorId, state.escalationLevel);
            }
        }

        /// <summary>
        /// Returns a copy of the stolen-item list for a given survivor.
        /// </summary>
        public List<string> GetStolenItems(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return new List<string>();
            if (!_states.TryGetValue(survivorId, out var state)) return new List<string>();
            return new List<string>(state.stolenItems);
        }

        // ── Save / Load ─────────────────────────────────────────────────

        public TraitKleptomaniacSave CaptureState()
        {
            var save = new TraitKleptomaniacSave();
            foreach (var kvp in _states)
            {
                save.keys.Add(kvp.Key);
                save.values.Add(kvp.Value);
            }
            return save;
        }

        public void RestoreState(TraitKleptomaniacSave saved)
        {
            _states.Clear();
            if (saved == null || saved.keys == null) return;
            for (int i = 0; i < saved.keys.Count; i++)
            {
                var val = (saved.values != null && i < saved.values.Count) ? saved.values[i] : null;
                if (val != null) _states[saved.keys[i]] = val;
            }
        }
    }
}
