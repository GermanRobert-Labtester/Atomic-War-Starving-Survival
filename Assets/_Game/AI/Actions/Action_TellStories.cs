using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.AI.Actions
{
    [Serializable]
    public class TellStoriesState
    {
        public string actionId = "action_tell_stories";
        public float anxietyFreezeHours = 24f;
        public bool requiresBooks = true;
        public float adultTimeCostHours = 1f;
        public List<string> frozenChildIds = new List<string>();
        public List<float> frozenExpiryTimestamps = new List<float>();
    }

    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_TellStories
    {
        public event Action<string, string, float> OnAnxietyFrozen;
        public event Action<string> OnStorytellingFailed;

        private TellStoriesState _state;
        private Dictionary<string, float> _frozenChildren = new Dictionary<string, float>();

        public Action_TellStories(TellStoriesState state = null)
        {
            _state = state ?? new TellStoriesState();
            RebuildMap();
        }

        public string ActionId => _state.actionId;

        /// <summary>
        /// Adult tells stories to a child. Requires books.
        /// On success: freezes the child's anxiety for 24 hours, costs the adult 1 hour.
        /// On failure (no books): raises OnStorytellingFailed.
        /// Returns true on success.
        /// </summary>
        public bool TellStory(string adultId, string childId, bool hasBooks)
        {
            if (string.IsNullOrEmpty(adultId) || string.IsNullOrEmpty(childId))
            {
                Debug.LogWarning("[Action_TellStories] TellStory called with null/empty id.");
                return false;
            }

            if (!hasBooks)
            {
                OnStorytellingFailed?.Invoke(adultId);
                return false;
            }

            // Calculate expiry: current game-time should be passed in or tracked externally.
            // We use a simple timestamp model; the caller should advance game hours.
            float expiryTimestamp = GetCurrentTimestamp() + _state.anxietyFreezeHours;
            _frozenChildren[childId] = expiryTimestamp;

            OnAnxietyFrozen?.Invoke(adultId, childId, _state.anxietyFreezeHours);

            return true;
        }

        /// <summary>
        /// Returns true if the child's anxiety is currently frozen (within the freeze window).
        /// </summary>
        public bool IsAnxietyFrozen(string childId)
        {
            if (string.IsNullOrEmpty(childId))
            {
                return false;
            }

            if (!_frozenChildren.TryGetValue(childId, out float expiry))
            {
                return false;
            }

            if (GetCurrentTimestamp() >= expiry)
            {
                _frozenChildren.Remove(childId);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Call each game-hour to clean up expired freeze entries.
        /// currentGameHour is the total elapsed game hours since start.
        /// </summary>
        public void TickHour(float currentGameHour)
        {
            _currentGameHour = currentGameHour;

            var expired = new List<string>();
            foreach (var kvp in _frozenChildren)
            {
                if (currentGameHour >= kvp.Value)
                {
                    expired.Add(kvp.Key);
                }
            }

            foreach (var childId in expired)
            {
                _frozenChildren.Remove(childId);
            }
        }

        /// <summary>
        /// Returns the adult time cost for telling a story.
        /// </summary>
        public float GetAdultTimeCost()
        {
            return _state.adultTimeCostHours;
        }

        // Internal game-hour tracker for timestamp-based expiry
        private float _currentGameHour = 0f;

        private float GetCurrentTimestamp()
        {
            return _currentGameHour;
        }

        public TellStoriesState CaptureState()
        {
            var state = new TellStoriesState
            {
                actionId = _state.actionId,
                anxietyFreezeHours = _state.anxietyFreezeHours,
                requiresBooks = _state.requiresBooks,
                adultTimeCostHours = _state.adultTimeCostHours,
                frozenChildIds = new List<string>(),
                frozenExpiryTimestamps = new List<float>()
            };

            foreach (var kvp in _frozenChildren)
            {
                state.frozenChildIds.Add(kvp.Key);
                state.frozenExpiryTimestamps.Add(kvp.Value);
            }

            return state;
        }

        public void RestoreState(TellStoriesState state)
        {
            _state = state ?? new TellStoriesState();
            RebuildMap();
        }

        private void RebuildMap()
        {
            _frozenChildren.Clear();

            if (_state.frozenChildIds == null || _state.frozenExpiryTimestamps == null)
                return;

            int count = Mathf.Min(_state.frozenChildIds.Count, _state.frozenExpiryTimestamps.Count);
            for (int i = 0; i < count; i++)
            {
                _frozenChildren[_state.frozenChildIds[i]] = _state.frozenExpiryTimestamps[i];
            }
        }
    }
}
