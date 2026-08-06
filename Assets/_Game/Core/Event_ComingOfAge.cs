using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ComingOfAgeState
    {
        public string eventId = "event_coming_of_age";
        public int daysRequired = 60;
        public bool isTriggered = false;
        public List<string> trackedChildIds = new List<string>();
        public List<int> trackedDaysSurvived = new List<int>();
    }

    public class Event_ComingOfAge
    {
        public event Action<string, string> OnComingOfAge;
        public event Action<string> OnDependentTraitsRemoved;

        private ComingOfAgeState _state;
        private Dictionary<string, int> _childDaysMap = new Dictionary<string, int>();

        public Event_ComingOfAge(ComingOfAgeState state = null)
        {
            _state = state ?? new ComingOfAgeState();
            RebuildMap();
        }

        public string EventId => _state.eventId;
        public bool IsTriggered => _state.isTriggered;

        /// <summary>
        /// Increments the day counter for a child. Call once per in-game day.
        /// </summary>
        public void TickDay(string childId)
        {
            if (string.IsNullOrEmpty(childId))
            {
                Debug.LogWarning("[Event_ComingOfAge] TickDay called with null/empty childId.");
                return;
            }

            if (!_childDaysMap.ContainsKey(childId))
            {
                _childDaysMap[childId] = 0;
            }

            _childDaysMap[childId]++;
        }

        /// <summary>
        /// Attempts the coming-of-age transition for a child.
        /// If days survived >= daysRequired, child becomes teenager:
        /// loses dependent traits, gains an expert trait based on the closest adult.
        /// Returns true if transition occurred.
        /// </summary>
        public bool TryTransition(string childId, string closestAdultTraitId)
        {
            if (string.IsNullOrEmpty(childId))
            {
                Debug.LogWarning("[Event_ComingOfAge] TryTransition called with null/empty childId.");
                return false;
            }

            if (!_childDaysMap.ContainsKey(childId))
            {
                Debug.LogWarning($"[Event_ComingOfAge] Child {childId} not tracked.");
                return false;
            }

            if (_childDaysMap[childId] < _state.daysRequired)
            {
                return false;
            }

            string expertTraitId = string.IsNullOrEmpty(closestAdultTraitId)
                ? "trait_survivor"
                : closestAdultTraitId;

            _state.isTriggered = true;

            OnDependentTraitsRemoved?.Invoke(childId);
            OnComingOfAge?.Invoke(childId, expertTraitId);

            _childDaysMap.Remove(childId);

            return true;
        }

        /// <summary>
        /// Returns the days survived for a tracked child, or -1 if not tracked.
        /// </summary>
        public int GetDaysSurvived(string childId)
        {
            if (_childDaysMap.TryGetValue(childId, out int days))
            {
                return days;
            }
            return -1;
        }

        public ComingOfAgeState CaptureState()
        {
            var state = new ComingOfAgeState
            {
                eventId = _state.eventId,
                daysRequired = _state.daysRequired,
                isTriggered = _state.isTriggered,
                trackedChildIds = new List<string>(),
                trackedDaysSurvived = new List<int>()
            };

            foreach (var kvp in _childDaysMap)
            {
                state.trackedChildIds.Add(kvp.Key);
                state.trackedDaysSurvived.Add(kvp.Value);
            }

            return state;
        }

        public void RestoreState(ComingOfAgeState state)
        {
            _state = state ?? new ComingOfAgeState();
            RebuildMap();
        }

        private void RebuildMap()
        {
            _childDaysMap.Clear();

            if (_state.trackedChildIds == null || _state.trackedDaysSurvived == null)
                return;

            int count = Mathf.Min(_state.trackedChildIds.Count, _state.trackedDaysSurvived.Count);
            for (int i = 0; i < count; i++)
            {
                _childDaysMap[_state.trackedChildIds[i]] = _state.trackedDaysSurvived[i];
            }
        }
    }
}
