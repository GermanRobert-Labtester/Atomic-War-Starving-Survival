using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BunkerFeverState
    {
        public string afflictionId = "affliction_bunker_fever";
        public int daysIndoorsThreshold = 30;
        public float selfHarmDamage = 5f;
        public int daysIndoors;
        public bool hasFever;
    }

    /// <summary>
    /// Bunker fever: prolonged confinement inside the shelter causes an
    /// uncontrollable itch that escalates to self-laceration. The only cure
    /// is leaving the bunker on an expedition.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    
    [Serializable]
    public class AfflictionBunkerFeverSave
    {
        public List<string> keys = new List<string>();
        public List<BunkerFeverState> values = new List<BunkerFeverState>();
    }
public class Affliction_BunkerFever
    {
        // ── Events ──────────────────────────────────────────────────────
        public event Action<string> OnItchStarted;          // survivorId
        public event Action<string, float> OnSelfHarm;      // survivorId, damage
        public event Action<string> OnFeverCured;           // survivorId

        // ── State ───────────────────────────────────────────────────────
        private Dictionary<string, BunkerFeverState> _states = new Dictionary<string, BunkerFeverState>();

        // ── Public API ──────────────────────────────────────────────────

        /// <summary>
        /// Called once per in-game day. Tracks consecutive days spent indoors.
        /// After exceeding the threshold the survivor develops bunker fever.
        /// </summary>
        public void TickDay(string survivorId, bool wentOutside)
        {
            if (string.IsNullOrEmpty(survivorId)) return;

            if (!_states.ContainsKey(survivorId))
            {
                _states[survivorId] = new BunkerFeverState
                {
                    afflictionId = "affliction_bunker_fever",
                    daysIndoorsThreshold = 30,
                    selfHarmDamage = 5f,
                    daysIndoors = 0,
                    hasFever = false
                };
            }

            var state = _states[survivorId];

            if (wentOutside)
            {
                // Going outside resets the indoor counter
                state.daysIndoors = 0;

                // If they already had fever, cure it
                if (state.hasFever)
                {
                    state.hasFever = false;
                    OnFeverCured?.Invoke(survivorId);
                }
                return;
            }

            // Stayed inside another day
            state.daysIndoors++;

            if (!state.hasFever && state.daysIndoors > state.daysIndoorsThreshold)
            {
                state.hasFever = true;
                OnItchStarted?.Invoke(survivorId);
            }
        }

        /// <summary>
        /// Applies self-harm damage from scratching / lacerations when the
        /// survivor has active bunker fever.
        /// </summary>
        public void ApplySelfHarm(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_states.TryGetValue(survivorId, out var state)) return;
            if (!state.hasFever) return;

            float damage = state.selfHarmDamage;
            OnSelfHarm?.Invoke(survivorId, damage);
        }

        /// <summary>
        /// Cures the fever. Mechanically identical to going outside via
        /// TickDay(wentOutside:true), but available for direct invocation
        /// (e.g. from a narrative event).
        /// </summary>
        public void Cure(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_states.TryGetValue(survivorId, out var state)) return;
            if (!state.hasFever) return;

            state.hasFever = false;
            state.daysIndoors = 0;
            OnFeverCured?.Invoke(survivorId);
        }

        /// <summary>
        /// Returns true if the survivor currently has bunker fever.
        /// </summary>
        public bool HasFever(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (!_states.TryGetValue(survivorId, out var state)) return false;
            return state.hasFever;
        }

        // ── Save / Load ─────────────────────────────────────────────────

        public AfflictionBunkerFeverSave CaptureState()
        {
            var save = new AfflictionBunkerFeverSave();
            foreach (var kvp in _states)
            {
                save.keys.Add(kvp.Key);
                save.values.Add(kvp.Value);
            }
            return save;
        }

        public void RestoreState(AfflictionBunkerFeverSave saved)
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
