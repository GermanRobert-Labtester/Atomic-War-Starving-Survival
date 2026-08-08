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
    /// JsonUtility-safe snapshot for per-survivor leech attachments (parallel arrays; no Dictionary).
    /// </summary>
    [Serializable]
    public class LeechesSaveState
    {
        public string encounterId = "encounter_leeches";
        public string[] survivorIds = Array.Empty<string>();
        public int[] attachedLeechCounts = Array.Empty<int>();
        public float[] totalRadDrained = Array.Empty<float>();
        public float radDrainPerTick = 5f;
        public float bloodLossPerTick = 8f;
        public int maxLeeches = 10;
    }

    /// <summary>
    /// Prompt #557: Encounter: Radioactive Leeches.
    /// Triggers when wading through Flooded nodes.
    /// Leeches attach silently, drain Radiation (natural Rad-Away).
    /// Also cause severe BloodLoss. Player must choose when to burn them off.
    /// </summary>
    /// <summary>DEMOTE-Encounter-batch — dormant ghost; SO expedition encounters remain live. Re-promote with Boot+Save+host.</summary>
    public class Encounter_Leeches
    {
        private Dictionary<string, LeechesState> _attachedBySurvivorId = new Dictionary<string, LeechesState>();

        public event Action<string, LeechesState> OnLeechesAttached;
        public event Action<string, LeechesState> OnLeechesBurnedOff;
        public event Action<string, LeechesState> OnBloodLossCritical;

        public Dictionary<string, LeechesState> AttachedBySurvivorId => _attachedBySurvivorId;

        public void AttachLeeches(string survivorId, int count)
        {
            if (string.IsNullOrEmpty(survivorId))
                return;

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

        public LeechesSaveState CaptureState()
        {
            int n = _attachedBySurvivorId.Count;
            var ids = new string[n];
            var counts = new int[n];
            var rads = new float[n];
            float radDrain = 5f;
            float bloodLoss = 8f;
            int maxLeeches = 10;
            int i = 0;
            foreach (var kvp in _attachedBySurvivorId)
            {
                ids[i] = kvp.Key;
                counts[i] = kvp.Value.attachedLeechCount;
                rads[i] = kvp.Value.totalRadDrained;
                radDrain = kvp.Value.radDrainPerTick;
                bloodLoss = kvp.Value.bloodLossPerTick;
                maxLeeches = kvp.Value.maxLeeches;
                i++;
            }

            return new LeechesSaveState
            {
                encounterId = "encounter_leeches",
                survivorIds = ids,
                attachedLeechCounts = counts,
                totalRadDrained = rads,
                radDrainPerTick = radDrain,
                bloodLossPerTick = bloodLoss,
                maxLeeches = maxLeeches
            };
        }

        public void RestoreState(LeechesSaveState saved)
        {
            _attachedBySurvivorId = new Dictionary<string, LeechesState>();
            if (saved == null || saved.survivorIds == null)
                return;

            int n = saved.survivorIds.Length;
            for (int i = 0; i < n; i++)
            {
                string id = saved.survivorIds[i];
                if (string.IsNullOrEmpty(id))
                    continue;

                int count = saved.attachedLeechCounts != null && i < saved.attachedLeechCounts.Length
                    ? saved.attachedLeechCounts[i]
                    : 0;
                float rad = saved.totalRadDrained != null && i < saved.totalRadDrained.Length
                    ? saved.totalRadDrained[i]
                    : 0f;

                _attachedBySurvivorId[id] = new LeechesState
                {
                    attachedLeechCount = count,
                    totalRadDrained = rad,
                    radDrainPerTick = saved.radDrainPerTick,
                    bloodLossPerTick = saved.bloodLossPerTick,
                    maxLeeches = saved.maxLeeches
                };
            }
        }
    }
}
