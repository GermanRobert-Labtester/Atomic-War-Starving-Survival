using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AmnesiaState
    {
        public string survivorId;
        public bool isAmnesiac;
        public int daysForgotten = 30;
        public float xpToDelete;
        public string triggerCause;
    }

    public class AmnesiaSystem
    {
        private readonly Dictionary<string, AmnesiaState> _states = new Dictionary<string, AmnesiaState>();

        public IReadOnlyDictionary<string, AmnesiaState> States => _states;

        public event Action<string, string> OnAmnesiaTriggered;  // survivorId, cause
        public event Action<string, float> OnXpDeleted;  // survivorId, xpAmount

        private AmnesiaState GetOrCreate(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state))
            {
                state = new AmnesiaState
                {
                    survivorId = survivorId,
                    isAmnesiac = false,
                    daysForgotten = 30,
                    xpToDelete = 0f,
                    triggerCause = string.Empty
                };
                _states[survivorId] = state;
            }
            return state;
        }

        /// <summary>
        /// Afflicts survivor with amnesia. Stores the XP amount to be deleted.
        /// Actual deletion is deferred to ApplyXpDeletion via a delegate.
        /// </summary>
        public void Afflict(string survivorId, string cause, float xpGainedInLast30Days)
        {
            var state = GetOrCreate(survivorId);
            if (state.isAmnesiac)
                return;

            state.isAmnesiac = true;
            state.triggerCause = cause;
            state.xpToDelete = xpGainedInLast30Days;

            OnAmnesiaTriggered?.Invoke(survivorId, cause);
        }

        /// <summary>
        /// Calls the provided delegate to actually remove XP from the skill system.
        /// The delegate receives the XP amount to delete and returns the actual amount deleted.
        /// </summary>
        public float ApplyXpDeletion(string survivorId, Func<float, float> deleteXp)
        {
            if (!_states.TryGetValue(survivorId, out var state) || !state.isAmnesiac)
                return 0f;

            float deleted = deleteXp(state.xpToDelete);
            state.xpToDelete = 0f;

            OnXpDeleted?.Invoke(survivorId, deleted);
            return deleted;
        }

        /// <summary>
        /// Amnesia is permanent — XP cannot be recovered.
        /// </summary>
        public bool IsRecoverable()
        {
            return false;
        }
    }
}
