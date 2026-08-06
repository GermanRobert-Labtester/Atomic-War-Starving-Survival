using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TeenRebellionState
    {
        public string eventId = "event_teen_rebellion";
        public int resourceWasted = 0;
        public float powerWasted = 0f;
        public float trustPenalty = -0.2f;
        public List<string> activeRebellionTeenIds = new List<string>();
        public List<string> activeRebellionRooms = new List<string>();
        public List<float> activeRebellionHoursRemaining = new List<float>();
        public List<string> punishedTeenIds = new List<string>();
    }

    public class Event_TeenRebellion
    {
        public event Action<string, string> OnRebellionStarted;
        public event Action<string, int, float> OnResourcesWasted;
        public event Action<string, float> OnTrustLowered;

        private TeenRebellionState _state;

        private Dictionary<string, RebellionEntry> _activeRebellions = new Dictionary<string, RebellionEntry>();
        private HashSet<string> _punishedTeens = new HashSet<string>();

        private static readonly string[] RoomOptions = new string[]
        {
            "storage_room",
            "generator_room",
            "sleeping_quarters",
            "airlock"
        };

        [Serializable]
        private class RebellionEntry
        {
            public string roomLockedIn;
            public float hoursRemaining;
        }

        public Event_TeenRebellion(TeenRebellionState state = null)
        {
            _state = state ?? new TeenRebellionState();
            RebuildMaps();
        }

        public string EventId => _state.eventId;

        /// <summary>
        /// Triggers a rebellion: locks the teen in a random room, wastes food and power.
        /// </summary>
        public void TriggerRebellion(string teenId, Random rng)
        {
            if (string.IsNullOrEmpty(teenId))
            {
                Debug.LogWarning("[Event_TeenRebellion] TriggerRebellion called with null/empty teenId.");
                return;
            }

            if (rng == null) rng = new Random();

            string room = RoomOptions[rng.Next(RoomOptions.Length)];
            int foodWasted = rng.Next(1, 4);
            float powerWasted = (float)(rng.NextDouble() * 0.5 + 0.1);

            _state.resourceWasted = foodWasted;
            _state.powerWasted = powerWasted;

            _activeRebellions[teenId] = new RebellionEntry
            {
                roomLockedIn = room,
                hoursRemaining = 24f
            };

            OnRebellionStarted?.Invoke(teenId, room);
            OnResourcesWasted?.Invoke(teenId, foodWasted, powerWasted);
        }

        /// <summary>
        /// Punish the teen. Applies a permanent trust penalty.
        /// </summary>
        public void Punish(string teenId)
        {
            if (string.IsNullOrEmpty(teenId))
            {
                Debug.LogWarning("[Event_TeenRebellion] Punish called with null/empty teenId.");
                return;
            }

            _punishedTeens.Add(teenId);
            OnTrustLowered?.Invoke(teenId, _state.trustPenalty);

            // Rebellion ends immediately when punished
            _activeRebellions.Remove(teenId);
        }

        /// <summary>
        /// Ignore the rebellion. It fades after 24 hours (handled by TickHour).
        /// </summary>
        public void Ignore(string teenId)
        {
            if (string.IsNullOrEmpty(teenId))
            {
                Debug.LogWarning("[Event_TeenRebellion] Ignore called with null/empty teenId.");
                return;
            }

            // No trust penalty applied; rebellion will naturally expire via TickHour
        }

        /// <summary>
        /// Call once per in-game hour to tick down active rebellions.
        /// Rebellion fades when hoursRemaining reaches 0.
        /// </summary>
        public void TickHour()
        {
            var expired = new List<string>();

            foreach (var kvp in _activeRebellions)
            {
                kvp.Value.hoursRemaining -= 1f;
                if (kvp.Value.hoursRemaining <= 0f)
                {
                    expired.Add(kvp.Key);
                }
            }

            foreach (var teenId in expired)
            {
                _activeRebellions.Remove(teenId);
            }
        }

        /// <summary>
        /// Returns true if the teen has an active rebellion.
        /// </summary>
        public bool IsRebelling(string teenId)
        {
            return _activeRebellions.ContainsKey(teenId);
        }

        /// <summary>
        /// Returns true if the teen has been permanently punished.
        /// </summary>
        public bool IsPunished(string teenId)
        {
            return _punishedTeens.Contains(teenId);
        }

        public TeenRebellionState CaptureState()
        {
            var state = new TeenRebellionState
            {
                eventId = _state.eventId,
                resourceWasted = _state.resourceWasted,
                powerWasted = _state.powerWasted,
                trustPenalty = _state.trustPenalty,
                activeRebellionTeenIds = new List<string>(),
                activeRebellionRooms = new List<string>(),
                activeRebellionHoursRemaining = new List<float>(),
                punishedTeenIds = new List<string>(_punishedTeens)
            };

            foreach (var kvp in _activeRebellions)
            {
                state.activeRebellionTeenIds.Add(kvp.Key);
                state.activeRebellionRooms.Add(kvp.Value.roomLockedIn);
                state.activeRebellionHoursRemaining.Add(kvp.Value.hoursRemaining);
            }

            return state;
        }

        public void RestoreState(TeenRebellionState state)
        {
            _state = state ?? new TeenRebellionState();
            RebuildMaps();
        }

        private void RebuildMaps()
        {
            _activeRebellions.Clear();
            _punishedTeens.Clear();

            if (_state.activeRebellionTeenIds != null &&
                _state.activeRebellionRooms != null &&
                _state.activeRebellionHoursRemaining != null)
            {
                int count = Mathf.Min(
                    _state.activeRebellionTeenIds.Count,
                    Mathf.Min(_state.activeRebellionRooms.Count, _state.activeRebellionHoursRemaining.Count));

                for (int i = 0; i < count; i++)
                {
                    _activeRebellions[_state.activeRebellionTeenIds[i]] = new RebellionEntry
                    {
                        roomLockedIn = _state.activeRebellionRooms[i],
                        hoursRemaining = _state.activeRebellionHoursRemaining[i]
                    };
                }
            }

            if (_state.punishedTeenIds != null)
            {
                foreach (var id in _state.punishedTeenIds)
                {
                    _punishedTeens.Add(id);
                }
            }
        }
    }
}
