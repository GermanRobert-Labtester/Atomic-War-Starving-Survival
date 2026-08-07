using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SpontaneousMurderState
    {
        public string eventId = "event_spontaneous_murder";
        public string killerId;
        public string victimId;
        public string weaponUsed;
        public int daysMaxedAnxiety;
        public int daysMaxedDepression;
        public bool murderOccurred;
    }

    /// <summary>
    /// Prompt #845: Cabin Fever Murder — If Anxiety AND Depression are both
    /// maxed (1.0) for 14 consecutive days, the survivor snaps and kills the
    /// closest survivor with whatever tool is held. No warning.
    /// </summary>
    public class Event_SpontaneousMurder
    {
        private SpontaneousMurderState _state = new SpontaneousMurderState();

        private const int SnapThresholdDays = 14;
        private const float MaxStatValue = 1.0f;

        public event Action<string, int, int> OnDaysMaxedUpdated;    // survivorId, anxDays, depDays
        public event Action<string> OnSnapTriggered;                 // killerId
        public event Action<string, string, string> OnMurderCommitted; // killerId, victimId, weapon
        public event Action<string> OnBodyDiscovered;                // victimId

        public SpontaneousMurderState CaptureState() => _state;

        public void RestoreState(SpontaneousMurderState state) => _state = state ?? new SpontaneousMurderState();

        /// <summary>
        /// Called daily for each survivor. Tracks consecutive days at max
        /// anxiety and depression. Triggers snap at 14 days.
        /// </summary>
        public bool TickDay(string survivorId, float anxiety, float depression)
        {
            if (_state.murderOccurred) return false;

            // Track anxiety max days
            if (anxiety >= MaxStatValue)
                _state.daysMaxedAnxiety++;
            else
                _state.daysMaxedAnxiety = 0;

            // Track depression max days
            if (depression >= MaxStatValue)
                _state.daysMaxedDepression++;
            else
                _state.daysMaxedDepression = 0;

            OnDaysMaxedUpdated?.Invoke(survivorId, _state.daysMaxedAnxiety, _state.daysMaxedDepression);

            return CheckSnap(anxiety, depression, _state.daysMaxedAnxiety, _state.daysMaxedDepression);
        }

        /// <summary>
        /// Returns true if both anxiety and depression have been maxed for
        /// the required consecutive days.
        /// </summary>
        public bool CheckSnap(float anxiety, float depression, int daysAnxMaxed, int daysDepMaxed)
        {
            if (anxiety < MaxStatValue || depression < MaxStatValue) return false;
            if (daysAnxMaxed < SnapThresholdDays || daysDepMaxed < SnapThresholdDays) return false;
            return true;
        }

        /// <summary>
        /// Executes the murder. No warning event is raised — the snap is instant.
        /// </summary>
        public void ExecuteMurder(string killerId, string closestSurvivorId, string heldTool)
        {
            if (_state.murderOccurred) return;

            _state.killerId = killerId;
            _state.victimId = closestSurvivorId;
            _state.weaponUsed = heldTool;
            _state.murderOccurred = true;

            OnSnapTriggered?.Invoke(killerId);
            OnMurderCommitted?.Invoke(killerId, closestSurvivorId, heldTool);
            OnBodyDiscovered?.Invoke(closestSurvivorId);
        }

        /// <summary>
        /// Returns true if a murder has already occurred.
        /// </summary>
        public bool HasMurderOccurred() => _state.murderOccurred;

        /// <summary>
        /// Returns the number of remaining days until snap given current streak.
        /// </summary>
        public int GetDaysUntilSnap()
        {
            int minDays = Math.Min(_state.daysMaxedAnxiety, _state.daysMaxedDepression);
            return Math.Max(0, SnapThresholdDays - minDays);
        }
    }
}
