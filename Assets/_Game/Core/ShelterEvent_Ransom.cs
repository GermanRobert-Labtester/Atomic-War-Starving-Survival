using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RansomEventState
    {
        public string eventId = "shelter_event_ransom";
        public string demandingFaction = "terrorists";
        public int waterRansomDemand = 20;
        public float globalKarmaGainOnPay = 25f;
        public float moraleDropOnRadioExecution = 20f;
        public bool isRansomPaid = false;
        public bool isExecutedOnRadio = false;
    }

    /// <summary>
    /// Prompt #411: Event: Hostage Ransom Demand.
    /// Hostile faction demands water ransom for a captured civilian over the radio.
    /// Paying grants global Karma; refusing causes live radio execution, dropping bunker Morale.
    /// </summary>
    public class ShelterEvent_Ransom
    {
        private RansomEventState _state = new RansomEventState();

        public event Action<RansomEventState, float> OnRansomPaidKarmaGained;
        public event Action<RansomEventState, float> OnHostageExecutedRadioMoraleDropped;

        public RansomEventState State => _state;

        public bool PayRansom(ref int cleanWaterStorage, out float karmaGain)
        {
            karmaGain = 0f;
            if (cleanWaterStorage >= _state.waterRansomDemand && !_state.isRansomPaid && !_state.isExecutedOnRadio)
            {
                cleanWaterStorage -= _state.waterRansomDemand;
                _state.isRansomPaid = true;
                karmaGain = _state.globalKarmaGainOnPay;

                OnRansomPaidKarmaGained?.Invoke(_state, karmaGain);
                return true;
            }
            return false;
        }

        public void RefuseRansomAndExecute(ref float bunkerMorale)
        {
            if (!_state.isRansomPaid && !_state.isExecutedOnRadio)
            {
                _state.isExecutedOnRadio = true;
                bunkerMorale = Mathf.Max(0f, bunkerMorale - _state.moraleDropOnRadioExecution);

                OnHostageExecutedRadioMoraleDropped?.Invoke(_state, _state.moraleDropOnRadioExecution);
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public RansomEventState CaptureState() => _state;

        public void RestoreState(RansomEventState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
