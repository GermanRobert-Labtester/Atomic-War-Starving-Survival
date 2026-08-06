using System;
using System.Collections.Generic;

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
        public List<GossipRumor> rumors = new List<GossipRumor>();
        public Dictionary<string, float> affinityDecayMap = new Dictionary<string, float>();
        public List<string> allSurvivorIds = new List<string>();
    }

    /// <summary>
    /// Prompt #839: Bunker Gossip — Witness sees crime, gossips to friends,
    /// criminal's Affinity with bunker rots over days as knowledge spreads.
    /// </summary>
    public class System_Gossip
    {
        private GossipSystemState _state = new GossipSystemState();

        public event Action<string, string, string> OnCrimeWitnessed;   // witnessId, criminalId, crime
        public event Action<string, string, string> OnRumorSpread;      // fromId, toId, criminalId
        public event Action<string, string, float> OnAffinityDecayed;   // criminalId, targetId, amount

        public GossipSystemState CaptureState() => _state;

        public void RestoreState(GossipSystemState state) => _state = state;

        /// <summary>
        /// Registers the full survivor roster used for spread targeting and affinity decay.
        /// </summary>
        public void SetSurvivorRoster(List<string> survivorIds)
        {
            _state.allSurvivorIds = new List<string>(survivorIds);
        }

        /// <summary>
        /// A witness observes a crime and begins the gossip chain.
        /// </summary>
        public void WitnessCrime(string witnessId, string criminalId, string crimeType, int day)
        {
            // Check if this exact rumor already exists
            foreach (var r in _state.rumors)
            {
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

            OnCrimeWitnessed?.Invoke(witnessId, criminalId, crimeType);
        }

        /// <summary>
        /// Called once per day. Each person who knows tells 1-2 friends.
        /// Affinity decay = 0.02 per person who knows the rumor.
        /// </summary>
        public void TickDay()
        {
            var newInformed = new List<(GossipRumor rumor, List<string> targets)>();

            foreach (var rumor in _state.rumors)
            {
                var targets = new List<string>();
                int tellCount = 0;

                foreach (var survivorId in _state.allSurvivorIds)
                {
                    if (tellCount >= 2) break;
                    if (rumor.informedIds.Contains(survivorId)) continue;

                    targets.Add(survivorId);
                    tellCount++;
                }

                if (targets.Count > 0)
                {
                    newInformed.Add((rumor, targets));
                }
            }

            foreach (var (rumor, targets) in newInformed)
            {
                string spreader = rumor.informedIds[rumor.informedIds.Count - 1];
                foreach (var target in targets)
                {
                    rumor.informedIds.Add(target);
                    rumor.spreadCount++;
                    OnRumorSpread?.Invoke(spreader, target, rumor.criminalId);
                }
            }

            // Apply affinity decay: 0.02 per person who knows
            foreach (var rumor in _state.rumors)
            {
                float decay = 0.02f * rumor.informedIds.Count;

                if (!_state.affinityDecayMap.ContainsKey(rumor.criminalId))
                    _state.affinityDecayMap[rumor.criminalId] = 0f;
                _state.affinityDecayMap[rumor.criminalId] += decay;

                foreach (var survivorId in _state.allSurvivorIds)
                {
                    if (survivorId != rumor.criminalId)
                    {
                        OnAffinityDecayed?.Invoke(rumor.criminalId, survivorId, decay);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the total accumulated affinity decay for a criminal.
        /// </summary>
        public float GetAffinityDecay(string criminalId)
        {
            return _state.affinityDecayMap.TryGetValue(criminalId, out float decay) ? decay : 0f;
        }

        /// <summary>
        /// Manually spread a rumor from a specific witness.
        /// </summary>
        public void SpreadRumor(string witnessId)
        {
            foreach (var rumor in _state.rumors)
            {
                if (rumor.witnessId == witnessId)
                {
                    foreach (var survivorId in _state.allSurvivorIds)
                    {
                        if (!rumor.informedIds.Contains(survivorId))
                        {
                            rumor.informedIds.Add(survivorId);
                            rumor.spreadCount++;
                            OnRumorSpread?.Invoke(witnessId, survivorId, rumor.criminalId);
                            break;
                        }
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Returns true if the survivor has heard a rumor about the criminal.
        /// </summary>
        public bool HasHeardRumor(string survivorId, string criminalId)
        {
            foreach (var rumor in _state.rumors)
            {
                if (rumor.criminalId == criminalId && rumor.informedIds.Contains(survivorId))
                    return true;
            }
            return false;
        }
    }
}
