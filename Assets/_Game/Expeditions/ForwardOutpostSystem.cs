using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Expeditions
{
    [Serializable]
    public class OutpostEntry
    {
        public string nodeId;
        public string outpostName;
        public int storedRations = 10;
        public int storedAmmo = 20;
        public List<string> garrisonSurvivorIds = new List<string>();
        public float fortificationLevel = 1.0f; // 1.0 = basic sandbags, 3.0 = reinforced bunker
        public bool isUnderSiege = false;
    }

    [Serializable]
    public class OutpostSystemState
    {
        public List<OutpostEntry> establishedOutposts = new List<OutpostEntry>();
        public int maxOutpostsAllowed = 3;
    }

    /// <summary>
    /// Expansion V / Spec §3.3: Forward Outpost / Surface Camp System.
    /// Allows survivors to establish forward waystations at charted map nodes,
    /// providing travel stamina savings, resupply caches, and regional staging.
    /// </summary>
    public class ForwardOutpostSystem
    {
        private OutpostSystemState _state = new OutpostSystemState();

        public event Action<OutpostEntry> OnOutpostEstablished;
        public event Action<OutpostEntry> OnOutpostSupplied;
        public event Action<OutpostEntry, bool> OnRaidResolved;

        public OutpostSystemState State => _state;
        public int OutpostCount => _state.establishedOutposts?.Count ?? 0;

        public ForwardOutpostSystem(OutpostSystemState state = null)
        {
            _state = state ?? new OutpostSystemState();
            if (_state.establishedOutposts == null)
                _state.establishedOutposts = new List<OutpostEntry>();
        }

        public bool TryEstablishOutpost(string nodeId, string name, float initialFortification = 1.0f)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            if (_state.establishedOutposts.Count >= _state.maxOutpostsAllowed) return false;
            if (_state.establishedOutposts.Exists(o => o.nodeId == nodeId)) return false;

            var outpost = new OutpostEntry
            {
                nodeId = nodeId,
                outpostName = string.IsNullOrEmpty(name) ? $"Outpost {nodeId}" : name,
                fortificationLevel = Mathf.Clamp(initialFortification, 1.0f, 3.0f),
                storedRations = 10,
                storedAmmo = 15,
                garrisonSurvivorIds = new List<string>()
            };

            _state.establishedOutposts.Add(outpost);
            OnOutpostEstablished?.Invoke(outpost);
            return true;
        }

        public OutpostEntry GetOutpost(string nodeId)
        {
            return _state.establishedOutposts.Find(o => o.nodeId == nodeId);
        }

        public bool IsOutpostAt(string nodeId)
        {
            return _state.establishedOutposts.Exists(o => o.nodeId == nodeId);
        }

        public void ResupplyOutpost(string nodeId, int rations, int ammo)
        {
            var outpost = GetOutpost(nodeId);
            if (outpost == null) return;

            outpost.storedRations += Mathf.Max(0, rations);
            outpost.storedAmmo += Mathf.Max(0, ammo);
            OnOutpostSupplied?.Invoke(outpost);
        }

        public float CalculateTravelFatigueMultiplier(string destinationNodeId, float baseFatigue)
        {
            if (IsOutpostAt(destinationNodeId))
            {
                // Outposts provide resting spots, reducing travel fatigue by 40%
                return baseFatigue * 0.6f;
            }
            return baseFatigue;
        }

        public bool ResolveRaid(string nodeId, float raidStrength, int worldSeed)
        {
            var outpost = GetOutpost(nodeId);
            if (outpost == null) return false;

            float defensePower = (outpost.fortificationLevel * 20f)
                                 + (outpost.garrisonSurvivorIds.Count * 15f)
                                 + (outpost.storedAmmo * 1.5f);

            var rng = new System.Random(unchecked(worldSeed * 37 + nodeId.GetHashCode()));
            float roll = (float)rng.NextDouble() * 20f;
            bool success = (defensePower + roll) >= raidStrength;

            if (success)
            {
                outpost.storedAmmo = Mathf.Max(0, outpost.storedAmmo - 5);
            }
            else
            {
                outpost.storedRations = Mathf.Max(0, outpost.storedRations - 8);
                outpost.storedAmmo = Mathf.Max(0, outpost.storedAmmo - 10);
                outpost.fortificationLevel = Mathf.Max(1.0f, outpost.fortificationLevel - 0.5f);
            }

            OnRaidResolved?.Invoke(outpost, success);
            return success;
        }

        public OutpostSystemState CaptureState() => _state;

        public void RestoreState(OutpostSystemState state)
        {
            _state = state ?? new OutpostSystemState();
            if (_state.establishedOutposts == null)
                _state.establishedOutposts = new List<OutpostEntry>();
        }
    }
}
