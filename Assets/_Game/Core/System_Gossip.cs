using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GossipRumor
    {
        public string witnessId;
        public string criminalId;
        public string crimeType;
        public int dayStarted;
        public int spreadCount;
        public List<string> informedIds = new List<string>();
    }

    [Serializable]
    public class GossipSystemState
    {
        public string system_id = "system_gossip";
        public List<GossipRumor> rumors = new List<GossipRumor>();
        // Parallel lists — JsonUtility cannot serialize Dictionary.
        public List<string> affinity_decay_ids = new List<string>();
        public List<float> affinity_decay_values = new List<float>();
        public List<string> allSurvivorIds = new List<string>();
    }

    /// <summary>
    /// Prompt #839: Bunker Gossip — Witness sees crime, gossips to friends,
    /// criminal's Affinity with bunker rots over days as knowledge spreads.
    /// </summary>
    public class System_Gossip
    {
        public const float AffinityDecayPerInformed = 0.02f;
        public const int MaxTellsPerDay = 2;

        private GossipSystemState _state = new GossipSystemState();
        private readonly Dictionary<string, float> _affinityDecayMap =
            new Dictionary<string, float>(StringComparer.Ordinal);

        public event Action<string, string, string> OnCrimeWitnessed;   // witnessId, criminalId, crime
        public event Action<string, string, string> OnRumorSpread;      // fromId, toId, criminalId
        public event Action<string, string, float> OnAffinityDecayed;   // criminalId, targetId, amount

        public int RumorCount => _state.rumors != null ? _state.rumors.Count : 0;
        public IReadOnlyList<GossipRumor> Rumors =>
            _state.rumors ?? (IReadOnlyList<GossipRumor>)Array.Empty<GossipRumor>();

        /// <summary>
        /// Registers the full survivor roster used for spread targeting and affinity decay.
        /// </summary>
        public void SetSurvivorRoster(IList<string> survivorIds)
        {
            _state.allSurvivorIds = new List<string>();
            if (survivorIds == null) return;
            for (int i = 0; i < survivorIds.Count; i++)
            {
                if (string.IsNullOrEmpty(survivorIds[i])) continue;
                if (!_state.allSurvivorIds.Contains(survivorIds[i]))
                    _state.allSurvivorIds.Add(survivorIds[i]);
            }
        }

        /// <summary>
        /// A witness observes a crime and begins the gossip chain.
        /// </summary>
        public void WitnessCrime(string witnessId, string criminalId, string crimeType, int day)
        {
            if (string.IsNullOrEmpty(witnessId) || string.IsNullOrEmpty(criminalId)) return;
            if (string.Equals(witnessId, criminalId, StringComparison.Ordinal)) return;
            if (string.IsNullOrEmpty(crimeType)) crimeType = "unknown";

            if (_state.rumors == null)
                _state.rumors = new List<GossipRumor>();

            for (int i = 0; i < _state.rumors.Count; i++)
            {
                var r = _state.rumors[i];
                if (r == null) continue;
                if (r.witnessId == witnessId && r.criminalId == criminalId && r.crimeType == crimeType)
                    return;
            }

            var rumor = new GossipRumor
            {
                witnessId = witnessId,
                criminalId = criminalId,
                crimeType = crimeType,
                dayStarted = day,
                spreadCount = 1,
                informedIds = new List<string> { witnessId }
            };
            _state.rumors.Add(rumor);

            EnsureRosterContains(witnessId);
            EnsureRosterContains(criminalId);

            OnCrimeWitnessed?.Invoke(witnessId, criminalId, crimeType);
        }

        /// <summary>
        /// Called once per day. Each rumor spreads to up to 2 uninformed survivors.
        /// Affinity decay = 0.02 per person who knows the rumor (applied to each other survivor).
        /// </summary>
        public void TickDay()
        {
            if (_state.rumors == null || _state.rumors.Count == 0) return;
            if (_state.allSurvivorIds == null || _state.allSurvivorIds.Count == 0) return;

            var newInformed = PickTellTargets();
            InformTargets(newInformed);
            ApplyAffinityDecay();
        }

        /// <summary>
        /// For each rumor, pick up to <see cref="MaxTellsPerDay"/> survivors who
        /// don't already know and aren't the criminal. Selection is snapshotted
        /// before any rumor.informedIds is mutated, so a survivor told about rumor
        /// A this tick can still be picked as a target for rumor B.
        /// </summary>
        private List<(GossipRumor rumor, List<string> targets)> PickTellTargets()
        {
            var newInformed = new List<(GossipRumor rumor, List<string> targets)>();

            for (int ri = 0; ri < _state.rumors.Count; ri++)
            {
                var rumor = _state.rumors[ri];
                if (rumor == null) continue;
                if (rumor.informedIds == null)
                    rumor.informedIds = new List<string>();

                var targets = new List<string>();
                int tellCount = 0;

                for (int si = 0; si < _state.allSurvivorIds.Count; si++)
                {
                    if (tellCount >= MaxTellsPerDay) break;
                    string survivorId = _state.allSurvivorIds[si];
                    if (string.IsNullOrEmpty(survivorId)) continue;
                    if (rumor.informedIds.Contains(survivorId)) continue;
                    // Criminal already knows their own crime; skip as gossip target.
                    if (string.Equals(survivorId, rumor.criminalId, StringComparison.Ordinal)) continue;

                    targets.Add(survivorId);
                    tellCount++;
                }

                if (targets.Count > 0)
                    newInformed.Add((rumor, targets));
            }
            return newInformed;
        }

        private void InformTargets(List<(GossipRumor rumor, List<string> targets)> newInformed)
        {
            for (int i = 0; i < newInformed.Count; i++)
            {
                var rumor = newInformed[i].rumor;
                var targets = newInformed[i].targets;
                string spreader = rumor.informedIds.Count > 0
                    ? rumor.informedIds[rumor.informedIds.Count - 1]
                    : rumor.witnessId;

                for (int t = 0; t < targets.Count; t++)
                {
                    string target = targets[t];
                    if (rumor.informedIds.Contains(target)) continue;
                    rumor.informedIds.Add(target);
                    rumor.spreadCount++;
                    OnRumorSpread?.Invoke(spreader, target, rumor.criminalId);
                }
            }
        }

        /// <summary>Affinity decay: <see cref="AffinityDecayPerInformed"/> per person who knows, once per rumor per day.</summary>
        private void ApplyAffinityDecay()
        {
            for (int ri = 0; ri < _state.rumors.Count; ri++)
            {
                var rumor = _state.rumors[ri];
                if (rumor == null || rumor.informedIds == null) continue;
                if (string.IsNullOrEmpty(rumor.criminalId)) continue;

                float decay = AffinityDecayPerInformed * rumor.informedIds.Count;
                if (decay <= 0f) continue;

                if (!_affinityDecayMap.ContainsKey(rumor.criminalId))
                    _affinityDecayMap[rumor.criminalId] = 0f;
                _affinityDecayMap[rumor.criminalId] += decay;

                for (int si = 0; si < _state.allSurvivorIds.Count; si++)
                {
                    string survivorId = _state.allSurvivorIds[si];
                    if (string.IsNullOrEmpty(survivorId)) continue;
                    if (string.Equals(survivorId, rumor.criminalId, StringComparison.Ordinal)) continue;
                    OnAffinityDecayed?.Invoke(rumor.criminalId, survivorId, decay);
                }
            }
        }

        /// <summary>
        /// Returns the total accumulated affinity decay for a criminal.
        /// </summary>
        public float GetAffinityDecay(string criminalId)
        {
            if (string.IsNullOrEmpty(criminalId)) return 0f;
            return _affinityDecayMap.TryGetValue(criminalId, out float decay) ? decay : 0f;
        }

        /// <summary>
        /// Manually spread a rumor from a specific witness to one new listener.
        /// </summary>
        public void SpreadRumor(string witnessId)
        {
            if (string.IsNullOrEmpty(witnessId) || _state.rumors == null) return;
            if (_state.allSurvivorIds == null) return;

            for (int ri = 0; ri < _state.rumors.Count; ri++)
            {
                var rumor = _state.rumors[ri];
                if (rumor == null || rumor.witnessId != witnessId) continue;
                if (rumor.informedIds == null)
                    rumor.informedIds = new List<string>();

                for (int si = 0; si < _state.allSurvivorIds.Count; si++)
                {
                    string survivorId = _state.allSurvivorIds[si];
                    if (string.IsNullOrEmpty(survivorId)) continue;
                    if (rumor.informedIds.Contains(survivorId)) continue;
                    if (string.Equals(survivorId, rumor.criminalId, StringComparison.Ordinal)) continue;

                    rumor.informedIds.Add(survivorId);
                    rumor.spreadCount++;
                    OnRumorSpread?.Invoke(witnessId, survivorId, rumor.criminalId);
                    return;
                }
            }
        }

        /// <summary>
        /// Returns true if the survivor has heard a rumor about the criminal.
        /// </summary>
        public bool HasHeardRumor(string survivorId, string criminalId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(criminalId)) return false;
            if (_state.rumors == null) return false;

            for (int i = 0; i < _state.rumors.Count; i++)
            {
                var rumor = _state.rumors[i];
                if (rumor == null || rumor.informedIds == null) continue;
                if (rumor.criminalId == criminalId && rumor.informedIds.Contains(survivorId))
                    return true;
            }
            return false;
        }

        /// <summary>How many people know about a criminal's crimes (union across rumors).</summary>
        public int GetInformedCount(string criminalId)
        {
            if (string.IsNullOrEmpty(criminalId) || _state.rumors == null) return 0;
            var set = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < _state.rumors.Count; i++)
            {
                var rumor = _state.rumors[i];
                if (rumor == null || rumor.criminalId != criminalId || rumor.informedIds == null) continue;
                for (int j = 0; j < rumor.informedIds.Count; j++)
                {
                    if (!string.IsNullOrEmpty(rumor.informedIds[j]))
                        set.Add(rumor.informedIds[j]);
                }
            }
            return set.Count;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public GossipSystemState CaptureState()
        {
            var copy = new GossipSystemState
            {
                system_id = "system_gossip",
                rumors = new List<GossipRumor>(),
                affinity_decay_ids = new List<string>(),
                affinity_decay_values = new List<float>(),
                allSurvivorIds = new List<string>()
            };

            if (_state.rumors != null)
            {
                for (int i = 0; i < _state.rumors.Count; i++)
                {
                    var r = _state.rumors[i];
                    if (r == null) continue;
                    var rc = new GossipRumor
                    {
                        witnessId = r.witnessId,
                        criminalId = r.criminalId,
                        crimeType = r.crimeType,
                        dayStarted = r.dayStarted,
                        spreadCount = r.spreadCount,
                        informedIds = new List<string>()
                    };
                    if (r.informedIds != null)
                    {
                        for (int j = 0; j < r.informedIds.Count; j++)
                            if (!string.IsNullOrEmpty(r.informedIds[j]))
                                rc.informedIds.Add(r.informedIds[j]);
                    }
                    copy.rumors.Add(rc);
                }
            }

            foreach (var kv in _affinityDecayMap)
            {
                copy.affinity_decay_ids.Add(kv.Key);
                copy.affinity_decay_values.Add(kv.Value);
            }

            if (_state.allSurvivorIds != null)
            {
                for (int i = 0; i < _state.allSurvivorIds.Count; i++)
                    if (!string.IsNullOrEmpty(_state.allSurvivorIds[i]))
                        copy.allSurvivorIds.Add(_state.allSurvivorIds[i]);
            }

            return copy;
        }

        public void RestoreState(GossipSystemState saved)
        {
            _affinityDecayMap.Clear();
            if (saved == null)
            {
                _state = new GossipSystemState();
                return;
            }

            _state = new GossipSystemState
            {
                system_id = "system_gossip",
                rumors = new List<GossipRumor>(),
                affinity_decay_ids = new List<string>(),
                affinity_decay_values = new List<float>(),
                allSurvivorIds = new List<string>()
            };

            if (saved.rumors != null)
            {
                for (int i = 0; i < saved.rumors.Count; i++)
                {
                    var r = saved.rumors[i];
                    if (r == null) continue;
                    var rc = new GossipRumor
                    {
                        witnessId = r.witnessId,
                        criminalId = r.criminalId,
                        crimeType = r.crimeType,
                        dayStarted = r.dayStarted,
                        spreadCount = r.spreadCount,
                        informedIds = new List<string>()
                    };
                    if (r.informedIds != null)
                    {
                        for (int j = 0; j < r.informedIds.Count; j++)
                            if (!string.IsNullOrEmpty(r.informedIds[j]))
                                rc.informedIds.Add(r.informedIds[j]);
                    }
                    _state.rumors.Add(rc);
                }
            }

            if (saved.affinity_decay_ids != null && saved.affinity_decay_values != null)
            {
                int n = Mathf.Min(saved.affinity_decay_ids.Count, saved.affinity_decay_values.Count);
                for (int i = 0; i < n; i++)
                {
                    if (string.IsNullOrEmpty(saved.affinity_decay_ids[i])) continue;
                    _affinityDecayMap[saved.affinity_decay_ids[i]] = saved.affinity_decay_values[i];
                }
            }

            if (saved.allSurvivorIds != null)
            {
                for (int i = 0; i < saved.allSurvivorIds.Count; i++)
                    if (!string.IsNullOrEmpty(saved.allSurvivorIds[i]))
                        _state.allSurvivorIds.Add(saved.allSurvivorIds[i]);
            }
        }

        private void EnsureRosterContains(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (_state.allSurvivorIds == null)
                _state.allSurvivorIds = new List<string>();
            if (!_state.allSurvivorIds.Contains(id))
                _state.allSurvivorIds.Add(id);
        }
    }
}
