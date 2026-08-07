using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RecordPlayerState
    {
        public string moduleId = "shelter_module_record_player";
        public string displayName = "The Record Player";
        public bool isBuilt = false;
        public float powerRequiredWatts = 5f;
        public bool isPlaying = false;
        public float moraleAuraBonus = 20f;
        public bool isRecordScratched = false;
    }

    /// <summary>
    /// Prompt #440: Module: The Record Player.
    /// Requires 5W of Power and VinylRecords. While playing, provides an AoE Morale aura.
    /// If power fluctuates, the record scratches and the morale aura breaks.
    /// </summary>
    public class ShelterModule_RecordPlayer
    {
        private RecordPlayerState _state = new RecordPlayerState();

        public event Action<RecordPlayerState, float> OnMoraleAuraActive;
        public event Action<RecordPlayerState> OnRecordScratchedAuraBroken;

        public RecordPlayerState State => _state;

        public bool StartPlaying(bool hasPower, bool hasVinylRecord)
        {
            if (!_state.isBuilt || !hasPower || !hasVinylRecord || _state.isRecordScratched)
                return false;

            _state.isPlaying = true;
            OnMoraleAuraActive?.Invoke(_state, _state.moraleAuraBonus);
            return true;
        }

        public void HandlePowerFluctuation()
        {
            if (_state.isPlaying)
            {
                _state.isPlaying = false;
                _state.isRecordScratched = true;
                OnRecordScratchedAuraBroken?.Invoke(_state);
            }
        }
    
        public RecordPlayerState CaptureState()
        {
            return _state;
        }

        public void RestoreState(RecordPlayerState saved)
        {
            _state = saved ?? new RecordPlayerState();
        }
    }
}

