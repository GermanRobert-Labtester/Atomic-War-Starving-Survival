using System;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class RansomNoteState
    {
        public string eventId = "event_ransom_note";
        public float demandAmount;
        public string resourceType = string.Empty;
        public int daysUntilReturn;
        public int timesPaid;
        public float currentDemand;
        public bool noteActive;
    }

    /// <summary>
    /// Prompt #827: Ransom Note. Before a siege, the Warlord slides a note
    /// under the door: "Give us 100 Water and we walk away." Paying avoids
    /// the fight, but the Warlord returns in 30 days demanding double.
    /// Each payment escalates the next demand exponentially.
    /// Refusal triggers the siege immediately.
    /// Plain C#. Save/load safe.
    /// </summary>
    public class Event_RansomNote
    {
        private RansomNoteState _state = new RansomNoteState();

        private const float InitialDemand = 100f;
        private const int ReturnIntervalDays = 30;

        // -- Events --
        public event Action<float, string> OnNoteDelivered;   // (amount, resourceType)
        public event Action OnRansomPaid;
        public event Action OnRansomRefused;
        public event Action<int> OnWarlordReturned;           // new demand amount (as int)

        public RansomNoteState State => _state;

        /// <summary>
        /// The Warlord's note arrives with a demand.
        /// </summary>
        /// <param name="amount">Resource amount demanded.</param>
        /// <param name="resource">Type of resource (e.g. "water").</param>
        public void DeliverNote(float amount, string resource)
        {
            _state.demandAmount = amount;
            _state.resourceType = resource ?? string.Empty;
            _state.currentDemand = amount;
            _state.daysUntilReturn = ReturnIntervalDays;
            _state.noteActive = true;

            OnNoteDelivered?.Invoke(amount, _state.resourceType);
        }

        /// <summary>
        /// Pay the ransom from available resources. The Warlord leaves but
        /// will return in 30 days demanding double.
        /// </summary>
        /// <param name="availableResource">
        /// Amount of the demanded resource the player has.
        /// </param>
        /// <returns>
        /// True if the ransom was paid successfully. False if insufficient
        /// resources.
        /// </returns>
        public bool PayRansom(float availableResource)
        {
            if (!_state.noteActive) return false;
            if (availableResource < _state.currentDemand) return false;

            _state.timesPaid++;
            _state.noteActive = false;
            _state.daysUntilReturn = ReturnIntervalDays;

            // Next demand is double
            _state.currentDemand = _state.demandAmount * 2f;
            _state.demandAmount = _state.currentDemand;

            OnRansomPaid?.Invoke();
            return true;
        }

        /// <summary>
        /// Refuse the ransom. The siege begins immediately.
        /// </summary>
        public void RefuseRansom()
        {
            if (!_state.noteActive) return;

            _state.noteActive = false;
            OnRansomRefused?.Invoke();
        }

        /// <summary>
        /// Check whether the Warlord has returned on the given day.
        /// Call this each day to see if the return timer has elapsed.
        /// </summary>
        /// <param name="currentDay">The current in-game day number.</param>
        /// <returns>
        /// True if the Warlord has returned with a new demand.
        /// The new demand can be read via GetDemand().
        /// </returns>
        public bool CheckReturn(int currentDay)
        {
            if (_state.timesPaid <= 0) return false;

            _state.daysUntilReturn--;

            if (_state.daysUntilReturn <= 0)
            {
                _state.noteActive = true;
                _state.daysUntilReturn = ReturnIntervalDays;

                OnWarlordReturned?.Invoke((int)_state.currentDemand);
                return true;
            }

            return false;
        }

        /// <summary>Current demand amount.</summary>
        public float GetDemand()
        {
            return _state.currentDemand;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public RansomNoteState CaptureState()
        {
            return new RansomNoteState
            {
                eventId = _state.eventId,
                demandAmount = _state.demandAmount,
                resourceType = _state.resourceType,
                daysUntilReturn = _state.daysUntilReturn,
                timesPaid = _state.timesPaid,
                currentDemand = _state.currentDemand,
                noteActive = _state.noteActive
            };
        }

        public void RestoreState(RansomNoteState saved)
        {
            _state = saved ?? new RansomNoteState();
        }
    }
}
