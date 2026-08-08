using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// DEMOTE-001 — content ghost. Not constructed by BootActionFamily and not
    /// save-wired until a medical/mutagenesis host calls <see cref="Stabilize"/>.
    /// Kept as dormant design for re-promotion (do not delete the class).
    /// </summary>
    [Serializable]
    public class StabilizeDNAState
    {
        public string actionId = "action_stabilize_dna";
        public bool requiresSurgeon = true;
        public bool requiresImmunosuppressants = true;
    }

    /// <summary>DEMOTE-001 — dormant Action tracker (see type remarks on state).</summary>
    public class Action_StabilizeDNA
    {
        public event Action<string, string> OnMutationStabilized;
        public event Action<string> OnStabilizationFailed;

        private readonly StabilizeDNAState _state;

        public Action_StabilizeDNA()
        {
            _state = new StabilizeDNAState();
        }

        public bool Stabilize(string survivorId, string mutationId, bool isSurgeon, bool hasImmunosuppressants)
        {
            if (!isSurgeon || !hasImmunosuppressants)
            {
                OnStabilizationFailed?.Invoke(survivorId);
                return false;
            }

            OnMutationStabilized?.Invoke(survivorId, mutationId);
            return true;
        }

        public StabilizeDNAState CaptureState() => _state;

        public void RestoreState(StabilizeDNAState state)
        {
            _state.actionId = state.actionId;
            _state.requiresSurgeon = state.requiresSurgeon;
            _state.requiresImmunosuppressants = state.requiresImmunosuppressants;
        }
    }
}
