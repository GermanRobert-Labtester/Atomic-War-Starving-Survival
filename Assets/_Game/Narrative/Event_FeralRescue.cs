using System;
using UnityEngine;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class FeralRescueState
    {
        public string eventId = "event_feral_rescue";
        public int socializationDaysRequired = 60;
        public int daysElapsed = 0;
        public bool isSocialized = false;
        public string survivorId = "";
        public bool isDiscovered = false;
    }

    public class Event_FeralRescue
    {
        public event Action<string> OnFeralChildFound;
        public event Action<string> OnDiseaseIntroduced;
        public event Action<string> OnSocializationProgress;
        public event Action<string> OnChildTamed;

        private readonly FeralRescueState _state;

        public Event_FeralRescue()
        {
            _state = new FeralRescueState();
        }

        public void DiscoverFeral(string survivorId)
        {
            _state.survivorId = survivorId;
            _state.isDiscovered = true;
            _state.daysElapsed = 0;
            _state.isSocialized = false;

            OnFeralChildFound?.Invoke(survivorId);
            OnDiseaseIntroduced?.Invoke("disease_feral");
        }

        public void TickDay()
        {
            if (!_state.isDiscovered || _state.isSocialized)
                return;

            _state.daysElapsed++;

            int remaining = _state.socializationDaysRequired - _state.daysElapsed;
            OnSocializationProgress?.Invoke(remaining.ToString());

            if (_state.daysElapsed >= _state.socializationDaysRequired)
            {
                _state.isSocialized = true;
                OnChildTamed?.Invoke(_state.survivorId);
            }
        }

        public bool IsSocialized()
        {
            return _state.isSocialized;
        }

        public int GetDaysRemaining()
        {
            int remaining = _state.socializationDaysRequired - _state.daysElapsed;
            return remaining > 0 ? remaining : 0;
        }

        public FeralRescueState CaptureState() => _state;

        public void RestoreState(FeralRescueState state)
        {
            if (state == null) return;
            _state.eventId = state.eventId;
            _state.socializationDaysRequired = state.socializationDaysRequired;
            _state.daysElapsed = state.daysElapsed;
            _state.isSocialized = state.isSocialized;
            _state.survivorId = state.survivorId;
            _state.isDiscovered = state.isDiscovered;
        }
    }
}
