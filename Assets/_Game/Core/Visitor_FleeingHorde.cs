using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FleeingHordeState
    {
        public string id = "visitor_fleeing_horde";
        public string displayName = "The Fleeing Horde";
        public bool isStormPanicActive = false;
        public bool isCombatDisabled = false;
        public bool isStaminaRaceActive = false;
    }

    /// <summary>
    /// Prompt #368: System: The Fleeing Horde (Storm Mechanics).
    /// Triggered when a FalloutStorm hits while scavenging. All NPCs stop fighting and rush for the exit.
    /// Combat disables, converting the expedition into a pure stamina race to the shelter airlock.
    /// </summary>
    public class Visitor_FleeingHorde
    {
        private FleeingHordeState _state = new FleeingHordeState();

        public event Action<FleeingHordeState> OnStormPanicTriggered;

        public FleeingHordeState State => _state;

        public void TriggerStormPanic()
        {
            _state.isStormPanicActive = true;
            _state.isCombatDisabled = true;
            _state.isStaminaRaceActive = true;

            OnStormPanicTriggered?.Invoke(_state);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public FleeingHordeState CaptureState() => _state;

        public void RestoreState(FleeingHordeState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
