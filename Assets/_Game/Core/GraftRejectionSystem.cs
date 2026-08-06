using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GraftRejectionState
    {
        public string survivorId;
        public string limbId;
        public int daysSinceLastImmunosuppressant = 0;
        public int rejectionThresholdDays = 7;
        public float limbEfficiency = 100f;
        public bool isRejecting = false;
        public bool hasSepsis = false;
    }

    [Serializable]
    public class GraftRejectionSave
    {
        public List<GraftRejectionState> entries = new List<GraftRejectionState>();
    }

    /// <summary>
    /// Prompt #556: System: Graft Rejection (Cybernetics).
    /// Prosthetics and Bionics trigger immune response. The survivor must consume
    /// Immunosuppressants (rare meds) every 7 days, or the body rejects the limb:
    /// causes Sepsis and drops limb efficiency to 0%.
    /// </summary>
    public class GraftRejectionSystem
    {
        private readonly Dictionary<string, GraftRejectionState> _states = new Dictionary<string, GraftRejectionState>();

        public event Action<string, string> OnGraftRegistered;                // (survivorId, limbId)
        public event Action<string> OnRejectionStarted;                       // (survivorId)
        public event Action<string> OnSepsisFromRejection;                    // (survivorId)
        public event Action<string> OnImmunosuppressantAdministered;          // (survivorId)

        public IReadOnlyDictionary<string, GraftRejectionState> States => _states;

        /// <summary>
        /// Register a new graft (prosthetic/bionic limb) for a survivor.
        /// Resets the immunosuppressant counter to day 0 with full efficiency.
        /// </summary>
        public void RegisterGraft(string survivorId, string limbId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(limbId)) return;

            var state = new GraftRejectionState
            {
                survivorId = survivorId,
                limbId = limbId,
                daysSinceLastImmunosuppressant = 0,
                limbEfficiency = 100f,
                isRejecting = false,
                hasSepsis = false
            };
            _states[survivorId] = state;

            OnGraftRegistered?.Invoke(survivorId, limbId);
        }

        /// <summary>
        /// Advance one day. If the survivor exceeds the rejection threshold (7 days)
        /// without immunosuppressants, the limb is rejected: efficiency drops to 0
        /// and Sepsis sets in.
        /// </summary>
        public void TickDay(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_states.TryGetValue(survivorId, out var state)) return;

            state.daysSinceLastImmunosuppressant++;

            if (state.daysSinceLastImmunosuppressant >= state.rejectionThresholdDays && !state.isRejecting)
            {
                state.isRejecting = true;
                state.limbEfficiency = 0f;
                state.hasSepsis = true;

                OnRejectionStarted?.Invoke(survivorId);
                OnSepsisFromRejection?.Invoke(survivorId);
            }
        }

        /// <summary>
        /// Administer an immunosuppressant: reset the day counter to 0,
        /// restore limb efficiency to 100%, and clear rejection/sepsis flags.
        /// </summary>
        public void AdministerImmunosuppressant(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_states.TryGetValue(survivorId, out var state)) return;

            state.daysSinceLastImmunosuppressant = 0;
            state.limbEfficiency = 100f;
            state.isRejecting = false;
            state.hasSepsis = false;

            OnImmunosuppressantAdministered?.Invoke(survivorId);
        }

        /// <summary>Returns current limb efficiency (0-100). Returns 100 if no graft registered.</summary>
        public float GetLimbEfficiency(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return 100f;
            if (!_states.TryGetValue(survivorId, out var state)) return 100f;
            return state.limbEfficiency;
        }

        public GraftRejectionSave CaptureState()
        {
            var save = new GraftRejectionSave();
            foreach (var kvp in _states)
            {
                save.entries.Add(new GraftRejectionState
                {
                    survivorId = kvp.Value.survivorId,
                    limbId = kvp.Value.limbId,
                    daysSinceLastImmunosuppressant = kvp.Value.daysSinceLastImmunosuppressant,
                    rejectionThresholdDays = kvp.Value.rejectionThresholdDays,
                    limbEfficiency = kvp.Value.limbEfficiency,
                    isRejecting = kvp.Value.isRejecting,
                    hasSepsis = kvp.Value.hasSepsis
                });
            }
            return save;
        }

        public void RestoreState(GraftRejectionSave save)
        {
            _states.Clear();
            if (save?.entries == null) return;
            foreach (var entry in save.entries)
            {
                _states[entry.survivorId] = new GraftRejectionState
                {
                    survivorId = entry.survivorId,
                    limbId = entry.limbId,
                    daysSinceLastImmunosuppressant = entry.daysSinceLastImmunosuppressant,
                    rejectionThresholdDays = entry.rejectionThresholdDays,
                    limbEfficiency = entry.limbEfficiency,
                    isRejecting = entry.isRejecting,
                    hasSepsis = entry.hasSepsis
                };
            }
        }
    }
}
