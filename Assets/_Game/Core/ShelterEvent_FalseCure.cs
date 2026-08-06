using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FalseCureState
    {
        public string eventId = "event_false_cure";
        public bool isBroadcastReceived = false;
        public bool isJourneyUndertaken = false;
        public string destinationNodeId = "";
        public bool trapRevealed = false;
        public float moralePenalty = -40f;
        public float radHealingPromised = 100f;
    }

    /// <summary>
    /// Prompt #565: Event: The False Cure.
    /// Radio broadcast promises a cure for ChronicRadiation at a distant hospital.
    /// The grueling journey reveals a room full of corpses who drank poisoned Kool-Aid.
    /// A brutal narrative trap.
    /// </summary>
    public class ShelterEvent_FalseCure
    {
        private FalseCureState _state = new FalseCureState();

        public event Action<FalseCureState> OnBroadcastReceived;
        public event Action<FalseCureState, string> OnJourneyCompleted;
        public event Action<FalseCureState, float> OnTrapRevealed;

        public FalseCureState State => _state;

        public void ReceiveBroadcast()
        {
            _state.isBroadcastReceived = true;
            OnBroadcastReceived?.Invoke(_state);
        }

        public string UndertakeJourney(string expeditionResult)
        {
            if (!_state.isBroadcastReceived) return "";

            _state.isJourneyUndertaken = true;
            OnJourneyCompleted?.Invoke(_state, expeditionResult);

            return "The journey was grueling. You arrive at the hospital, " +
                   "pushing through collapsed corridors to the promised ward.";
        }

        public string RevealTrap()
        {
            if (!_state.isJourneyUndertaken) return "";

            _state.trapRevealed = true;
            OnTrapRevealed?.Invoke(_state, _state.moralePenalty);

            return "The ward is filled with rows of corpses, each clutching an empty cup. " +
                   "There was never a cure. Just poisoned Kool-Aid and a radio broadcast " +
                   "designed to lure the desperate. The dead smiled when they drank it.";
        }
    }
}
