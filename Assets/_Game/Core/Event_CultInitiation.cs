using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CultInitiationState
    {
        public string eventId = "event_cult_initiation";
        public string initiateId;
        public string initiationType; // "fast" or "storm"
        public int daysRemaining;
        public bool survived;
        public bool playerIntervened;
        public bool mutinyTriggered;
        public bool active;
        public bool complete;
    }

    /// <summary>
    /// Prompt #841: Cult Initiation — Preacher converts someone who must
    /// pass an initiation (fast 3 days or stand in airlock during storm).
    /// Player can intervene but causes religious mutiny.
    /// </summary>
    public class Event_CultInitiation
    {
        private CultInitiationState _state = new CultInitiationState();

        private const int FastDurationDays = 3;
        private const float FastMinHealth = 60f;
        private const float StormMinWarmth = 40f;

        public event Action<string, string> OnInitiationStarted;    // initiateId, type
        public event Action<string, int> OnDayPassed;              // initiateId, day
        public event Action OnPlayerIntervened;
        public event Action<string> OnMutinyTriggered;             // preacherId
        public event Action<string> OnInitiationSurvived;          // initiateId
        public event Action<string> OnInitiationFailed;            // initiateId

        public CultInitiationState CaptureState() => _state;

        public void RestoreState(CultInitiationState state) => _state = state ?? new CultInitiationState();

        /// <summary>
        /// Begins an initiation of the given type ("fast" or "storm").
        /// </summary>
        public void StartInitiation(string initiateId, string type)
        {
            _state.initiateId = initiateId;
            _state.initiationType = type;
            _state.daysRemaining = FastDurationDays;
            _state.survived = false;
            _state.playerIntervened = false;
            _state.mutinyTriggered = false;
            _state.active = true;
            _state.complete = false;

            OnInitiationStarted?.Invoke(initiateId, type);
        }

        /// <summary>
        /// Advances the initiation by one day.
        /// Caller must supply current health (for fast) or warmth (for storm)
        /// to determine survival.
        /// </summary>
        public void TickDay(float currentValue = 0f, string preacherId = null)
        {
            if (!_state.active || _state.complete) return;

            _state.daysRemaining--;
            int dayNumber = FastDurationDays - _state.daysRemaining;
            OnDayPassed?.Invoke(_state.initiateId, dayNumber);

            if (_state.daysRemaining <= 0)
            {
                _state.complete = true;
                _state.active = false;

                bool passes;
                if (_state.initiationType == "fast")
                {
                    // Need 60+ health to survive 3 days without food
                    passes = currentValue >= FastMinHealth;
                }
                else // "storm"
                {
                    // Need 40+ warmth to survive airlock during storm
                    passes = currentValue >= StormMinWarmth;
                }

                _state.survived = passes;
                if (passes)
                    OnInitiationSurvived?.Invoke(_state.initiateId);
                else
                    OnInitiationFailed?.Invoke(_state.initiateId);
            }
        }

        /// <summary>
        /// Player intervenes to stop the initiation. Triggers mutiny
        /// among preacher and followers.
        /// </summary>
        public void PlayerIntervene(string preacherId = null)
        {
            if (!_state.active || _state.playerIntervened) return;

            _state.playerIntervened = true;
            _state.mutinyTriggered = true;
            _state.active = false;
            _state.complete = true;

            OnPlayerIntervened?.Invoke();
            OnMutinyTriggered?.Invoke(preacherId ?? string.Empty);
        }

        /// <summary>
        /// Returns true if the player's intervention caused a mutiny.
        /// </summary>
        public bool IsMutinyTriggered() => _state.mutinyTriggered;

        /// <summary>
        /// Returns true if the initiation has run its course (survived or failed).
        /// </summary>
        public bool IsInitiationComplete() => _state.complete;

        /// <summary>
        /// Returns true if the initiate survived the ordeal.
        /// </summary>
        public bool DidSurvive() => _state.survived;
    }
}
