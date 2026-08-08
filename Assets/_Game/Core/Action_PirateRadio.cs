using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PirateRadioState
    {
        public string actionId = "action_pirate_radio";
        public string displayName = "Pirate Radio Broadcast";
        public bool requiresVinylRecords = true;
        public float moraleBoostAllies = 15f;
        public float raiderCombatReduction = 0.20f;
        public float broadcastDurationHours = 6f;
        public float hoursRemaining = 0f;
        public bool isBroadcasting = false;
    }

    /// <summary>
    /// Prompt #633: Action: Pirate Radio.
    /// Broadcast music over the airwaves (requires VinylRecords). Boosts Allied
    /// Faction Morale globally and lowers nearby Raider combat effectiveness.
    /// </summary>
    /// <summary>DEMOTE-Action-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_PirateRadio
    {
        private PirateRadioState _state = new PirateRadioState();

        public event Action<PirateRadioState> OnBroadcastStarted;
        public event Action<PirateRadioState> OnBroadcastEnded;
        public event Action<PirateRadioState, float> OnTick;

        public PirateRadioState State => _state;

        public bool Broadcast(bool hasVinylRecords)
        {
            if (!hasVinylRecords && _state.requiresVinylRecords) return false;

            _state.isBroadcasting = true;
            _state.hoursRemaining = _state.broadcastDurationHours;
            OnBroadcastStarted?.Invoke(_state);
            return true;
        }

        public void TickHour()
        {
            if (!_state.isBroadcasting) return;

            _state.hoursRemaining -= 1f;
            OnTick?.Invoke(_state, _state.hoursRemaining);

            if (_state.hoursRemaining <= 0f)
            {
                _state.isBroadcasting = false;
                _state.hoursRemaining = 0f;
                OnBroadcastEnded?.Invoke(_state);
            }
        }

        public float GetAllyMoraleBonus()
        {
            return _state.isBroadcasting ? _state.moraleBoostAllies : 0f;
        }

        public float GetRaiderCombatPenalty()
        {
            return _state.isBroadcasting ? _state.raiderCombatReduction : 0f;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public PirateRadioState CaptureState() => _state;

        public void RestoreState(PirateRadioState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
