using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class ConfessionalModuleState
    {
        public string moduleId = "shelter_module_confessional";
        public bool isOccupied;
        public string speakerId;
        public string listenerId;
        public int sessionsCompleted;
        public List<string> guiltCured = new List<string>();
        public bool sessionActive;
        public float hoursElapsed;
    }

    /// <summary>
    /// Prompt #843: Confessional — Small room where a Guilt/Trauma survivor
    /// enters as speaker, another sits opposite as listener. Guilt is cured
    /// through empathic connection. Both gain affinity and morale.
    /// </summary>
    public class ShelterModule_Confessional
    {
        /// <summary>
        /// MISC-005: seeded stream backing the default <c>randomFloat</c>. The
        /// parameter exists so hosts can pass a campaign rng for deterministic
        /// replay; the old default reached for wall-clock UnityEngine.Random, so
        /// every caller that omitted it silently opted out of determinism.
        /// </summary>
    private static System.Random FallbackRng =>
        AtomicWar._Game.Utilities.SeededRandom.Stream("sheltermodule_confessional");

        private ConfessionalModuleState _state = new ConfessionalModuleState();

        private const float SessionDurationHours = 2f;
        private const float BaseCurechance = 0.5f;
        private const float EmpathyBonus = 0.3f;
        private const float AffinityGain = 0.1f;

        public event Action<string> OnSpeakerEntered;                          // survivorId
        public event Action<string> OnListenerEntered;                         // survivorId
        public event Action<string, string> OnSessionStarted;                  // speakerId, listenerId
        public event Action<string, string> OnGuiltCured;                      // speakerId, guiltType
        public event Action<string, string, bool> OnSessionEnded;              // speakerId, listenerId, success

        public ConfessionalModuleState CaptureState() => _state;

        public void RestoreState(ConfessionalModuleState state)
        {
            _state = state ?? new ConfessionalModuleState();
            if (_state.guiltCured == null)
                _state.guiltCured = new List<string>();
        }

        /// <summary>
        /// The guilt/trauma-bearing survivor enters as the speaker.
        /// </summary>
        public bool EnterAsSpeaker(string survivorId)
        {
            if (_state.isOccupied) return false;

            _state.speakerId = survivorId;
            _state.isOccupied = true;

            OnSpeakerEntered?.Invoke(survivorId);
            return true;
        }

        /// <summary>
        /// Another survivor enters as the listener.
        /// </summary>
        public bool EnterAsListener(string survivorId)
        {
            if (string.IsNullOrEmpty(_state.speakerId)) return false;
            if (!string.IsNullOrEmpty(_state.listenerId)) return false;

            _state.listenerId = survivorId;

            OnListenerEntered?.Invoke(survivorId);
            return true;
        }

        /// <summary>
        /// Begins the confessional session once both speaker and listener are present.
        /// </summary>
        public bool StartSession()
        {
            if (string.IsNullOrEmpty(_state.speakerId) || string.IsNullOrEmpty(_state.listenerId))
                return false;
            if (_state.sessionActive) return false;

            _state.sessionActive = true;
            _state.hoursElapsed = 0f;

            OnSessionStarted?.Invoke(_state.speakerId, _state.listenerId);
            return true;
        }

        /// <summary>
        /// Advances the session by one hour.
        /// </summary>
        public void TickHour()
        {
            if (!_state.sessionActive) return;

            _state.hoursElapsed += 1f;

            if (_state.hoursElapsed >= SessionDurationHours)
            {
                EndSession();
            }
        }

        /// <summary>
        /// Ends the confessional session. Caller must supply empathy (0-1)
        /// and guilt type to resolve the cure.
        /// </summary>
        public void EndSession(float listenerEmpathy = 0.5f, string guiltType = "general_guilt",
            Func<float> randomFloat = null)
        {
            if (!_state.sessionActive) return;

            Func<float> rng = randomFloat ?? (() => (float)FallbackRng.NextDouble());

            float curechance = BaseCurechance + (listenerEmpathy * EmpathyBonus);
            bool success = rng() < curechance;

            if (success)
            {
                _state.guiltCured.Add(_state.speakerId + ":" + guiltType);
                OnGuiltCured?.Invoke(_state.speakerId, guiltType);
            }

            _state.sessionsCompleted++;
            _state.sessionActive = false;

            OnSessionEnded?.Invoke(_state.speakerId, _state.listenerId, success);

            // Reset occupancy
            _state.speakerId = null;
            _state.listenerId = null;
            _state.isOccupied = false;
            _state.hoursElapsed = 0f;
        }

        /// <summary>
        /// Returns true if a session is currently in progress.
        /// </summary>
        public bool IsSessionActive() => _state.sessionActive;

        /// <summary>
        /// Returns the affinity bonus both participants receive from a session.
        /// </summary>
        public float GetAffinityGain() => AffinityGain;
    }
}
