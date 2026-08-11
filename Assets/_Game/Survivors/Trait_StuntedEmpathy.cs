using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    [Serializable]
    public class StuntedEmpathyState
    {
        public string traitId = "trait_stunted_empathy";
        public int deathWitnessThreshold = 3;
        public int deathsWitnessed = 0;
        public bool moraleShattered = false;
        public bool guaranteedSociopathOnAdult = false;
        public List<string> trackedChildIds = new List<string>();
        public List<int> trackedDeathCounts = new List<int>();
        public List<bool> trackedShatteredFlags = new List<bool>();
    }

    public class Trait_StuntedEmpathy
    {
        public event Action<string> OnDeathWitnessed;
        public event Action<string> OnMoraleShattered;
        public event Action<string> OnSociopathGuaranteed;

        private StuntedEmpathyState _state;
        private Dictionary<string, ChildEmpathyData> _childData = new Dictionary<string, ChildEmpathyData>();

        [Serializable]
        private class ChildEmpathyData
        {
            public int deathsWitnessed;
            public bool moraleShattered;
        }

        public Trait_StuntedEmpathy(StuntedEmpathyState state = null)
        {
            _state = state ?? new StuntedEmpathyState();
            RebuildMap();
        }

        public string TraitId => _state.traitId;

        /// <summary>
        /// Records that a child witnessed a death. Increments counter.
        /// If deaths witnessed >= threshold, permanently shatters morale.
        /// </summary>
        public void WitnessDeath(string childId)
        {
            if (string.IsNullOrEmpty(childId))
            {
                Debug.LogWarning("[Trait_StuntedEmpathy] WitnessDeath called with null/empty childId.");
                return;
            }

            if (!_childData.ContainsKey(childId))
            {
                _childData[childId] = new ChildEmpathyData();
            }

            var data = _childData[childId];
            data.deathsWitnessed++;

            OnDeathWitnessed?.Invoke(childId);

            if (data.deathsWitnessed >= _state.deathWitnessThreshold && !data.moraleShattered)
            {
                data.moraleShattered = true;
                _state.moraleShattered = true;
                OnMoraleShattered?.Invoke(childId);
            }
        }

        /// <summary>
        /// Called when the child grows up. If morale is shattered, guarantees a sociopath trait.
        /// </summary>
        public void OnGrowingUp(string childId)
        {
            if (string.IsNullOrEmpty(childId))
            {
                Debug.LogWarning("[Trait_StuntedEmpathy] OnGrowingUp called with null/empty childId.");
                return;
            }

            if (!_childData.ContainsKey(childId))
            {
                return;
            }

            var data = _childData[childId];

            if (data.moraleShattered)
            {
                _state.guaranteedSociopathOnAdult = true;
                OnSociopathGuaranteed?.Invoke(childId);
            }

            // Clean up child data after transition
            _childData.Remove(childId);
        }

        /// <summary>
        /// Returns true if the child's morale has been permanently shattered.
        /// </summary>
        public bool IsShattered(string childId)
        {
            if (_childData.TryGetValue(childId, out var data))
            {
                return data.moraleShattered;
            }
            return false;
        }

        /// <summary>
        /// Returns the number of deaths witnessed by the child.
        /// </summary>
        public int GetDeathsWitnessed(string childId)
        {
            if (_childData.TryGetValue(childId, out var data))
            {
                return data.deathsWitnessed;
            }
            return 0;
        }

        public StuntedEmpathyState CaptureState()
        {
            var state = new StuntedEmpathyState
            {
                traitId = _state.traitId,
                deathWitnessThreshold = _state.deathWitnessThreshold,
                deathsWitnessed = _state.deathsWitnessed,
                moraleShattered = _state.moraleShattered,
                guaranteedSociopathOnAdult = _state.guaranteedSociopathOnAdult,
                trackedChildIds = new List<string>(),
                trackedDeathCounts = new List<int>(),
                trackedShatteredFlags = new List<bool>()
            };

            foreach (var kvp in _childData)
            {
                state.trackedChildIds.Add(kvp.Key);
                state.trackedDeathCounts.Add(kvp.Value.deathsWitnessed);
                state.trackedShatteredFlags.Add(kvp.Value.moraleShattered);
            }

            return state;
        }

        public void RestoreState(StuntedEmpathyState state)
        {
            _state = state ?? new StuntedEmpathyState();
            RebuildMap();
        }

        private void RebuildMap()
        {
            _childData.Clear();

            if (_state.trackedChildIds == null ||
                _state.trackedDeathCounts == null ||
                _state.trackedShatteredFlags == null)
                return;

            int count = Mathf.Min(
                _state.trackedChildIds.Count,
                Mathf.Min(_state.trackedDeathCounts.Count, _state.trackedShatteredFlags.Count));

            for (int i = 0; i < count; i++)
            {
                _childData[_state.trackedChildIds[i]] = new ChildEmpathyData
                {
                    deathsWitnessed = _state.trackedDeathCounts[i],
                    moraleShattered = _state.trackedShatteredFlags[i]
                };
            }
        }
    }
}
