using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class LeechesState
    {
        public string id = "encounter_leeches";
        public string displayName = "Radioactive Leeches";
        public int attachedLeechCount = 0;
        public float radDrainPerTick = 5f;
        public float bloodLossPerTick = 8f;
        public int maxLeeches = 10;
        public float totalRadDrained = 0f;
    }

    /// <summary>
    /// Prompt #557: Encounter: Radioactive Leeches.
    /// Triggers when wading through Flooded nodes.
    /// Leeches attach silently, drain Radiation (natural Rad-Away).
    /// Also cause severe BloodLoss. Player must choose when to burn them off.
    /// </summary>
    public class Encounter_Leeches
    {
        private Dictionary<string, LeechesState> _attachedBySurvivorId = new Dictionary<string, LeechesState>();

        public event Action<string, LeechesState> OnLeechesAttached;
        public event Action<string, LeechesState> OnLeechesBurnedOff;
        public event Action<string, LeechesState> OnBloodLossCritical;

        public Dictionary<string, LeechesState> AttachedBySurvivorId => _attachedBySurvivorId;

        public void AttachLeeches(string survivorId, int count)
        {
            if (!_attachedBySurvivorId.ContainsKey(survivorId))
            {
                _attachedBySurvivorId[survivorId] = new LeechesState();
            }

            LeechesState state = _attachedBySurvivorId[survivorId];
            state.attachedLeechCount = Mathf.Min(state.attachedLeechCount + count, state.maxLeeches);

            OnLeechesAttached?.Invoke(survivorId, state);
        }

        public void TickHour(string survivorId)
        {
            if (!_attachedBySurvivorId.ContainsKey(survivorId))
            {
                return;
            }

            LeechesState state = _attachedBySurvivorId[survivorId];

            if (state.attachedLeechCount > 0)
            {
                float radDrain = state.radDrainPerTick * state.attachedLeechCount;
                float bloodLoss = state.bloodLossPerTick * state.attachedLeechCount;

                state.totalRadDrained += radDrain;

                // Check for critical blood loss (arbitrary threshold)
                if (bloodLoss > 50f)
                {
                    OnBloodLossCritical?.Invoke(survivorId, state);
                }
            }
        }

        public void BurnOffLeeches(string survivorId)
        {
            if (!_attachedBySurvivorId.ContainsKey(survivorId))
            {
                return;
            }

            LeechesState state = _attachedBySurvivorId[survivorId];
            state.attachedLeechCount = 0;

            OnLeechesBurnedOff?.Invoke(survivorId, state);
        }

        public float GetNetRadReduction(string survivorId)
        {
            if (!_attachedBySurvivorId.ContainsKey(survivorId))
            {
                return 0f;
            }

            return _attachedBySurvivorId[survivorId].totalRadDrained;
        }
    }
}
